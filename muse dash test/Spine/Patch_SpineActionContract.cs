using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using HarmonyLib;
using Il2Cpp;
using Il2CppAssets.Scripts.GameCore.Managers;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using MelonLoader;
using MelonLoader.Utils;

namespace muse_dash_test
{
    /// <summary>
    /// 스파인 "액션 계약서" 프로브 — 1단계(공급 측).
    ///
    /// 완전한 커스텀 캐릭터가 가능한지는 아래 세 목록을 맞대 보면 답이 나옵니다.
    ///   1) 액션 정의   : 프리팹이 들고 있는 SkeletActionData 배열(<c>actionData</c>).
    ///                    게임 액션 키(name)와 실제 애니메이션 참조(spineActionKeyIndex)의 매핑 레이어입니다.
    ///   2) 실제 애니메이션 : 스파인 스켈레톤이 실제로 보유한 애니메이션 이름 목록.
    ///   3) 수요        : 게임이 실제로 요청한 액션 키. ← 2단계에서 별도로 수집합니다.
    ///
    /// 1과 2를 나란히 보면 "게임이 부르는 이름"과 "스켈레톤이 가진 이름"이 같은지 다른지가 드러납니다.
    /// 다르면 매핑 레이어가 실재한다는 뜻이라 커스텀 애니메이션 이름을 자유롭게 쓸 수 있습니다.
    ///
    /// 수요 수집(<c>TryGetSkeletActionData</c> 훅)은 이 파일에서 의도적으로 제외했습니다.
    /// 해당 메서드는 private 이며 out 파라미터를 가지는데, IL2CPP 환경에서 이를 패치하자
    /// 배틀 시작 직후 관리형 예외 없이 프로세스가 종료됐습니다(2026-08-11). 원인이 확정되기 전까지
    /// 공급 측만 수집하고, 수요는 public·단순 시그니처 메서드로 따로 붙입니다.
    ///
    /// 이 프로브는 읽기 전용이며 게임 상태를 일절 바꾸지 않습니다.
    /// 조사가 끝나면 이 파일은 삭제해도 무방합니다.
    /// </summary>
    internal static class SpineActionContract
    {
        /// <summary>계약서 텍스트가 떨어지는 폴더. 게임 루트의 "spine contract".</summary>
        public static readonly string ContractDirectory =
            Path.Combine(MelonEnvironment.GameRootDirectory, "spine contract");

        /// <summary>
        /// 덤프 대상을 가리는 이름 조각. 노트/적 오브젝트까지 포함하면 한 스테이지에 수백 개가 잡혀
        /// 배틀 시작 시점에 파일 쓰기와 스켈레톤 조회가 몰리므로, 배틀 캐릭터만 남깁니다.
        /// (기존 Patch_SkinNameProbe 와 같은 기준입니다.)
        /// </summary>
        private const string TargetNameFragment = "battle";

        /// <summary>공급 덤프는 GameObject 이름당 1회만 수행합니다(씬 재진입 시 중복 방지).</summary>
        private static readonly HashSet<string> DumpedObjects = new HashSet<string>();

        /// <summary>파일 쓰기에 반복 실패할 때 로그가 폭발하지 않도록 1회만 알립니다.</summary>
        private static bool hasWarnedWriteFailure;

        private static readonly UTF8Encoding Utf8Bom = new UTF8Encoding(true);

        /// <summary>
        /// 배틀 캐릭터의 액션 정의 배열과 실제 스파인 애니메이션 목록을 파일로 덤프합니다.
        /// 두 목록은 독립적으로 수집되어, 한쪽 수집이 실패해도 나머지는 기록됩니다.
        /// </summary>
        public static void DumpSupply(SpineActionController sac)
        {
            // 스킨 주입 Postfix 안에서 호출되므로, 어떤 예외도 주입 경로로 새어 나가지 않게 전체를 감쌉니다.
            try
            {
                DumpSupplyCore(sac);
            }
            catch (Exception ex)
            {
                MelonLogger.Warning($"[SpineContract] 공급 덤프 예외: {ex.Message}");
            }
        }

        private static void DumpSupplyCore(SpineActionController sac)
        {
            if (sac == null) return;

            string objName = sac.gameObject.name;
            if (string.IsNullOrEmpty(objName)) return;
            if (objName.IndexOf(TargetNameFragment, StringComparison.OrdinalIgnoreCase) < 0) return;
            if (!DumpedObjects.Add(objName)) return;

            var sb = new StringBuilder();
            sb.AppendLine("# 스파인 액션 계약서 — 공급 측");
            sb.AppendLine($"# 대상 오브젝트 : {objName}");
            sb.AppendLine($"# 수집 시각     : {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            sb.AppendLine();

            AppendActionData(sb, sac);
            sb.AppendLine();
            AppendSkeletonAnimations(sb, sac);

            WriteFile(MakeSafeFileName(objName) + ".txt", sb.ToString());
            MelonLogger.Msg($"[SpineContract] 공급 덤프 완료: {objName}");
        }

        /// <summary>
        /// 프리팹에 직렬화된 <c>SkeletActionData</c> 배열을 기록합니다.
        /// 이 배열이 게임 액션 키와 실제 애니메이션 사이의 매핑 레이어입니다.
        /// </summary>
        private static void AppendActionData(StringBuilder sb, SpineActionController sac)
        {
            sb.AppendLine("## [1] 액션 정의 (actionData: SkeletActionData[])");
            try
            {
                var actionData = sac.actionData;
                if (actionData == null)
                {
                    sb.AppendLine("  (actionData 가 null 입니다)");
                    return;
                }

                int count = actionData.Length;
                sb.AppendLine($"  총 {count}개");
                sb.AppendLine();

                for (int i = 0; i < count; i++)
                {
                    var d = actionData[i];
                    if (d == null)
                    {
                        sb.AppendLine($"  [{i,3}] (null)");
                        continue;
                    }

                    // actionIdx 가 이 액션이 실제로 재생하는 스파인 애니메이션 이름 목록입니다.
                    // 게임 액션 키(name)와 애니메이션 이름을 잇는 매핑이 여기에 들어 있습니다.
                    sb.AppendLine($"  [{i,3}] name=\"{d.name}\" → [{JoinStrings(d.actionIdx)}]");
                    sb.AppendLine(
                        $"        spineActionKeyIndex={d.spineActionKeyIndex}" +
                        $" actionEventIdx=[{JoinInts(d.actionEventIdx)}]" +
                        $" protectLevel={d.protectLevel}" +
                        $" isSelfProtect={d.isSelfProtect}" +
                        $" isEndLoop={d.isEndLoop}" +
                        $" isRandomSequence={d.isRandomSequence}" +
                        $" isIgnoreAnimationClip={d.isIgnoreAnimationClip}");
                }
            }
            catch (Exception ex)
            {
                sb.AppendLine($"  (수집 실패: {ex.Message})");
            }
        }

        /// <summary>
        /// 스켈레톤이 실제로 보유한 애니메이션 이름을 기록합니다.
        /// 커스텀 스켈레톤이 주입된 뒤라면 여기에 커스텀 애니메이션 이름이 나옵니다.
        /// </summary>
        private static void AppendSkeletonAnimations(StringBuilder sb, SpineActionController sac)
        {
            sb.AppendLine("## [2] 실제 스파인 애니메이션 (SkeletonData.Animations)");
            try
            {
                var ska = sac.skeletonAnimation;
                if (ska == null)
                {
                    sb.AppendLine("  (skeletonAnimation 이 null 입니다)");
                    return;
                }

                var asset = ska.skeletonDataAsset;
                if (asset == null)
                {
                    sb.AppendLine("  (skeletonDataAsset 이 null 입니다)");
                    return;
                }

                // 인자는 quiet 플래그입니다(로딩 실패 시 로그를 남기지 않음).
                // CustomSkinInjector 가 이미 같은 호출로 스켈레톤을 확보하고 있어 경로 자체는 검증돼 있습니다.
                var skeletonData = asset.GetSkeletonData(true);
                if (skeletonData == null)
                {
                    sb.AppendLine("  (SkeletonData 를 얻지 못했습니다)");
                    return;
                }

                var animations = skeletonData.Animations;
                if (animations == null)
                {
                    sb.AppendLine("  (Animations 가 null 입니다)");
                    return;
                }

                int count = animations.Count;
                sb.AppendLine($"  총 {count}개");
                sb.AppendLine();

                for (int i = 0; i < count; i++)
                {
                    var anim = animations.Items[i];
                    sb.AppendLine($"  [{i,3}] \"{(anim != null ? anim.Name : "(null)")}\"");
                }
            }
            catch (Exception ex)
            {
                sb.AppendLine($"  (수집 실패: {ex.Message})");
            }
        }

        // ───────────────────────────────── 수요 ─────────────────────────────────

        /// <summary>이미 기록한 (출처, 키) 조합. 같은 키가 매 프레임 반복돼도 한 번만 남깁니다.</summary>
        private static readonly HashSet<string> SeenDemand = new HashSet<string>();

        /// <summary>지금 샌드백 연타 구간 안인지. 이 구간에서만 중복 제거 없이 기록합니다.</summary>
        public static bool InMultiHit { get; private set; }

        private static float _lastAnimTime;
        private static string _currentMultiHitAnim = "double_hit_1";
        private const float MultiHitAnimInterval = 0.15f; // 150ms 간격으로 교체하여 끊김 없이 매끄럽게 재생

        /// <summary>
        /// 연타 구간에서 애니메이션이 1프레임만에 뜩뜩 끊기지 않도록 150ms 간격으로
        /// "double_hit_1"과 "double_hit_2"를 매끄럽게 교체합니다.
        /// </summary>
        public static string GetMultiHitAnimation()
        {
            float now = UnityEngine.Time.time;
            if (now - _lastAnimTime >= MultiHitAnimInterval)
            {
                _lastAnimTime = now;
                _currentMultiHitAnim = (_currentMultiHitAnim == "double_hit_1") ? "double_hit_2" : "double_hit_1";
            }
            return _currentMultiHitAnim;
        }

        /// <summary>
        /// 액션 키를 보고 연타 구간의 경계를 추적합니다.
        /// 두 마커 키는 actionData 에 정의가 없어 애니메이션을 재생하지 않는 순수 신호입니다.
        /// </summary>
        public static void TrackMultiHitWindow(string actionKey)
        {
            if (actionKey == MultiHitStartKey)
            {
                InMultiHit = true;
                _lastAnimTime = 0f;
                _currentMultiHitAnim = "double_hit_1";
                MelonLogger.Msg("[샌드백원본] 연타 구간 진입 — double_hit_1 / double_hit_2 매끄러운 교체 재생을 적용합니다.");
            }
            else if (actionKey == MultiHitEndKey)
            {
                InMultiHit = false;
                MelonLogger.Msg("[샌드백원본] 연타 구간 종료.");
            }
        }

        private static float _groundWorldY = float.NaN;
        private static bool _hasCapturedGroundY;

        /// <summary>
        /// 평소(지상 이동 중)일 때 캐릭터의 실제 월드 Y 좌표를 기억합니다.
        /// </summary>
        public static void CaptureGroundYIfNeeded(SpineActionController sac)
        {
            if (sac == null || sac.transform == null || InMultiHit) return;
            try
            {
                if (!_hasCapturedGroundY)
                {
                    _groundWorldY = sac.transform.position.y;
                    _hasCapturedGroundY = true;
                    MelonLogger.Msg($"[SpineContract] 지상 WorldY 좌표 캡처 완료: {_groundWorldY}");
                }
            }
            catch { }
        }

        /// <summary>
        /// 샌드백 연타 구간 동안 캐릭터가 공중으로 붕 뜨지 않도록 월드/로컬 Y 좌표를 지상 높이로 강제 고정합니다.
        /// </summary>
        public static void EnforceGroundPosition(SpineActionController sac)
        {
            try
            {
                if (sac == null || sac.transform == null) return;

                if (!float.IsNaN(_groundWorldY))
                {
                    var worldPos = sac.transform.position;
                    if (Math.Abs(worldPos.y - _groundWorldY) > 0.001f)
                    {
                        worldPos.y = _groundWorldY;
                        sac.transform.position = worldPos;
                    }
                }

                var localPos = sac.transform.localPosition;
                if (localPos.y != 0f)
                {
                    localPos.y = 0f;
                    sac.transform.localPosition = localPos;
                }

                if (sac.transform.parent != null)
                {
                    var parentPos = sac.transform.parent.localPosition;
                    if (parentPos.y != 0f)
                    {
                        parentPos.y = 0f;
                        sac.transform.parent.localPosition = parentPos;
                    }
                }
            }
            catch { }
        }

        /// <summary>씬을 벗어날 때 구간 상태가 남아 있지 않도록 초기화합니다.</summary>
        public static void ResetWindow()
        {
            InMultiHit = false;
            _lastAnimTime = 0f;
            _currentMultiHitAnim = "double_hit_1";
            _hasCapturedGroundY = false;
            _groundWorldY = float.NaN;
            FlushDemand();
        }

        /// <summary>기록 순서를 보존한 수요 목록. 파일에 등장 순서대로 씁니다.</summary>
        private static readonly List<string> DemandLines = new List<string>();

        /// <summary>훅이 실제로 도는지 1회만 확인 로그를 남깁니다(등록만 되고 실행 안 되는 사례가 있었음).</summary>
        private static readonly HashSet<string> AliveHooks = new HashSet<string>();

        /// <summary>
        /// 게임이 요청한 액션 키/애니메이션 이름을 누적합니다.
        /// 매 타격마다 디스크에 파일 쓰기를 수행하면 렉이 발생하므로 메인 타격 도중 FlushDemand() 호출은 제외합니다.
        /// </summary>
        public static void RecordDemand(string source, string key, string objName)
        {
            try
            {
                if (AliveHooks.Add(source))
                {
                    MelonLogger.Msg($"[SpineContract.Hook] \"{source}\" 훅 살아있음");
                }

                if (string.IsNullOrEmpty(key)) key = "(null)";

                string entry = $"{source,-22} \"{key}\"" + (string.IsNullOrEmpty(objName) ? "" : $"   ← {objName}");
                if (!SeenDemand.Add(entry)) return;

                DemandLines.Add(entry);
                MelonLogger.Msg($"[SpineContract.수요] {entry}");
            }
            catch (Exception ex)
            {
                MelonLogger.Warning($"[SpineContract] 수요 기록 예외: {ex.Message}");
            }
        }

        /// <summary>샌드백 연타 구간의 시작/종료를 알리는 상태 마커 키.</summary>
        private const string MultiHitStartKey = "char_multihit_start";
        private const string MultiHitEndKey = "char_multihit_end";

        /// <summary>
        /// 중복 제거 없이 호출을 그대로 남깁니다. 샌드백 연타 구간처럼 "같은 키가 몇 번, 어떤 간격으로
        /// 반복되는가"가 정보인 구간에서만 씁니다. 곡 전체에 켜면 로그가 폭발하므로 구간 밖에서는
        /// 호출하지 않습니다.
        /// </summary>
        public static void RecordRaw(string source, string key, string objName)
        {
            try
            {
                MelonLogger.Msg($"[샌드백원본] {source,-13} \"{key ?? "(null)"}\""
                    + (string.IsNullOrEmpty(objName) ? "" : $"   ← {objName}"));
            }
            catch (Exception)
            {
                // 관측 실패가 게임 흐름을 막지 않도록 조용히 넘깁니다.
            }
        }

        /// <summary>배틀 캐릭터에서 온 호출인지 판별합니다. 노트/적 오브젝트의 잡음을 걸러냅니다.</summary>
        public static bool IsBattleObject(SpineActionController sac)
        {
            try
            {
                if (sac == null) return false;
                string n = sac.gameObject.name;
                return !string.IsNullOrEmpty(n)
                    && n.IndexOf(TargetNameFragment, StringComparison.OrdinalIgnoreCase) >= 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>배틀 오브젝트 이름을 안전하게 읽습니다.</summary>
        public static string SafeName(SpineActionController sac)
        {
            try
            {
                return sac != null ? sac.gameObject.name : "(null)";
            }
            catch (Exception)
            {
                return "(이름 실패)";
            }
        }

        /// <summary>누적된 수요 목록을 등장 순서대로 파일에 씁니다.</summary>
        private static void FlushDemand()
        {
            var sb = new StringBuilder();
            sb.AppendLine("# 스파인 액션 계약서 — 수요 측");
            sb.AppendLine("# 게임이 실제로 요청한 액션 키와 애니메이션 이름입니다. 처음 등장한 순서대로 기록됩니다.");
            sb.AppendLine("#");
            sb.AppendLine("#   PlayByKey            : 게임이 부르는 액션 키");
            sb.AppendLine("#   SetAnimation         : 스파인에 최종적으로 넘어간 애니메이션 이름");
            sb.AppendLine("#   AttacksWithoutExchg  : 공격 디스패처가 넘긴 액션 키 (result/id 포함)");
            sb.AppendLine($"#");
            sb.AppendLine($"# 갱신 시각 : {DateTime.Now:yyyy-MM-dd HH:mm:ss}   총 {DemandLines.Count}건");
            sb.AppendLine();

            foreach (var line in DemandLines)
            {
                sb.AppendLine("  " + line);
            }

            WriteFile("_요청된_액션_키.txt", sb.ToString());
        }

        /// <summary>Il2Cpp 문자열 배열을 사람이 읽을 수 있게 이어 붙입니다.</summary>
        private static string JoinStrings(Il2CppStringArray array)
        {
            if (array == null) return "(null)";

            var sb = new StringBuilder();
            for (int i = 0; i < array.Length; i++)
            {
                if (i > 0) sb.Append(", ");
                sb.Append('"').Append(array[i] ?? "(null)").Append('"');
            }
            return sb.ToString();
        }

        /// <summary>Il2Cpp 정수 배열을 사람이 읽을 수 있게 이어 붙입니다.</summary>
        private static string JoinInts(Il2CppStructArray<int> array)
        {
            if (array == null) return "(null)";

            var sb = new StringBuilder();
            for (int i = 0; i < array.Length; i++)
            {
                if (i > 0) sb.Append(", ");
                sb.Append(array[i]);
            }
            return sb.ToString();
        }

        private static void WriteFile(string fileName, string content)
        {
            try
            {
                Directory.CreateDirectory(ContractDirectory);
                File.WriteAllText(Path.Combine(ContractDirectory, fileName), content, Utf8Bom);
            }
            catch (Exception ex)
            {
                if (!hasWarnedWriteFailure)
                {
                    hasWarnedWriteFailure = true;
                    MelonLogger.Warning($"[SpineContract] 계약서 파일 쓰기 실패(이후 동일 오류는 생략): {ex.Message}");
                }
            }
        }

        /// <summary>"sleepy_girl_battle(Clone)" 처럼 경로에 못 쓰는 문자가 섞인 이름을 파일명으로 정규화합니다.</summary>
        private static string MakeSafeFileName(string name)
        {
            var sb = new StringBuilder(name.Length);
            var invalid = Path.GetInvalidFileNameChars();
            foreach (var c in name)
            {
                sb.Append(Array.IndexOf(invalid, c) >= 0 ? '_' : c);
            }
            return sb.ToString();
        }
    }

    /// <summary>
    /// 스파인에 최종적으로 넘어간 애니메이션 이름을 기록합니다.
    /// 액션 키가 아니라 실제 재생되는 이름이라, 매핑 결과를 그대로 관측할 수 있습니다.
    /// Prefix 인 이유: 존재하지 않는 애니메이션을 요청해 실패하더라도 "요청했다"는 사실은 남겨야 합니다.
    /// </summary>
    [HarmonyPatch(typeof(SpineActionController), nameof(SpineActionController.SetAnimation))]
    internal static class Patch_SpineContract_SetAnimation
    {
        public static void Prefix(SpineActionController __instance, ref string n)
        {
            if (!SpineActionContract.IsBattleObject(__instance)) return;

            if (!SpineActionContract.InMultiHit)
            {
                SpineActionContract.CaptureGroundYIfNeeded(__instance);
            }
            else
            {
                n = SpineActionContract.GetMultiHitAnimation();
                SpineActionContract.EnforceGroundPosition(__instance);
            }

            string objName = SpineActionContract.SafeName(__instance);
            SpineActionContract.RecordDemand("SetAnimation", n, objName);

            // 연타 구간에서는 반복 자체가 정보이므로 중복 제거 없이 한 번 더 남깁니다.
            if (SpineActionContract.InMultiHit)
            {
                SpineActionContract.RecordRaw("SetAnimation", n, objName);
            }
        }
    }

    /// <summary>게임이 부르는 액션 키를 기록합니다(매핑 이전 단계).</summary>
    [HarmonyPatch(typeof(SpineActionController), nameof(SpineActionController.PlayByKey))]
    internal static class Patch_SpineContract_PlayByKey
    {
        public static void Prefix(SpineActionController __instance, ref string actionKey)
        {
            if (!SpineActionContract.IsBattleObject(__instance)) return;

            string objName = SpineActionContract.SafeName(__instance);
            SpineActionContract.RecordDemand("PlayByKey", actionKey, objName);

            SpineActionContract.TrackMultiHitWindow(actionKey);

            if (!SpineActionContract.InMultiHit)
            {
                SpineActionContract.CaptureGroundYIfNeeded(__instance);
            }
            else
            {
                if (actionKey == "char_jumphit" || actionKey == "char_atk_p" || actionKey == "char_hit")
                {
                    actionKey = "char_bighit";
                }
                SpineActionContract.EnforceGroundPosition(__instance);
                SpineActionContract.RecordRaw("PlayByKey", actionKey, objName);
            }
        }
    }

    /// <summary>
    /// 공격 디스패처. 판정 결과(result)와 노트 id 까지 같이 넘어오므로
    /// "어떤 노트를 어떻게 쳤을 때 어떤 액션 키가 나가는가"를 직접 볼 수 있습니다.
    /// 샌드백(멀티히트) 타격이 어느 키로 가는지 확인하는 지점입니다.
    /// </summary>
    [HarmonyPatch(typeof(AbstractGirlManager), nameof(AbstractGirlManager.AttacksWithoutExchange))]
    internal static class Patch_SpineContract_Attacks
    {
        public static void Prefix(uint result, string actKey, int id)
        {
            SpineActionContract.RecordDemand("AttacksWithoutExchg", $"{actKey} (result={result}, id={id})", null);
        }
    }

    // 공급 측 덤프에는 전용 [HarmonyPatch] 클래스가 없습니다.
    //
    // SpineActionController.OnControllerStart 에 패치를 붙여 봤지만 Postfix 가 한 번도 실행되지
    // 않았습니다. 진입 즉시 무조건 찍는 로그조차 나오지 않았고, 등록과 대상 해석은 정상이었습니다
    // (PatchHealth 통과). 배틀 오브젝트에서는 OnControllerStart 자체가 호출되지 않는 것으로 보입니다
    // — 스킨 주입도 Init 훅에서만 이루어지고 있었습니다(2026-08-11).
    //
    // 판별 근거: Awake 프로브는 3개 오브젝트(ghost/battle/shadow)를 잡는데 주입 로그는 2회뿐입니다.
    // Init 과 OnControllerStart 두 훅이 모두 돌았다면 대상 오브젝트당 2회씩 찍혔어야 합니다.
    //
}
