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
            // 덤프 파일 생성 경로 설정 (hwa 폴더 내의 md 파일)
            try
            {
                string hwaDir = Path.Combine(MelonLoader.Utils.MelonEnvironment.GameRootDirectory, "hwa");
                if (!Directory.Exists(hwaDir))
                {
                    Directory.CreateDirectory(hwaDir);
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
                var sb = new StringBuilder();
                sb.AppendLine("# 🧪 오프라인 샌드박스 패치용 API 디스커버리 덤프");
                sb.AppendLine();
                sb.AppendLine($"- **생성 일시**: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                sb.AppendLine("- **설명**: 이 파일은 Steamworks.NET 라이브러리(Il2Cppcom.rlabrecque.steamworks.net)의 훅 타겟 탐색을 위해 추출된 정보입니다.");
                sb.AppendLine();
                sb.AppendLine("---");
                sb.AppendLine();

                var assemblies = AppDomain.CurrentDomain.GetAssemblies();
                int matchedTypesCount = 0;
                int matchedMethodsCount = 0;

                foreach (var assembly in assemblies)
                {
                    string asmName = assembly.GetName().Name;
                    
                    // 오직 Il2Cppcom.rlabrecque.steamworks.net 어셈블리만 스캔합니다.
                    if (asmName != "Il2Cppcom.rlabrecque.steamworks.net")
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

                    sb.AppendLine($"## 📦 Assembly: `{asmName}`");
                    sb.AppendLine();

                    foreach (var type in types)
                    {
                        if (type == null) continue;

                        string typeName = type.FullName ?? type.Name;
                        matchedTypesCount++;

                        sb.AppendLine($"### 🔍 Class: `{typeName}`");
                        sb.AppendLine();

                        // 필드 정보 스캔
                        try
                        {
                            var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
                            if (fields.Length > 0)
                            {
                                sb.AppendLine("#### 📋 Fields");
                                foreach (var field in fields)
                                {
                                    sb.AppendLine($"- `{field.FieldType.Name} {field.Name}`");
                                }
                                sb.AppendLine();
                            }
                        }
                        catch {}

                        // 메서드 정보 스캔
                        try
                        {
                            var methods = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly);
                            if (methods.Length > 0)
                            {
                                sb.AppendLine("#### ⚙️ Methods");
                                foreach (var method in methods)
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
                        }
                        catch {}
                        
                        sb.AppendLine("---");
                        sb.AppendLine();
                    }
                }

                File.WriteAllText(outputPath, sb.ToString(), Encoding.UTF8);
                MelonLogger.Msg($"[OfflineSandbox] 덤프 완료! 발견된 클래스: {matchedTypesCount}개, 메서드: {matchedMethodsCount}개");
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[OfflineSandbox] 분석 덤프 도중 오류 발생: {ex}");
            }
        }
    }
}
