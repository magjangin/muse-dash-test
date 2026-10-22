using System;
using MelonLoader;
using Il2CppPeroPeroGames.GlobalDefines;
using Il2CppFormulaBase;

namespace muse_dash_test
{
    // === 판정 강제 퍼펙트 패치 (설정 파일 연동) ===
    // config.txt의 '강제퍼펙트=true' 또는 'AllPerfect=1'이면 노트를 Perfect로 승격/조작합니다.
    // 오토플레이(DBSkill.SetAutoPlay)와는 완전히 별개입니다. 입력도 타이밍도 그대로 사람이 합니다.
    //
    // 개입 지점은 두 단계(다층 방어선)입니다:
    //   1. StageBattleComponent.InitTimeNodeOrder (사전 판정 테이블 조작)
    //      뮤즈대시는 로딩 시점에 각 틱별 판정을 TimeNodeOrder 객체에 미리 구워서(Pre-bake)
    //      m_TimeNodeOrders 테이블에 등록합니다. 이 테이블의 isPerfectNode를 true로,
    //      result를 Prefect로 덮어써서 엔진이 처음부터 Perfect 틱을 밟은 것으로 인식하게 만듭니다.
    //   2. GameTouchPlay.TouchResult (터치 판정 산출 직후 훅)
    //      실시간 타격 결과가 화면 표시·애니메이션·집계로 흩어지기 직전 Perfect로 보정하는 안전망입니다.
    //
    // 안 친 노트(미스)는 이 경로를 타지 않으므로 손대지 않습니다. 미스는 화면·체력·기록 모두
    // 순정 그대로 동작합니다.

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
        private static WeakReference<StageBattleComponent> activeStageBattle;

        internal static bool Enabled => InputOverlay.forcePerfect;

        internal static void SetActiveStageBattle(StageBattleComponent component)
        {
            activeStageBattle = component != null ? new WeakReference<StageBattleComponent>(component) : null;
        }

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
            if (enabled && activeStageBattle != null && activeStageBattle.TryGetTarget(out var comp))
            {
                PromoteTimeNodeOrders(comp);
            }
            if (!enabled)
            {
                LogSummary(important: true);
            }
            announced = false;
        }

        /// <summary>
        /// 곡 로딩 시 StageBattleComponent가 생성해 둔 틱별 사전 판정 테이블(m_TimeNodeOrders)의
        /// 모든 판정(isPerfectNode, result)을 원천적으로 Perfect로 조작합니다.
        /// </summary>
        internal static int PromoteTimeNodeOrders(StageBattleComponent stageBattle)
        {
            if (stageBattle == null) return 0;

            try
            {
                var dict = stageBattle.m_TimeNodeOrders;
                if (dict == null) return 0;

                int promoted = 0;
                int total = 0;

                foreach (var pair in dict)
                {
                    var tnoList = pair.Value;
                    if (tnoList == null) continue;

                    for (int i = 0; i < tnoList.Count; i++)
                    {
                        var tno = tnoList[i];
                        if (tno == null) continue;

                        total++;
                        // 1. 퍼펙트 노드 플래그 활성화
                        tno.isPerfectNode = true;

                        // 2. Great(3) 또는 Cool(2) 틱으로 사전 구워진 판정을 Prefect(4)로 원천 승격
                        if (tno.result == (byte)TaskResult.Great || tno.result == (byte)TaskResult.Cool)
                        {
                            tno.result = (byte)TaskResult.Prefect;
                            promoted++;
                        }
                    }
                }

                if (promoted > 0 || total > 0)
                {
                    ModLogger.Msg($"[ForcePerfect] 사전 판정 테이블(m_TimeNodeOrders) 조작 완료: 승격={promoted} / 전체={total}");
                }

                return promoted;
            }
            catch (Exception ex)
            {
                ModLogger.Error($"[ForcePerfect.PromoteTimeNodeOrders] 판정 테이블 조작 중 예외: {ex}");
                return 0;
            }
        }

        /// <summary>
        /// 누적 승격 현황을 남깁니다. 플레이 중 10초마다 부르는 쪽은 Verbose로,
        /// 설정을 끌 때처럼 "이번 세션은 여기서 끝"인 자리만 <paramref name="important"/>로 Info에 남깁니다.
        /// (배틀 내내 같은 줄이 반복되면 정작 봐야 할 로그가 밀립니다)
        /// </summary>
        private static void LogSummary(bool important = false)
        {
            lastSummaryTime = DateTime.UtcNow;
            if (!pendingSummary) return;
            pendingSummary = false;

            string message = $"[ForcePerfect] 누적 승격 현황: Miss={promotedByOrigin[(int)TaskResult.Miss]}, " +
                            $"Cool={promotedByOrigin[(int)TaskResult.Cool]}, Great={promotedByOrigin[(int)TaskResult.Great]} " +
                            $"| 훅별: GameTouchPlay.TouchResult={promotedTotal}";

            if (important) ModLogger.Msg(message); else ModLogger.Verbose(message);
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
    /// [1단계: 원천 조작] 곡 로딩 시점에 StageBattleComponent.InitTimeNodeOrder가 완료된 직후,
    /// 사전 생성된 틱별 판정 테이블(m_TimeNodeOrders)의 판정 파라미터를 Perfect로 일괄 조작합니다.
    /// </summary>
    [HarmonyLib.HarmonyPatch(typeof(Il2CppFormulaBase.StageBattleComponent), GameBindings.StageBattleComponent.InitTimeNodeOrder)]
    public class StageBattleComponent_InitTimeNodeOrder_ForcePerfect_Patch
    {
        public static void Postfix(Il2CppFormulaBase.StageBattleComponent __instance)
        {
            try
            {
                ForcePerfectState.SetActiveStageBattle(__instance);
                if (!ModConfig.EnableForcePerfect || !ForcePerfectState.Enabled) return;

                ForcePerfectState.PromoteTimeNodeOrders(__instance);
            }
            catch (Exception ex)
            {
                ModLogger.Error($"[StageBattleComponent.InitTimeNodeOrder] 사전 판정 테이블 조작 중 예외 발생: {ex}");
            }
        }
    }

    /// <summary>
    /// [2단계: 실시간 안전망] 터치 판정이 산출된 직후, 화면 표시·캐릭터 액션·집계로 흩어지기 전에 결과 값을 Perfect로 보정합니다.
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
