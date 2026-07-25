using MelonLoader;
using System;
using System.Reflection;
using Il2CppAssets.Scripts.UI.Panels;

// PnlBattle.GameStart 호출 로그만 남기는 보조 패치
[HarmonyLib.HarmonyPatch(typeof(PnlBattle), muse_dash_test.GameBindings.PnlBattle.GameStart)]
public class PnlBattle_GameStart_Patch
{
    public static void Postfix(PnlBattle __instance)
    {
        MelonLogger.Msg($"[PnlBattle.GameStart] 호출됨: {__instance}");

        try
        {
            string uid = muse_dash_test.CustomPlaySession.Current.LastKnownMusicUid;
            if (muse_dash_test.HwaResourceManager.TryGetHwaPrimarySong(uid, out string title, out string artist, out _, out _, out _, out _, out _, out _, out _))
            {
                muse_dash_test.DiscordPresenceManager.SetPlayingSong(title, artist, "커스텀 플레이");
            }
            else
            {
                muse_dash_test.DiscordPresenceManager.SetPlayingSong($"곡 {uid}", "Muse Dash");
            }
        }
        catch (Exception ex)
        {
            MelonLogger.Error($"[PnlBattleGameStartPatch] Discord Presence 갱신 예외: {ex.Message}");
        }
    }
}