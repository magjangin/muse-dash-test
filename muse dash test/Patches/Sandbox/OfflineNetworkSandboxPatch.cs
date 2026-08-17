using HarmonyLib;
using Il2CppAssets.Scripts.Structs.Network;
using Il2CppAssets.Scripts.UI.Panels;
using MelonLoader;
using System;

namespace muse_dash_test
{
    /// <summary>
    /// 오프라인 샌드박스 활성화 시, 외부 공식 서버(peropero.net, thinkingdata 등)로의 
    /// 백그라운드 웹 요청을 원천 차단하고 랭킹을 100% 로컬 독립형으로 격리하는 패치입니다.
    /// </summary>
    [HarmonyPatch]
    public static class OfflineNetworkSandboxPatch
    {
        // ----------------------------------------------------------------------
        // 1. StandardNetworkRequest 원천 차단 (버전 체크, 통계 피드백, IAP 검증 등)
        // ----------------------------------------------------------------------
        [HarmonyPatch(typeof(StandardNetworkRequest), nameof(StandardNetworkRequest.SendRequest))]
        [HarmonyPrefix]
        public static bool StandardNetworkRequest_SendRequest_Prefix(StandardNetworkRequest __instance)
        {
            if (!OfflineCustomSandbox.IsEnabled) return true;

            try
            {
                string url = __instance?.url;
                if (!string.IsNullOrEmpty(url) && (url.Contains("peropero.net") || url.Contains("thinkingdata")))
                {
                    MelonLogger.Msg($"[OfflineSandbox.Network] 🛡️ 외부 서버 요청 원천 차단 (오프라인 격리): {url}");
                    try
                    {
                        __instance.FailCallback(-1, "Offline Sandbox Blocked");
                    }
                    catch { }
                    return false; // 원본 네트워크 전송 스킵
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Warning($"[OfflineSandbox.Network] SendRequest Prefix 예외: {ex.Message}");
            }

            return true;
        }

        // ----------------------------------------------------------------------
        // 2. PnlRank 랭킹 조회 차단 및 로컬 독립 UI 전환
        // ----------------------------------------------------------------------
        [HarmonyPatch(typeof(PnlRank), nameof(PnlRank.RefreshGeneral))]
        [HarmonyPrefix]
        public static bool PnlRank_RefreshGeneral_Prefix(PnlRank __instance, string uid)
        {
            if (!OfflineCustomSandbox.IsEnabled) return true;

            try
            {
                MelonLogger.Msg($"[OfflineSandbox.PnlRank] 🛡️ 오프라인 샌드박스: 원격 랭킹 조회 스킵 (Target UID='{uid}')");

                // 로딩 인디케이터 즉시 끄고 로컬 UI 상태로 전환
                if (__instance != null)
                {
                    if (__instance.loading != null) __instance.loading.SetActive(false);
                    if (__instance.loadingTipSwitch != null) __instance.loadingTipSwitch.SetActive(false);
                    if (__instance.noNet != null) __instance.noNet.SetActive(false);
                    if (__instance.scrollView != null) __instance.scrollView.SetActive(false);
                    if (__instance.noRank != null) __instance.noRank.SetActive(true);
                    if (__instance.server != null) __instance.server.SetActive(true);
                }

                return false; // 원격 서버 랭킹 HTTP GET 요청 스킵
            }
            catch (Exception ex)
            {
                MelonLogger.Warning($"[OfflineSandbox.PnlRank] RefreshGeneral Prefix 예외: {ex.Message}");
                return true;
            }
        }
    }
}
