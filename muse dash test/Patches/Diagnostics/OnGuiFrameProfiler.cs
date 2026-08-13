using MelonLoader;
using UnityEngine;

namespace muse_dash_test
{
    /// <summary>
    /// OnGUI가 한 프레임에 몇 번 호출되는지를 실측해 1회성 요약 로그로 남기고,
    /// 실제 픽셀이 찍히는 Repaint 이벤트에서만 그리도록 게이트를 제공합니다.
    ///
    /// <para><b>왜 필요한가</b>: Unity IMGUI의 OnGUI는 프레임당 1회가 아닙니다.
    /// Layout 1회 + Repaint 1회 + <i>입력 이벤트 1건당 1회</i>씩 호출됩니다.
    /// 리듬 게임은 초당 수십 건의 KeyDown/KeyUp이 발생하므로, 게이트가 없으면
    /// 키 입력 오버레이와 판정바 전체가 그 횟수만큼 다시 그려져 IMGUI 드로우 콜이 배수로 뜁니다.
    /// Repaint에서만 그리면 프레임당 정확히 1회로 고정됩니다.</para>
    ///
    /// <para><b>안전장치</b>: 만약 이 호스트에서 Repaint 이벤트가 관측되지 않으면(런타임/후킹 방식 차이)
    /// 오버레이가 통째로 사라지므로, 관측 구간이 끝날 때까지 Repaint를 한 번도 못 봤으면
    /// 게이트를 풀고(=항상 그리기) 경고를 남깁니다.</para>
    /// </summary>
    public static class OnGuiFrameProfiler
    {
        /// <summary>이 프레임 수만큼만 표본을 모은 뒤 요약 1줄을 남기고 계측을 멈춥니다.</summary>
        private const int SampleFrames = 180;

        /// <summary>OnGUI 호출이 이 프레임 수 이상 끊기면 스테이지를 벗어났다 돌아온 것으로 보고 다시 계측합니다.</summary>
        private const int SessionGapFrames = 60;

        private static int lastSeenFrame = -1;
        private static int firstSampleFrame = -1;
        private static int sampledFrames;
        private static int totalCalls;
        private static int repaintCalls;
        private static bool sawRepaint;
        private static bool reported;
        private static bool gateDisabled;

        /// <summary>
        /// OnGUI 진입 시 매번 호출합니다. 호출 횟수를 집계하고, 이번 이벤트에서 그려도 되는지 반환합니다.
        /// </summary>
        public static bool ShouldDraw()
        {
            int frame = Time.frameCount;

            // 스테이지를 벗어났다 다시 들어온 경우: 표본을 새로 모읍니다.
            if (lastSeenFrame >= 0 && frame - lastSeenFrame > SessionGapFrames)
            {
                Reset();
            }

            if (frame != lastSeenFrame)
            {
                sampledFrames++;
                lastSeenFrame = frame;
                if (firstSampleFrame < 0) firstSampleFrame = frame;
            }

            totalCalls++;

            bool isRepaint = Event.current != null && Event.current.type == EventType.Repaint;
            if (isRepaint)
            {
                repaintCalls++;
                sawRepaint = true;
            }

            if (!reported && sampledFrames >= SampleFrames)
            {
                Report();
            }

            // 게이트가 풀린 상태(= Repaint를 못 본 환경)에서는 예전처럼 모든 이벤트에서 그립니다.
            return gateDisabled || isRepaint;
        }

        private static void Report()
        {
            reported = true;

            float callsPerFrame = sampledFrames > 0 ? totalCalls / (float)sampledFrames : 0f;

            if (!sawRepaint)
            {
                gateDisabled = true;
                MelonLogger.Warning(
                    $"[OnGuiProfiler] {sampledFrames}프레임 동안 Repaint 이벤트를 한 번도 관측하지 못했습니다. " +
                    "오버레이가 사라지는 것을 막기 위해 Repaint 게이트를 해제하고 모든 OnGUI 이벤트에서 그립니다. " +
                    $"(OnGUI 호출 {totalCalls}회, 프레임당 {callsPerFrame:0.00}회)");
                return;
            }

            int skipped = totalCalls - repaintCalls;
            MelonLogger.Msg(
                $"[OnGuiProfiler] 인게임 {sampledFrames}프레임 실측: OnGUI 총 {totalCalls}회 호출 " +
                $"(프레임당 {callsPerFrame:0.00}회) / 그중 Repaint {repaintCalls}회. " +
                $"Repaint 게이트가 오버레이·판정바 드로우 {skipped}회를 걸러냈습니다 " +
                $"(드로우량 {(totalCalls > 0 ? (1f - repaintCalls / (float)totalCalls) * 100f : 0f):0.0}% 감소).");
        }

        /// <summary>표본을 초기화합니다. 게이트 해제 상태(<c>gateDisabled</c>)는 유지합니다.</summary>
        public static void Reset()
        {
            lastSeenFrame = -1;
            firstSampleFrame = -1;
            sampledFrames = 0;
            totalCalls = 0;
            repaintCalls = 0;
            sawRepaint = false;
            reported = false;
        }
    }
}
