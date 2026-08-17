using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using HarmonyLib;
using Il2CppAssets.Scripts.Structs.Network;
using Il2CppPeroPeroGames.DataStatistics;
using MelonLoader;
using UnityEngine.Networking;

namespace muse_dash_test
{
    /// <summary>
    /// 네트워크 요청 및 배틀 결과 데이터(판정/점수/통계)의 페이로드를 가로채어 완벽하게 덤프하는 진단 패치입니다.
    /// </summary>
    [HarmonyPatch]
    public static class NetworkTracePatch
    {
        // -------------------------------------------------------------
        // 1. UploadHandlerRaw Hook (UnityWebRequest POST 바디 원천 포획)
        // -------------------------------------------------------------

        [HarmonyPatch(typeof(UploadHandlerRaw), MethodType.Constructor, new Type[] { typeof(byte[]) })]
        [HarmonyPostfix]
        public static void UploadHandlerRaw_Ctor_Postfix(byte[] data)
        {
            try
            {
                if (data != null && data.Length > 0)
                {
                    string body = Encoding.UTF8.GetString(data);
                    MelonLogger.Msg($"[NetworkTrace.UploadHandlerRaw] 생성된 POST Payload ({data.Length} bytes):\n{body}");
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Warning($"[NetworkTrace.UploadHandlerRaw] 덤프 에러: {ex.Message}");
            }
        }

        // -------------------------------------------------------------
        // 2. ThinkingDataBattleHelper & Analytics Hook (게임 내 배틀 통계 및 전송 데이터 덤프)
        // -------------------------------------------------------------

        [HarmonyPatch(typeof(ThinkingDataBattleHelper), nameof(ThinkingDataBattleHelper.PushDataByTrack), new Type[] { typeof(string), typeof(Il2CppSystem.Collections.Generic.Dictionary<string, Il2CppSystem.Object>) })]
        [HarmonyPrefix]
        public static void ThinkingData_PushDataByTrack_Prefix(string eventName, Il2CppSystem.Collections.Generic.Dictionary<string, Il2CppSystem.Object> data)
        {
            try
            {
                var sb = new StringBuilder();
                sb.AppendLine($"[NetworkTrace.BattleStatistics] 이벤트 전송: '{eventName}'");

                if (data != null && data.Count > 0)
                {
                    sb.AppendLine("  [전송 필드 목록]:");
                    foreach (var pair in data)
                    {
                        string key = pair.Key;
                        string val = pair.Value != null ? pair.Value.ToString() : "(null)";
                        sb.AppendLine($"    - {key}: {val}");
                    }
                }
                MelonLogger.Msg(sb.ToString().TrimEnd());
            }
            catch (Exception ex)
            {
                MelonLogger.Warning($"[NetworkTrace.BattleStatistics] 덤프 에러: {ex.Message}");
            }
        }

        [HarmonyPatch(typeof(ThinkingDataBattleHelper), nameof(ThinkingDataBattleHelper.SendMDPlayEvent))]
        [HarmonyPrefix]
        public static void ThinkingData_SendMDPlayEvent_Prefix(ThinkingDataBattleHelper __instance, string eventName)
        {
            try
            {
                var sb = new StringBuilder();
                sb.AppendLine($"[NetworkTrace.SendMDPlayEvent] 배틀 결과 이벤트 호출: '{eventName}'");

                if (__instance?.m_PlayerData != null)
                {
                    sb.AppendLine("  [m_PlayerData 필드 전체 목록]:");
                    foreach (var pair in __instance.m_PlayerData)
                    {
                        string key = pair.Key;
                        string val = pair.Value != null ? pair.Value.ToString() : "(null)";
                        sb.AppendLine($"    - {key}: {val}");
                    }
                }

                if (__instance?.m_PlayerNoteData != null)
                {
                    sb.AppendLine($"  [m_PlayerNoteData 노트 타격 배열]: 총 {__instance.m_PlayerNoteData.Count}개 노트 기록");
                }

                MelonLogger.Msg(sb.ToString().TrimEnd());
            }
            catch (Exception ex)
            {
                MelonLogger.Warning($"[NetworkTrace.SendMDPlayEvent] 덤프 에러: {ex.Message}");
            }
        }

        // -------------------------------------------------------------
        // 3. MuseDash StandardNetworkRequest Hook
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
            Il2CppSystem.Collections.Generic.Dictionary<string, Il2CppSystem.Object> datas)
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

                MelonLogger.Msg(sb.ToString().TrimEnd());
            }
            catch (Exception ex)
            {
                MelonLogger.Warning($"[NetworkTrace.StandardNetworkRequest] 페이로드 덤프 에러: {ex.Message}");
            }
        }

        // -------------------------------------------------------------
        // 4. UnityEngine.Networking.UnityWebRequest Hook
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
    }
}
