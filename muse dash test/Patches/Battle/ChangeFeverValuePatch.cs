using MelonLoader;
using Il2Cpp;

namespace muse_dash_test
{
    // === 진짜 피버 메커니즘 루트 레벨 제어 패치 (GeneralFeverManager 전용) ===

    /*
    [HarmonyLib.HarmonyPatch(typeof(Il2CppAssets.Scripts.GameCore.Managers.AbstractFeverManager), "AddFever", new System.Type[] { typeof(int) })]
    public class AbstractFeverManager_AddFever_Patch
    {
        public static bool Prefix(Il2CppAssets.Scripts.GameCore.Managers.AbstractFeverManager __instance, ref int value)
        {
            try
            {
                string typeName = __instance != null ? __instance.GetIl2CppType().FullName : "Unknown";
                if (typeName.Contains("GeneralFeverManager"))
                {
                    value = 999999; // 즉시 피버 게이지를 가득 채웁니다.
                }
            }
            catch (System.Exception ex)
            {
                MelonLogger.Error($"[AbstractFeverManager.AddFever.Prefix] 예외 발생: {ex}");
            }
            return true; // 원래 메서드를 정상 실행하여 게이지가 가득 차게 합니다.
        }
    }
    */
}
