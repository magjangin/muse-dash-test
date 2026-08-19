using System;
using System.Collections.Generic;
using MelonLoader;
using UnityEngine;
using UnityEngine.InputSystem;

namespace muse_dash_test
{
    /// <summary>
    /// 터치스크린 접점 하나의 이번 프레임 상태입니다.
    /// </summary>
    public struct TouchContact
    {
        /// <summary>접점 슬롯 인덱스(0~9). 게임에 넘길 입력 식별자로 사용합니다.</summary>
        public int Id;
        public Vector2 Position;
        public bool Began;
        public bool Held;
        public bool Ended;
    }

    /// <summary>
    /// 터치스크린 입력 판독기입니다.
    ///
    /// Unity 2019.4의 레거시 <c>UnityEngine.Input</c>은 Windows 스탠드얼론에서
    /// 디지타이저를 읽지 못합니다(ROG Ally 실측: touchSupported=True인데 touchCount=0).
    /// 반면 게임에 탑재된 새 Input System은 Touchscreen 디바이스를 10접점으로
    /// 정상 노출하므로(실측 확인), 이쪽을 1순위로 사용합니다.
    ///
    /// 레거시 경로는 다른 환경을 위한 폴백으로만 남깁니다.
    /// </summary>
    public static class TouchInput
    {
        private static readonly List<TouchContact> _contacts = new List<TouchContact>(10);
        private static int _cachedFrame = -1;
        private static bool _errorLogged;
        private static bool _backendLogged;
        private static float _lastContactTime = float.NegativeInfinity;

        /// <summary>
        /// 마지막 접점 이후 마우스를 무시할 유예 시간(초)입니다.
        ///
        /// Windows의 마우스 승격은 터치보다 한두 프레임 늦게 도착하므로, 손가락을 떼는
        /// 순간 접점이 사라진 직후에 승격된 마우스 릴리즈가 뒤늦게 들어와 엉뚱한 레인에
        /// 유령 입력을 넣을 수 있습니다. 이를 막기 위한 여유입니다.
        /// </summary>
        private const float MouseSuppressGrace = 0.25f;

        /// <summary>사용 가능한 터치 백엔드 이름 (진단 로그용).</summary>
        public static string ActiveBackend { get; private set; } = "미확인";

        /// <summary>
        /// 이번 프레임의 활성 접점 목록입니다. 프레임당 1회만 실제 조회하고 이후 캐시를 돌려줍니다.
        /// </summary>
        public static List<TouchContact> GetContacts()
        {
            int frame = Time.frameCount;
            if (frame == _cachedFrame) return _contacts;
            _cachedFrame = frame;

            _contacts.Clear();

            if (!TryReadInputSystem())
            {
                ReadLegacy();
            }

            if (_contacts.Count > 0)
            {
                _lastContactTime = Time.unscaledTime;

                // 어느 백엔드로 터치가 처리되는지 세션당 한 줄만 남깁니다.
                // 나중에 문제 제보를 받았을 때 경로를 되짚을 최소한의 단서입니다.
                if (!_backendLogged)
                {
                    _backendLogged = true;
                    MelonLogger.Msg($"[TouchInput] 터치스크린 입력을 {ActiveBackend} 백엔드로 처리합니다.");
                }
            }

            return _contacts;
        }

        /// <summary>
        /// 이번 프레임에 화면에 닿아 있는 손가락이 하나라도 있는지 여부입니다.
        /// true인 동안에는 Windows가 승격시킨 마우스 이벤트를 무시해야 중복 판정이 나지 않습니다.
        /// </summary>
        public static bool AnyContactThisFrame()
        {
            return GetContacts().Count > 0;
        }

        /// <summary>
        /// 지금 마우스 입력을 무시해야 하는지 여부입니다.
        ///
        /// 터치스크린에 손가락이 닿아 있는 동안 Windows는 첫 손가락을 마우스 클릭으로도
        /// 승격시켜 보냅니다. 두 경로를 모두 받으면 한 번의 터치가 두 번 판정되므로,
        /// 접점이 있는 동안과 그 직후 유예 시간에는 마우스를 버립니다.
        /// 손가락이 화면에 없을 때는 실제 마우스가 정상 동작합니다.
        /// </summary>
        public static bool ShouldIgnoreMouse()
        {
            if (GetContacts().Count > 0) return true;
            return (Time.unscaledTime - _lastContactTime) < MouseSuppressGrace;
        }

        /// <summary>
        /// 새 Input System에서 접점을 읽습니다. Touchscreen 디바이스가 없으면 false를 반환합니다.
        /// </summary>
        private static bool TryReadInputSystem()
        {
            try
            {
                var screen = Touchscreen.current;
                if (screen == null) return false;

                var touches = screen.touches;
                int slotCount = touches.Count;

                for (int i = 0; i < slotCount; i++)
                {
                    var t = touches[i];
                    if (t == null) continue;

                    bool began = t.press.wasPressedThisFrame;
                    bool ended = t.press.wasReleasedThisFrame;
                    bool held = t.press.isPressed;

                    // 눌리지도, 떼어지지도, 유지되지도 않은 슬롯은 비어 있는 것입니다.
                    if (!began && !ended && !held) continue;

                    _contacts.Add(new TouchContact
                    {
                        // 게임에 넘기는 식별자는 슬롯 인덱스를 씁니다.
                        // Input System의 touchId는 단조 증가하여 값이 무한정 커지므로 부적합합니다.
                        Id = i,
                        Position = t.position.ReadValue(),
                        Began = began,
                        Held = held,
                        Ended = ended
                    });
                }

                ActiveBackend = "InputSystem";
                return true;
            }
            catch (Exception ex)
            {
                if (!_errorLogged)
                {
                    _errorLogged = true;
                    MelonLogger.Warning($"[TouchInput] Input System 판독 실패, 레거시로 폴백합니다: {ex.GetType().Name}: {ex.Message}");
                }
                return false;
            }
        }

        /// <summary>
        /// 레거시 UnityEngine.Input 폴백입니다. Windows 스탠드얼론에서는 동작하지 않습니다.
        /// </summary>
        private static void ReadLegacy()
        {
            try
            {
                int count = Input.touchCount;
                if (count <= 0)
                {
                    ActiveBackend = "없음";
                    return;
                }

                for (int i = 0; i < count; i++)
                {
                    Touch touch = Input.GetTouch(i);
                    var phase = touch.phase;

                    _contacts.Add(new TouchContact
                    {
                        Id = touch.fingerId,
                        Position = touch.position,
                        Began = phase == UnityEngine.TouchPhase.Began,
                        Held = phase == UnityEngine.TouchPhase.Moved || phase == UnityEngine.TouchPhase.Stationary,
                        Ended = phase == UnityEngine.TouchPhase.Ended || phase == UnityEngine.TouchPhase.Canceled
                    });
                }

                ActiveBackend = "Legacy";
            }
            catch (Exception ex)
            {
                if (!_errorLogged)
                {
                    _errorLogged = true;
                    MelonLogger.Warning($"[TouchInput] 레거시 판독 실패: {ex.GetType().Name}: {ex.Message}");
                }
            }
        }
    }
}
