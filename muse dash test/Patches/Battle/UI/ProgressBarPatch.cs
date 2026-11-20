using System;
using System.Reflection;
using System.Runtime.CompilerServices;
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
        private static bool hasComputedChartDuration;
        private static float customAudioDuration;

        /// <summary>
        /// 이번 배틀이 끝났는지(<c>StageBattleComponent.End</c>). 끝난 뒤에는 진행바를 100%로 둡니다.
        /// <see cref="Reset"/>은 <c>End</c> 안의 <c>StopMedia</c>가 부르므로 여기서 지우지 않고, 다음 배틀을 시작할 때만
        /// 지웁니다(<see cref="BeginBattle"/>).
        /// </summary>
        private static bool battleEnded;

        public static void Reset()
        {
            cachedChartDuration = 0f;
            hasComputedChartDuration = false;
            customAudioDuration = 0f;
        }

        /// <summary>새 배틀을 불러올 때(<c>HwaBattleMediaController.ResetState</c>)와 진행바가 시작될 때(<c>PnlBattle.MusicProgressInit</c>) 부릅니다.</summary>
        public static void BeginBattle()
        {
            battleEnded = false;
        }

        /// <summary>
        /// 배틀 종료 직전의 진행 상태를 로그용 문자열로 만듭니다. <c>StopMedia</c>가 BGM 소스를 치우기 전에 불러야 합니다.
        /// </summary>
        public static string DescribeProgressBeforeEnd()
        {
            try
            {
                float duration = GetEffectiveDuration();
                bool hasRatio = TryGetCustomProgressRatio(out float ratio);
                var bgm = HwaBattleMediaController.ActiveAudioSource;
                string bgmTime = bgm != null && bgm.clip != null ? bgm.time.ToString("F2") + "s" : "(none)";
                return $"ratio={(hasRatio ? ratio.ToString("P1") : "(none)")}, bgm.time={bgmTime}, " +
                       $"duration={duration:F2}s (audio={customAudioDuration:F2}s, chart={cachedChartDuration:F2}s)";
            }
            catch (Exception ex)
            {
                return "(진행 상태 조회 예외: " + ex.GetType().Name + ")";
            }
        }

        /// <summary>
        /// 배틀이 끝나면 진행바를 끝까지 채웁니다.
        /// <para><b>왜</b>: 게임은 커스텀 음원이 끝나기 약 0.5초 전에 <c>End</c>를 부릅니다(오프셋 주입으로 게임 시계가
        /// BGM보다 앞섬, 2026-11-20 로그: 주입 후 176.14초에 End, 음원 176.65초). 진행바는 <c>bgm.time / 음원 길이</c>라
        /// 다 차기 전에 결과 화면으로 넘어갔습니다. 바닐라는 트윈 길이가 음원 길이와 같아 끝에서 가득 찹니다.</para>
        /// </summary>
        public static void FillOnBattleEnd(Slider sld)
        {
            battleEnded = true;
            if (sld != null) sld.value = 1f;
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
                return ReadChartDurationFromStageInfo();
            }
            catch (Exception)
            {
                return 0f;
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static float ReadChartDurationFromStageInfo()
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

        public static float GetEffectiveDuration()
        {
            if (!hasComputedChartDuration)
            {
                cachedChartDuration = ComputeCustomChartDuration();
                hasComputedChartDuration = true;
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
            if (battleEnded)
            {
                ratio = 1f;
                return true;
            }

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
                try
                {
                    hasElapsed = TryReadStageBattleElapsed(out elapsed);
                }
                catch (Exception)
                {
                    hasElapsed = false;
                }
            }

            if (!hasElapsed) return false;

            ratio = Mathf.Clamp01(elapsed / duration);
            return true;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static bool TryReadStageBattleElapsed(out float elapsed)
        {
            elapsed = 0f;
            var stage = StageBattleComponent.instance;
            if (stage != null && stage.isInGame)
            {
                elapsed = stage.timeFromMusicStart;
                return true;
            }
            return false;
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

    [HarmonyPatch(typeof(PnlBattle), nameof(PnlBattle.MusicProgressInit))]
    public class PnlBattle_MusicProgressInit_Patch
    {
        public static void Postfix(PnlBattle __instance)
        {
            try
            {
                ModLogger.Msg("[ProgressBarPatch] PnlBattle.MusicProgressInit 호출됨 - 진행바 상태 점검(관찰 전용)");
                ProgressBarPatch.BeginBattle();

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

    /// <summary>
    /// <c>PnlBattle.MusicProgressInit</c> 내부의 DOTween DOSetter 메서드(<c>_MusicProgressInit_b__48_1(float)</c>)를 후킹합니다.
    /// 컴파일러 생성 세터 메서드 이름의 순번(48)은 게임 업데이트로 <c>PnlBattle</c>에 메서드가 추가/삭제되면
    /// 바뀔 수 있으므로, 고정 이름이 없을 때 <c>_MusicProgressInit_b__*</c> 패턴 + <c>void(float)</c> 시그니처로 동적 탐색합니다.
    /// </summary>
    [HarmonyPatch]
    public class PnlBattle_MusicProgressSetter_Patch
    {
        public static MethodBase TargetMethod()
        {
            var exact = AccessTools.Method(typeof(PnlBattle), nameof(PnlBattle._MusicProgressInit_b__48_1), new Type[] { typeof(float) });
            if (exact != null) return exact;

            foreach (var m in typeof(PnlBattle).GetMethods(
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly))
            {
                if (!m.Name.StartsWith("_MusicProgressInit_b__", StringComparison.Ordinal)) continue;
                if (m.ReturnType != typeof(void)) continue;
                var ps = m.GetParameters();
                if (ps.Length == 1 && ps[0].ParameterType == typeof(float))
                {
                    ModLogger.Msg($"[ProgressBarPatch] 게임 업데이트 폴백으로 진행바 세터 메서드를 해석했습니다: {m.Name}");
                    return m;
                }
            }

            return null;
        }

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
            catch (Exception ex)
            {
                ModLogger.Verbose($"[ProgressBarPatch.SetProgress] Prefix 예외: {ex.GetType().Name}: {ex.Message}");
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
            catch (Exception ex)
            {
                ModLogger.Verbose($"[ProgressBarPatch.GetTimeProgress] Postfix 예외: {ex.GetType().Name}: {ex.Message}");
            }
        }
    }
}
