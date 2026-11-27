namespace muse_dash_test
{
    /// <summary>
    /// <c>MusicTagManager.RefreshStageDisplayMusics(int tagIndex)</c>의 원본을 막을지 정하는 규칙입니다.
    /// 게임에 의존하지 않아 로직 테스트로 검증합니다. 집행은 <c>MusicTagManager_RefreshStageDisplayMusics_Patch</c>.
    ///
    /// <para><b>왜</b>: 원본은 커스텀 태그(<see cref="CustomContentIds.TagIndex"/> = 1999)에서 NRE를 내므로 그 번호는 막아야 합니다.
    /// <c>-1</c>은 "지금 고른 태그"라는 뜻이라 원본이 <c>m_DbMusicTag.m_CurSelectedTagIndex</c>로 바꿔 씁니다(ISIL).
    /// 예전 패치는 <c>-1</c>을 통째로 막아, 게임이 곡 선택 화면에 들어가거나 '전체 태그로 가서 곡으로 이동'할 때 부르는
    /// 목록 갱신이 모두 무시됐습니다. 그 결과 ALL 태그(원본 802곡)인데 곡 목록이 0곡으로 남았습니다
    /// (2026-11-27, 에이전트 브리지 <c>stage.tags</c> 실측: <c>-1</c> → 변화 없음, <c>1</c> → 803곡).
    /// 그래서 <c>-1</c>은 지금 태그가 커스텀일 때만 막습니다.</para>
    /// </summary>
    public static class TagRefreshRules
    {
        /// <summary>게임이 "지금 고른 태그"를 뜻할 때 넘기는 값입니다.</summary>
        public const int CurrentTag = -1;

        /// <param name="tagIndex">원본에 넘어온 인자.</param>
        /// <param name="currentTagIndex">지금 고른 태그(<c>m_DbMusicTag.m_CurSelectedTagIndex</c>). 읽지 못했으면 null.</param>
        /// <returns>true면 원본을 실행하지 않습니다.</returns>
        public static bool ShouldSkipOriginal(int tagIndex, int? currentTagIndex)
        {
            if (tagIndex == CustomContentIds.TagIndex) return true;
            if (tagIndex != CurrentTag) return false;
            // 지금 태그를 모르면 예전처럼 막습니다(커스텀 태그였다면 원본이 NRE를 내므로 안전한 쪽).
            return currentTagIndex == null || currentTagIndex.Value == CustomContentIds.TagIndex;
        }
    }
}
