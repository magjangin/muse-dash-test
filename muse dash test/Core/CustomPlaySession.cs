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
        public bool IsDynamicBossSwap { get; set; }

        public int TotalStandard { get; set; }
        public int TotalGears { get; set; }
        public int TotalHearts { get; set; }
        public int TotalBlueNotes { get; set; }

        public string LastKnownMusicUid =>
            !string.IsNullOrEmpty(SelectedMusicUid) ? SelectedMusicUid : LastClickedMusicUid;

        public void RememberMusicSelection(string uid)
        {
            SelectedMusicUid = uid ?? string.Empty;
            ShouldApplyExperimentChart = CustomContentIds.IsVirtualSong(uid);
        }

        public void ResetCounts()
        {
            TotalStandard = 0;
            TotalGears = 0;
            TotalHearts = 0;
            TotalBlueNotes = 0;
        }
    }
}
