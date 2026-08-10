using MelonLoader;
using HarmonyLib;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

namespace muse_dash_test
{
    /// <summary>
    /// 콜라보 팩(명일방주, 미쿠, 린/렌 등) 및 특수 DLC의 소유권/만료 시각을 
    /// 오프라인 샌드박스 플래그(OfflineCustomSandbox.IsEnabled)에 맞춰 
    /// 오버라이드 및 바이패스하는 패치 모듈입니다.
    /// 
    /// - C# System.DateTime 대신 Il2CppSystem.DateTime 호환 타입 사용
    /// - TargetMethods가 없을 경우 Harmony PatchAll 불발을 방지하는 Prepare 검증 적용
    /// </summary>
    public static class OfflineCollabSandbox
    {
        // ──────────────────────────────────────────────────────────────────────────
        // 안전한 Il2Cpp 타입 탐색 헬퍼
        // ──────────────────────────────────────────────────────────────────────────
        private static readonly Dictionary<string, Type> TypeCache = new Dictionary<string, Type>();

        private static Type FindIl2CppType(params string[] candidateNames)
        {
            foreach (var name in candidateNames)
            {
                if (TypeCache.TryGetValue(name, out var cachedType))
                    return cachedType;

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
                        "Il2CppAssets.Scripts.Database.DlcUIExtensionInfo",
                        "Assets.Scripts.Database.DlcUIExtensionInfo",
                        "Il2Cpp.DlcUIExtensionInfo",
                        "DlcUIExtensionInfo"
                    );
                    if (type == null) return null;

                    return AccessTools.PropertyGetter(type, "dlcEndTime") 
                        ?? AccessTools.PropertyGetter(type, "getDlcEndTime")
                        ?? AccessTools.Method(type, "get_dlcEndTime")
                        ?? AccessTools.Method(type, "get_getDlcEndTime");
                }

                // C# System.DateTime 대신 Il2Cpp 런타임 호환 Il2CppSystem.DateTime 사용
                static bool Prefix(ref Il2CppSystem.DateTime __result)
                {
                    if (!OfflineCustomSandbox.IsEnabled)
                        return true;

                    // 2099-12-31 23:59:59 Il2CppSystem.DateTime 객체 생성 및 리턴
                    __result = new Il2CppSystem.DateTime(2099, 12, 31, 23, 59, 59);
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
                        "Il2CppAssets.Scripts.UI.Panels.SpecialDLCManager",
                        "Assets.Scripts.UI.Panels.SpecialDLCManager",
                        "Il2Cpp.SpecialDLCManager",
                        "SpecialDLCManager"
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
                new string[] { "Il2CppAssets.Scripts.UI.Panels.DLCInfoActiveTime", "Assets.Scripts.UI.Panels.DLCInfoActiveTime", "Il2Cpp.DLCInfoActiveTime", "DLCInfoActiveTime" },
                new string[] { "Il2CppAssets.Scripts.UI.Panels.DLCInfoActiveTimeArknights", "Assets.Scripts.UI.Panels.DLCInfoActiveTimeArknights", "Il2Cpp.DLCInfoActiveTimeArknights", "DLCInfoActiveTimeArknights" },
                new string[] { "Il2CppAssets.Scripts.UI.Panels.DLCInfoActiveTimeMiku", "Assets.Scripts.UI.Panels.DLCInfoActiveTimeMiku", "Il2Cpp.DLCInfoActiveTimeMiku", "DLCInfoActiveTimeMiku" },
                new string[] { "Il2CppAssets.Scripts.UI.Panels.DLCInfoActiveTimeRinLen", "Assets.Scripts.UI.Panels.DLCInfoActiveTimeRinLen", "Il2Cpp.DLCInfoActiveTimeRinLen", "DLCInfoActiveTimeRinLen" }
            };

            [HarmonyPatch]
            public class AutoRefreshCountdownPatch
            {
                static bool Prepare()
                {
                    // TargetMethods()에서 찾은 대상 메서드가 최소 1개 이상일 때만 패치를 활성화 (Undefined target method 에러 방지)
                    var methods = GetResolvedMethods();
                    return methods != null && methods.Count > 0;
                }

                private static List<MethodBase> GetResolvedMethods()
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

                static IEnumerable<MethodBase> TargetMethods()
                {
                    return GetResolvedMethods();
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
