using MelonLoader;
using System;
using System.Reflection;
using Il2CppAssets.Scripts.UI.Panels;

// PnlBattle.GameStart 호출 로그만 남기는 보조 패치
[HarmonyLib.HarmonyPatch(typeof(PnlBattle), muse_dash_test.GameBindings.PnlBattle.GameStart)]
public class PnlBattle_GameStart_Patch
{
    public static void Postfix(PnlBattle __instance)
    {
        MelonLogger.Msg($"[PnlBattle.GameStart] 호출됨: {__instance}");
    }
}