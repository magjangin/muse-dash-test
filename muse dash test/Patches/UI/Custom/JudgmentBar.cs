using System;
using System.Collections.Generic;
using MelonLoader;
using UnityEngine;
using Il2CppGameLogic;
using Il2CppFormulaBase;

namespace muse_dash_test
{
    public struct HitTick
    {
        public float offsetMs;
        public float timeAdded;
        public Color color;
    }

    public static class JudgmentBar
    {
        private static readonly List<HitTick> hitHistory = new List<HitTick>();
        private static float lastHitOffsetMs = 0f;
        private static float lastHitTime = -999f;
        private static Color lastHitColor = Color.white;

        // 단일 화이트 텍스처를 캐싱하여 GUI.color와 조합해 모든 단색 도형을 렌더링 (GC 및 메모리 최적화)
        private static Texture2D whiteTex;
        private static GUIStyle labelStyle;

        public static void RegisterHit(float gapInSeconds, byte result)
        {
            try
            {
                // 초 단위를 밀리초 단위로 변환
                float offsetMs = gapInSeconds * 1000f;

                // 판정에 따른 마커 색상 지정
                Color tickColor = Color.white;

                // 오차 절대값에 따라 판정 색상 정의
                float absOffset = Math.Abs(offsetMs);
                if (absOffset <= 50f)
                {
                    // Perfect: 눈에 띄는 화사한 황금색
                    tickColor = new Color(1f, 0.82f, 0f); 
                }
                else if (absOffset <= 130f)
                {
                    // Great: 산뜻한 연두/초록색
                    tickColor = new Color(0.25f, 0.85f, 0.3f); 
                }
                else
                {
                    // 그 외 (Early/Late 및 Miss)
                    if (offsetMs < 0f)
                    {
                        // Early: 시원한 하늘색
                        tickColor = new Color(0.2f, 0.6f, 0.95f);
                    }
                    else
                    {
                        // Late: 강렬한 빨간색
                        tickColor = new Color(0.95f, 0.25f, 0.25f);
                    }
                }

                // 최근 기록 갱신
                lastHitOffsetMs = offsetMs;
                lastHitTime = Time.time;
                lastHitColor = tickColor;

                // 역사 기록 추가 (잔상 틱용)
                hitHistory.Add(new HitTick
                {
                    offsetMs = offsetMs,
                    timeAdded = Time.time,
                    color = tickColor
                });
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[JudgmentBar] 히트 데이터 등록 중 에러: {ex.Message}");
            }
        }

        public static void DrawJudgmentBar()
        {
            try
            {
                if (!InputOverlay.showBar) return;

                // 1. 유효 시간이 지난 틱 제거
                float duration = InputOverlay.tickDuration;
                hitHistory.RemoveAll(tick => Time.time - tick.timeAdded > duration);

                // 2. 화이트 텍스처 초기화
                if (whiteTex == null)
                {
                    whiteTex = new Texture2D(1, 1);
                    whiteTex.SetPixel(0, 0, Color.white);
                    whiteTex.Apply();
                }

                // 3. 레이아웃 크기 및 좌표 산정 (화면 하단 중앙)
                float screenWidth = Screen.width;
                float screenHeight = Screen.height;
                float barW = InputOverlay.barWidth;
                float barH = InputOverlay.barHeight;
                
                float centerX = screenWidth / 2f;
                float y = screenHeight - InputOverlay.barOffsetFromBottom - barH;

                // 키뷰어와 판정바가 동시에 활성화된 경우, 판정바를 키뷰어의 상단에 붙여서 연동 배치합니다.
                float overlayTopY = InputOverlay.GetOverlayTopY();
                if (overlayTopY > 0f)
                {
                    y = overlayTopY - barH - 10f; // 10px 마진 추가
                }

                // 4. 배율 설정 (가로 크기가 나타내는 최대 범위는 ±150ms)
                float maxMsRange = 150f;
                float scale = (barW / 2f) / maxMsRange;

                // 5. 판정바 트랙 배경 및 가이드라인 그리기
                // (1) 전체 배경 트랙 그리기 (±150ms 범위)
                DrawColorRect(new Rect(centerX - barW / 2f, y, barW, barH), new Color(0.08f, 0.08f, 0.08f, 0.6f));

                // (2) Great 범위 박스 시각화 (±130ms)
                float greatW = 130f * 2f * scale;
                DrawColorRect(new Rect(centerX - greatW / 2f, y + 1f, greatW, barH - 2f), new Color(0.6f, 0.55f, 0.15f, 0.15f));

                // (3) Perfect 범위 박스 시각화 (±50ms)
                float perfectW = 50f * 2f * scale;
                DrawColorRect(new Rect(centerX - perfectW / 2f, y + 1f, perfectW, barH - 2f), new Color(0.15f, 0.65f, 0.75f, 0.22f));

                // (4) 센터 Perfect 중심선 (0ms)
                DrawColorRect(new Rect(centerX - 1f, y - 3f, 2f, barH + 6f), Color.white);

                // 6. 히트 잔상 틱들 렌더링
                foreach (var tick in hitHistory)
                {
                    float ms = Mathf.Clamp(tick.offsetMs, -maxMsRange, maxMsRange);
                    float tickX = centerX + (ms * scale);

                    // 남은 시간에 비례하여 알파(투명도) 페이드아웃 적용
                    float elapsed = Time.time - tick.timeAdded;
                    float alpha = Mathf.Clamp01(1f - (elapsed / duration));

                    Color finalColor = new Color(tick.color.r, tick.color.g, tick.color.b, alpha * 0.9f);
                    
                    // 틱 라인 그리기 (약간 더 굵게 1.5px 정도로 정렬)
                    DrawColorRect(new Rect(tickX - 1f, y - 1f, 2f, barH + 2f), finalColor);
                }

                // 7. 실시간 ms 오차 텍스트 출력
                float textElapsed = Time.time - lastHitTime;
                if (textElapsed < duration)
                {
                    string msText = "";
                    if (InputOverlay.barResponsive)
                    {
                        float absOffset = Math.Abs(lastHitOffsetMs);
                        if (absOffset <= 5f) msText = "완벽해요! (0ms)";
                        else if (absOffset <= 25f) msText = "정말 최고에요!";
                        else if (absOffset <= 50f) msText = "퍼펙트!";
                        else if (absOffset <= 90f) msText = "잘했어요!";
                        else if (absOffset <= 130f) msText = "좋아요!";
                        else if (lastHitOffsetMs < 0f) msText = "조금 빨라요!";
                        else msText = "조금 느려요!";
                    }
                    else
                    {
                        string sign = lastHitOffsetMs >= 0f ? "+" : "";
                        msText = $"{sign}{lastHitOffsetMs:F0} ms";
                    }

                    // 페이드아웃 효과 적용
                    float alpha = Mathf.Clamp01(1f - (textElapsed / duration));
                    
                    if (labelStyle == null)
                    {
                        labelStyle = new GUIStyle(GUI.skin.label);
                        labelStyle.fontStyle = FontStyle.Bold;
                        labelStyle.alignment = TextAnchor.MiddleCenter;
                    }
                    labelStyle.fontSize = (int)InputOverlay.barFontSize;
                    
                    // 섀도우 텍스트 효과 (가독성을 위한 검은 외곽선 역할)
                    labelStyle.normal.textColor = new Color(0f, 0f, 0f, alpha * 0.8f);
                    float offset = 1.5f;
                    Rect labelRect = new Rect(centerX - 100f, y - 25f, 200f, 20f);
                    
                    GUI.Label(new Rect(labelRect.x - offset, labelRect.y - offset, labelRect.width, labelRect.height), msText, labelStyle);
                    GUI.Label(new Rect(labelRect.x + offset, labelRect.y - offset, labelRect.width, labelRect.height), msText, labelStyle);
                    GUI.Label(new Rect(labelRect.x - offset, labelRect.y + offset, labelRect.width, labelRect.height), msText, labelStyle);
                    GUI.Label(new Rect(labelRect.x + offset, labelRect.y + offset, labelRect.width, labelRect.height), msText, labelStyle);

                    // 전면 메인 컬러 텍스트
                    labelStyle.normal.textColor = new Color(lastHitColor.r, lastHitColor.g, lastHitColor.b, alpha);
                    GUI.Label(labelRect, msText, labelStyle);
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[JudgmentBar] OnGUI 드로우 에러: {ex.Message}");
            }
        }

        private static void DrawColorRect(Rect rect, Color color)
        {
            GUI.color = color;
            GUI.DrawTexture(rect, whiteTex);
            GUI.color = Color.white;
        }
    }

    // ==========================================
    // 플레이어의 히트 판정 발생 시 오차ms 연동 훅
    // ==========================================

    [HarmonyLib.HarmonyPatch(typeof(GameTouchPlay), "TouchResult")]
    public static class GameTouchPlay_TouchResult_Patch
    {
        public static void Postfix(GameTouchPlay __instance, int idx, byte resultCode, uint actionType, TimeNodeOrder tno, bool isSkill, bool isElfinSkill)
        {
            try
            {
                // Miss(0) 또는 타격/판정 정보가 유효하지 않은 경우 무시
                if (resultCode == 0 || tno == null || tno.md == null) return;

                // 롱노트 누르고 있는 도중(isLongPressing), 롱노트 뗄 때(isLongPressEnd), 연타 도중(isMuling) 틱은 제외 (버그 방지)
                if (tno.isLongPressing || tno.isLongPressEnd || tno.isMuling) return;

                if (GameGlobal.gGameMusic != null)
                {
                    var music = GameGlobal.gGameMusic;
                    var md = tno.md;

                    float lastMusicTick = music.m_LastMusicTick;
                    float tickVal = Convert.ToSingle(md.tick.ToString());

                    // 오차(초 단위) 계산: (lastMusicTick / 1000f) - tickVal
                    float gap = (lastMusicTick / 1000f) - tickVal;

                    // 판정바 모듈에 등록
                    JudgmentBar.RegisterHit(gap, resultCode);
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[JudgmentBar.Hook] TouchResult Postfix 예외 발생: {ex}");
            }
        }
    }
}
