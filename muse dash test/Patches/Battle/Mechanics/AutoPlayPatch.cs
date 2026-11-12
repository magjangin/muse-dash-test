using System;
using MelonLoader;
using Il2CppAssets.Scripts.Database;

namespace muse_dash_test
{
    // === 오토플레이 설정 파일 연동 패치 ===
    // 게임이 SetAutoPlay를 호출할 때마다 인자를 InputOverlay.forceAutoPlay로 덮어씁니다.
    // forceAutoPlay는 모드 로드 직후 항상 false이므로 오토는 꺼진 상태로 시작하며,
    // 게임 도중 config.txt의 '오토플레이' 값을 저장하면 그때부터 그 값이 적용됩니다.

    [HarmonyLib.HarmonyPatch(typeof(DBSkill), nameof(DBSkill.SetAutoPlay))]
    public class DBSkill_SetAutoPlay_Patch
    {
        public static void Prefix(DBSkill __instance, ref bool enable)
        {
            if (!ModConfig.EnableAutoPlay) return;
            try
            {
                // 설정 파일에 적힌 오토플레이 강제 값으로 덮어씌웁니다.
                // 게임이 배틀마다 여러 번 부르므로, 값을 실제로 바꾼 경우만 Info로 남깁니다.
                bool original = enable;
                enable = InputOverlay.forceAutoPlay;
                if (original != enable)
                {
                    ModLogger.Msg($"[AutoPlayPatch] DBSkill.SetAutoPlay 인자 덮어씀: {original} -> {enable}");
                }
                else
                {
                    ModLogger.Verbose($"[AutoPlayPatch] DBSkill.SetAutoPlay 호출: enable={enable} (설정값과 같아 그대로)");
                }
            }
            catch (Exception ex)
            {
                ModLogger.Error($"[DBSkill.SetAutoPlay.Prefix] Prefix 예외 발생: {ex}");
            }
        }

        public static void Postfix(DBSkill __instance, bool enable)
        {
            try
            {
                ModLogger.Verbose($"[AutoPlayPatch] DBSkill.SetAutoPlay Postfix 최종 설정 적용 완료: enable={enable}, instance={__instance}");
            }
            catch (Exception ex)
            {
                ModLogger.Error($"[DBSkill.SetAutoPlay.Postfix] Postfix 예외 발생: {ex}");
            }
        }
    }
}
