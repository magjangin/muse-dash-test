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
                MelonLogger.Msg($"[HwaBattleMediaController] ResumeMedia 호출됨 (isExit={isExit}) - 비디오 및 BGM을 재개합니다.");
                if (isExit) return;

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

                GameObject bgmGo = GameObject.Find("HwaBattleBgmSource");
                if (bgmGo != null)
                {
                    AudioSource bgm = bgmGo.GetComponent<AudioSource>();
                    if (bgm != null)
                    {
                        bgm.Stop();
                        MelonLogger.Msg("[HwaBattleMediaController] 커스텀 BGM 오디오 재생을 완전히 멈췄습니다.");
                    }
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[HwaBattleMediaController] StopMedia 중 오류: {ex}");
            }
        }
    }
}
