using MelonLoader;
using HarmonyLib;
using Il2CppSteamworks;
using Il2Cpp;
using System.Collections.Generic;

namespace muse_dash_test
{
    [HarmonyPatch(typeof(SteamApps), nameof(SteamApps.BIsDlcInstalled))]
    public class DLCPatch
    {
        private static HashSet<uint> loggedDLCs = new HashSet<uint>();

        static bool Prefix(ref bool __result, AppId_t appID)
        {
            // 각 DLC ID는 처음 호출될 때만 로그 출력
            if (loggedDLCs.Add(appID.m_AppId))
            {
                MelonLogger.Msg($"DLC {appID.m_AppId}에 대한 IsDlcInstalled 호출됨");
            }

            __result = true;
            return false;
        }
    }

    [HarmonyPatch(typeof(SteamManager), nameof(SteamManager.DLCVerify))]
    public class DLCVerifyPatch
    {
        static bool Prefix(SteamManager __instance)
        {
            MelonLogger.Msg("DLCVerify 강제 실행");
            __instance.m_DoSomething1 = true;
            __instance.m_DoSomething3 = true;
            return true;
        }
    }

    public class DLCUnlock : MelonMod
    {
        public override void OnInitializeMelon()
        {
            LoggerInstance.Msg("DLC 언락 모드 로드됨");
        }
    }
}
