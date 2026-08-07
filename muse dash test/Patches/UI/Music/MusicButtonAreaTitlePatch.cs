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
                
                // 상태가 바뀔 때만 1회 로그를 남겨 스크롤 로깅 폭발 없이 명확히 띄웁니다.
                if (CustomPlaySession.Current.IsExperimentModeActive != isExperimentMode)
                {
                    CustomPlaySession.Current.IsExperimentModeActive = isExperimentMode;
                    MelonLogger.Msg($"🧪 [ExperimentMode] 실험 모드 상태 변경: title='{title}', active={isExperimentMode}");
                }
            }
        }
        catch (Exception ex)
        {
            MelonLogger.Error($"MusicButtonAreaTitle.RefreshTxt Prefix 예외: {ex}");
        }
    }
}
