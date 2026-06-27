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

        public string LastKnownMusicUid =>
            !string.IsNullOrEmpty(SelectedMusicUid) ? SelectedMusicUid : LastClickedMusicUid;

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
            MelonLoader.MelonLogger.Msg($"[CustomPlaySession.Debug] RememberMusicSelection 호출: prevUid={prevUid}, newUid={uid ?? "(null)"}, experimentMode={isExperimentMode}, isVirtualSong={decision.IsVirtualSong}, isRegisteredHost={decision.IsRegisteredHost}, prevShouldApply={prevShouldApply}, newShouldApply={decision.ShouldApply}, reason={decision.ReasonCode}, detail={decision.Description}");
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
            MelonLoader.MelonLogger.Msg($"[CustomPlaySession.Debug] ResetApplyDecision 호출: prevShouldApply={prevShouldApply}, newShouldApply=false");
        }
    }
}
