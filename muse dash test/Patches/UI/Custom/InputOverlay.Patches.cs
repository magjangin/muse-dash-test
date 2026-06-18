using System;
using MelonLoader;

namespace muse_dash_test
{
    // ==========================================
    // 키바인딩 수정 완료/취소 시 실시간 캐시 갱신 패치
    // ==========================================

    [HarmonyLib.HarmonyPatch(typeof(Il2CppUI.Panels.PnlInputs.PCInputModules.PnlInputKeyboard), "OnClickBtnCustomComplete", new Type[] { typeof(string) })]
    public static class PnlInputKeyboard_OnClickBtnCustomComplete_Patch
    {
        public static void Postfix(string keyName)
        {
            MelonLogger.Msg($"[InputOverlay.Hook] OnClickBtnCustomComplete 호출됨: keyName={keyName}");
            InputOverlay.ResetCache();
        }
    }

    [HarmonyLib.HarmonyPatch(typeof(Il2CppUI.Panels.PnlInputs.PCInputModules.PnlInputKeyboard), "OnCancelCustomize")]
    public static class PnlInputKeyboard_OnCancelCustomize_Patch
    {
        public static void Postfix()
        {
            MelonLogger.Msg("[InputOverlay.Hook] OnCancelCustomize 호출됨");
            InputOverlay.ResetCache();
        }
    }
}
