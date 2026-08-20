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
        }
        catch (Exception ex)
        {
            ModLogger.Error($"[PnlBattleGameStartPatch] Discord Presence 갱신 예외: {ex.Message}");
        }
    }
}