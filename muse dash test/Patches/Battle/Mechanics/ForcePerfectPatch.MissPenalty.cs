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

        private static int missSkippedCount;

        /// <summary>미스 반응(Miss) 자체를 건너뛴 횟수. 연출이 실제로 잠잠해졌는지 대조용입니다.</summary>
        internal static void RecordMissSkipped()
        {
            missSkippedCount++;
            pendingSummary = true;
            MaybeLogSummary();
        }

        internal static void RecordBlocked(int hurtValue, int hp)
        {
            blockedCount++;
            blockedHpTotal += -hurtValue;
            pendingSummary = true;

            if (!announced)
            {
                announced = true;
                MelonLogger.Msg($"[ForcePerfect.MissPenalty] 미스 데미지 첫 차단: Hurt(hurtValue={hurtValue}) 호출 자체를 건너뜀, HP={hp} 유지");
            }

            MaybeLogSummary();
        }

        private static void MaybeLogSummary()
        {
            if ((DateTime.UtcNow - lastSummaryTime).TotalSeconds < SummaryIntervalSeconds) return;

            lastSummaryTime = DateTime.UtcNow;
            if (!pendingSummary) return;
            pendingSummary = false;
            MelonLogger.Msg($"[ForcePerfect.MissPenalty] 누적 차단: 데미지 {blockedCount}회(지켜낸 체력 {blockedHpTotal}), 미스 반응 건너뜀 {missSkippedCount}회");
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

        private static int lastWatchedHp = int.MinValue;

        /// <summary>
        /// 매 프레임 체력을 확인해 값이 바뀐 순간만 로그로 남깁니다.
        /// Miss()/Hurt()를 모두 건너뛰었는데도 체력이 줄면, 깎는 주체가 그 둘이 아니라는 뜻이므로
        /// 그 프레임이 미스 프레임인지 여부와 함께 기록해 범인을 좁힙니다.
        /// </summary>
        internal static void WatchHp()
        {
            if (!ForcePerfectState.Enabled)
            {
                lastWatchedHp = int.MinValue;
                return;
            }

            int hp = CurrentHp();
            if (hp < 0)
            {
                lastWatchedHp = int.MinValue;
                return;
            }

            if (lastWatchedHp == int.MinValue)
            {
                lastWatchedHp = hp;
                return;
            }

            if (hp == lastWatchedHp) return;

            int delta = hp - lastWatchedHp;
            MelonLogger.Msg($"[ForcePerfect.HpWatch] 체력 변화 감지: {lastWatchedHp} -> {hp} (delta={delta}, frame={Time.frameCount}, 미스프레임={IsMissFrame()}, 직전미스프레임={missFrame})");
            lastWatchedHp = hp;
        }
    }

    /// <summary>
    /// 안 친 노트를 집계에 "퍼펙트로 처리된 노트"로 등록합니다.
    ///
    /// 실측: 안 친 노트는 <c>BattleEnemyManager.SetPlayResult</c>만 거치고
    /// <c>TaskStageTarget.SetPlayResult</c>는 아예 호출되지 않습니다. 그래서 노트별 저장값은
    /// Prefect로 바뀌어도 카운터(m_PerfectResult)는 그대로고, 결과창은 그 노트를 "판정되지 않은 나머지"로
    /// 계산해 MISS로 표시합니다(총 노트 412 - Perfect 189 = MISS 223).
    /// 따라서 집계 진입점을 우리가 대신 한 번 호출해 줍니다.
    /// </summary>
    internal static class MissRegistration
    {
        private static bool reentrant;
        private static int registered;
        private static bool announced;

        internal static void RegisterAsPerfect(int idx)
        {
            if (reentrant) return;

            try
            {
                var target = muse_dash_test.Patches.VictoryDataCache.ActiveTarget;
                if (target == null)
                {
                    target = Il2CppAssets.Scripts.GameCore.HostComponent.TaskStageTarget.instance;
                }
                if (target == null) return;

                reentrant = true;
                target.SetPlayResult(idx, ForcePerfectState.Perfect);
                registered++;

                if (!announced)
                {
                    announced = true;
                    MelonLogger.Msg($"[ForcePerfect.MissRegistration] 안 친 노트를 집계에 퍼펙트로 첫 등록: idx={idx}");
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[ForcePerfect.MissRegistration] 집계 등록 중 예외 발생: {ex}");
            }
            finally
            {
                reentrant = false;
            }
        }

        internal static int RegisteredCount => registered;
    }

    /// <summary>
    /// 미스 반응의 시작점. 프레임 표식을 남긴 뒤 원본 실행 자체를 건너뜁니다.
    /// 체력은 여기서 안 깎이지만 MISS 연출이 여기서 나오는 것으로 보여, 연출까지 함께 잠재웁니다.
    /// </summary>
    [HarmonyLib.HarmonyPatch(typeof(Il2CppAssets.Scripts.GameCore.HostComponent.BattleRoleAttributeComponent), GameBindings.BattleRoleAttributeComponent.Miss)]
    public class BattleRoleAttributeComponent_Miss_MissPenalty_Patch
    {
        public static bool Prefix()
        {
            try
            {
                if (!ForcePerfectState.Enabled) return true;

                // 표식은 원본을 건너뛰더라도 반드시 먼저 남깁니다. 뒤따르는 Hurt를 가려내는 유일한 단서입니다.
                MissPenaltyGate.MarkMissFrame();
                MissPenaltyGate.RecordMissSkipped();
                return false;
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[BattleRoleAttributeComponent.Miss.Prefix] 미스 반응 차단 중 예외 발생: {ex}");
                return true;
            }
        }
    }

    /// <summary>
    /// 실제 데미지 지점. 미스와 같은 프레임이면 호출 자체를 건너뜁니다.
    /// hurtValue=0으로만 두면 내부에서 최소 1 데미지로 클램프되는 정황이 있어(체력이 -1씩 감소),
    /// 값을 고치는 대신 통째로 막습니다.
    /// </summary>
    [HarmonyLib.HarmonyPatch(typeof(Il2CppAssets.Scripts.GameCore.HostComponent.BattleRoleAttributeComponent), GameBindings.BattleRoleAttributeComponent.Hurt)]
    public class BattleRoleAttributeComponent_Hurt_MissPenalty_Patch
    {
        public static bool Prefix(int hurtValue, bool isAir)
        {
            try
            {
                if (!ForcePerfectState.Enabled) return true;
                if (!MissPenaltyGate.IsMissFrame()) return true; // 장애물 피격 등 미스와 무관한 데미지는 그대로 둡니다.

                MissPenaltyGate.RecordBlocked(hurtValue, MissPenaltyGate.CurrentHp());
                return false;
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[BattleRoleAttributeComponent.Hurt.Prefix] 미스 데미지 차단 중 예외 발생: {ex}");
                return true;
            }
        }
    }
}
