using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using HarmonyLib;
using Il2CppAssets.Scripts.Structs.Network;
using MelonLoader;
using UnityEngine.Networking;

namespace muse_dash_test
{
    /// <summary>
    /// UnityWebRequest, StandardNetworkRequest 및 HttpClient의 네트워크 요청 URL과 Payload(Body)를 가로채어 로그에 출력하는 진단 패치입니다.
    /// </summary>
    [HarmonyPatch]
    public static class NetworkTracePatch
    {
        // -------------------------------------------------------------
        // 1. MuseDash 전용 NetworkRequest / StandardNetworkRequest Hook
        // -------------------------------------------------------------

        [HarmonyPatch(typeof(StandardNetworkRequest), MethodType.Constructor, new Type[] {
            typeof(string),
            typeof(ulong),
            typeof(uint),
            typeof(uint),
            typeof(string),
            typeof(Il2CppSystem.Collections.Generic.Dictionary<string, Il2CppSystem.Object>),
            typeof(Il2CppSystem.Collections.Generic.Dictionary<string, string>),
            typeof(Il2CppSystem.Action<Il2CppNewtonsoft.Json.Linq.JObject>),
            typeof(Il2CppSystem.Action<NetworkRequest>)
        })]
        [HarmonyPostfix]
        public static void StandardNetworkRequest_Ctor_Postfix(
            string url,
            ulong id,
            uint retryCount,
            uint interval,
            string method,
            Il2CppSystem.Collections.Generic.Dictionary<string, Il2CppSystem.Object> datas,
            Il2CppSystem.Collections.Generic.Dictionary<string, string> headers)
        {
            try
            {
                var sb = new StringBuilder();
                sb.AppendLine($"[NetworkTrace.StandardNetworkRequest] ({method ?? "GET"}) URL: {url}");

                if (datas != null && datas.Count > 0)
                {
                    sb.AppendLine("  [Payload Data Key-Values]:");
                    foreach (var pair in datas)
                    {
                        string key = pair.Key;
                        string val = pair.Value != null ? pair.Value.ToString() : "(null)";
                        sb.AppendLine($"    - {key}: {val}");
                    }
                }
                else
                {
                    sb.AppendLine("  [Payload Data]: (empty/null)");
                }

                MelonLogger.Msg(sb.ToString().TrimEnd());
            }
            catch (Exception ex)
            {
                MelonLogger.Warning($"[NetworkTrace.StandardNetworkRequest] 페이로드 덤프 에러: {ex.Message}");
            }
        }

        // -------------------------------------------------------------
        // 2. UnityEngine.Networking.UnityWebRequest Hook
        // -------------------------------------------------------------

        [HarmonyPatch(typeof(UnityWebRequest), nameof(UnityWebRequest.SendWebRequest))]
        [HarmonyPrefix]
        public static void UnityWebRequest_SendWebRequest_Prefix(UnityWebRequest __instance)
        {
            try
            {
                string url = __instance?.url;
                string method = __instance?.method;
                string body = null;

                if (__instance?.uploadHandler != null)
                {
                    try
                    {
                        var data = __instance.uploadHandler.data;
                        if (data != null && data.Length > 0)
                        {
                            body = Encoding.UTF8.GetString(data);
                        }
                    }
                    catch { }
                }

                if (!string.IsNullOrEmpty(body))
                {
                    MelonLogger.Msg($"[NetworkTrace.UnityWebRequest] ({method ?? "GET"}) URL: {url ?? "(null)"}\n  [Body Payload]: {body}");
                }
                else
                {
                    MelonLogger.Msg($"[NetworkTrace.UnityWebRequest] ({method ?? "GET"}) URL: {url ?? "(null)"}");
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Warning($"[NetworkTrace.UnityWebRequest] 로그 추출 실패: {ex.Message}");
            }
        }

        // -------------------------------------------------------------
        // 3. System.Net.Http.HttpClient Hook (.NET)
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
