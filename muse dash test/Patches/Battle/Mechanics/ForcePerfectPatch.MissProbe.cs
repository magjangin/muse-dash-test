using System;
using MelonLoader;

namespace muse_dash_test
{
    // === 안 친 노트(미스) 경로 관찰 전용 프로브 ===
    // 값은 하나도 바꾸지 않습니다. 강제퍼펙트가 켜져 있는 동안에만 로그를 남깁니다.
    //
    // 확인하려는 것:
    //   1. GameMissPlay.MissCube가 프레임마다 불리는 뜨거운 경로인지(= 여기에 Prefix를 걸어도 되는지)
    //   2. 실제 미스가 성립하는 순간(__result == true)의 인자와 그때의 체력
    //   3. 체력을 실제로 깎는 주체가 BattleRoleAttributeComponent.Miss()인지 Hurt(hurtValue, isAir)인지
    // 이 셋이 나오면 "미스 페널티만 무효화" 방식을 어디에 걸지 확정할 수 있습니다.

    internal static class MissProbeStats
    {
        private const double ReportIntervalSeconds = 10.0;

        private static long totalCalls;
        private static long totalMisses;
        private static long callsSinceReport;
        private static DateTime lastReportTime = DateTime.MinValue;

        internal static void CountCall(bool wasMiss)
        {
            totalCalls++;
            callsSinceReport++;
            if (wasMiss) totalMisses++;

            if ((DateTime.UtcNow - lastReportTime).TotalSeconds >= ReportIntervalSeconds)
            {
                MelonLogger.Msg($"[ForcePerfect.Probe] MissCube 호출 빈도: 최근 {ReportIntervalSeconds}초간 {callsSinceReport}회 " +
                                $"(누적 {totalCalls}회, 그중 미스 성립 {totalMisses}회)");
                callsSinceReport = 0;
                lastReportTime = DateTime.UtcNow;
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

    /// <summary>안 친 노트의 미스 처리 지점. 호출 빈도만 세고, 실제 미스가 성립할 때만 상세 로그를 남깁니다.</summary>
    [HarmonyLib.HarmonyPatch(typeof(Il2CppGameLogic.GameMissPlay), GameBindings.GameMissPlay.MissCube)]
    public class GameMissPlay_MissCube_ForcePerfectProbe_Patch
    {
        public static void Postfix(int idx, Il2CppSystem.Decimal currentTick, bool __result)
        {
            try
            {
                if (!ForcePerfectState.Enabled) return;

                MissProbeStats.CountCall(__result);
                if (!__result) return; // 미스가 성립하지 않은 호출은 로그를 남기지 않습니다(스팸 방지).

                MelonLogger.Msg($"[ForcePerfect.Probe] MissCube 미스 성립: idx={idx}, currentTick={currentTick}, HP={MissProbeStats.CurrentHp()}");
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[GameMissPlay.MissCube.Postfix] 프로브 예외 발생: {ex}");
            }
        }
    }

    /// <summary>미스로 인한 체력 처리의 주체를 가려내기 위한 프로브.</summary>
    [HarmonyLib.HarmonyPatch(typeof(Il2CppAssets.Scripts.GameCore.HostComponent.BattleRoleAttributeComponent), GameBindings.BattleRoleAttributeComponent.Miss)]
    public class BattleRoleAttributeComponent_Miss_ForcePerfectProbe_Patch
    {
        private static int hpBefore = -1;

        public static void Prefix()
        {
            hpBefore = MissProbeStats.CurrentHp();
        }

        public static void Postfix()
        {
            try
            {
                if (!ForcePerfectState.Enabled) return;
                MelonLogger.Msg($"[ForcePerfect.Probe] BattleRoleAttributeComponent.Miss() 호출됨: HP {hpBefore} -> {MissProbeStats.CurrentHp()}");
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[BattleRoleAttributeComponent.Miss.Postfix] 프로브 예외 발생: {ex}");
            }
        }
    }

    /// <summary>실제 데미지 적용 지점. 미스 외의 피격(장애물 등)도 함께 잡히므로 hurtValue로 구분합니다.</summary>
    [HarmonyLib.HarmonyPatch(typeof(Il2CppAssets.Scripts.GameCore.HostComponent.BattleRoleAttributeComponent), GameBindings.BattleRoleAttributeComponent.Hurt)]
    public class BattleRoleAttributeComponent_Hurt_ForcePerfectProbe_Patch
    {
        private static int hpBefore = -1;

        public static void Prefix()
        {
            hpBefore = MissProbeStats.CurrentHp();
        }

        public static void Postfix(int hurtValue, bool isAir)
        {
            try
            {
                if (!ForcePerfectState.Enabled) return;
                MelonLogger.Msg($"[ForcePerfect.Probe] BattleRoleAttributeComponent.Hurt(hurtValue={hurtValue}, isAir={isAir}) 호출됨: HP {hpBefore} -> {MissProbeStats.CurrentHp()}");
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[BattleRoleAttributeComponent.Hurt.Postfix] 프로브 예외 발생: {ex}");
            }
        }
    }
}
