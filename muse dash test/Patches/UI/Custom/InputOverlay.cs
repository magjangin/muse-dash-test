using System;
using System.Collections.Generic;
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
        private static readonly List<UnityEngine.KeyCode> airKeys = new List<UnityEngine.KeyCode>();
        private static readonly List<UnityEngine.KeyCode> groundKeys = new List<UnityEngine.KeyCode>();
        private static readonly List<UnityEngine.KeyCode> activeAirKeys = new List<UnityEngine.KeyCode>();
        private static readonly List<UnityEngine.KeyCode> activeGroundKeys = new List<UnityEngine.KeyCode>();
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
        /// 게임 데이터 구조: m_ButtonKeyEnties = Dictionary&lt;proposal, Dictionary&lt;keyName, List&lt;KeyCode&gt;&gt;&gt;.
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

                // 1. 현재 proposal에 해당하는 키 매핑 딕셔너리를 조회. 없으면 첫 proposal로 폴백.
                if (!TryGetDictItem(buttonKeyEnties, proposal, out var proposalDict))
                {
                    MelonLogger.Warning($"[InputOverlay] '{proposal}' proposal이 없습니다. 첫 proposal로 폴백을 시도합니다...");
                    var proposals = GetDictKeys(buttonKeyEnties);
                    if (proposals.Count == 0 || !TryGetDictItem(buttonKeyEnties, proposals[0], out proposalDict))
                    {
                        MelonLogger.Error("[InputOverlay] 사용 가능한 proposal 키가 존재하지 않습니다.");
                        return;
                    }
                    proposal = proposals[0];
                    MelonLogger.Msg($"[InputOverlay] 폴백 proposal로 '{proposal}'을 사용합니다.");
                }

                // 2. 공중/지상 공격 키 이름 확정 (정확 매칭 → 키워드 포함 폴백).
                string airKeyName = ResolveProposalKeyName(proposalDict, "KeyBattleAir", "Air");
                string groundKeyName = ResolveProposalKeyName(proposalDict, "KeyBattleGround", "Ground");

                // 3. 각 키 이름에 바인딩된 KeyCode 리스트를 추출.
                airKeys.Clear();
                if (airKeyName != null && TryGetDictItem(proposalDict, airKeyName, out var airList))
                {
                    ReadKeyCodeList(airList, airKeys);
                }

                groundKeys.Clear();
                if (groundKeyName != null && TryGetDictItem(proposalDict, groundKeyName, out var groundList))
                {
                    ReadKeyCodeList(groundList, groundKeys);
                }

                keysLoaded = true;

                MelonLogger.Msg($"[InputOverlay] 키 바인딩 데이터 로드 완료! 공중 키: [{FormatKeys(airKeys)}], 지상 키: [{FormatKeys(groundKeys)}]");
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

        // ==========================================
        // Il2Cpp 컬렉션 리플렉션 헬퍼
        // (제네릭 Il2Cpp Dictionary/List는 컴파일 타임 타입을 알 수 없어 메서드 기반 리플렉션으로 접근)
        // ==========================================

        /// <summary>
        /// Il2Cpp 딕셔너리에서 key에 해당하는 값을 ContainsKey + get_Item으로 안전하게 조회합니다.
        /// </summary>
        private static bool TryGetDictItem(object dict, string key, out object value)
        {
            value = null;
            if (dict == null || key == null) return false;

            try
            {
                var type = dict.GetType();
                var containsKey = type.GetMethod("ContainsKey");
                if (containsKey != null && !(bool)containsKey.Invoke(dict, new object[] { key }))
                {
                    return false;
                }

                var getItem = type.GetMethod("get_Item");
                if (getItem == null) return false;

                value = getItem.Invoke(dict, new object[] { key });
                return value != null;
            }
            catch (Exception ex)
            {
                MelonLogger.Warning($"[InputOverlay] 딕셔너리 조회 실패 (key={key}): {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Il2Cpp 딕셔너리의 Keys를 열거하여 문자열 리스트로 반환합니다.
        /// </summary>
        private static List<string> GetDictKeys(object dict)
        {
            var result = new List<string>();
            if (dict == null) return result;

            try
            {
                var keysObj = dict.GetType().GetProperty("Keys")?.GetValue(dict);
                if (keysObj == null) return result;

                var enumerator = keysObj.GetType().GetMethod("GetEnumerator")?.Invoke(keysObj, null);
                if (enumerator == null) return result;

                var moveNext = enumerator.GetType().GetMethod("MoveNext");
                var current = enumerator.GetType().GetProperty("Current");
                if (moveNext == null || current == null) return result;

                while ((bool)moveNext.Invoke(enumerator, null))
                {
                    var k = current.GetValue(enumerator)?.ToString();
                    if (k != null) result.Add(k);
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Warning($"[InputOverlay] 딕셔너리 키 열거 실패: {ex.Message}");
            }
            return result;
        }

        /// <summary>
        /// proposalDict에서 정확한 키명(exactKey)을 우선 찾고, 없으면 keyword를 포함하는 첫 키로 폴백합니다.
        /// </summary>
        private static string ResolveProposalKeyName(object proposalDict, string exactKey, string keyword)
        {
            try
            {
                var containsKey = proposalDict.GetType().GetMethod("ContainsKey");
                if (containsKey != null && (bool)containsKey.Invoke(proposalDict, new object[] { exactKey }))
                {
                    return exactKey;
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Warning($"[InputOverlay] '{exactKey}' 존재 확인 실패: {ex.Message}");
            }

            foreach (var k in GetDictKeys(proposalDict))
            {
                if (k.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return k;
                }
            }
            return null;
        }

        /// <summary>
        /// Il2Cpp List&lt;KeyCode&gt;를 Count + get_Item으로 순회하여 dest에 채웁니다(기존 내용 위에 추가).
        /// </summary>
        private static void ReadKeyCodeList(object listObj, List<UnityEngine.KeyCode> dest)
        {
            if (listObj == null) return;

            try
            {
                var type = listObj.GetType();
                var countProp = type.GetProperty("Count");
                var getItem = type.GetMethod("get_Item");
                if (countProp == null || getItem == null) return;

                int count = (int)countProp.GetValue(listObj);
                for (int i = 0; i < count; i++)
                {
                    dest.Add((UnityEngine.KeyCode)getItem.Invoke(listObj, new object[] { i }));
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Warning($"[InputOverlay] 키코드 리스트 읽기 실패: {ex.Message}");
            }
        }

        /// <summary>
        /// KeyCode.None을 제외한 유효한 키들만 추려 로그용 문자열로 결합합니다.
        /// </summary>
        private static string FormatKeys(List<UnityEngine.KeyCode> keys)
        {
            var clean = new List<string>();
            foreach (var k in keys)
            {
                if (k != KeyCode.None) clean.Add(k.ToString());
            }
            return string.Join(", ", clean);
        }
    }
}
