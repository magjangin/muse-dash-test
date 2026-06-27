using MelonLoader;
using System;
using muse_dash_test;

// 메인 메뉴/캐릭터·엘핀 선택 패널(PnlMenu)이 표시될 때, 곡 선택 화면에서 주입했던
// 커스텀 곡 미리듣기 BGM이 따라와 계속 재생되는 문제를 막기 위한 패치입니다.
[HarmonyLib.HarmonyPatch(typeof(Il2CppAssets.Scripts.UI.Panels.PnlMenu), "OnEnable")]
public class PnlMenu_OnEnable_Patch
{
    public static void Postfix(Il2CppAssets.Scripts.UI.Panels.PnlMenu __instance)
    {
        try
        {
            HwaMenuBgmController.StopCustomMenuBgm("PnlMenu.OnEnable");
        }
        catch (Exception ex)
        {
            MelonLogger.Error($"PnlMenu.OnEnable Postfix 예외: {ex}");
        }
    }
}
