using MelonLoader;
using System;

// Il2Cpp.PnlPreparation Awake 후킹
[HarmonyLib.HarmonyPatch(typeof(Il2Cpp.PnlPreparation), "Awake")]
public class PnlPreparation_Awake_Patch
{
    public static void Prefix(Il2Cpp.PnlPreparation __instance)
    {
        try { }
        catch (Exception ex)
        {
            MelonLogger.Error($"PnlPreparation.Awake Prefix 예외: {ex}");
        }
    }

    public static void Postfix(Il2Cpp.PnlPreparation __instance)
    {
        try
        {
            PnlMusicUtils.LogPreparationMusicInfo(__instance);
            MelonCoroutines.Start(PnlMusicUtils.LogPreparationMusicInfoAfterDelay(__instance, "PnlPreparation.Awake.Delay", 0.25f));
            MelonCoroutines.Start(PnlMusicUtils.LogPreparationMusicInfoAfterDelay(__instance, "PnlPreparation.Awake.DelayLong", 1.0f));
        }
        catch (Exception ex)
        {
            MelonLogger.Error($"PnlPreparation.Awake Postfix 예외: {ex}");
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
            PnlMusicUtils.LogPreparationMusicInfo(__instance, "PnlPreparation.OnBattleStart");
        }
        catch (Exception ex)
        {
            MelonLogger.Error($"PnlPreparation.OnBattleStart Postfix 예외: {ex}");
        }
    }
}
