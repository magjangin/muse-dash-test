using System;
using System.Collections.Generic;
using Il2Cpp;
using MelonLoader;

namespace muse_dash_test
{
    /// <summary>
    /// 실험: 샌드백(멀티히트) 연타 중에만 캐릭터 타격 애니메이션을 다른 동작으로 바꿉니다.
    /// 지상 연타는 복선 동작으로, 공중 연타는 공중 그레이트 동작으로 돌립니다.
    ///
    /// 배경 — 2026-08-11 액션 계약서 실측으로 확인한 사실:
    ///   · 샌드백에는 전용 애니메이션이 없습니다. 게임은 연타 구간 동안 일반 노트와 똑같은
    ///     <c>char_jumphit</c>(공중) / <c>char_atk_p</c>(지상) 계열 키를 빠르게 반복 호출할 뿐입니다.
    ///   · 연타 구간의 경계만 <c>char_multihit_start</c> / <c>char_multihit_end</c> 키로 통지됩니다.
    ///     이 두 키는 actionData 에 정의가 없어서 애니메이션을 재생하지 않는 순수 상태 마커입니다.
    ///   · 복선은 <c>char_bighit</c> → <c>double_hit_1</c> / <c>double_hit_2</c> 입니다.
    ///
    /// 따라서 actionData 매핑만 고치면 일반 노트까지 같이 바뀝니다. 샌드백만 분리하려면
    /// 연타 구간 안인지를 추적해서 그 동안에만 키를 갈아끼워야 합니다.
    ///
    /// 구현 방식 — 파라미터를 ref 로 고쳐 쓰지 않고, 원본을 건너뛴 뒤 다른 키로 다시 호출합니다.
    /// IL2CPP 에서 byref 파라미터를 만지는 것은 위험한 형태라(같은 날 프로세스가 죽은 전례가 있음)
    /// 값 파라미터만 쓰는 경로를 택했습니다.
    /// </summary>
    internal static class SandbagAnimationOverride
    {
        /// <summary>실험 스위치. false 로 두면 원본 동작 그대로입니다.</summary>
        public static bool Enabled = true;

        /// <summary>연타 구간의 시작/종료를 알리는 상태 마커 키.</summary>
        private const string MultiHitStartKey = "char_multihit_start";
        private const string MultiHitEndKey = "char_multihit_end";

        /// <summary>
        /// 연타 중 바꿔치기할 액션 키 (원본 → 대체).
        ///
        /// 경로(공중/지상)를 반드시 맞춰야 합니다. 게임 로직은 공중 샌드백이면 캐릭터를 띄운 채로
        /// 두는데, 여기에 지상 동작을 재생하면 땅에 선 포즈가 공중에서 나와 어긋나 보입니다.
        /// 실제로 공중 샌드백을 지상 <c>char_bighit</c> 으로 돌렸을 때 그 증상이 나왔습니다(2026-08-11).
        ///
        /// 경로 구분은 계약서의 <c>actionEventIdx</c> 로 확인했습니다 — 0 이면 공중 계열,
        /// 4 면 지상 공격 계열입니다.
        /// </summary>
        private static readonly Dictionary<string, string> RedirectMap = new Dictionary<string, string>
        {
            // 공중(eventIdx 0) — 공중 동작끼리 교체합니다.
            //   char_jumphit → air_hit_perfect_*  를  char_jumphit_great → air_hit_great_*  로.
            //   char_jumphit_great 는 그대로 두어 공중 연타 전체가 great 동작으로 통일됩니다.
            { "char_jumphit", "char_jumphit_great" },

            // 지상(eventIdx 4) — 복선 동작으로 교체합니다.
            //   char_atk_p / char_atk_g → road_hit_*  를  char_bighit → double_hit_*  로.
            { "char_atk_p", "char_bighit" },
            { "char_atk_g", "char_bighit" },
        };

        /// <summary>지금 연타 구간 안인지.</summary>
        private static bool inMultiHit;

        /// <summary>바꿔치기로 다시 호출할 때 자기 자신을 또 가로채지 않도록 하는 재진입 가드.</summary>
        private static bool redirecting;

        /// <summary>
        /// 액션 키를 가로채 필요하면 복선 키로 돌립니다.
        /// </summary>
        /// <returns>원본 호출을 그대로 진행하면 true, 이미 대체 호출을 마쳤으면 false.</returns>
        public static bool HandlePlayByKey(SpineActionController sac, string actionKey, bool isOverride)
        {
            if (!Enabled || redirecting || sac == null || string.IsNullOrEmpty(actionKey)) return true;

            try
            {
                if (actionKey == MultiHitStartKey)
                {
                    inMultiHit = true;
                    MelonLogger.Msg("[샌드백실험] 연타 구간 진입 — 타격을 복선 애니메이션으로 대체합니다.");
                    return true;
                }

                if (actionKey == MultiHitEndKey)
                {
                    inMultiHit = false;
                    MelonLogger.Msg("[샌드백실험] 연타 구간 종료 — 원래 애니메이션으로 복귀합니다.");
                    return true;
                }

                if (!inMultiHit) return true;
                if (!RedirectMap.TryGetValue(actionKey, out string replacementKey)) return true;

                redirecting = true;
                try
                {
                    sac.PlayByKey(replacementKey, isOverride);
                }
                finally
                {
                    redirecting = false;
                }

                MelonLogger.Msg($"[샌드백실험] \"{actionKey}\" → \"{replacementKey}\" 로 대체 재생");
                return false;
            }
            catch (Exception ex)
            {
                MelonLogger.Warning($"[샌드백실험] 예외로 원본 동작을 유지합니다: {ex.Message}");
                redirecting = false;
                return true;
            }
        }

        /// <summary>스테이지를 벗어날 때 구간 상태가 남아 있지 않도록 초기화합니다.</summary>
        public static void Reset()
        {
            inMultiHit = false;
            redirecting = false;
        }
    }

    // 이 실험에는 전용 [HarmonyPatch] 클래스가 없습니다.
    // PlayByKey 에는 계약서 프로브가 이미 Prefix 를 붙이고 있고, 같은 메서드에 패치 클래스를
    // 둘 붙였을 때 양쪽이 모두 실행된다는 확인을 아직 못 했습니다. 확인되지 않은 것에 기대는 대신
    // Patch_SpineContract_PlayByKey 하나에서 관측과 대체를 함께 처리합니다.
}
