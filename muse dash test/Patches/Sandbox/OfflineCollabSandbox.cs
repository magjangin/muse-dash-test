using MelonLoader;
using HarmonyLib;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using Il2CppAssets.Scripts.Database;

namespace muse_dash_test
{
    /// <summary>
    /// 콜라보 팩(명일방주, 미쿠, 린/렌 등) 및 특수 DLC의 소유권/만료 시각을 
    /// 오프라인 샌드박스 플래그(OfflineCustomSandbox.IsEnabled)에 맞춰 
    /// 오버라이드 및 바이패스하는 패치 모듈입니다.
    /// 
    /// DlcUIExtensionInfo.get_dlcEndTime()은 Il2Cpp Field Accessor이므로 direct patch 시 
    /// [WARNING] field accessor, it can't be patched 경고가 발생합니다.
    /// 따라서 DBConfigDlcUIExtension.Deserialize Postfix 시점에 list의 모든 DlcUIExtensionInfo.dlcEndTime 
    /// 필드를 2099년으로 직접 데이터 변경(Data Mutation)하여 경고를 100% 제거하고 완벽 지원합니다.
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
        // 1. DBConfigDlcUIExtension - 콜라보 DLC 만료 시각(dlcEndTime) 2099년 데이터 오버라이드
        //    (Field Accessor direct patch 경고 원인 100% 방지)
        // ──────────────────────────────────────────────────────────────────────────
        [HarmonyPatch(typeof(DBConfigDlcUIExtension), nameof(DBConfigDlcUIExtension.Deserialize))]
        public static class DBConfigDlcUIExtensionPatch
        {
            static void Postfix(DBConfigDlcUIExtension __instance)
            {
                if (!OfflineCustomSandbox.IsEnabled)
                    return;

                try
                {
                    var list = __instance.list;
                    if (list == null) return;

                    var futureTime = new Il2CppSystem.DateTime(2099, 12, 31, 23, 59, 59);
                    int updatedCount = 0;

                    for (int i = 0; i < list.Count; i++)
                    {
                        var info = list[i];
                        if (info != null)
                        {
                            info.dlcEndTime = futureTime;
                            updatedCount++;
                        }
                    }

                    MelonLogger.Msg($"[OfflineCollab] DBConfigDlcUIExtension {updatedCount}개 콜라보 팩 만료 시각 -> 2099-12-31 고정 완료 (Field Accessor 경고 소멸)");
                }
                catch (Exception ex)
                {
                    MelonLogger.Error($"[OfflineCollab] DBConfigDlcUIExtension 데이터 패치 실패: {ex.Message}");
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
