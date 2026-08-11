using System;
using MelonLoader;
using UnityEngine;

namespace muse_dash_test
{
    // === 안 친 노트의 미스 페널티(체력 감소) 무효화 ===
    //
    // 실측으로 확인된 미스 처리 순서(전부 같은 프레임, 1~2ms 안에 붙어서 발생):
    //   TaskStageTarget.TriggerNoteMiss()          - 카운터는 건드리지 않음 (m_MissResult=0 유지)
    //   BattleRoleAttributeComponent.Miss()        - 체력 변화 없음 (연출/상태 처리로 추정)
    //   BattleRoleAttributeComponent.Hurt(-30,...) - 실제로 체력을 깎는 주체
    //   BattleEnemyManager.SetPlayResult(idx, Miss)- 여기서 우리가 Perfect로 승격
    //
    // 그래서 Miss()가 불린 프레임을 표식으로 잡고, 같은 프레임에 들어오는 Hurt만 무력화합니다.
    // 장애물 피격은 Miss() 없이 Hurt만 오므로 그대로 살아 있습니다. 무적이 아니라 "미스 데미지만 0"입니다.
    //
    // 참고: GameGlobal.MISS_NO_CHECK_TICK(원본 -5)을 999999로 밀어봤지만 미스는 그대로 발생했습니다.
    // 이 경로는 그 값을 참조하지 않으므로 해당 실험은 되돌렸습니다.

    internal static class MissPenaltyGate
    {
        private const double SummaryIntervalSeconds = 10.0;

        private static int missFrame = -1;
        private static int blockedCount;
        private static int blockedHpTotal;
        private static bool announced;
        private static DateTime lastSummaryTime = DateTime.MinValue;
        private static bool pendingSummary;

        /// <summary>미스 반응이 시작된 프레임을 기록합니다. 바로 뒤따르는 Hurt를 가려내는 표식입니다.</summary>
        internal static void MarkMissFrame()
        {
            missFrame = Time.frameCount;
        }

        internal static bool IsMissFrame()
        {
            return missFrame == Time.frameCount;
        }

        internal static void RecordBlocked(int hurtValue, int hp)
        {
            blockedCount++;
            blockedHpTotal += -hurtValue;
            pendingSummary = true;

            if (!announced)
            {
                announced = true;
                MelonLogger.Msg($"[ForcePerfect.MissPenalty] 미스 데미지 첫 차단: hurtValue={hurtValue} -> 0, HP={hp} 유지");
            }

            if ((DateTime.UtcNow - lastSummaryTime).TotalSeconds >= SummaryIntervalSeconds)
            {
                lastSummaryTime = DateTime.UtcNow;
                if (!pendingSummary) return;
                pendingSummary = false;
                MelonLogger.Msg($"[ForcePerfect.MissPenalty] 누적 차단: {blockedCount}회, 지켜낸 체력 {blockedHpTotal}");
            }
        }

        /// <summary>현재 체력을 안전하게 읽습니다. 전투 밖이면 -1을 돌려줍니다.</summary>
        internal static int CurrentHp()
        {
            try
            {
                var role = Il2CppAssets.Scripts.GameCore.HostComponent.BattleRoleAttributeComponent.instance;
                return role == null ? -1 : role.GetHp();
            }
            catch (Exception)
            {
                return -1;
            }
        }
    }

    /// <summary>
    /// 미스 반응의 시작점. 체력은 건드리지 않으므로 막지 않고, 프레임 표식만 남깁니다.
    /// (MISS 연출이 여기서 나오는지는 아직 미확인이라 의도적으로 통과시킵니다.)
    /// </summary>
    [HarmonyLib.HarmonyPatch(typeof(Il2CppAssets.Scripts.GameCore.HostComponent.BattleRoleAttributeComponent), GameBindings.BattleRoleAttributeComponent.Miss)]
    public class BattleRoleAttributeComponent_Miss_MissPenalty_Patch
    {
        public static void Prefix()
        {
            try
            {
                if (!ForcePerfectState.Enabled) return;
                MissPenaltyGate.MarkMissFrame();
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[BattleRoleAttributeComponent.Miss.Prefix] 미스 프레임 표식 중 예외 발생: {ex}");
            }
        }
    }

    /// <summary>실제 데미지 지점. 미스와 같은 프레임일 때만 피해량을 0으로 만듭니다.</summary>
    [HarmonyLib.HarmonyPatch(typeof(Il2CppAssets.Scripts.GameCore.HostComponent.BattleRoleAttributeComponent), GameBindings.BattleRoleAttributeComponent.Hurt)]
    public class BattleRoleAttributeComponent_Hurt_MissPenalty_Patch
    {
        public static void Prefix(ref int hurtValue, bool isAir)
        {
            try
            {
                if (!ForcePerfectState.Enabled) return;
                if (hurtValue == 0) return;
                if (!MissPenaltyGate.IsMissFrame()) return; // 장애물 피격 등 미스와 무관한 데미지는 그대로 둡니다.

                int original = hurtValue;
                hurtValue = 0;
                MissPenaltyGate.RecordBlocked(original, MissPenaltyGate.CurrentHp());
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[BattleRoleAttributeComponent.Hurt.Prefix] 미스 데미지 차단 중 예외 발생: {ex}");
            }
        }
    }
}
