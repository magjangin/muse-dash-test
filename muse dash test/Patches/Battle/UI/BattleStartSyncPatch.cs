using HarmonyLib;
using Il2CppAssets.Scripts.UI.GameMain;
using Il2CppFormulaBase;

namespace muse_dash_test
{
    /// <summary>
    /// 커스텀 BGM/BGA 주입을 레디·고 직후(PnlReadyToggle.PlayReadyGo)로 미루고,
    /// 재생은 게임이 음악을 실제로 시작(FixedOffset: 오프셋 시크 + 음소거 해제)하는 시점에 시작합니다.
    /// 배틀 로드 직후에 주입하면 게임 음악 시작보다 수 초 앞서 흘러 어긋난 소리가 들렸다가 재동기화됐습니다.
    /// </summary>
    [HarmonyPatch(typeof(PnlReadyToggle), nameof(PnlReadyToggle.PlayReadyGo))]
    public class PnlReadyToggle_PlayReadyGo_Inject_Patch
    {
        public static void Postfix()
        {
            try
            {
                ModLogger.Msg("[HwaBattleMediaController] 레디·고 감지, 커스텀 미디어 주입을 시작합니다.");
                HwaBattleMediaController.StartBattleMediaInjection();
            }
            catch (System.Exception ex)
            {
                ModLogger.Error($"[PlayReadyGo] 주입 예외: {ex}");
            }
        }
    }

    /// <summary>FixedOffset은 즉시 시작 경로에서 FixedMusicOffset을 인라인하므로 둘 다 후킹합니다.</summary>
    [HarmonyPatch(typeof(StageBattleComponent), nameof(StageBattleComponent.FixedOffset))]
    public class StageBattleComponent_FixedOffset_StartPlay_Patch
    {
        public static void Postfix()
        {
            try
            {
                HwaBattleMediaController.OnGameMusicStarted(GameBgmSource.TryGet(out _));
            }
            catch (System.Exception ex)
            {
                ModLogger.Error($"[FixedOffset] 커스텀 BGM 시작 처리 예외: {ex}");
            }
        }
    }

    [HarmonyPatch(typeof(StageBattleComponent), nameof(StageBattleComponent.FixedMusicOffset))]
    public class StageBattleComponent_FixedMusicOffset_StartPlay_Patch
    {
        public static void Postfix(UnityEngine.AudioSource bgm)
        {
            try
            {
                HwaBattleMediaController.OnGameMusicStarted(bgm);
            }
            catch (System.Exception ex)
            {
                ModLogger.Error($"[FixedMusicOffset] 커스텀 BGM 시작 처리 예외: {ex}");
            }
        }
    }
}
