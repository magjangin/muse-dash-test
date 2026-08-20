using System;
using MelonLoader;
using Il2CppPeroPeroGames.GlobalDefines;

namespace muse_dash_test
{
    // === 판정 강제 퍼펙트 패치 (설정 파일 연동) ===
    // config.txt의 '강제퍼펙트=true'이면 친 노트의 판정을 Perfect로 승격시킵니다.
    // 오토플레이(DBSkill.SetAutoPlay)와는 완전히 별개입니다. 입력도 타이밍도 그대로 사람이 하고,
    // 산출된 판정 값만 바뀝니다.
    //
    // 개입 지점은 하나입니다.
    //   GameTouchPlay.TouchResult(int idx, byte resultCode, uint actionType, ...) - 터치 판정 산출 직후.
    //   이 값이 판정 표시(ShowPlayResult), 캐릭터 액션(GirlActionController.Attack(actKey, result)),
    //   집계(TaskStageTarget)로 함께 흘러가므로 화면과 기록이 어긋나지 않습니다.
    //
    // 안 친 노트(미스)는 이 경로를 타지 않으므로 손대지 않습니다. 미스는 화면·체력·기록 모두
    // 순정 그대로 동작합니다. (미스까지 다루려면 BattleEnemyManager.SetPlayResult 승격,
    // 미스 페널티 차단, 집계 등록이 함께 필요하며 그 실험 기록은 git 히스토리에 남아 있습니다.)

    /// <summary>
    /// 판정 승격 규칙과 적용 통계를 담는 공용 상태입니다.
    /// </summary>
    internal static class ForcePerfectState
    {
        // TaskResult: None=0, Miss=1, Cool=2, Great=3, Prefect=4(게임 원본 오타), JumpOver=5, Fever=6
        internal const uint Perfect = (uint)TaskResult.Prefect;

        private const double SummaryIntervalSeconds = 10.0;

        private static readonly int[] promotedByOrigin = new int[7];
        private static int promotedTotal;
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
            promotedTotal++;
            pendingSummary = true;

            if (!announced)
            {
                announced = true;
                ModLogger.Msg($"[ForcePerfect] 판정 강제 첫 적용: {source} idx={idx}, {Describe(original)} -> Prefect");
            }

            if ((DateTime.UtcNow - lastSummaryTime).TotalSeconds >= SummaryIntervalSeconds)
            {
                LogSummary();
            }
        }

        internal static void OnSettingChanged(bool enabled)
        {
            ModLogger.Msg($"[ForcePerfect] 강제퍼펙트 설정이 {(enabled ? "켜짐" : "꺼짐")}으로 변경되었습니다. (오토플레이와는 무관하며 판정 값만 바뀝니다)");
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

            ModLogger.Msg($"[ForcePerfect] 누적 승격 현황: Miss={promotedByOrigin[(int)TaskResult.Miss]}, " +
                            $"Cool={promotedByOrigin[(int)TaskResult.Cool]}, Great={promotedByOrigin[(int)TaskResult.Great]} " +
                            $"| 훅별: GameTouchPlay.TouchResult={promotedTotal}");
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

    /// <summary>
    /// 터치 판정이 산출된 직후, 화면 표시·캐릭터 액션·집계로 흩어지기 전에 결과 값을 Perfect로 덮어씁니다.
    /// </summary>
    [HarmonyLib.HarmonyPatch(typeof(Il2CppGameLogic.GameTouchPlay), GameBindings.GameTouchPlay.TouchResult)]
    public class GameTouchPlay_TouchResult_ForcePerfect_Patch
    {
        public static void Prefix(int idx, ref byte resultCode)
        {
            try
            {
                if (!ModConfig.EnableForcePerfect || !ForcePerfectState.Enabled) return;

                uint original = resultCode;
                if (!ForcePerfectState.ShouldPromote(original)) return;

                resultCode = (byte)ForcePerfectState.Perfect;
                ForcePerfectState.Record("GameTouchPlay.TouchResult", idx, original);
            }
            catch (Exception ex)
            {
                ModLogger.Error($"[GameTouchPlay.TouchResult.Prefix] 판정 강제 중 예외 발생: {ex}");
            }
        }
    }
}
