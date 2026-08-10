using MelonLoader;
using HarmonyLib;
using System;
using System.Reflection;
using System.Collections.Generic;

namespace muse_dash_test
{
    /// <summary>
    /// 콜라보 팩(명일방주, 미쿠, 린/렌 등) 및 특수 DLC의 소유권/만료 시각을 
    /// 오프라인 샌드박스 플래그(OfflineCustomSandbox.IsEnabled)에 맞춰 
    /// 오버라이드 및 바이패스하는 패치 모듈입니다.
    /// Harmony AccessTools.TypeByName의 전역 어셈블리 스캔 경고(노란줄)를 방지하기 위해 
    /// 타깃 어셈블리 direct lookup 방식으로 타입을 탐색합니다.
    /// </summary>
    public static class OfflineCollabSandbox
    {
        // ──────────────────────────────────────────────────────────────────────────
        // 안전한 Il2Cpp 타입 탐색 헬퍼 (Harmony 전역 스캔 경고/노란줄 방지)
        // ──────────────────────────────────────────────────────────────────────────
        private static readonly Dictionary<string, Type> TypeCache = new Dictionary<string, Type>();

        private static Type FindIl2CppType(params string[] candidateNames)
        {
            foreach (var name in candidateNames)
            {
                if (TypeCache.TryGetValue(name, out var cachedType))
                    return cachedType;

                // 1. AppDomain 어셈블리 direct lookup (Assembly-CSharp 관련 우선)
                foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
                {
                    string asmName = asm.FullName;
                    if (asmName.StartsWith("Assembly-CSharp") || asmName.StartsWith("Il2Cpp") || asmName.StartsWith("MelonLoader"))
                    {
                        try
                        {
                            Type type = asm.GetType(name, false);
                            if (type != null)
                            {
                                TypeCache[name] = type;
                                return type;
                            }
                        }
                        catch { }
                    }
                }
            }
            return null;
        }

        // ──────────────────────────────────────────────────────────────────────────
        // 1. DlcUIExtensionInfo - 콜라보 DLC 만료 시각(dlcEndTime) 2099년 오버라이드
        // ──────────────────────────────────────────────────────────────────────────
        public static class DlcUIExtensionInfoPatch
        {
            [HarmonyPatch]
            public class GetDlcEndTimePatch
            {
                static bool Prepare()
                {
                    return TargetMethod() != null;
                }

                static MethodBase TargetMethod()
                {
                    Type type = FindIl2CppType(
                        "Il2Cpp.DlcUIExtensionInfo",
                        "DlcUIExtensionInfo",
                        "Il2CppAssets.Scripts.Database.DlcUIExtensionInfo",
                        "Assets.Scripts.Database.DlcUIExtensionInfo"
                    );
                    if (type == null) return null;

                    return AccessTools.PropertyGetter(type, "dlcEndTime") 
                        ?? AccessTools.PropertyGetter(type, "getDlcEndTime")
                        ?? AccessTools.Method(type, "get_dlcEndTime")
                        ?? AccessTools.Method(type, "get_getDlcEndTime");
                }

                static bool Prefix(ref DateTime __result)
                {
                    if (!OfflineCustomSandbox.IsEnabled)
                        return true;

                    __result = new DateTime(2099, 12, 31, 23, 59, 59, DateTimeKind.Utc);
                    return false;
                }
            }
        }

        // ──────────────────────────────────────────────────────────────────────────
        // 2. SpecialDLCManager - 특수 DLC / 콜라보 획득 조건(IsFreeToGet) 강제 허용
        // ──────────────────────────────────────────────────────────────────────────
        public static class SpecialDLCManagerPatch
        {
            [HarmonyPatch]
            public class IsFreeToGetPatch
            {
                static bool Prepare()
                {
                    return TargetMethod() != null;
                }

                static MethodBase TargetMethod()
                {
                    Type type = FindIl2CppType(
                        "Il2Cpp.SpecialDLCManager",
                        "SpecialDLCManager",
                        "Il2CppAssets.Scripts.UI.Panels.SpecialDLCManager",
                        "Assets.Scripts.UI.Panels.SpecialDLCManager"
                    );
                    if (type == null) return null;

                    return AccessTools.Method(type, "IsFreeToGet");
                }

                static bool Prefix(ref bool __result)
                {
                    if (!OfflineCustomSandbox.IsEnabled)
                        return true;

                    MelonLogger.Msg("[OfflineCollab] SpecialDLCManager.IsFreeToGet -> 강제 true 허용");
                    __result = true;
                    return false;
                }
            }
        }

        // ──────────────────────────────────────────────────────────────────────────
        // 3. DLCInfoActiveTime 계열 - 카운트다운 타이머 & 락 UI 갱신 스킵
        // ──────────────────────────────────────────────────────────────────────────
        public static class DLCInfoActiveTimePatch
        {
            private static readonly string[][] CandidateTypeGroups = new string[][]
            {
                new string[] { "Il2Cpp.DLCInfoActiveTime", "DLCInfoActiveTime", "Il2CppAssets.Scripts.UI.Panels.DLCInfoActiveTime" },
                new string[] { "Il2Cpp.DLCInfoActiveTimeArknights", "DLCInfoActiveTimeArknights", "Il2CppAssets.Scripts.UI.Panels.DLCInfoActiveTimeArknights" },
                new string[] { "Il2Cpp.DLCInfoActiveTimeMiku", "DLCInfoActiveTimeMiku", "Il2CppAssets.Scripts.UI.Panels.DLCInfoActiveTimeMiku" },
                new string[] { "Il2Cpp.DLCInfoActiveTimeRinLen", "DLCInfoActiveTimeRinLen", "Il2CppAssets.Scripts.UI.Panels.DLCInfoActiveTimeRinLen" }
            };

            [HarmonyPatch]
            public class AutoRefreshCountdownPatch
            {
                static IEnumerable<MethodBase> TargetMethods()
                {
                    var targets = new List<MethodBase>();
                    foreach (var candidates in CandidateTypeGroups)
                    {
                        Type type = FindIl2CppType(candidates);
                        if (type != null)
                        {
                            var method = AccessTools.Method(type, "AutoRefreshCountdown");
                            if (method != null) targets.Add(method);
                        }
                    }
                    return targets;
                }

                static bool Prefix()
                {
                    if (!OfflineCustomSandbox.IsEnabled)
                        return true;

                    return false;
                }
            }
        }
    }
}
