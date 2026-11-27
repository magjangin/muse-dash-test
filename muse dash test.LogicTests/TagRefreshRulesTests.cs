namespace muse_dash_test.LogicTests
{
    // TagRefreshRules 검증.
    // -1을 통째로 막으면 곡 선택 화면의 목록이 0곡으로 남고(2026-11-27), 1999를 통과시키면 원본이 NRE를 냅니다.
    public static class TagRefreshRulesTests
    {
        public static void CustomTag_IsAlwaysSkipped()
        {
            Assert.True(TagRefreshRules.ShouldSkipOriginal(CustomContentIds.TagIndex, null));
            Assert.True(TagRefreshRules.ShouldSkipOriginal(CustomContentIds.TagIndex, 1));
        }

        public static void CurrentTag_IsSkippedOnlyWhenCurrentIsCustom()
        {
            // 2026-11-27 실측 상황: 지금 태그 = ALL(1). 원본이 돌아야 803곡이 채워집니다.
            Assert.False(TagRefreshRules.ShouldSkipOriginal(TagRefreshRules.CurrentTag, 1));
            Assert.False(TagRefreshRules.ShouldSkipOriginal(TagRefreshRules.CurrentTag, 0));
            Assert.False(TagRefreshRules.ShouldSkipOriginal(TagRefreshRules.CurrentTag, 11));
            Assert.True(TagRefreshRules.ShouldSkipOriginal(TagRefreshRules.CurrentTag, CustomContentIds.TagIndex));
        }

        public static void CurrentTag_Unknown_IsSkippedAsBefore()
        {
            Assert.True(TagRefreshRules.ShouldSkipOriginal(TagRefreshRules.CurrentTag, null));
        }

        public static void VanillaTags_RunOriginal()
        {
            Assert.False(TagRefreshRules.ShouldSkipOriginal(1, null));
            Assert.False(TagRefreshRules.ShouldSkipOriginal(32768, null));
        }
    }
}
