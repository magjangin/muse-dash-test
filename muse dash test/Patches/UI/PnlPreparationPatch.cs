using MelonLoader;
using System;

// Il2Cpp.PnlPreparation OnEnable 후킹
[HarmonyLib.HarmonyPatch(typeof(Il2Cpp.PnlPreparation), "OnEnable")]
public class PnlPreparation_OnEnable_Patch
{
    public static void Prefix(Il2Cpp.PnlPreparation __instance)
    {
        try
        {
            if (__instance != null)
            {
                string designerText = PnlStagePatchHelper.GetLongNameControllerText(__instance.designerLongNameController);
                string artistText = PnlStagePatchHelper.GetLongNameControllerText(__instance.songAuthorLongNameController);
                string achvText = __instance.stageAchievementValue != null ? __instance.stageAchievementValue.text : "(null)";

            }
        }
        catch (Exception ex)
        {
            MelonLogger.Error($"PnlPreparation.OnEnable Prefix 로그 예외: {ex}");
        }
    }

    public static void Postfix(Il2Cpp.PnlPreparation __instance)
    {
        try
        {
            if (!global::muse_dash_test.UiFeatureFlags.IsUiOverridesEnabled())
            {
                return;
            }

            if (__instance != null)
            {
                string designerText = PnlStagePatchHelper.GetLongNameControllerText(__instance.designerLongNameController);
                string artistText = PnlStagePatchHelper.GetLongNameControllerText(__instance.songAuthorLongNameController);
                string achvText = __instance.stageAchievementValue != null ? __instance.stageAchievementValue.text : "(null)";

            }

            PnlMusicUtils.LogPreparationMusicInfo(__instance, "PnlPreparation.OnEnable");
            MelonCoroutines.Start(PnlMusicUtils.LogPreparationMusicInfoAfterDelay(__instance, "PnlPreparation.OnEnable.Delay", 0.25f));
            MelonCoroutines.Start(PnlMusicUtils.LogPreparationMusicInfoAfterDelay(__instance, "PnlPreparation.OnEnable.DelayLong", 1.0f));
        }
        catch (Exception ex)
        {
            MelonLogger.Error($"PnlPreparation.OnEnable Postfix 예외: {ex}");
        }
    }
}

// Il2Cpp.PnlPreparation RefreshUi 후킹
[HarmonyLib.HarmonyPatch(typeof(Il2Cpp.PnlPreparation), "RefreshUi")]
public class PnlPreparation_RefreshUi_Patch
{
    public static void Prefix(Il2Cpp.PnlPreparation __instance)
    {
        try
        {
            if (__instance != null)
            {
                string designerText = PnlStagePatchHelper.GetLongNameControllerText(__instance.designerLongNameController);
                string artistText = PnlStagePatchHelper.GetLongNameControllerText(__instance.songAuthorLongNameController);
                string achvText = __instance.stageAchievementValue != null ? __instance.stageAchievementValue.text : "(null)";

            }
        }
        catch (Exception ex)
        {
            MelonLogger.Error($"PnlPreparation.RefreshUi Prefix 로그 예외: {ex}");
        }
    }

    public static void Postfix(Il2Cpp.PnlPreparation __instance)
    {
        try
        {
            if (!global::muse_dash_test.UiFeatureFlags.IsUiOverridesEnabled())
            {
                return;
            }

            if (__instance != null)
            {
                string designerText = PnlStagePatchHelper.GetLongNameControllerText(__instance.designerLongNameController);
                string artistText = PnlStagePatchHelper.GetLongNameControllerText(__instance.songAuthorLongNameController);
                string achvText = __instance.stageAchievementValue != null ? __instance.stageAchievementValue.text : "(null)";

            }

            PnlMusicUtils.LogPreparationMusicInfo(__instance, "PnlPreparation.RefreshUi");
        }
        catch (Exception ex)
        {
            MelonLogger.Error($"PnlPreparation.RefreshUi Postfix 예외: {ex}");
        }
    }
}

// Il2Cpp.PnlPreparation.GameStart 후킹
[HarmonyLib.HarmonyPatch(typeof(Il2Cpp.PnlPreparation), "GameStart")]
public class PnlPreparation_GameStart_Patch
{
    public static void Postfix(Il2Cpp.PnlPreparation __instance)
    {
        try
        {
            if (!global::muse_dash_test.UiFeatureFlags.IsUiOverridesEnabled())
            {
                return;
            }

            if (__instance != null)
            {
                string designerText = PnlStagePatchHelper.GetLongNameControllerText(__instance.designerLongNameController);
                string artistText = PnlStagePatchHelper.GetLongNameControllerText(__instance.songAuthorLongNameController);
                string achvText = __instance.stageAchievementValue != null ? __instance.stageAchievementValue.text : "(null)";

            }

            PnlMusicUtils.LogPreparationMusicInfo(__instance, "PnlPreparation.GameStart");
        }
        catch (Exception ex)
        {
            MelonLogger.Error($"PnlPreparation.GameStart Postfix 예외: {ex}");
        }
    }
}

// Il2Cpp.PnlPreparation.OnBattleStart 후킹
[HarmonyLib.HarmonyPatch(typeof(Il2Cpp.PnlPreparation), "OnBattleStart")]
public class PnlPreparation_OnBattleStart_Patch
{
    public static void Postfix(Il2Cpp.PnlPreparation __instance)
    {
        try
        {
            if (!global::muse_dash_test.UiFeatureFlags.IsUiOverridesEnabled())
            {
                return;
            }

            if (__instance != null)
            {
                string designerText = PnlStagePatchHelper.GetLongNameControllerText(__instance.designerLongNameController);
                string artistText = PnlStagePatchHelper.GetLongNameControllerText(__instance.songAuthorLongNameController);
                string achvText = __instance.stageAchievementValue != null ? __instance.stageAchievementValue.text : "(null)";

            }

            PnlMusicUtils.LogPreparationMusicInfo(__instance, "PnlPreparation.OnBattleStart");
        }
        catch (Exception ex)
        {
            MelonLogger.Error($"PnlPreparation.OnBattleStart Postfix 예외: {ex}");
        }
    }
}