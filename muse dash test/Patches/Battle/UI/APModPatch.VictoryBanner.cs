using MelonLoader;
using System;
using HarmonyLib;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using UnityEngine;
using UnityEngine.UI;

namespace muse_dash_test.Patches
{
    public static class VictoryFlowGuard
    {
        private static bool isEndReached = false;

        public static void StartGuard()
        {
            isEndReached = false;
            MelonCoroutines.Start(CheckTimeout());
        }

        public static void MarkCompleted()
        {
            isEndReached = true;
        }

        private static System.Collections.IEnumerator CheckTimeout()
        {
            yield return new UnityEngine.WaitForSeconds(6.0f);
            if (!isEndReached)
            {
                MelonLogger.Msg("ℹ️ [APMod.Guard] 승리 연출 후 결과 화면 진입이 스킵되었거나 이탈(재시작/퇴장)되었습니다.");
            }
        }
    }

    // Intercept the transient Full Combo banner display and change it to ALL PERFECT if appropriate
    [HarmonyPatch(typeof(Il2CppAssets.Scripts.UI.GameMain.PnlVictory2dManager), GameBindings.PnlVictory2dManager.OnShowVictory, new Type[] { typeof(Il2CppSystem.Object), typeof(Il2CppSystem.Object), typeof(Il2CppReferenceArray<Il2CppSystem.Object>) })]
    public class PnlVictory2dManager_OnShowVictory_Patch
    {
        public static void Postfix(Il2CppAssets.Scripts.UI.GameMain.PnlVictory2dManager __instance, Il2CppSystem.Object sender, Il2CppSystem.Object rev, Il2CppReferenceArray<Il2CppSystem.Object> pars)
        {
            if (!ModConfig.EnableAPMod) return;
            try
            {
                MelonLogger.Msg("[APMod] PnlVictory2dManager.OnShowVictory Postfix 감지!");
                VictoryFlowGuard.StartGuard();

                // 결과 화면 진입 시 커스텀 BGM/BGA 미디어를 강제로 정지시킵니다.
                HwaBattleMediaController.StopMedia();

                // [기록 1단계] 커스텀 곡이면 별도 record/ 폴더에 플레이 결과를 저장합니다.
                // 배너 UI(__instance) 유무와 무관하게 기록은 남도록 여기서 먼저 처리합니다.
                TrySaveCustomRecord();

                if (__instance == null)
                {
                    MelonLogger.Msg("[APMod] __instance가 null입니다!");
                    return;
                }

                var comp = __instance.m_CurVictoryComp;
                if (comp == null)
                {
                    MelonLogger.Msg("[APMod] m_CurVictoryComp가 null입니다!");
                    return;
                }

                var fcGo = comp.fullCombo; // PnlFullComboText GameObject
                if (fcGo == null)
                {
                    MelonLogger.Msg("[APMod] comp.fullCombo가 null입니다.");
                    return;
                }

                if (IsAllPerfect())
                {
                    MelonLogger.Msg("[APMod] ★ALL PERFECT 달성!★ 승리 배너 수정 프로세스 개시.");
                    ShowAllPerfectBanner(fcGo.transform);
                }
                else
                {
                    MelonLogger.Msg("[APMod] 일반 풀콤보 또는 퍼펙트 미달성. 기존 FULL COMBO 문자 복원 활성화.");
                    RestoreFullComboBanner(fcGo.transform);
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[APMod] OnShowVictory Postfix 예외 발생: {ex}");
            }
        }

        // [기록 1단계] 커스텀 곡 플레이 결과를 record/ 폴더에 저장합니다.
        private static void TrySaveCustomRecord()
        {
            try
            {
                string uid = CustomPlaySession.Current.LastKnownMusicUid;
                if (!CustomContentIds.IsVirtualSong(uid))
                {
                    // 순정 곡은 게임 세이브가 처리하므로 우리 기록 대상이 아닙니다.
                    return;
                }

                var target = VictoryDataCache.ActiveTarget;
                if (target == null)
                {
                    MelonLogger.Warning("[CustomRecordStore] ActiveTarget이 null이라 기록을 저장할 수 없습니다.");
                    return;
                }

                int perfect = ModReflection.GetInt(target, "PerfectResult");
                int great = target.m_GreatResult;
                int miss = target.m_MissResult;
                float accuracy = target.GetAccuracy();
                bool isFullCombo = target.IsFullCombo();
                bool isAllPerfect = isFullCombo && great == 0 && miss == 0;

                // 실제 점수/최대콤보를 게임 필드(m_Score, m_MaxCombo)에서 직접 읽습니다. (추정 공식 대신)
                int score = ModReflection.GetInt(target, "Score");
                int maxCombo = ModReflection.GetInt(target, "MaxCombo");

                int difficulty = CustomRecordStore.ResolveCurrentDifficulty();

                var session = CustomPlaySession.Current;
                CustomRecordStore.SaveResult(
                    uid, difficulty,
                    session.TotalStandard, session.TotalGears, session.TotalHearts, session.TotalBlueNotes,
                    perfect, great, miss,
                    score, maxCombo,
                    accuracy,
                    isFullCombo, isAllPerfect);
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[CustomRecordStore] 기록 저장 시도 중 예외: {ex}");
            }
        }

        // 풀콤보이면서 Great 0, Miss 0 이면 ALL PERFECT로 판정합니다.
        private static bool IsAllPerfect()
        {
            if (VictoryDataCache.ActiveTarget == null)
            {
                MelonLogger.Msg("[APMod.Debug.Victory] [주의] VictoryDataCache.ActiveTarget가 null입니다! 콤보 판정을 가져올 수 없습니다.");
                return false;
            }

            bool isFullCombo = VictoryDataCache.ActiveTarget.IsFullCombo();
            int greatCount = VictoryDataCache.ActiveTarget.m_GreatResult;
            int missCount = VictoryDataCache.ActiveTarget.m_MissResult;
            float accuracy = VictoryDataCache.ActiveTarget.GetAccuracy();

            MelonLogger.Msg($"[APMod.Debug.Victory] 판정 결과 확인 - FC={isFullCombo}, Great={greatCount}, Miss={missCount}, Accuracy={accuracy}");

            return isFullCombo && greatCount == 0 && missCount == 0;
        }

        // 기존 "FULL COMBO!" 문자들을 숨기고 커스텀 "ALL PERFECT!" 배너를 표시합니다.
        private static void ShowAllPerfectBanner(Transform fcTransform)
        {
            int hiddenCount = 0;
            for (int i = 0; i < fcTransform.childCount; i++)
            {
                var child = fcTransform.GetChild(i);
                if (child == null) continue;

                MelonLogger.Msg($"[APMod.Debug.Victory] 발견된 자식 오브젝트: index={i}, name='{child.name}', active={child.gameObject.activeSelf}");
                if (child.name != "CustomAPText")
                {
                    child.gameObject.SetActive(false);
                    hiddenCount++;
                }
            }
            MelonLogger.Msg($"[APMod.Debug.Victory] 기존 FULL COMBO 관련 오브젝트 총 {hiddenCount}개 숨김 처리 완료.");

            var customApTransform = fcTransform.Find("CustomAPText");
            if (customApTransform != null)
            {
                customApTransform.gameObject.SetActive(true);
                MelonLogger.Msg("[APMod] 커스텀 ALL PERFECT 배너 텍스트 활성화 완료.");
                return;
            }

            CreateAllPerfectBanner(fcTransform);
        }

        // 커스텀 "ALL PERFECT!" 텍스트 오브젝트를 신규 생성하고 스타일을 적용합니다.
        private static void CreateAllPerfectBanner(Transform fcTransform)
        {
            MelonLogger.Msg("[APMod] CustomAPText 게임 오브젝트 신규 생성 프로세스 시작...");
            var apGo = new GameObject("CustomAPText");
            apGo.transform.SetParent(fcTransform, false);

            var customTextComp = apGo.AddComponent<Text>();
            customTextComp.font = ResolveBannerFont();
            customTextComp.text = "ALL PERFECT !";
            customTextComp.fontSize = 110;
            customTextComp.alignment = TextAnchor.MiddleCenter;
            // Harmonious vibrant gold/yellow color
            customTextComp.color = new Color(1f, 0.85f, 0f, 1f);

            // 3D 입체감을 위한 그림자
            var shadow = apGo.AddComponent<Shadow>();
            shadow.effectColor = new Color(0f, 0f, 0f, 0.8f);
            shadow.effectDistance = new Vector2(6f, -6f);

            // Muse Dash 스타일의 두꺼운 검정 외곽선
            var outline = apGo.AddComponent<Outline>();
            outline.effectColor = new Color(0f, 0f, 0f, 1f);
            outline.effectDistance = new Vector2(4f, -4f);

            var rect = apGo.GetComponent<RectTransform>();
            if (rect != null)
            {
                rect.anchoredPosition = Vector2.zero;
                rect.sizeDelta = new Vector2(1000f, 200f);
            }

            MelonLogger.Msg("[APMod] 커스텀 ALL PERFECT 배너 텍스트 생성 성공.");
        }

        // 게임플레이 중 캐싱한 프리미엄 폰트 → PnlVictory accuracyTxt → 씬 내 임의 Text → Arial 순으로 폰트를 해석합니다.
        private static Font ResolveBannerFont()
        {
            Font targetFont = VictoryDataCache.PremiumFont;
            if (targetFont != null)
            {
                MelonLogger.Msg($"[APMod.Debug.Victory] 캐싱해 둔 HUD 메인 시그니처 폰트 적용: '{targetFont.name}'");
                return targetFont;
            }

            MelonLogger.Msg("[APMod.Debug.Victory] 캐싱된 HUD 폰트가 null 상태입니다. PnlVictory에서 조회를 시도합니다.");
            var victoryPanel = GameObject.FindObjectOfType<Il2Cpp.PnlVictory>();
            if (victoryPanel != null)
            {
                // m_CurControls.accuracyTxt.font 체인을 리플렉션으로 안전하게 탐색 (필드명 변경 시 예외 없이 폴백)
                var curControls = ModReflection.GetValue(victoryPanel, "CurControls", silent: true);
                var accuracyTxt = curControls != null ? ModReflection.GetValue(curControls, "accuracyTxt", silent: true) : null;
                targetFont = accuracyTxt != null ? ModReflection.GetValue(accuracyTxt, "font", silent: true) as Font : null;
                if (targetFont != null)
                {
                    VictoryDataCache.PremiumFont = targetFont; // 다음 호출 시 FindObjectOfType 회피용 캐싱
                    MelonLogger.Msg($"[APMod.Debug.Victory] PnlVictory의 accuracyTxt에서 폰트 추출 및 캐싱 성공: '{targetFont.name}'");
                    return targetFont;
                }
            }

            var anyText = GameObject.FindObjectOfType<Text>();
            if (anyText != null && anyText.font != null)
            {
                targetFont = anyText.font;
                VictoryDataCache.PremiumFont = targetFont; // 다음 호출 시 FindObjectOfType 회피용 캐싱
                MelonLogger.Msg("[APMod] 활성화된 씬 내 Text 컴포넌트에서 폰트 획득 및 캐싱 완료.");
                return targetFont;
            }

            MelonLogger.Msg("[APMod] 폴백 빌트인 Arial 폰트 적용.");
            return Resources.GetBuiltinResource<Font>("Arial.ttf");
        }

        // 일반 풀콤보/퍼펙트 미달성 시 기존 "FULL COMBO!" 문자를 복원하고 커스텀 배너를 숨깁니다.
        private static void RestoreFullComboBanner(Transform fcTransform)
        {
            for (int i = 0; i < fcTransform.childCount; i++)
            {
                var child = fcTransform.GetChild(i);
                if (child != null && child.name.StartsWith("Img"))
                {
                    child.gameObject.SetActive(true);
                }
            }

            var customApTransform = fcTransform.Find("CustomAPText");
            if (customApTransform != null)
            {
                customApTransform.gameObject.SetActive(false);
            }
        }

    }
}
