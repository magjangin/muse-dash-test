using System;
using System.Collections.Generic;
using MelonLoader;

namespace muse_dash_test
{
    /// <summary>
    /// 기능 단위 실행 격리 게이트입니다.
    /// 한 기능에서 발생한 예외가 다른 기능이나 MelonLoader 라이프사이클 전체를 끌어내리지 않도록
    /// 각 호출을 격리하고, 다음 두 가지 안정성 장치를 제공합니다.
    ///   1) 로그 스로틀링: 동일 오류가 매 프레임 반복돼도 처음 1회만 상세 로깅하여 로그 폭발을 방지합니다.
    ///   2) 서킷 브레이커: 한 기능이 연속으로 일정 횟수 실패하면 해당 기능만 자동 비활성화하여
    ///      매 프레임 예외로 인한 프레임 드랍을 막습니다. (게임 업데이트로 패치 대상이 사라진 경우 등)
    /// 비활성화된 기능은 씬 전환 시 <see cref="RearmAll"/>로 1회 재시도 기회를 얻습니다.
    /// </summary>
    public static class FeatureGuard
    {
        /// <summary>연속 실패가 이 횟수에 도달하면 해당 기능을 자동 비활성화합니다.</summary>
        private const int DefaultMaxConsecutiveFailures = 10;

        private sealed class State
        {
            public int ConsecutiveFailures;
            public bool Disabled;
            public string LastErrorText;
        }

        private static readonly Dictionary<string, State> States = new Dictionary<string, State>();

        /// <summary>
        /// 지정한 기능 본문을 격리 실행합니다. 예외는 외부로 전파되지 않습니다.
        /// </summary>
        /// <param name="feature">로그/통계에 사용할 기능 식별 이름.</param>
        /// <param name="body">실행할 기능 본문.</param>
        /// <param name="maxConsecutiveFailures">서킷 브레이커 임계치(기본 10). 0 이하면 자동 비활성화하지 않습니다.</param>
        /// <returns>본문이 예외 없이 실행되면 true, 비활성화 상태이거나 예외 발생 시 false.</returns>
        public static bool Run(string feature, Action body, int maxConsecutiveFailures = DefaultMaxConsecutiveFailures)
        {
            if (body == null) return false;

            State state;
            if (!States.TryGetValue(feature, out state))
            {
                state = new State();
                States[feature] = state;
            }

            if (state.Disabled) return false;

            try
            {
                body();

                // 이전에 실패하다가 정상 복구된 경우 한 번 알립니다.
                if (state.ConsecutiveFailures > 0)
                {
                    MelonLogger.Msg($"[FeatureGuard] '{feature}' 기능이 정상 복구되었습니다.");
                    state.ConsecutiveFailures = 0;
                    state.LastErrorText = null;
                }
                return true;
            }
            catch (Exception ex)
            {
                state.ConsecutiveFailures++;

                // 동일 오류 반복 시 로그 스로틀링: 첫 발생 또는 오류 내용이 바뀐 경우에만 상세 로깅.
                string errorText = ex.ToString();
                if (state.ConsecutiveFailures == 1 || errorText != state.LastErrorText)
                {
                    MelonLogger.Error($"[FeatureGuard] '{feature}' 실행 오류 (연속 {state.ConsecutiveFailures}회): {ex}");
                    state.LastErrorText = errorText;
                }

                if (maxConsecutiveFailures > 0 && state.ConsecutiveFailures >= maxConsecutiveFailures)
                {
                    state.Disabled = true;
                    MelonLogger.Error(
                        $"[FeatureGuard] '{feature}' 기능이 {maxConsecutiveFailures}회 연속 실패하여 자동 비활성화되었습니다. " +
                        "게임 업데이트로 패치 대상이 변경되었을 수 있습니다. (씬 전환 시 1회 재시도)");
                }
                return false;
            }
        }

        /// <summary>
        /// 자동 비활성화된 모든 기능을 재무장(재시도 가능 상태)합니다.
        /// 씬 전환처럼 게임 상태가 크게 바뀌는 시점에 호출하여, 특정 씬에서만 일시적으로
        /// 실패하던 기능이 영구히 꺼진 채 남지 않도록 1회 재시도 기회를 줍니다.
        /// </summary>
        public static void RearmAll()
        {
            foreach (var kv in States)
            {
                if (kv.Value.Disabled)
                {
                    kv.Value.Disabled = false;
                    kv.Value.ConsecutiveFailures = 0;
                    kv.Value.LastErrorText = null;
                    MelonLogger.Msg($"[FeatureGuard] '{kv.Key}' 기능을 재시도 가능 상태로 되돌렸습니다.");
                }
            }
        }

        /// <summary>지정 기능이 서킷 브레이커로 비활성화되었는지 여부입니다.</summary>
        public static bool IsDisabled(string feature)
        {
            return States.TryGetValue(feature, out var state) && state.Disabled;
        }
    }
}
