using MelonLoader;
using System;
using UnityEngine;
using UnityEngine.Events;
using HarmonyLib;
using Il2Cpp;
using Il2CppAssets.Scripts.UI.PnlOptions;

namespace muse_dash_test.Patches.UI.Setting
{
    /// <summary>
    /// PC 환경에서 모바일 터치 설정창(PnlInputMobile)을 강제로 활성화하고 동작을 검증하는 테스트 패치 클래스입니다.
    /// </summary>
    [HarmonyPatch(typeof(PnlPlaySetting), nameof(PnlPlaySetting.OnAwake))]
    public static class PnlPlaySetting_MobileInputPatch
    {
        public static void Postfix(PnlPlaySetting __instance)
        {
            try
            {
                if (__instance == null) return;

                MelonLogger.Msg("📱 [MobileSetting] PnlPlaySetting.OnAwake 감지 - 모바일 입력 설정 패널 연동 시도");

                if (__instance.m_BtnInputSetting != null)
                {
                    // 기존 리스너 외에 모바일 패널 강제 활성화 리스너 추가
                    __instance.m_BtnInputSetting.onClick.AddListener((UnityAction)(() =>
                    {
                        try
                        {
                            if (!ModConfig.EnableMobileTouch || !InputOverlay.enableMobileTouch)
                            {
                                MelonLogger.Msg("📱 [MobileSetting] 모바일 터치 설정이 꺼져 있어 PC 기본 키설정 패널을 유지합니다.");
                                return;
                            }

                            MelonLogger.Msg("📱 [MobileSetting] 입력 설정 버튼 클릭됨! -> PnlInputMobile 강제 표시 시도");

                            if (__instance.m_PnlInputSettingStandlone != null)
                            {
                                __instance.m_PnlInputSettingStandlone.SetActive(false);
                            }

                            if (__instance.m_PnlInputSettingMobile != null)
                            {
                                __instance.m_PnlInputSettingMobile.SetActive(true);
                                MelonLogger.Msg("📱 [MobileSetting] m_PnlInputSettingMobile.SetActive(true) 성공!");
                            }
                            else
                            {
                                MelonLogger.Warning("⚠️ [MobileSetting] m_PnlInputSettingMobile 오브젝트가 null입니다.");
                            }
                        }
                        catch (Exception ex)
                        {
                            MelonLogger.Error($"[MobileSetting] onClick 핸들러 에러: {ex}");
                        }
                    }));
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[MobileSetting] PnlPlaySetting.OnAwake 패치 에러: {ex}");
            }
        }
    }

    /// <summary>
    /// PnlInputMobile의 라이프사이클 및 설정 변경 이벤트 로깅 및 동작 검증 패치
    /// </summary>
    [HarmonyPatch(typeof(PnlInputMobile))]
    public static class PnlInputMobile_LifecyclePatch
    {
        [HarmonyPatch("Awake")]
        [HarmonyPostfix]
        public static void Awake_Postfix(PnlInputMobile __instance)
        {
            MelonLogger.Msg($"📱 [PnlInputMobile] Awake 완료! (IsLeftRight: {__instance.m_IsLeftRight}, IsTouchReverse: {__instance.m_IsTouchReverse}, IsAutoFever: {__instance.m_IsAutoFever})");
        }

        [HarmonyPatch("SetAutoFever")]
        [HarmonyPostfix]
        public static void SetAutoFever_Postfix(bool autoFever)
        {
            MelonLogger.Msg($"📱 [PnlInputMobile] 오토 피버(AutoFever) 설정 변경됨: {autoFever}");
        }

        [HarmonyPatch("SetTouchReverse")]
        [HarmonyPostfix]
        public static void SetTouchReverse_Postfix(bool reverse)
        {
            MelonLogger.Msg($"📱 [PnlInputMobile] 터치 좌우 반전(TouchReverse) 설정 변경됨: {reverse}");
        }

        [HarmonyPatch("SetLeftRight")]
        [HarmonyPostfix]
        public static void SetLeftRight_Postfix(bool leftRight)
        {
            MelonLogger.Msg($"📱 [PnlInputMobile] 좌우 분할 모드(LeftRight) 설정 변경됨: {leftRight}");
        }
    }
}
