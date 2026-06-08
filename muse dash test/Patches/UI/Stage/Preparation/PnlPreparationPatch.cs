using MelonLoader;
using System;
using System.Reflection;

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

// Il2Cpp.PnlPreparation.OnDownloadBestReport 후킹
[HarmonyLib.HarmonyPatch(typeof(Il2Cpp.PnlPreparation), "OnDownloadBestReport")]
public class PnlPreparation_OnDownloadBestReport_Patch
{
    public static void Prefix(Il2Cpp.PnlPreparation __instance)
    {
        try
        {
            MelonLogger.Msg($"[PnlPreparation.OnDownloadBestReport.Prefix] 호출 감지: instance={(__instance != null ? __instance.ToString() : "null")}");
            DumpRecordContext(__instance, "Prefix");
        }
        catch (Exception ex)
        {
            MelonLogger.Error($"PnlPreparation.OnDownloadBestReport Prefix 예외: {ex}");
        }
    }

    public static void Postfix(Il2Cpp.PnlPreparation __instance)
    {
        try
        {
            MelonLogger.Msg($"[PnlPreparation.OnDownloadBestReport.Postfix] 처리 완료: instance={(__instance != null ? __instance.ToString() : "null")}");
            DumpRecordContext(__instance, "Postfix");
            PnlMusicUtils.LogPreparationMusicInfo(__instance, "PnlPreparation.OnDownloadBestReport");
            MelonCoroutines.Start(PnlMusicUtils.LogPreparationMusicInfoAfterDelay(__instance, "PnlPreparation.OnDownloadBestReport.Delay", 0.25f));
        }
        catch (Exception ex)
        {
            MelonLogger.Error($"PnlPreparation.OnDownloadBestReport Postfix 예외: {ex}");
        }
    }

    private static void DumpRecordContext(Il2Cpp.PnlPreparation __instance, string phase)
    {
        try
        {
            string selectedUid = PnlStagePatchHelper.GetCurrentSelectedMusicUid();
            if (string.IsNullOrEmpty(selectedUid))
            {
                selectedUid = muse_dash_test.MusicButtonCell_OnButtonClicked_Patch.LastClickedMusicUid;
            }

            MelonLogger.Msg($"[PnlPreparation.OnDownloadBestReport.{phase}] selectedUid={selectedUid ?? "(null)"}");

            if (__instance == null)
            {
                return;
            }

            Type type = __instance.GetType();
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var field in fields)
            {
                string fieldName = field.Name ?? string.Empty;
                if (fieldName.IndexOf("record", StringComparison.OrdinalIgnoreCase) < 0 &&
                    fieldName.IndexOf("score", StringComparison.OrdinalIgnoreCase) < 0 &&
                    fieldName.IndexOf("uid", StringComparison.OrdinalIgnoreCase) < 0 &&
                    fieldName.IndexOf("music", StringComparison.OrdinalIgnoreCase) < 0 &&
                    fieldName.IndexOf("best", StringComparison.OrdinalIgnoreCase) < 0 &&
                    fieldName.IndexOf("difficulty", StringComparison.OrdinalIgnoreCase) < 0 &&
                    fieldName.IndexOf("result", StringComparison.OrdinalIgnoreCase) < 0)
                {
                    continue;
                }

                object value;
                try
                {
                    value = field.GetValue(__instance);
                }
                catch (Exception ex)
                {
                    value = $"(error: {ex.Message})";
                }

                MelonLogger.Msg($"[PnlPreparation.OnDownloadBestReport.{phase}] field {fieldName}={value ?? "(null)"}");
            }

            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var property in properties)
            {
                string propertyName = property.Name ?? string.Empty;
                if (!property.CanRead || property.GetIndexParameters().Length != 0)
                {
                    continue;
                }

                if (propertyName.IndexOf("record", StringComparison.OrdinalIgnoreCase) < 0 &&
                    propertyName.IndexOf("score", StringComparison.OrdinalIgnoreCase) < 0 &&
                    propertyName.IndexOf("uid", StringComparison.OrdinalIgnoreCase) < 0 &&
                    propertyName.IndexOf("music", StringComparison.OrdinalIgnoreCase) < 0 &&
                    propertyName.IndexOf("best", StringComparison.OrdinalIgnoreCase) < 0 &&
                    propertyName.IndexOf("difficulty", StringComparison.OrdinalIgnoreCase) < 0 &&
                    propertyName.IndexOf("result", StringComparison.OrdinalIgnoreCase) < 0)
                {
                    continue;
                }

                object value;
                try
                {
                    value = property.GetValue(__instance);
                }
                catch (Exception ex)
                {
                    value = $"(error: {ex.Message})";
                }

                MelonLogger.Msg($"[PnlPreparation.OnDownloadBestReport.{phase}] property {propertyName}={value ?? "(null)"}");
            }
        }
        catch (Exception ex)
        {
            MelonLogger.Error($"PnlPreparation.OnDownloadBestReport {phase} 기록 컨텍스트 덤프 예외: {ex}");
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
            if (__instance != null)
            {
                string designerText = PnlStagePatchHelper.GetLongNameControllerText(__instance.designerLongNameController);
                string artistText = PnlStagePatchHelper.GetLongNameControllerText(__instance.songAuthorLongNameController);
                string achvText = __instance.stageAchievementValue != null ? __instance.stageAchievementValue.text : "(null)";

            }

            PnlMusicUtils.LogPreparationMusicInfo(__instance, "PnlPreparation.RefreshUi");
            MelonCoroutines.Start(PnlMusicUtils.LogPreparationMusicInfoAfterDelay(__instance, "PnlPreparation.RefreshUi.Delay", 0.25f));
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
