using MelonLoader;
using System;
using Il2CppFormulaBase;
using Il2CppGameLogic;

namespace muse_dash_test
{
    // Il2CppFormulaBase.StageBattleComponent.LoadMusicData 하모니 패치
    [HarmonyLib.HarmonyPatch(typeof(StageBattleComponent), "LoadMusicData")]
    public class StageBattleComponent_LoadMusicData_Patch
    {
        public static void Postfix(StageBattleComponent __instance) { }
    }

    [HarmonyLib.HarmonyPatch(typeof(StageBattleComponent), "InitData")]
    public class StageBattleComponent_InitData_Patch
    {
        public static void Postfix(StageBattleComponent __instance)
        {
            string uid = CustomPlaySession.Current.SelectedMusicUid;
            if (string.IsNullOrEmpty(uid))
            {
                uid = PnlStagePatchHelper.GetCurrentSelectedMusicUid() ?? CustomPlaySession.Current.LastClickedMusicUid ?? "(unknown)";
            }
            MelonLogger.Msg($"StageBattleComponent.InitData 호출됨: {__instance}, 곡 UID={uid}");
        }
    }

    [HarmonyLib.HarmonyPatch(typeof(StageBattleComponent), "Load")]
    public class StageBattleComponent_Load_Patch
    {
        public static void Postfix(StageBattleComponent __instance)
        {
            try
            {
                MelonLogger.Msg($"[StageBattleComponent.Load] 호출됨: {__instance}");
                // 매 배틀 로드 시마다 미디어 주입 시작 상태 초기화 및 주입 실행
                HwaBattleMediaController.ResetState();
                HwaBattleMediaController.StartBattleMediaInjection();

                // APMod (All Perfect Mod) 폰트 탐색 상태 리셋
                Patches.VictoryDataCache.AttemptedFontCache = false;
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[StageBattleComponent.Load] 예외 발생: {ex}");
            }
        }
    }

    [HarmonyLib.HarmonyPatch(typeof(StageBattleComponent), "Pause")]
    public class StageBattleComponent_Pause_Patch
    {
        public static void Postfix(StageBattleComponent __instance, bool pauseCorountine)
        {
            MelonLogger.Msg("[StageBattleComponentPatch] StageBattleComponent.Pause 호출됨");
            HwaBattleMediaController.PauseMedia();
        }
    }

    [HarmonyLib.HarmonyPatch(typeof(StageBattleComponent), "Resume")]
    public class StageBattleComponent_Resume_Patch
    {
        public static void Postfix(StageBattleComponent __instance, bool isExit)
        {
            MelonLogger.Msg($"[StageBattleComponentPatch] StageBattleComponent.Resume 호출됨 (isExit={isExit})");
            HwaBattleMediaController.ResumeMedia(isExit);
        }
    }

    [HarmonyLib.HarmonyPatch(typeof(StageBattleComponent), "End")]
    public class StageBattleComponent_End_Patch
    {
        public static void Postfix(StageBattleComponent __instance)
        {
            MelonLogger.Msg("[StageBattleComponentPatch] StageBattleComponent.End 호출됨");
            HwaBattleMediaController.StopMedia();
        }
    }

    [HarmonyLib.HarmonyPatch(typeof(StageBattleComponent), "Exit")]
    public class StageBattleComponent_Exit_Patch
    {
        public static void Postfix(StageBattleComponent __instance)
        {
            MelonLogger.Msg("[StageBattleComponentPatch] StageBattleComponent.Exit 호출됨");
            HwaBattleMediaController.StopMedia();
        }
    }

    [HarmonyLib.HarmonyPatch(typeof(StageBattleComponent), "Release")]
    public class StageBattleComponent_Release_Patch
    {
        public static void Postfix(StageBattleComponent __instance)
        {
            MelonLogger.Msg("[StageBattleComponentPatch] StageBattleComponent.Release 호출됨");
            HwaBattleMediaController.StopMedia();
        }
    }

    [HarmonyLib.HarmonyPatch(typeof(StageBattleComponent), "GameRestart")]
    public class StageBattleComponent_GameRestart_Patch
    {
        public static void Postfix(StageBattleComponent __instance)
        {
            MelonLogger.Msg("[StageBattleComponentPatch] StageBattleComponent.GameRestart 호출됨");
            HwaBattleMediaController.StopMedia();
        }
    }
}
