using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Il2Cpp;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using MelonLoader;
using MelonLoader.Utils;

namespace muse_dash_test
{
    /// <summary>
    /// 스파인 "액션 계약서" 프로브 (공급 측 덤프 전용).
    /// 배틀 캐릭터 로드 시 프리팹의 SkeletActionData 배열과 스켈레톤의 애니메이션 목록을 txt 파일로 덤프합니다.
    /// </summary>
    internal static class SpineActionContract
    {
        /// <summary>계약서 텍스트가 떨어지는 폴더. 게임 루트의 "spine contract".</summary>
        public static readonly string ContractDirectory =
            Path.Combine(MelonEnvironment.GameRootDirectory, "spine contract");

        private const string TargetNameFragment = "battle";
        private static readonly HashSet<string> DumpedObjects = new HashSet<string>();
        private static bool hasWarnedWriteFailure;
        private static readonly UTF8Encoding Utf8Bom = new UTF8Encoding(true);

        public static void ResetWindow() { }

        public static void DumpSupply(SpineActionController sac)
        {
            try
            {
                DumpSupplyCore(sac, requireNameFragment: true);
            }
            catch (Exception ex)
            {
                MelonLogger.Warning($"[SpineContract] 공급 덤프 예외: {ex.Message}");
            }
        }

        /// <summary>
        /// 이름 필터("battle")를 무시하고 덤프합니다. 노트 오브젝트처럼 이름 규칙이 다른 대상용입니다.
        /// 오브젝트 이름 단위 1회 덤프는 그대로 유지됩니다.
        /// </summary>
        public static void DumpSupplyForce(SpineActionController sac)
        {
            try
            {
                DumpSupplyCore(sac, requireNameFragment: false);
            }
            catch (Exception ex)
            {
                MelonLogger.Warning($"[SpineContract] 강제 공급 덤프 예외: {ex.Message}");
            }
        }

        private static void DumpSupplyCore(SpineActionController sac, bool requireNameFragment)
        {
            if (!InputOverlay.dumpSpineContract) return;
            if (sac == null) return;

            string objName = sac.gameObject.name;
            if (string.IsNullOrEmpty(objName)) return;
            if (requireNameFragment && objName.IndexOf(TargetNameFragment, StringComparison.OrdinalIgnoreCase) < 0) return;

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

        /// <summary>
        /// 계약서 파일을 씁니다. `config.txt`의 '스파인 계약서 덤프'가 꺼져 있으면 폴더도 만들지 않고 그냥 돌아갑니다.
        /// 폴더 생성이 여기 한 곳뿐이라, 이 게이트가 곧 'spine contract' 폴더 생성 스위치입니다.
        /// </summary>
        public static void WriteFile(string fileName, string content)
        {
            if (!InputOverlay.dumpSpineContract) return;

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
}
