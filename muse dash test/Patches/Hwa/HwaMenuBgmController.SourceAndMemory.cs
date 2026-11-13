using MelonLoader;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace muse_dash_test
{
    public static partial class HwaMenuBgmController
    {
        private static readonly List<RetiredClip> retiredMenuClips = new List<RetiredClip>();

        /// <summary>
        /// 밀려난 스트리밍 클립을 이 시간(실시간 초)이 지나기 전에는 파괴하지 않습니다.
        /// <para>메뉴 미리듣기는 스트리밍으로 읽습니다. 예전에 스트리밍 클립을 교체 직후 바로 <c>Destroy</c>했더니
        /// FMOD 비동기 스트리머가 아직 읽고 있던 버퍼를 놓쳐 휠을 빠르게 돌릴 때 0xc0000005로 죽었습니다(8351e5c).
        /// 그때는 풀 디코딩으로 바꿔 피했지만, 곡을 넘길 때마다 메인 스레드가 곡 길이에 비례해 멈췄습니다
        /// (2026-11-12 실측 134~184초 곡에서 316~450ms. <c>DownloadHandlerAudioClip.compressed</c>는 효과가 없었습니다).
        /// 스트리밍 클립은 디코딩 버퍼만 들고 있어 몇 초 더 살려 둬도 메모리 부담이 작습니다.</para>
        /// </summary>
        private const float RetireGraceSeconds = 3f;

        private struct RetiredClip
        {
            public AudioClip Clip;
            public float RetiredAt;
        }

        private static void RetireMenuClip(AudioClip clip)
        {
            if (clip == null) return;
            foreach (var retired in retiredMenuClips)
            {
                if (retired.Clip == clip) return;
            }

            retiredMenuClips.Add(new RetiredClip { Clip = clip, RetiredAt = Time.realtimeSinceStartup });
            // 다른 곳에서 정리를 부르지 않아도(곡을 고른 채 그대로 두거나 화면을 떠난 경우) 유예가 끝나면 해제되게 합니다.
            MelonCoroutines.Start(SweepRetiredClipsAfterGrace());
        }

        private static IEnumerator SweepRetiredClipsAfterGrace()
        {
            yield return new WaitForSeconds(RetireGraceSeconds + 0.5f);
            DestroyUnreferencedRetiredClips("retire grace elapsed");
        }

        public static void DestroyUnreferencedRetiredClips(string reason)
        {
            if (retiredMenuClips.Count == 0) return;

            AudioSource activeSource = FindMenuAudioSource();
            IntPtr activeClipPtr = (activeSource != null && activeSource.clip != null)
                ? activeSource.clip.Pointer
                : IntPtr.Zero;

            float now = Time.realtimeSinceStartup;
            for (int i = retiredMenuClips.Count - 1; i >= 0; i--)
            {
                AudioClip candidate = retiredMenuClips[i].Clip;
                if (candidate == null)
                {
                    retiredMenuClips.RemoveAt(i);
                    continue;
                }

                if (activeClipPtr != IntPtr.Zero && candidate.Pointer == activeClipPtr)
                {
                    continue;
                }

                // 스트리머가 아직 이 클립의 버퍼를 읽고 있을 수 있습니다(RetireGraceSeconds 참고).
                if (now - retiredMenuClips[i].RetiredAt < RetireGraceSeconds)
                {
                    continue;
                }

                string candidateName = candidate.name;
                retiredMenuClips.RemoveAt(i);
                try
                {
                    UnityEngine.Object.Destroy(candidate);
                    ModLogger.Verbose($"[MenuBGM.Memory] 은퇴한 메뉴 BGM 클립 해제 완료 ({reason}): {candidateName}");
                }
                catch (Exception ex)
                {
                    ModLogger.Verbose($"[MenuBGM.Memory] 은퇴 클립 해제 무시된 예외: {ex.Message}");
                }
            }
        }

        private static AudioSource FindMenuAudioSource()
        {
            try
            {
                // 게임이 원곡을 트는 소스를 먼저 씁니다. 이름 검색과 스캔은 그걸 못 얻었을 때만 탑니다(GameBgmSource 참고).
                AudioSource gameBgm = GameBgmSource.TryGet(out string failure);
                if (gameBgm != null && gameBgm.gameObject.activeInHierarchy)
                {
                    ModLogger.Verbose($"[MenuBGM] AudioManager.bgm 사용: GO={gameBgm.gameObject.name}");
                    return gameBgm;
                }
                ModLogger.Verbose($"[MenuBGM] AudioManager.bgm을 쓰지 못해 이름으로 찾습니다: {failure ?? "비활성 상태"}");

                // 0단계: 핀포인트로 "BGM" 오브젝트 검색 시도
                GameObject bgmGo = GameObject.Find("BGM");
                if (bgmGo != null)
                {
                    AudioSource source = bgmGo.GetComponent<AudioSource>();
                    if (source != null && source.gameObject.activeInHierarchy)
                    {
                        ModLogger.Verbose($"[MenuBGM] 핀포인트 매칭 성공: GO={bgmGo.name}");
                        return source;
                    }
                }

                // 핀포인트 검색 실패 시에만 예외적으로 씬 내 전체 스캔 진행 (폴백)
                ModLogger.Msg("[MenuBGM] 핀포인트 검색 실패, 씬 내 모든 AudioSource 스캔 폴백 진행");
                AudioSource[] sources = UnityEngine.Object.FindObjectsOfType<AudioSource>();
                if (sources == null || sources.Length == 0)
                {
                    ModLogger.Warning("[MenuBGM] 씬 내에 어떠한 AudioSource도 존재하지 않습니다.");
                    return null;
                }

                foreach (AudioSource source in sources)
                {
                    if (source == null) continue;
                    string goName = source.gameObject != null ? source.gameObject.name : "(null)";
                    string clipName = source.clip != null ? source.clip.name : "(null)";
                    ModLogger.Verbose($"[MenuBGM] 후보 - GO: {goName}, Clip: {clipName}, Playing: {source.isPlaying}, Vol: {source.volume}, Mute: {source.mute}, SpatialBlend: {source.spatialBlend}, Enabled: {source.enabled}, Active: {source.gameObject?.activeInHierarchy}");
                }

                AudioSource selectedSource = FindActiveSource(s => s.gameObject.name.Equals("BGM", StringComparison.OrdinalIgnoreCase));
                if (selectedSource != null)
                {
                    ModLogger.Verbose($"[MenuBGM] 0단계(이름 매칭) 성공: GO={selectedSource.gameObject.name}");
                    return selectedSource;
                }

                selectedSource = FindActiveSource(IsLikelySoundtrackSource);
                if (selectedSource != null)
                {
                    ModLogger.Verbose($"[MenuBGM] 1단계 매칭 성공: GO={selectedSource.gameObject.name.ToLower()}, Clip={selectedSource.clip.name}");
                    return selectedSource;
                }

                selectedSource = FindActiveSource(IsPlayingNonEffectSource);
                if (selectedSource != null)
                {
                    ModLogger.Verbose($"[MenuBGM] 2단계 매칭 성공: GO={selectedSource.gameObject.name.ToLower()}, Clip={selectedSource.clip.name}");
                    return selectedSource;
                }

                AudioSource FindActiveSource(Func<AudioSource, bool> predicate)
                {
                    foreach (AudioSource candidate in sources)
                    {
                        if (candidate == null || candidate.gameObject == null || !candidate.gameObject.activeInHierarchy) continue;
                        // "BGM_OneShot"은 이름에 bgm이 들어 있어 아래 부분 일치에 걸립니다(6.7.0 배틀 사고와 같은 함정).
                        if (GameBgmSource.IsOneShotSource(candidate)) continue;
                        if (predicate(candidate)) return candidate;
                    }

                    return null;
                }
            }
            catch (Exception ex)
            {
                ModLogger.Error($"[MenuBGM] FindMenuAudioSource 예외: {ex}");
            }
            return null;
        }

        private static bool IsLikelySoundtrackSource(AudioSource source)
        {
            if (source.clip == null || !source.isPlaying) return false;

            string clipName = source.clip.name.ToLower();
            string goName = source.gameObject.name.ToLower();
            return clipName.Contains("demo") || clipName.Contains("music") || clipName.Contains("bgm") || goName.Contains("music") || goName.Contains("bgm");
        }

        private static bool IsPlayingNonEffectSource(AudioSource source)
        {
            if (source.clip == null || !source.isPlaying) return false;

            string clipName = source.clip.name.ToLower();
            return !clipName.Contains("click") && !clipName.Contains("sfx") && !clipName.Contains("button");
        }
    }
}
