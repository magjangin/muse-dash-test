using UnityEngine;
using MelonLoader;

namespace muse_dash_test
{
    public class HywStageManager
    {
        private bool isInStage = false;
        private UnityEngine.UI.Text targetTextComponent = null;
        private string lastText = "";

        // 워터마크 복구 검사 주기. 매 프레임 텍스트를 읽지 않기 위한 스로틀입니다.
        private float noteEventTimer = 0f;
        private const float NoteEventCheckInterval = 0.1f;

        public bool IsInStage => isInStage;
        public static bool IsInStageStatic { get; private set; } = false;

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
                        IsInStageStatic = false;
                        targetTextComponent = null;
                        lastText = "";
                        MelonLogger.Msg("[HywHpTextMod] 스테이지 종료 감지.");
                    }
                    return;
                }

                // 🚀 성능 최적화: 이미 체력바 텍스트 컴포넌트가 정상 확보되었으면
                // 매 주기 무거운 GameObject.Find 16개 씬 전체 순회 검색을 건너뜁니다.
                if (isInStage && targetTextComponent != null)
                {
                    IsInStageStatic = true;
                    return;
                }

                bool foundHealth = HealthBarFinder.FindHealthBar() != null;

                if (foundHealth)
                {
                    if (!isInStage)
                    {
                        // 최초 진입
                        isInStage = true;
                        IsInStageStatic = true;
                        targetTextComponent = null;
                        lastText = "";
                        
                        // 아래는 워터마크 적용만 EnableHpTextMod에 묶습니다.
                        // 위의 isInStage/IsInStageStatic 추적은 입력 오버레이·판정바·HitPoint 설치와
                        // OnGUI 게이트가 함께 쓰므로 이 설정과 무관하게 항상 돌아야 합니다.
                        if (HywHpText.ShouldApply)
                        {
                            MelonLogger.Msg("[HywHpTextMod] 스테이지 진입 감지: UI 덮어쓰기를 실행합니다.");
                            ModifyHealthBar();
                        }
                    }
                    else if (HywHpText.ShouldApply && targetTextComponent == null)
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
                        IsInStageStatic = false;
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
            if (!HywHpText.ShouldApply) return;

            // 이 메서드는 MainMod.OnUpdate에서 매 프레임 호출됩니다. targetTextComponent.text 읽기는
            // IL2CPP → 관리 문자열 마샬링이라 프레임마다 새 문자열이 할당됩니다. 워터마크 복구는
            // 즉각성이 필요한 작업이 아니므로 0.1초 주기로 낮춰 인게임 GC 압력을 줄입니다.
            noteEventTimer += Time.deltaTime;
            if (noteEventTimer < NoteEventCheckInterval) return;
            noteEventTimer = 0f;

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
            if (!HywHpText.ShouldApply) return;

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
