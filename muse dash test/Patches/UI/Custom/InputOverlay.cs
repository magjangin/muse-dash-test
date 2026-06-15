using System;
using System.IO;
using System.Text;
using System.Reflection;
using MelonLoader;
using UnityEngine;

namespace muse_dash_test
{
    /// <summary>
    /// 플레이어가 설정한 공격 키(지상/공중)를 실시간으로 모니터링하여
    /// 화면 하단 중앙에 시각적인 텍스트 박스로 입력 상태(Key Viewer / Input Overlay)를 표시해 주는 모드 클래스입니다.
    /// </summary>
    public static class InputOverlay
    {
        private static readonly System.Collections.Generic.List<UnityEngine.KeyCode> airKeys = new System.Collections.Generic.List<UnityEngine.KeyCode>();
        private static readonly System.Collections.Generic.List<UnityEngine.KeyCode> groundKeys = new System.Collections.Generic.List<UnityEngine.KeyCode>();
        private static bool keysLoaded = false;
        private static float checkTimer = 0f;
        private const float CheckInterval = 2.0f; // 키 세팅 재스캔 간격 (설정 변경 연동)

        // 설정 파일 경로 설정
        private static readonly string configFolder = Path.Combine(MelonLoader.Utils.MelonEnvironment.GameRootDirectory, "save custom key");
        private static readonly string configPath = Path.Combine(configFolder, "config.txt");
        private static DateTime lastWriteTime = DateTime.MinValue;

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

        // 추가 설정 필드 (오토플레이 & 피버 차단)
        public static bool blockFever = false;
        public static bool forceAutoPlay = false;

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

        public static void ResetCache()
        {
            keysLoaded = false;
            MelonLogger.Msg("[InputOverlay] 키 바인딩 캐시가 초기화되었습니다. 다음 프레임에 재로드합니다.");
        }

        /// <summary>
        /// 설정 파일이 변경되었거나 아직 로드되지 않았다면 실시간으로 다시 읽어옵니다.
        /// </summary>
        public static void LoadConfigIfNeeded()
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

                DateTime currentWriteTime = File.GetLastWriteTime(configPath);
                if (currentWriteTime != lastWriteTime)
                {
                    lastWriteTime = currentWriteTime;
                    ParseConfigFile();
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

                File.WriteAllText(configPath, sb.ToString(), Encoding.UTF8);
                MelonLogger.Msg($"[InputOverlay] 기본 설정 파일(config.txt)을 새로 생성했습니다: {configPath}");
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[InputOverlay] 기본 설정 저장 중 실패: {ex.Message}");
            }
        }

        private static void ParseConfigFile()
        {
            try
            {
                if (!File.Exists(configPath)) return;

                string[] lines = File.ReadAllLines(configPath);
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
                            bool.TryParse(val, out showOverlay);
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
                            bool.TryParse(val, out showBar);
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
                            bool.TryParse(val, out barResponsive);
                            break;
                        case "오토플레이":
                            bool.TryParse(val, out forceAutoPlay);
                            break;
                        case "피버충전금지":
                            bool.TryParse(val, out blockFever);
                            break;
                    }
                }
                MelonLogger.Msg($"[InputOverlay] 설정을 성공적으로 적용했습니다. (키크기={keyWidth}x{keyHeight}, 하단여백={offsetFromBottom}, 판정바={showBar}, 판정바여백={barOffsetFromBottom})");
                UpdateTextures();
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[InputOverlay] 설정 파일 파싱 중 에러 발생: {ex.Message}");
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
        }

        /// <summary>
        /// 디버그용: 실제 인게임 플레이 시 공격 키 입력이 정상적으로 모니터링되는지 실시간 체크합니다.
        /// </summary>
        public static void UpdateKeyTest()
        {
            LoadConfigIfNeeded();

            if (!keysLoaded)
            {
                checkTimer += Time.deltaTime;
                if (checkTimer >= CheckInterval || airKeys.Count == 0)
                {
                    checkTimer = 0f;
                    LoadPlayerKeybinds();
                }
                return;
            }

            for (int i = 0; i < airKeys.Count; i++)
            {
                var key = airKeys[i];
                if (Input.GetKeyDown(key))
                {
                    MelonLogger.Msg($"[InputOverlay.Test] 공중 공격 키 입력 감지 (KeyDown): {key}");
                }
                if (Input.GetKeyUp(key))
                {
                    MelonLogger.Msg($"[InputOverlay.Test] 공중 공격 키 입력 해제 (KeyUp): {key}");
                }
            }

            for (int i = 0; i < groundKeys.Count; i++)
            {
                var key = groundKeys[i];
                if (Input.GetKeyDown(key))
                {
                    MelonLogger.Msg($"[InputOverlay.Test] 지상 공격 키 입력 감지 (KeyDown): {key}");
                }
                if (Input.GetKeyUp(key))
                {
                    MelonLogger.Msg($"[InputOverlay.Test] 지상 공격 키 입력 해제 (KeyUp): {key}");
                }
            }
        }

        /// <summary>
        /// StandloneCtrlConfig 로부터 유저가 커스텀 설정한 키 매핑 목록을 리플렉션으로 안전하게 읽어옵니다.
        /// </summary>
        public static void LoadPlayerKeybinds()
        {
            try
            {
                var configs = UnityEngine.Resources.FindObjectsOfTypeAll<Il2CppAssets.Scripts.GameCore.Controller.Configs.StandloneCtrlConfig>();
                if (configs == null || configs.Count == 0)
                {
                    return;
                }

                var config = configs[0];
                string proposal = ModReflection.GetValue(config, "CurrentProposal")?.ToString() ?? "Custom";

                var buttonKeyEnties = ModReflection.GetValue(config, "m_ButtonKeyEnties");
                if (buttonKeyEnties == null)
                {
                    MelonLogger.Warning("[InputOverlay] StandloneCtrlConfig의 m_ButtonKeyEnties 필드가 null입니다.");
                    return;
                }

                var get_ItemMethod = buttonKeyEnties.GetType().GetMethod("get_Item");
                if (get_ItemMethod == null) return;

                // ContainsKey 체크를 통해 딕셔너리에 해당 proposal 키가 있는지 우선 점검
                var containsKeyMethod = buttonKeyEnties.GetType().GetMethod("ContainsKey");
                bool hasProposalKey = false;
                if (containsKeyMethod != null)
                {
                    hasProposalKey = (bool)containsKeyMethod.Invoke(buttonKeyEnties, new object[] { proposal });
                }

                // 딕셔너리에 proposal 키가 없는 경우의 안전 조치
                if (!hasProposalKey)
                {
                    MelonLogger.Warning($"[InputOverlay] '{proposal}' proposal이 buttonKeyEnties 딕셔너리에 없습니다. 폴백을 시도합니다...");
                    
                    var keysProp = buttonKeyEnties.GetType().GetProperty("Keys");
                    if (keysProp != null)
                    {
                        var keysObj = keysProp.GetValue(buttonKeyEnties);
                        if (keysObj != null)
                        {
                            var getEnumeratorMethod = keysObj.GetType().GetMethod("GetEnumerator");
                            if (getEnumeratorMethod != null)
                            {
                                var enumerator = getEnumeratorMethod.Invoke(keysObj, null);
                                var moveNextMethod = enumerator.GetType().GetMethod("MoveNext");
                                var currentProp = enumerator.GetType().GetProperty("Current");
                                
                                if (moveNextMethod != null && currentProp != null)
                                {
                                    while ((bool)moveNextMethod.Invoke(enumerator, null))
                                    {
                                        var k = currentProp.GetValue(enumerator)?.ToString();
                                        MelonLogger.Msg($"  [InputOverlay.Debug] 발견된 제안 Key: '{k}'");
                                        if (string.IsNullOrEmpty(proposal) || !hasProposalKey)
                                        {
                                            proposal = k; // 일단 첫 번째로 발견된 키를 사용
                                            hasProposalKey = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                if (!hasProposalKey || string.IsNullOrEmpty(proposal))
                {
                    MelonLogger.Error("[InputOverlay] 딕셔너리 내에 사용 가능한 proposal 키가 아예 존재하지 않습니다.");
                    return;
                }

                var proposalDict = get_ItemMethod.Invoke(buttonKeyEnties, new object[] { proposal });
                if (proposalDict == null) return;

                var get_ItemMethod2 = proposalDict.GetType().GetMethod("get_Item");
                var containsKeyMethod2 = proposalDict.GetType().GetMethod("ContainsKey");
                if (get_ItemMethod2 == null) return;

                // 1. 공중/지상 키 코드 리스트 추출 키 조사 및 폴백
                bool hasAirKey = false;
                bool hasGroundKey = false;
                if (containsKeyMethod2 != null)
                {
                    hasAirKey = (bool)containsKeyMethod2.Invoke(proposalDict, new object[] { "KeyBattleAir" });
                    hasGroundKey = (bool)containsKeyMethod2.Invoke(proposalDict, new object[] { "KeyBattleGround" });
                }

                string realAirKeyName = hasAirKey ? "KeyBattleAir" : null;
                string realGroundKeyName = hasGroundKey ? "KeyBattleGround" : null;

                // 키를 직접 찾지 못한 경우에만 딕셔너리를 뒤져서 매칭
                if (!hasAirKey || !hasGroundKey)
                {
                    var keysProp2 = proposalDict.GetType().GetProperty("Keys");
                    if (keysProp2 != null)
                    {
                        var keysObj2 = keysProp2.GetValue(proposalDict);
                        if (keysObj2 != null)
                        {
                            var getEnumeratorMethod2 = keysObj2.GetType().GetMethod("GetEnumerator");
                            if (getEnumeratorMethod2 != null)
                            {
                                var enumerator2 = getEnumeratorMethod2.Invoke(keysObj2, null);
                                var moveNextMethod2 = enumerator2.GetType().GetMethod("MoveNext");
                                var currentProp2 = enumerator2.GetType().GetProperty("Current");
                                if (moveNextMethod2 != null && currentProp2 != null)
                                {
                                    while ((bool)moveNextMethod2.Invoke(enumerator2, null))
                                    {
                                        string k = currentProp2.GetValue(enumerator2)?.ToString();
                                        if (k != null)
                                        {
                                            // 대소문자 무관 및 키워드 매칭 폴백
                                            if (k.IndexOf("Air", StringComparison.OrdinalIgnoreCase) >= 0 && !hasAirKey)
                                            {
                                                realAirKeyName = k;
                                                hasAirKey = true;
                                            }
                                            else if (k.IndexOf("Ground", StringComparison.OrdinalIgnoreCase) >= 0 && !hasGroundKey)
                                            {
                                                realGroundKeyName = k;
                                                hasGroundKey = true;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                // 2. 공중 키 바인딩 목록 추출
                airKeys.Clear();
                if (hasAirKey && !string.IsNullOrEmpty(realAirKeyName))
                {
                    var airKeysObj = get_ItemMethod2.Invoke(proposalDict, new object[] { realAirKeyName });
                    if (airKeysObj != null)
                    {
                        var countProp = airKeysObj.GetType().GetProperty("Count");
                        var get_ItemMethod3 = airKeysObj.GetType().GetMethod("get_Item");
                        if (countProp != null && get_ItemMethod3 != null)
                        {
                            int count = (int)countProp.GetValue(airKeysObj);
                            for (int i = 0; i < count; i++)
                            {
                                var key = (UnityEngine.KeyCode)get_ItemMethod3.Invoke(airKeysObj, new object[] { i });
                                airKeys.Add(key);
                            }
                        }
                    }
                }

                // 3. 지상 키 바인딩 목록 추출
                groundKeys.Clear();
                if (hasGroundKey && !string.IsNullOrEmpty(realGroundKeyName))
                {
                    var groundKeysObj = get_ItemMethod2.Invoke(proposalDict, new object[] { realGroundKeyName });
                    if (groundKeysObj != null)
                    {
                        var countProp = groundKeysObj.GetType().GetProperty("Count");
                        var get_ItemMethod3 = groundKeysObj.GetType().GetMethod("get_Item");
                        if (countProp != null && get_ItemMethod3 != null)
                        {
                            int count = (int)countProp.GetValue(groundKeysObj);
                            for (int i = 0; i < count; i++)
                            {
                                var key = (UnityEngine.KeyCode)get_ItemMethod3.Invoke(groundKeysObj, new object[] { i });
                                groundKeys.Add(key);
                            }
                        }
                    }
                }

                keysLoaded = true;

                // 유효한 키들만 추려서 간소화된 메시지로 로그 출력
                var cleanAir = new System.Collections.Generic.List<string>();
                foreach (var k in airKeys) { if (k != KeyCode.None) cleanAir.Add(k.ToString()); }

                var cleanGround = new System.Collections.Generic.List<string>();
                foreach (var k in groundKeys) { if (k != KeyCode.None) cleanGround.Add(k.ToString()); }

                MelonLogger.Msg($"[InputOverlay] 키 바인딩 데이터 로드 완료! 공중 키: [{string.Join(", ", cleanAir)}], 지상 키: [{string.Join(", ", cleanGround)}]");
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[InputOverlay] 키매핑 추출 중 예외 발생: {ex}");
                if (ex.InnerException != null)
                {
                    MelonLogger.Error($"[InputOverlay] 상세 원인(InnerException): {ex.InnerException}");
                }
            }
        }

        /// <summary>
        /// 키뷰어가 활성화되어 있을 때, 키뷰어 오버레이의 최상단 Y좌표를 계산하여 반환합니다.
        /// 꺼져 있거나 표시되지 않는 경우 -1f를 반환합니다.
        /// </summary>
        public static float GetOverlayTopY()
        {
            if (!showOverlay || !keysLoaded) return -1f;

            int airCount = 0;
            for (int i = 0; i < airKeys.Count; i++)
            {
                if (airKeys[i] != KeyCode.None) airCount++;
            }

            int groundCount = 0;
            for (int i = 0; i < groundKeys.Count; i++)
            {
                if (groundKeys[i] != KeyCode.None) groundCount++;
            }

            int maxKeys = Math.Max(airCount, groundCount);
            if (maxKeys == 0) return -1f;

            float screenHeight = Screen.height;
            float startY = screenHeight - (keyHeight * 2f) - spacing - offsetFromBottom;
            return startY;
        }

        /// <summary>
        /// 인게임 화면상에 실시간 키 입력 오버레이 박스를 렌더링합니다.
        /// </summary>
        public static void DrawInputOverlay()
        {
            // 실시간으로 설정 변경 감지 및 반영
            LoadConfigIfNeeded();

            if (!showOverlay) return;

            // 키 정보가 로드되지 않았다면 로드를 시도합니다.
            if (!keysLoaded)
            {
                checkTimer += Time.deltaTime;
                if (checkTimer >= CheckInterval || airKeys.Count == 0)
                {
                    checkTimer = 0f;
                    LoadPlayerKeybinds();
                }
                return;
            }

            // None이 아닌 실제 유효한 키들만 수집하여 오버레이를 그립니다.
            var activeAirKeys = new System.Collections.Generic.List<KeyCode>();
            for (int i = 0; i < airKeys.Count; i++)
            {
                if (airKeys[i] != KeyCode.None) activeAirKeys.Add(airKeys[i]);
            }

            var activeGroundKeys = new System.Collections.Generic.List<KeyCode>();
            for (int i = 0; i < groundKeys.Count; i++)
            {
                if (groundKeys[i] != KeyCode.None) activeGroundKeys.Add(groundKeys[i]);
            }

            int maxKeys = Math.Max(activeAirKeys.Count, activeGroundKeys.Count);
            if (maxKeys == 0) return;

            // 화면 가로/세로 구하기
            float screenWidth = Screen.width;
            float screenHeight = Screen.height;

            float totalWidth = (maxKeys * keyWidth) + ((maxKeys - 1) * spacing);
            float startX = (screenWidth - totalWidth) / 2f;
            
            // 피버 바 위쪽이자 판정 텍스트 방해하지 않는 하단 중앙 구역
            float startY = screenHeight - (keyHeight * 2f) - spacing - offsetFromBottom;

            // 텍스처 초기화 (기본 설정 로드 시 파싱되므로 폴백으로만 처리)
            if (airActiveTex == null || groundActiveTex == null || inactiveTex == null)
            {
                UpdateTextures();
            }

            // GUI 스타일 설정
            GUIStyle airActiveStyle = new GUIStyle(GUI.skin.box);
            airActiveStyle.normal.background = airActiveTex;
            airActiveStyle.normal.textColor = Color.white;
            airActiveStyle.alignment = TextAnchor.MiddleCenter;
            airActiveStyle.fontSize = (int)fontSize;
            airActiveStyle.fontStyle = FontStyle.Bold;

            GUIStyle groundActiveStyle = new GUIStyle(GUI.skin.box);
            groundActiveStyle.normal.background = groundActiveTex;
            groundActiveStyle.normal.textColor = Color.white;
            groundActiveStyle.alignment = TextAnchor.MiddleCenter;
            groundActiveStyle.fontSize = (int)fontSize;
            groundActiveStyle.fontStyle = FontStyle.Bold;

            GUIStyle inactiveStyle = new GUIStyle(GUI.skin.box);
            inactiveStyle.normal.background = inactiveTex;
            inactiveStyle.normal.textColor = new Color(0.75f, 0.75f, 0.75f, 1.0f);
            inactiveStyle.alignment = TextAnchor.MiddleCenter;
            inactiveStyle.fontSize = Math.Max(12, (int)fontSize - 2);

            // 1. 공중(위쪽 행) 그리기 - 파란색
            for (int i = 0; i < activeAirKeys.Count; i++)
            {
                var key = activeAirKeys[i];
                bool isPressed = Input.GetKey(key);
                float x = startX + (i * (keyWidth + spacing));
                float y = startY;

                string keyName = CleanKeyName(key);
                GUI.Box(new Rect(x, y, keyWidth, keyHeight), keyName, isPressed ? airActiveStyle : inactiveStyle);
            }

            // 2. 지상(아래쪽 행) 그리기 - 빨간색
            for (int i = 0; i < activeGroundKeys.Count; i++)
            {
                var key = activeGroundKeys[i];
                bool isPressed = Input.GetKey(key);
                float x = startX + (i * (keyWidth + spacing));
                float y = startY + keyHeight + spacing;

                string keyName = CleanKeyName(key);
                GUI.Box(new Rect(x, y, keyWidth, keyHeight), keyName, isPressed ? groundActiveStyle : inactiveStyle);
            }
        }

        private static string CleanKeyName(KeyCode key)
        {
            string name = key.ToString();
            // 알파벳 숫자는 그대로 표시
            if (name.StartsWith("Alpha")) return name.Substring(5);
            // 방향키 간략화
            if (name == "UpArrow") return "↑";
            if (name == "DownArrow") return "↓";
            if (name == "LeftArrow") return "←";
            if (name == "RightArrow") return "→";
            if (name == "Backspace") return "BS";
            if (name == "Delete") return "DEL";
            if (name == "Escape") return "ESC";
            return name;
        }

        private static Texture2D CreateColorTexture(Color color)
        {
            Texture2D texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, color);
            texture.Apply();
            return texture;
        }
    }

    // ==========================================
    // 키바인딩 수정 완료/취소 시 실시간 캐시 갱신 패치
    // ==========================================

    [HarmonyLib.HarmonyPatch(typeof(Il2CppUI.Panels.PnlInputs.PCInputModules.PnlInputKeyboard), "OnClickBtnCustomComplete", new Type[] { typeof(string) })]
    public static class PnlInputKeyboard_OnClickBtnCustomComplete_Patch
    {
        public static void Postfix(string keyName)
        {
            MelonLogger.Msg($"[InputOverlay.Hook] OnClickBtnCustomComplete 호출됨: keyName={keyName}");
            InputOverlay.ResetCache();
        }
    }

    [HarmonyLib.HarmonyPatch(typeof(Il2CppUI.Panels.PnlInputs.PCInputModules.PnlInputKeyboard), "OnCancelCustomize")]
    public static class PnlInputKeyboard_OnCancelCustomize_Patch
    {
        public static void Postfix()
        {
            MelonLogger.Msg("[InputOverlay.Hook] OnCancelCustomize 호출됨");
            InputOverlay.ResetCache();
        }
    }
}
