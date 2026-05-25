using MelonLoader;
using System;
using Il2CppAssets.Scripts.UI.Panels;
using UnityEngine.UI;

// PnlStage.Awake 후킹
[HarmonyLib.HarmonyPatch(typeof(PnlStage), "Awake")]
public class PnlStage_Awake_Patch
{
    public static void Prefix(PnlStage __instance)
    {
        try { }
        catch (Exception ex) { MelonLogger.Error($"PnlStage.Awake Prefix 예외: {ex}"); }
    }

    public static void Postfix(PnlStage __instance)
    {
        try
        {
            PnlStagePatchHelper.SyncExperimentModeFromStage(__instance);
        }
        catch (Exception ex) { MelonLogger.Error($"PnlStage.Awake Postfix 예외: {ex}"); }
    }
}

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
            PnlStagePatchHelper.SyncExperimentModeFromStage(__instance);
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
            PnlStagePatchHelper.SyncExperimentModeFromStage(__instance);
            PnlStagePatchHelper.ApplyCustomTagTitleAccessors("PnlStage.ChangeMusic", __instance);
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
        try { }
        catch (Exception ex) { MelonLogger.Error($"PnlStage.ChangeFinalMusic Postfix 예외: {ex}"); }
    }
}

// PnlStage.RefreshTagTitle 후킹
[HarmonyLib.HarmonyPatch(typeof(PnlStage), "RefreshTagTitle")]
public class PnlStage_RefreshTagTitle_Patch
{
    public static void Prefix(PnlStage __instance) { }
    public static void Postfix(PnlStage __instance) { }
}

// PnlStage.musicNameTitle getter 후킹
[HarmonyLib.HarmonyPatch(typeof(PnlStage), nameof(PnlStage.musicNameTitle), HarmonyLib.MethodType.Getter)]
public class PnlStage_GetMusicNameTitle_Patch
{
    public static void Postfix(PnlStage __instance, ref Text __result) { }
}

// PnlStage.artistNameTitle getter 후킹
[HarmonyLib.HarmonyPatch(typeof(PnlStage), nameof(PnlStage.artistNameTitle), HarmonyLib.MethodType.Getter)]
public class PnlStage_GetArtistNameTitle_Patch
{
    public static void Postfix(PnlStage __instance, ref Text __result) { }
}
