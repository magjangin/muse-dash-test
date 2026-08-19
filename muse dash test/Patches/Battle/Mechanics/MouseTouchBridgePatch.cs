using System;
using HarmonyLib;
using MelonLoader;
using UnityEngine;
using Il2CppGameLogic;

namespace muse_dash_test.Patches.Battle.Mechanics
{
    /// <summary>
    /// PC 마우스 클릭(좌클릭/우클릭)을 모바일 터치 이벤트(GameTouchPlay)로 실시간 변환하는 브릿지 패치 클래스입니다.
    /// PnlInputMobile에서 설정한 좌우/상하 분할 모드 및 반전 설정이 100% 그대로 반영됩니다.
    /// </summary>
    [HarmonyPatch(typeof(GameTouchPlay), nameof(GameTouchPlay.TimeUpdateStep))]
    public static class MouseTouchBridgePatch
    {
        // 마우스 버튼별 현재 입력된 Air/Ground 상태 추적
        private static bool _btn0IsAir = false;
        private static bool _btn0Pressed = false;

        private static bool _btn1IsAir = false;
        private static bool _btn1Pressed = false;

        public static void Postfix(GameTouchPlay __instance)
        {
            if (__instance == null) return;

            try
            {
                // 1. 마우스 좌클릭 (첫 번째 터치 손가락: keyIndex = 0)
                HandleMouseButton(0, ref _btn0Pressed, ref _btn0IsAir, __instance);

                // 2. 마우스 우클릭 (두 번째 터치 손가락: keyIndex = 1, 멀티터치 및 연타 대응)
                HandleMouseButton(1, ref _btn1Pressed, ref _btn1IsAir, __instance);
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[MouseTouchBridge] 마우스-터치 브릿지 처리 중 예외 발생: {ex}");
            }
        }

        private static void HandleMouseButton(int button, ref bool isPressed, ref bool isAirRecorded, GameTouchPlay touchPlay)
        {
            // 버튼을 눌렀을 때 (Touch Phase: Began)
            if (Input.GetMouseButtonDown(button))
            {
                Vector3 mousePos = Input.mousePosition;
                bool isAir = CalculateIsAir(mousePos);

                isPressed = true;
                isAirRecorded = isAir;

                touchPlay.AddPressKey(isAir, button);
                touchPlay.AddPress(isAir, button);
                touchPlay.TouchTrigger(0);

                MelonLogger.Msg($"🖱️ [MouseTouch] 마우스 버튼 {button} 클릭 -> {(isAir ? "공중(Air/Jump)" : "지상(Ground/Punch)")} 터치 주입! (좌표: X={mousePos.x:F0}, Y={mousePos.y:F0})");
            }
            // 버튼을 뗐을 때 (Touch Phase: Ended)
            else if (Input.GetMouseButtonUp(button))
            {
                if (isPressed)
                {
                    touchPlay.RemovePressKey(isAirRecorded, button);
                    touchPlay.RemovePress(isAirRecorded, button);
                    touchPlay.RemovePressKey(!isAirRecorded, button);
                    touchPlay.RemovePress(!isAirRecorded, button);

                    isPressed = false;
                }
            }
        }

        /// <summary>
        /// PnlInputMobile 설정(좌우/상하 모드, 반전 모드)에 따라 마우스 위치가 공중(Air)인지 지상(Ground)인지 계산합니다.
        /// </summary>
        private static bool CalculateIsAir(Vector3 screenPos)
        {
            bool isLeftRight = GameTouchPlay.isTouchLeftRight;
            bool isReverse = GameTouchPlay.isTouchReverse || GameTouchPlay.isReverse;

            if (isLeftRight)
            {
                // [좌우 분할 모드] 기본값: 화면 왼쪽 = 공중(Air), 오른쪽 = 지상(Ground)
                bool isLeft = screenPos.x < (Screen.width * 0.5f);
                return isReverse ? !isLeft : isLeft;
            }
            else
            {
                // [상하 분할 모드] 기본값: 화면 위쪽 = 공중(Air), 아래쪽 = 지상(Ground)
                bool isTop = screenPos.y > (Screen.height * 0.5f);
                return isReverse ? !isTop : isTop;
            }
        }
    }
}
