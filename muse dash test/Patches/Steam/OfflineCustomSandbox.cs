using MelonLoader;
using HarmonyLib;
using Il2CppSteamworks;
using Il2Cpp;
using System;
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
            MelonLogger.Msg("[OfflineSandbox] 개인 연구 및 오프라인 커스텀 샌드박스 초기화 완료");
        }
    }
}
