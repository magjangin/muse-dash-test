using MelonLoader;
using Il2Cpp;

namespace muse_dash_test
{
    // === 피버 메커니즘 차단 패치 (설정 파일 연동) ===

    [HarmonyLib.HarmonyPatch(typeof(Il2CppAssets.Scripts.GameCore.Managers.AbstractFeverManager), "AddFever", new System.Type[] { typeof(int) })]
    public class AbstractFeverManager_AddFever_Patch
    {
        public static bool Prefix(Il2CppAssets.Scripts.GameCore.Managers.AbstractFeverManager __instance, ref int value)
        {
            try
            {
                if (InputOverlay.blockFever)
                {
                    // 피버 게이지가 쌓이지 않도록 0으로 설정합니다.
                    value = 0;
                }
            }
            catch (System.Exception ex)
            {
                MelonLogger.Error($"[AbstractFeverManager.AddFever.Prefix] 예외 발생: {ex}");
            }
            return true;
        }
    }
}
