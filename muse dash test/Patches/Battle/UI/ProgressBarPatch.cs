using System;
using HarmonyLib;
using Il2CppAssets.Scripts.Database;
using Il2CppAssets.Scripts.UI.Panels;
using Il2CppFormulaBase;
using UnityEngine;
using UnityEngine.UI;

namespace muse_dash_test
{
    /// <summary>
    /// 커스텀 차트/커스텀 BGM 플레이 시 배틀 상단 진행바(<c>sldProgress</c>)와 <c>StageBattleComponent.GetTimeProgress()</c>가
    /// 호스트 원본 곡 길이가 아닌 실제 커스텀 BGM/채보 총 길이를 기준으로 진행되도록 동기화합니다.
    ///
    /// <para><b>배경 (ISIL 분석):</b></para>
    /// <list type="bullet">
    /// <item><description><c>StageBattleComponent.GameStart()</c>는 <c>GameMusicScene.Run()</c>보다 먼저
    /// <c>PnlBattle.MusicProgressInit()</c>을 호출합니다. 이 시점에는 아직 <c>HwaBattleMediaController</c>의
    /// 비동기 <c>.ogg</c> 로드가 시작되기 전이라 <c>AudioManager.instance.bgm.clip</c>이 호스트 곡(예: 0-0)의 클립입니다.</description></item>
    /// <item><description>따라서 게임이 생성한 DOTween(<c>m_TwnProgress</c>)은 호스트 곡 길이로 <c>_MusicProgressInit_b__48_1(float)</c>을
    /// 호출합니다. 이 세터와 <c>GetTimeProgress()</c>를 커스텀 곡 총 길이(<c>max(customAudioLength, chartEndTime)</c>) 기준의
    /// 실제 재생 비율로 치환하고, 호스트 곡보다 긴 커스텀 곡에서도 끝까지 진행바가 갱신되도록 보정합니다.</description></item>
    /// </list>
    /// </summary>
    public static class ProgressBarPatch
    {
        private static float cachedChartDuration;
        private static float customAudioDuration;

        public static void Reset()
        {
            cachedChartDuration = 0f;
            customAudioDuration = 0f;
        }

        public static void OnCustomAudioClipLoaded(float clipLength)
        {
            if (clipLength > 1f)
            {
                customAudioDuration = clipLength;
                ModLogger.Msg($"[ProgressBarPatch] 커스텀 BGM 길이 반영: audio={clipLength:F2}s, chart={cachedChartDuration:F2}s, effective={GetEffectiveDuration():F2}s");
            }
        }

        public static float ComputeCustomChartDuration()
        {
            try
            {
                var db = GlobalDataBase.s_StageInfo;
                var list = db != null ? db.musicList : null;
                if (list == null || list.Count <= 1) return 0f;

                float maxEndTick = 0f;
                for (int i = 1; i < list.Count; i++)
                {
                    var note = list[i];
                    if (note == null) continue;

                    float tick = (float)note.tick;
                    float len = note.configData != null ? (float)note.configData.length : 0f;
                    float end = tick + Mathf.Max(0f, len);
                    if (end > maxEndTick)
                    {
                        maxEndTick = end;
                    }
                }

                return maxEndTick > 0f ? (maxEndTick + 2.0f) : 0f;
            }
            catch (Exception)
            {
                return 0f;
            }
        }

        public static float GetEffectiveDuration()
        {
            if (cachedChartDuration <= 0f)
            {
                cachedChartDuration = ComputeCustomChartDuration();
            }

            if (customAudioDuration > 1f && cachedChartDuration > 1f)
            {
                return Mathf.Max(customAudioDuration, cachedChartDuration);
            }

            return customAudioDuration > 1f ? customAudioDuration : cachedChartDuration;
        }

        public static bool TryGetCustomProgressRatio(out float ratio)
        {
            ratio = 0f;
            float duration = GetEffectiveDuration();
            if (duration <= 1f) return false;

            float elapsed = 0f;
            bool hasElapsed = false;

            var bgm = HwaBattleMediaController.ActiveAudioSource;
            if (bgm != null && bgm.clip != null && bgm.isPlaying)
            {
                elapsed = bgm.time;
                hasElapsed = true;
            }
            else
            {
                var stage = StageBattleComponent.instance;
                if (stage != null && stage.isInGame)
                {
                    elapsed = stage.timeFromMusicStart;
                    hasElapsed = true;
                }
            }

            if (!hasElapsed) return false;

            ratio = Mathf.Clamp01(elapsed / duration);
            return true;
        }

        public static void UpdateProgress(Slider sld)
        {
            if (sld == null) return;
            if (TryGetCustomProgressRatio(out float ratio))
            {
                sld.value = ratio;
            }
        }
    }

    [HarmonyPatch(typeof(PnlBattle), GameBindings.PnlBattle.MusicProgressInit)]
    public class PnlBattle_MusicProgressInit_Patch
    {
        public static void Postfix(PnlBattle __instance)
        {
            try
            {
                ModLogger.Msg("[ProgressBarPatch] PnlBattle.MusicProgressInit 호출됨 - 진행바 상태 점검(관찰 전용)");

                if (CustomPlaySession.Current.ShouldApplyExperimentChart)
                {
                    float chartDuration = ProgressBarPatch.ComputeCustomChartDuration();
                    ModLogger.Msg($"[ProgressBarPatch] 커스텀 채보 기준 초기 총 길이 계산: {chartDuration:F2}s (effective={ProgressBarPatch.GetEffectiveDuration():F2}s)");
                }

                if (__instance.CurrentBattleUIComp != null)
                {
                    Slider sld = __instance.CurrentBattleUIComp.sldProgress;
                    if (sld != null)
                    {
                        ModLogger.Msg("[ProgressBarPatch] sldProgress 슬라이더 감지됨 (관찰 전용, 별도 제어 없음)");
                    }
                    else
                    {
                        ModLogger.Warning("[ProgressBarPatch] sldProgress 슬라이더 컴포넌트가 null입니다.");
                    }
                }
                else
                {
                    ModLogger.Warning("[ProgressBarPatch] CurrentBattleUIComp가 null입니다.");
                }
            }
            catch (Exception ex)
            {
                ModLogger.Error($"[ProgressBarPatch.MusicProgressInit.Postfix] 예외 발생: {ex}");
            }
        }
    }

    [HarmonyPatch(typeof(PnlBattle), nameof(PnlBattle._MusicProgressInit_b__48_1), new Type[] { typeof(float) })]
    public class PnlBattle_MusicProgressSetter_Patch
    {
        public static void Prefix(ref float value)
        {
            try
            {
                if (!CustomPlaySession.Current.ShouldApplyExperimentChart) return;
                if (ProgressBarPatch.TryGetCustomProgressRatio(out float customRatio))
                {
                    value = customRatio;
                }
            }
            catch (Exception)
            {
            }
        }
    }

    [HarmonyPatch(typeof(StageBattleComponent), nameof(StageBattleComponent.GetTimeProgress))]
    public class StageBattleComponent_GetTimeProgress_Patch
    {
        public static void Postfix(ref float __result)
        {
            try
            {
                if (!CustomPlaySession.Current.ShouldApplyExperimentChart) return;
                if (ProgressBarPatch.TryGetCustomProgressRatio(out float customRatio))
                {
                    __result = customRatio;
                }
            }
            catch (Exception)
            {
            }
        }
    }
}
