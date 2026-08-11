using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using HarmonyLib;
using Il2Cpp;
using MelonLoader;
using MelonLoader.Utils;

namespace muse_dash_test
{
    /// <summary>
    /// 스파인 "액션 계약서" 프로브.
    ///
    /// 완전한 커스텀 캐릭터가 가능한지는 결국 아래 세 목록을 맞대 보면 답이 나옵니다.
    ///   1) 공급(액션 정의)      : 프리팹이 들고 있는 SkeletActionData 배열(<c>actionData</c>).
    ///                             게임 액션 키(name)와 실제 애니메이션 참조(spineActionKeyIndex)의 매핑 레이어입니다.
    ///   2) 공급(실제 애니메이션): 스파인 스켈레톤이 실제로 가지고 있는 애니메이션 이름 목록.
    ///   3) 수요                 : 게임이 <c>TryGetSkeletActionData</c>로 실제 요청한 액션 키와 성공/실패.
    ///
    /// 3(수요)이 곧 커스텀 스켈레톤이 만족해야 할 계약서이고, 1과 2를 비교하면
    /// "이름을 게임에 맞춰야 하는가, 아니면 매핑을 내 쪽으로 돌릴 수 있는가"가 갈립니다.
    ///
    /// 공급 덤프와 수요 추적은 서로 독립적입니다. 한쪽이 실패해도 다른 쪽 결과는 그대로 남습니다.
    /// 이 프로브는 읽기 전용이며 게임 상태를 일절 바꾸지 않습니다.
    /// 조사가 끝나면 이 파일은 삭제해도 무방합니다.
    /// </summary>
    internal static class SpineActionContract
    {
        /// <summary>계약서 텍스트가 떨어지는 폴더. 게임 루트의 "spine contract".</summary>
        public static readonly string ContractDirectory =
            Path.Combine(MelonEnvironment.GameRootDirectory, "spine contract");

        /// <summary>공급 덤프는 GameObject 이름당 1회만 수행합니다(매 씬 재생성 시 중복 방지).</summary>
        private static readonly HashSet<string> DumpedObjects = new HashSet<string>();

        /// <summary>게임이 요청한 액션 키 → 한 번이라도 해석에 성공했는지 여부.</summary>
        private static readonly Dictionary<string, bool> RequestedKeys = new Dictionary<string, bool>();

        /// <summary>파일 쓰기에 반복 실패할 때 로그가 폭발하지 않도록 1회만 알립니다.</summary>
        private static bool hasWarnedWriteFailure;

        private static readonly UTF8Encoding Utf8Bom = new UTF8Encoding(true);

        // ───────────────────────────────── 공급 ─────────────────────────────────

        /// <summary>
        /// 해당 컨트롤러의 액션 정의 배열과 실제 스파인 애니메이션 목록을 파일로 덤프합니다.
        /// 두 목록은 서로 독립적으로 수집되어, 한쪽 수집이 실패해도 나머지는 기록됩니다.
        /// </summary>
        public static void DumpSupply(SpineActionController sac)
        {
            if (sac == null) return;

            string objName;
            try
            {
                objName = sac.gameObject.name;
            }
            catch (Exception)
            {
                return;
            }

            if (string.IsNullOrEmpty(objName)) return;
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

                    sb.AppendLine(
                        $"  [{i,3}] name=\"{d.name}\"" +
                        $" spineActionKeyIndex={d.spineActionKeyIndex}" +
                        $" actionIdx={d.actionIdx}" +
                        $" actionEventIdx={d.actionEventIdx}" +
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
        /// 커스텀 스켈레톤을 주입했다면 여기에 커스텀 애니메이션 이름이 나옵니다.
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

                // 이미 로드된 데이터를 재사용합니다(quiet: true → 없으면 새로 읽지 않음).
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

        /// <summary>
        /// 게임이 요청한 액션 키를 누적합니다. 한 번이라도 해석에 성공하면 성공으로 승격합니다.
        /// 새 키가 등장했을 때만 파일을 갱신하므로 매 프레임 디스크를 때리지 않습니다.
        /// </summary>
        public static void RecordDemand(string actionKey, bool resolved)
        {
            if (string.IsNullOrEmpty(actionKey)) return;

            bool isNewKey = !RequestedKeys.TryGetValue(actionKey, out bool wasResolved);
            if (!isNewKey && (wasResolved || !resolved)) return;

            RequestedKeys[actionKey] = resolved || wasResolved;
            FlushDemand();

            if (isNewKey)
            {
                MelonLogger.Msg($"[SpineContract] 요청된 액션 키: \"{actionKey}\" (해석 {(resolved ? "성공" : "실패")})");
            }
        }

        /// <summary>누적된 수요 목록을 하나의 파일로 갱신합니다.</summary>
        private static void FlushDemand()
        {
            var sb = new StringBuilder();
            sb.AppendLine("# 스파인 액션 계약서 — 수요 측");
            sb.AppendLine("# 게임이 TryGetSkeletActionData 로 실제 요청한 액션 키 목록입니다.");
            sb.AppendLine("# 이 목록이 곧 커스텀 스켈레톤이 만족해야 할 계약서입니다.");
            sb.AppendLine($"# 갱신 시각 : {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            sb.AppendLine($"# 총 {RequestedKeys.Count}개");
            sb.AppendLine();

            var keys = new List<string>(RequestedKeys.Keys);
            keys.Sort(StringComparer.Ordinal);
            foreach (var key in keys)
            {
                sb.AppendLine($"  {(RequestedKeys[key] ? "[해석 성공]" : "[해석 실패]")} \"{key}\"");
            }

            WriteFile("_요청된_액션_키.txt", sb.ToString());
        }

        // ─────────────────────────────── 파일 출력 ───────────────────────────────

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

        /// <summary>"black_girl_battle(Clone)" 처럼 경로에 못 쓰는 문자가 섞인 이름을 파일명으로 정규화합니다.</summary>
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
    /// 오브젝트가 스테이지에 붙는 시점에 공급 목록을 덤프합니다.
    /// (기존 스킨 주입 패치가 이미 검증한 훅 지점이라 안전합니다.)
    /// </summary>
    [HarmonyPatch(typeof(SpineActionController), nameof(SpineActionController.OnControllerStart))]
    internal static class Patch_SpineActionContract_Supply
    {
        public static bool Prepare() => true;

        public static void Postfix(SpineActionController __instance)
        {
            try
            {
                SpineActionContract.DumpSupply(__instance);
            }
            catch (Exception ex)
            {
                MelonLogger.Warning($"[SpineContract] 공급 덤프 예외: {ex.Message}");
            }
        }
    }

    /// <summary>
    /// 게임이 액션 키를 해석할 때마다 수요 목록에 누적합니다.
    /// <c>TryGetSkeletActionData(string, out SkeletActionData)</c> 는 private 이므로 이름으로 지정하고,
    /// out 파라미터는 선언하지 않아 마샬링에 관여하지 않습니다(__0 = 첫 번째 인자인 액션 키).
    /// </summary>
    [HarmonyPatch(typeof(SpineActionController), "TryGetSkeletActionData")]
    internal static class Patch_SpineActionContract_Demand
    {
        public static bool Prepare() => true;

        public static void Postfix(string __0, bool __result)
        {
            try
            {
                SpineActionContract.RecordDemand(__0, __result);
            }
            catch (Exception ex)
            {
                MelonLogger.Warning($"[SpineContract] 수요 기록 예외: {ex.Message}");
            }
        }
    }
}
