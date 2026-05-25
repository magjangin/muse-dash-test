using MelonLoader;
using System;
using Il2CppAssets.Scripts.UI.Panels;

// PnlBattle.GameStart 후킹
[HarmonyLib.HarmonyPatch(typeof(PnlBattle), "GameStart")]
public class PnlBattle_GameStart_Patch
{
    public static void Prefix(PnlBattle __instance)
    {
        try { }
        catch (Exception ex) { MelonLogger.Error($"PnlBattle.GameStart Prefix 예외: {ex}"); }
    }

    public static void Postfix(PnlBattle __instance)
    {
        try { }
        catch (Exception ex) { MelonLogger.Error($"PnlBattle.GameStart Postfix 예외: {ex}"); }
    }
}
