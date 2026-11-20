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
    /// 호출합니다. 이 세터와 <c>GetTimeProgress()</c>를 게임 시계와 커스텀 음원 길이(음원이 없으면 채보 길이) 기준의
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
                string gameClock = TryReadGameClock(out float clock) ? clock.ToString("F2") + "s" : "(none)";
                return $"ratio={(hasRatio ? ratio.ToString("P1") : "(none)")}, game={gameClock}, bgm.time={bgmTime}, " +
                       $"duration={duration:F2}s (audio={customAudioDuration:F2}s, chart={cachedChartDuration:F2}s)";
            }
            catch (Exception ex)
            {
                return "(진행 상태 조회 예외: " + ex.GetType().Name + ")";
            }
        }

        /// <summary>
        /// 배틀이 끝나면 진행바를 끝까지 둡니다. 게임 시계 기준 비율(<see cref="TryGetCustomProgressRatio"/>)은 <c>End</c>
        /// 순간 이미 100%라 눈에 보이는 점프는 없고, <c>End</c> 뒤 게임 시계가 멈추거나 초기화돼도 100%를 유지하는 안전망입니다.
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

            // 커스텀 음원이 있으면 그 길이만 씁니다. 게임이 End를 재는 길이(bgm.clip.length)가 바로 이 클립이라
            // 채보가 더 길어도 max를 쓰면 End 때 진행바가 덜 찹니다. 음원이 없을 때만 채보 길이로 물러납니다.
            return customAudioDuration > 1f ? customAudioDuration : cachedChartDuration;
        }

        /// <summary>
        /// 진행 비율 = 게임 시계(<c>timeFromMusicStart</c>) / 게임이 끝을 재는 길이.
        /// <para><b>왜 게임 시계인가</b>: 게임의 <c>GetTimeProgress</c>도 <c>timeFromMusicStart / bgm.clip.length</c>이고,
        /// <c>GameMusic.BattleEnd</c>는 <c>timeFromMusicStart ≥ bgm.clip.length</c>가 되는 프레임에 <c>End</c>를 예약합니다(ISIL,
        /// 2026-11-21). 같은 시계·같은 분모를 쓰면 진행바는 <c>End</c> 순간 정확히 100%가 됩니다. 예전처럼 <c>bgm.time</c>을 쓰면
        /// 오프셋 주입만큼(약 0.5초) 게임 시계보다 늦어 다 차기 전에 결과로 넘어갔습니다.</para>
        /// <para>게임 시계는 <c>Stopwatch</c>라 음악 시작 전에는 지난 배틀 값이 남아 있을 수 있어, 게임이 음악을 시작한
        /// 뒤(<c>FixedOffset</c>, <see cref="HwaBattleMediaController.gameMusicStarted"/>)에만 씁니다. 승리 배너 뒤에도
        /// <c>End</c>까지 계속 차오르도록 <c>isInGame</c>은 보지 않습니다(게임이 <c>End</c> 예약 순간 끕니다).</para>
        /// </summary>
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
            if (!HwaBattleMediaController.gameMusicStarted) return false;

            float elapsed;
            try
            {
                if (!TryReadGameClock(out elapsed)) return false;
            }
            catch (Exception)
            {
                return false;
            }

            ratio = Mathf.Clamp01(elapsed / duration);
            return true;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static bool TryReadGameClock(out float elapsed)
        {
            elapsed = 0f;
            var stage = StageBattleComponent.instance;
            if (stage == null) return false;
            elapsed = stage.timeFromMusicStart;
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
