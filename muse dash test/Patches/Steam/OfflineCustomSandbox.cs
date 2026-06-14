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

    public static class OfflineCustomSandbox
    {
        public static void Initialize()
        {
            MelonLogger.Msg("[OfflineSandbox] 개인 연구 및 오프라인 커스텀 샌드박스 초기화 시작...");

            // 덤프 파일 생성 경로 설정 (hwa 폴더 내의 md 파일)
            try
            {
                string hwaDir = Path.Combine(MelonLoader.Utils.MelonEnvironment.GameRootDirectory, "hwa");
                if (!Directory.Exists(hwaDir))
                {
                    Directory.CreateDirectory(hwaDir);
                    MelonLogger.Msg($"[OfflineSandbox] hwa 디렉토리가 없어 새로 생성했습니다: {hwaDir}");
                }
                string dumpPath = Path.Combine(hwaDir, "OfflineSandbox_DiscoveryDump.md");
                SandboxDumper.ExecuteDump(dumpPath);
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[OfflineSandbox] 덤프 프로세스 시작 실패: {ex.Message}");
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
                MelonLogger.Msg($"[OfflineSandbox.Dumper] 대상 경로: {outputPath}");

                var sb = new StringBuilder();
                sb.AppendLine("# 🧪 오프라인 샌드박스 패치용 API 디스커버리 덤프");
                sb.AppendLine();
                sb.AppendLine($"- **생성 일시**: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                sb.AppendLine("- **설명**: 이 파일은 뮤즈대시 2 등의 신작 모딩 시 훅 타겟 탐색을 돕기 위해 리플렉션으로 추출된 정보입니다.");
                sb.AppendLine();
                sb.AppendLine("---");
                sb.AppendLine();

                string[] keywords = { "steam", "dlc", "verify", "purchase", "license", "ownership", "install", "store", "authorize", "drm" };
                
                var assemblies = AppDomain.CurrentDomain.GetAssemblies();
                MelonLogger.Msg($"[OfflineSandbox.Dumper] 현재 로드된 총 {assemblies.Length}개의 어셈블리 스캔 시작...");

                int matchedTypesCount = 0;
                int matchedMethodsCount = 0;
                int processedAssemblies = 0;

                foreach (var assembly in assemblies)
                {
                    string asmName = assembly.GetName().Name;
                    // 게임 및 스팀 관련 바이너리만 타겟팅 (성능 및 노이즈 방지)
                    if (!asmName.StartsWith("Il2Cpp") && asmName != "Assembly-CSharp")
                        continue;

                    processedAssemblies++;
                    MelonLogger.Msg($"[OfflineSandbox.Dumper] 어셈블리 스캔 중: {asmName}");

                    Type[] types;
                    try
                    {
                        types = assembly.GetTypes();
                    }
                    catch (ReflectionTypeLoadException ex)
                    {
                        types = ex.Types;
                        MelonLogger.Warning($"[OfflineSandbox.Dumper] 어셈블리 {asmName} 로드 중 일부 타입 로드 경고 발생 (복구 스캔 진행)");
                    }
                    catch (Exception ex)
                    {
                        MelonLogger.Error($"[OfflineSandbox.Dumper] 어셈블리 {asmName} 스캔 실패: {ex.Message}");
                        continue;
                    }

                    if (types == null) continue;

                    var matchedTypesInAssembly = new List<(Type type, List<FieldInfo> fields, List<MethodInfo> methods)>();

                    foreach (var type in types)
                    {
                        if (type == null) continue;

                        string typeName = type.FullName ?? type.Name;
                        bool typeMatches = ContainsAnyKeyword(typeName, keywords);

                        var matchedFields = new List<FieldInfo>();
                        try
                        {
                            var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
                            foreach (var field in fields)
                            {
                                if (ContainsAnyKeyword(field.Name, keywords) || typeMatches)
                                {
                                    matchedFields.Add(field);
                                }
                            }
                        }
                        catch {}

                        var matchedMethods = new List<MethodInfo>();
                        try
                        {
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
                        catch {}

                        if (typeMatches || matchedFields.Count > 0 || matchedMethods.Count > 0)
                        {
                            matchedTypesInAssembly.Add((type, matchedFields, matchedMethods));
                        }
                    }

                    if (matchedTypesInAssembly.Count > 0)
                    {
                        sb.AppendLine($"## 📦 Assembly: `{asmName}`");
                        sb.AppendLine();

                        foreach (var item in matchedTypesInAssembly)
                        {
                            matchedTypesCount++;
                            string typeName = item.type.FullName ?? item.type.Name;
                            sb.AppendLine($"### 🔍 Class: `{typeName}`");
                            sb.AppendLine();

                            if (item.fields.Count > 0)
                            {
                                sb.AppendLine("#### 📋 Fields");
                                foreach (var field in item.fields)
                                {
                                    sb.AppendLine($"- `{field.FieldType.Name} {field.Name}`");
                                }
                                sb.AppendLine();
                            }

                            if (item.methods.Count > 0)
                            {
                                sb.AppendLine("#### ⚙️ Methods");
                                foreach (var method in item.methods)
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
                                    sb.AppendLine($"- `{modifier}{method.ReturnType.Name} {method.Name}({paramsSb})`");
                                }
                                sb.AppendLine();
                            }
                            sb.AppendLine("---");
                            sb.AppendLine();
                        }
                    }
                }

                File.WriteAllText(outputPath, sb.ToString(), Encoding.UTF8);
                MelonLogger.Msg($"[OfflineSandbox.Dumper] 스캔한 대상 어셈블리 개수: {processedAssemblies}개");
                MelonLogger.Msg($"[OfflineSandbox.Dumper] 덤프 완료! 발견된 클래스: {matchedTypesCount}개, 메서드: {matchedMethodsCount}개");
                MelonLogger.Msg($"[OfflineSandbox.Dumper] 저장 완료 경로: {outputPath}");
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
