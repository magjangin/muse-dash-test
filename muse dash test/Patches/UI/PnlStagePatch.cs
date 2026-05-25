using MelonLoader;
using System;
using Il2CppAssets.Scripts.UI.Panels;
using UnityEngine.UI;

// PnlStage.Start 후킹
[HarmonyLib.HarmonyPatch(typeof(PnlStage), "Start")]
public class PnlStage_Start_Patch
{
    public static void Prefix(PnlStage __instance)
    {
        try { }
        catch (Exception ex) { MelonLogger.Error($"PnlStage.Start Prefix 예외: {ex}"); }
    }

    public static void Postfix(PnlStage __instance)
    {
        try
        {
            PnlStagePatchHelper.LogPnlStageProperties("PnlStage.Start.Properties", __instance);
            PnlMusicUtils.LogMusicInfo("PnlStage.Start", __instance);
            MelonCoroutines.Start(PnlMusicUtils.LogMusicInfoAfterDelay("PnlStage.Start.Delay", __instance, 0.5f));
        }
        catch (Exception ex) { MelonLogger.Error($"PnlStage.Start Postfix 예외: {ex}"); }
    }
}

// PnlStage.ChangeMusic(int) 후킹
[HarmonyLib.HarmonyPatch(typeof(PnlStage), "ChangeMusic", new Type[] { typeof(int) })]
public class PnlStage_ChangeMusic_Patch
{
    public static void Prefix(PnlStage __instance, int i)
    {
        try { }
        catch (Exception ex) { MelonLogger.Error($"PnlStage.ChangeMusic Prefix 예외: {ex}"); }
    }

    public static void Postfix(PnlStage __instance, int i)
    {
        try
        {
            PnlStagePatchHelper.ApplyCustomTagTitleAccessors("PnlStage.ChangeMusic", __instance);
            PnlStagePatchHelper.LogPnlStageProperties("PnlStage.ChangeMusic.Properties", __instance);
            PnlStagePatchHelper.LogMusicRootComponents("PnlStage.ChangeMusic.MusicRoot", __instance);
            PnlMusicUtils.LogMusicInfo("PnlStage.ChangeMusic", __instance);
        }
        catch (Exception ex) { MelonLogger.Error($"PnlStage.ChangeMusic Postfix 예외: {ex}"); }
    }
}

// PnlStage.ChangeFinalMusic(int) 후킹
[HarmonyLib.HarmonyPatch(typeof(PnlStage), "ChangeFinalMusic", new Type[] { typeof(int) })]
public class PnlStage_ChangeFinalMusic_Patch
{
    public static void Prefix(PnlStage __instance, int i)
    {
        try { }
        catch (Exception ex) { MelonLogger.Error($"PnlStage.ChangeFinalMusic Prefix 예외: {ex}"); }
    }

    public static void Postfix(PnlStage __instance, int i)
    {
        try
        {
            PnlMusicUtils.LogMusicInfo("PnlStage.ChangeFinalMusic", __instance);
        }
        catch (Exception ex) { MelonLogger.Error($"PnlStage.ChangeFinalMusic Postfix 예외: {ex}"); }
    }
}

// PnlStage.RefreshTagTitle 후킹
[HarmonyLib.HarmonyPatch(typeof(PnlStage), "RefreshTagTitle")]
public class PnlStage_RefreshTagTitle_Patch
{
    public static void Prefix(PnlStage __instance)
    {
        PnlStagePatchHelper.LogPnlStageRefresh("PnlStage.RefreshTagTitle.Prefix", __instance);
    }

    public static void Postfix(PnlStage __instance)
    {
        PnlStagePatchHelper.LogPnlStageRefresh("PnlStage.RefreshTagTitle.Postfix", __instance);
    }
}

// PnlStage.musicNameTitle getter 후킹
[HarmonyLib.HarmonyPatch(typeof(PnlStage), nameof(PnlStage.musicNameTitle), HarmonyLib.MethodType.Getter)]
public class PnlStage_GetMusicNameTitle_Patch
{
    public static bool Prepare()
    {
        MelonLogger.Msg("[PnlStage.get_musicNameTitle] 접근자 후킹 준비 완료");
        return true;
    }

    public static void Postfix(PnlStage __instance, ref Text __result)
    {
        PnlStagePatchHelper.LogTextAccessor("PnlStage.get_musicNameTitle", __instance, __result);
    }
}

// PnlStage.artistNameTitle getter 후킹
[HarmonyLib.HarmonyPatch(typeof(PnlStage), nameof(PnlStage.artistNameTitle), HarmonyLib.MethodType.Getter)]
public class PnlStage_GetArtistNameTitle_Patch
{
    public static bool Prepare()
    {
        MelonLogger.Msg("[PnlStage.get_artistNameTitle] 접근자 후킹 준비 완료");
        return true;
    }

    public static void Postfix(PnlStage __instance, ref Text __result)
    {
        PnlStagePatchHelper.LogTextAccessor("PnlStage.get_artistNameTitle", __instance, __result);
    }
}
