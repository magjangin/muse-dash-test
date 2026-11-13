using HarmonyLib;
using MelonLoader;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace muse_dash_test
{
    /// <summary>
    /// 모드 어셈블리의 모든 [HarmonyPatch] 대상이 현재 게임 빌드에서 실제로 해석되는지 시작 시 점검합니다.
    /// 게임 업데이트로 타입/메서드가 사라지면 어떤 패치가 비활성화될지 한눈에 요약 로그로 보여줍니다.
    /// (패치 적용 여부와 무관하게, AccessTools로 대상 존재만 독립적으로 검사하므로 호출 시점 제약이 없습니다.)
    /// </summary>
    public static partial class PatchHealthCheck
    {
        public static void Run()
        {
            try
            {
                var missing = new List<string>();
                var unresolvable = new List<string>();
                var unreadable = new List<string>();
                var dynamicTargets = new List<string>();
                var seen = new HashSet<string>(StringComparer.Ordinal);
                int total = 0;

                // 게임 업데이트로 타입이 사라졌을 때가 이 점검이 가장 필요한 순간인데, 예전에는 그때 통째로 죽었습니다.
                // [HarmonyPatch(typeof(사라진타입))] 클래스 하나에서 GetCustomAttributes가 TypeLoadException을 던지면
                // 바깥 catch로 빠져 나머지 클래스는 점검조차 안 됐습니다(2026-09-27 실측, .NET 6/10 동일).
                // 그래서 타입 목록은 MelonLoader와 같은 GetValidTypes로 받고, 클래스마다 따로 격리합니다.
                foreach (var type in Assembly.GetExecutingAssembly().GetValidTypes())
                {
                    try
                    {
                        CheckPatchClass(type, seen, missing, unresolvable, dynamicTargets, ref total);
                    }
                    catch (Exception ex)
                    {
                        unreadable.Add($"{type.Name}: {ex.GetType().Name}: {ex.Message}");
                    }
                }

                if (missing.Count == 0)
                {
                    ModLogger.Msg($"[PatchHealth] 패치 대상 {total}개 전부 정상 해석되었습니다.");
                }
                else
                {
                    ModLogger.Warning($"[PatchHealth] {missing.Count}/{total}개 패치 대상이 현재 게임 빌드에서 해석되지 않았습니다(해당 기능 비활성 가능):");
                    foreach (var m in missing)
                    {
                        ModLogger.Warning($"[PatchHealth]   - {m}");
                    }
                }

                if (unreadable.Count > 0)
                {
                    ModLogger.Warning($"[PatchHealth] 어트리뷰트를 읽지 못한 패치 클래스 {unreadable.Count}개(대상 타입이 게임에서 사라졌을 수 있음):");
                    foreach (var u in unreadable)
                    {
                        ModLogger.Warning($"[PatchHealth]   - {u}");
                    }
                }

                if (unresolvable.Count > 0)
                {
                    ModLogger.Warning($"[PatchHealth] 대상 타입을 알 수 없어 점검하지 못한 패치 {unresolvable.Count}개:");
                    foreach (var u in unresolvable)
                    {
                        ModLogger.Warning($"[PatchHealth]   - {u}");
                    }
                }

                if (dynamicTargets.Count > 0)
                {
                    ModLogger.Msg($"[PatchHealth] 동적 대상 패치(TargetMethod/TargetMethods) 정상 확인: {string.Join(", ", dynamicTargets)}");
                }

                // 추가: MusicTagManager.InitAlbumTagInfo 패치가 깨졌는지 여부 감지 및 Init 메서드 덤프 로직
                CheckMusicTagManagerPatchHealth();
            }
            catch (Exception ex)
            {
                ModLogger.Error($"[PatchHealth] 패치 점검 중 예외: {ex}");
            }
        }

        /// <summary>
        /// <see cref="PatchInstaller"/>가 패치를 실제로 걸기 전에 대상 메서드들이 현재 게임 빌드에 존재하는지 확인합니다.
        /// </summary>
        internal static void InspectPatchClassTargets(
            Type patchClass,
            out int resolvableCount,
            out List<string> missingTargets)
        {
            resolvableCount = 0;
            missingTargets = new List<string>();

            var classAttrs = patchClass.GetCustomAttributes(typeof(HarmonyPatch), true);
            if (classAttrs == null || classAttrs.Length == 0) return;

            if (HasDynamicTargets(patchClass))
            {
                if (TryResolveDynamicTargetCount(patchClass, out int dynCount, out string dynError) && dynCount > 0)
                {
                    resolvableCount = dynCount;
                }
                else
                {
                    missingTargets.Add($"{patchClass.Name} → {dynError ?? "TargetMethod/TargetMethods 결과 없음"}");
                }
                return;
            }

            PatchTarget classTarget = Merge(default(PatchTarget), classAttrs);
            foreach (var target in EnumerateTargets(patchClass, classTarget))
            {
                string label = $"{patchClass.Name} → {target.Describe()}";
                if (target.DeclaringType == null)
                {
                    missingTargets.Add(label);
                    continue;
                }

                if (TargetExists(target.DeclaringType, target.MethodName, target.ArgumentTypes, target.MethodType))
                {
                    resolvableCount++;
                }
                else
                {
                    missingTargets.Add(label);
                }
            }
        }

        /// <summary>패치 클래스 하나의 대상들을 점검해 결과 목록에 더합니다. 패치 클래스가 아니면 아무것도 하지 않습니다.</summary>
        private static void CheckPatchClass(
            Type type,
            HashSet<string> seen,
            List<string> missing,
            List<string> unresolvable,
            List<string> dynamicTargets,
            ref int total)
        {
            var classAttrs = type.GetCustomAttributes(typeof(HarmonyPatch), true);
            if (classAttrs == null || classAttrs.Length == 0) return;

            // 대상을 런타임에 계산하는 패치(TargetMethod/TargetMethods)도 직접 호출해 최소 1개 이상 해석되는지 점검합니다.
            if (HasDynamicTargets(type))
            {
                total++;
                if (TryResolveDynamicTargetCount(type, out int dynCount, out string dynError) && dynCount > 0)
                {
                    dynamicTargets.Add($"{type.Name}({dynCount}개)");
                }
                else
                {
                    missing.Add($"{type.Name} → 동적 대상 해석 실패 ({dynError ?? "0개 반환"})");
                }
                return;
            }

            PatchTarget classTarget = Merge(default(PatchTarget), classAttrs);

            foreach (var target in EnumerateTargets(type, classTarget))
            {
                string label = $"{type.Name} → {target.Describe()}";
                if (!seen.Add(label)) continue;

                if (target.DeclaringType == null)
                {
                    unresolvable.Add(label);
                    continue;
                }

                total++;
                if (!TargetExists(target.DeclaringType, target.MethodName, target.ArgumentTypes, target.MethodType))
                {
                    missing.Add(label);
                }
            }
        }

        internal static bool TryResolveDynamicTargets(Type patchClass, out List<MethodBase> targets, out string error)
        {
            targets = new List<MethodBase>();
            error = null;
            try
            {
                foreach (var method in patchClass.GetMethods(
                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.DeclaredOnly))
                {
                    if (method.GetParameters().Length != 0) continue;

                    if (string.Equals(method.Name, "TargetMethod", StringComparison.Ordinal)
                        || method.IsDefined(typeof(HarmonyTargetMethod), true))
                    {
                        var resolved = method.Invoke(null, null) as MethodBase;
                        if (resolved != null) targets.Add(resolved);
                    }
                    else if (string.Equals(method.Name, "TargetMethods", StringComparison.Ordinal)
                        || method.IsDefined(typeof(HarmonyTargetMethods), true))
                    {
                        if (method.Invoke(null, null) is IEnumerable enumerable)
                        {
                            foreach (var item in enumerable)
                            {
                                if (item is MethodBase mb) targets.Add(mb);
                            }
                        }
                    }
                }
                return targets.Count > 0;
            }
            catch (Exception ex)
            {
                var inner = ex is TargetInvocationException tie && tie.InnerException != null ? tie.InnerException : ex;
                error = $"{inner.GetType().Name}: {inner.Message}";
                return false;
            }
        }

        private static bool TryResolveDynamicTargetCount(Type patchClass, out int count, out string error)
        {
            bool ok = TryResolveDynamicTargets(patchClass, out var list, out error);
            count = list != null ? list.Count : 0;
            return ok;
        }
    }
}
