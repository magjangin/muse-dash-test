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
                bool isExperimentMode = PnlStagePatchHelper.IsExperimentModeTitle(title);
                CustomPlaySession.Current.IsExperimentModeActive = isExperimentMode;
                ModLogger.Msg($"[MusicButtonAreaTitle] title='{title ?? "(null)"}', isExperimentMode={isExperimentMode}");
            }
        }
        catch (Exception ex)
        {
            ModLogger.Error($"MusicButtonAreaTitle.RefreshTxt Prefix 예외: {ex}");
        }
    }
}
