using MelonLoader;
using HarmonyLib;
using Il2CppAssets.Scripts.UI.Panels;
using UnityEngine;
using UnityEngine.UI;

namespace muse_dash_test
{
    // === 배틀 스테이지 진행바(sldProgress) 상태 진단 로깅 패치 ===
    // 주의: 이 패치는 진행바를 숨기거나 바꾸는 등의 "제어"를 하지 않는다.
    // sldProgress 컴포넌트의 존재 여부만 관찰해 로그로 남기는 진단 전용이다.
    // (게임 기본 동작인 진행바 노출을 그대로 두므로 별도 조작이 없음)

    [HarmonyPatch(typeof(PnlBattle), GameBindings.PnlBattle.MusicProgressInit)]
    public class PnlBattle_MusicProgressInit_Patch
    {
        public static void Postfix(PnlBattle __instance)
        {
            try
            {
                MelonLogger.Msg("[ProgressBarPatch] PnlBattle.MusicProgressInit 호출됨 - 진행바 상태 점검(관찰 전용)");

                if (__instance.CurrentBattleUIComp != null)
                {
                    Slider sld = __instance.CurrentBattleUIComp.sldProgress;
                    if (sld != null)
                    {
                        MelonLogger.Msg("[ProgressBarPatch] sldProgress 슬라이더 감지됨 (관찰 전용, 별도 제어 없음)");
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
