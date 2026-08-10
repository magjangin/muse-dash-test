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
    /// </summary>
    public static class OfflineCollabSandbox
    {
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
                    // DlcUIExtensionInfo 또는 관련 타입 존재 여부 확인
                    return TargetMethod() != null;
                }

                static MethodBase TargetMethod()
                {
                    Type type = AccessTools.TypeByName("Il2Cpp.DlcUIExtensionInfo") 
                             ?? AccessTools.TypeByName("DlcUIExtensionInfo");
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

                    // 만료 시각을 먼 미래(2099-12-31)로 고정하여 만료 락 바이패스
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
                    Type type = AccessTools.TypeByName("Il2Cpp.SpecialDLCManager") 
                             ?? AccessTools.TypeByName("SpecialDLCManager");
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
            private static readonly string[] TargetTypeNames = new string[]
            {
                "Il2Cpp.DLCInfoActiveTime",
                "DLCInfoActiveTime",
                "Il2Cpp.DLCInfoActiveTimeArknights",
                "DLCInfoActiveTimeArknights",
                "Il2Cpp.DLCInfoActiveTimeMiku",
                "DLCInfoActiveTimeMiku",
                "Il2Cpp.DLCInfoActiveTimeRinLen",
                "DLCInfoActiveTimeRinLen"
            };

            [HarmonyPatch]
            public class AutoRefreshCountdownPatch
            {
                static IEnumerable<MethodBase> TargetMethods()
                {
                    var targets = new List<MethodBase>();
                    foreach (var name in TargetTypeNames)
                    {
                        Type type = AccessTools.TypeByName(name);
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

                    // 만료 카운트다운 계산 및 락 UI 갱신 스킵
                    return false;
                }
            }
        }
    }
}
