using HarmonyLib;
using MelonLoader;
using System;
using System.Collections.Generic;
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
            var skipped = new List<string>();
            var failed = new List<string>();

            foreach (Type type in assembly.GetValidTypes())
            {
                try
                {
                    bool isPatchClass = type.IsDefined(typeof(HarmonyAttribute), true);
                    if (!isPatchClass) continue;

                    if (TryFindUncompilablePatchMethod(type, out MethodInfo broken, out Exception jitError))
                    {
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
                    failed.Add(type.Name);
                    ModLogger.Error($"[PatchInstaller] '{type.FullName}' 패치 적용 실패: {ex}");
                }
            }

            if (skipped.Count == 0 && failed.Count == 0)
            {
                ModLogger.Msg($"[PatchInstaller] 패치 클래스 {applied}개를 적용했습니다.");
                return;
            }

            ModLogger.Warning(
                $"[PatchInstaller] 패치 클래스 {applied}개 적용, {skipped.Count}개 제외(대상 미존재/JIT 실패), {failed.Count}개 적용 실패. " +
                "제외·실패한 기능만 꺼지고 게임 원본 동작은 그대로입니다.");
            if (skipped.Count > 0) ModLogger.Warning($"[PatchInstaller]   제외: {string.Join(", ", skipped)}");
            if (failed.Count > 0) ModLogger.Warning($"[PatchInstaller]   실패: {string.Join(", ", failed)}");
        }

        /// <summary>
        /// 다중 메서드 패치 클래스에서 일부 대상 메서드만 사라졌을 때, 여전히 존재하는 대상 메서드들에만
        /// 개별적으로 Prefix/Postfix/Transpiler/Finalizer를 겁니다.
        /// </summary>
        private static int ApplySurvivingMethodPatches(HarmonyLib.Harmony harmony, Type patchClass)
        {
            int patched = 0;
            var classAttrs = patchClass.GetCustomAttributes(typeof(HarmonyPatch), true);
            var classTarget = PatchHealthCheck.Merge(default, classAttrs);

            foreach (MethodInfo method in patchClass.GetMethods(
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.DeclaredOnly))
            {
                if (!PatchHealthCheck.IsPatchMethod(method)) continue;

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
                        continue;

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

                try
                {
                    RuntimeHelpers.PrepareMethod(candidate.MethodHandle);
                }
                catch (Exception ex)
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
    }
}
