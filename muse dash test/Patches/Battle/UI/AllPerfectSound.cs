using MelonLoader;
using System;
using System.Collections.Generic;
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

        /// <summary>
        /// 클립(네이티브 포인터)별 "풀콤보 효과음인가" 판정 캐시입니다.
        ///
        /// <para><b>왜 필요한가</b>: 아래 Prefix들은 <see cref="AudioSource.PlayOneShot"/>이라는
        /// 게임 전역 API에 걸려 있어 노트 타격음까지 <i>모든</i> 효과음마다 실행됩니다.
        /// 판정에 <c>clip.name</c>이 필요한데, 읽을 때마다 IL2CPP 문자열을 관리 문자열로 복사(할당)합니다.
        /// 앞단의 <see cref="IsResultContextActive"/> 게이트는 이걸 막아 주지 못했습니다 — 그 값은
        /// <c>AddScore</c>가 매 배틀 첫 타격에서 채우므로, 배틀 대부분의 구간에서 이미 참이었습니다.</para>
        ///
        /// <para>타격음은 몇 가지 클립이 계속 재사용되므로, 클립마다 이름을 한 번만 읽고 결과를 기억합니다.
        /// 포인터는 에셋이 해제된 뒤 재사용될 수 있어서 배틀을 로드할 때마다 비웁니다(<see cref="ResetClipCache"/>).</para>
        /// </summary>
        private static readonly Dictionary<IntPtr, bool> fullComboClipByPointer = new Dictionary<IntPtr, bool>();

        /// <summary>배틀 로드 시 호출합니다. 이전 배틀에서 기억한 클립 판정을 버립니다.</summary>
        public static void ResetClipCache()
        {
            fullComboClipByPointer.Clear();
        }

        /// <summary>해당 클립이 게임 기본 풀콤보 효과음인지 판별합니다. 클립마다 이름은 한 번만 읽습니다.</summary>
        public static bool IsFullComboClip(AudioClip clip)
        {
            if (clip == null) return false;

            IntPtr pointer = clip.Pointer;
            if (fullComboClipByPointer.TryGetValue(pointer, out bool known)) return known;

            string name = clip.name;
            bool isFullCombo = !string.IsNullOrEmpty(name)
                && name.StartsWith(FullComboClipPrefix, StringComparison.OrdinalIgnoreCase);
            fullComboClipByPointer[pointer] = isFullCombo;
            return isFullCombo;
        }

        /// <summary>
        /// 결과 판정을 물어볼 수 있는 상태인지 확인합니다(= 이번 배틀의 TaskStageTarget이 캐시된 상태).
        /// 정적 필드 하나만 보므로 사실상 공짜입니다. 다만 배틀 첫 타격 이후로는 참이 되므로,
        /// 효과음마다의 이름 읽기는 이 게이트가 아니라 <see cref="IsFullComboClip"/>의 캐시가 줄입니다.
        /// </summary>
        public static bool IsResultContextActive()
        {
            return VictoryDataCache.ActiveTarget != null;
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
                ModLogger.Warning($"[APSound] AP 판정 실패: {ex.Message}");
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
            // 아래 두 줄은 게임 전역 효과음마다 실행됩니다. 가장 싼 검사부터 둡니다.
            if (!ModConfig.EnableAllPerfectSound) return true;
            if (!AllPerfectSound.IsResultContextActive()) return true;   // 이번 배틀 첫 타격 전이면 여기서 끝
            try
            {
                if (!AllPerfectSound.IsFullComboClip(clip)) return true; // FC 효과음이 아니면 통과
                if (!AllPerfectSound.IsAllPerfect()) return true;        // AP가 아니면 FC 효과음 그대로

                ModLogger.Msg($"[APSound] ★AP★ 풀콤보 효과음('{clip.name}') 재생을 뮤트합니다.");
                return false; // AP일 때 FC 효과음 차단
            }
            catch (Exception ex)
            {
                ModLogger.Error($"[APSound] PlayOneShot 뮤트 중 예외: {ex}");
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
            // 볼륨 인자 있는 오버로드와 동일한 순서/게이트를 씁니다.
            // (이 오버로드에는 EnableAllPerfectSound 검사가 빠져 있어, 설정을 꺼도 절반만 꺼졌습니다.)
            if (!ModConfig.EnableAllPerfectSound) return true;
            if (!AllPerfectSound.IsResultContextActive()) return true;
            try
            {
                if (!AllPerfectSound.IsFullComboClip(clip)) return true;
                if (!AllPerfectSound.IsAllPerfect()) return true;

                ModLogger.Msg($"[APSound] ★AP★ 풀콤보 효과음('{clip.name}') 재생을 뮤트합니다.");
                return false;
            }
            catch (Exception ex)
            {
                ModLogger.Error($"[APSound] PlayOneShot(novol) 뮤트 중 예외: {ex}");
                return true;
            }
        }
    }
}
