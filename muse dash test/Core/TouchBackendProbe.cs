using System;
using System.Runtime.InteropServices;
using System.Text;
using MelonLoader;
using UnityEngine;

namespace muse_dash_test
{
    /// <summary>
    /// 터치스크린 입력이 어느 계층에서 끊기는지 확정하기 위한 1회성 진단 클래스입니다.
    /// ROG Ally / 스팀덱 등에서 Input.touchCount가 0으로 나오는 원인이
    /// (a) 하드웨어 디지타이저 부재 (b) Unity 레거시 Input의 Windows 터치 미지원
    /// (c) 새 Input System 백엔드 비활성 중 어느 것인지 한 번에 판별합니다.
    /// 진단이 끝나면 이 파일은 통째로 제거해도 됩니다.
    /// </summary>
    public static class TouchBackendProbe
    {
        private const int SM_DIGITIZER = 94;
        private const int SM_MAXIMUMTOUCHES = 95;

        // GetSystemMetrics(SM_DIGITIZER) 반환 플래그
        private const int NID_INTEGRATED_TOUCH = 0x01;
        private const int NID_EXTERNAL_TOUCH = 0x02;
        private const int NID_INTEGRATED_PEN = 0x04;
        private const int NID_EXTERNAL_PEN = 0x08;
        private const int NID_MULTI_INPUT = 0x40;
        private const int NID_READY = 0x80;

        // 터치에서 승격된 마우스 메시지의 서명값 (하위 8비트는 접점 식별자라 마스킹)
        private const uint MOUSEEVENTF_FROMTOUCH = 0xFF515700;

        [DllImport("user32.dll")]
        private static extern int GetSystemMetrics(int nIndex);

        [DllImport("user32.dll")]
        private static extern IntPtr GetMessageExtraInfo();

        private static bool _reported;
        private static float _timer;

        /// <summary>
        /// 매 프레임 호출합니다. 게임이 완전히 기동한 뒤(3초) 한 번만 진단 결과를 출력합니다.
        /// </summary>
        public static void Tick()
        {
            if (_reported) return;

            _timer += Time.unscaledDeltaTime;
            if (_timer < 3f) return;

            _reported = true;
            Report();
        }

        private static void Report()
        {
            var sb = new StringBuilder();
            sb.AppendLine("════════ [Touch Backend Probe] 터치 입력 계층 진단 ════════");

            // ── 1층: Windows가 디지타이저 하드웨어를 인식하는가 ────────────────
            try
            {
                int digitizer = GetSystemMetrics(SM_DIGITIZER);
                int maxTouches = GetSystemMetrics(SM_MAXIMUMTOUCHES);
                sb.AppendLine($"[1/3 Windows] SM_DIGITIZER=0x{digitizer:X2}, SM_MAXIMUMTOUCHES={maxTouches}");
                sb.AppendLine($"          ├ 내장터치={(digitizer & NID_INTEGRATED_TOUCH) != 0}, 외장터치={(digitizer & NID_EXTERNAL_TOUCH) != 0}");
                sb.AppendLine($"          ├ 내장펜={(digitizer & NID_INTEGRATED_PEN) != 0}, 외장펜={(digitizer & NID_EXTERNAL_PEN) != 0}");
                sb.AppendLine($"          └ 멀티터치={(digitizer & NID_MULTI_INPUT) != 0}, 스택준비됨(READY)={(digitizer & NID_READY) != 0}");
            }
            catch (Exception ex)
            {
                sb.AppendLine($"[1/3 Windows] P/Invoke 실패: {ex.Message}");
            }

            // ── 2층: Unity 레거시 Input이 터치를 받는가 ────────────────────────
            try
            {
                sb.AppendLine($"[2/3 Legacy] touchSupported={Input.touchSupported}, multiTouchEnabled={Input.multiTouchEnabled}, " +
                              $"simulateMouseWithTouches={Input.simulateMouseWithTouches}, touchCount={Input.touchCount}");
                sb.AppendLine($"          └ stylusTouchSupported={Input.stylusTouchSupported}, touchPressureSupported={Input.touchPressureSupported}");
            }
            catch (Exception ex)
            {
                sb.AppendLine($"[2/3 Legacy] 조회 실패: {ex.Message}");
            }

            // ── 3층: 새 Input System 백엔드가 살아 있는가 ──────────────────────
            ReportInputSystem(sb);

            sb.Append("═══════════════════════════════════════════════════════════");
            MelonLogger.Msg(sb.ToString());
        }

        /// <summary>
        /// 새 Input System은 Player Settings의 activeInputHandler가 꺼져 있으면
        /// DLL이 있어도 네이티브 백엔드가 동작하지 않으므로, 타입 존재 여부가 아니라
        /// 실제 디바이스 목록으로 판별해야 합니다.
        /// </summary>
        private static void ReportInputSystem(StringBuilder sb)
        {
            try
            {
                var touchscreen = UnityEngine.InputSystem.Touchscreen.current;
                sb.AppendLine($"[3/3 InputSystem] Touchscreen.current={(touchscreen != null ? touchscreen.name : "null")}");

                var devices = UnityEngine.InputSystem.InputSystem.devices;
                if (devices == null)
                {
                    sb.AppendLine("          └ devices=null (백엔드 비활성 의심)");
                    return;
                }

                sb.AppendLine($"          ├ 디바이스 {devices.Count}개:");
                for (int i = 0; i < devices.Count; i++)
                {
                    var d = devices[i];
                    sb.AppendLine($"          │   [{i}] {d.name} ({d.GetType().Name}), enabled={d.enabled}");
                }

                if (touchscreen != null)
                {
                    sb.AppendLine($"          └ 활성 접점(touches)={touchscreen.touches.Count}개 슬롯");
                }
            }
            catch (Exception ex)
            {
                sb.AppendLine($"[3/3 InputSystem] 조회 실패 → 백엔드 비활성 또는 미로드: {ex.GetType().Name}: {ex.Message}");
            }
        }

        /// <summary>
        /// 마우스 다운이 실제 마우스인지, 터치에서 승격된 것인지 판별합니다.
        /// 메시지 펌프 타이밍에 의존하므로 보조 증거로만 사용합니다.
        /// </summary>
        public static string DescribeMouseOrigin()
        {
            try
            {
                uint extra = (uint)GetMessageExtraInfo().ToInt64();
                bool fromTouch = (extra & 0xFFFFFF80) == MOUSEEVENTF_FROMTOUCH;
                return fromTouch ? $"터치승격(0x{extra:X8})" : $"실제마우스?(0x{extra:X8})";
            }
            catch (Exception ex)
            {
                return $"판별실패({ex.GetType().Name})";
            }
        }
    }
}
