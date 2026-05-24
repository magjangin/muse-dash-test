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
