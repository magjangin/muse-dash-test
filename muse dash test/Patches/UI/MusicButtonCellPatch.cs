using MelonLoader;
using System;

// MusicButtonCell.InitMusicCell 후킹
[HarmonyLib.HarmonyPatch(typeof(Il2CppAssets.Scripts.UI.Panels.PnlMusicTag.MusicButtonCell), "InitMusicCell", new Type[] { typeof(Il2CppAssets.Scripts.Database.MusicInfo), typeof(int) })]
public class MusicButtonCell_InitMusicCell_Patch
{
    public static bool Prepare()
    {
        MelonLogger.Msg("[MusicButtonCell.InitMusicCell] 후킹 준비 완료");
        return true;
    }

    public static void Prefix(Il2CppAssets.Scripts.UI.Panels.PnlMusicTag.MusicButtonCell __instance, ref Il2CppAssets.Scripts.Database.MusicInfo initMusicInfo, ref int tabIndex)
    {
        try
        {
            if (__instance != null && initMusicInfo != null)
            {
                string musicUid = initMusicInfo.uid;
                string musicName = initMusicInfo.name;
                string musicAuthor = initMusicInfo.author;
                string gameObjectName = __instance.gameObject != null ? __instance.gameObject.name : "(null)";
                
                MelonLogger.Msg($"[MusicButtonCell.InitMusicCell] Prefix: GameObject={gameObjectName} | Uid={musicUid} | Name={musicName} | Author={musicAuthor} | TabIndex={tabIndex}");
            }
        }
        catch (Exception ex)
        {
            MelonLogger.Error($"MusicButtonCell.InitMusicCell Prefix 예외: {ex}");
        }
    }
}

// MusicButtonCell.OnButtonClicked 후킹
[HarmonyLib.HarmonyPatch(typeof(Il2CppAssets.Scripts.UI.Panels.PnlMusicTag.MusicButtonCell), "OnButtonClicked")]
public class MusicButtonCell_OnButtonClicked_Patch
{
    public static bool Prepare()
    {
        MelonLogger.Msg("[MusicButtonCell.OnButtonClicked] 후킹 준비 완료");
        return true;
    }

    public static void Prefix(Il2CppAssets.Scripts.UI.Panels.PnlMusicTag.MusicButtonCell __instance)
    {
        try
        {
            if (__instance != null)
            {
                var musicInfo = __instance.musicInfo;
                string musicUid = musicInfo != null ? musicInfo.uid : "(unknown)";
                string musicName = musicInfo != null ? musicInfo.name : "(unknown)";
                string gameObjectName = __instance.gameObject != null ? __instance.gameObject.name : "(null)";
                
                MelonLogger.Msg($"[MusicButtonCell.OnButtonClicked] Prefix: 곡 셀 클릭됨! GameObject={gameObjectName} | Uid={musicUid} | Name={musicName}");
            }
        }
        catch (Exception ex)
        {
            MelonLogger.Error($"MusicButtonCell.OnButtonClicked Prefix 예외: {ex}");
        }
    }
}
