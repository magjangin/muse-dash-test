using MelonLoader;
using System;

// MusicButtonAreaTitle.RefreshTxt 후킹
[HarmonyLib.HarmonyPatch(typeof(Il2Cpp.MusicButtonAreaTitle), "RefreshTxt", new Type[] { typeof(string), typeof(bool) })]
public class MusicButtonAreaTitle_RefreshTxt_Patch
{
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
            }
        }
        catch (Exception ex)
        {
            MelonLogger.Error($"MusicButtonAreaTitle.RefreshTxt Prefix 예외: {ex}");
        }
    }
}
