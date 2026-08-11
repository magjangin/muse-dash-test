using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Il2Cpp;
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

    // 이 프로브에는 전용 [HarmonyPatch] 클래스가 없습니다.
    // SpineActionController.OnControllerStart 에는 Patch_Inject_OnControllerStart 가 이미 붙어 있는데,
    // 같은 메서드에 두 번째 패치 클래스를 붙였더니 등록·대상 해석은 정상인데도(PatchHealth 통과)
    // Postfix 가 한 번도 실행되지 않았습니다. 진입 즉시 무조건 찍는 로그조차 나오지 않았습니다(2026-08-11).
    // 그래서 별도 클래스를 두지 않고, 이미 도는 것이 확인된 Patch_Inject_OnControllerStart 의
    // Postfix 안에서 DumpSupply 를 호출합니다.
}
