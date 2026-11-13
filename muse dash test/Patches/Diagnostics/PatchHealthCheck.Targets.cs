using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace muse_dash_test
{
    public static partial class PatchHealthCheck
    {
        /// <summary>점검 대상 한 건. Harmony가 클래스/메서드 어트리뷰트를 합쳐 계산하는 그 대상입니다.</summary>
        internal struct PatchTarget
        {
            public Type DeclaringType;
            public string MethodName;
            public Type[] ArgumentTypes;
            public MethodType MethodType;

            public string Describe()
            {
                string name = string.IsNullOrEmpty(MethodName)
                    ? MethodType.ToString()
                    : (MethodType == MethodType.Normal ? MethodName : $"{MethodName}[{MethodType}]");

                string args = string.Empty;
                if (ArgumentTypes != null && ArgumentTypes.Length > 0)
                {
                    var names = new string[ArgumentTypes.Length];
                    for (int i = 0; i < ArgumentTypes.Length; i++)
                    {
                        names[i] = ArgumentTypes[i]?.Name ?? "?";
                    }
                    args = "(" + string.Join(", ", names) + ")";
                }

                return $"{DeclaringType?.Name ?? "(타입 미상)"}.{name}{args}";
            }
        }

        /// <summary>어트리뷰트 없이 이름만으로도 Harmony가 패치 메서드로 인정하는 이름들입니다.</summary>
        private static readonly string[] PatchMethodNames =
        {
            "Prefix", "Postfix", "Transpiler", "Finalizer", "ReversePatch"
        };

        private static readonly Type[] PatchMethodAttributes =
        {
            typeof(HarmonyPrefix), typeof(HarmonyPostfix), typeof(HarmonyTranspiler),
            typeof(HarmonyFinalizer), typeof(HarmonyReversePatch)
        };

        /// <summary>
        /// 한 패치 클래스가 실제로 노리는 대상들을 열거합니다.
        ///
        /// <para><b>왜 클래스 레벨만 보면 안 되는가</b>: Harmony는 클래스 레벨 <c>[HarmonyPatch]</c>와
        /// <b>각 패치 메서드를 붙은</b> <c>[HarmonyPatch]</c>를 합쳐 대상을 정합니다.</para>
        /// </summary>
        internal static IEnumerable<PatchTarget> EnumerateTargets(Type patchClass, PatchTarget classTarget)
        {
            bool foundPatchMethod = false;

            foreach (var method in patchClass.GetMethods(
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.DeclaredOnly))
            {
                if (!IsPatchMethod(method)) continue;
                foundPatchMethod = true;

                var methodAttrs = method.GetCustomAttributes(typeof(HarmonyPatch), true);

                // 메서드 레벨 지정이 없으면 클래스 레벨이 곧 대상입니다(가장 흔한 형태).
                yield return (methodAttrs == null || methodAttrs.Length == 0)
                    ? classTarget
                    : Merge(classTarget, methodAttrs);
            }

            // 패치 메서드를 못 찾았으면 클래스 레벨 지정만으로 판단합니다.
            if (!foundPatchMethod)
            {
                yield return classTarget;
            }
        }

        /// <summary>Harmony가 패치 메서드로 취급하는 메서드인지 확인합니다. <see cref="PatchInstaller"/>도 이 기준을 씁니다.</summary>
        internal static bool IsPatchMethod(MethodInfo method)
        {
            for (int i = 0; i < PatchMethodNames.Length; i++)
            {
                if (string.Equals(method.Name, PatchMethodNames[i], StringComparison.Ordinal)) return true;
            }

            for (int i = 0; i < PatchMethodAttributes.Length; i++)
            {
                if (method.IsDefined(PatchMethodAttributes[i], true)) return true;
            }

            return false;
        }

        /// <summary>대상을 런타임에 계산하는 패치(TargetMethod/TargetMethods)인지 확인합니다.</summary>
        internal static bool HasDynamicTargets(Type patchClass)
        {
            foreach (var method in patchClass.GetMethods(
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.DeclaredOnly))
            {
                if (IsDynamicTargetSelector(method))
                {
                    return true;
                }
            }

            return false;
        }

        internal static bool IsDynamicTargetSelector(MethodInfo method)
        {
            return string.Equals(method.Name, "TargetMethod", StringComparison.Ordinal)
                || string.Equals(method.Name, "TargetMethods", StringComparison.Ordinal)
                || method.IsDefined(typeof(HarmonyTargetMethod), true)
                || method.IsDefined(typeof(HarmonyTargetMethods), true);
        }

        /// <summary>여러 [HarmonyPatch] 어트리뷰트를 기준 대상 위에 순서대로 덮어씁니다.</summary>
        internal static PatchTarget Merge(PatchTarget baseTarget, object[] attrs)
        {
            PatchTarget result = baseTarget;

            foreach (HarmonyPatch attr in attrs)
            {
                var info = attr?.info;
                if (info == null) continue;
                if (info.declaringType != null) result.DeclaringType = info.declaringType;
                if (!string.IsNullOrEmpty(info.methodName)) result.MethodName = info.methodName;
                if (info.argumentTypes != null) result.ArgumentTypes = info.argumentTypes;
                if (info.methodType.HasValue) result.MethodType = info.methodType.Value;
            }

            return result;
        }

        private static void CheckMusicTagManagerPatchHealth()
        {
            try
            {
                var targetMethod = AccessTools.Method(typeof(Il2Cpp.MusicTagManager), "InitAlbumTagInfo");
                if (targetMethod == null)
                {
                    ModLogger.Warning("[PatchHealth] MusicTagManager.InitAlbumTagInfo 메서드를 찾을 수 없습니다! Harmony 패치가 동작하지 않을 가능성이 높습니다. Init으로 시작하는 메서드를 찾아 덤프 파일을 생성합니다.");
                    DumpInitMethods();
                }
            }
            catch (Exception ex)
            {
                ModLogger.Error($"[PatchHealth] MusicTagManager 패치 타겟 검사 중 오류: {ex}");
            }
        }

        private static void DumpInitMethods()
        {
            try
            {
                string hwaPath = HwaResourceManager.HwaFolderPath;
                string dumpPath = Path.Combine(hwaPath, "tag_manager_dump.txt");

                var methods = typeof(Il2Cpp.MusicTagManager).GetMethods(
                    BindingFlags.Public |
                    BindingFlags.NonPublic |
                    BindingFlags.Instance |
                    BindingFlags.Static
                );

                int count = 0;
                using (var writer = new StreamWriter(dumpPath, false, Encoding.UTF8))
                {
                    writer.WriteLine("=== MusicTagManager Methods starting with 'Init' ===");
                    writer.WriteLine($"Generated at: {DateTime.Now}");
                    writer.WriteLine();

                    foreach (var m in methods)
                    {
                        if (m.Name.StartsWith("Init", StringComparison.OrdinalIgnoreCase))
                        {
                            count++;
                            var parameters = m.GetParameters();
                            var paramStrings = new List<string>();
                            foreach (var p in parameters)
                            {
                                paramStrings.Add($"{p.ParameterType.FullName} {p.Name}");
                            }
                            string paramList = string.Join(", ", paramStrings);
                            writer.WriteLine($"- {m.ReturnType.FullName} {m.Name}({paramList})");
                        }
                    }

                    writer.WriteLine();
                    writer.WriteLine($"Total methods found: {count}");
                }

                ModLogger.Msg($"[PatchHealth] MusicTagManager의 'Init'로 시작하는 메서드 {count}개를 덤프했습니다: {dumpPath}");
            }
            catch (Exception ex)
            {
                ModLogger.Error($"[PatchHealth] MusicTagManager 메서드 덤프 중 오류 발생: {ex}");
            }
        }

        internal static MethodBase ResolveTargetMethod(Type declaringType, string methodName, Type[] argumentTypes, MethodType methodType)
        {
            try
            {
                if (declaringType == null) return null;
                switch (methodType)
                {
                    case MethodType.Constructor:
                        return AccessTools.Constructor(declaringType, argumentTypes);
                    case MethodType.Getter:
                        return !string.IsNullOrEmpty(methodName) ? AccessTools.PropertyGetter(declaringType, methodName) : null;
                    case MethodType.Setter:
                        return !string.IsNullOrEmpty(methodName) ? AccessTools.PropertySetter(declaringType, methodName) : null;
                    default:
                        if (string.IsNullOrEmpty(methodName)) return null;
                        return argumentTypes != null
                            ? AccessTools.Method(declaringType, methodName, argumentTypes)
                            : AccessTools.Method(declaringType, methodName);
                }
            }
            catch
            {
                return null;
            }
        }

        private static bool TargetExists(Type declaringType, string methodName, Type[] argumentTypes, MethodType methodType)
        {
            return ResolveTargetMethod(declaringType, methodName, argumentTypes, methodType) != null;
        }
    }
}
