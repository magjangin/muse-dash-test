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
    // [회귀 조사 결과] 이 훅을 비활성화해도 결과창 전환 문제는 그대로였다 → 무죄. 재활성화함.
    [HarmonyLib.HarmonyPatch(typeof(RankCell), nameof(RankCell.SetValue), new Type[] { typeof(int), typeof(string), typeof(int), typeof(float) })]
    public class RankCell_SetValue_HookPatch
    {
        public static void Prefix(RankCell __instance, int number, string nickName, int score, float acc)
        {
            if (number > 10 || !ModLogger.IsLevelEnabled(ModLogLevel.Verbose)) return;
            try
            {
                string uid = CustomPlaySession.Current.LastKnownMusicUid;
                float displayAcc = acc * 100f;
                var inv = System.Globalization.CultureInfo.InvariantCulture;
                ModLogger.Verbose($"🎯 [RankCell.SetValue] Number={number.ToString("D2", inv)}, NickName='{nickName}', Score={score.ToString("N0", inv)}, Acc={displayAcc.ToString("F2", inv)}% (UID={uid ?? "unknown"})");
            }
            catch (Exception ex) { ModLogger.Verbose($"[RankCellHook] 무시된 예외: {ex.Message}"); }
        }

        public static void Postfix(RankCell __instance, int number)
        {
            try
            {
                if (__instance != null && __instance.gameObject != null)
                {
                    // 11위 이하(Top 10 초과) 셀은 세팅 후 즉시 비활성화 처리
                    if (number > 10 && __instance.gameObject.activeSelf)
                    {
                        __instance.gameObject.SetActive(false);
                    }
                }
            }
            catch (Exception ex) { ModLogger.Verbose($"[RankCellHook] 무시된 예외: {ex.Message}"); }
        }
    }
}
