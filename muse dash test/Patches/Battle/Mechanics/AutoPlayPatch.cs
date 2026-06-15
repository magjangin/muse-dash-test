using System;
using MelonLoader;
using Il2CppAssets.Scripts.Database;

namespace muse_dash_test
{
    // === 오토플레이 설정 파일 연동 패치 ===

    [HarmonyLib.HarmonyPatch(typeof(DBSkill), "SetAutoPlay")]
    public class DBSkill_SetAutoPlay_Patch
    {
        public static void Prefix(DBSkill __instance, ref bool enable)
        {
            try
            {
                // 설정 파일에 적힌 오토플레이 강제 값으로 덮어씌웁니다.
                enable = InputOverlay.forceAutoPlay;
                MelonLogger.Msg($"[AutoPlayPatch] DBSkill.SetAutoPlay 강제 조작: enable -> {enable}");
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[DBSkill.SetAutoPlay.Prefix] Prefix 예외 발생: {ex}");
            }
        }
    }
}
