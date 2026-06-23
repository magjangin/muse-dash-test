using HarmonyLib;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace muse_dash_test
{
    /// <summary>
    /// 모드 어셈블리의 모든 [HarmonyPatch] 대상이 현재 게임 빌드에서 실제로 해석되는지 시작 시 점검합니다.
    /// 게임 업데이트로 타입/메서드가 사라지면 어떤 패치가 비활성화될지 한눈에 요약 로그로 보여줍니다.
    /// (패치 적용 여부와 무관하게, AccessTools로 대상 존재만 독립적으로 검사하므로 호출 시점 제약이 없습니다.)
    /// </summary>
    public static class PatchHealthCheck
    {
        public static void Run()
        {
            try
            {
                var missing = new List<string>();
                int total = 0;

                foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
                {
                    var attrs = type.GetCustomAttributes(typeof(HarmonyPatch), true);
                    if (attrs == null || attrs.Length == 0) continue;

                    // 한 클래스에 여러 [HarmonyPatch]가 나뉘어 붙는 경우(타입/메서드 분리 지정)를 병합합니다.
                    Type declaringType = null;
                    string methodName = null;
                    Type[] argumentTypes = null;
                    MethodType methodType = MethodType.Normal;

                    foreach (HarmonyPatch attr in attrs)
                    {
                        var info = attr.info;
                        if (info == null) continue;
                        if (info.declaringType != null) declaringType = info.declaringType;
                        if (!string.IsNullOrEmpty(info.methodName)) methodName = info.methodName;
                        if (info.argumentTypes != null) argumentTypes = info.argumentTypes;
                        if (info.methodType.HasValue) methodType = info.methodType.Value;
                    }

                    if (declaringType == null) continue; // 대상 타입을 알 수 없는 패치는 점검 제외
                    total++;

                    if (!TargetExists(declaringType, methodName, argumentTypes, methodType))
                    {
                        missing.Add($"{type.Name} → {declaringType.Name}.{methodName ?? methodType.ToString()}");
                    }
                }

                if (missing.Count == 0)
                {
                    MelonLogger.Msg($"[PatchHealth] 패치 대상 {total}개 전부 정상 해석되었습니다.");
                }
                else
                {
                    MelonLogger.Warning($"[PatchHealth] {missing.Count}/{total}개 패치 대상이 현재 게임 빌드에서 해석되지 않았습니다(해당 기능 비활성 가능):");
                    foreach (var m in missing)
                    {
                        MelonLogger.Warning($"[PatchHealth]   - {m}");
                    }
                }

                // 추가: MusicTagManager.InitAlbumTagInfo 패치가 깨졌는지 여부 감지 및 Init 메서드 덤프 로직
                CheckMusicTagManagerPatchHealth();
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[PatchHealth] 패치 점검 중 예외: {ex}");
            }
        }

        private static void CheckMusicTagManagerPatchHealth()
        {
            try
            {
                var targetMethod = AccessTools.Method(typeof(Il2Cpp.MusicTagManager), "InitAlbumTagInfo");
                if (targetMethod == null)
                {
                    MelonLogger.Warning("[PatchHealth] MusicTagManager.InitAlbumTagInfo 메서드를 찾을 수 없습니다! Harmony 패치가 동작하지 않을 가능성이 높습니다. Init으로 시작하는 메서드를 찾아 덤프 파일을 생성합니다.");
                    DumpInitMethods();
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[PatchHealth] MusicTagManager 패치 타겟 검사 중 오류: {ex}");
            }
        }

        private static void DumpInitMethods()
        {
            try
            {
                string hwaPath = HwaResourceManager.HwaFolderPath;
                string dumpPath = System.IO.Path.Combine(hwaPath, "tag_manager_dump.txt");

                var methods = typeof(Il2Cpp.MusicTagManager).GetMethods(
                    System.Reflection.BindingFlags.Public | 
                    System.Reflection.BindingFlags.NonPublic | 
                    System.Reflection.BindingFlags.Instance | 
                    System.Reflection.BindingFlags.Static
                );

                int count = 0;
                using (var writer = new System.IO.StreamWriter(dumpPath, false, System.Text.Encoding.UTF8))
                {
                    writer.WriteLine($"=== MusicTagManager Methods starting with 'Init' ===");
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

                MelonLogger.Msg($"[PatchHealth] MusicTagManager의 'Init'로 시작하는 메서드 {count}개를 덤프했습니다: {dumpPath}");
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[PatchHealth] MusicTagManager 메서드 덤프 중 오류 발생: {ex}");
            }
        }

        private static bool TargetExists(Type declaringType, string methodName, Type[] argumentTypes, MethodType methodType)
        {
            try
            {
                switch (methodType)
                {
                    case MethodType.Constructor:
                        return AccessTools.Constructor(declaringType, argumentTypes) != null;
                    case MethodType.Getter:
                        return !string.IsNullOrEmpty(methodName) && AccessTools.PropertyGetter(declaringType, methodName) != null;
                    case MethodType.Setter:
                        return !string.IsNullOrEmpty(methodName) && AccessTools.PropertySetter(declaringType, methodName) != null;
                    default:
                        if (string.IsNullOrEmpty(methodName)) return false;
                        return (argumentTypes != null
                            ? AccessTools.Method(declaringType, methodName, argumentTypes)
                            : AccessTools.Method(declaringType, methodName)) != null;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
