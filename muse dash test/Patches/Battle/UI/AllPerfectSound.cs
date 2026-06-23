using MelonLoader;
using System;
using HarmonyLib;
using UnityEngine;

namespace muse_dash_test.Patches
{
    /// <summary>
    /// ALL PERFECT 달성 시, 게임의 기본 풀콤보 효과음(sfx_full_combo*)을 그대로 울리지 않도록 뮤트합니다.
    /// (AP는 풀콤보보다 상위 판정인데 동일한 FC 효과음이 울리는 어색함을 제거)
    /// 일반 풀콤보/미달성에는 손대지 않아 기존 효과음이 그대로 재생됩니다.
    /// </summary>
    public static class AllPerfectSound
    {
        // 풀콤보 효과음 클립 이름 접두사. (sfx_full_combo, sfx_full_combo_djmax 등 스킨별 변형 포함)
        private const string FullComboClipPrefix = "sfx_full_combo";

        /// <summary>해당 클립이 게임 기본 풀콤보 효과음인지 판별합니다.</summary>
        public static bool IsFullComboClip(AudioClip clip)
        {
            return clip != null
                && !string.IsNullOrEmpty(clip.name)
                && clip.name.StartsWith(FullComboClipPrefix, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>현재 결과가 ALL PERFECT(풀콤보 + Great 0 + Miss 0)인지 판정합니다.</summary>
        public static bool IsAllPerfect()
        {
            var target = VictoryDataCache.ActiveTarget;
            if (target == null) return false;

            try
            {
                return target.IsFullCombo() && target.m_GreatResult == 0 && target.m_MissResult == 0;
            }
            catch (Exception ex)
            {
                MelonLogger.Warning($"[APSound] AP 판정 실패: {ex.Message}");
                return false;
            }
        }
    }

    // 풀콤보 효과음 PlayOneShot(AudioClip, float)을 가로채 AP일 때 차단합니다.
    // (확인된 호출: PlayOneShot(vol=0.40) clip='sfx_full_combo_djmax')
    [HarmonyPatch(typeof(AudioSource), nameof(AudioSource.PlayOneShot), new Type[] { typeof(AudioClip), typeof(float) })]
    public class AudioSource_PlayOneShot_APMute_Patch
    {
        public static bool Prefix(AudioClip clip)
        {
            try
            {
                if (!AllPerfectSound.IsFullComboClip(clip)) return true; // FC 효과음이 아니면 통과
                if (!AllPerfectSound.IsAllPerfect()) return true;        // AP가 아니면 FC 효과음 그대로

                MelonLogger.Msg($"[APSound] ★AP★ 풀콤보 효과음('{clip.name}') 재생을 뮤트합니다.");
                return false; // AP일 때 FC 효과음 차단
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[APSound] PlayOneShot 뮤트 중 예외: {ex}");
                return true;
            }
        }
    }

    // 볼륨 인자 없는 오버로드도 동일하게 처리(스킨/빌드 차이 대비).
    [HarmonyPatch(typeof(AudioSource), nameof(AudioSource.PlayOneShot), new Type[] { typeof(AudioClip) })]
    public class AudioSource_PlayOneShotNoVol_APMute_Patch
    {
        public static bool Prefix(AudioClip clip)
        {
            try
            {
                if (!AllPerfectSound.IsFullComboClip(clip)) return true;
                if (!AllPerfectSound.IsAllPerfect()) return true;

                MelonLogger.Msg($"[APSound] ★AP★ 풀콤보 효과음('{clip.name}') 재생을 뮤트합니다.");
                return false;
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[APSound] PlayOneShot(novol) 뮤트 중 예외: {ex}");
                return true;
            }
        }
    }
}
