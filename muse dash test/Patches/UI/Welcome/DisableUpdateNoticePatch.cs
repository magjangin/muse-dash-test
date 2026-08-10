using System;
using HarmonyLib;
using Il2CppAssets.Scripts.UI;
using Il2CppAssets.Scripts.UI.Panels;
using MelonLoader;
using UnityEngine;

namespace muse_dash_test.Patches.UI.Welcome
{
    /// <summary>
    /// 타이틀 화면, 설정(PnlOptions) 등 전역 UI에 표시되는 "업데이트 요청!" 배지 및 신규 버전 알림 UI를 자동으로 숨기는 패치입니다.
    /// 보존판(Archive) 환경에서 최신 서버 버전과 차이가 발생해도 타이틀 및 설정 화면이 깔끔하게 유지되도록 합니다.
    /// </summary>
    [HarmonyPatch(typeof(WelcomeSelect), nameof(WelcomeSelect.OnGetRecommendVersion))]
    public static class WelcomeSelect_OnGetRecommendVersion_Patch
    {
        public static bool Prefix(ref bool isRecommendVersionReleased)
        {
            // 신규 권장 버전 출시 여부를 강제로 false로 설정하여 업데이트 알림 생성을 차단합니다.
            isRecommendVersionReleased = false;
            return true;
        }
    }

    [HarmonyPatch(typeof(WelcomeSelect), nameof(WelcomeSelect.OnEnable))]
    public static class WelcomeSelect_OnEnable_Patch
    {
        public static void Postfix(WelcomeSelect __instance)
        {
            DisableUpdateNoticeHelper.DisableUpdateTip(__instance);
        }
    }

    [HarmonyPatch(typeof(WelcomeSelect), nameof(WelcomeSelect.Start))]
    public static class WelcomeSelect_Start_Patch
    {
        public static void Postfix(WelcomeSelect __instance)
        {
            DisableUpdateNoticeHelper.DisableUpdateTip(__instance);
        }
    }

    [HarmonyPatch(typeof(WelcomeSelect), nameof(WelcomeSelect.AdjustWelcomeUI))]
    public static class WelcomeSelect_AdjustWelcomeUI_Patch
    {
        public static void Postfix(WelcomeSelect __instance)
        {
            DisableUpdateNoticeHelper.DisableUpdateTip(__instance);
        }
    }

    /// <summary>
    /// 설정(PnlOptions) 및 기타 UI에 부착되는 전역 추천 버전 컨트롤러(RecommendVersionController) 비활성화 패치
    /// </summary>
    [HarmonyPatch(typeof(RecommendVersionController), nameof(RecommendVersionController.OnEnable))]
    public static class RecommendVersionController_OnEnable_Patch
    {
        public static void Postfix(RecommendVersionController __instance)
        {
            DisableUpdateNoticeHelper.DisableRecommendVersionController(__instance);
        }
    }

    [HarmonyPatch(typeof(RecommendVersionController), nameof(RecommendVersionController.Awake))]
    public static class RecommendVersionController_Awake_Patch
    {
        public static void Postfix(RecommendVersionController __instance)
        {
            DisableUpdateNoticeHelper.DisableRecommendVersionController(__instance);
        }
    }

    [HarmonyPatch(typeof(RecommendVersionController), nameof(RecommendVersionController.Init))]
    public static class RecommendVersionController_Init_Patch
    {
        public static void Postfix(RecommendVersionController __instance)
        {
            DisableUpdateNoticeHelper.DisableRecommendVersionController(__instance);
        }
    }

    public static class DisableUpdateNoticeHelper
    {
        public static void DisableUpdateTip(WelcomeSelect instance)
        {
            try
            {
                if (instance == null) return;

                if (instance.m_NewVersionTip != null && instance.m_NewVersionTip.activeSelf)
                {
                    instance.m_NewVersionTip.SetActive(false);
                    MelonLogger.Msg("[UI.UpdateNotice] 타이틀 화면의 '업데이트 요청!' 배지를 자동으로 비활성화했습니다.");
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Warning($"[UI.UpdateNotice] 업데이트 배지 비활성화 중 경고: {ex.Message}");
            }
        }

        public static void DisableRecommendVersionController(RecommendVersionController controller)
        {
            try
            {
                if (controller == null || controller.gameObject == null) return;

                if (controller.gameObject.activeSelf)
                {
                    controller.gameObject.SetActive(false);
                    MelonLogger.Msg("[UI.UpdateNotice] 설정 화면 등의 전역 '업데이트 요청!' 컨트롤러(RecommendVersionController)를 비활성화했습니다.");
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Warning($"[UI.UpdateNotice] RecommendVersionController 비활성화 중 경고: {ex.Message}");
            }
        }
    }
}
