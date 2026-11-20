using System.Collections.Generic;

namespace muse_dash_test.LogicTests
{
    // SaveSelectionRules 검증.
    // 판정이 틀리면 모드를 뺀 바닐라가 시작할 때 RefreshStageDisplayMusics(1999)에서 NRE가 나 곡 선택 화면이 멈춥니다(2026-11-20).
    public static class SaveSelectionRulesTests
    {
        public static void ModValues_AreDetected()
        {
            // 2026-11-20 실제 세이브(레지스트리 Account)에 남아 있던 값입니다.
            Assert.True(SaveSelectionRules.PointsToModContent(SaveSelectionRules.TagIndexKey, "1999"));
            Assert.True(SaveSelectionRules.PointsToModContent(SaveSelectionRules.AlbumUidKey, "1999-0"));
            Assert.True(SaveSelectionRules.PointsToModContent(SaveSelectionRules.MusicUidKey, "1999-1"));
        }

        public static void VanillaValues_AreLeftAlone()
        {
            Assert.False(SaveSelectionRules.PointsToModContent(SaveSelectionRules.TagIndexKey, "0"));
            Assert.False(SaveSelectionRules.PointsToModContent(SaveSelectionRules.TagIndexKey, "19"));
            Assert.False(SaveSelectionRules.PointsToModContent(SaveSelectionRules.AlbumUidKey, "music_package_0"));
            Assert.False(SaveSelectionRules.PointsToModContent(SaveSelectionRules.MusicUidKey, "48-8"));
            Assert.False(SaveSelectionRules.PointsToModContent(SaveSelectionRules.MusicUidKey, null));
            // 다른 키는 판정하지 않습니다(인덱스 값이 우연히 1999여도).
            Assert.False(SaveSelectionRules.PointsToModContent("SelectedMusicIndex", "1999"));
        }

        public static void AnyModValue_ReplacesAllThreeTogether()
        {
            var values = new Dictionary<string, string>
            {
                { SaveSelectionRules.TagIndexKey, "0" },
                { SaveSelectionRules.AlbumUidKey, "music_package_0" },
                { SaveSelectionRules.MusicUidKey, "1999-3" },
            };
            Assert.True(SaveSelectionRules.AnyPointsToModContent(values));
        }

        public static void Replacement_PrefersLastVanillaSelection()
        {
            var last = new Dictionary<string, string>
            {
                { SaveSelectionRules.TagIndexKey, "3" },
                { SaveSelectionRules.AlbumUidKey, "music_package_43" },
                { SaveSelectionRules.MusicUidKey, "48-8" },
            };
            Assert.Equal("3", SaveSelectionRules.ReplacementFor(SaveSelectionRules.TagIndexKey, last));
            Assert.Equal("music_package_43", SaveSelectionRules.ReplacementFor(SaveSelectionRules.AlbumUidKey, last));
            Assert.Equal("48-8", SaveSelectionRules.ReplacementFor(SaveSelectionRules.MusicUidKey, last));
        }

        public static void Replacement_FallsBackToDefaultMusic()
        {
            Assert.Equal("0", SaveSelectionRules.ReplacementFor(SaveSelectionRules.TagIndexKey, null));
            Assert.Equal("music_package_0", SaveSelectionRules.ReplacementFor(SaveSelectionRules.AlbumUidKey, new Dictionary<string, string>()));
            Assert.Equal("0-0", SaveSelectionRules.ReplacementFor(SaveSelectionRules.MusicUidKey, null));

            // 기억한 값이 모드 값이면 쓰지 않습니다.
            var polluted = new Dictionary<string, string> { { SaveSelectionRules.MusicUidKey, "1999-2" } };
            Assert.Equal("0-0", SaveSelectionRules.ReplacementFor(SaveSelectionRules.MusicUidKey, polluted));
        }
    }
}
