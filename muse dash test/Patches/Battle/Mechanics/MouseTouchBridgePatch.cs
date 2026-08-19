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

        /// <summary>
        /// 터치 슬롯별로 "누르기 시작한 순간"의 레인(공중/지상)을 기억합니다.
        /// 누른 뒤 손가락을 반대 영역으로 끌어도 판정 레인이 바뀌지 않도록 하기 위함이며,
        /// 마우스의 _btn0IsAir / _btn1IsAir와 같은 역할입니다.
        /// </summary>
        private static readonly bool[] _touchSlotIsAir = new bool[16];

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

                // 화면에 손가락이 닿아 있는 동안 Windows가 승격시킨 마우스는 버립니다.
                // 그대로 두면 손가락 하나가 터치와 마우스 양쪽으로 세어져 이중 판정이 납니다.
                bool ignoreMouse = TouchInput.ShouldIgnoreMouse();

                // 1. 마우스 좌클릭 처리
                if (!ignoreMouse && Input.GetMouseButtonDown(0))
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

                // 2. 마우스 우클릭 (연타)
                if (!ignoreMouse && Input.GetMouseButtonDown(1))
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

                // 3. 터치스크린 하드웨어 (ROG Ally, 스팀덱, 액정 태블릿, 서피스 등) 직접 화면 터치
                //    레거시 Input.touches는 Windows 스탠드얼론에서 동작하지 않으므로
                //    새 Input System의 Touchscreen을 통해 읽습니다.
                foreach (var contact in TouchInput.GetContacts())
                {
                    if (!contact.Began) continue;

                    bool isAir = CalculateIsAir(contact.Position);
                    if (contact.Id >= 0 && contact.Id < _touchSlotIsAir.Length)
                        _touchSlotIsAir[contact.Id] = isAir;

                    if (isAir && buttonName == MDButtonType.BATTLE_AIR)
                    {
                        if (!result.Contains(contact.Id)) result.Add(contact.Id);
                        if (shouldLog)
                        {
                            MelonLogger.Msg($"👆 [Screen Touch #{contact.Id} Down] 공중(AIR) 직접 화면 터치 (X={contact.Position.x:F0}, Y={contact.Position.y:F0}, Backend={TouchInput.ActiveBackend})");
                            _lastLoggedFrameDown = Time.frameCount;
                        }
                    }
                    else if (!isAir && buttonName == MDButtonType.BATTLE_GROUND)
                    {
                        if (!result.Contains(contact.Id)) result.Add(contact.Id);
                        if (shouldLog)
                        {
                            MelonLogger.Msg($"👆 [Screen Touch #{contact.Id} Down] 지상(GROUND) 직접 화면 터치 (X={contact.Position.x:F0}, Y={contact.Position.y:F0}, Backend={TouchInput.ActiveBackend})");
                            _lastLoggedFrameDown = Time.frameCount;
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

                bool ignoreMouse = TouchInput.ShouldIgnoreMouse();

                // 마우스 좌클릭 홀드 (점프 체공 및 롱노트 유지)
                if (!ignoreMouse && Input.GetMouseButton(0))
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
                if (!ignoreMouse && Input.GetMouseButton(1))
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

                // 터치스크린 홀드 지원 (롱노트 유지)
                foreach (var contact in TouchInput.GetContacts())
                {
                    if (!contact.Held) continue;

                    // 누른 순간 확정된 레인을 유지합니다. 손가락을 반대편으로 끌어도
                    // 판정 레인이 도중에 바뀌면 롱노트가 끊기기 때문입니다.
                    bool isAir = LaneOf(contact);

                    if (isAir && buttonName == MDButtonType.BATTLE_AIR)
                    {
                        if (!result.Contains(contact.Id)) result.Add(contact.Id);
                    }
                    else if (!isAir && buttonName == MDButtonType.BATTLE_GROUND)
                    {
                        if (!result.Contains(contact.Id)) result.Add(contact.Id);
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
                bool ignoreMouse = TouchInput.ShouldIgnoreMouse();

                // 마우스 좌클릭 릴리즈
                if (!ignoreMouse && Input.GetMouseButtonUp(0))
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
                if (!ignoreMouse && Input.GetMouseButtonUp(1))
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

                // 터치스크린 릴리즈 지원
                foreach (var contact in TouchInput.GetContacts())
                {
                    if (!contact.Ended) continue;

                    bool isAir = LaneOf(contact);

                    if (isAir && buttonName == MDButtonType.BATTLE_AIR)
                    {
                        if (!result.Contains(contact.Id)) result.Add(contact.Id);
                        if (shouldLog)
                        {
                            MelonLogger.Msg($"👆 [Screen Touch #{contact.Id} Up] 공중(AIR) 릴리즈");
                            _lastLoggedFrameUp = Time.frameCount;
                        }
                    }
                    else if (!isAir && buttonName == MDButtonType.BATTLE_GROUND)
                    {
                        if (!result.Contains(contact.Id)) result.Add(contact.Id);
                        if (shouldLog)
                        {
                            MelonLogger.Msg($"👆 [Screen Touch #{contact.Id} Up] 지상(GROUND) 릴리즈");
                            _lastLoggedFrameUp = Time.frameCount;
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
        /// 접점이 속한 판정 레인을 돌려줍니다.
        /// 누르기 시작할 때 확정해 둔 값을 우선 쓰고, 슬롯 범위를 벗어난 예외적인 경우에만
        /// 현재 좌표로 계산합니다.
        /// </summary>
        private static bool LaneOf(TouchContact contact)
        {
            if (contact.Id >= 0 && contact.Id < _touchSlotIsAir.Length)
                return _touchSlotIsAir[contact.Id];

            return CalculateIsAir(contact.Position);
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
