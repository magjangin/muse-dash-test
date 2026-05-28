using MelonLoader;
using System;
using Il2CppAssets.Scripts.UI.Panels;
using UnityEngine.UI;

// PnlStage.OnEnable 후킹
[HarmonyLib.HarmonyPatch(typeof(PnlStage), "OnEnable")]
public class PnlStage_OnEnable_Patch
{
    public static void Prefix(PnlStage __instance)
    {
        try
        {
        }
        catch (Exception ex) { MelonLogger.Error($"PnlStage.OnEnable Prefix 예외: {ex}"); }
    }

    public static void Postfix(PnlStage __instance)
    {
        try
        {
            PnlStagePatchHelper.SyncExperimentModeFromStage(__instance);
            PnlStagePatchHelper.ApplyCustomTagTitleAccessors("PnlStage.OnEnable", __instance);
            PnlStagePatchHelper.LogPnlStageRefresh("PnlStage.OnEnable", __instance);
            PnlStagePatchHelper.LogPnlStageProperties("PnlStage.OnEnable", __instance);
            PnlStagePatchHelper.LogMusicRootComponents("PnlStage.OnEnable", __instance);
        }
        catch (Exception ex) { MelonLogger.Error($"PnlStage.OnEnable Postfix 예외: {ex}"); }
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
            PnlStagePatchHelper.ForceApplyCustomTagTitleAccessors("PnlStage.ChangeMusic.Force", __instance);
            PnlStagePatchHelper.LogPnlStageRefresh("PnlStage.ChangeMusic", __instance);
            PnlStagePatchHelper.LogMusicRootComponents("PnlStage.ChangeMusic", __instance);
            PnlStagePatchHelper.LogButtons("PnlStage.ChangeMusic", __instance);
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
        try
        {
        }
        catch (Exception ex) { MelonLogger.Error($"PnlStage.ChangeFinalMusic Prefix 예외: {ex}"); }
    }

    public static void Postfix(PnlStage __instance, int i)
    {
        try
        {
            PnlStagePatchHelper.ForceApplyCustomTagTitleAccessors("PnlStage.ChangeFinalMusic.Force", __instance);
            PnlStagePatchHelper.LogPnlStageRefresh("PnlStage.ChangeFinalMusic", __instance);
            PnlStagePatchHelper.LogMusicRootComponents("PnlStage.ChangeFinalMusic", __instance);
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
    }

    public static void Postfix(PnlStage __instance)
    {
        try
        {
            PnlStagePatchHelper.ApplyCustomTagTitleAccessors("PnlStage.RefreshTagTitle", __instance);
            PnlStagePatchHelper.ForceApplyCustomTagTitleAccessors("PnlStage.RefreshTagTitle.Force", __instance);
            PnlStagePatchHelper.LogPnlStageRefresh("PnlStage.RefreshTagTitle", __instance);
        }
        catch (Exception ex) { MelonLogger.Error($"PnlStage.RefreshTagTitle Postfix 예외: {ex}"); }
    }
}

// PnlStage.musicNameTitle getter 후킹
[HarmonyLib.HarmonyPatch(typeof(PnlStage), nameof(PnlStage.musicNameTitle), HarmonyLib.MethodType.Getter)]
public class PnlStage_GetMusicNameTitle_Patch
{
    public static void Postfix(PnlStage __instance, ref Text __result)
    {
    }
}

// PnlStage.artistNameTitle getter 후킹
[HarmonyLib.HarmonyPatch(typeof(PnlStage), nameof(PnlStage.artistNameTitle), HarmonyLib.MethodType.Getter)]
public class PnlStage_GetArtistNameTitle_Patch
{
    public static void Postfix(PnlStage __instance, ref Text __result)
    {
    }
}

// PnlStage.OnAddCollection(MusicInfo) 후킹
[HarmonyLib.HarmonyPatch(typeof(PnlStage), "OnAddCollection", new Type[] { typeof(Il2CppAssets.Scripts.Database.MusicInfo) })]
public class PnlStage_OnAddCollection_Patch
{
    public static void Prefix(PnlStage __instance, Il2CppAssets.Scripts.Database.MusicInfo musicInfo)
    {
        try
        {
            MelonLogger.Msg($"PnlStage.OnAddCollection Prefix: {PnlStagePatchHelper.DescribeMusicInfo(musicInfo)}");
        }
        catch (Exception ex) { MelonLogger.Error($"PnlStage.OnAddCollection Prefix 예외: {ex}"); }
    }

    public static void Postfix(PnlStage __instance, Il2CppAssets.Scripts.Database.MusicInfo musicInfo)
    {
        try
        {
            MelonLogger.Msg($"PnlStage.OnAddCollection Postfix: {PnlStagePatchHelper.DescribeMusicInfo(musicInfo)}");
        }
        catch (Exception ex) { MelonLogger.Error($"PnlStage.OnAddCollection Postfix 예외: {ex}"); }
    }
}

// PnlStage.OnRemoveCollection(MusicInfo) 후킹
[HarmonyLib.HarmonyPatch(typeof(PnlStage), "OnRemoveCollection", new Type[] { typeof(Il2CppAssets.Scripts.Database.MusicInfo) })]
public class PnlStage_OnRemoveCollection_Patch
{
    public static void Prefix(PnlStage __instance, Il2CppAssets.Scripts.Database.MusicInfo musicInfo)
    {
        try
        {
            MelonLogger.Msg($"PnlStage.OnRemoveCollection Prefix: {PnlStagePatchHelper.DescribeMusicInfo(musicInfo)}");
        }
        catch (Exception ex) { MelonLogger.Error($"PnlStage.OnRemoveCollection Prefix 예외: {ex}"); }
    }

    public static void Postfix(PnlStage __instance, Il2CppAssets.Scripts.Database.MusicInfo musicInfo)
    {
        try
        {
            MelonLogger.Msg($"PnlStage.OnRemoveCollection Postfix: {PnlStagePatchHelper.DescribeMusicInfo(musicInfo)}");
        }
        catch (Exception ex) { MelonLogger.Error($"PnlStage.OnRemoveCollection Postfix 예외: {ex}"); }
    }
}

// PnlStage.OnHideMusic(MusicInfo) 후킹
[HarmonyLib.HarmonyPatch(typeof(PnlStage), "OnHideMusic", new Type[] { typeof(Il2CppAssets.Scripts.Database.MusicInfo) })]
public class PnlStage_OnHideMusic_Patch
{
    public static void Prefix(PnlStage __instance, Il2CppAssets.Scripts.Database.MusicInfo musicInfo)
    {
        try
        {
            MelonLogger.Msg($"PnlStage.OnHideMusic Prefix: {PnlStagePatchHelper.DescribeMusicInfo(musicInfo)}");
        }
        catch (Exception ex) { MelonLogger.Error($"PnlStage.OnHideMusic Prefix 예외: {ex}"); }
    }

    public static void Postfix(PnlStage __instance, Il2CppAssets.Scripts.Database.MusicInfo musicInfo)
    {
        try
        {
            MelonLogger.Msg($"PnlStage.OnHideMusic Postfix: {PnlStagePatchHelper.DescribeMusicInfo(musicInfo)}");
        }
        catch (Exception ex) { MelonLogger.Error($"PnlStage.OnHideMusic Postfix 예외: {ex}"); }
    }
}

// PnlStage.OnRemoveHideMusic(MusicInfo) 후킹
[HarmonyLib.HarmonyPatch(typeof(PnlStage), "OnRemoveHideMusic", new Type[] { typeof(Il2CppAssets.Scripts.Database.MusicInfo) })]
public class PnlStage_OnRemoveHideMusic_Patch
{
    public static void Prefix(PnlStage __instance, Il2CppAssets.Scripts.Database.MusicInfo musicInfo)
    {
        try
        {
            MelonLogger.Msg($"PnlStage.OnRemoveHideMusic Prefix: {PnlStagePatchHelper.DescribeMusicInfo(musicInfo)}");
        }
        catch (Exception ex) { MelonLogger.Error($"PnlStage.OnRemoveHideMusic Prefix 예외: {ex}"); }
    }

    public static void Postfix(PnlStage __instance, Il2CppAssets.Scripts.Database.MusicInfo musicInfo)
    {
        try
        {
            MelonLogger.Msg($"PnlStage.OnRemoveHideMusic Postfix: {PnlStagePatchHelper.DescribeMusicInfo(musicInfo)}");
        }
        catch (Exception ex) { MelonLogger.Error($"PnlStage.OnRemoveHideMusic Postfix 예외: {ex}"); }
    }
}

// PnlStage.SetLikeAndHideTglState(MusicInfo, bool) 후킹
[HarmonyLib.HarmonyPatch(typeof(PnlStage), "SetLikeAndHideTglState", new Type[] { typeof(Il2CppAssets.Scripts.Database.MusicInfo), typeof(bool) })]
public class PnlStage_SetLikeAndHideTglState_Patch
{
    public static void Prefix(PnlStage __instance, Il2CppAssets.Scripts.Database.MusicInfo musicInfo, bool isUnlock)
    {
        try
        {
            MelonLogger.Msg($"PnlStage.SetLikeAndHideTglState Prefix: {PnlStagePatchHelper.DescribeMusicInfo(musicInfo)}, isUnlock={isUnlock}");
        }
        catch (Exception ex) { MelonLogger.Error($"PnlStage.SetLikeAndHideTglState Prefix 예외: {ex}"); }
    }

    public static void Postfix(PnlStage __instance, Il2CppAssets.Scripts.Database.MusicInfo musicInfo, bool isUnlock)
    {
        try
        {
            MelonLogger.Msg($"PnlStage.SetLikeAndHideTglState Postfix: {PnlStagePatchHelper.DescribeMusicInfo(musicInfo)}, isUnlock={isUnlock}");
        }
        catch (Exception ex) { MelonLogger.Error($"PnlStage.SetLikeAndHideTglState Postfix 예외: {ex}"); }
    }
}

// PnlStage.SetAchievementPercent(MusicInfo) 후킹
[HarmonyLib.HarmonyPatch(typeof(PnlStage), "SetAchievementPercent", new Type[] { typeof(Il2CppAssets.Scripts.Database.MusicInfo) })]
public class PnlStage_SetAchievementPercent_Patch
{
    public static void Prefix(PnlStage __instance, Il2CppAssets.Scripts.Database.MusicInfo musicInfo)
    {
        try
        {
            MelonLogger.Msg($"PnlStage.SetAchievementPercent Prefix: {PnlStagePatchHelper.DescribeMusicInfo(musicInfo)}");
        }
        catch (Exception ex) { MelonLogger.Error($"PnlStage.SetAchievementPercent Prefix 예외: {ex}"); }
    }

    public static void Postfix(PnlStage __instance, Il2CppAssets.Scripts.Database.MusicInfo musicInfo)
    {
        try
        {
            MelonLogger.Msg($"PnlStage.SetAchievementPercent Postfix: {PnlStagePatchHelper.DescribeMusicInfo(musicInfo)}");
        }
        catch (Exception ex) { MelonLogger.Error($"PnlStage.SetAchievementPercent Postfix 예외: {ex}"); }
    }
}

// PnlStage.RefreshDiffUI(MusicInfo) 후킹
[HarmonyLib.HarmonyPatch(typeof(PnlStage), "RefreshDiffUI", new Type[] { typeof(Il2CppAssets.Scripts.Database.MusicInfo) })]
public class PnlStage_RefreshDiffUI_Patch
{
    public static void Prefix(PnlStage __instance, Il2CppAssets.Scripts.Database.MusicInfo musicInfo)
    {
        try
        {
            if (musicInfo != null && !string.IsNullOrEmpty(musicInfo.uid))
            {
                PnlStagePatchHelper.LastSelectedMusicUid = musicInfo.uid;
            }
            string musicText = __instance.musicNameTitle != null ? __instance.musicNameTitle.text : "(null)";
            string artistText = __instance.artistNameTitle != null ? __instance.artistNameTitle.text : "(null)";
            MelonLogger.Msg($"PnlStage.RefreshDiffUI Prefix: musicNameTitle={musicText}, artistNameTitle={artistText}");
        }
        catch (Exception ex) { MelonLogger.Error($"PnlStage.RefreshDiffUI Prefix 예외: {ex}"); }
    }

    public static void Postfix(PnlStage __instance, Il2CppAssets.Scripts.Database.MusicInfo musicInfo)
    {
        try
        {
            PnlStagePatchHelper.ApplyCustomTagTitleAccessorsForMusicInfo("PnlStage.RefreshDiffUI.Direct", __instance, musicInfo);
            PnlStagePatchHelper.ApplyCustomTagTitleAccessors("PnlStage.RefreshDiffUI", __instance);
            PnlStagePatchHelper.ForceApplyCustomTagTitleAccessors("PnlStage.RefreshDiffUI.Force", __instance);

            string musicText = __instance.musicNameTitle != null ? __instance.musicNameTitle.text : "(null)";
            string artistText = __instance.artistNameTitle != null ? __instance.artistNameTitle.text : "(null)";
            MelonLogger.Msg($"PnlStage.RefreshDiffUI Postfix: musicNameTitle={musicText}, artistNameTitle={artistText}");
        }
        catch (Exception ex) { MelonLogger.Error($"PnlStage.RefreshDiffUI Postfix 예외: {ex}"); }
    }
}

// PnlStage.RefreshBg(MusicInfo, bool) 후킹
[HarmonyLib.HarmonyPatch(typeof(PnlStage), "RefreshBg", new Type[] { typeof(Il2CppAssets.Scripts.Database.MusicInfo), typeof(bool) })]
public class PnlStage_RefreshBg_Patch
{
    public static void Prefix(PnlStage __instance, Il2CppAssets.Scripts.Database.MusicInfo musicInfo, bool hideText)
    {
        try
        {
            MelonLogger.Msg($"PnlStage.RefreshBg Prefix: {PnlStagePatchHelper.DescribeMusicInfo(musicInfo)}, hideText={hideText}");
        }
        catch (Exception ex) { MelonLogger.Error($"PnlStage.RefreshBg Prefix 예외: {ex}"); }
    }

    public static void Postfix(PnlStage __instance, Il2CppAssets.Scripts.Database.MusicInfo musicInfo, bool hideText)
    {
        try
        {
            MelonLogger.Msg($"PnlStage.RefreshBg Postfix: {PnlStagePatchHelper.DescribeMusicInfo(musicInfo)}, hideText={hideText}");
        }
        catch (Exception ex) { MelonLogger.Error($"PnlStage.RefreshBg Postfix 예외: {ex}"); }
    }
}

// PnlStage.IsContainOffPlan(MusicInfo) 후킹
[HarmonyLib.HarmonyPatch(typeof(PnlStage), "IsContainOffPlan", new Type[] { typeof(Il2CppAssets.Scripts.Database.MusicInfo) })]
public class PnlStage_IsContainOffPlan_Patch
{
    public static void Prefix(PnlStage __instance, Il2CppAssets.Scripts.Database.MusicInfo musicInfo)
    {
        try
        {
            MelonLogger.Msg($"PnlStage.IsContainOffPlan Prefix: {PnlStagePatchHelper.DescribeMusicInfo(musicInfo)}");
        }
        catch (Exception ex) { MelonLogger.Error($"PnlStage.IsContainOffPlan Prefix 예외: {ex}"); }
    }

    public static void Postfix(PnlStage __instance, Il2CppAssets.Scripts.Database.MusicInfo musicInfo, ref bool __result)
    {
        try
        {
            MelonLogger.Msg($"PnlStage.IsContainOffPlan Postfix: {PnlStagePatchHelper.DescribeMusicInfo(musicInfo)}, result={__result}");
        }
        catch (Exception ex) { MelonLogger.Error($"PnlStage.IsContainOffPlan Postfix 예외: {ex}"); }
    }
}