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

                // [중요] 여기서 injectedClip을 Destroy하면 안 됩니다.
                // StopMedia는 승리 직후 OnShowVictory 동기 이벤트 안에서 호출되는데, 그 이벤트는
                // 게임의 GameMusic.BattleEnd() 실행 도중 동기로 디스패치됩니다. 게임의 BGM AudioSource는
                // 아직 이 클립을 참조 중이라, 여기서 파괴하면 BattleEnd가 복귀해 파괴된 클립을 건드려
                // NullReferenceException으로 죽고(매 프레임 반복), 결과 화면으로 절대 넘어가지 못합니다.
                // (v0.7.6에서 이 Destroy가 추가되며 결과창 회귀가 발생함.)
                // 메모리 누수는 이미 안전한 시점에 처리됩니다:
                //   1) 다음 배틀 주입 시 previousInjected 클립을 파괴 (HwaBattleMediaController.cs)
                //   2) ResetState()의 방어적 파괴
                // 따라서 여기서는 우리 참조만 남겨두고 파괴하지 않습니다.

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
