using System;
using MelonLoader;

namespace muse_dash_test
{
    // === 미스 판정 구간(GameGlobal.MISS_NO_CHECK_TICK) 실험 ===
    // 미스가 난 뒤에 수습하는 대신, 애초에 미스가 성립하지 않게 만드는 방향입니다.
    // 이름대로 "미스를 체크하지 않는 틱 구간"이라면 값을 키울수록 안 친 노트가 미스로 잡히지 않고,
    // 그러면 MISS 연출도 체력 감소도 통째로 발생하지 않습니다.
    //
    // 아직 의미가 확인된 값이 아니므로 순서를 지킵니다.
    //   1. 배틀 시작 시 원래 값을 먼저 로그로 남긴다 (강제퍼펙트가 꺼져 있어도 항상 기록)
    //   2. 강제퍼펙트가 켜져 있을 때만 덮어쓴다
    //   3. 꺼지면 원래 값으로 되돌린다 (전역 static이라 이 판만 바뀌고 끝나도록)

    internal static class MissWindowOverride
    {
        /// <summary>실험용 대체값. 미스 체크 구간을 사실상 무한대로 밀어버립니다.</summary>
        private const int OverrideTicks = 999999;

        private static bool captured;
        private static Il2CppSystem.Decimal originalValue;
        private static bool applied;

        /// <summary>원래 값을 최초 1회만 보관합니다. 되돌릴 기준점이 됩니다.</summary>
        private static void CaptureOriginal()
        {
            if (captured) return;
            originalValue = Il2CppGameLogic.GameGlobal.MISS_NO_CHECK_TICK;
            captured = true;
            MelonLogger.Msg($"[ForcePerfect.MissWindow] GameGlobal.MISS_NO_CHECK_TICK 원본 값 확보: {originalValue}");
        }

        /// <summary>
        /// 강제퍼펙트 설정 상태에 맞춰 덮어쓰기/되돌리기를 맞춥니다.
        /// 배틀이 새로 시작될 때는 게임이 이 전역 값을 자체적으로 되돌렸을 수 있으므로
        /// <paramref name="force"/>로 플래그와 무관하게 다시 씁니다.
        /// </summary>
        internal static void Sync(bool force = false)
        {
            try
            {
                CaptureOriginal();

                bool shouldApply = ForcePerfectState.Enabled;
                if (!force && shouldApply == applied) return;

                if (shouldApply)
                {
                    Il2CppGameLogic.GameGlobal.MISS_NO_CHECK_TICK = new Il2CppSystem.Decimal(OverrideTicks);
                    applied = true;
                    MelonLogger.Msg($"[ForcePerfect.MissWindow] MISS_NO_CHECK_TICK 덮어쓰기 적용: {originalValue} -> {Il2CppGameLogic.GameGlobal.MISS_NO_CHECK_TICK}");
                }
                else
                {
                    Il2CppGameLogic.GameGlobal.MISS_NO_CHECK_TICK = originalValue;
                    applied = false;
                    MelonLogger.Msg($"[ForcePerfect.MissWindow] MISS_NO_CHECK_TICK 원본 값으로 복원: {Il2CppGameLogic.GameGlobal.MISS_NO_CHECK_TICK}");
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[ForcePerfect.MissWindow] MISS_NO_CHECK_TICK 처리 중 예외 발생: {ex}");
            }
        }

        /// <summary>배틀 시작 시점의 실제 값을 그대로 남깁니다. 게임이 판마다 값을 되돌리는지 확인용입니다.</summary>
        internal static void LogCurrent(string context)
        {
            try
            {
                MelonLogger.Msg($"[ForcePerfect.MissWindow] {context} 시점 MISS_NO_CHECK_TICK={Il2CppGameLogic.GameGlobal.MISS_NO_CHECK_TICK} " +
                                $"(강제퍼펙트={ForcePerfectState.Enabled}, 덮어쓰기 적용={applied})");
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[ForcePerfect.MissWindow] 값 로깅 중 예외 발생: {ex}");
            }
        }
    }

    /// <summary>배틀이 시작될 때 값을 확인하고, 설정에 맞춰 덮어쓰기를 걸어둡니다.</summary>
    [HarmonyLib.HarmonyPatch(typeof(Il2CppFormulaBase.StageBattleComponent), GameBindings.StageBattleComponent.GameStart)]
    public class StageBattleComponent_GameStart_MissWindow_Patch
    {
        public static void Postfix()
        {
            MissWindowOverride.LogCurrent("GameStart 직후(덮어쓰기 전)");
            MissWindowOverride.Sync(force: true);
        }
    }
}
