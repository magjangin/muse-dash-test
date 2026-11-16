using System;
using System.Collections;
using HarmonyLib;
using Il2CppFormulaBase;
using MelonLoader;
using UnityEngine;

namespace muse_dash_test
{
    /// <summary>
    /// 진단용: 게임이 음악을 시작(FixedOffset)한 순간 전후로 BGM 시계와 게임 시계를 0.2초 간격으로 기록합니다.
    /// "처음에 어긋난 소리가 들렸다가 재동기화" 현상의 원인(BGM 위치 vs timeFromMusicStart)을 찾기 위한 것입니다.
    /// </summary>
    [HarmonyPatch(typeof(StageBattleComponent), nameof(StageBattleComponent.FixedOffset))]
    public class StageBattleComponent_FixedOffset_Diag_Patch
    {
        public static void Prefix(StageBattleComponent __instance)
        {
            Log("FixedOffset 직전", __instance);
        }

        public static void Postfix(StageBattleComponent __instance)
        {
            Log("FixedOffset 직후", __instance);
            MelonCoroutines.Start(Sample());
        }

        private static void Log(string tag, StageBattleComponent stage)
        {
            try
            {
                AudioSource bgm = GameBgmSource.TryGet(out _);
                ModLogger.Msg($"[BgmDiag] {tag}: bgm.time={(bgm != null ? bgm.time.ToString("F3") : "-")}, playing={(bgm != null && bgm.isPlaying)}, mute={(bgm != null && bgm.mute)}, 게임시계={stage.timeFromMusicStart:F3}, offset={stage.offset:F4}, getMusicOffset={stage.getMusicOffset}");
            }
            catch (Exception ex)
            {
                ModLogger.Error($"[BgmDiag] {tag} 실패: {ex.Message}");
            }
        }

        private static IEnumerator Sample()
        {
            for (int i = 0; i < 25; i++)
            {
                try
                {
                    AudioSource bgm = GameBgmSource.TryGet(out _);
                    var stage = StageBattleComponent.instance;
                    if (bgm != null && stage != null)
                    {
                        ModLogger.Msg($"[BgmDiag] +{i * 0.2f:F1}s: bgm.time={bgm.time:F3}, mute={bgm.mute}, 게임시계={stage.timeFromMusicStart:F3}, 차이(bgm-게임)={bgm.time - stage.timeFromMusicStart:F3}");
                    }
                }
                catch (Exception) { }
                yield return new WaitForSecondsRealtime(0.2f);
            }
        }
    }
}
