using MelonLoader;
using System;
using muse_dash_test;

// MusicButtonAreaTitle.RefreshTxt 후킹
[HarmonyLib.HarmonyPatch(typeof(Il2Cpp.MusicButtonAreaTitle), "RefreshTxt", new Type[] { typeof(string), typeof(bool) })]
public class MusicButtonAreaTitle_RefreshTxt_Patch
{
    public static void Prefix(Il2Cpp.MusicButtonAreaTitle __instance, ref string title, ref bool isSpecialFont)
    {
        try
        {
            if (__instance != null)
            {
                bool isExperimentMode = title == "실험 모드" || title == "Experiment Mod" || title == "实验模式" || title == "實驗模式" || title == "実験モード";
                CustomPlaySession.Current.IsExperimentModeActive = isExperimentMode;
                MelonLogger.Msg($"[MusicButtonAreaTitle] title='{title ?? "(null)"}', isExperimentMode={isExperimentMode}");
            }
        }
        catch (Exception ex)
        {
            MelonLogger.Error($"MusicButtonAreaTitle.RefreshTxt Prefix 예외: {ex}");
        }
    }
}
