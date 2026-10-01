using MelonLoader;
using System;
using Il2CppFormulaBase;

namespace muse_dash_test
{
    // Il2CppFormulaBase.StageBattleComponent.LoadMusicData 하모니 패치
    // 호출 시점만 로그로 남기는 관찰 전용 패치다.
    // (예전에는 여기서 부르는 MusicData 상세 덤프 StageBattleMusicDataDump가 주석으로 꺼진 채 남아 있었다.
    //  호출하는 곳이 없어 지웠다. StageBattleComponent의 필드·프로퍼티를 리플렉션으로 전부 훑는 진단이라
    //  CHECKLIST의 "IL2CPP 객체를 리플렉션으로 깊게 훑는 진단" 항목에 해당한다. 필요하면 커밋 e896584의
    //  이 파일에서 되살릴 수 있다.)
    [HarmonyLib.HarmonyPatch(typeof(StageBattleComponent), "LoadMusicData")]
    public class StageBattleComponent_LoadMusicData_Patch
    {
        public static void Postfix(StageBattleComponent __instance)
        {
            try
            {
                ModLogger.Msg($"[StageBattleComponent.LoadMusicData] 호출됨: {__instance}");
            }
            catch (Exception ex)
            {
                ModLogger.Error($"[StageBattleComponent.LoadMusicData] Postfix 예외: {ex}");
            }
        }
    }

    [HarmonyLib.HarmonyPatch(typeof(StageBattleComponent), "InitData")]
    public class StageBattleComponent_InitData_Patch
    {
        public static void Postfix(StageBattleComponent __instance)
        {
            string uid = CustomPlaySession.Current.LastKnownMusicUid;
            ModLogger.Msg($"StageBattleComponent.InitData 호출됨: {__instance}, 곡 UID={uid}");
        }
    }

    [HarmonyLib.HarmonyPatch(typeof(StageBattleComponent), "Load")]
    public class StageBattleComponent_Load_Patch
    {
        public static void Postfix(StageBattleComponent __instance)
        {
            try
            {
                ModLogger.Msg($"[StageBattleComponent.Load] 호출됨: {__instance}");
                // 매 배틀 로드 시마다 미디어 주입 시작 상태 초기화 및 주입 실행
                HwaBattleMediaController.ResetState();
                HwaBattleMediaController.StartBattleMediaInjection();

                // APMod (All Perfect Mod) 세션 상태 리셋 (ActiveTarget, FontCache 초기화)
                // 이번 배틀의 ActiveTarget은 AddScore/GetAccuracy/IsFullCombo 훅이 다시 채웁니다.
                Patches.VictoryDataCache.ResetSession();
                // 클립 포인터는 에셋이 해제된 뒤 재사용될 수 있으므로 배틀마다 FC 효과음 판정 캐시를 버립니다.
                Patches.AllPerfectSound.ResetClipCache();
            }
            catch (Exception ex)
            {
                ModLogger.Error($"[StageBattleComponent.Load] 예외 발생: {ex}");
            }
        }
    }

    [HarmonyLib.HarmonyPatch(typeof(StageBattleComponent), "Pause")]
    public class StageBattleComponent_Pause_Patch
    {
        public static void Postfix(StageBattleComponent __instance, bool pauseCorountine)
        {
            ModLogger.Msg("[StageBattleComponentPatch] StageBattleComponent.Pause 호출됨");
            HwaBattleMediaController.PauseMedia();
        }
    }

    [HarmonyLib.HarmonyPatch(typeof(StageBattleComponent), "Resume")]
    public class StageBattleComponent_Resume_Patch
    {
        public static void Postfix(StageBattleComponent __instance, bool isExit)
        {
            ModLogger.Msg($"[StageBattleComponentPatch] StageBattleComponent.Resume 호출됨 (isExit={isExit})");
            HwaBattleMediaController.ResumeMedia(isExit);
        }
    }

    [HarmonyLib.HarmonyPatch(typeof(StageBattleComponent), "End")]
    public class StageBattleComponent_End_Patch
    {
        public static void Postfix(StageBattleComponent __instance)
        {
            ModLogger.Msg("[StageBattleComponentPatch] StageBattleComponent.End 호출됨");
            muse_dash_test.Patches.VictoryFlowGuard.MarkCompleted();
            HwaBattleMediaController.StopMedia();
        }
    }

    [HarmonyLib.HarmonyPatch(typeof(StageBattleComponent), "Exit")]
    public class StageBattleComponent_Exit_Patch
    {
        public static void Postfix(StageBattleComponent __instance)
        {
            ModLogger.Msg("[StageBattleComponentPatch] StageBattleComponent.Exit 호출됨");
            muse_dash_test.Patches.VictoryFlowGuard.MarkCompleted();
            // 리셋을 먼저 수행해 StopMedia가 예외를 던져도 stale 플래그가 남지 않게 합니다.
            try { CustomPlaySession.Current.ResetApplyDecision(); }
            catch (Exception ex) { ModLogger.Error($"[StageBattleComponentPatch] Exit ResetApplyDecision 예외: {ex}"); }
            try { HwaBattleMediaController.StopMedia(); }
            catch (Exception ex) { ModLogger.Error($"[StageBattleComponentPatch] Exit StopMedia 예외: {ex}"); }
        }
    }

    [HarmonyLib.HarmonyPatch(typeof(StageBattleComponent), "Release")]
    public class StageBattleComponent_Release_Patch
    {
        public static void Postfix(StageBattleComponent __instance)
        {
            ModLogger.Msg("[StageBattleComponentPatch] StageBattleComponent.Release 호출됨");
            muse_dash_test.Patches.VictoryFlowGuard.MarkCompleted();
            try { CustomPlaySession.Current.ResetApplyDecision(); }
            catch (Exception ex) { ModLogger.Error($"[StageBattleComponentPatch] Release ResetApplyDecision 예외: {ex}"); }
            try { HwaBattleMediaController.StopMedia(); }
            catch (Exception ex) { ModLogger.Error($"[StageBattleComponentPatch] Release StopMedia 예외: {ex}"); }
        }
    }

    [HarmonyLib.HarmonyPatch(typeof(StageBattleComponent), "GameRestart")]
    public class StageBattleComponent_GameRestart_Patch
    {
        public static void Postfix(StageBattleComponent __instance)
        {
            ModLogger.Msg("[StageBattleComponentPatch] StageBattleComponent.GameRestart 호출됨");
            muse_dash_test.Patches.VictoryFlowGuard.MarkCompleted();
            try { CustomPlaySession.Current.ResetApplyDecision(); }
            catch (Exception ex) { ModLogger.Error($"[StageBattleComponentPatch] GameRestart ResetApplyDecision 예외: {ex}"); }
            try { HwaBattleMediaController.StopMedia(); }
            catch (Exception ex) { ModLogger.Error($"[StageBattleComponentPatch] GameRestart StopMedia 예외: {ex}"); }
        }
    }
}
