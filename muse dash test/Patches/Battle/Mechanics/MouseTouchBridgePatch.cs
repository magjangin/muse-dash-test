using System;
using HarmonyLib;
using MelonLoader;
using UnityEngine;
using Il2CppAssets.Scripts.GameCore.Controller;
using Il2CppAssets.Scripts.PeroTools.Commons;
using Il2CppAssets.Scripts.PeroTools.Managers;
using Il2CppGameLogic;
using Il2CppPeroPeroGames.GlobalDefines;
using Il2CppSystem.Collections.Generic;

namespace muse_dash_test.Patches.Battle.Mechanics
{
    /// <summary>
    /// PC 환경에서 마우스 클릭 및 화면 터치 입력을 가로채어
    /// BATTLE_AIR / BATTLE_GROUND의 상호 배타적(동시 입력 충돌 방지) 판정을 보장하는 브릿지 패치 클래스입니다.
    /// 패드(게임패드/조이스틱) 연결 시에도 패드 입력을 차단하고 마우스 커서/터치스크린 우선으로 강제 전환합니다.
    /// </summary>
    [HarmonyPatch]
    public static class MouseTouchBridgePatch
    {
        private static bool _btn0IsAir = true;
        private static bool _btn1IsAir = false;

        #region Cursor & Controller Override Patches

        /// <summary>
        /// 모바일 터치 모드 활성화 시 마우스 커서가 숨겨지지 않도록 항상 표시 상태를 유지합니다.
        /// </summary>
        [HarmonyPatch(typeof(HideCursor), nameof(HideCursor.Update))]
        [HarmonyPrefix]
        public static bool HideCursor_Update_Prefix()
        {
            if (ModConfig.EnableMobileTouch && InputOverlay.enableMobileTouch)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                return false;
            }
            return true;
        }

        /// <summary>
        /// 모바일 터치 모드 활성화 시 패드가 장착되어 있어도 게임 시스템이 활성 컨트롤러를 Mouse로 인식하도록 오버라이드합니다.
        /// </summary>
        [HarmonyPatch(typeof(InputManager), nameof(InputManager.controllerType), MethodType.Getter)]
        [HarmonyPrefix]
        public static bool InputManager_ControllerType_Prefix(ref InputController __result)
        {
            if (ModConfig.EnableMobileTouch && InputOverlay.enableMobileTouch)
            {
                __result = InputController.Mouse;
                return false;
            }
            return true;
        }

        #endregion

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

        private static int _lastLoggedFrameDown = -1;
        private static int _lastLoggedFrameUp = -1;

        #region Core Injection Logic

        private static void InjectButtonDown(MDButtonType buttonName, ref List<int> result)
        {
            if (!ModConfig.EnableMobileTouch || !InputOverlay.enableMobileTouch) return;

            try
            {
                if (result == null) result = new List<int>();

                // 패드(조이스틱) 및 기본 키 매핑 간섭을 100% 차단하고 마우스/터치 전용으로 동작하도록 초기화
                result.Clear();

                bool shouldLog = (Time.frameCount != _lastLoggedFrameDown);

                // 1. 마우스 좌클릭 처리 (또는 터치로 인한 마우스 에뮬레이션)
                if (Input.GetMouseButtonDown(0))
                {
                    _btn0IsAir = CalculateIsAir(Input.mousePosition);

                    if (_btn0IsAir && buttonName == MDButtonType.BATTLE_AIR)
                    {
                        if (!result.Contains(0)) result.Add(0);
                        if (shouldLog)
                        {
                            MelonLogger.Msg($"🖱️ [Touch/Mouse Down] 공중(AIR) (X={Input.mousePosition.x:F0}, Y={Input.mousePosition.y:F0}, LeftRight={GameTouchPlay.isTouchLeftRight}, Reverse={GameTouchPlay.isTouchReverse || GameTouchPlay.isReverse})");
                            _lastLoggedFrameDown = Time.frameCount;
                        }
                    }
                    else if (!_btn0IsAir && buttonName == MDButtonType.BATTLE_GROUND)
                    {
                        if (!result.Contains(0)) result.Add(0);
                        if (shouldLog)
                        {
                            MelonLogger.Msg($"🖱️ [Touch/Mouse Down] 지상(GROUND) (X={Input.mousePosition.x:F0}, Y={Input.mousePosition.y:F0}, LeftRight={GameTouchPlay.isTouchLeftRight}, Reverse={GameTouchPlay.isTouchReverse || GameTouchPlay.isReverse})");
                            _lastLoggedFrameDown = Time.frameCount;
                        }
                    }
                }

                // 2. 마우스 우클릭 (두 번째 손가락 / 연타)
                if (Input.GetMouseButtonDown(1))
                {
                    _btn1IsAir = CalculateIsAir(Input.mousePosition);

                    if (_btn1IsAir && buttonName == MDButtonType.BATTLE_AIR)
                    {
                        if (!result.Contains(1)) result.Add(1);
                        if (shouldLog)
                        {
                            MelonLogger.Msg($"🖱️ [Touch Down (Right)] 공중(AIR) 우클릭 연타 (X={Input.mousePosition.x:F0}, Y={Input.mousePosition.y:F0})");
                            _lastLoggedFrameDown = Time.frameCount;
                        }
                    }
                    else if (!_btn1IsAir && buttonName == MDButtonType.BATTLE_GROUND)
                    {
                        if (!result.Contains(1)) result.Add(1);
                        if (shouldLog)
                        {
                            MelonLogger.Msg($"🖱️ [Touch Down (Right)] 지상(GROUND) 우클릭 연타 (X={Input.mousePosition.x:F0}, Y={Input.mousePosition.y:F0})");
                            _lastLoggedFrameDown = Time.frameCount;
                        }
                    }
                }

                // 3. 터치스크린 하드웨어 (스팀덱, 액정 태블릿, ROG Ally, 서피스 등) 직접 화면 터치 지원
                if (Input.touchCount > 0)
                {
                    for (int i = 0; i < Input.touchCount; i++)
                    {
                        Touch touch = Input.GetTouch(i);
                        if (touch.phase == TouchPhase.Began)
                        {
                            bool isAir = CalculateIsAir(touch.position);
                            if (isAir && buttonName == MDButtonType.BATTLE_AIR)
                            {
                                if (!result.Contains(touch.fingerId)) result.Add(touch.fingerId);
                                if (shouldLog)
                                {
                                    MelonLogger.Msg($"👆 [Screen Touch #{touch.fingerId} Down] 공중(AIR) 직접 화면 터치 (X={touch.position.x:F0}, Y={touch.position.y:F0}, Phase={touch.phase})");
                                    _lastLoggedFrameDown = Time.frameCount;
                                }
                            }
                            else if (!isAir && buttonName == MDButtonType.BATTLE_GROUND)
                            {
                                if (!result.Contains(touch.fingerId)) result.Add(touch.fingerId);
                                if (shouldLog)
                                {
                                    MelonLogger.Msg($"👆 [Screen Touch #{touch.fingerId} Down] 지상(GROUND) 직접 화면 터치 (X={touch.position.x:F0}, Y={touch.position.y:F0}, Phase={touch.phase})");
                                    _lastLoggedFrameDown = Time.frameCount;
                                }
                            }
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
            if (!ModConfig.EnableMobileTouch || !InputOverlay.enableMobileTouch) return;

            try
            {
                if (result == null) result = new List<int>();

                // 패드 및 기본 키 매핑 간섭 차단
                result.Clear();

                // 마우스 좌클릭 홀드 (점프 체공 및 롱노트 유지)
                if (Input.GetMouseButton(0))
                {
                    if (_btn0IsAir && buttonName == MDButtonType.BATTLE_AIR)
                    {
                        if (!result.Contains(0)) result.Add(0);
                    }
                    else if (!_btn0IsAir && buttonName == MDButtonType.BATTLE_GROUND)
                    {
                        if (!result.Contains(0)) result.Add(0);
                    }
                }

                // 마우스 우클릭 홀드
                if (Input.GetMouseButton(1))
                {
                    if (_btn1IsAir && buttonName == MDButtonType.BATTLE_AIR)
                    {
                        if (!result.Contains(1)) result.Add(1);
                    }
                    else if (!_btn1IsAir && buttonName == MDButtonType.BATTLE_GROUND)
                    {
                        if (!result.Contains(1)) result.Add(1);
                    }
                }

                // 터치스크린 하드웨어 홀드 지원
                if (Input.touchCount > 0)
                {
                    for (int i = 0; i < Input.touchCount; i++)
                    {
                        Touch touch = Input.GetTouch(i);
                        if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
                        {
                            bool isAir = CalculateIsAir(touch.position);
                            if (isAir && buttonName == MDButtonType.BATTLE_AIR)
                            {
                                if (!result.Contains(touch.fingerId)) result.Add(touch.fingerId);
                            }
                            else if (!isAir && buttonName == MDButtonType.BATTLE_GROUND)
                            {
                                if (!result.Contains(touch.fingerId)) result.Add(touch.fingerId);
                            }
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
            if (!ModConfig.EnableMobileTouch || !InputOverlay.enableMobileTouch) return;

            try
            {
                if (result == null) result = new List<int>();

                // 패드 및 기본 키 매핑 간섭 차단
                result.Clear();

                bool shouldLog = (Time.frameCount != _lastLoggedFrameUp);

                // 마우스 좌클릭 릴리즈
                if (Input.GetMouseButtonUp(0))
                {
                    if (_btn0IsAir && buttonName == MDButtonType.BATTLE_AIR)
                    {
                        if (!result.Contains(0)) result.Add(0);
                        if (shouldLog)
                        {
                            MelonLogger.Msg($"🖱️ [Touch Up] 공중(AIR) 릴리즈");
                            _lastLoggedFrameUp = Time.frameCount;
                        }
                    }
                    else if (!_btn0IsAir && buttonName == MDButtonType.BATTLE_GROUND)
                    {
                        if (!result.Contains(0)) result.Add(0);
                        if (shouldLog)
                        {
                            MelonLogger.Msg($"🖱️ [Touch Up] 지상(GROUND) 릴리즈");
                            _lastLoggedFrameUp = Time.frameCount;
                        }
                    }
                }

                // 마우스 우클릭 릴리즈
                if (Input.GetMouseButtonUp(1))
                {
                    if (_btn1IsAir && buttonName == MDButtonType.BATTLE_AIR)
                    {
                        if (!result.Contains(1)) result.Add(1);
                    }
                    else if (!_btn1IsAir && buttonName == MDButtonType.BATTLE_GROUND)
                    {
                        if (!result.Contains(1)) result.Add(1);
                    }
                }

                // 터치스크린 하드웨어 릴리즈 지원
                if (Input.touchCount > 0)
                {
                    for (int i = 0; i < Input.touchCount; i++)
                    {
                        Touch touch = Input.GetTouch(i);
                        if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                        {
                            bool isAir = CalculateIsAir(touch.position);
                            if (isAir && buttonName == MDButtonType.BATTLE_AIR)
                            {
                                if (!result.Contains(touch.fingerId)) result.Add(touch.fingerId);
                                if (shouldLog)
                                {
                                    MelonLogger.Msg($"👆 [Screen Touch #{touch.fingerId} Up] 공중(AIR) 릴리즈");
                                    _lastLoggedFrameUp = Time.frameCount;
                                }
                            }
                            else if (!isAir && buttonName == MDButtonType.BATTLE_GROUND)
                            {
                                if (!result.Contains(touch.fingerId)) result.Add(touch.fingerId);
                                if (shouldLog)
                                {
                                    MelonLogger.Msg($"👆 [Screen Touch #{touch.fingerId} Up] 지상(GROUND) 릴리즈");
                                    _lastLoggedFrameUp = Time.frameCount;
                                }
                            }
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
