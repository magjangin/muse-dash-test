using MelonLoader;
using System;
using muse_dash_test;

// Il2Cpp.PnlPreparation OnEnable 후킹
[HarmonyLib.HarmonyPatch(typeof(Il2Cpp.PnlPreparation), "OnEnable")]
public class PnlPreparation_OnEnable_Patch
{
    public static void Postfix(Il2Cpp.PnlPreparation __instance)
    {
        try
        {
            // 2. 가상 곡 메뉴 BGM / 미리듣기 음악 전환
            string selectedUid = PnlStagePatchHelper.GetCurrentSelectedMusicUid();
            if (CustomContentIds.IsVirtualSong(selectedUid))
            {
                HwaMenuBgmController.TriggerMenuBgmChange(selectedUid);
            }

            // 3. 커스텀 레코드(점수/달성도%) UI 덮어쓰기
            CustomRecordUiPatchHelper.ApplyCustomRecordToPnlPreparation(__instance);
            MelonCoroutines.Start(CustomRecordUiPatchHelper.DelayedApplyPrep(__instance, 0.25f));
            MelonCoroutines.Start(CustomRecordUiPatchHelper.DelayedApplyPrep(__instance, 1.0f));

            // 1번 수동 텍스트 덮어쓰기(ApplyPrepMusicInfo)는 제거됨
        }
        catch (Exception ex)
        {
            MelonLogger.Error($"PnlPreparation.OnEnable Postfix 예외: {ex}");
        }
    }
}

// Il2Cpp.PnlPreparation.OnDownloadBestReport 후킹
[HarmonyLib.HarmonyPatch(typeof(Il2Cpp.PnlPreparation), "OnDownloadBestReport")]
public class PnlPreparation_OnDownloadBestReport_Patch
{
    public static void Postfix(Il2Cpp.PnlPreparation __instance)
    {
        try
        {
            CustomRecordUiPatchHelper.ApplyCustomRecordToPnlPreparation(__instance);
            MelonCoroutines.Start(CustomRecordUiPatchHelper.DelayedApplyPrep(__instance, 0.25f));
        }
        catch (Exception ex)
        {
            MelonLogger.Error($"PnlPreparation.OnDownloadBestReport Postfix 예외: {ex}");
        }
    }
}

// Il2Cpp.PnlPreparation RefreshUi 후킹
[HarmonyLib.HarmonyPatch(typeof(Il2Cpp.PnlPreparation), "RefreshUi")]
public class PnlPreparation_RefreshUi_Patch
{
    public static void Postfix(Il2Cpp.PnlPreparation __instance)
    {
        try
        {
            CustomRecordUiPatchHelper.ApplyCustomRecordToPnlPreparation(__instance);
            MelonCoroutines.Start(CustomRecordUiPatchHelper.DelayedApplyPrep(__instance, 0.25f));
        }
        catch (Exception ex)
        {
            MelonLogger.Error($"PnlPreparation.RefreshUi Postfix 예외: {ex}");
        }
    }
}
