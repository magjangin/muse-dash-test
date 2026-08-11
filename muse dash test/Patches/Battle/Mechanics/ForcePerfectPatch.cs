using System;
using MelonLoader;
using Il2CppPeroPeroGames.GlobalDefines;

namespace muse_dash_test
{
    // === 판정 강제 퍼펙트 패치 (설정 파일 연동) ===
    // config.txt의 '강제퍼펙트=true'이면 게임에 기록되는 판정 결과를 Perfect로 승격시킵니다.
    // 오토플레이(DBSkill.SetAutoPlay)와는 완전히 별개입니다. 입력은 그대로 사람이 하고,
    // 노트를 실제로 치는 타이밍도 그대로이며, "기록되는 판정 값"만 바뀝니다.
    //
    // 판정 값이 지나가는 두 지점을 모두 덮어씁니다.
    //   - TaskStageTarget.SetPlayResult(int idx, uint result, bool isMulEnd)                : 판정 집계(Perfect/Great/Miss 카운터) 쪽
    //   - BattleEnemyManager.SetPlayResult(int idx, byte result, bool, bool, bool)          : 노트별 결과 저장 쪽
    // 두 경로가 서로를 호출하더라도 같은 값을 두 번 쓰는 것뿐이라 부작용은 없습니다.

    /// <summary>
    /// 판정 승격 규칙과 적용 통계를 담는 공용 상태입니다.
    /// </summary>
    internal static class ForcePerfectState
    {
        // TaskResult: None=0, Miss=1, Cool=2, Great=3, Prefect=4(게임 원본 오타), JumpOver=5, Fever=6
        internal const uint Perfect = (uint)TaskResult.Prefect;

        private const double SummaryIntervalSeconds = 10.0;

        private static readonly int[] promotedByOrigin = new int[7];
        private static DateTime lastSummaryTime = DateTime.MinValue;
        private static bool pendingSummary;
        private static bool announced;

        internal static bool Enabled => InputOverlay.forcePerfect;

        /// <summary>
        /// Perfect로 승격할 판정인지 판단합니다. 실제 "타격 판정"인 Miss/Cool/Great만 대상입니다.
        /// None(아직 판정 전), JumpOver(톱니 회피), Fever는 종류가 다른 결과이므로 건드리면
        /// 정확도 분모와 톱니/피버 집계가 어긋납니다.
        /// </summary>
        internal static bool ShouldPromote(uint result)
        {
            return result == (uint)TaskResult.Miss
                || result == (uint)TaskResult.Cool
                || result == (uint)TaskResult.Great;
        }

        internal static void Record(string source, int idx, uint original)
        {
            if (original < promotedByOrigin.Length)
            {
                promotedByOrigin[original]++;
            }
            pendingSummary = true;

            if (!announced)
            {
                announced = true;
                MelonLogger.Msg($"[ForcePerfect] 판정 강제 첫 적용: {source} idx={idx}, {Describe(original)} -> Prefect");
            }

            if ((DateTime.UtcNow - lastSummaryTime).TotalSeconds >= SummaryIntervalSeconds)
            {
                LogSummary();
            }
        }

        internal static void OnSettingChanged(bool enabled)
        {
            MelonLogger.Msg($"[ForcePerfect] 강제퍼펙트 설정이 {(enabled ? "켜짐" : "꺼짐")}으로 변경되었습니다. (오토플레이와는 무관하며 판정 값만 바뀝니다)");
            if (!enabled)
            {
                LogSummary();
            }
            announced = false;
        }

        private static void LogSummary()
        {
            lastSummaryTime = DateTime.UtcNow;
            if (!pendingSummary) return;
            pendingSummary = false;

            MelonLogger.Msg($"[ForcePerfect] 누적 승격 현황: Miss={promotedByOrigin[(int)TaskResult.Miss]}, " +
                            $"Cool={promotedByOrigin[(int)TaskResult.Cool]}, Great={promotedByOrigin[(int)TaskResult.Great]}");
        }

        internal static string Describe(uint result)
        {
            switch (result)
            {
                case (uint)TaskResult.None: return "None";
                case (uint)TaskResult.Miss: return "Miss";
                case (uint)TaskResult.Cool: return "Cool";
                case (uint)TaskResult.Great: return "Great";
                case (uint)TaskResult.Prefect: return "Prefect";
                case (uint)TaskResult.JumpOver: return "JumpOver";
                case (uint)TaskResult.Fever: return "Fever";
                default: return $"Unknown({result})";
            }
        }
    }

    /// <summary>판정 집계 진입점에서 결과 값을 Perfect로 덮어씁니다.</summary>
    [HarmonyLib.HarmonyPatch(typeof(Il2CppAssets.Scripts.GameCore.HostComponent.TaskStageTarget), GameBindings.TaskStageTarget.SetPlayResult)]
    public class TaskStageTarget_SetPlayResult_ForcePerfect_Patch
    {
        public static void Prefix(int idx, ref uint result)
        {
            try
            {
                if (!ForcePerfectState.Enabled) return;

                uint original = result;
                if (!ForcePerfectState.ShouldPromote(original)) return;

                result = ForcePerfectState.Perfect;
                ForcePerfectState.Record("TaskStageTarget.SetPlayResult", idx, original);
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[TaskStageTarget.SetPlayResult.Prefix] 판정 강제 중 예외 발생: {ex}");
            }
        }
    }

    /// <summary>노트별 결과 저장소에서도 같은 값으로 덮어씁니다.</summary>
    [HarmonyLib.HarmonyPatch(typeof(Il2CppAssets.Scripts.GameCore.HostComponent.BattleEnemyManager), GameBindings.BattleEnemyManager.SetPlayResult)]
    public class BattleEnemyManager_SetPlayResult_ForcePerfect_Patch
    {
        public static void Prefix(int idx, ref byte result)
        {
            try
            {
                if (!ForcePerfectState.Enabled) return;

                uint original = result;
                if (!ForcePerfectState.ShouldPromote(original)) return;

                result = (byte)ForcePerfectState.Perfect;
                ForcePerfectState.Record("BattleEnemyManager.SetPlayResult", idx, original);
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[BattleEnemyManager.SetPlayResult.Prefix] 판정 강제 중 예외 발생: {ex}");
            }
        }
    }

    // --- 아래 두 개는 관찰 전용(값 변경 없음) 프로브입니다 ---
    // SetPlayResult를 덮어써도 Miss 카운터가 따로 올라가면 m_MissResult != 0 이 되어 AP 조건이 깨집니다.
    // 강제퍼펙트가 켜진 동안에만 로그를 남겨, 미스 경로가 여전히 살아 있는지 한 판 돌려보면 알 수 있게 합니다.

    [HarmonyLib.HarmonyPatch(typeof(Il2CppAssets.Scripts.GameCore.HostComponent.TaskStageTarget), GameBindings.TaskStageTarget.AddMiss)]
    public class TaskStageTarget_AddMiss_ForcePerfectProbe_Patch
    {
        public static void Postfix(Il2CppAssets.Scripts.GameCore.HostComponent.TaskStageTarget __instance, int value)
        {
            try
            {
                if (!ForcePerfectState.Enabled || __instance == null) return;
                MelonLogger.Msg($"[ForcePerfect.Probe] AddMiss(value={value}) 호출됨 - 판정 강제와 별개로 미스 카운터가 올라갔습니다. " +
                                $"m_MissResult={__instance.m_MissResult}, m_GreatResult={__instance.m_GreatResult}, m_MissCombo={__instance.m_MissCombo}");
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[TaskStageTarget.AddMiss.Postfix] 프로브 예외 발생: {ex}");
            }
        }
    }

    [HarmonyLib.HarmonyPatch(typeof(Il2CppAssets.Scripts.GameCore.HostComponent.TaskStageTarget), GameBindings.TaskStageTarget.TriggerNoteMiss)]
    public class TaskStageTarget_TriggerNoteMiss_ForcePerfectProbe_Patch
    {
        public static void Postfix(Il2CppAssets.Scripts.GameCore.HostComponent.TaskStageTarget __instance)
        {
            try
            {
                if (!ForcePerfectState.Enabled || __instance == null) return;
                MelonLogger.Msg($"[ForcePerfect.Probe] TriggerNoteMiss 호출됨 - 노트 미스 경로가 살아 있습니다. " +
                                $"m_MissResult={__instance.m_MissResult}, m_MissCombo={__instance.m_MissCombo}");
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[TaskStageTarget.TriggerNoteMiss.Postfix] 프로브 예외 발생: {ex}");
            }
        }
    }
}
