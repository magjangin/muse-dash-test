using System;
using MelonLoader;
using UnityEngine;

namespace muse_dash_test
{
    /// <summary>
    /// 플레이어가 설정한 공격 키(지상/공중)를 실시간으로 모니터링하여
    /// 화면 하단 중앙에 시각적인 텍스트 박스로 입력 상태(Key Viewer / Input Overlay)를 표시해 주는 모드 클래스입니다.
    /// 설정 파싱/렌더링/하모니 패치는 InputOverlay.Config / InputOverlay.Render / InputOverlay.Patches 파일로 분리되어 있습니다.
    /// </summary>
    public static partial class InputOverlay
    {
        private static readonly System.Collections.Generic.List<UnityEngine.KeyCode> airKeys = new System.Collections.Generic.List<UnityEngine.KeyCode>();
        private static readonly System.Collections.Generic.List<UnityEngine.KeyCode> groundKeys = new System.Collections.Generic.List<UnityEngine.KeyCode>();
        private static bool keysLoaded = false;
        private static float checkTimer = 0f;
        private const float CheckInterval = 2.0f; // 키 세팅 재스캔 간격 (설정 변경 연동)

        public static void ResetCache()
        {
            keysLoaded = false;
            MelonLogger.Msg("[InputOverlay] 키 바인딩 캐시가 초기화되었습니다. 다음 프레임에 재로드합니다.");
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
    }
}
