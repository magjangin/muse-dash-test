using MelonLoader;
using System;
using muse_dash_test;
using Il2CppAssets.Scripts.UI.Controls;

namespace muse_dash_test.Patches.UI.Stage
{
    /// <summary>
    /// Muse Dash 원본 RankCell.SetValue 메서드가 호출될 때 전달되는 랭킹 데이터
    /// (순위, 닉네임, 점수, 정확도)를 100% 순정 상태에서 실시간 관찰 로깅하는 전용 분석 패치 클래스입니다.
    /// </summary>
    [HarmonyLib.HarmonyPatch(typeof(RankCell), nameof(RankCell.SetValue))]
    public class RankCell_SetValue_HookPatch
    {
        public static void Prefix(RankCell __instance, int number, string nickName, int score, float acc)
        {
            try
            {
                string uid = CustomPlaySession.Current.SelectedMusicUid;
                if (string.IsNullOrEmpty(uid)) uid = PnlStagePatchHelper.GetCurrentSelectedMusicUid();

                // accuracy 비율 변환 (1.0f -> 100.0%)
                float displayAcc = acc * 100f;

                MelonLogger.Msg($"🎯 [RankCell.SetValue] Number={number:D2}, NickName='{nickName}', Score={score:N0}, Acc={displayAcc:F2}% (UID={uid ?? "unknown"})");
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[RankCell.SetValue] 로깅 중 예외: {ex}");
            }
        }
    }
}
