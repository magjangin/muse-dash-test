using MelonLoader;
using UnityEngine;
using UnityEngine.Video;
using System;

namespace muse_dash_test
{
    /// <summary>
    /// 배틀 진행 중인 경우, 마스터 오디오 시계(BGM AudioSource 또는 StageBattleComponent.timeFromMusicStart)를 기준으로
    /// BGA 비디오(VideoPlayer)의 재생 시점 드리프트만 보정하는 컨트롤러입니다.
    ///
    /// [주의 - 노트 버벅거림/밀림 방지]
    /// 스트리밍 Ogg Vorbis(`streamAudio = true`)로 로드된 BGM AudioSource에 대해 `bgmSource.time = expectedTime`을
    /// 강제 시크(seek)하면 디코더 버퍼 재탐색으로 프레임 히치가 발생하며, 게임의 `GameMusic.GameMusicFixTimerUpdate`가
    /// 참조하는 `bgm.time`이 흔들려 노트가 뒤로 밀리는 현상이 유발됩니다.
    /// 따라서 BGM AudioSource는 절대 역으로 시크하지 않고 마스터 클럭으로만 읽습니다.
    /// </summary>
    public static class HwaSyncManager
    {
        private static float syncCooldownTimer = 0f;
        private static float syncCheckTimer = 0f;
        private const float SyncCheckInterval = 0.1f;
        private const float VideoDriftThreshold = 0.35f;
        private const float VideoSeekCooldown = 2.0f;
        private static VideoPlayer cachedBgaPlayer = null;
        private static bool isCacheInitialized = false;

        public static void ResetCache()
        {
            syncCooldownTimer = 0f;
            syncCheckTimer = 0f;
            cachedBgaPlayer = null;
            isCacheInitialized = false;
        }

        public static void HandleBattleSynchronization()
        {
            try
            {
                if (syncCooldownTimer > 0f)
                {
                    syncCooldownTimer = Math.Max(0f, syncCooldownTimer - Time.deltaTime);
                }

                syncCheckTimer += Time.deltaTime;
                if (syncCheckTimer < SyncCheckInterval)
                {
                    return;
                }
                syncCheckTimer = 0f;

                var pnl = Il2CppAssets.Scripts.UI.Panels.PnlBattle.instance;
                if (pnl != null && pnl.CurrentBattleUIComp != null)
                {
                    var sld = pnl.CurrentBattleUIComp.sldProgress;
                    if (sld != null && sld.gameObject.activeInHierarchy)
                    {
                        if (CustomPlaySession.Current.ShouldApplyExperimentChart)
                        {
                            // 호스트 곡보다 긴 커스텀 곡에서 DOTween 진행바가 먼저 멈추더라도
                            // 실제 커스텀 곡/채보 진행률로 슬라이더를 계속 갱신합니다.
                            ProgressBarPatch.UpdateProgress(sld);

                            if (!isCacheInitialized || cachedBgaPlayer == null)
                            {
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

                            VideoPlayer bgaPlayer = cachedBgaPlayer;
                            if (syncCooldownTimer <= 0f && Time.timeScale > 0f && bgaPlayer != null && bgaPlayer.isPrepared && bgaPlayer.isPlaying)
                            {
                                float videoDuration = (float)bgaPlayer.length;
                                if (videoDuration > 0.5f && TryGetMasterPlaybackTime(out float masterTime))
                                {
                                    float expectedVideoTime = bgaPlayer.isLooping
                                        ? (masterTime % videoDuration)
                                        : Mathf.Min(masterTime, videoDuration);
                                    float currentVideoTime = (float)bgaPlayer.time;
                                    float drift = Mathf.Abs(currentVideoTime - expectedVideoTime);

                                    // 루프 경계 근처에서는 위상차로 인해 오차가 크게 보일 수 있으므로 보정에서 제외합니다.
                                    if (bgaPlayer.isLooping && (videoDuration - drift) < VideoDriftThreshold)
                                    {
                                        drift = videoDuration - drift;
                                    }

                                    if (drift > VideoDriftThreshold)
                                    {
                                        ModLogger.Verbose($"[Sync.BGA] 비디오 싱크 보정 적용: 오차={currentVideoTime - expectedVideoTime:F3}초 | 현재={currentVideoTime:F2}초 -> 마스터={expectedVideoTime:F2}초");
                                        bgaPlayer.time = expectedVideoTime;
                                        syncCooldownTimer = VideoSeekCooldown;
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    ResetCache();
                }
            }
            catch (Exception)
            {
                // 동기화 보정 예외 무시
            }
        }

        private static bool TryGetMasterPlaybackTime(out float masterTime)
        {
            masterTime = 0f;

            AudioSource bgmSource = HwaBattleMediaController.ActiveAudioSource;
            if (bgmSource != null && bgmSource.clip != null && bgmSource.isPlaying)
            {
                masterTime = bgmSource.time;
                return masterTime >= 0f;
            }

            var stage = Il2CppFormulaBase.StageBattleComponent.instance;
            if (stage != null && stage.isInGame)
            {
                masterTime = stage.timeFromMusicStart;
                return masterTime >= 0f;
            }

            return false;
        }
    }
}
