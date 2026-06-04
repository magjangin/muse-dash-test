using MelonLoader;
using System;
using HarmonyLib;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using UnityEngine;
using UnityEngine.UI;

namespace muse_dash_test.Patches
{
    public static class VictoryDataCache
    {
        public static Il2CppAssets.Scripts.GameCore.HostComponent.TaskStageTarget ActiveTarget { get; set; }
        public static Font PremiumFont { get; set; }
        public static bool AttemptedFontCache { get; set; }
    }

    // Cache the TaskStageTarget instance during gameplay when score is updated
    [HarmonyPatch(typeof(Il2CppAssets.Scripts.GameCore.HostComponent.TaskStageTarget), "AddScore")]
    public class TaskStageTarget_AddScore_Patch
    {
        public static void Prefix(Il2CppAssets.Scripts.GameCore.HostComponent.TaskStageTarget __instance)
        {
            try
            {
                if (VictoryDataCache.ActiveTarget != __instance)
                {
                    VictoryDataCache.ActiveTarget = __instance;
                    MelonLogger.Msg($"[APMod] AddScore를 통해 TaskStageTarget 캐싱 완료. Pointer={__instance.Pointer}");
                }

                // Cache the premium stylized gameplay font from the HUD
                if (VictoryDataCache.PremiumFont == null && !VictoryDataCache.AttemptedFontCache)
                {
                    var battleInstance = Il2CppAssets.Scripts.UI.Panels.PnlBattle.instance;
                    if (battleInstance != null && battleInstance.currentComps != null && battleInstance.currentComps.scoreValue != null)
                    {
                        var scoreValue = battleInstance.currentComps.scoreValue;
                        
                        // Prevent querying every hit when UI objects are not initialized yet
                        if (scoreValue.text == null && scoreValue.djmaxText == null && scoreValue.arkNightText == null)
                        {
                            return;
                        }

                        VictoryDataCache.AttemptedFontCache = true;
                        Font font = null;
                        
                        MelonLogger.Msg($"[APMod.Debug.Font] HUD 폰트 캐싱 시도 시작 - textObj={scoreValue.text != null}, djmaxTextObj={scoreValue.djmaxText != null}, arkNightTextObj={scoreValue.arkNightText != null}");
                        
                        if (scoreValue.text != null) 
                        {
                            font = scoreValue.text.font;
                            if (font != null) MelonLogger.Msg($"[APMod.Debug.Font] 일반 폰트 획득 완료: '{font.name}'");
                        }
                        if (font == null && scoreValue.djmaxText != null) 
                        {
                            font = scoreValue.djmaxText.font;
                            if (font != null) MelonLogger.Msg($"[APMod.Debug.Font] DJMAX 폰트 획득 완료: '{font.name}'");
                        }
                        if (font == null && scoreValue.arkNightText != null) 
                        {
                            font = scoreValue.arkNightText.font;
                            if (font != null) MelonLogger.Msg($"[APMod.Debug.Font] 아크나이츠 폰트 획득 완료: '{font.name}'");
                        }

                        if (font != null)
                        {
                            VictoryDataCache.PremiumFont = font;
                            MelonLogger.Msg($"[APMod] 게임플레이 HUD에서 최종 메인 시그니처 폰트 캐싱 완료: '{font.name}'");
                        }
                        else
                        {
                            MelonLogger.Warning("[APMod.Debug.Font] HUD 텍스트 컴포넌트들을 찾았으나 Font 리소스가 null 상태입니다.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[APMod] AddScore Prefix 예외 발생: {ex}");
            }
        }
    }

    // Cache the TaskStageTarget instance when accuracy is requested
    [HarmonyPatch(typeof(Il2CppAssets.Scripts.GameCore.HostComponent.TaskStageTarget), "GetAccuracy")]
    public class TaskStageTarget_GetAccuracy_Patch
    {
        public static void Postfix(Il2CppAssets.Scripts.GameCore.HostComponent.TaskStageTarget __instance, float __result)
        {
            try
            {
                if (VictoryDataCache.ActiveTarget != __instance)
                {
                    VictoryDataCache.ActiveTarget = __instance;
                    MelonLogger.Msg($"[APMod] GetAccuracy를 통해 TaskStageTarget 캐싱 완료 ({__result}). Pointer={__instance.Pointer}");
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[APMod] GetAccuracy Postfix 예외 발생: {ex}");
            }
        }
    }

    // Cache the TaskStageTarget instance when full combo is checked
    [HarmonyPatch(typeof(Il2CppAssets.Scripts.GameCore.HostComponent.TaskStageTarget), "IsFullCombo")]
    public class TaskStageTarget_IsFullCombo_Patch
    {
        public static void Postfix(Il2CppAssets.Scripts.GameCore.HostComponent.TaskStageTarget __instance, bool __result)
        {
            try
            {
                if (VictoryDataCache.ActiveTarget != __instance)
                {
                    VictoryDataCache.ActiveTarget = __instance;
                    MelonLogger.Msg($"[APMod] IsFullCombo를 통해 TaskStageTarget 캐싱 완료 ({__result}). Pointer={__instance.Pointer}");
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[APMod] IsFullCombo Postfix 예외 발생: {ex}");
            }
        }
    }

    // Intercept the transient Full Combo banner display and change it to ALL PERFECT if appropriate
    [HarmonyPatch(typeof(Il2CppAssets.Scripts.UI.GameMain.PnlVictory2dManager), "OnShowVictory", new Type[] { typeof(Il2CppSystem.Object), typeof(Il2CppSystem.Object), typeof(Il2CppReferenceArray<Il2CppSystem.Object>) })]
    public class PnlVictory2dManager_OnShowVictory_Patch
    {
        public static void Postfix(Il2CppAssets.Scripts.UI.GameMain.PnlVictory2dManager __instance, Il2CppSystem.Object sender, Il2CppSystem.Object rev, Il2CppReferenceArray<Il2CppSystem.Object> pars)
        {
            try
            {
                MelonLogger.Msg("[APMod] PnlVictory2dManager.OnShowVictory Postfix 감지!");

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

                bool isFullCombo = false;
                bool isAllPerfect = false;

                if (VictoryDataCache.ActiveTarget != null)
                {
                    isFullCombo = VictoryDataCache.ActiveTarget.IsFullCombo();
                    int greatCount = VictoryDataCache.ActiveTarget.m_GreatResult;
                    int missCount = VictoryDataCache.ActiveTarget.m_MissResult;
                    float accuracy = VictoryDataCache.ActiveTarget.GetAccuracy();
                    
                    MelonLogger.Msg($"[APMod.Debug.Victory] 판정 결과 확인 - FC={isFullCombo}, Great={greatCount}, Miss={missCount}, Accuracy={accuracy}");
                    
                    // If it is a full combo, and there are zero Greats and zero Misses, it is an All Perfect!
                    if (isFullCombo && greatCount == 0 && missCount == 0)
                    {
                        isAllPerfect = true;
                    }
                }
                else
                {
                    MelonLogger.Msg("[APMod.Debug.Victory] [주의] VictoryDataCache.ActiveTarget가 null입니다! 콤보 판정을 가져올 수 없습니다.");
                }

                if (isAllPerfect)
                {
                    MelonLogger.Msg("[APMod] ★ALL PERFECT 달성!★ 승리 배너 수정 프로세스 개시.");

                    // 1. Hide the original "FULL COMBO!" individual letter child GameObjects
                    int hiddenCount = 0;
                    for (int i = 0; i < fcGo.transform.childCount; i++)
                    {
                        var child = fcGo.transform.GetChild(i);
                        if (child != null)
                        {
                            MelonLogger.Msg($"[APMod.Debug.Victory] 발견된 자식 오브젝트: index={i}, name='{child.name}', active={child.gameObject.activeSelf}");
                            if (child.name != "CustomAPText")
                            {
                                child.gameObject.SetActive(false);
                                hiddenCount++;
                            }
                        }
                    }
                    MelonLogger.Msg($"[APMod.Debug.Victory] 기존 FULL COMBO 관련 오브젝트 총 {hiddenCount}개 숨김 처리 완료.");

                    // 2. Create or activate our custom "ALL PERFECT!" text
                    var customApTransform = fcGo.transform.Find("CustomAPText");
                    Text customTextComp = null;
                    if (customApTransform == null)
                    {
                        MelonLogger.Msg("[APMod] CustomAPText 게임 오브젝트 신규 생성 프로세스 시작...");
                        var apGo = new GameObject("CustomAPText");
                        apGo.transform.SetParent(fcGo.transform, false);

                        customTextComp = apGo.AddComponent<Text>();

                        // Borrow the premium stylized font cached during gameplay if available,
                        // otherwise try to find one from the victory panel or active UI, and finally fall back to Arial
                        Font targetFont = VictoryDataCache.PremiumFont;
                        if (targetFont != null)
                        {
                            MelonLogger.Msg($"[APMod.Debug.Victory] 캐싱해 둔 HUD 메인 시그니처 폰트 적용: '{targetFont.name}'");
                        }
                        else
                        {
                            MelonLogger.Msg("[APMod.Debug.Victory] 캐싱된 HUD 폰트가 null 상태입니다. PnlVictory에서 조회를 시도합니다.");
                            var victoryPanel = GameObject.FindObjectOfType<Il2Cpp.PnlVictory>();
                            if (victoryPanel != null && victoryPanel.m_CurControls != null && victoryPanel.m_CurControls.accuracyTxt != null)
                            {
                                targetFont = victoryPanel.m_CurControls.accuracyTxt.font;
                                if (targetFont != null)
                                {
                                    VictoryDataCache.PremiumFont = targetFont; // Cache it to avoid FindObjectOfType next time
                                    MelonLogger.Msg($"[APMod.Debug.Victory] PnlVictory의 accuracyTxt에서 폰트 추출 및 캐싱 성공: '{targetFont.name}'");
                                }
                            }
                        }

                        if (targetFont == null)
                        {
                            var anyText = GameObject.FindObjectOfType<Text>();
                            if (anyText != null && anyText.font != null)
                            {
                                targetFont = anyText.font;
                                VictoryDataCache.PremiumFont = targetFont; // Cache it to avoid FindObjectOfType next time
                                MelonLogger.Msg("[APMod] 활성화된 씬 내 Text 컴포넌트에서 폰트 획득 및 캐싱 완료.");
                            }
                        }

                        if (targetFont == null)
                        {
                            targetFont = Resources.GetBuiltinResource<Font>("Arial.ttf");
                            MelonLogger.Msg("[APMod] 폴백 빌트인 Arial 폰트 적용.");
                        }

                        customTextComp.font = targetFont;

                        // Set custom stylized text
                        customTextComp.text = "ALL PERFECT !";
                        customTextComp.fontSize = 110;
                        customTextComp.alignment = TextAnchor.MiddleCenter;
                        
                        // Harmonious vibrant gold/yellow color
                        customTextComp.color = new Color(1f, 0.85f, 0f, 1f);

                        // Add Shadow for a clean 3D outline effect
                        var shadow = apGo.AddComponent<Shadow>();
                        shadow.effectColor = new Color(0f, 0f, 0f, 0.8f);
                        shadow.effectDistance = new Vector2(6f, -6f);

                        // Add Outline for a gorgeous thick outline matching Muse Dash style
                        var outline = apGo.AddComponent<Outline>();
                        outline.effectColor = new Color(0f, 0f, 0f, 1f); // Thick black outline
                        outline.effectDistance = new Vector2(4f, -4f);

                        // Align centered in RectTransform
                        var rect = apGo.GetComponent<RectTransform>();
                        if (rect != null)
                        {
                            rect.anchoredPosition = Vector2.zero;
                            rect.sizeDelta = new Vector2(1000f, 200f);
                        }

                        MelonLogger.Msg("[APMod] 커스텀 ALL PERFECT 배너 텍스트 생성 성공.");
                    }
                    else
                    {
                        customApTransform.gameObject.SetActive(true);
                        MelonLogger.Msg("[APMod] 커스텀 ALL PERFECT 배너 텍스트 활성화 완료.");
                    }
                }
                else
                {
                    MelonLogger.Msg("[APMod] 일반 풀콤보 또는 퍼펙트 미달성. 기존 FULL COMBO 문자 복원 활성화.");
                    for (int i = 0; i < fcGo.transform.childCount; i++)
                    {
                        var child = fcGo.transform.GetChild(i);
                        if (child != null && child.name.StartsWith("Img"))
                        {
                            child.gameObject.SetActive(true);
                        }
                    }

                    var customApTransform = fcGo.transform.Find("CustomAPText");
                    if (customApTransform != null)
                    {
                        customApTransform.gameObject.SetActive(false);
                    }
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[APMod] OnShowVictory Postfix 예외 발생: {ex}");
            }
        }
    }
}
