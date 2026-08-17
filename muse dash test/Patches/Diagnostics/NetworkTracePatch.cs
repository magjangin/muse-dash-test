using System;
using System.Net.Http;
using System.Threading;
using HarmonyLib;
using MelonLoader;
using UnityEngine.Networking;

namespace muse_dash_test
{
    /// <summary>
    /// UnityWebRequest 및 HttpClient의 네트워크 요청 URL을 가로채어 로그에 출력하는 진단 패치입니다.
    /// </summary>
    [HarmonyPatch]
    public static class NetworkTracePatch
    {
        // -------------------------------------------------------------
        // 1. UnityEngine.Networking.UnityWebRequest Hook
        // -------------------------------------------------------------

        [HarmonyPatch(typeof(UnityWebRequest), nameof(UnityWebRequest.SendWebRequest))]
        [HarmonyPrefix]
        public static void UnityWebRequest_SendWebRequest_Prefix(UnityWebRequest __instance)
        {
            try
            {
                string url = __instance?.url;
                string method = __instance?.method;
                MelonLogger.Msg($"[NetworkTrace.UnityWebRequest] ({method ?? "GET"}) URL: {url ?? "(null)"}");
            }
            catch (Exception ex)
            {
                MelonLogger.Warning($"[NetworkTrace.UnityWebRequest] 로그 추출 실패: {ex.Message}");
            }
        }

        [HarmonyPatch(typeof(UnityWebRequest), MethodType.Constructor, new Type[] { typeof(string) })]
        [HarmonyPostfix]
        public static void UnityWebRequest_Ctor_Postfix(UnityWebRequest __instance, string url)
        {
            try
            {
                MelonLogger.Msg($"[NetworkTrace.UnityWebRequest.Ctor] 생성됨 URL: {url ?? "(null)"}");
            }
            catch (Exception ex)
            {
                MelonLogger.Warning($"[NetworkTrace.UnityWebRequest.Ctor] 로그 추출 실패: {ex.Message}");
            }
        }

        [HarmonyPatch(typeof(UnityWebRequest), MethodType.Constructor, new Type[] { typeof(string), typeof(string) })]
        [HarmonyPostfix]
        public static void UnityWebRequest_Ctor2_Postfix(UnityWebRequest __instance, string url, string method)
        {
            try
            {
                MelonLogger.Msg($"[NetworkTrace.UnityWebRequest.Ctor] 생성됨 ({method}) URL: {url ?? "(null)"}");
            }
            catch (Exception ex)
            {
                MelonLogger.Warning($"[NetworkTrace.UnityWebRequest.Ctor] 로그 추출 실패: {ex.Message}");
            }
        }

        // -------------------------------------------------------------
        // 2. System.Net.Http.HttpClient Hook (.NET)
        // -------------------------------------------------------------

        [HarmonyPatch(typeof(HttpClient), nameof(HttpClient.SendAsync), new Type[] { typeof(HttpRequestMessage), typeof(CancellationToken) })]
        [HarmonyPrefix]
        public static void HttpClient_SendAsync_Prefix(HttpClient __instance, HttpRequestMessage request)
        {
            try
            {
                string url = request?.RequestUri?.ToString();
                string method = request?.Method?.Method;
                string baseAddr = __instance?.BaseAddress?.ToString();
                MelonLogger.Msg($"[NetworkTrace.HttpClient] ({method ?? "GET"}) URL: {url ?? baseAddr ?? "(null)"}");
            }
            catch (Exception ex)
            {
                MelonLogger.Warning($"[NetworkTrace.HttpClient] 로그 추출 실패: {ex.Message}");
            }
        }
    }
}
