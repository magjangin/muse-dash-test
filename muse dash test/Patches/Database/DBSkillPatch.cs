using MelonLoader;
using Il2CppAssets.Scripts.Database;

namespace muse_dash_test
{
    [HarmonyLib.HarmonyPatch(typeof(DBSkill), nameof(DBSkill.AwakeInit))]
    public class DBSkill_AwakeInit_Patch
    {
        public static void Postfix(DBSkill __instance)
        {
            try
            {
                MelonLogger.Msg($"[DBSkill.AwakeInit] DBSkill.AwakeInit() 완료 감지: instance={__instance}");
            }
            catch (System.Exception ex)
            {
                MelonLogger.Error($"[DBSkill.AwakeInit] Postfix 예외 발생: {ex}");
            }
        }
    }
}
