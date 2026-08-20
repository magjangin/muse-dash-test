using System;
using System.Collections.Generic;
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
        private static bool canonicalizeDone = false;   // 표준 형식 정규화는 세션당 1회만 (무한 재작성 방지)

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
        // 오토플레이는 모드가 로드될 때 항상 꺼진 상태로 시작합니다. config.txt에 '오토플레이=true'가
        // 남아 있어도 이번 실행의 첫 로드에서는 무시합니다(아래 autoPlayFollowsConfig 참고).
        public static bool forceAutoPlay = false;
        public static bool enableCinema = true;

        // 판정 강제(올 퍼펙트) 설정. 오토플레이와 무관하게, 입력은 사람이 그대로 하고
        // 게임에 "기록되는 판정"만 Perfect로 승격시킵니다. (<see cref="ForcePerfectState"/>)
        public static bool forcePerfect = false;

        // 공식곡의 고스트 노트(UID xx=17)가 판정선 근처에서 사라지지 않도록 알파를 되돌립니다.
        // 커스텀 곡은 이 값 대신 각자의 `hwa info.txt`에 적은 '커스텀 곡 고스트 노트 보이기'를 따릅니다.
        public static bool showGhostNotes = true;

        // 모바일 터치 및 마우스-터치 브릿지 설정 필드 (세부 좌우/반전 설정은 인게임 PnlInputMobile UI에서 직접 제어)
        public static bool enableMobileTouch = false;

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
            // 인게임 플레이 배틀 중에는 디스크 I/O 스캔을 건너뛰어 프레임 드랍을 원천 차단합니다.
            if (HywStageManager.IsInStageStatic) return;

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
                        WriteConfigFile("기본 설정 파일(config.txt)을 새로 생성했습니다");
                        canonicalizeDone = true;
                    }
                    else if (!canonicalizeDone)
                    {
                        // 파일을 '쓰는' 작업은 LastWriteTime을 바꿔 아래 감시 로직의 재파싱을 부릅니다.
                        // 그래서 쓰기 판정이 한 번이라도 어긋나면 "쓰기 → 재파싱 → 쓰기"가 매 폴링마다
                        // 무한 반복됩니다(실제로 그 사고가 있었습니다). 세션당 1회로 못박아 차단합니다.
                        canonicalizeDone = true;
                        EnsureCanonicalConfigFile();
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
                ModLogger.Error($"[InputOverlay] 설정 감지 중 예외 발생: {ex.Message}");
            }
        }

        /// <summary>
        /// 고스트 노트 항목의 옛 키 이름. 파일에는 더 이상 이 이름으로 쓰지 않지만
        /// (표준 이름은 <see cref="GhostNotesKey"/>), 옛 파일을 읽을 때는 계속 받아 줍니다.
        /// 표준 형식으로 다시 쓰는 순간 새 이름으로 옮겨집니다.
        /// </summary>
        private const string GhostNotesLegacyKey = "고스트노트보이기";

        private static string ReadConfigTextRobust()
        {
            if (!File.Exists(configPath)) return "";

            try
            {
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            }
            catch (Exception) { }

            byte[] bytes = File.ReadAllBytes(configPath);

            // UTF-8 BOM은 Encoding.UTF8.GetString이 걷어내지 않아 첫 글자로 남습니다(U+FEFF).
            // 이 텍스트를 다시 UTF8Encoding(true)로 쓰는 경로(이름 이관/중복 정리)가 있으므로,
            // 그대로 두면 BOM이 파일 안에 두 번 들어갑니다. 읽는 쪽에서 한 번만 걷어냅니다.
            int offset = (bytes.Length >= 3 && bytes[0] == 0xEF && bytes[1] == 0xBB && bytes[2] == 0xBF) ? 3 : 0;

            // 1. UTF-8 디코딩 시도
            string utf8Text = Encoding.UTF8.GetString(bytes, offset, bytes.Length - offset);
            if (utf8Text.Contains("오토플레이") || utf8Text.Contains("키가로크기") || utf8Text.Contains("판정바"))
            {
                return utf8Text;
            }

            // 2. CP949 (EUC-KR) 디코딩 시도 (윈도우 메모장 ANSI 저장 폴백)
            try
            {
                var cp949 = Encoding.GetEncoding(949);
                string cp949Text = cp949.GetString(bytes, offset, bytes.Length - offset);
                if (cp949Text.Contains("오토플레이") || cp949Text.Contains("키가로크기") || cp949Text.Contains("판정바"))
                {
                    ModLogger.Msg("[InputOverlay] config.txt를 CP949(EUC-KR) 인코딩으로 인식하여 로드했습니다.");
                    return cp949Text;
                }
            }
            catch (Exception ex)
            {
                ModLogger.Warning($"[InputOverlay] CP949 디코딩 중 예외: {ex.Message}");
            }

            return utf8Text;
        }

        /// <summary>키 비교용. 설정 파일에서는 띄어쓰기를 자유롭게 쓰게 두고, 코드에서는 붙여 쓴 이름 하나로 봅니다.</summary>
        private static string StripSpaces(string text)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;

            var sb = new StringBuilder(text.Length);
            foreach (char c in text)
            {
                if (!char.IsWhiteSpace(c)) sb.Append(c);
            }
            return sb.ToString();
        }

        private static void TryParseFloat(string val, ref float target)
        {
            if (float.TryParse(val, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out float parsed))
            {
                target = parsed;
            }
        }

        private static void ParseConfigFile()
        {
            try
            {
                if (!File.Exists(configPath)) return;

                string text = ReadConfigTextRobust();
                if (string.IsNullOrEmpty(text)) return;

                ModConfig.VerboseLog($"[InputOverlay] ParseConfigFile 시작. 총 문자 수={text.Length}, configPath={configPath}");

                string[] lines = text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                ModConfig.VerboseLog($"[InputOverlay] 분리된 줄 수={lines.Length}");

                foreach (string line in lines)
                {
                    string trimmed = line.Trim();
                    if (string.IsNullOrEmpty(trimmed) || trimmed.StartsWith("#"))
                        continue;

                    int idx = trimmed.IndexOf('=');
                    if (idx <= 0) continue;

                    string key = trimmed.Substring(0, idx).Trim();
                    string val = trimmed.Substring(idx + 1).Trim();

                    // 키의 띄어쓰기는 무시합니다. '공식곡에서도 고스트 노트 보이기'처럼 읽기 좋게 적어도 되게.
                    switch (StripSpaces(key).ToLower())
                    {
                        case "키가로크기":
                            TryParseFloat(val, ref keyWidth);
                            break;
                        case "키세로크기":
                            TryParseFloat(val, ref keyHeight);
                            break;
                        case "키간격":
                            TryParseFloat(val, ref spacing);
                            break;
                        case "하단여백":
                            TryParseFloat(val, ref offsetFromBottom);
                            break;
                        case "오버레이표시":
                            showOverlay = ParseBool(val, key, showOverlay);
                            break;
                        case "글자크기":
                            TryParseFloat(val, ref fontSize);
                            break;
                        case "공중색상":
                            airColorName = val;
                            break;
                        case "공중투명도":
                            TryParseFloat(val, ref airAlpha);
                            break;
                        case "지상색상":
                            groundColorName = val;
                            break;
                        case "지상투명도":
                            TryParseFloat(val, ref groundAlpha);
                            break;
                        case "대기색상":
                            inactiveColorName = val;
                            break;
                        case "대기투명도":
                            TryParseFloat(val, ref inactiveAlpha);
                            break;
                        case "판정바표시":
                            showBar = ParseBool(val, key, showBar);
                            break;
                        case "판정바가로크기":
                            TryParseFloat(val, ref barWidth);
                            break;
                        case "판정바세로크기":
                            TryParseFloat(val, ref barHeight);
                            break;
                        case "판정바하단여백":
                            TryParseFloat(val, ref barOffsetFromBottom);
                            break;
                        case "판정바글자크기":
                            TryParseFloat(val, ref barFontSize);
                            break;
                        case "판정바틱유지시간":
                            TryParseFloat(val, ref tickDuration);
                            break;
                        case "판정바반응형":
                            barResponsive = ParseBool(val, key, barResponsive);
                            break;
                        case "오토플레이":
                        {
                            forceAutoPlay = ParseBool(val, key, forceAutoPlay);
                            break;
                        }
                        case "피버충전금지":
                            blockFever = ParseBool(val, key, blockFever);
                            break;
                        case "시네마":
                            enableCinema = ParseBool(val, key, enableCinema);
                            break;
                        case GhostNotesKey:
                        case GhostNotesLegacyKey:
                            showGhostNotes = ParseBool(val, key, showGhostNotes);
                            break;
                        case "강제퍼펙트":
                        {
                            bool previous = forcePerfect;
                            forcePerfect = ParseBool(val, key, forcePerfect);
                            if (previous != forcePerfect)
                            {
                                ForcePerfectState.OnSettingChanged(forcePerfect);
                            }
                            break;
                        }
                        case "모바일터치조작":
                        case "모바일터치":
                            enableMobileTouch = ParseBool(val, key, enableMobileTouch);
                            break;
                    }
                }

                ModLogger.Msg($"[InputOverlay] 설정을 성공적으로 적용했습니다. (키크기={keyWidth}x{keyHeight}, 하단여백={offsetFromBottom}, 판정바={showBar}, 오토플레이={forceAutoPlay}, 피버충전금지={blockFever}, 시네마={enableCinema}, 강제퍼펙트={forcePerfect}, 고스트노트={showGhostNotes}, 모바일터치={enableMobileTouch})");
                UpdateTextures();
            }
            catch (Exception ex)
            {
                ModLogger.Error($"[InputOverlay] 설정 파일 파싱 중 에러 발생: {ex.Message}");
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
                    ModConfig.VerboseLog($"[InputOverlay] '{key}' → true (입력값: '{val}')");
                    return true;
                case "false":
                case "off":
                case "끔":
                case "0":
                    ModConfig.VerboseLog($"[InputOverlay] '{key}' → false (입력값: '{val}')");
                    return false;
                default:
                    ModLogger.Warning($"[InputOverlay] '{key}' 파싱 실패: '{val}'은 인식할 수 없는 값입니다. (true/false/on/off/켜짐/끔/1/0 중 하나를 사용하세요) 기존 값({fallback}) 유지.");
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

            // 스타일 캐시 갱신은 OnGUI 컨텍스트에서만 가능(GUI.skin 접근)하므로 즉시 호출하지 않고
            // 더티 플래그만 세워 다음 OnGUI 프레임(DrawInputOverlay)에서 반영합니다.
            // (OnUpdate/초기화 등 OnGUI 바깥에서 ParseConfigFile이 호출될 때의 "You can only call GUI functions from inside OnGUI" 예외 방지)
            stylesDirty = true;
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
