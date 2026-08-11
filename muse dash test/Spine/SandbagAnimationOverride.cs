using System;
using System.Collections.Generic;
using Il2Cpp;
using Il2CppSpine;
using MelonLoader;

namespace muse_dash_test
{
    /// <summary>
    /// 실험: 샌드백(멀티히트) 연타 중에만 캐릭터 타격 애니메이션을 복선 애니메이션으로 바꿉니다.
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
        /// <summary>
        /// 키 대체 스위치. false 면 게임 원래 동작 그대로 둡니다.
        /// 구간 추적(<see cref="InMultiHit"/>)은 이 값과 무관하게 항상 동작하므로,
        /// 꺼둔 상태에서 연타 구간의 원본 호출 순서를 관측할 수 있습니다.
        /// </summary>
        public static bool Enabled = false;

        /// <summary>연타 구간의 시작/종료를 알리는 상태 마커 키.</summary>
        private const string MultiHitStartKey = "char_multihit_start";
        private const string MultiHitEndKey = "char_multihit_end";

        /// <summary>연타 중 이 키들이 오면 복선 키로 바꿔치기합니다.</summary>
        private static readonly string[] RedirectedKeys =
        {
            "char_jumphit",        // 공중 샌드백 퍼펙트
            "char_jumphit_great",  // 공중 샌드백 그레이트
            "char_atk_p",          // 지상 샌드백 퍼펙트
            "char_atk_g",          // 지상 샌드백 그레이트
        };

        /// <summary>복선 액션 키. actionData 상 double_hit_1 / double_hit_2 로 매핑됩니다.</summary>
        private const string DoubleNoteKey = "char_bighit";

        /// <summary>
        /// 연타 구간을 시작 신호 한 번으로 처리하는 모드.
        ///
        /// 타격마다 <c>SetAnimation</c> 이 불리기 때문에, 타격 단위로 교체하면 대체 동작이
        /// 매번 0프레임부터 다시 시작되어 끝까지 재생될 틈이 없습니다.
        /// 대신 <c>char_multihit_start</c> 에서 한 번만 걸고 구간 내내 유지하면
        /// 동작이 온전히 재생됩니다.
        /// </summary>
        public static bool StartDrivenEnabled = true;

        /// <summary>연타 구간 내내 유지할 애니메이션. 반복 재생으로 겁니다.</summary>
        private const string HoldAnimation = "double_hit_1";

        /// <summary>
        /// 타격 단위 애니메이션 교체 스위치.
        /// <see cref="StartDrivenEnabled"/> 와 배타적입니다 — 시작 구동 모드가 켜져 있으면 무시됩니다.
        /// </summary>
        public static bool AnimationSwapEnabled = false;

        /// <summary>
        /// 연타 구간 안에서만 적용할 애니메이션 이름 교체표 (원본 → 대체).
        ///
        /// 퍼펙트 변형 네 개를 모두 복선 동작으로 돌립니다. 복선 액션(<c>char_bighit</c>)이
        /// <c>double_hit_1</c>/<c>double_hit_2</c> 두 개를 무작위로 돌려쓰므로, 여기서도 번갈아
        /// 배정해 연타가 한 동작만 반복하는 것처럼 보이지 않게 했습니다.
        ///
        /// 그레이트 변형(<c>air_hit_great_*</c>)은 손대지 않습니다. 퍼펙트일 때만 복선 동작이
        /// 나오므로 판정에 따라 모습이 갈립니다.
        /// </summary>
        private static readonly Dictionary<string, string> AnimationSwapMap = new Dictionary<string, string>
        {
            { "air_hit_perfect_1", "double_hit_1" },
            { "air_hit_perfect_2", "double_hit_2" },
            { "air_hit_perfect_3", "double_hit_1" },
            { "air_hit_perfect_4", "double_hit_2" },
        };

        /// <summary>지금 연타 구간 안인지.</summary>
        private static bool inMultiHit;

        /// <summary>애니메이션 교체로 다시 호출할 때 자기 자신을 또 가로채지 않도록 하는 가드.</summary>
        private static bool swappingAnimation;

        /// <summary>구간 유지를 위해 막아낸 애니메이션 변경 횟수. 구간 종료 시 한 번만 보고합니다.</summary>
        private static int suppressedCount;

        /// <summary>연타 구간 안이면 true. 이 구간에서만 원본 호출을 중복 제거 없이 기록합니다.</summary>
        public static bool InMultiHit => inMultiHit;

        /// <summary>바꿔치기로 다시 호출할 때 자기 자신을 또 가로채지 않도록 하는 재진입 가드.</summary>
        private static bool redirecting;

        /// <summary>
        /// 액션 키를 가로채 필요하면 복선 키로 돌립니다.
        /// </summary>
        /// <returns>원본 호출을 그대로 진행하면 true, 이미 대체 호출을 마쳤으면 false.</returns>
        public static bool HandlePlayByKey(SpineActionController sac, string actionKey, bool isOverride)
        {
            if (redirecting || sac == null || string.IsNullOrEmpty(actionKey)) return true;

            try
            {
                // 구간 추적은 대체 스위치와 무관하게 항상 수행합니다. 꺼둔 상태에서도
                // "연타 구간이 언제부터 언제까지인가"는 원본 관측에 필요한 정보입니다.
                if (actionKey == MultiHitStartKey)
                {
                    inMultiHit = true;

                    if (StartDrivenEnabled)
                    {
                        // 시작 신호에서 한 번만 걸고, 구간 내내 이 동작을 유지합니다.
                        // 반복 재생으로 걸어야 짧은 동작이 한 번 끝나고 사라지지 않습니다.
                        swappingAnimation = true;
                        try
                        {
                            sac.SetAnimation(HoldAnimation, true);
                            MelonLogger.Msg($"[샌드백실험] 연타 구간 진입 — \"{HoldAnimation}\" 을(를) 반복 재생으로 걸고 유지합니다.");
                        }
                        catch (Exception ex)
                        {
                            MelonLogger.Warning($"[샌드백실험] 시작 애니메이션 적용 실패: {ex.Message}");
                        }
                        finally
                        {
                            swappingAnimation = false;
                        }
                    }
                    else
                    {
                        MelonLogger.Msg(Enabled
                            ? "[샌드백실험] 연타 구간 진입 — 타격을 복선 애니메이션으로 대체합니다."
                            : "[샌드백원본] 연타 구간 진입 — 대체는 꺼져 있고, 원본 호출을 전부 기록합니다.");
                    }

                    return true;
                }

                if (actionKey == MultiHitEndKey)
                {
                    inMultiHit = false;

                    if (StartDrivenEnabled)
                    {
                        MelonLogger.Msg($"[샌드백실험] 연타 구간 종료 — 유지 중 막아낸 애니메이션 변경 {suppressedCount}건.");
                    }
                    else
                    {
                        MelonLogger.Msg(Enabled
                            ? "[샌드백실험] 연타 구간 종료 — 원래 애니메이션으로 복귀합니다."
                            : "[샌드백원본] 연타 구간 종료.");
                    }

                    suppressedCount = 0;
                    return true;
                }

                if (!Enabled) return true;
                if (!inMultiHit) return true;
                if (Array.IndexOf(RedirectedKeys, actionKey) < 0) return true;

                redirecting = true;
                try
                {
                    sac.PlayByKey(DoubleNoteKey, isOverride);
                }
                finally
                {
                    redirecting = false;
                }

                MelonLogger.Msg($"[샌드백실험] \"{actionKey}\" → \"{DoubleNoteKey}\" 로 대체 재생");
                return false;
            }
            catch (Exception ex)
            {
                MelonLogger.Warning($"[샌드백실험] 예외로 원본 동작을 유지합니다: {ex.Message}");
                redirecting = false;
                return true;
            }
        }

        /// <summary>
        /// 연타 구간 안에서 특정 애니메이션 이름을 다른 이름으로 갈아끼웁니다.
        ///
        /// 원본을 건너뛰고 대체 이름으로 다시 호출합니다. 호출자가 반환된 TrackEntry 로
        /// 후속 처리를 할 수 있으므로 <paramref name="result"/> 에 대체 호출의 결과를 넣어 줍니다.
        /// 대체 호출이 실패하면 아무것도 바꾸지 않고 원본을 그대로 진행시킵니다.
        /// </summary>
        /// <returns>원본 호출을 진행하면 true, 대체 호출을 마쳤으면 false.</returns>
        public static bool HandleSetAnimation(SpineActionController sac, string animationName, bool isLoop, ref TrackEntry result)
        {
            if (swappingAnimation || !inMultiHit) return true;
            if (sac == null || string.IsNullOrEmpty(animationName)) return true;

            if (StartDrivenEnabled)
            {
                // 시작에서 건 동작을 구간 내내 유지해야 하므로, 타격마다 들어오는 변경을 막습니다.
                // 막더라도 호출자가 후속 처리에 쓸 TrackEntry 는 돌려줘야 하므로
                // 지금 재생 중인 엔트리를 대신 넘깁니다. 그걸 얻지 못하면 막지 않습니다.
                if (animationName == HoldAnimation) return true;

                try
                {
                    var current = sac.skeletonAnimation.AnimationState.GetCurrent(0);
                    if (current == null) return true;

                    result = current;
                    suppressedCount++;
                    return false;
                }
                catch (Exception ex)
                {
                    MelonLogger.Warning($"[샌드백실험] 유지 실패, 원본 변경을 허용합니다: {ex.Message}");
                    return true;
                }
            }

            if (!AnimationSwapEnabled) return true;
            if (!AnimationSwapMap.TryGetValue(animationName, out string replacement)) return true;

            swappingAnimation = true;
            try
            {
                result = sac.SetAnimation(replacement, isLoop);
                MelonLogger.Msg($"[샌드백실험] 애니메이션 \"{animationName}\" → \"{replacement}\" 로 교체");
                return false;
            }
            catch (Exception ex)
            {
                MelonLogger.Warning($"[샌드백실험] 애니메이션 교체 실패, 원본을 유지합니다: {ex.Message}");
                return true;
            }
            finally
            {
                swappingAnimation = false;
            }
        }

        /// <summary>스테이지를 벗어날 때 구간 상태가 남아 있지 않도록 초기화합니다.</summary>
        public static void Reset()
        {
            inMultiHit = false;
            redirecting = false;
            swappingAnimation = false;
            suppressedCount = 0;
        }
    }

    // 이 실험에는 전용 [HarmonyPatch] 클래스가 없습니다.
    // PlayByKey 에는 계약서 프로브가 이미 Prefix 를 붙이고 있고, 같은 메서드에 패치 클래스를
    // 둘 붙였을 때 양쪽이 모두 실행된다는 확인을 아직 못 했습니다. 확인되지 않은 것에 기대는 대신
    // Patch_SpineContract_PlayByKey 하나에서 관측과 대체를 함께 처리합니다.
}
