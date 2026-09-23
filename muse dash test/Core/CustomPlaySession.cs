using System;

namespace muse_dash_test
{
    /// <summary>현재 커스텀 곡 선택과 플레이에서 공유되는 런타임 상태입니다.</summary>
    public sealed class CustomPlaySession
    {
        public static CustomPlaySession Current { get; } = new CustomPlaySession();

        private CustomPlaySession() { }

        public string SelectedMusicUid { get; set; } = string.Empty;
        public string LastClickedMusicUid { get; set; } = string.Empty;
        public bool IsExperimentModeActive { get; set; }
        public bool ShouldApplyExperimentChart { get; private set; }
        public string LastApplyDecisionReasonCode { get; private set; } = string.Empty;
        public string LastApplyDecisionDescription { get; private set; } = string.Empty;
        public bool IsDynamicBossSwap { get; set; }

        public int TotalStandard { get; set; }
        public int TotalGears { get; set; }
        public int TotalHearts { get; set; }
        public int TotalBlueNotes { get; set; }

        /// <summary>
        /// 지금 다루는 곡의 UID입니다. "현재 곡"이 필요한 곳은 전부 이 한 곳을 거칩니다.
        ///   1) <see cref="SelectedMusicUid"/> (곡 선택·준비 화면에서 곡이 바뀔 때마다 갱신)
        ///   2) 곡 선택 화면(PnlStage)이 들고 있는 곡 — 1이 비어 있을 때만 씬을 뒤집니다
        ///   3) <see cref="LastClickedMusicUid"/>
        /// 셋 다 없으면 빈 문자열입니다.
        /// <para>예전에는 이 순서를 10여 곳이 각자 적어 두었고, 이 프로퍼티만 2단계를 빼먹고 있었습니다.</para>
        /// </summary>
        public string LastKnownMusicUid
        {
            get
            {
                if (!string.IsNullOrEmpty(SelectedMusicUid)) return SelectedMusicUid;

                string onStage = PnlStagePatchHelper.FindSelectedMusicUidOnStage();
                if (!string.IsNullOrEmpty(onStage)) return onStage;

                return LastClickedMusicUid;
            }
        }

        public string DescribeApplyDecision()
        {
            return $"apply={ShouldApplyExperimentChart}, reason={LastApplyDecisionReasonCode}, detail={LastApplyDecisionDescription}, isExperimentModeActive={IsExperimentModeActive}, selectedUid={SelectedMusicUid}, lastClickedUid={LastClickedMusicUid}";
        }

        public void RememberMusicSelection(string uid)
        {
            string prevUid = SelectedMusicUid;
            bool prevShouldApply = ShouldApplyExperimentChart;
            SelectedMusicUid = uid ?? string.Empty;
            bool isExperimentMode = IsExperimentModeActive;
            var decision = HwaResourceManager.DecideCustomChartForSelection(uid, IsExperimentModeActive);
            ShouldApplyExperimentChart = decision.ShouldApply;
            LastApplyDecisionReasonCode = decision.ReasonCode;
            LastApplyDecisionDescription = decision.Description;
            ModConfig.VerboseLog($"[CustomPlaySession.Debug] RememberMusicSelection 호출: prevUid={prevUid}, newUid={uid ?? "(null)"}, experimentMode={isExperimentMode}, isVirtualSong={decision.IsVirtualSong}, isRegisteredHost={decision.IsRegisteredHost}, prevShouldApply={prevShouldApply}, newShouldApply={decision.ShouldApply}, reason={decision.ReasonCode}, detail={decision.Description}");

            try
            {
                DiscordPresenceManager.UpdateForSelection(uid);

                // 원본 게임의 DiscordManager에도 즉시 통보하여 곡 목록/선택 패널에서 곡을 넘길 때마다 디스코드에 바로 갱신
                var discordManager = Il2CppPeroTools2.Commons.Singleton<Il2Cpp.DiscordManager>.instance;
                if (discordManager != null)
                {
                    DiscordPresenceManager.ResolveSongDetails(uid, out string title, out string artist);
                    string info = $"{title} - {artist}";
                    discordManager.SetUpdateActivity(true, info);
                }
            }
            catch (Exception ex)
            {
                ModLogger.Error($"[CustomPlaySession] Discord Presence 갱신 에러: {ex.Message}");
            }
        }

        public void ResetCounts()
        {
            TotalStandard = 0;
            TotalGears = 0;
            TotalHearts = 0;
            TotalBlueNotes = 0;
        }

        /// <summary>
        /// 배틀 종료/이탈 시 실험 차트 적용 결정을 초기화합니다.
        /// RememberMusicSelection은 곡 선택 시점에만 호출되므로, 이 메서드가 없으면
        /// 도중에 나가도 ShouldApplyExperimentChart가 직전 값(true)으로 남아 다음 곡
        /// 선택 전까지 stale 상태가 됩니다.
        /// </summary>
        public void ResetApplyDecision()
        {
            bool prevShouldApply = ShouldApplyExperimentChart;
            ShouldApplyExperimentChart = false;
            LastApplyDecisionReasonCode = "BATTLE_EXIT_RESET";
            LastApplyDecisionDescription = "배틀 종료/이탈로 실험 차트 적용 결정 초기화";
            ModConfig.VerboseLog($"[CustomPlaySession.Debug] ResetApplyDecision 호출: prevShouldApply={prevShouldApply}, newShouldApply=false");
        }
    }
}
