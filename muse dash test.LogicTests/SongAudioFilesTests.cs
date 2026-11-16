namespace muse_dash_test.LogicTests
{
    // SongAudioFiles: 곡 폴더에서 BGM ogg 하나를 고르는 규칙(미리듣기와 배틀이 공유) 검증.
    public static class SongAudioFilesTests
    {
        public static void PickBgmOgg_PrefersOggNamedAfterSettingsTxt()
        {
            string picked = SongAudioFiles.PickBgmOgg(
                new[] { @"C:\hwa\곡\music.ogg", @"C:\hwa\곡\info.ogg" },
                new[] { @"C:\hwa\곡\info.txt" });

            Assert.Equal(@"C:\hwa\곡\info.ogg", picked);
        }

        public static void PickBgmOgg_FallsBackToKeywordThenFirstByName()
        {
            // txt와 이름이 같은 ogg가 없으면 키워드가 든 파일을, 그것도 없으면 이름순 첫 파일을 고릅니다.
            Assert.Equal(@"C:\hwa\곡\z_music.ogg", SongAudioFiles.PickBgmOgg(
                new[] { @"C:\hwa\곡\b.ogg", @"C:\hwa\곡\z_music.ogg", @"C:\hwa\곡\a.ogg" },
                new[] { @"C:\hwa\곡\hwa info.txt" }));

            Assert.Equal(@"C:\hwa\곡\a.ogg", SongAudioFiles.PickBgmOgg(
                new[] { @"C:\hwa\곡\b.ogg", @"C:\hwa\곡\a.ogg" },
                null));
        }

        public static void PickBgmOgg_TreatsDemoAsOrdinaryName()
        {
            // 메뉴 전용 규칙이 따로 있던 시절에는 미리듣기만 'demo'를 골랐고 배틀은 'music'을 골랐습니다.
            // 이제 둘은 같은 파일을 틀어야 하므로 'demo'는 키워드가 아닙니다.
            Assert.Equal(@"C:\hwa\곡\music.ogg", SongAudioFiles.PickBgmOgg(
                new[] { @"C:\hwa\곡\demo.ogg", @"C:\hwa\곡\music.ogg" },
                null));
        }

        public static void PickBgmOgg_ReturnsNullWithoutOggAndLeavesInputUntouched()
        {
            Assert.Null(SongAudioFiles.PickBgmOgg(new string[0], null));
            Assert.Null(SongAudioFiles.PickBgmOgg(null, null));

            var oggFiles = new[] { @"C:\b.ogg", @"C:\a.ogg" };
            SongAudioFiles.PickBgmOgg(oggFiles, null);
            Assert.Equal(@"C:\b.ogg", oggFiles[0]);
        }

        public static void ResolveBgmOggPath_CachesAndDropsDeletedFile()
        {
            string dir = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "sat_" + System.Guid.NewGuid().ToString("N"));
            System.IO.Directory.CreateDirectory(dir);
            try
            {
                SongAudioFiles.ClearCache();
                string a = System.IO.Path.Combine(dir, "a.ogg");
                System.IO.File.WriteAllText(a, "x");
                Assert.Equal(a, SongAudioFiles.ResolveBgmOggPath(dir));

                // 유예 안에서는 새 파일이 생겨도 캐시된 결과를 돌려줍니다(디렉터리 재스캔 생략).
                string bgm = System.IO.Path.Combine(dir, "bgm.ogg");
                System.IO.File.WriteAllText(bgm, "x");
                Assert.Equal(a, SongAudioFiles.ResolveBgmOggPath(dir));

                // 캐시된 파일이 사라지면 즉시 다시 스캔합니다.
                System.IO.File.Delete(a);
                Assert.Equal(bgm, SongAudioFiles.ResolveBgmOggPath(dir));
            }
            finally
            {
                SongAudioFiles.ClearCache();
                System.IO.Directory.Delete(dir, true);
            }
        }
    }
}
