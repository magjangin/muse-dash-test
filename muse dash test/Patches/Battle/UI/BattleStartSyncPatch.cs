using HarmonyLib;
using Il2CppFormulaBase;

namespace muse_dash_test
{
    /// <summary>
    /// 게임이 음악을 실제로 시작(오프셋 시크 + 음소거 해제)하는 시점에 커스텀 BGM을 틉니다.
    /// FixedOffset은 즉시 시작 경로에서 FixedMusicOffset을 인라인하므로 둘 다 후킹합니다.
    /// </summary>
    [HarmonyPatch(typeof(StageBattleComponent), nameof(StageBattleComponent.FixedOffset))]
    public class StageBattleComponent_FixedOffset_StartPlay_Patch
    {
        public static void Postfix()
        {
            HwaBattleMediaController.StartPlaybackAtGameStart(GameBgmSource.TryGet(out _));
        }
    }

    [HarmonyPatch(typeof(StageBattleComponent), nameof(StageBattleComponent.FixedMusicOffset))]
    public class StageBattleComponent_FixedMusicOffset_StartPlay_Patch
    {
        public static void Postfix(UnityEngine.AudioSource bgm)
        {
            HwaBattleMediaController.StartPlaybackAtGameStart(bgm);
        }
    }
}
