using MelonLoader;
using UnityEngine;
using UnityEngine.Video;
using System;

namespace muse_dash_test
{
    /// <summary>
    /// 배틀 진행 중인 경우, BGM 오디오와 BGA 비디오의 재생 시점을 분석하여 실시간으로 동기화(Sync) 오차를 보정하는 컨트롤러입니다.
    /// </summary>
    public static class HwaSyncManager
    {
        private static float syncCooldownTimer = 0f;
        private static AudioSource cachedBgmSource = null;
        private static VideoPlayer cachedBgaPlayer = null;
        private static bool isCacheInitialized = false;

        public static void HandleBattleSynchronization()
        {
            try
            {
                var pnl = Il2CppAssets.Scripts.UI.Panels.PnlBattle.instance;
                if (pnl != null && pnl.CurrentBattleUIComp != null)
                {
                    var sld = pnl.CurrentBattleUIComp.sldProgress;
                    if (sld != null && sld.gameObject.activeInHierarchy)
                    {
                        if (ExperimentPlayContext.ShouldApplyExperimentChart)
                        {
                            if (syncCooldownTimer > 0f)
                            {
                                syncCooldownTimer -= Time.deltaTime;
                            }

                            if (!isCacheInitialized)
                            {
                                GameObject bgmGo = GameObject.Find("HwaBattleBgmSource");
                                if (bgmGo != null)
                                {
                                    cachedBgmSource = bgmGo.GetComponent<AudioSource>();
                                }

                                Camera mainCam = Camera.main;
                                if (mainCam != null)
                                {
                                    Transform quad = mainCam.transform.Find("VideoBackgroundQuad");
                                    if (quad != null)
                                    {
                                        cachedBgaPlayer = quad.GetComponent<VideoPlayer>();
                                    }
                                }
                                isCacheInitialized = true;
                            }

                            AudioSource bgmSource = cachedBgmSource;
                            VideoPlayer bgaPlayer = cachedBgaPlayer;

                            if (syncCooldownTimer <= 0f && Time.timeScale > 0f)
                            {
                                float progressRatio = sld.value;

                                if (bgmSource != null && bgmSource.clip != null && bgmSource.isPlaying)
                                {
                                    float totalDuration = bgmSource.clip.length;
                                    float expectedTime = progressRatio * totalDuration;
                                    float currentAudioTime = bgmSource.time;

                                    if (Mathf.Abs(currentAudioTime - expectedTime) > 0.2f)
                                    {
                                        MelonLogger.Msg($"[Sync.BGM] 싱크 보정 적용! 오차: {currentAudioTime - expectedTime:F3}초 | 기존: {currentAudioTime:F2}초 -> 목표: {expectedTime:F2}초");
                                        bgmSource.time = expectedTime;
                                        syncCooldownTimer = 0.5f; // 0.5초 쿨다운을 두어 동기화 루프 충돌 방지
                                    }
                                }

                                if (bgaPlayer != null && bgaPlayer.isPrepared && bgaPlayer.isPlaying)
                                {
                                    float totalDuration = (float)bgaPlayer.length;
                                    float expectedTime = progressRatio * totalDuration;
                                    float currentVideoTime = (float)bgaPlayer.time;

                                    if (Mathf.Abs(currentVideoTime - expectedTime) > 0.2f)
                                    {
                                        MelonLogger.Msg($"[Sync.BGA] 싱크 보정 적용! 오차: {currentVideoTime - expectedTime:F3}초 | 기존: {currentVideoTime:F2}초 -> 목표: {expectedTime:F2}초");
                                        bgaPlayer.time = expectedTime;
                                        syncCooldownTimer = 0.5f; // 0.5초 쿨다운을 두어 비디오 버퍼 정비 보장
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    syncCooldownTimer = 0f; // 배틀 중이 아닐 때는 타이머 초기화
                    cachedBgmSource = null;
                    cachedBgaPlayer = null;
                    isCacheInitialized = false;
                }
            }
            catch (Exception)
            {
                // 동기화 보정 예외 무시
            }
        }

        public static void ResetCooldown()
        {
            syncCooldownTimer = 0f;
        }
    }
}
