using MelonLoader;
using UnityEngine;
using UnityEngine.Video;
using System;

namespace muse_dash_test
{
    /// <summary>
    /// 배틀 진행 중인 경우, 마스터 오디오 시계(BGM AudioSource 또는 StageBattleComponent.timeFromMusicStart)를 기준으로
    /// BGA 비디오(VideoPlayer)의 재생 시작과 드리프트를 맞추는 컨트롤러입니다(재생 속도 미세 조절, 큰 오차만 시크).
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
        private const float VideoSeekCooldown = 2.0f;
        private const float BgaSearchInterval = 1.0f;
        private static VideoPlayer cachedBgaPlayer = null;
        private static float nextBgaSearchTime = 0f;
        private static bool loggedSyncError = false;

        public static void ResetCache()
        {
            syncCooldownTimer = 0f;
            syncCheckTimer = 0f;
            cachedBgaPlayer = null;
            nextBgaSearchTime = 0f;
            loggedSyncError = false;
        }

        /// <summary>
        /// 일시정지 해제(ResumeMedia) 직후 비디오/오디오 재개 레이턴시 차이로 인한 싱크 급변동을 방지하기 위해
        /// 짧은 동기화 유예 쿨다운(0.5초)을 부여하고 드리프트 통계를 재설정합니다.
        /// </summary>
        public static void NotifyResume()
        {
            syncCooldownTimer = Math.Max(syncCooldownTimer, 0.5f);
            ResetDriftStats();
        }

        public static void HandleBattleSynchronization()
        {
            try
            {
                if (syncCooldownTimer > 0f)
                {
                    syncCooldownTimer = Math.Max(0f, syncCooldownTimer - Time.deltaTime);
                }

                // 커스텀 곡이 아니면 IL2CPP 조회(PnlBattle.instance 등)를 하나도 하지 않고 바로 나갑니다. 매 프레임 불리는데
                // 예전에는 이 검사가 패널 조회 뒤에 있어 공식 곡 배틀과 메뉴에서도 프레임마다 래퍼를 만들었습니다.
                // 캐시는 커스텀 곡에서만 쓰이고, 배틀을 불러올 때마다 HwaBattleMediaController.ResetState가 비웁니다.
                if (!CustomPlaySession.Current.ShouldApplyExperimentChart) return;

                var pnl = Il2CppAssets.Scripts.UI.Panels.PnlBattle.instance;
                if (pnl == null || pnl.CurrentBattleUIComp == null)
                {
                    // 배틀 밖입니다. 다음 배틀에서 BGA 플레이어를 새로 찾도록 캐시를 버립니다.
                    ResetCache();
                    return;
                }

                var sld = pnl.CurrentBattleUIComp.sldProgress;
                if (sld == null || !sld.gameObject.activeInHierarchy) return;

                // 진행바는 매 프레임 갱신합니다. 게임 트윈은 호스트 곡 길이라 짧은 호스트에서는 중간에 멈추는데,
                // 그 뒤를 0.1초 간격으로만 갱신하면 바가 계단처럼 움직였습니다.
                ProgressBarPatch.UpdateProgress(sld);

                // BGA 시작은 매 프레임 확인합니다(음악 시작 순간을 놓치지 않게). 캐시된 참조라 BGA 없는 곡에서도 값쌉니다.
                VideoPlayer bgaPlayer = GetOrFindBgaPlayer();
                if (bgaPlayer != null && !HwaBattleMediaController.videoStartedAtMusic)
                {
                    TryStartVideoAtMusicTime(bgaPlayer);
                }

                syncCheckTimer += Time.deltaTime;
                if (syncCheckTimer < SyncCheckInterval)
                {
                    return;
                }
                syncCheckTimer = 0f;

                if (Time.timeScale <= 0f) return;
                if (bgaPlayer == null || !bgaPlayer.isPrepared || !bgaPlayer.isPlaying) return;

                CorrectVideoDrift(bgaPlayer);
            }
            catch (Exception ex)
            {
                if (!loggedSyncError)
                {
                    loggedSyncError = true;
                    ModLogger.Warning($"[Sync.BGA] 동기화 보정 예외 (이후 생략): {ex.GetType().Name}: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// 메인 카메라 밑의 BGA 쿼드에서 <see cref="VideoPlayer"/>를 찾아 캐싱합니다. BGA가 없는 곡이면 null입니다.
        /// 한 번 찾은 뒤에는 플레이어가 파괴됐을 때만 다시 찾습니다.
        ///
        /// <para>못 찾은 동안에는 <see cref="BgaSearchInterval"/>마다 한 번만 다시 찾습니다. 쿼드는 비디오 준비가 끝난 뒤에
        /// 늦게 생길 수 있어 재탐색 자체는 필요하지만, 예전에는 BGA가 없는 곡에서 곡 내내 0.1초마다
        /// <c>Camera.main</c>(태그 검색)과 <c>Transform.Find</c>를 되풀이했습니다.</para>
        /// </summary>
        private static VideoPlayer GetOrFindBgaPlayer()
        {
            if (cachedBgaPlayer != null) return cachedBgaPlayer;

            float now = Time.unscaledTime;
            if (now < nextBgaSearchTime) return null;
            nextBgaSearchTime = now + BgaSearchInterval;

            Camera mainCam = Camera.main;
            if (mainCam != null)
            {
                Transform quad = mainCam.transform.Find("VideoBackgroundQuad");
                if (quad != null)
                {
                    cachedBgaPlayer = quad.GetComponent<VideoPlayer>();
                }
            }
            return cachedBgaPlayer;
        }

        /// <summary>
        /// 게임이 음악을 시작했고(<c>FixedOffset</c>) BGM이 실제로 돌고 있으며 비디오 준비가 끝났으면, BGM 위치로 맞춰 재생을 시작합니다.
        /// 비디오는 곡 오디오와 같은 타임라인으로 만든 것이므로(il vento d'oro: 영상 301.48초, 음원 301.49초) 기준은 들리는 소리인 <c>bgm.time</c>입니다.
        /// </summary>
        private static void TryStartVideoAtMusicTime(VideoPlayer bgaPlayer)
        {
            if (!HwaBattleMediaController.gameMusicStarted || !bgaPlayer.isPrepared) return;
            if (Time.timeScale <= 0f) return; // 일시정지 중에는 시작하지 않습니다
            if (!TryGetMasterPlaybackTime(out float masterTime)) return;

            HwaBattleMediaController.videoStartedAtMusic = true;
            float videoDuration = (float)bgaPlayer.length;
            float startTime = videoDuration > 0.5f && bgaPlayer.isLooping ? masterTime % videoDuration : masterTime;
            if (startTime > 0.001f) bgaPlayer.time = startTime;
            bgaPlayer.playbackSpeed = 1f;
            bgaPlayer.Play();
            // 시작 시크가 끝날 때까지 video.time은 0을 돌려줍니다. 바로 보정하면 같은 위치로 한 번 더 시크했습니다
            // (2026-11-21 실기: 시작 35ms 뒤 "오차 -1.285초" 시크). 시크 대기 시간만큼 보정을 쉽니다.
            syncCooldownTimer = VideoSeekCooldown;
            ResetDriftStats();
            ModLogger.Msg($"[Sync.BGA] 음악 시작에 맞춰 BGA 재생 시작: bgm.time={masterTime:F3}s, video.time→{startTime:F3}s");
        }

        // 작은 오차는 재생 속도를 살짝 바꿔 따라붙고(시크는 키프레임 탐색이라 멈칫함), 이보다 크면 시크합니다.
        private const float VideoSeekThreshold = 0.5f;
        private const float VideoSpeedDeadband = 0.015f; // 15ms 이내 오차는 안정 유지
        private const float VideoSpeedGain = 1.2f;      // 오차 0.05초(50ms) → 속도 ±6% 조절로 보다 빠르게 밀착 추종
        private const float VideoMaxSpeedDelta = 0.15f;
        private const float DriftReportInterval = 30f;

        private static float driftReportTimer;
        private static float driftMaxAbs;
        private static float driftSumAbs;
        private static int driftSamples;
        private static int driftSeeks;

        private static void ResetDriftStats()
        {
            driftReportTimer = 0f;
            driftMaxAbs = 0f;
            driftSumAbs = 0f;
            driftSamples = 0;
            driftSeeks = 0;
        }

        /// <summary>
        /// BGA를 BGM 위치(<c>bgm.time</c>)에 붙입니다. 예전에는 0.35초가 넘을 때만 시크해서 그 안의 오차(반 박자 남짓)가 그대로 남았습니다.
        /// 이제 오차가 <see cref="VideoSpeedDeadband"/>를 넘으면 <c>playbackSpeed</c>를 오차에 비례해 최대 ±15% 바꿔 부드럽게 따라붙고,
        /// <see cref="VideoSeekThreshold"/>를 넘을 때만 시크합니다(시크 뒤 <see cref="VideoSeekCooldown"/> 동안 대기).
        /// </summary>
        private static void CorrectVideoDrift(VideoPlayer bgaPlayer)
        {
            float videoDuration = (float)bgaPlayer.length;
            if (videoDuration <= 0.5f || !TryGetMasterPlaybackTime(out float masterTime)) return;

            float expectedVideoTime = bgaPlayer.isLooping
                ? (masterTime % videoDuration)
                : Mathf.Min(masterTime, videoDuration);
            float currentVideoTime = (float)bgaPlayer.time;
            float diff = currentVideoTime - expectedVideoTime; // + : 영상이 앞섬

            // 루프 경계를 넘나드는 순간의 위상차는 반대쪽으로 접습니다.
            if (bgaPlayer.isLooping)
            {
                if (diff > videoDuration * 0.5f) diff -= videoDuration;
                else if (diff < -videoDuration * 0.5f) diff += videoDuration;
            }

            // 시크 직후(쿨다운 중)에는 video.time이 아직 옛 값이라 통계에도 넣지 않습니다.
            if (syncCooldownTimer > 0f) return;

            float absDiff = Mathf.Abs(diff);
            RecordDrift(absDiff);

            if (absDiff > VideoSeekThreshold)
            {
                ModLogger.Msg($"[Sync.BGA] 오차가 커서 시크: 오차={diff:+0.000;-0.000}초 | 현재={currentVideoTime:F2}초 -> BGM={expectedVideoTime:F2}초");
                bgaPlayer.playbackSpeed = 1f;
                bgaPlayer.time = expectedVideoTime;
                syncCooldownTimer = VideoSeekCooldown;
                driftSeeks++;
                return;
            }

            float targetSpeed = absDiff <= VideoSpeedDeadband
                ? 1f
                : 1f - Mathf.Clamp(diff * VideoSpeedGain, -VideoMaxSpeedDelta, VideoMaxSpeedDelta);
            if (Mathf.Abs(bgaPlayer.playbackSpeed - targetSpeed) > 0.005f)
            {
                bgaPlayer.playbackSpeed = targetSpeed;
            }
        }

        private static void RecordDrift(float absDiff)
        {
            driftSamples++;
            driftSumAbs += absDiff;
            if (absDiff > driftMaxAbs) driftMaxAbs = absDiff;
            driftReportTimer += SyncCheckInterval;
            if (driftReportTimer < DriftReportInterval) return;

            // 곡마다 여러 번 나오는 반복 줄이라 Verbose입니다(로그 레벨 기준: 곡당 요약 한 줄만 Info). 싱크를 의심하면
            // MelonPreferences LogLevel을 Verbose로 올려 이 줄을 보십시오. 이상 징후인 시크 줄은 Info로 둡니다.
            ModLogger.Verbose($"[Sync.BGA] 최근 {driftReportTimer:F0}초 BGA-BGM 오차: 평균 {driftSumAbs / driftSamples * 1000f:F0}ms, 최대 {driftMaxAbs * 1000f:F0}ms, 시크 {driftSeeks}회");
            driftReportTimer = 0f;
            driftMaxAbs = 0f;
            driftSumAbs = 0f;
            driftSamples = 0;
            driftSeeks = 0;
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
