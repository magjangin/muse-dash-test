using MelonLoader;
using System;
using System.Reflection;
using UnityEngine;
using muse_dash_test;
using Il2CppAssets.Scripts.UI.Panels;

namespace muse_dash_test.Patches.UI.Stage
{
    /// <summary>
    /// PnlRank 메인 갱신 및 서버 응답 콜백 메서드(Refresh, RefreshGeneral, UIRefresh, NsRefreshFail)의 
    /// 실시간 호출 여부를 정확한 il2cpp 파라미터 시그니처로 추적하는 관찰 패치 클래스입니다.
    /// </summary>
    [HarmonyLib.HarmonyPatch(typeof(PnlRank), nameof(PnlRank.RefreshGeneral))]
    public class PnlRank_RefreshGeneral_LoggingPatch
    {
        public static void Prefix(PnlRank __instance, string uid)
        {
            try
            {
                if (__instance == null) return;
                int bufferChildCount = __instance.m_RankCellBuffer != null ? __instance.m_RankCellBuffer.childCount : 0;
                int ranksDictCount = PnlRankLoggingHelper.GetMRanksCountReflect(__instance);

                MelonLogger.Msg($"👑 [PnlRank.RefreshGeneral] Called! Target UID='{uid}', BufferChildCount={bufferChildCount}, RanksDictCount={ranksDictCount}");
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[PnlRank.RefreshGeneral Logging] 예외: {ex}");
            }
        }
    }

    [HarmonyLib.HarmonyPatch(typeof(PnlRank), nameof(PnlRank.UIRefresh))]
    public class PnlRank_UIRefresh_LoggingPatch
    {
        public static void Prefix(PnlRank __instance, string uid)
        {
            try
            {
                if (__instance == null) return;
                int bufferChildCount = __instance.m_RankCellBuffer != null ? __instance.m_RankCellBuffer.childCount : 0;
                int ranksDictCount = PnlRankLoggingHelper.GetMRanksCountReflect(__instance);

                MelonLogger.Msg($"👑 [PnlRank.UIRefresh] Called! Target UID='{uid}', BufferChildCount={bufferChildCount}, RanksDictCount={ranksDictCount}");
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[PnlRank.UIRefresh Logging] 예외: {ex}");
            }
        }
    }

    [HarmonyLib.HarmonyPatch(typeof(PnlRank), nameof(PnlRank.Refresh))]
    public class PnlRank_Refresh_LoggingPatch
    {
        public static void Prefix(PnlRank __instance, bool force)
        {
            try
            {
                string uid = CustomPlaySession.Current.SelectedMusicUid ?? PnlStagePatchHelper.GetCurrentSelectedMusicUid();
                MelonLogger.Msg($"👑 [PnlRank.Refresh] Called! force={force}, UID='{uid}'");
            }
            catch { }
        }
    }

    [HarmonyLib.HarmonyPatch(typeof(PnlRank), nameof(PnlRank.NsRefreshFail))]
    public class PnlRank_NsRefreshFail_LoggingPatch
    {
        public static void Prefix(PnlRank __instance, object errorType, string uid)
        {
            try
            {
                MelonLogger.Warning($"⚠️ [PnlRank.NsRefreshFail] 서버 갱신 실패 감지! ErrorType={errorType}, UID='{uid}'");
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[PnlRank.NsRefreshFail Logging] 예외: {ex}");
            }
        }
    }

    public static class PnlRankLoggingHelper
    {
        public static int GetMRanksCountReflect(PnlRank instance)
        {
            if (instance == null) return 0;
            try
            {
                PropertyInfo prop = typeof(PnlRank).GetProperty("m_Ranks", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if (prop != null)
                {
                    object dictObj = prop.GetValue(instance, null);
                    if (dictObj != null)
                    {
                        PropertyInfo countProp = dictObj.GetType().GetProperty("Count");
                        if (countProp != null)
                        {
                            object val = countProp.GetValue(dictObj, null);
                            if (val is int count) return count;
                        }
                    }
                }
            }
            catch { }
            return 0;
        }
    }
}
