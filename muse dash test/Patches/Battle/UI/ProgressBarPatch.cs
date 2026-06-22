using MelonLoader;
using HarmonyLib;
using Il2CppAssets.Scripts.UI.Panels;
using UnityEngine;
using UnityEngine.UI;

namespace muse_dash_test
{
    // === 배틀 스테이지 진행바(sldProgress) 제어 패치 ===

    [HarmonyPatch(typeof(PnlBattle), GameBindings.PnlBattle.MusicProgressInit)]
    public class PnlBattle_MusicProgressInit_Patch
    {
        public static void Postfix(PnlBattle __instance)
        {
            try
            {
                MelonLogger.Msg("[ProgressBarPatch] PnlBattle.MusicProgressInit 호출됨 - 진행바 제어 시작");

                if (__instance.CurrentBattleUIComp != null)
                {
                    Slider sld = __instance.CurrentBattleUIComp.sldProgress;
                    if (sld != null)
                    {
                        MelonLogger.Msg("[ProgressBarPatch] sldProgress 슬라이더를 성공적으로 감지했습니다. (진행바 노출 유지)");
                    }
                    else
                    {
                        MelonLogger.Warning("[ProgressBarPatch] sldProgress 슬라이더 컴포넌트가 null입니다.");
                    }
                }
                else
                {
                    MelonLogger.Warning("[ProgressBarPatch] CurrentBattleUIComp가 null입니다.");
                }
            }
            catch (System.Exception ex)
            {
                MelonLogger.Error($"[ProgressBarPatch.MusicProgressInit.Postfix] 예외 발생: {ex}");
            }
        }
    }
}
