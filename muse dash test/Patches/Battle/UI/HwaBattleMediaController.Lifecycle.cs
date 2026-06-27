using MelonLoader;
using System;
using UnityEngine;
using UnityEngine.Video;

namespace muse_dash_test
{
    /// <summary>
    /// HwaBattleMediaController의 재생 라이프사이클 제어부입니다.
    /// 일시정지/재개(일시정지 화면 진입·복귀), 정지(배틀 종료)에 맞춰
    /// 커스텀 BGM(HwaBattleBgmSource)과 배경 비디오(VideoPlayer)의 상태를 일괄 조작합니다.
    /// 미디어 로딩/주입 로직은 HwaBattleMediaController.cs를 참고하세요.
    /// </summary>
    public static partial class HwaBattleMediaController
    {
        // ==========================================
        // 라이프사이클 미디어 상태 일괄 조작 인터페이스
        // ==========================================

        public static void PauseMedia()
        {
            try
            {
                MelonLogger.Msg("[HwaBattleMediaController] PauseMedia 호출됨 - 비디오 및 BGM을 일시정지합니다.");
                Camera mainCam = Camera.main;
                if (mainCam != null)
                {
                    VideoPlayer vp = mainCam.GetComponentInChildren<VideoPlayer>();
                    if (vp != null && vp.isPlaying)
                    {
                        vp.Pause();
                        MelonLogger.Msg("[HwaBattleMediaController] 배경 비디오 재생을 일시정지했습니다.");
                    }
                }

                GameObject bgmGo = GameObject.Find("HwaBattleBgmSource");
                if (bgmGo != null)
                {
                    AudioSource bgm = bgmGo.GetComponent<AudioSource>();
                    if (bgm != null && bgm.isPlaying)
                    {
                        bgm.Pause();
                        MelonLogger.Msg("[HwaBattleMediaController] 커스텀 BGM 오디오 재생을 일시정지했습니다.");
                    }
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[HwaBattleMediaController] PauseMedia 중 오류: {ex}");
            }
        }

        public static void ResumeMedia(bool isExit)
        {
            try
            {
                MelonLogger.Msg($"[HwaBattleMediaController] ResumeMedia 호출됨 (isExit={isExit}) - 비디오 및 BGM을 재개/정지합니다.");
                if (isExit)
                {
                    StopMedia();
                    return;
                }

                Camera mainCam = Camera.main;
                if (mainCam != null)
                {
                    VideoPlayer vp = mainCam.GetComponentInChildren<VideoPlayer>();
                    if (vp != null)
                    {
                        bool wasPlaying = vp.isPlaying;
                        bool wasPrepared = vp.isPrepared;
                        vp.Play();
                        MelonLogger.Msg($"[HwaBattleMediaController] 배경 비디오 재생을 재개했습니다. (이전 상태: isPlaying={wasPlaying}, isPrepared={wasPrepared})");
                    }
                }

                GameObject bgmGo = GameObject.Find("HwaBattleBgmSource");
                if (bgmGo != null)
                {
                    AudioSource bgm = bgmGo.GetComponent<AudioSource>();
                    if (bgm != null && !bgm.isPlaying)
                    {
                        bgm.UnPause();
                        MelonLogger.Msg("[HwaBattleMediaController] 커스텀 BGM 오디오 재생을 재개했습니다.");
                    }
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[HwaBattleMediaController] ResumeMedia 중 오류: {ex}");
            }
        }

        public static void StopMedia()
        {
            try
            {
                MelonLogger.Msg("[HwaBattleMediaController] StopMedia 호출됨 - 비디오 및 BGM을 정지합니다.");
                
                Camera mainCam = Camera.main;
                if (mainCam != null)
                {
                    VideoPlayer vp = mainCam.GetComponentInChildren<VideoPlayer>();
                    if (vp != null)
                    {
                        vp.Stop();
                        MelonLogger.Msg("[HwaBattleMediaController] 배경 비디오 재생을 완전히 멈췄습니다.");
                    }
                }

                // 캐싱된 주입 BGM 소스 정지
                if (injectedAudioSource != null)
                {
                    try
                    {
                        if (injectedAudioSource.gameObject != null)
                        {
                            injectedAudioSource.Stop();
                            MelonLogger.Msg($"[HwaBattleMediaController] 캐싱된 주입 BGM 소스 정지 완료: go={injectedAudioSource.gameObject.name}, playing={injectedAudioSource.isPlaying}");
                        }
                    }
                    catch (Exception ex)
                    {
                        MelonLogger.Warning($"[HwaBattleMediaController] 캐싱된 주입 BGM 소스 정지 중 예외 발생: {ex.Message}");
                    }
                    injectedAudioSource = null;
                }

                // 주입했던 커스텀 클립 메모리 해제 (배틀 종료 시 ogg가 새지 않도록)
                if (injectedClip != null)
                {
                    UnityEngine.Object.Destroy(injectedClip);
                    injectedClip = null;
                    MelonLogger.Msg("[HwaBattleMediaController] 주입 커스텀 클립 해제 완료 (메모리 누수 방지)");
                }

                GameObject bgmGo = GameObject.Find("HwaBattleBgmSource");
                if (bgmGo != null)
                {
                    AudioSource bgm = bgmGo.GetComponent<AudioSource>();
                    if (bgm != null)
                    {
                        bgm.Stop();
                        MelonLogger.Msg("[HwaBattleMediaController] HwaBattleBgmSource 재생을 완전히 멈췄습니다.");
                    }
                }

                // 진단용: 씬에 남아있는 모든 AudioSource 로그 출력
                AudioSource[] allSources = UnityEngine.Object.FindObjectsOfType<AudioSource>();
                if (allSources != null && allSources.Length > 0)
                {
                    MelonLogger.Msg($"[HwaBattleMediaController.Debug] 현재 씬의 AudioSource 진단 (총 {allSources.Length}개):");
                    foreach (var src in allSources)
                    {
                        if (src == null || src.gameObject == null) continue;
                        string clipName = src.clip != null ? src.clip.name : "(null)";
                        MelonLogger.Msg($"  - GameObject='{src.gameObject.name}', clip='{clipName}', isPlaying={src.isPlaying}, loop={src.loop}, volume={src.volume}");
                    }
                }
                else
                {
                    MelonLogger.Msg("[HwaBattleMediaController.Debug] 현재 씬에 AudioSource가 존재하지 않습니다.");
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[HwaBattleMediaController] StopMedia 중 오류: {ex}");
            }
        }
    }
}
