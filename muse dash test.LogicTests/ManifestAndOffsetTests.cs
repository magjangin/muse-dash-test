using Il2CppFormulaBase;

namespace muse_dash_test.LogicTests
{
    internal static class ManifestAndOffsetTests
    {
        public static void FirstSeparatorPreservesAllRemainingSeparators()
        {
            foreach (string separator in new[] { "=", ":", "：" })
            {
                Assert.True(HwaManifestLoader.TryParseManifestLine($"곡제목 {separator} Re:Zero：A=B // 주석", out var key, out var value));
                Assert.Equal("곡제목", key);
                Assert.Equal("Re:Zero：A=B", value);
            }
        }

        public static void CustomTitleAliasesPreserveHostSong()
        {
            foreach (string alias in new[] { "커스텀 곡 이름", "커스텀 곡명", "custom song name", "커스텀 곡제목", "custom title" })
            {
                var manifest = new HwaManifest();
                HwaManifestLoader.ApplyManifestValue(manifest, "가져올곡", "원본");
                Assert.True(HwaManifestLoader.TryParseManifestLine($"{alias} = Re:Zero", out var key, out var value));
                HwaManifestLoader.ApplyManifestValue(manifest, key, value);
                Assert.Equal("원본", manifest.Title);
                Assert.Equal("Re:Zero", manifest.CustomTitle);
            }
        }

        public static void CommentsAndInvalidLinesRemainIgnored()
        {
            foreach (string line in new[] { "", "// offset: 1", "제목", "= 값", "offset=" })
                Assert.False(HwaManifestLoader.TryParseManifestLine(line, out _, out _));
            Assert.True(HwaManifestLoader.TryParseManifestLine("곡제목 = https://example.com/a:b", out _, out var value));
            Assert.Equal("https://example.com/a:b", value);
        }

        public static void BothOffsetHooksKeepLegacyAbsoluteOffset()
        {
            try
            {
                CustomPlaySession.Current.SelectedMusicUid = "1999-1";
                HwaResourceManager.Manifest = new HwaManifest { Offset = -0.05123 };
                foreach (var hook in Hooks())
                {
                    var stage = new StageBattleComponent { offset = -0.1f };
                    hook(stage);
                    Assert.Equal(-0.05123f, stage.offset);
                    stage.offset = 0f; // 게임 원본이 새 보정값을 계산한 이후의 Postfix 입력
                    hook(stage);
                    Assert.Equal(-0.05123f, stage.offset);
                    stage.offset = 0.1f;
                    hook(stage);
                    Assert.Equal(-0.05123f, stage.offset);
                }
            }
            finally { Reset(); }
        }

        public static void MissingOffsetAndOfficialSongsRemainUnchanged()
        {
            try
            {
                foreach (var hook in Hooks())
                {
                    foreach (var manifest in new[] { null, new HwaManifest() })
                    {
                        CustomPlaySession.Current.SelectedMusicUid = "1999-1";
                        HwaResourceManager.Manifest = manifest;
                        var stage = new StageBattleComponent { offset = -0.1f };
                        hook(stage);
                        Assert.Equal(-0.1f, stage.offset);
                    }
                    CustomPlaySession.Current.SelectedMusicUid = "0-0";
                    HwaResourceManager.Manifest = new HwaManifest { Offset = 0.5 };
                    var official = new StageBattleComponent { offset = -0.1f };
                    hook(official);
                    Assert.Equal(-0.1f, official.offset);
                }
            }
            finally { Reset(); }
        }

        public static void ExpensiveUidLookupIsNotRepeatedForEveryHookCall()
        {
            try
            {
                // 세션이 곡을 모르는 상태여야 폴백(비싼 리플렉션 조회)이 돕니다.
                CustomPlaySession.Current.SelectedMusicUid = null;
                CustomPlaySession.Current.LastClickedMusicUid = null;
                PnlStagePatchHelper.StubUid = "1999-1";
                HwaResourceManager.Manifest = new HwaManifest { Offset = -0.02 };
                OffsetHookPatches.ResetUidCache();
                PnlStagePatchHelper.CallCount = 0;

                var stage = new StageBattleComponent { offset = 0f };
                for (int i = 0; i < 20; i++)
                {
                    OffsetHookPatches.PostfixFixedOffset(stage);
                }

                // 훅을 20번 불러도 비싼 조회가 20번 돌지는 않습니다.
                // (캐시 창이 100ms라 느린 머신에서 한 번 더 도는 것까지는 허용합니다.)
                Assert.True(PnlStagePatchHelper.CallCount <= 2, $"비싼 UID 조회가 {PnlStagePatchHelper.CallCount}번 돌았습니다.");
                // 캐시를 써도 주입 결과는 그대로여야 합니다.
                Assert.Equal(-0.02f, stage.offset);

                // 씬이 바뀌면 시간 만료를 기다리지 않고 바로 다시 조회해야 합니다.
                int before = PnlStagePatchHelper.CallCount;
                OffsetHookPatches.ResetUidCache();
                OffsetHookPatches.PostfixFixedOffset(stage);
                Assert.True(PnlStagePatchHelper.CallCount == before + 1,
                    $"캐시를 버린 뒤 조회가 {PnlStagePatchHelper.CallCount - before}번 돌았습니다.");
            }
            finally { Reset(); }
        }

        private static Action<StageBattleComponent>[] Hooks() => new Action<StageBattleComponent>[]
        {
            OffsetHookPatches.PostfixFixedOffset,
            stage => OffsetHookPatches.PostfixFixedMusicOffset(stage, null)
        };

        private static void Reset()
        {
            CustomPlaySession.Current.SelectedMusicUid = null;
            CustomPlaySession.Current.LastClickedMusicUid = null;
            HwaResourceManager.Manifest = null;
            PnlStagePatchHelper.StubUid = null;
            PnlStagePatchHelper.CallCount = 0;
            OffsetHookPatches.ResetUidCache();
        }
    }
}
