using MelonLoader;
using System;
using muse_dash_test;
using Il2CppAssets.Scripts.UI.Controls;

namespace muse_dash_test.Patches.UI.Stage
{
    /// <summary>
    /// RankCell.SetValue 호출을 감시하고, 11위 이하(number > 10) 셀이 세팅될 때 
    /// 즉각 해당 셀 오브젝트를 비활성화하여 랭킹창 표시 한도를 상위 10개(Top 10)로 완벽 제어하는 패치 클래스입니다.
    /// </summary>
    [HarmonyLib.HarmonyPatch(typeof(RankCell), nameof(RankCell.SetValue))]
    public class RankCell_SetValue_HookPatch
    {
        private static int hiddenCount = 0;

        public static void Prefix(RankCell __instance, int number, string nickName, int score, float acc)
        {
            try
            {
                string uid = CustomPlaySession.Current.SelectedMusicUid;
                if (string.IsNullOrEmpty(uid)) uid = PnlStagePatchHelper.GetCurrentSelectedMusicUid();

                float displayAcc = acc * 100f;
                if (number <= 10)
                {
                    MelonLogger.Msg($"🎯 [RankCell.SetValue] Number={number:D2}, NickName='{nickName}', Score={score:N0}, Acc={displayAcc:F2}% (UID={uid ?? "unknown"})");
                }
            }
            catch { }
        }

        public static void Postfix(RankCell __instance, int number)
        {
            try
            {
                if (__instance != null && __instance.gameObject != null)
                {
                    // 11위 이하(Top 10 초과) 셀은 세팅 후 즉시 비활성화 처리
                    if (number > 10)
                    {
                        if (__instance.gameObject.activeSelf)
                        {
                            __instance.gameObject.SetActive(false);
                            hiddenCount++;
                        }
                    }
                    else
                    {
                        if (number == 1) hiddenCount = 0; // 1위 세팅 시 카운터 리셋
                    }
                }
            }
            catch { }
        }
    }
}
