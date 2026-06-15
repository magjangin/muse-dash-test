using System;
using System.IO;
using System.Net.WebSockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MelonLoader;

namespace muse_dash_test
{
    /// <summary>
    /// OBS Studio의 내장 WebSocket 서버(obs-websocket v5)에 접속하여
    /// 녹화 시작/정지 등의 명령을 전송하는 컨트롤러입니다.
    /// 외부 라이브러리 없이 .NET 기본 ClientWebSocket만 사용합니다.
    /// </summary>
    public static class ObsController
    {
        private static bool enabled = false;
        private static string host = "127.0.0.1";
        private static int port = 4455;
        private static string password = "";
        private static float stopDelaySeconds = 5f;
        private static bool configLoaded = false;

        private static readonly string configFolder = Path.Combine(MelonLoader.Utils.MelonEnvironment.GameRootDirectory, "save custom key");
        private static readonly string configPath = Path.Combine(configFolder, "obs config.txt");

        public static void LoadConfig()
        {
            try
            {
                if (!Directory.Exists(configFolder))
                {
                    Directory.CreateDirectory(configFolder);
                }

                if (!File.Exists(configPath))
                {
                    SaveDefaultConfig();
                }

                foreach (string raw in File.ReadAllLines(configPath))
                {
                    string line = raw.Trim();
                    if (string.IsNullOrEmpty(line) || line.StartsWith("#")) continue;

                    int idx = line.IndexOf('=');
                    if (idx <= 0) continue;

                    string key = line.Substring(0, idx).Trim();
                    string val = line.Substring(idx + 1).Trim();

                    switch (key.ToLower())
                    {
                        case "obs연동":
                            bool.TryParse(val, out enabled);
                            break;
                        case "주소":
                            if (!string.IsNullOrEmpty(val)) host = val;
                            break;
                        case "포트":
                            int.TryParse(val, out port);
                            break;
                        case "비밀번호":
                            password = val;
                            break;
                        case "녹화정지지연초":
                            float.TryParse(val, out stopDelaySeconds);
                            break;
                    }
                }

                configLoaded = true;
                MelonLogger.Msg($"[OBS] 설정 로드 완료: 연동={enabled}, 주소={host}:{port}, 인증={(string.IsNullOrEmpty(password) ? "없음" : "사용")}");
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[OBS] 설정 로드 실패: {ex.Message}");
            }
        }

        private static void SaveDefaultConfig()
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("# OBS 자동 녹화 연동 설정 파일");
                sb.AppendLine("# OBS에서 [도구 -> WebSocket 서버 설정 -> WebSocket 서버 활성화]를 켜야 동작합니다.");
                sb.AppendLine("# 인증을 켰다면 '서버 비밀번호 표시'로 나온 비밀번호를 아래 비밀번호= 뒤에 붙여넣으세요.");
                sb.AppendLine("# 인증을 껐다면 비밀번호= 는 비워두면 됩니다.");
                sb.AppendLine();
                sb.AppendLine("OBS연동=false");
                sb.AppendLine("주소=127.0.0.1");
                sb.AppendLine("포트=4455");
                sb.AppendLine("비밀번호=");
                sb.AppendLine();
                sb.AppendLine("# 곡이 끝난 뒤(결과 화면) 녹화를 멈추기까지 기다릴 시간(초). 결과 화면을 녹화에 담고 싶으면 늘리세요.");
                sb.AppendLine("녹화정지지연초=5");

                File.WriteAllText(configPath, sb.ToString(), Encoding.UTF8);
                MelonLogger.Msg($"[OBS] 기본 설정 파일을 생성했습니다: {configPath}");
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[OBS] 기본 설정 저장 실패: {ex.Message}");
            }
        }

        public static void StartRecording()
        {
            SendRequest("StartRecord", 0f);
        }

        public static void StopRecording()
        {
            SendRequest("StopRecord", stopDelaySeconds);
        }

        private static void SendRequest(string requestType, float delaySeconds)
        {
            if (!configLoaded)
            {
                LoadConfig();
            }

            if (!enabled)
            {
                return;
            }

            // 게임 메인 스레드를 막지 않도록 백그라운드에서 실행
            Task.Run(async () =>
            {
                try
                {
                    if (delaySeconds > 0f)
                    {
                        await Task.Delay(TimeSpan.FromSeconds(delaySeconds));
                    }

                    await DoRequestAsync(requestType);
                    MelonLogger.Msg($"[OBS] '{requestType}' 명령 전송 완료.");
                }
                catch (Exception ex)
                {
                    MelonLogger.Warning($"[OBS] '{requestType}' 명령 전송 실패 (OBS가 켜져 있고 WebSocket 서버가 활성화되어 있는지 확인하세요): {ex.Message}");
                }
            });
        }

        private static async Task DoRequestAsync(string requestType)
        {
            using (var ws = new ClientWebSocket())
            using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5)))
            {
                var uri = new Uri($"ws://{host}:{port}");
                await ws.ConnectAsync(uri, cts.Token);

                // 1. Hello (op 0) 수신
                string hello = await ReceiveAsync(ws, cts.Token);

                // 2. Identify (op 1) 전송 (인증 필요 시 챌린지-응답 계산)
                bool authRequired = hello.Contains("\"challenge\"");
                if (authRequired && string.IsNullOrEmpty(password))
                {
                    MelonLogger.Warning("[OBS] OBS는 인증을 요구하는데 설정파일의 비밀번호= 가 비어 있습니다. 'obs config.txt'에 OBS의 서버 비밀번호를 넣어주세요. (또는 OBS에서 인증 비활성화)");
                }

                string identifyPayload;
                if (authRequired && !string.IsNullOrEmpty(password))
                {
                    string challenge = ExtractJsonString(hello, "challenge");
                    string salt = ExtractJsonString(hello, "salt");
                    string auth = ComputeAuth(password, salt, challenge);
                    identifyPayload = "{\"op\":1,\"d\":{\"rpcVersion\":1,\"authentication\":\"" + auth + "\"}}";
                }
                else
                {
                    identifyPayload = "{\"op\":1,\"d\":{\"rpcVersion\":1}}";
                }
                await SendAsync(ws, identifyPayload, cts.Token);

                // 3. Identified (op 2) 수신
                await ReceiveAsync(ws, cts.Token);

                // 4. Request (op 6) 전송
                string requestId = Guid.NewGuid().ToString("N");
                string requestPayload = "{\"op\":6,\"d\":{\"requestType\":\"" + requestType + "\",\"requestId\":\"" + requestId + "\"}}";
                await SendAsync(ws, requestPayload, cts.Token);

                // 5. RequestResponse (op 7) 수신
                await ReceiveAsync(ws, cts.Token);

                try
                {
                    await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "done", cts.Token);
                }
                catch { /* 종료 중 예외는 무시 */ }
            }
        }

        private static async Task SendAsync(ClientWebSocket ws, string text, CancellationToken token)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(text);
            await ws.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, token);
        }

        private static async Task<string> ReceiveAsync(ClientWebSocket ws, CancellationToken token)
        {
            var buffer = new byte[8192];
            var sb = new StringBuilder();
            WebSocketReceiveResult result;
            do
            {
                result = await ws.ReceiveAsync(new ArraySegment<byte>(buffer), token);

                // OBS가 연결을 끊은 경우(인증 실패 등) 종료 사유를 명확히 알려줍니다.
                if (result.MessageType == WebSocketMessageType.Close)
                {
                    int code = result.CloseStatus.HasValue ? (int)result.CloseStatus.Value : 0;
                    string desc = result.CloseStatusDescription ?? "";
                    string hint = code == 4009
                        ? " → 인증 실패: 'obs config.txt'의 비밀번호가 OBS 서버 비밀번호와 일치하는지 확인하세요."
                        : "";
                    throw new Exception($"OBS가 연결을 종료했습니다 (코드={code}, 사유='{desc}').{hint}");
                }

                sb.Append(Encoding.UTF8.GetString(buffer, 0, result.Count));
            }
            while (!result.EndOfMessage);
            return sb.ToString();
        }

        /// <summary>
        /// OBS WebSocket v5 인증 문자열 계산:
        /// base64( sha256( base64( sha256(password + salt) ) + challenge ) )
        /// </summary>
        private static string ComputeAuth(string password, string salt, string challenge)
        {
            using (var sha = SHA256.Create())
            {
                byte[] secretHash = sha.ComputeHash(Encoding.UTF8.GetBytes(password + salt));
                string secret = Convert.ToBase64String(secretHash);

                byte[] authHash = sha.ComputeHash(Encoding.UTF8.GetBytes(secret + challenge));
                return Convert.ToBase64String(authHash);
            }
        }

        /// <summary>
        /// 작은 JSON 문자열에서 "key":"value" 형태의 문자열 값을 추출합니다.
        /// </summary>
        private static string ExtractJsonString(string json, string key)
        {
            string token = "\"" + key + "\"";
            int keyIdx = json.IndexOf(token, StringComparison.Ordinal);
            if (keyIdx < 0) return "";

            int colon = json.IndexOf(':', keyIdx + token.Length);
            if (colon < 0) return "";

            int firstQuote = json.IndexOf('"', colon + 1);
            if (firstQuote < 0) return "";

            int endQuote = json.IndexOf('"', firstQuote + 1);
            if (endQuote < 0) return "";

            return json.Substring(firstQuote + 1, endQuote - firstQuote - 1);
        }
    }
}
