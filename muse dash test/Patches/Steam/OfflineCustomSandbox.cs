using MelonLoader;
using HarmonyLib;
using Il2CppSteamworks;
using Il2Cpp;
using System;
using System.IO;
using System.Text;
using System.Reflection;
using System.Collections.Generic;

namespace muse_dash_test
{
    [HarmonyPatch(typeof(SteamApps), nameof(SteamApps.BIsDlcInstalled))]
    public class OfflineCustomSandboxPatch
    {
        private static HashSet<uint> loggedDLCs = new HashSet<uint>();

        static bool Prefix(ref bool __result, AppId_t appID)
        {
            // 개인 연구 및 오프라인 커스텀 테스트 환경을 위한 DLC 가상 인스턴스 확인
            if (loggedDLCs.Add(appID.m_AppId))
            {
                MelonLogger.Msg($"[OfflineSandbox] 오프라인 샌드박스 DLC {appID.m_AppId} 확인됨");
            }

            __result = true;
            return false;
        }
    }

    [HarmonyPatch(typeof(SteamManager), nameof(SteamManager.DLCVerify))]
    public class OfflineVerifyPatch
    {
        static bool Prefix(SteamManager __instance)
        {
            MelonLogger.Msg("[OfflineSandbox] 오프라인 커스텀 환경 검증 바이패스");
            __instance.m_DoSomething1 = true;
            __instance.m_DoSomething3 = true;
            return true;
        }
    }

    public class OfflineCustomSandbox : MelonMod
    {
        public override void OnInitializeMelon()
        {
            LoggerInstance.Msg("[OfflineSandbox] 개인 연구 및 오프라인 커스텀 샌드박스 로드 완료");

            // 덤프 파일 생성 경로 설정 (게임 설치 경로 루트)
            try
            {
                string dumpPath = Path.Combine(MelonLoader.Utils.MelonEnvironment.GameRootDirectory, "OfflineSandbox_DiscoveryDump.txt");
                SandboxDumper.ExecuteDump(dumpPath);
            }
            catch (Exception ex)
            {
                LoggerInstance.Error($"[OfflineSandbox] 덤프 프로세스 시작 실패: {ex.Message}");
            }
        }
    }

    public static class SandboxDumper
    {
        public static void ExecuteDump(string outputPath)
        {
            try
            {
                MelonLogger.Msg("[OfflineSandbox.Dumper] 오프라인 샌드박스 분석용 디스커버리 덤프 시작...");
                var sb = new StringBuilder();
                sb.AppendLine("==========================================================================");
                sb.AppendLine("   오프라인 샌드박스 패치용 스팀/DLC/인증 관련 API 디스커버리 덤프");
                sb.AppendLine("   생성 일시: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                sb.AppendLine("   이 파일은 뮤즈대시 2 등의 신작 모딩 시 훅 타겟 탐색을 돕기 위해 생성되었습니다.");
                sb.AppendLine("==========================================================================");
                sb.AppendLine();

                string[] keywords = { "steam", "dlc", "verify", "purchase", "license", "ownership", "install", "store", "authorize", "drm" };
                
                var assemblies = AppDomain.CurrentDomain.GetAssemblies();
                int matchedTypesCount = 0;
                int matchedMethodsCount = 0;

                foreach (var assembly in assemblies)
                {
                    string asmName = assembly.GetName().Name;
                    // 게임 및 스팀 관련 바이너리만 타겟팅 (성능 및 노이즈 방지)
                    if (!asmName.StartsWith("Il2Cpp") && asmName != "Assembly-CSharp")
                        continue;

                    Type[] types;
                    try
                    {
                        types = assembly.GetTypes();
                    }
                    catch (ReflectionTypeLoadException ex)
                    {
                        types = ex.Types;
                    }
                    catch
                    {
                        continue;
                    }

                    if (types == null) continue;

                    foreach (var type in types)
                    {
                        if (type == null) continue;

                        string typeName = type.FullName ?? type.Name;
                        bool typeMatches = ContainsAnyKeyword(typeName, keywords);

                        var matchedMethods = new List<MethodInfo>();
                        
                        try
                        {
                            // 타입 내부의 모든 메서드 스캔
                            var methods = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly);
                            foreach (var method in methods)
                            {
                                if (method == null) continue;
                                if (typeMatches || ContainsAnyKeyword(method.Name, keywords))
                                {
                                    matchedMethods.Add(method);
                                }
                            }
                        }
                        catch
                        {
                            // 일부 난독화되거나 네이티브 결합된 메서드는 리플렉션 오류가 날 수 있음
                        }

                        if (typeMatches || matchedMethods.Count > 0)
                        {
                            matchedTypesCount++;
                            sb.AppendLine($"[Class] {typeName} (Assembly: {asmName})");

                            // 필드 정보 스캔
                            try
                            {
                                var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
                                foreach (var field in fields)
                                {
                                    if (ContainsAnyKeyword(field.Name, keywords) || typeMatches)
                                    {
                                        sb.AppendLine($"  [Field] {field.FieldType.Name} {field.Name}");
                                    }
                                }
                            }
                            catch {}

                            // 메서드 정보 스캔
                            foreach (var method in matchedMethods)
                            {
                                matchedMethodsCount++;
                                var paramsSb = new StringBuilder();
                                var parameters = method.GetParameters();
                                for (int i = 0; i < parameters.Length; i++)
                                {
                                    paramsSb.Append($"{parameters[i].ParameterType.Name} {parameters[i].Name}");
                                    if (i < parameters.Length - 1) paramsSb.Append(", ");
                                }

                                string modifier = method.IsStatic ? "static " : "";
                                sb.AppendLine($"  [Method] {modifier}{method.ReturnType.Name} {method.Name}({paramsSb.ToString()})");
                            }
                            sb.AppendLine();
                        }
                    }
                }

                File.WriteAllText(outputPath, sb.ToString(), Encoding.UTF8);
                MelonLogger.Msg($"[OfflineSandbox.Dumper] 덤프 완료! 발견된 클래스: {matchedTypesCount}개, 메서드: {matchedMethodsCount}개");
                MelonLogger.Msg($"[OfflineSandbox.Dumper] 저장 경로: {outputPath}");
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[OfflineSandbox.Dumper] 분석 덤프 도중 오류 발생: {ex}");
            }
        }

        private static bool ContainsAnyKeyword(string value, string[] keywords)
        {
            if (string.IsNullOrEmpty(value)) return false;
            foreach (var kw in keywords)
            {
                if (value.IndexOf(kw, StringComparison.OrdinalIgnoreCase) >= 0)
                    return true;
            }
            return false;
        }
    }
}
