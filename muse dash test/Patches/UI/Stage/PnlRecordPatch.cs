using MelonLoader;
using System;
using muse_dash_test;

[HarmonyLib.HarmonyPatch(typeof(Il2Cpp.PnlRecord), "RefreshRecord")]
public class PnlRecord_RefreshRecord_Patch
{
    public static void Prefix(Il2Cpp.PnlRecord __instance)
    {
        try
        {
            MelonLogger.Msg($"[PnlRecord.RefreshRecord.Prefix] 호출 감지: instance={(__instance != null ? __instance.ToString() : "null")}");
        }
        catch (Exception ex)
        {
            MelonLogger.Error($"PnlRecord.RefreshRecord Prefix 예외: {ex}");
        }
    }

    public static void Postfix(Il2Cpp.PnlRecord __instance)
    {
        try
        {
            MelonLogger.Msg($"[PnlRecord.RefreshRecord.Postfix] 처리 완료: instance={(__instance != null ? __instance.ToString() : "null")}");
            CustomRecordUiPatchHelper.ApplyCustomRecordToPnlRecord(__instance);
        }
        catch (Exception ex)
        {
            MelonLogger.Error($"PnlRecord.RefreshRecord Postfix 예외: {ex}");
        }
    }
}
