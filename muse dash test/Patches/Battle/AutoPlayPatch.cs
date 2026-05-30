using MelonLoader;
using Il2CppAssets.Scripts.Database;
using Il2CppAssets.Scripts.GameCore.Managers;
using Il2CppGameLogic;
using Il2CppFormulaBase;

namespace muse_dash_test
{
    [HarmonyLib.HarmonyPatch(typeof(DBSkill), "SetAutoPlay")]
    public class DBSkill_SetAutoPlay_Patch
    {
        public static void Prefix(DBSkill __instance, ref bool enable)
        {
            try
            {
                MelonLogger.Msg($"[DBSkill.SetAutoPlay.Prefix] 호출 감지: 기존 인자 enable={enable}");
            }
            catch (System.Exception ex)
            {
                MelonLogger.Error($"[DBSkill.SetAutoPlay.Prefix] Prefix 예외 발생: {ex}");
            }
        }

        public static void Postfix(DBSkill __instance, bool enable)
        {
            try
            {
                MelonLogger.Msg($"[DBSkill.SetAutoPlay.Postfix] 최종 설정 적용완료: enable={enable}, instance={__instance}");
            }
            catch (System.Exception ex)
            {
                MelonLogger.Error($"[DBSkill.SetAutoPlay.Postfix] Postfix 예외 발생: {ex}");
            }
        }
    }

}
