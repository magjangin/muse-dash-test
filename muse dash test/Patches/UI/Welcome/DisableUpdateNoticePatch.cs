using System;
using HarmonyLib;
using Il2CppAssets.Scripts.Helpers;
using Il2CppAssets.Scripts.UI;
using Il2CppAssets.Scripts.UI.Panels;
using MelonLoader;
using UnityEngine;

namespace muse_dash_test.Patches.UI.Welcome
{
    /// <summary>
    /// 타이틀 화면, 설정(OptionSelect) 및 전역 버전 점검 엔진(VersionHelper)을 후킹하여
    /// 로직상 게임이 최신 상태(업데이트 불필요)로 판정되도록 처리하고 알림 UI를 완전 차단하는 패치입니다.
    /// </summary>
    public static class DisableUpdateNoticePatch
    {
        // -------------------------------------------------------------
        // 1. 버전 관리 로직 엔진 (VersionHelper) 후킹
        // -------------------------------------------------------------

        /// <summary>
        /// 업데이트가 필요한지 판단하는 로직 메서드를 후킹하여 강제로 false(업데이트 불필요)를 반환합니다.
        /// </summary>
        [HarmonyPatch(typeof(VersionHelper), nameof(VersionHelper.CheckNeedUpdate))]
        public static class VersionHelper_CheckNeedUpdate_Patch
        {
            public static bool Prefix(ref bool __result)
            {
                __result = false;
                MelonLogger.Msg("[UI.UpdateNotice] 🛡️ VersionHelper.CheckNeedUpdate() 호출 감지 ➔ 강제로 false(업데이트 불필요) 반환!");
                return false; // 원본 실행 건너뛰기
            }
        }

        /// <summary>
        /// 서버 네트워크 버전 체크 요청을 인터셉트하여 s_NeedUpdateValue를 false로 갱신하고 콜백에 false를 전달합니다.
        /// </summary>
        [HarmonyPatch(typeof(VersionHelper), nameof(VersionHelper.CheckVersion))]
        public static class VersionHelper_CheckVersion_Patch
        {
            public static bool Prefix(Il2CppSystem.Action<bool> callback)
            {
                try
                {
                    VersionHelper.s_NeedUpdateValue = false;
                    MelonLogger.Msg("[UI.UpdateNotice] 🛡️ VersionHelper.CheckVersion() 호출 감지 ➔ 네트워크 서버 요청 차단 및 s_NeedUpdateValue=false 갱신!");
                    callback?.Invoke(false);
                }
                catch (Exception ex)
                {
                    MelonLogger.Warning($"[UI.UpdateNotice] VersionHelper.CheckVersion 콜백 처리 중 경고: {ex.Message}");
                }
                return false; // 원본 네트워크 요청 생략
            }
        }

        /// <summary>
        /// 업데이트 안내 팝업 및 팁 표시 로직을 차단합니다.
        /// </summary>
        [HarmonyPatch(typeof(VersionHelper), nameof(VersionHelper.CheckUpdateStateAndShowConfirm))]
        public static class VersionHelper_CheckUpdateStateAndShowConfirm_Patch
        {
            public static bool Prefix()
            {
                MelonLogger.Msg("[UI.UpdateNotice] 🛡️ VersionHelper.CheckUpdateStateAndShowConfirm() 호출 차단!");
                return false;
            }
        }

        [HarmonyPatch(typeof(VersionHelper), nameof(VersionHelper.ShowUpdateTip))]
        public static class VersionHelper_ShowUpdateTip_Patch
        {
            public static bool Prefix()
            {
                MelonLogger.Msg("[UI.UpdateNotice] 🛡️ VersionHelper.ShowUpdateTip() 호출 차단!");
                return false;
            }
        }

        [HarmonyPatch(typeof(VersionHelper), nameof(VersionHelper.ShowUpdateConfirmPopup))]
        public static class VersionHelper_ShowUpdateConfirmPopup_Patch
        {
            public static bool Prefix()
            {
                MelonLogger.Msg("[UI.UpdateNotice] 🛡️ VersionHelper.ShowUpdateConfirmPopup() 호출 차단!");
                return false;
            }
        }

        // -------------------------------------------------------------
        // 2. 패널별 버전 수신 핸들러 (WelcomeSelect, OptionSelect) 후킹
        // -------------------------------------------------------------

        [HarmonyPatch(typeof(WelcomeSelect), nameof(WelcomeSelect.OnGetRecommendVersion))]
        public static class WelcomeSelect_OnGetRecommendVersion_Patch
        {
            public static bool Prefix(ref bool isRecommendVersionReleased)
            {
                isRecommendVersionReleased = false;
                MelonLogger.Msg("[UI.UpdateNotice] 🛡️ WelcomeSelect.OnGetRecommendVersion() 호출 감지 ➔ isRecommendVersionReleased = false 전달!");
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

        [HarmonyPatch(typeof(Il2Cpp.OptionSelect), nameof(Il2Cpp.OptionSelect.OnGetRecommendVersion))]
        public static class OptionSelect_OnGetRecommendVersion_Patch
        {
            public static bool Prefix(ref bool showRecommendSign)
            {
                showRecommendSign = false;
                MelonLogger.Msg("[UI.UpdateNotice] 🛡️ OptionSelect.OnGetRecommendVersion() 호출 감지 ➔ showRecommendSign = false 전달!");
                return true;
            }
        }

        // -------------------------------------------------------------
        // 3. UI 컨트롤러 (RecommendVersionController) 비활성화
        // -------------------------------------------------------------

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
