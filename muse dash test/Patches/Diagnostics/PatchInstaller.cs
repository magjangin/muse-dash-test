using HarmonyLib;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace muse_dash_test
{
    /// <summary>
    /// 이 모드의 Harmony 패치를 MelonLoader 대신 직접 겁니다(<c>[assembly: HarmonyDontPatchAll]</c>, MainMod.cs).
    /// 걸기 전에 패치 메서드를 미리 JIT 해 보고, 현재 게임 빌드에서 컴파일되지 않거나 대상 메서드가 사라진 패치는
    /// <b>걸지 않고 격리</b>합니다.
    ///
    /// <para><b>왜 필요한가</b>: 게임이 업데이트되면 MelonLoader가 Il2CppAssemblies를 새로 만듭니다. 패치 본문이
    /// 쓰던 게임 멤버가 사라졌으면 그 패치 메서드는 첫 호출 때 JIT에 실패해 <c>MissingMethodException</c>을
    /// 던집니다. 이 예외는 <b>패치 메서드 안의 try/catch로 잡히지 않습니다</b> — 메서드가 컴파일되기 전에 호출
    /// 지점에서 나기 때문입니다(2026-09-27 실측, .NET 6/10 동일). 그러면 Il2CppInterop 트램펄린이 예외를 받아
    /// "During invoking native->managed trampoline"을 <b>호출마다</b> 남기고 기본값(null/0/false)을 돌려주며,
    /// Prefix였다면 게임 원본 메서드는 아예 실행되지 않습니다. 기능 하나가 꺼지는 데서 끝나지 않고 게임 쪽이
    /// 망가집니다.</para>
    ///
    /// <para>또한 한 패치 클래스 안에 여러 메서드 패치가 묶여 있을 때(<c>MouseTouchBridgePatch</c>,
    /// <c>PnlInputMobile_LifecyclePatch</c> 등) 대상 메서드 하나가 사라지면 <c>CreateClassProcessor.Patch()</c>는
    /// 클래스 전체를 포기합니다. 이 설치기는 일부 대상만 사라진 경우 살아남은 메서드 패치들을 개별 적용하여
    /// 나머지 기능을 살립니다.</para>
    /// </summary>
    public static class PatchInstaller
    {
        public static void ApplyAll(HarmonyLib.Harmony harmony, Assembly assembly)
        {
            int applied = 0;
            int disabledByPrepare = 0;
            var skipped = new List<string>();
            var failed = new List<string>();

            foreach (Type type in assembly.GetValidTypes())
            {
                try
                {
                    bool isPatchClass = type.IsDefined(typeof(HarmonyAttribute), true);
                    if (!isPatchClass) continue;

                    // Prepare()가 false를 돌려주면 설정/로그 레벨로 비활성화된 패치이므로
                    // 불필요한 사전 JIT 컴파일·대상 검사·적용·카운트에서 모두 먼저 제외합니다.
                    if (!ShouldApplyByPrepare(type))
                    {
                        disabledByPrepare++;
                        continue;
                    }

                    if (TryFindUncompilablePatchMethod(type, out MethodInfo broken, out Exception jitError))
                    {
                        // 다중 메서드 정적 패치 클래스에서 일부 메서드만 JIT에 실패한 경우:
                        // 컴파일과 대상 해석을 모두 통과한 나머지 패치 메서드만 부분 적용해 기능을 최대한 살립니다.
                        if (!PatchHealthCheck.HasDynamicTargets(type))
                        {
                            int survivorApplied = ApplySurvivingMethodPatches(harmony, type);
                            if (survivorApplied > 0)
                            {
                                applied++;
                                ModLogger.Warning(
                                    $"[PatchInstaller] '{type.Name}' 중 JIT 실패 메서드({broken.Name}: {jitError.GetType().Name})를 건너뛰고 " +
                                    $"나머지 {survivorApplied}개 패치만 부분 적용했습니다.");
                                continue;
                            }
                        }

                        skipped.Add(type.Name);
                        ModLogger.Warning(
                            $"[PatchInstaller] '{type.Name}' 패치를 걸지 않았습니다. {broken.Name}이(가) 현재 게임 빌드에서 " +
                            $"컴파일되지 않습니다(게임 업데이트로 참조 멤버가 사라졌을 수 있음): {jitError.GetType().Name}: {jitError.Message}");
                        continue;
                    }

                    PatchHealthCheck.InspectPatchClassTargets(type, out int resolvableCount, out List<string> missingTargets);
                    if (missingTargets.Count > 0)
                    {
                        if (resolvableCount == 0)
                        {
                            skipped.Add(type.Name);
                            ModLogger.Warning(
                                $"[PatchInstaller] '{type.Name}' 패치를 걸지 않았습니다. 대상 메서드가 현재 게임 빌드에 없습니다: {string.Join(", ", missingTargets)}");
                            continue;
                        }

                        // 다중 메서드 패치 클래스에서 일부 대상만 사라진 경우: 살아남은 대상만 개별 적용합니다.
                        int survivorApplied = ApplySurvivingMethodPatches(harmony, type);
                        if (survivorApplied > 0)
                        {
                            applied++;
                            ModLogger.Warning(
                                $"[PatchInstaller] '{type.Name}' 중 사라진 대상 {missingTargets.Count}개를 건너뛰고 나머지 {survivorApplied}개 패치만 부분 적용했습니다 (제외: {string.Join(", ", missingTargets)})");
                        }
                        else
                        {
                            skipped.Add(type.Name);
                        }
                        continue;
                    }

                    harmony.CreateClassProcessor(type, allowUnannotatedType: false).Patch();
                    applied++;
                }
                catch (Exception ex)
                {
                    // JIT와 대상 해석은 통과했는데 Harmony가 거는 단계에서 실패한 경우입니다(게임 업데이트로 원본 파라미터
                    // 이름이 바뀌어 패치 메서드의 인자를 묶지 못하는 경우 등). 다중 메서드 클래스면 메서드 하나 때문에
                    // 나머지까지 꺼지지 않도록, 이미 걸린 일부를 걷어낸 뒤 메서드별로 다시 겁니다.
                    int recovered = TryRecoverFailedClass(harmony, type);
                    if (recovered > 0)
                    {
                        applied++;
                        ModLogger.Warning(
                            $"[PatchInstaller] '{type.Name}' 클래스 단위 적용이 실패해 메서드별로 다시 걸었습니다: {recovered}개 적용 " +
                            $"(실패 원인: {ex.GetType().Name}: {ex.Message})");
                        continue;
                    }

                    failed.Add(type.Name);
                    ModLogger.Error($"[PatchInstaller] '{type.FullName}' 패치 적용 실패: {ex}");
                }
            }

            string disabledNote = disabledByPrepare > 0 ? $" (설정으로 비활성 {disabledByPrepare}개)" : string.Empty;
            if (skipped.Count == 0 && failed.Count == 0)
            {
                ModLogger.Msg($"[PatchInstaller] 패치 클래스 {applied}개를 적용했습니다{disabledNote}.");
                return;
            }

            ModLogger.Warning(
                $"[PatchInstaller] 패치 클래스 {applied}개 적용{disabledNote}, {skipped.Count}개 제외(대상 미존재/JIT 실패), {failed.Count}개 적용 실패. " +
                "제외·실패한 기능만 꺼지고 게임 원본 동작은 그대로입니다.");
            if (skipped.Count > 0) ModLogger.Warning($"[PatchInstaller]   제외: {string.Join(", ", skipped)}");
            if (failed.Count > 0) ModLogger.Warning($"[PatchInstaller]   실패: {string.Join(", ", failed)}");
        }

        /// <summary>
        /// 패치 클래스에 <c>Prepare()</c> 또는 <c>[HarmonyPrepare]</c>(매개변수 없음, bool 반환)가 있으면 실행해
        /// 이 클래스를 실제로 걸어야 하는지 확인합니다. 예외가 나면 경고를 남기고 false를 돌려 안전하게 건너뜁니다.
        /// </summary>
        internal static bool ShouldApplyByPrepare(Type patchClass)
        {
            foreach (MethodInfo method in patchClass.GetMethods(
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.DeclaredOnly))
            {
                bool isPrepare = string.Equals(method.Name, "Prepare", StringComparison.Ordinal)
                    || method.IsDefined(typeof(HarmonyPrepare), true);
                if (!isPrepare) continue;
                if (method.ReturnType != typeof(bool) || method.GetParameters().Length != 0) continue;

                try
                {
                    if (!(bool)method.Invoke(null, null))
                    {
                        ModLogger.Verbose($"[PatchInstaller] '{patchClass.Name}'은(는) Prepare() 결과(false)로 건너뛰었습니다.");
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    var inner = ex is TargetInvocationException tie && tie.InnerException != null ? tie.InnerException : ex;
                    ModLogger.Warning($"[PatchInstaller] '{patchClass.Name}.Prepare()' 예외로 패치를 건너뜁니다: {inner.GetType().Name}: {inner.Message}");
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 다중 메서드 패치 클래스에서 일부 대상 메서드만 사라졌을 때, 여전히 존재하는 대상 메서드들에만
        /// 개별적으로 Prefix/Postfix/Transpiler/Finalizer를 겁니다.
        /// 동적 타깃(TargetMethod/TargetMethods)과 정적 어트리뷰트 타깃을 모두 지원합니다.
        /// </summary>
        private static int ApplySurvivingMethodPatches(HarmonyLib.Harmony harmony, Type patchClass)
        {
            if (!ShouldApplyByPrepare(patchClass)) return 0;

            int patched = 0;

            // 1) 동적 타깃(TargetMethod / TargetMethods)을 가진 클래스인 경우
            if (PatchHealthCheck.HasDynamicTargets(patchClass))
            {
                if (!PatchHealthCheck.TryResolveDynamicTargets(patchClass, out List<MethodBase> dynamicTargets, out _) || dynamicTargets.Count == 0)
                {
                    return 0;
                }

                var patchMethods = patchClass.GetMethods(
                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.DeclaredOnly)
                    .Where(m => PatchHealthCheck.IsPatchMethod(m) && TryPreparePatchMethod(m, out _)).ToList();

                foreach (var original in dynamicTargets)
                {
                    if (original == null) continue;
                    try
                    {
                        var processor = harmony.CreateProcessor(original);
                        bool anyAdded = false;
                        foreach (var method in patchMethods)
                        {
                            if (TryRegisterPatchMethod(processor, method))
                            {
                                anyAdded = true;
                            }
                        }

                        if (anyAdded)
                        {
                            processor.Patch();
                            patched++;
                        }
                    }
                    catch (Exception ex)
                    {
                        ModLogger.Warning($"[PatchInstaller] '{patchClass.Name}' 동적 타깃({original.Name}) 개별 패치 실패: {ex.Message}");
                    }
                }

                return patched;
            }

            // 2) 정적 어트리뷰트 기반 타깃인 경우
            var classAttrs = patchClass.GetCustomAttributes(typeof(HarmonyPatch), true);
            var classTarget = PatchHealthCheck.Merge(default, classAttrs);

            foreach (MethodInfo method in patchClass.GetMethods(
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.DeclaredOnly))
            {
                if (!PatchHealthCheck.IsPatchMethod(method)) continue;

                if (!TryPreparePatchMethod(method, out Exception jitError))
                {
                    ModLogger.Warning(
                        $"[PatchInstaller] '{patchClass.Name}.{method.Name}' 개별 패치 건너뜀(JIT 실패): {jitError.GetType().Name}: {jitError.Message}");
                    continue;
                }

                var methodAttrs = method.GetCustomAttributes(typeof(HarmonyPatch), true);
                var target = (methodAttrs == null || methodAttrs.Length == 0)
                    ? classTarget
                    : PatchHealthCheck.Merge(classTarget, methodAttrs);

                MethodBase original = PatchHealthCheck.ResolveTargetMethod(
                    target.DeclaringType, target.MethodName, target.ArgumentTypes, target.MethodType);
                if (original == null) continue;

                try
                {
                    var processor = harmony.CreateProcessor(original);
                    if (!TryRegisterPatchMethod(processor, method)) continue;

                    processor.Patch();
                    patched++;
                }
                catch (Exception ex)
                {
                    ModLogger.Warning($"[PatchInstaller] '{patchClass.Name}.{method.Name}' 개별 패치 실패: {ex.Message}");
                }
            }

            return patched;
        }

        /// <summary>
        /// <c>CreateClassProcessor(type).Patch()</c>가 던졌을 때의 복구입니다. Harmony는 클래스의 패치 메서드를 차례로 걸다가
        /// 중간에 던지므로, 앞쪽 메서드는 이미 걸려 있을 수 있습니다. 그대로 메서드별 재적용을 하면 같은 패치가 두 번 걸리므로
        /// 먼저 이 클래스가 건 패치를 모두 걷어냅니다. 걷어내기에 실패하면 이중 적용을 피하려고 재적용하지 않습니다(0 반환).
        /// 메서드가 하나뿐인 클래스는 같은 이유로 다시 실패하므로 재적용 결과도 0이 되고, 호출자가 실패로 기록합니다.
        /// </summary>
        private static int TryRecoverFailedClass(HarmonyLib.Harmony harmony, Type patchClass)
        {
            try
            {
                UnpatchClass(harmony, patchClass);
            }
            catch (Exception ex)
            {
                ModLogger.Warning($"[PatchInstaller] '{patchClass.Name}' 부분 적용분을 걷어내지 못해 메서드별 재적용을 건너뜁니다: {ex.GetType().Name}: {ex.Message}");
                return 0;
            }

            try
            {
                return ApplySurvivingMethodPatches(harmony, patchClass);
            }
            catch (Exception ex)
            {
                ModLogger.Warning($"[PatchInstaller] '{patchClass.Name}' 메서드별 재적용 실패: {ex.GetType().Name}: {ex.Message}");
                return 0;
            }
        }

        /// <summary>이 Harmony 인스턴스가 <paramref name="patchClass"/>의 메서드로 건 패치만 골라 걷어냅니다.</summary>
        private static void UnpatchClass(HarmonyLib.Harmony harmony, Type patchClass)
        {
            foreach (MethodBase original in harmony.GetPatchedMethods().ToList())
            {
                HarmonyLib.Patches info = HarmonyLib.Harmony.GetPatchInfo(original);
                if (info == null) continue;

                var mine = info.Prefixes.Concat(info.Postfixes).Concat(info.Transpilers).Concat(info.Finalizers)
                    .Where(p => p.owner == harmony.Id && p.PatchMethod != null && p.PatchMethod.DeclaringType == patchClass)
                    .Select(p => p.PatchMethod)
                    .ToList();
                foreach (MethodInfo patchMethod in mine)
                {
                    harmony.Unpatch(original, patchMethod);
                }
            }
        }

        private static bool TryRegisterPatchMethod(PatchProcessor processor, MethodInfo method)
        {
            var hm = new HarmonyMethod(method);
            if (method.Name == "Prefix" || method.IsDefined(typeof(HarmonyPrefix), true))
                processor.AddPrefix(hm);
            else if (method.Name == "Postfix" || method.IsDefined(typeof(HarmonyPostfix), true))
                processor.AddPostfix(hm);
            else if (method.Name == "Transpiler" || method.IsDefined(typeof(HarmonyTranspiler), true))
                processor.AddTranspiler(hm);
            else if (method.Name == "Finalizer" || method.IsDefined(typeof(HarmonyFinalizer), true))
                processor.AddFinalizer(hm);
            else
                return false;

            return true;
        }

        /// <summary>
        /// 패치 클래스의 패치 메서드(Prefix/Postfix/Finalizer/TargetMethod 등) 중
        /// 현재 로드된 게임 어셈블리로 JIT 되지 않는 것이 있으면 그 메서드와 예외를 돌려줍니다.
        /// 로그를 남기지 않는 순수 점검이라 게임 밖에서도 부를 수 있습니다.
        /// </summary>
        internal static bool TryFindUncompilablePatchMethod(Type patchClass, out MethodInfo method, out Exception error)
        {
            foreach (MethodInfo candidate in patchClass.GetMethods(
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.DeclaredOnly))
            {
                if (candidate.IsAbstract || candidate.ContainsGenericParameters) continue;
                if (!PatchHealthCheck.IsPatchMethod(candidate) && !PatchHealthCheck.IsDynamicTargetSelector(candidate)) continue;

                if (!TryPreparePatchMethod(candidate, out Exception ex))
                {
                    method = candidate;
                    error = ex;
                    return true;
                }
            }

            method = null;
            error = null;
            return false;
        }

        private static bool TryPreparePatchMethod(MethodInfo candidate, out Exception error)
        {
            try
            {
                RuntimeHelpers.PrepareMethod(candidate.MethodHandle);
                error = null;
                return true;
            }
            catch (Exception ex)
            {
                error = ex;
                return false;
            }
        }
    }
}
