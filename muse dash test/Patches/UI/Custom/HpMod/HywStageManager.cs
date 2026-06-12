using UnityEngine;
using MelonLoader;
using made_by_hyw.GameObjects;

namespace muse_dash_test
{
    public class HywStageManager
    {
        private bool isInStage = false;
        private UnityEngine.UI.Text targetTextComponent = null;
        private string lastText = "";

        public bool IsInStage => isInStage;

        public void CheckForStageAndModify()
        {
            try
            {
                var pnl = Il2CppAssets.Scripts.UI.Panels.PnlBattle.instance;
                bool isBattleActive = pnl != null && pnl.CurrentBattleUIComp != null;

                if (!isBattleActive)
                {
                    if (isInStage)
                    {
                        // 스테이지 종료
                        isInStage = false;
                        targetTextComponent = null;
                        lastText = "";
                        MelonLogger.Msg("[HywHpTextMod] 스테이지 종료 감지.");
                    }
                    return;
                }

                bool foundHealth = HealthBarFinder.FindHealthBar() != null;

                if (foundHealth)
                {
                    if (!isInStage)
                    {
                        // 최초 진입
                        isInStage = true;
                        targetTextComponent = null;
                        lastText = "";
                        
                        if (ExperimentPlayContext.ShouldApplyExperimentChart)
                        {
                            MelonLogger.Msg("[HywHpTextMod] 스테이지 진입 감지: UI 덮어쓰기를 실행합니다.");
                            ModifyHealthBar();
                        }
                    }
                    else if (ExperimentPlayContext.ShouldApplyExperimentChart && targetTextComponent == null)
                    {
                        // 재시작 감지 (이전 스테이지 컴포넌트가 파괴됨)
                        targetTextComponent = null;
                        lastText = "";
                        MelonLogger.Msg("[HywHpTextMod] 스테이지 재시작 감지: UI를 다시 덮어씁니다.");
                        ModifyHealthBar();
                    }
                }
                else
                {
                    if (isInStage)
                    {
                        // 스테이지 종료
                        isInStage = false;
                        targetTextComponent = null;
                        lastText = "";
                        MelonLogger.Msg("[HywHpTextMod] 스테이지 종료 감지.");
                    }
                }
            }
            catch (System.Exception ex)
            {
                MelonLogger.Msg($"[HywHpTextMod] 오류 발생: {ex.Message}");
            }
        }

        public void CheckForNoteEvents()
        {
            if (!ExperimentPlayContext.ShouldApplyExperimentChart) return;

            if (targetTextComponent != null)
            {
                string currentText = targetTextComponent.text;
                
                // 노트 관련 이벤트로 인한 텍스트 변경 감지
                if (currentText != lastText && currentText != "made in 화영왕")
                {
                    // 즉시 원하는 텍스트로 변경
                    targetTextComponent.text = "made in 화영왕";
                    lastText = "made in 화영왕";
                }
                else if (currentText == "made in 화영왕")
                {
                    lastText = currentText;
                }
            }
        }

        private void ModifyHealthBar()
        {
            if (!ExperimentPlayContext.ShouldApplyExperimentChart) return;

            try
            {
                var textComponent = HealthBarFinder.FindHealthText();
                if (textComponent == null)
                {
                    return;
                }

                // 텍스트 컴포넌트 저장
                targetTextComponent = textComponent;
                lastText = textComponent.text;

                // 텍스트 렌더링 상태 분석 디버그 로그 추가 (부모 캔버스 소팅 방식 점검)
                Canvas parentCanvas = textComponent.GetComponentInParent<Canvas>();
                if (parentCanvas != null)
                {
                    MelonLogger.Msg($"[HywHpTextMod.Debug] 체력바 텍스트 렌더링 상태 상세 분석:");
                    MelonLogger.Msg($"  - Parent Canvas: {parentCanvas.name}");
                    MelonLogger.Msg($"  - Render Mode: {parentCanvas.renderMode}");
                    MelonLogger.Msg($"  - Sorting Layer Name: {parentCanvas.sortingLayerName}");
                    MelonLogger.Msg($"  - Sorting Order: {parentCanvas.sortingOrder}");
                    MelonLogger.Msg($"  - Local Position: {textComponent.transform.localPosition}");
                    MelonLogger.Msg($"  - World Position: {textComponent.transform.position}");
                }
                else
                {
                    MelonLogger.Warning("[HywHpTextMod.Debug] 체력바 텍스트의 상위 Canvas를 찾을 수 없습니다.");
                }
                
                // 텍스트 스타일 적용
                HywTextStyler.ApplyMadeByHywStyle(textComponent);
                MelonLogger.Msg("체력바 텍스트가 성공적으로 변경되었습니다!");
            }
            catch (System.Exception ex)
            {
                MelonLogger.Msg($"체력바 수정 중 오류 발생: {ex.Message}");
            }
        }
    }
}
