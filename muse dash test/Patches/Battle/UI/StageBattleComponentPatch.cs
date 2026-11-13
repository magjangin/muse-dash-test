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
    [HarmonyLib.HarmonyPatch(typeof(StageBattleComponent), nameof(StageBattleComponent.LoadMusicData))]
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

    // 숙주 곡에 없는 난이도를 고르면 게임이 없는 차트 에셋을 불러오다 멈춥니다(BmsHostMaps 참고).
    // 숙주 차트는 틀로만 쓰므로 있는 난이도의 에셋을 대신 불러오게 하고, 난이도 번호 자체는 그대로 둡니다.
    [HarmonyLib.HarmonyPatch(typeof(StageBattleComponent), nameof(StageBattleComponent.GetMusicDataFromStageInfo))]
    public class StageBattleComponent_GetMusicDataFromStageInfo_Patch
    {
        public static void Prefix()
        {
            try
            {
                string uid = CustomPlaySession.Current.LastKnownMusicUid;
                if (!CustomContentIds.IsVirtualSong(uid)) return;

                var stage = Il2CppAssets.Scripts.Database.GlobalDataBase.s_DbBattleStage;
                if (stage == null) return;

                int difficulty = stage.selectedDifficulty;
                int map = HwaResourceManager.ResolveHostMap(uid, difficulty);
                if (map == difficulty) return;

                string from = stage.m_StageMapName;
                if (string.IsNullOrEmpty(from) || !from.EndsWith(difficulty.ToString(System.Globalization.CultureInfo.InvariantCulture), StringComparison.Ordinal))
                {
                    ModLogger.Warning($"[StageBattleComponent.GetMusicDataFromStageInfo] 차트 에셋 이름이 예상과 달라 그대로 둡니다: uid={uid}, difficulty={difficulty}, name={from ?? "(null)"}");
                    return;
                }

                string to = from.Substring(0, from.Length - difficulty.ToString(System.Globalization.CultureInfo.InvariantCulture).Length)
                    + map.ToString(System.Globalization.CultureInfo.InvariantCulture);
                stage.m_StageMapName = to;
                ModLogger.Msg($"[StageBattleComponent.GetMusicDataFromStageInfo] 숙주에 없는 난이도라 다른 차트 에셋을 틀로 씁니다: uid={uid}, difficulty={difficulty}, {from} -> {to}");
            }
            catch (Exception ex)
            {
                ModLogger.Error($"[StageBattleComponent.GetMusicDataFromStageInfo] Prefix 예외: {ex}");
            }
        }
    }

    [HarmonyLib.HarmonyPatch(typeof(StageBattleComponent), nameof(StageBattleComponent.InitData))]
    public class StageBattleComponent_InitData_Patch
    {
        public static void Postfix(StageBattleComponent __instance)
        {
            try
            {
                string uid = CustomPlaySession.Current.LastKnownMusicUid;
                ModLogger.Msg($"StageBattleComponent.InitData 호출됨: {__instance}, 곡 UID={uid}");
            }
            catch (Exception ex)
            {
                ModLogger.Error($"[StageBattleComponent.InitData] Postfix 예외: {ex}");
            }
        }
    }

    [HarmonyLib.HarmonyPatch(typeof(StageBattleComponent), nameof(StageBattleComponent.Load))]
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

                // 사전 판정 테이블(TimeNodeOrders) 올 퍼펙트 조작 및 활성 컴포넌트 등록
                ForcePerfectState.SetActiveStageBattle(__instance);
                if (ModConfig.EnableForcePerfect && ForcePerfectState.Enabled)
                {
                    ForcePerfectState.PromoteTimeNodeOrders(__instance);
                }
            }
            catch (Exception ex)
            {
                ModLogger.Error($"[StageBattleComponent.Load] 예외 발생: {ex}");
            }
        }
    }

    [HarmonyLib.HarmonyPatch(typeof(StageBattleComponent), nameof(StageBattleComponent.Pause))]
    public class StageBattleComponent_Pause_Patch
    {
        public static void Postfix(StageBattleComponent __instance, bool pauseCorountine)
        {
            // 게임의 일시정지 흐름 안입니다. 여기서 던지면 게임 쪽 Pause 처리까지 끊기므로 반드시 삼킵니다.
            try
            {
                ModLogger.Msg("[StageBattleComponentPatch] StageBattleComponent.Pause 호출됨");
                HwaBattleMediaController.PauseMedia();
            }
            catch (Exception ex)
            {
                ModLogger.Error($"[StageBattleComponentPatch] Pause Postfix 예외: {ex}");
            }
        }
    }

    [HarmonyLib.HarmonyPatch(typeof(StageBattleComponent), nameof(StageBattleComponent.Resume))]
    public class StageBattleComponent_Resume_Patch
    {
        public static void Postfix(StageBattleComponent __instance, bool isExit)
        {
            try
            {
                ModLogger.Msg($"[StageBattleComponentPatch] StageBattleComponent.Resume 호출됨 (isExit={isExit})");
                HwaBattleMediaController.ResumeMedia(isExit);
            }
            catch (Exception ex)
            {
                ModLogger.Error($"[StageBattleComponentPatch] Resume Postfix 예외: {ex}");
            }
        }
    }

    [HarmonyLib.HarmonyPatch(typeof(StageBattleComponent), nameof(StageBattleComponent.End))]
    public class StageBattleComponent_End_Patch
    {
        public static void Postfix(StageBattleComponent __instance)
        {
            ModLogger.Msg("[StageBattleComponentPatch] StageBattleComponent.End 호출됨");
            muse_dash_test.Patches.VictoryFlowGuard.MarkCompleted();
            // StopMedia 앞부분(ProgressBarPatch.Reset·HwaSyncManager.ResetCache)은 자체 try 밖입니다.
            // Exit/Release Postfix와 같이 여기서 감싸 게임의 End 흐름으로 예외가 새지 않게 합니다.
            try { HwaBattleMediaController.StopMedia(); }
            catch (Exception ex) { ModLogger.Error($"[StageBattleComponentPatch] End StopMedia 예외: {ex}"); }
        }
    }

    [HarmonyLib.HarmonyPatch(typeof(StageBattleComponent), nameof(StageBattleComponent.Exit))]
    public class StageBattleComponent_Exit_Patch
    {
        public static void Postfix(StageBattleComponent __instance)
        {
            ModLogger.Msg("[StageBattleComponentPatch] StageBattleComponent.Exit 호출됨");
            muse_dash_test.Patches.VictoryFlowGuard.MarkCompleted();
            ForcePerfectState.SetActiveStageBattle(null);
            // 리셋을 먼저 수행해 StopMedia가 예외를 던져도 stale 플래그가 남지 않게 합니다.
            try { CustomPlaySession.Current.ResetApplyDecision(); }
            catch (Exception ex) { ModLogger.Error($"[StageBattleComponentPatch] Exit ResetApplyDecision 예외: {ex}"); }
            try { HwaBattleMediaController.StopMedia(); }
            catch (Exception ex) { ModLogger.Error($"[StageBattleComponentPatch] Exit StopMedia 예외: {ex}"); }
        }
    }

    [HarmonyLib.HarmonyPatch(typeof(StageBattleComponent), nameof(StageBattleComponent.Release))]
    public class StageBattleComponent_Release_Patch
    {
        public static void Postfix(StageBattleComponent __instance)
        {
            ModLogger.Msg("[StageBattleComponentPatch] StageBattleComponent.Release 호출됨");
            muse_dash_test.Patches.VictoryFlowGuard.MarkCompleted();
            ForcePerfectState.SetActiveStageBattle(null);
            try { CustomPlaySession.Current.ResetApplyDecision(); }
            catch (Exception ex) { ModLogger.Error($"[StageBattleComponentPatch] Release ResetApplyDecision 예외: {ex}"); }
            try { HwaBattleMediaController.StopMedia(); }
            catch (Exception ex) { ModLogger.Error($"[StageBattleComponentPatch] Release StopMedia 예외: {ex}"); }
        }
    }

    [HarmonyLib.HarmonyPatch(typeof(StageBattleComponent), nameof(StageBattleComponent.GameRestart))]
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
