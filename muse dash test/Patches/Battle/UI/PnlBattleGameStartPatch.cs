using MelonLoader;
using System;
using System.Reflection;
using Il2CppAssets.Scripts.UI.Panels;
using muse_dash_test;

// PnlBattle.GameStart 호출 로그만 남기는 보조 패치
[HarmonyLib.HarmonyPatch(typeof(PnlBattle), muse_dash_test.GameBindings.PnlBattle.GameStart)]
public class PnlBattle_GameStart_Patch
{
    public static void Postfix(PnlBattle __instance)
    {
        ModLogger.Msg($"[PnlBattle.GameStart] 호출됨: {__instance}");

        try
        {
            string uid = muse_dash_test.CustomPlaySession.Current.LastKnownMusicUid;
            muse_dash_test.DiscordPresenceManager.UpdateForPlaying(uid);
            muse_dash_test.SpineActionController_PlayByKey_GhostNote_Patch.ResetCache();
        }
        catch (Exception ex)
        {
            ModLogger.Error($"[PnlBattleGameStartPatch] 배틀 시작 처리 예외: {ex.Message}");
        }
    }
}