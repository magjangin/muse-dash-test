using System;
using HarmonyLib;
using MelonLoader;
using UnityEngine;
using Il2CppAssets.Scripts.GameCore.Controller;
using Il2CppGameLogic;
using Il2CppPeroPeroGames.GlobalDefines;
using Il2CppSystem.Collections.Generic;

namespace muse_dash_test.Patches.Battle.Mechanics
{
    /// <summary>
    /// PC 환경의 StandloneController에서 마우스 클릭 및 화면 터치 입력을 가로채어
    /// BATTLE_AIR / BATTLE_GROUND 판정으로 직접 주입하는 브릿지 패치 클래스입니다.
    /// PnlInputMobile의 좌우/상하 분할 모드 및 반전 설정이 100% 반영됩니다.
    /// </summary>
    [HarmonyPatch(typeof(StandloneController))]
    public static class MouseTouchBridgePatch
    {
        private static bool _btn0IsAir = false;
        private static bool _btn1IsAir = false;

        [HarmonyPatch(nameof(StandloneController.GetButtonDown))]
        [HarmonyPostfix]
        public static void GetButtonDown_Postfix(StandloneController __instance, MDButtonType buttonName, ref List<int> __result)
        {
            try
            {
                if (__result == null) __result = new List<int>();

                // 1. 마우스 좌클릭 (Finger 0)
                if (Input.GetMouseButtonDown(0))
                {
                    _btn0IsAir = CalculateIsAir(Input.mousePosition);
                    bool match = (_btn0IsAir && buttonName == MDButtonType.BATTLE_AIR) ||
                                 (!_btn0IsAir && buttonName == MDButtonType.BATTLE_GROUND);
                    if (match)
                    {
                        __result.Add(100);
                        MelonLogger.Msg($"🖱️ [MouseTouch Down] 마우스 좌클릭 -> {buttonName} (Air: {_btn0IsAir}, X={Input.mousePosition.x:F0}, Y={Input.mousePosition.y:F0})");
                    }
                }

                // 2. 마우스 우클릭 (Finger 1, 멀티터치 및 연타 지원)
                if (Input.GetMouseButtonDown(1))
                {
                    _btn1IsAir = CalculateIsAir(Input.mousePosition);
                    bool match = (_btn1IsAir && buttonName == MDButtonType.BATTLE_AIR) ||
                                 (!_btn1IsAir && buttonName == MDButtonType.BATTLE_GROUND);
                    if (match)
                    {
                        __result.Add(101);
                        MelonLogger.Msg($"🖱️ [MouseTouch Down] 마우스 우클릭 -> {buttonName} (Air: {_btn1IsAir}, X={Input.mousePosition.x:F0}, Y={Input.mousePosition.y:F0})");
                    }
                }

                // 3. 실제 터치스크린 터치(Input.touches) 처리
                int touchCount = Input.touchCount;
                for (int i = 0; i < touchCount; i++)
                {
                    Touch touch = Input.GetTouch(i);
                    if (touch.phase == TouchPhase.Began)
                    {
                        bool isAir = CalculateIsAir(touch.position);
                        bool match = (isAir && buttonName == MDButtonType.BATTLE_AIR) ||
                                     (!isAir && buttonName == MDButtonType.BATTLE_GROUND);
                        if (match)
                        {
                            __result.Add(200 + touch.fingerId);
                            MelonLogger.Msg($"📱 [ScreenTouch Down] 터치 ID {touch.fingerId} -> {buttonName} (Air: {isAir})");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[MouseTouchBridge] GetButtonDown 에러: {ex}");
            }
        }

        [HarmonyPatch(nameof(StandloneController.GetButton))]
        [HarmonyPostfix]
        public static void GetButton_Postfix(StandloneController __instance, MDButtonType buttonName, ref List<int> __result)
        {
            try
            {
                if (__result == null) __result = new List<int>();

                // 1. 마우스 좌클릭 홀드 (롱노트 지원)
                if (Input.GetMouseButton(0))
                {
                    bool match = (_btn0IsAir && buttonName == MDButtonType.BATTLE_AIR) ||
                                 (!_btn0IsAir && buttonName == MDButtonType.BATTLE_GROUND);
                    if (match)
                    {
                        __result.Add(100);
                    }
                }

                // 2. 마우스 우클릭 홀드 (롱노트 지원)
                if (Input.GetMouseButton(1))
                {
                    bool match = (_btn1IsAir && buttonName == MDButtonType.BATTLE_AIR) ||
                                 (!_btn1IsAir && buttonName == MDButtonType.BATTLE_GROUND);
                    if (match)
                    {
                        __result.Add(101);
                    }
                }

                // 3. 터치스크린 홀드
                int touchCount = Input.touchCount;
                for (int i = 0; i < touchCount; i++)
                {
                    Touch touch = Input.GetTouch(i);
                    if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
                    {
                        bool isAir = CalculateIsAir(touch.position);
                        bool match = (isAir && buttonName == MDButtonType.BATTLE_AIR) ||
                                     (!isAir && buttonName == MDButtonType.BATTLE_GROUND);
                        if (match)
                        {
                            __result.Add(200 + touch.fingerId);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[MouseTouchBridge] GetButton 에러: {ex}");
            }
        }

        [HarmonyPatch(nameof(StandloneController.GetButtonUp))]
        [HarmonyPostfix]
        public static void GetButtonUp_Postfix(StandloneController __instance, MDButtonType buttonName, ref List<int> __result)
        {
            try
            {
                if (__result == null) __result = new List<int>();

                // 1. 마우스 좌클릭 릴리즈
                if (Input.GetMouseButtonUp(0))
                {
                    bool match = (_btn0IsAir && buttonName == MDButtonType.BATTLE_AIR) ||
                                 (!_btn0IsAir && buttonName == MDButtonType.BATTLE_GROUND);
                    if (match)
                    {
                        __result.Add(100);
                        MelonLogger.Msg($"🖱️ [MouseTouch Up] 마우스 좌클릭 릴리즈 -> {buttonName}");
                    }
                }

                // 2. 마우스 우클릭 릴리즈
                if (Input.GetMouseButtonUp(1))
                {
                    bool match = (_btn1IsAir && buttonName == MDButtonType.BATTLE_AIR) ||
                                 (!_btn1IsAir && buttonName == MDButtonType.BATTLE_GROUND);
                    if (match)
                    {
                        __result.Add(101);
                        MelonLogger.Msg($"🖱️ [MouseTouch Up] 마우스 우클릭 릴리즈 -> {buttonName}");
                    }
                }

                // 3. 터치스크린 릴리즈
                int touchCount = Input.touchCount;
                for (int i = 0; i < touchCount; i++)
                {
                    Touch touch = Input.GetTouch(i);
                    if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                    {
                        bool isAir = CalculateIsAir(touch.position);
                        bool match = (isAir && buttonName == MDButtonType.BATTLE_AIR) ||
                                     (!isAir && buttonName == MDButtonType.BATTLE_GROUND);
                        if (match)
                        {
                            __result.Add(200 + touch.fingerId);
                            MelonLogger.Msg($"📱 [ScreenTouch Up] 터치 ID {touch.fingerId} 릴리즈 -> {buttonName}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[MouseTouchBridge] GetButtonUp 에러: {ex}");
            }
        }

        /// <summary>
        /// PnlInputMobile 설정(좌우/상하 모드, 반전 모드)에 따라 화면 좌표가 공중(Air)인지 지상(Ground)인지 계산합니다.
        /// </summary>
        private static bool CalculateIsAir(Vector2 screenPos)
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
