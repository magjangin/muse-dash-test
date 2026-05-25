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
        public static bool Prepare()
        {
            MelonLogger.Msg("[MusicButtonCell.OnButtonClicked] 후킹 준비 완료");
            return true;
        }

        public static void Prefix(MusicButtonCell __instance)
        {
            try
            {
                if (__instance != null)
                {
                    var musicInfo = __instance.musicInfo;
                    string uid = musicInfo != null ? musicInfo.uid : "(null)";
                    string name = musicInfo != null ? musicInfo.name : "(null)";
                    MelonLogger.Msg($"[MusicButtonCell.OnButtonClicked] Prefix 호출됨! Clicked Cell - Uid: {uid}, Name: {name}");
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
        public static bool Prepare()
        {
            MelonLogger.Msg("[MusicButtonCell.InitMusicCell] 후킹 준비 완료");
            return true;
        }

        public static void Prefix(MusicButtonCell __instance, MusicInfo initMusicInfo, int tabIndex)
        {
            try
            {
                if (__instance != null && initMusicInfo != null)
                {
                    MelonLogger.Msg($"[MusicButtonCell.InitMusicCell] Prefix 호출됨! Cell Init - Uid: {initMusicInfo.uid}, Name: {initMusicInfo.name}, TabIndex: {tabIndex}");
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"MusicButtonCell.InitMusicCell Prefix 예외: {ex}");
            }
        }
    }
}
