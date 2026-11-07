using MelonLoader;
using System;
using UnityEngine;
using UnityEngine.Video;

namespace muse_dash_test
{
    /// <summary>
    /// HwaBattleMediaController의 재생 라이프사이클 제어부입니다.
    /// 일시정지/재개(일시정지 화면 진입·복귀), 정지(배틀 종료)에 맞춰
    /// 커스텀 BGM(injectedAudioSource / HwaBattleBgmSource)과 배경 비디오(VideoPlayer)의 상태를 일괄 조작합니다.
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
                ModLogger.Msg("[HwaBattleMediaController] PauseMedia 호출됨 - 비디오 및 BGM을 일시정지합니다.");
                Camera mainCam = Camera.main;
                if (mainCam != null)
                {
                    VideoPlayer vp = mainCam.GetComponentInChildren<VideoPlayer>();
                    if (vp != null && vp.isPlaying)
                    {
                        vp.Pause();
                        ModLogger.Msg("[HwaBattleMediaController] 배경 비디오 재생을 일시정지했습니다.");
                    }
                }

                if (injectedAudioSource != null && injectedAudioSource.isPlaying)
                {
                    injectedAudioSource.Pause();
                    ModLogger.Msg("[HwaBattleMediaController] 주입된 BGM 오디오 재생을 일시정지했습니다.");
                }

                GameObject bgmGo = GameObject.Find("HwaBattleBgmSource");
                if (bgmGo != null)
                {
                    AudioSource bgm = bgmGo.GetComponent<AudioSource>();
                    if (bgm != null && bgm.isPlaying)
                    {
                        bgm.Pause();
                        ModLogger.Msg("[HwaBattleMediaController] 커스텀 BGM 오디오 재생을 일시정지했습니다.");
                    }
                }
            }
            catch (Exception ex)
            {
                ModLogger.Error($"[HwaBattleMediaController] PauseMedia 중 오류: {ex}");
            }
        }

        public static void ResumeMedia(bool isExit)
        {
            try
            {
                ModLogger.Msg($"[HwaBattleMediaController] ResumeMedia 호출됨 (isExit={isExit}) - 비디오 및 BGM을 재개/정지합니다.");
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
                        ModLogger.Msg($"[HwaBattleMediaController] 배경 비디오 재생을 재개했습니다. (이전 상태: isPlaying={wasPlaying}, isPrepared={wasPrepared})");
                    }
                }

                if (injectedAudioSource != null && !injectedAudioSource.isPlaying)
                {
                    injectedAudioSource.UnPause();
                    ModLogger.Msg("[HwaBattleMediaController] 주입된 BGM 오디오 재생을 재개했습니다.");
                }

                GameObject bgmGo = GameObject.Find("HwaBattleBgmSource");
                if (bgmGo != null)
                {
                    AudioSource bgm = bgmGo.GetComponent<AudioSource>();
                    if (bgm != null && !bgm.isPlaying)
                    {
                        bgm.UnPause();
                        ModLogger.Msg("[HwaBattleMediaController] 커스텀 BGM 오디오 재생을 재개했습니다.");
                    }
                }
            }
            catch (Exception ex)
            {
                ModLogger.Error($"[HwaBattleMediaController] ResumeMedia 중 오류: {ex}");
            }
        }

        public static void StopMedia()
        {
            // 진행 중인 로드가 끝나도 재생하지 않도록 세대를 올립니다(HwaBattleMediaController.mediaGeneration 참고).
            mediaGeneration++;
            ProgressBarPatch.Reset();
            HwaSyncManager.ResetCache();
            try
            {
                long heapMem = GC.GetTotalMemory(false);
                ModLogger.Msg($"[HwaBattleMediaController.Memory] StopMedia 호출됨 - 비디오 및 BGM 정지 시작 (ManagedHeap={heapMem / 1048576f:F2}MB)");
                
                Camera mainCam = Camera.main;
                if (mainCam != null)
                {
                    VideoPlayer vp = mainCam.GetComponentInChildren<VideoPlayer>();
                    if (vp != null)
                    {
                        vp.Stop();
                        ModLogger.Msg("[HwaBattleMediaController] 배경 비디오 재생을 완전히 멈췄습니다.");
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
                            ModLogger.Msg($"[HwaBattleMediaController] 캐싱된 주입 BGM 소스 정지 완료: go={injectedAudioSource.gameObject.name}, playing={injectedAudioSource.isPlaying}");
                        }
                    }
                    catch (Exception ex)
                    {
                        ModLogger.Warning($"[HwaBattleMediaController] 캐싱된 주입 BGM 소스 정지 중 예외 발생: {ex.Message}");
                    }
                    injectedAudioSource = null;
                }

                // [중요] 여기서 injectedClip을 즉시 Destroy하면 안 됩니다.
                // StopMedia는 승리 직후 OnShowVictory 동기 이벤트 안에서 호출되는데, 그 이벤트는
                // 게임의 GameMusic.BattleEnd() 실행 도중 동기로 디스패치됩니다. 게임의 BGM AudioSource는
                // 아직 이 클립을 참조 중이라, 여기서 파괴하면 BattleEnd가 복귀해 파괴된 클립을 건드려
                // NullReferenceException으로 죽고(매 프레임 반복), 결과 화면으로 절대 넘어가지 못합니다.
                // 또한 ResetState() 시점에도 아직 새 클립이 로드되기 전이라 AudioManager.bgm.clip이
                // 이전 클립을 가리키고 있으므로 그 자리에서 바로 파괴하면 재도전이 멈춥니다(b14b0a5).
                //
                // 대신 ResetState()에서는 클립을 retiredClips 목록으로만 옮기고, 다음 배틀 클립이나
                // 메뉴 BGM이 AudioManager.bgm.clip을 실제로 교체한 직후에만 DestroyUnreferencedRetiredClips로
                // 참조가 완전히 끊긴 은퇴 클립을 파괴하여 장시간(3시간+) 플레이 시 FMOD/오디오 스트림 누수를 막습니다.

                GameObject bgmGo = GameObject.Find("HwaBattleBgmSource");
                if (bgmGo != null)
                {
                    AudioSource bgm = bgmGo.GetComponent<AudioSource>();
                    if (bgm != null)
                    {
                        bgm.Stop();
                        ModLogger.Msg("[HwaBattleMediaController] HwaBattleBgmSource 재생을 완전히 멈췄습니다.");
                    }
                }

                // 진단용: Verbose가 켜져 있을 때만 씬의 AudioSource 전체를 스캔합니다.
                if (ModLogger.IsLevelEnabled(ModLogLevel.Verbose))
                {
                    AudioSource[] allSources = UnityEngine.Object.FindObjectsOfType<AudioSource>();
                    if (allSources != null && allSources.Length > 0)
                    {
                        ModLogger.Verbose($"[HwaBattleMediaController.Debug] 현재 씬의 AudioSource 진단 (총 {allSources.Length}개):");
                        foreach (var src in allSources)
                        {
                            if (src == null || src.gameObject == null) continue;
                            string clipName = src.clip != null ? src.clip.name : "(null)";
                            ModLogger.Verbose($"  - GameObject='{src.gameObject.name}', clip='{clipName}', isPlaying={src.isPlaying}, loop={src.loop}, volume={src.volume}");
                        }
                    }
                    else
                    {
                        ModLogger.Verbose("[HwaBattleMediaController.Debug] 현재 씬에 AudioSource가 존재하지 않습니다.");
                    }
                }
            }
            catch (Exception ex)
            {
                ModLogger.Error($"[HwaBattleMediaController] StopMedia 중 오류: {ex}");
            }
        }
    }
}
