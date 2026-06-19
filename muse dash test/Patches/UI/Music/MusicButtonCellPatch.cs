using MelonLoader;
using HarmonyLib;
using Il2CppAssets.Scripts.UI.Panels.PnlMusicTag;
using Il2CppAssets.Scripts.Database;
using System;

namespace muse_dash_test
{
    // MusicButtonCell.OnButtonClicked 후킹
    [HarmonyPatch(typeof(MusicButtonCell), nameof(MusicButtonCell.OnButtonClicked))]
    public class MusicButtonCell_OnButtonClicked_Patch
    {
        public static bool Prepare() => true;

        public static void Prefix(MusicButtonCell __instance)
        {
            try
            {
                if (__instance != null)
                {
                    var musicInfo = __instance.musicInfo;
                    string uid = musicInfo != null ? musicInfo.uid : "(null)";
                    CustomPlaySession.Current.LastClickedMusicUid = uid;
                    CustomPlaySession.Current.RememberMusicSelection(uid);
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"MusicButtonCell.OnButtonClicked Prefix 예외: {ex}");
            }
        }
    }

    // MusicButtonCell.InitMusicCell 후킹
    [HarmonyPatch(typeof(MusicButtonCell), nameof(MusicButtonCell.InitMusicCell), new Type[] { typeof(MusicInfo), typeof(int) })]
    public class MusicButtonCell_InitMusicCell_Patch
    {
        public static bool Prepare() => true;

        public static void Prefix(MusicButtonCell __instance, MusicInfo initMusicInfo, int tabIndex)
        {
            try
            {
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"MusicButtonCell.InitMusicCell Prefix 예외: {ex}");
            }
        }
    }
}
