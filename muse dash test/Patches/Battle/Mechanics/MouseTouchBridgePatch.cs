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
    /// PC 환경에서 마우스 클릭 및 화면 터치 입력을 가로채어
    /// BATTLE_AIR / BATTLE_GROUND의 상호 배타적(동시 입력 충돌 방지) 판정을 보장하는 브릿지 패치 클래스입니다.
    /// PC 기본 마우스 키 매핑에 의한 Ground/Air 동시 발동 버그를 완벽하게 차단합니다.
    /// </summary>
    [HarmonyPatch]
    public static class MouseTouchBridgePatch
    {
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

                // 마우스 좌클릭 처리
                if (Input.GetMouseButtonDown(0))
                {
                    _btn0IsAir = CalculateIsAir(Input.mousePosition);

                    // 클릭한 위치가 공중(Air)일 때
                    if (_btn0IsAir)
                    {
                        if (buttonName == MDButtonType.BATTLE_AIR)
                        {
                            if (!result.Contains(0)) result.Add(0);
                            MelonLogger.Msg($"🖱️ [Touch Down] 공중(AIR) 단독 입력 (X={Input.mousePosition.x:F0}, Y={Input.mousePosition.y:F0})");
                        }
                        else if (buttonName == MDButtonType.BATTLE_GROUND)
                        {
                            // 기본 PC 마우스 매핑으로 인한 지상 동시 발동 차단!
                            result.Clear();
                        }
                    }
                    // 클릭한 위치가 지상(Ground)일 때
                    else
                    {
                        if (buttonName == MDButtonType.BATTLE_GROUND)
                        {
                            if (!result.Contains(0)) result.Add(0);
                            MelonLogger.Msg($"🖱️ [Touch Down] 지상(GROUND) 단독 입력 (X={Input.mousePosition.x:F0}, Y={Input.mousePosition.y:F0})");
                        }
                        else if (buttonName == MDButtonType.BATTLE_AIR)
                        {
                            // 공중 동시 발동 차단!
                            result.Clear();
                        }
                    }
                }

                // 마우스 우클릭 (두 번째 손가락 / 연타)
                if (Input.GetMouseButtonDown(1))
                {
                    _btn1IsAir = CalculateIsAir(Input.mousePosition);

                    if (_btn1IsAir)
                    {
                        if (buttonName == MDButtonType.BATTLE_AIR)
                        {
                            if (!result.Contains(1)) result.Add(1);
                        }
                        else if (buttonName == MDButtonType.BATTLE_GROUND && !Input.GetMouseButton(0))
                        {
                            result.Clear();
                        }
                    }
                    else
                    {
                        if (buttonName == MDButtonType.BATTLE_GROUND)
                        {
                            if (!result.Contains(1)) result.Add(1);
                        }
                        else if (buttonName == MDButtonType.BATTLE_AIR && !Input.GetMouseButton(0))
                        {
                            result.Clear();
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

                // 마우스 좌클릭 홀드 (점프 체공 및 롱노트 유지)
                if (Input.GetMouseButton(0))
                {
                    if (_btn0IsAir)
                    {
                        if (buttonName == MDButtonType.BATTLE_AIR)
                        {
                            if (!result.Contains(0)) result.Add(0);
                        }
                        else if (buttonName == MDButtonType.BATTLE_GROUND)
                        {
                            // 체공 중 지상 홀드 신호 완전 차단 (지상으로 떨어지는 버그 방지)
                            result.Clear();
                        }
                    }
                    else
                    {
                        if (buttonName == MDButtonType.BATTLE_GROUND)
                        {
                            if (!result.Contains(0)) result.Add(0);
                        }
                        else if (buttonName == MDButtonType.BATTLE_AIR)
                        {
                            result.Clear();
                        }
                    }
                }

                // 마우스 우클릭 홀드
                if (Input.GetMouseButton(1))
                {
                    if (_btn1IsAir)
                    {
                        if (buttonName == MDButtonType.BATTLE_AIR)
                        {
                            if (!result.Contains(1)) result.Add(1);
                        }
                        else if (buttonName == MDButtonType.BATTLE_GROUND && !Input.GetMouseButton(0))
                        {
                            result.Clear();
                        }
                    }
                    else
                    {
                        if (buttonName == MDButtonType.BATTLE_GROUND)
                        {
                            if (!result.Contains(1)) result.Add(1);
                        }
                        else if (buttonName == MDButtonType.BATTLE_AIR && !Input.GetMouseButton(0))
                        {
                            result.Clear();
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

                // 마우스 좌클릭 릴리즈
                if (Input.GetMouseButtonUp(0))
                {
                    if (_btn0IsAir)
                    {
                        if (buttonName == MDButtonType.BATTLE_AIR)
                        {
                            if (!result.Contains(0)) result.Add(0);
                            MelonLogger.Msg($"🖱️ [Touch Up] 공중(AIR) 릴리즈");
                        }
                        else if (buttonName == MDButtonType.BATTLE_GROUND)
                        {
                            result.Clear();
                        }
                    }
                    else
                    {
                        if (buttonName == MDButtonType.BATTLE_GROUND)
                        {
                            if (!result.Contains(0)) result.Add(0);
                            MelonLogger.Msg($"🖱️ [Touch Up] 지상(GROUND) 릴리즈");
                        }
                        else if (buttonName == MDButtonType.BATTLE_AIR)
                        {
                            result.Clear();
                        }
                    }
                }

                // 마우스 우클릭 릴리즈
                if (Input.GetMouseButtonUp(1))
                {
                    if (_btn1IsAir)
                    {
                        if (buttonName == MDButtonType.BATTLE_AIR)
                        {
                            if (!result.Contains(1)) result.Add(1);
                        }
                        else if (buttonName == MDButtonType.BATTLE_GROUND && !Input.GetMouseButton(0))
                        {
                            result.Clear();
                        }
                    }
                    else
                    {
                        if (buttonName == MDButtonType.BATTLE_GROUND)
                        {
                            if (!result.Contains(1)) result.Add(1);
                        }
                        else if (buttonName == MDButtonType.BATTLE_AIR && !Input.GetMouseButton(0))
                        {
                            result.Clear();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[MouseTouchBridge] InjectButtonUp 에러: {ex}");
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

        #endregion
    }
}
