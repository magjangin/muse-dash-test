using MelonLoader;
using System;
using Il2CppAssets.Scripts.UI.Panels;
using UnityEngine.UI;
using Il2CppAssets.Scripts.PeroTools.Nice.Interface;
using Il2CppAssets.Scripts.PeroTools.Nice.Datas;
using Il2CppAssets.Scripts.PeroTools.Commons;
using muse_dash_test;

// PnlStage.OnEnable 후킹
[HarmonyLib.HarmonyPatch(typeof(PnlStage), "OnEnable")]
public class PnlStage_OnEnable_Patch
{
    public static void Prefix(PnlStage __instance)
    {
        try
        {
            var dataManager = Singleton<DataManager>.instance;
            if (dataManager != null)
            {
                var account = (DataObject)dataManager["Account"];
                if (account != null)
                {
                    IVariable val = account["IsUnlockAllMaster"];
                    if (val != null)
                    {
                        VariableUtils.SetResult(val, (Il2CppSystem.Object)true);
                        MelonLogger.Msg("[🔓 FixLocksPatch] PnlStage.OnEnable - IsUnlockAllMaster를 true로 설정 완료!");
                    }
                }
            }
        }
        catch (Exception ex) { MelonLogger.Error($"PnlStage.OnEnable Prefix 예외: {ex}"); }
    }

    public static void Postfix(PnlStage __instance)
    {
        try
        {
            PnlStagePatchHelper.SyncExperimentModeFromStage(__instance);
            PnlStagePatchHelper.ApplyTagTitle("PnlStage.OnEnable", __instance);
        }
        catch (Exception ex) { MelonLogger.Error($"PnlStage.OnEnable Postfix 예외: {ex}"); }
    }
}

// PnlStage.ChangeMusic(int) 후킹
[HarmonyLib.HarmonyPatch(typeof(PnlStage), "ChangeMusic", new Type[] { typeof(int) })]
public class PnlStage_ChangeMusic_Patch
{
    public static void Postfix(PnlStage __instance, int i)
    {
        try
        {
            MelonLogger.Msg($"[PnlStage.ChangeMusic] enter index={i}, previousSelectedUid={CustomPlaySession.Current.SelectedMusicUid}, previousShouldApply={CustomPlaySession.Current.ShouldApplyExperimentChart}, previousExperimentMode={CustomPlaySession.Current.IsExperimentModeActive}");
            PnlStagePatchHelper.SyncExperimentModeFromStage(__instance);
            PnlStagePatchHelper.ApplyTagTitle("PnlStage.ChangeMusic", __instance);
            PnlStagePatchHelper.ForceApplyTagTitle("PnlStage.ChangeMusic.Force", __instance);
            PnlStagePatchHelper.LogButtons("PnlStage.ChangeMusic", __instance);
            MelonLogger.Msg($"[PnlStage.ChangeMusic] exit index={i}, selectedUid={CustomPlaySession.Current.SelectedMusicUid}, currentShouldApply={CustomPlaySession.Current.ShouldApplyExperimentChart}, currentExperimentMode={CustomPlaySession.Current.IsExperimentModeActive}");
        }
        catch (Exception ex) { MelonLogger.Error($"PnlStage.ChangeMusic Postfix 예외: {ex}"); }
    }
}

// PnlStage.ChangeFinalMusic(int) 후킹
[HarmonyLib.HarmonyPatch(typeof(PnlStage), "ChangeFinalMusic", new Type[] { typeof(int) })]
public class PnlStage_ChangeFinalMusic_Patch
{
    public static void Postfix(PnlStage __instance, int i)
    {
        try
        {
            PnlStagePatchHelper.ForceApplyTagTitle("PnlStage.ChangeFinalMusic.Force", __instance);
        }
        catch (Exception ex) { MelonLogger.Error($"PnlStage.ChangeFinalMusic Postfix 예외: {ex}"); }
    }
}

// PnlStage.RefreshTagTitle 후킹
[HarmonyLib.HarmonyPatch(typeof(PnlStage), "RefreshTagTitle")]
public class PnlStage_RefreshTagTitle_Patch
{
    public static void Postfix(PnlStage __instance)
    {
        try
        {
            PnlStagePatchHelper.ApplyTagTitle("PnlStage.RefreshTagTitle", __instance);
            PnlStagePatchHelper.ForceApplyTagTitle("PnlStage.RefreshTagTitle.Force", __instance);
        }
        catch (Exception ex) { MelonLogger.Error($"PnlStage.RefreshTagTitle Postfix 예외: {ex}"); }
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
            CustomRecordUiPatchHelper.ApplyCustomRecordToPnlStage(__instance, musicInfo);
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
            var dataManager = Singleton<DataManager>.instance;
            DataObject account = null;
            IVariable val = null;
            bool success = false;

            if (dataManager != null)
            {
                account = (DataObject)dataManager["Account"];
                if (account != null)
                {
                    val = account["IsUnlockAllMaster"];
                    if (val != null)
                    {
                        VariableUtils.SetResult(val, (Il2CppSystem.Object)true);
                        success = true;
                    }
                }
            }

            string musicUid = musicInfo != null ? musicInfo.uid : "(null)";
            string dmPtr = dataManager != null ? "Active" : "Null";
            string accPtr = account != null ? "Found" : "Null";
            string varState = val != null ? "Found" : "Null";
            string execState = success ? "SUCCESS" : "FAILED";

            MelonLogger.Msg($"[🔓 Unlock Process] PnlStage.RefreshDiffUI -> Target: [{musicUid}]");
            MelonLogger.Msg($"  ├─ Step 1: Il2CppAssets.Scripts.PeroTools.Nice.Datas.DataManager dm = Il2CppAssets.Scripts.PeroTools.Commons.Singleton<Il2CppAssets.Scripts.PeroTools.Nice.Datas.DataManager>.instance; -> {dmPtr}");
            MelonLogger.Msg($"  ├─ Step 2: Il2CppAssets.Scripts.PeroTools.Nice.Datas.DataObject account = (Il2CppAssets.Scripts.PeroTools.Nice.Datas.DataObject)dm[\"Account\"]; -> {accPtr}");
            MelonLogger.Msg($"  ├─ Step 3: Il2CppAssets.Scripts.PeroTools.Nice.Interface.IVariable variable = account[\"IsUnlockAllMaster\"]; -> {varState}");
            MelonLogger.Msg($"  └─ Step 4: Il2CppAssets.Scripts.PeroTools.Commons.VariableUtils.SetResult(variable, true); -> {execState}");

            if (musicInfo != null && !string.IsNullOrEmpty(musicInfo.uid))
            {
                CustomPlaySession.Current.SelectedMusicUid = musicInfo.uid;
                CustomPlaySession.Current.RememberMusicSelection(musicInfo.uid);
                if (CustomContentIds.IsVirtualSong(musicInfo.uid))
                {
                    HwaMenuBgmController.TriggerMenuBgmChange(musicInfo.uid);
                }
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
            MelonLogger.Msg($"[PnlStage.RefreshDiffUI.Postfix] enter uid={musicInfo?.uid ?? "(null)"}, currentSelectedUid={CustomPlaySession.Current.SelectedMusicUid}, experimentMode={CustomPlaySession.Current.IsExperimentModeActive}, shouldApply={CustomPlaySession.Current.ShouldApplyExperimentChart}");
            CustomRecordUiPatchHelper.ApplyCustomRecordToPnlStage(__instance, musicInfo);

            if (PnlStagePatchHelper.ApplyTagTitleForMusicInfo("PnlStage.RefreshDiffUI.Direct", __instance, musicInfo))
            {
                // 다이내믹 주입에 성공했으면 다른 정적 주입은 실행하지 않고 리턴합니다.
                string mText = __instance.musicNameTitle != null ? __instance.musicNameTitle.text : "(null)";
                string aText = __instance.artistNameTitle != null ? __instance.artistNameTitle.text : "(null)";
                MelonLogger.Msg($"PnlStage.RefreshDiffUI Postfix: musicNameTitle={mText}, artistNameTitle={aText}");
                return;
            }
            PnlStagePatchHelper.ApplyTagTitle("PnlStage.RefreshDiffUI", __instance);
            PnlStagePatchHelper.ForceApplyTagTitle("PnlStage.RefreshDiffUI.Force", __instance);

            MelonLogger.Msg($"[PnlStage.RefreshDiffUI.Postfix] exit uid={musicInfo?.uid ?? "(null)"}, currentSelectedUid={CustomPlaySession.Current.SelectedMusicUid}, experimentMode={CustomPlaySession.Current.IsExperimentModeActive}, shouldApply={CustomPlaySession.Current.ShouldApplyExperimentChart}");
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
