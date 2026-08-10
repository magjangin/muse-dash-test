using System;
using HarmonyLib;
using Il2CppAssets.Scripts.UI.Panels;
using MelonLoader;
using UnityEngine;

namespace muse_dash_test.Patches.UI.Welcome
{
    /// <summary>
    /// 타이틀 화면 및 UI 진입 시 표시되는 "업데이트 요청!" 배지 및 신규 버전 알림 UI를 자동으로 숨기는 패치입니다.
    /// 보존판(Archive) 환경에서 최신 서버 버전과 차이가 발생해도 타이틀 화면이 깔끔하게 유지되도록 합니다.
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
    }
}
