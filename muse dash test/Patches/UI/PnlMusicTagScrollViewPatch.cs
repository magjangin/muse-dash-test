using MelonLoader;
using System;
using Il2CppAssets.Scripts.UI.Panels;

// PnlMusicTagScrollView.InitListView 테스트 후킹 패치
[HarmonyLib.HarmonyPatch(typeof(Il2Cpp.PnlMusicTagScrollView), nameof(Il2Cpp.PnlMusicTagScrollView.InitListView))]
public class PnlMusicTagScrollView_InitListView_Patch
{
    public static void Prefix(Il2Cpp.PnlMusicTagScrollView __instance, int itemTotalCount)
    {
        try
        {
            MelonLogger.Msg($"[★ PnlMusicTagScrollView.InitListView 후킹 성공! ★] 호출됨 | itemTotalCount={itemTotalCount}");
        }
        catch (Exception ex)
        {
            MelonLogger.Error($"PnlMusicTagScrollView.InitListView Prefix 예외: {ex}");
        }
    }
}

// PnlMusicTagViewItem.cacheAreaTitle getter 후킹
[HarmonyLib.HarmonyPatch(typeof(Il2Cpp.PnlMusicTagViewItem), "cacheAreaTitle", HarmonyLib.MethodType.Getter)]
public class PnlMusicTagViewItem_GetCacheAreaTitle_Patch
{
    public static bool Prepare()
    {
        MelonLogger.Msg("[PnlMusicTagViewItem.get_cacheAreaTitle] 접근자 후킹 준비 완료");
        return true;
    }

    public static void Postfix(Il2Cpp.PnlMusicTagViewItem __instance, ref Il2Cpp.MusicButtonAreaTitle __result)
    {
        try
        {
            if (__instance != null && __result != null)
            {
                string gameObjectName = __instance.gameObject != null ? __instance.gameObject.name : "(null)";
                string titleText = __result.title;
                MelonLogger.Msg($"[PnlMusicTagViewItem.get_cacheAreaTitle] GameObject={gameObjectName}, MusicButtonAreaTitle.title='{titleText}'");
            }
        }
        catch (Exception ex)
        {
            MelonLogger.Error($"PnlMusicTagViewItem.get_cacheAreaTitle Postfix 예외: {ex}");
        }
    }
}
