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
    [HarmonyPatch(typeof(Il2CppAssets.Scripts.GameCore.HostComponent.TaskStageTarget), GameBindings.TaskStageTarget.AddScore)]
    public class TaskStageTarget_AddScore_Patch
    {
        public static void Prefix(Il2CppAssets.Scripts.GameCore.HostComponent.TaskStageTarget __instance)
        {
            if (!ModConfig.EnableAPMod) return;
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
    [HarmonyPatch(typeof(Il2CppAssets.Scripts.GameCore.HostComponent.TaskStageTarget), GameBindings.TaskStageTarget.GetAccuracy)]
    public class TaskStageTarget_GetAccuracy_Patch
    {
        public static void Postfix(Il2CppAssets.Scripts.GameCore.HostComponent.TaskStageTarget __instance, ref float __result)
        {
            if (!ModConfig.EnableAPMod) return;
            try
            {
                if (VictoryDataCache.ActiveTarget != __instance)
                {
                    VictoryDataCache.ActiveTarget = __instance;
                    MelonLogger.Msg($"[APMod] GetAccuracy를 통해 TaskStageTarget 캐싱 완료 ({__result}). Pointer={__instance.Pointer}");
                }

                // 버그 분석을 위해 TaskStageTarget의 원래 변수 값들을 캡처합니다.
                float rawGetAccuracy = __result;
                float rawGetTrueAccuracy = __instance.GetTrueAccuracy();
                float rawGetTrueAccuracyNew = __instance.GetTrueAccuracyNew();

                if (CustomPlaySession.Current.ShouldApplyExperimentChart)
                {
                    float accuracyNew = AccuracyCalculator.CalculateTrueAccuracyNew(__instance);
                    __result = (float)Math.Round(accuracyNew, 3);
                }

                // 원래의 로깅 형식 요구사항에 맞춰 그대로 한 줄 출력합니다.
                MelonLogger.Msg($"[APMod.Debug.Accuracy] " +
                                $"m_MusicCount={__instance.m_MusicCount}, " +
                                $"m_PerfectResult={__instance.m_PerfectResult}, " +
                                $"m_GreatResult={__instance.m_GreatResult}, " +
                                $"m_MissResult={__instance.m_MissResult}, " +
                                $"m_CoolResult={__instance.m_CoolResult}, " +
                                $"m_HitCount={__instance.m_HitCount}, " +
                                $"m_LongPressCount={__instance.m_LongPressCount}, " +
                                $"m_LongPressHitCount={__instance.m_LongPressHitCount}, " +
                                $"m_EnergyCount={__instance.m_EnergyCount}, " +
                                $"GetAccuracy()={rawGetAccuracy:F6}, " +
                                $"GetTrueAccuracy()={rawGetTrueAccuracy:F6}, " +
                                $"GetTrueAccuracyNew()={rawGetTrueAccuracyNew:F6}");
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[APMod] GetAccuracy Postfix 예외 발생: {ex}");
            }
        }
    }

    [HarmonyPatch(typeof(Il2CppAssets.Scripts.GameCore.HostComponent.TaskStageTarget), GameBindings.TaskStageTarget.GetTrueAccuracy)]
    public class TaskStageTarget_GetTrueAccuracy_Patch
    {
        public static void Postfix(Il2CppAssets.Scripts.GameCore.HostComponent.TaskStageTarget __instance, ref float __result)
        {
            if (!ModConfig.EnableAPMod) return;
            try
            {
                if (CustomPlaySession.Current.ShouldApplyExperimentChart)
                {
                    __result = AccuracyCalculator.CalculateTrueAccuracy(__instance);
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[APMod] GetTrueAccuracy Postfix 예외 발생: {ex}");
            }
        }
    }

    [HarmonyPatch(typeof(Il2CppAssets.Scripts.GameCore.HostComponent.TaskStageTarget), GameBindings.TaskStageTarget.GetTrueAccuracyNew)]
    public class TaskStageTarget_GetTrueAccuracyNew_Patch
    {
        public static void Postfix(Il2CppAssets.Scripts.GameCore.HostComponent.TaskStageTarget __instance, ref float __result)
        {
            if (!ModConfig.EnableAPMod) return;
            try
            {
                if (CustomPlaySession.Current.ShouldApplyExperimentChart)
                {
                    __result = AccuracyCalculator.CalculateTrueAccuracyNew(__instance);
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[APMod] GetTrueAccuracyNew Postfix 예외 발생: {ex}");
            }
        }
    }

    // Cache the TaskStageTarget instance when full combo is checked
    [HarmonyPatch(typeof(Il2CppAssets.Scripts.GameCore.HostComponent.TaskStageTarget), GameBindings.TaskStageTarget.IsFullCombo)]
    public class TaskStageTarget_IsFullCombo_Patch
    {
        public static void Postfix(Il2CppAssets.Scripts.GameCore.HostComponent.TaskStageTarget __instance, bool __result)
        {
            if (!ModConfig.EnableAPMod) return;
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
}
