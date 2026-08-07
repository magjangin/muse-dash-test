using MelonLoader;
using System;
using muse_dash_test;

// MusicButtonAreaTitle.RefreshTxt 후킹 - 실험 모드 상태를 테스트를 위해 강제로 false로 설정합니다.
[HarmonyLib.HarmonyPatch(typeof(Il2Cpp.MusicButtonAreaTitle), "RefreshTxt", new Type[] { typeof(string), typeof(bool) })]
public class MusicButtonAreaTitle_RefreshTxt_Patch
{
    public static void Prefix(Il2Cpp.MusicButtonAreaTitle __instance, ref string title, ref bool isSpecialFont)
    {
        try
        {
            if (__instance != null)
            {
                bool previous = CustomPlaySession.Current.IsExperimentModeActive;
                CustomPlaySession.Current.IsExperimentModeActive = false; // 강제 false 고정
                
                if (previous != false)
                {
                    MelonLogger.Msg($"🧪 [ExperimentMode] 테스트를 위해 실험 모드 상태를 강제 false로 설정했습니다. (title='{title}')");
                }
            }
        }
        catch (Exception ex)
        {
            MelonLogger.Error($"MusicButtonAreaTitle.RefreshTxt Prefix 예외: {ex}");
        }
    }
}
