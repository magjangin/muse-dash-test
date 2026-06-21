using System;
using System.IO;
using System.Text;
using MelonLoader;
using UnityEngine;

namespace muse_dash_test
{
    // 설정 파일(config.txt) 생성/감시/파싱 및 오버레이 색상 텍스처 캐시.
    public static partial class InputOverlay
    {
        // 설정 파일 경로 설정
        private static readonly string configFolder = Path.Combine(MelonLoader.Utils.MelonEnvironment.GameRootDirectory, "save custom key");
        private static readonly string configPath = Path.Combine(configFolder, "config.txt");
        private static DateTime lastWriteTime = DateTime.MinValue;
        private static float lastConfigCheckTime = 0f;
        private const float ConfigCheckInterval = 1.0f; // 1초마다 실시간 변경 감지 (디스크 I/O 최적화)
        private static bool hasFailedToWrite = false;   // 쓰기/생성 실패 시 반복적인 시도 및 로그 스패밍 방지

        // 실제 런타임에 사용할 설정 필드 (기본값 제공)
        private static float keyWidth = 55f;
        private static float keyHeight = 55f;
        private static float spacing = 8f;
        private static float offsetFromBottom = 80f;
        private static bool showOverlay = false;
        private static float fontSize = 17f;

        // 판정바 설정 필드
        public static bool showBar = true;
        public static float barWidth = 300f;
        public static float barHeight = 10f;
        public static float barOffsetFromBottom = 130f;
        public static float barFontSize = 16f;
        public static float tickDuration = 1.2f;
        public static bool barResponsive = false;

        // 추가 설정 필드 (오토플레이 & 피버 차단 & 시네마)
        public static bool blockFever = false;
        public static bool forceAutoPlay = true;
        public static bool enableCinema = true;

        private static string airColorName = "파랑";
        private static float airAlpha = 85f;

        private static string groundColorName = "빨강";
        private static float groundAlpha = 85f;

        private static string inactiveColorName = "검정";
        private static float inactiveAlpha = 55f;

        // GUI 단색 배경용 텍스처 캐시
        private static Texture2D airActiveTex;
        private static Texture2D groundActiveTex;
        private static Texture2D inactiveTex;

        private static string cachedAirColorHex = "";
        private static string cachedGroundColorHex = "";
        private static string cachedInactiveColorHex = "";

        /// <summary>
        /// 설정 파일이 변경되었거나 아직 로드되지 않았다면 실시간으로 다시 읽어옵니다. (1초 주기 스캔)
        /// </summary>
        public static void LoadConfigIfNeeded()
        {
            float currentTime = Time.unscaledTime;
            if (currentTime - lastConfigCheckTime < ConfigCheckInterval)
            {
                return;
            }
            lastConfigCheckTime = currentTime;

            try
            {
                if (!hasFailedToWrite)
                {
                    if (!Directory.Exists(configFolder))
                    {
                        Directory.CreateDirectory(configFolder);
                    }

                    if (!File.Exists(configPath))
                    {
                        SaveDefaultConfig();
                    }
                    else
                    {
                        EnsureMissingKeysAdded();
                    }
                }

                if (File.Exists(configPath))
                {
                    DateTime currentWriteTime = File.GetLastWriteTime(configPath);
                    if (currentWriteTime != lastWriteTime)
                    {
                        lastWriteTime = currentWriteTime;
                        ParseConfigFile();
                    }
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[InputOverlay] 설정 감지 중 예외 발생: {ex.Message}");
            }
        }

        private static void SaveDefaultConfig()
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("# 뮤즈 대시 키 뷰어 설정 파일 (실시간 반영)");
                sb.AppendLine("# 이 파일을 메모장 등으로 수정하고 저장하면 인게임 오버레이에 즉시 반영됩니다.");
                sb.AppendLine();
                sb.AppendLine($"키가로크기={keyWidth}");
                sb.AppendLine($"키세로크기={keyHeight}");
                sb.AppendLine($"키간격={spacing}");
                sb.AppendLine($"하단여백={offsetFromBottom}");
                sb.AppendLine($"오버레이표시={showOverlay.ToString().ToLower()}");
                sb.AppendLine($"글자크기={fontSize}");
                sb.AppendLine();
                sb.AppendLine("# 색상 설정 (지원 색상: 파랑, 빨강, 노랑, 초록, 보라, 핑크, 하양, 검정, 회색, 어두운회색)");
                sb.AppendLine("# 투명도 값 범위: 0 (완전 투명) ~ 100 (불투명)");
                sb.AppendLine($"공중색상={airColorName}");
                sb.AppendLine($"공중투명도={airAlpha}");
                sb.AppendLine($"지상색상={groundColorName}");
                sb.AppendLine($"지상투명도={groundAlpha}");
                sb.AppendLine($"대기색상={inactiveColorName}");
                sb.AppendLine($"대기투명도={inactiveAlpha}");
                sb.AppendLine();
                sb.AppendLine("# 판정바(Hit Error Bar) 설정");
                sb.AppendLine($"판정바표시={showBar.ToString().ToLower()}");
                sb.AppendLine($"판정바가로크기={barWidth}");
                sb.AppendLine($"판정바세로크기={barHeight}");
                sb.AppendLine($"판정바하단여백={barOffsetFromBottom}");
                sb.AppendLine($"판정바글자크기={barFontSize}");
                sb.AppendLine($"판정바틱유지시간={tickDuration}");
                sb.AppendLine($"판정바반응형={barResponsive.ToString().ToLower()}");
                sb.AppendLine();
                sb.AppendLine("# 오토플레이 및 피버 설정");
                sb.AppendLine($"오토플레이={forceAutoPlay.ToString().ToLower()}");
                sb.AppendLine($"피버충전금지={blockFever.ToString().ToLower()}");
                sb.AppendLine();
                sb.AppendLine("# 시네마(BGA 동영상) 재생 설정");
                sb.AppendLine($"시네마={enableCinema.ToString().ToLower()}");

                File.WriteAllText(configPath, sb.ToString(), new UTF8Encoding(true));
                MelonLogger.Msg($"[InputOverlay] 기본 설정 파일(config.txt)을 새로 생성했습니다: {configPath}");
            }
            catch (Exception ex)
            {
                hasFailedToWrite = true;
                MelonLogger.Error($"[InputOverlay] 기본 설정 저장 중 실패 (쓰기 시도가 중단됩니다): {ex.Message}");
            }
        }

        private static string ReadConfigTextRobust()
        {
            if (!File.Exists(configPath)) return "";

            try
            {
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            }
            catch {}

            byte[] bytes = File.ReadAllBytes(configPath);
            
            // 1. UTF-8 디코딩 시도
            string utf8Text = Encoding.UTF8.GetString(bytes);
            if (utf8Text.Contains("오토플레이") || utf8Text.Contains("키가로크기") || utf8Text.Contains("판정바"))
            {
                return utf8Text;
            }

            // 2. CP949 (EUC-KR) 디코딩 시도 (윈도우 메모장 ANSI 저장 폴백)
            try
            {
                var cp949 = Encoding.GetEncoding(949);
                string cp949Text = cp949.GetString(bytes);
                if (cp949Text.Contains("오토플레이") || cp949Text.Contains("키가로크기") || cp949Text.Contains("판정바"))
                {
                    MelonLogger.Msg("[InputOverlay] config.txt를 CP949(EUC-KR) 인코딩으로 인식하여 로드했습니다.");
                    return cp949Text;
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Warning($"[InputOverlay] CP949 디코딩 중 예외: {ex.Message}");
            }

            return utf8Text;
        }

        private static void EnsureMissingKeysAdded()
        {
            try
            {
                if (!File.Exists(configPath)) return;

                string text = ReadConfigTextRobust();
                bool hasAutoPlay = text.Contains("오토플레이");
                bool hasBlockFever = text.Contains("피버충전금지");
                bool hasCinema = text.Contains("시네마");

                if (!hasAutoPlay || !hasBlockFever || !hasCinema)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine();
                    sb.AppendLine("# [자동 업데이트] 누락된 설정 항목 추가");
                    
                    if (!hasAutoPlay)
                    {
                        sb.AppendLine($"오토플레이={forceAutoPlay.ToString().ToLower()}");
                    }
                    if (!hasBlockFever)
                    {
                        sb.AppendLine($"피버충전금지={blockFever.ToString().ToLower()}");
                    }
                    if (!hasCinema)
                    {
                        sb.AppendLine($"시네마={enableCinema.ToString().ToLower()}");
                    }

                    File.AppendAllText(configPath, sb.ToString(), new UTF8Encoding(true));
                    MelonLogger.Msg("[InputOverlay] 기존 config.txt 파일에서 누락된 설정 항목(오토플레이/피버/시네마)을 자동 추가했습니다.");
                }
            }
            catch (Exception ex)
            {
                hasFailedToWrite = true;
                MelonLogger.Error($"[InputOverlay] 누락된 설정 추가 중 예외 발생 (쓰기 시도가 중단됩니다): {ex.Message}");
            }
        }

        private static void ParseConfigFile()
        {
            try
            {
                if (!File.Exists(configPath)) return;

                string text = ReadConfigTextRobust();
                if (string.IsNullOrEmpty(text)) return;

                MelonLogger.Msg($"[InputOverlay][DEBUG] ParseConfigFile 시작. 총 문자 수={text.Length}, configPath={configPath}");

                string[] lines = text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                MelonLogger.Msg($"[InputOverlay][DEBUG] 분리된 줄 수={lines.Length}");

                foreach (string line in lines)
                {
                    string trimmed = line.Trim();
                    if (string.IsNullOrEmpty(trimmed) || trimmed.StartsWith("#"))
                        continue;

                    int idx = trimmed.IndexOf('=');
                    if (idx <= 0) continue;

                    string key = trimmed.Substring(0, idx).Trim();
                    string val = trimmed.Substring(idx + 1).Trim();



                    switch (key.ToLower())
                    {
                        case "키가로크기":
                            float.TryParse(val, out keyWidth);
                            break;
                        case "키세로크기":
                            float.TryParse(val, out keyHeight);
                            break;
                        case "키간격":
                            float.TryParse(val, out spacing);
                            break;
                        case "하단여백":
                            float.TryParse(val, out offsetFromBottom);
                            break;
                        case "오버레이표시":
                            showOverlay = ParseBool(val, key, showOverlay);
                            break;
                        case "글자크기":
                            float.TryParse(val, out fontSize);
                            break;
                        case "공중색상":
                            airColorName = val;
                            break;
                        case "공중투명도":
                            float.TryParse(val, out airAlpha);
                            break;
                        case "지상색상":
                            groundColorName = val;
                            break;
                        case "지상투명도":
                            float.TryParse(val, out groundAlpha);
                            break;
                        case "대기색상":
                            inactiveColorName = val;
                            break;
                        case "대기투명도":
                            float.TryParse(val, out inactiveAlpha);
                            break;
                        case "판정바표시":
                            showBar = ParseBool(val, key, showBar);
                            break;
                        case "판정바가로크기":
                            float.TryParse(val, out barWidth);
                            break;
                        case "판정바세로크기":
                            float.TryParse(val, out barHeight);
                            break;
                        case "판정바하단여백":
                            float.TryParse(val, out barOffsetFromBottom);
                            break;
                        case "판정바글자크기":
                            float.TryParse(val, out barFontSize);
                            break;
                        case "판정바틱유지시간":
                            float.TryParse(val, out tickDuration);
                            break;
                        case "판정바반응형":
                            barResponsive = ParseBool(val, key, barResponsive);
                            break;
                        case "오토플레이":
                            forceAutoPlay = ParseBool(val, key, forceAutoPlay);
                            break;
                        case "피버충전금지":
                            blockFever = ParseBool(val, key, blockFever);
                            break;
                        case "시네마":
                            enableCinema = ParseBool(val, key, enableCinema);
                            break;
                    }
                }
                MelonLogger.Msg($"[InputOverlay] 설정을 성공적으로 적용했습니다. (키크기={keyWidth}x{keyHeight}, 하단여백={offsetFromBottom}, 판정바={showBar}, 판정바여백={barOffsetFromBottom}, 오토플레이={forceAutoPlay}, 피버충전금지={blockFever}, 시네마={enableCinema})");
                UpdateTextures();
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[InputOverlay] 설정 파일 파싱 중 에러 발생: {ex.Message}");
            }
        }
        /// <summary>
        /// true/false, on/off, 켜짐/끔, 1/0 형식을 모두 인식하는 bool 파싱 헬퍼.
        /// 파싱 실패 시 경고 로그를 남기고 기존 값(fallback)을 유지합니다.
        /// </summary>
        private static bool ParseBool(string val, string key, bool fallback)
        {
            switch (val.ToLower().Trim())
            {
                case "true":
                case "on":
                case "켜짐":
                case "1":
                    MelonLogger.Msg($"[InputOverlay] '{key}' → true (입력값: '{val}')");
                    return true;
                case "false":
                case "off":
                case "끔":
                case "0":
                    MelonLogger.Msg($"[InputOverlay] '{key}' → false (입력값: '{val}')");
                    return false;
                default:
                    MelonLogger.Warning($"[InputOverlay] '{key}' 파싱 실패: '{val}'은 인식할 수 없는 값입니다. (true/false/on/off/켜짐/끔/1/0 중 하나를 사용하세요) 기존 값({fallback}) 유지.");
                    return fallback;
            }
        }

        private static Color ParseColorName(string name, float alphaPercent)
        {
            Color baseColor = Color.gray;
            switch (name.Trim())
            {
                case "파랑":
                case "파란색":
                    baseColor = new Color(0.2f, 0.55f, 0.9f);
                    break;
                case "빨강":
                case "빨간색":
                    baseColor = new Color(0.95f, 0.25f, 0.25f);
                    break;
                case "노랑":
                case "노란색":
                    baseColor = new Color(0.95f, 0.85f, 0.1f);
                    break;
                case "초록":
                case "초록색":
                    baseColor = new Color(0.25f, 0.8f, 0.3f);
                    break;
                case "보라":
                case "보라색":
                    baseColor = new Color(0.6f, 0.3f, 0.9f);
                    break;
                case "핑크":
                case "분홍":
                case "분홍색":
                    baseColor = new Color(0.95f, 0.4f, 0.7f);
                    break;
                case "하양":
                case "하얀색":
                case "흰색":
                    baseColor = Color.white;
                    break;
                case "검정":
                case "검은색":
                case "검정색":
                    baseColor = Color.black;
                    break;
                case "회색":
                    baseColor = new Color(0.5f, 0.5f, 0.5f);
                    break;
                case "어두운회색":
                    baseColor = new Color(0.12f, 0.12f, 0.12f);
                    break;
                default:
                    if (ColorUtility.TryParseHtmlString(name, out Color parsed))
                    {
                        baseColor = parsed;
                    }
                    break;
            }
            baseColor.a = Mathf.Clamp01(alphaPercent / 100f);
            return baseColor;
        }

        private static void UpdateTextures()
        {
            // 한글 색상명 파싱 및 갱신
            string currentAirKey = $"{airColorName}_{airAlpha}";
            if (cachedAirColorHex != currentAirKey || airActiveTex == null)
            {
                cachedAirColorHex = currentAirKey;
                Color c = ParseColorName(airColorName, airAlpha);
                if (airActiveTex != null) UnityEngine.Object.Destroy(airActiveTex);
                airActiveTex = CreateColorTexture(c);
            }

            string currentGroundKey = $"{groundColorName}_{groundAlpha}";
            if (cachedGroundColorHex != currentGroundKey || groundActiveTex == null)
            {
                cachedGroundColorHex = currentGroundKey;
                Color c = ParseColorName(groundColorName, groundAlpha);
                if (groundActiveTex != null) UnityEngine.Object.Destroy(groundActiveTex);
                groundActiveTex = CreateColorTexture(c);
            }

            string currentInactiveKey = $"{inactiveColorName}_{inactiveAlpha}";
            if (cachedInactiveColorHex != currentInactiveKey || inactiveTex == null)
            {
                cachedInactiveColorHex = currentInactiveKey;
                Color c = ParseColorName(inactiveColorName, inactiveAlpha);
                if (inactiveTex != null) UnityEngine.Object.Destroy(inactiveTex);
                inactiveTex = CreateColorTexture(c);
            }

            // 스타일 캐시도 동시에 갱신
            UpdateStyles();
        }

        private static Texture2D CreateColorTexture(Color color)
        {
            Texture2D texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, color);
            texture.Apply();
            return texture;
        }
    }
}
