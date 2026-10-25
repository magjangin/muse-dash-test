namespace muse_dash_test.LogicTests
{
    internal static class BmsDtTableTests
    {
        public static void ClassifiesEachFamily()
        {
            Assert.Equal(DtFamily.Normal, BmsDtTable.Classify("051001", 1, ""));
            Assert.Equal(DtFamily.Normal, BmsDtTable.Classify("051007", 1, ""));      // Up
            Assert.Equal(DtFamily.Normal, BmsDtTable.Classify("000301", 7, ""));      // 음표
            Assert.Equal(DtFamily.Hold, BmsDtTable.Classify("050203", 3, ""));
            Assert.Equal(DtFamily.BossProjectile, BmsDtTable.Classify("020702", 1, "boss_far_atk_2"));
            Assert.Equal(DtFamily.FixedEvent, BmsDtTable.Classify("000401", 9, ""));
            Assert.Equal(DtFamily.FixedEvent, BmsDtTable.Classify("050101", 0, "in"));
            Assert.Equal(DtFamily.None, BmsDtTable.Classify("050401", 8, ""));        // 샌드백
            Assert.Equal(DtFamily.None, BmsDtTable.Classify("090908", 2, ""));        // 보스 톱니
        }

        public static void RecommendedValuesMatchConfirmedTable()
        {
            AssertRecommended(DtFamily.Normal, new[] { 1.48, 1.28, 1.18 });
            AssertRecommended(DtFamily.Hold, new[] { 1.47, 1.26, 1.10 });
            AssertRecommended(DtFamily.BossProjectile, new[] { 0.8, 0.76, 0.6 });
            Assert.True(BmsDtTable.TryGetRecommended(DtFamily.FixedEvent, 0, out double fixedDt));
            Assert.Equal(0.0, fixedDt);
            Assert.False(BmsDtTable.TryGetRecommended(DtFamily.None, 1, out _));
        }

        public static void NormalFamilyAcceptsCharterRange()
        {
            Assert.True(BmsDtTable.IsAllowed(DtFamily.Normal, 1, 1.48, out _));
            Assert.False(BmsDtTable.IsAllowed(DtFamily.Normal, 1, 1.38, out _));
            Assert.True(BmsDtTable.IsAllowed(DtFamily.Normal, 2, 1.18, out _));
            Assert.True(BmsDtTable.IsAllowed(DtFamily.Normal, 2, 1.38, out _));
            Assert.False(BmsDtTable.IsAllowed(DtFamily.Normal, 2, 1.40, out string expected2));
            Assert.Equal("1.18~1.38", expected2);
            Assert.True(BmsDtTable.IsAllowed(DtFamily.Normal, 3, 0.98, out _));
            Assert.True(BmsDtTable.IsAllowed(DtFamily.Normal, 3, 1.28, out _));
            Assert.False(BmsDtTable.IsAllowed(DtFamily.Normal, 3, 0.9, out _));
        }

        public static void OtherFamiliesAcceptOnlyRecommended()
        {
            Assert.True(BmsDtTable.IsAllowed(DtFamily.Hold, 3, 1.10, out _));
            Assert.False(BmsDtTable.IsAllowed(DtFamily.Hold, 3, 1.07, out _));
            Assert.False(BmsDtTable.IsAllowed(DtFamily.BossProjectile, 1, 0.7, out _));
            // dt를 적지 않은 노트와 권장표 밖 계열은 판정하지 않습니다.
            Assert.True(BmsDtTable.IsAllowed(DtFamily.Hold, 3, -1.0, out _));
            Assert.True(BmsDtTable.IsAllowed(DtFamily.None, 1, 5.0, out _));
        }

        public static void ManifestKeyFromHwaInfoTurnsFixOn()
        {
            var manifest = new HwaManifest();
            Assert.True(HwaManifestLoader.TryParseManifestLine("dt를 권장 표로 고치기 = 활성화", out var key, out var value));
            HwaManifestLoader.ApplyManifestValue(manifest, key, value);
            Assert.True(manifest.FixDtToRecommended == true);
            Assert.True(manifest.ShowGhostNotes == null);

            HwaManifestLoader.ApplyManifestValue(manifest, "fix dt", "off");
            Assert.True(manifest.FixDtToRecommended == false);
        }

        private static void AssertRecommended(DtFamily family, double[] expected)
        {
            for (int speed = 1; speed <= 3; speed++)
            {
                Assert.True(BmsDtTable.TryGetRecommended(family, speed, out double dt));
                Assert.Equal(expected[speed - 1], dt);
            }
        }
    }
}
