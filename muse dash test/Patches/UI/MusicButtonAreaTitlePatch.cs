using MelonLoader;
using System;

// MusicButtonAreaTitle.RefreshTxt 후킹
[HarmonyLib.HarmonyPatch(typeof(Il2Cpp.MusicButtonAreaTitle), "RefreshTxt", new Type[] { typeof(string), typeof(bool) })]
public class MusicButtonAreaTitle_RefreshTxt_Patch
{
    public static bool IsExperimentModActive = false;

    public static bool Prepare()
    {
        MelonLogger.Msg("[MusicButtonAreaTitle.RefreshTxt] 후킹 준비 완료");
        return true;
    }

    public static void Prefix(Il2Cpp.MusicButtonAreaTitle __instance, ref string title, ref bool isSpecialFont)
    {
        try
        {
            if (__instance != null)
            {
                string gameObjectName = __instance.gameObject != null ? __instance.gameObject.name : "(null)";
                MelonLogger.Msg($"[MusicButtonAreaTitle.RefreshTxt] Prefix 호출됨! GameObject={gameObjectName} | 원본 title='{title}' | isSpecialFont={isSpecialFont}");
                
                // 다국어 지원을 포함해 "실험 모드"가 설정되었는지 판단하여 플래그 갱신
                IsExperimentModActive = (title == "실험 모드" || title == "Experiment Mod" || title == "实验模式" || title == "實驗模式" || title == "実験モード");
                MelonLogger.Msg($"[MusicButtonAreaTitle.RefreshTxt] 실험 모드 활성화 여부 갱신: {IsExperimentModActive}");
            }
        }
        catch (Exception ex)
        {
            MelonLogger.Error($"MusicButtonAreaTitle.RefreshTxt Prefix 예외: {ex}");
        }
    }
}
