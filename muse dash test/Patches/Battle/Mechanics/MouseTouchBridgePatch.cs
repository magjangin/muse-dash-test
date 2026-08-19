using System;
using HarmonyLib;
using MelonLoader;
using UnityEngine;
using Il2CppAssets.Scripts.GameCore.Controller;
using Il2CppAssets.Scripts.PeroTools.Managers;
using Il2CppGameLogic;
using Il2CppPeroPeroGames.GlobalDefines;
using Il2CppSystem.Collections.Generic;

namespace muse_dash_test.Patches.Battle.Mechanics
{
    /// <summary>
    /// PC 환경의 StandloneController 및 InputManager에서 마우스 클릭 및 화면 터치 입력을 가로채어
    /// BATTLE_AIR / BATTLE_GROUND 판정 및 체공(Hold)으로 직접 주입하는 브릿지 패치 클래스입니다.
    /// PnlInputMobile의 좌우/상하 분할 모드 및 반전 설정이 100% 반영됩니다.
    /// </summary>
    [HarmonyPatch]
    public static class MouseTouchBridgePatch
    {
        // 마우스 버튼별 현재 입력된 Air 상태 추적
        private static bool _btn0IsAir = true;
        private static bool _btn1IsAir = false;

        #region StandloneController Patches

        [HarmonyPatch(typeof(StandloneController), nameof(StandloneController.GetButtonDown))]
        [HarmonyPostfix]
        public static void Standlone_GetButtonDown_Postfix(StandloneController __instance, MDButtonType buttonName, ref List<int> __result)
        {
            InjectButtonDown(buttonName, ref __result);
        }

        [HarmonyPatch(typeof(StandloneController), nameof(StandloneController.GetButton))]
        [HarmonyPostfix]
        public static void Standlone_GetButton_Postfix(StandloneController __instance, MDButtonType buttonName, ref List<int> __result)
        {
            InjectButtonHold(buttonName, ref __result);
        }

        [HarmonyPatch(typeof(StandloneController), nameof(StandloneController.GetButtonUp))]
        [HarmonyPostfix]
        public static void Standlone_GetButtonUp_Postfix(StandloneController __instance, MDButtonType buttonName, ref List<int> __result)
        {
            InjectButtonUp(buttonName, ref __result);
        }

        #endregion

        #region InputManager Patches

        [HarmonyPatch(typeof(InputManager), nameof(InputManager.GetButtonDown), new Type[] { typeof(MDButtonType) })]
        [HarmonyPostfix]
        public static void InputManager_GetButtonDown_Postfix(MDButtonType buttonName, ref List<int> __result)
        {
            InjectButtonDown(buttonName, ref __result);
        }

        [HarmonyPatch(typeof(InputManager), nameof(InputManager.GetButton), new Type[] { typeof(MDButtonType) })]
        [HarmonyPostfix]
        public static void InputManager_GetButton_Postfix(MDButtonType buttonName, ref List<int> __result)
        {
            InjectButtonHold(buttonName, ref __result);
        }

        [HarmonyPatch(typeof(InputManager), nameof(InputManager.GetButtonUp), new Type[] { typeof(MDButtonType) })]
        [HarmonyPostfix]
        public static void InputManager_GetButtonUp_Postfix(MDButtonType buttonName, ref List<int> __result)
        {
            InjectButtonUp(buttonName, ref __result);
        }

        #endregion

        #region Core Injection Logic

        private static void InjectButtonDown(MDButtonType buttonName, ref List<int> result)
        {
            try
            {
                if (result == null) result = new List<int>();

                // 1. 마우스 좌클릭 (Finger 0)
                if (Input.GetMouseButtonDown(0))
                {
                    _btn0IsAir = CalculateIsAir(Input.mousePosition);
                    if (IsMatchingButton(_btn0IsAir, buttonName))
                    {
                        if (!result.Contains(0)) result.Add(0);
                        MelonLogger.Msg($"🖱️ [MouseTouch Down] 좌클릭 -> {buttonName} (Air: {_btn0IsAir}, X={Input.mousePosition.x:F0}, Y={Input.mousePosition.y:F0})");
                    }
                }

                // 2. 마우스 우클릭 (Finger 1, 연타 및 멀티터치 지원)
                if (Input.GetMouseButtonDown(1))
                {
                    _btn1IsAir = CalculateIsAir(Input.mousePosition);
                    if (IsMatchingButton(_btn1IsAir, buttonName))
                    {
                        if (!result.Contains(1)) result.Add(1);
                        MelonLogger.Msg($"🖱️ [MouseTouch Down] 우클릭 -> {buttonName} (Air: {_btn1IsAir}, X={Input.mousePosition.x:F0}, Y={Input.mousePosition.y:F0})");
                    }
                }

                // 3. 실제 터치스크린 터치
                int touchCount = Input.touchCount;
                for (int i = 0; i < touchCount; i++)
                {
                    Touch touch = Input.GetTouch(i);
                    if (touch.phase == TouchPhase.Began)
                    {
                        bool isAir = CalculateIsAir(touch.position);
                        if (IsMatchingButton(isAir, buttonName))
                        {
                            int fingerKey = touch.fingerId % 2;
                            if (!result.Contains(fingerKey)) result.Add(fingerKey);
                            MelonLogger.Msg($"📱 [ScreenTouch Down] 터치 ID {touch.fingerId} -> {buttonName} (Air: {isAir})");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[MouseTouchBridge] InjectButtonDown 에러: {ex}");
            }
        }

        private static void InjectButtonHold(MDButtonType buttonName, ref List<int> result)
        {
            try
            {
                if (result == null) result = new List<int>();

                // 1. 마우스 좌클릭 홀드 (점프 체공 및 롱노트 유지)
                if (Input.GetMouseButton(0))
                {
                    if (IsMatchingButton(_btn0IsAir, buttonName))
                    {
                        if (!result.Contains(0)) result.Add(0);
                    }
                }

                // 2. 마우스 우클릭 홀드
                if (Input.GetMouseButton(1))
                {
                    if (IsMatchingButton(_btn1IsAir, buttonName))
                    {
                        if (!result.Contains(1)) result.Add(1);
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
                        if (IsMatchingButton(isAir, buttonName))
                        {
                            int fingerKey = touch.fingerId % 2;
                            if (!result.Contains(fingerKey)) result.Add(fingerKey);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[MouseTouchBridge] InjectButtonHold 에러: {ex}");
            }
        }

        private static void InjectButtonUp(MDButtonType buttonName, ref List<int> result)
        {
            try
            {
                if (result == null) result = new List<int>();

                // 1. 마우스 좌클릭 릴리즈
                if (Input.GetMouseButtonUp(0))
                {
                    if (IsMatchingButton(_btn0IsAir, buttonName))
                    {
                        if (!result.Contains(0)) result.Add(0);
                        MelonLogger.Msg($"🖱️ [MouseTouch Up] 좌클릭 릴리즈 -> {buttonName}");
                    }
                }

                // 2. 마우스 우클릭 릴리즈
                if (Input.GetMouseButtonUp(1))
                {
                    if (IsMatchingButton(_btn1IsAir, buttonName))
                    {
                        if (!result.Contains(1)) result.Add(1);
                        MelonLogger.Msg($"🖱️ [MouseTouch Up] 우클릭 릴리즈 -> {buttonName}");
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
                        if (IsMatchingButton(isAir, buttonName))
                        {
                            int fingerKey = touch.fingerId % 2;
                            if (!result.Contains(fingerKey)) result.Add(fingerKey);
                            MelonLogger.Msg($"📱 [ScreenTouch Up] 터치 ID {touch.fingerId} 릴리즈 -> {buttonName}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[MouseTouchBridge] InjectButtonUp 에러: {ex}");
            }
        }

        private static bool IsMatchingButton(bool isAir, MDButtonType buttonName)
        {
            if (isAir && buttonName == MDButtonType.BATTLE_AIR) return true;
            if (!isAir && buttonName == MDButtonType.BATTLE_GROUND) return true;
            return false;
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

        #endregion
    }
}
