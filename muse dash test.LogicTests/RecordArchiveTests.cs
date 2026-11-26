using System;
using System.IO;

namespace muse_dash_test.LogicTests
{
    // RecordArchive 검증. 채보를 갈아끼운 슬롯의 옛 기록이 지문별로 보관되고, 같은 채보로 되돌리면 찾을 수 있어야 합니다.
    public static class RecordArchiveTests
    {
        public static void PathFor_PutsFingerprintBeforeExtensionInArchiveFolder()
        {
            string path = Path.Combine("record", "neon_2.json");
            string archived = RecordArchive.PathFor(path, "ab12cd34ef56ab78");
            Assert.Equal(Path.Combine("record", "archive", "neon_2.ab12cd34ef56ab78.json"), archived);
        }

        public static void PathFor_EmptyFingerprint_ReturnsNull()
        {
            Assert.Null(RecordArchive.PathFor(Path.Combine("record", "neon_2.json"), ""));
            Assert.Null(RecordArchive.PathFor(Path.Combine("record", "neon_2.json"), null));
        }

        public static void ShouldArchive_OnlyWhenBothPresentAndDifferent()
        {
            Assert.True(RecordArchive.ShouldArchive("aaaa", "bbbb"));
            Assert.False(RecordArchive.ShouldArchive("aaaa", "AAAA"));
            Assert.False(RecordArchive.ShouldArchive("", "bbbb"));   // 지문 없는 옛 기록은 .bak이 맡습니다.
            Assert.False(RecordArchive.ShouldArchive("aaaa", null));
        }

        public static void ArchiveCopy_CopiesAndOverwritesSameFingerprint()
        {
            WithTempDir(dir =>
            {
                string record = Path.Combine(dir, "song_1.json");
                File.WriteAllText(record, "first");

                string archived = RecordArchive.ArchiveCopy(record, "aaaa", "bbbb");
                Assert.Equal(Path.Combine(dir, "archive", "song_1.aaaa.json"), archived);
                Assert.Equal("first", File.ReadAllText(archived));
                Assert.True(File.Exists(record), "원본은 그대로 두어야 합니다(덮어쓰기는 호출자가 합니다).");

                File.WriteAllText(record, "second");
                RecordArchive.ArchiveCopy(record, "aaaa", "cccc");
                Assert.Equal("second", File.ReadAllText(archived));
            });
        }

        public static void ArchiveCopy_SameFingerprintOrMissingFile_DoesNothing()
        {
            WithTempDir(dir =>
            {
                string record = Path.Combine(dir, "song_1.json");
                Assert.Null(RecordArchive.ArchiveCopy(record, "aaaa", "bbbb")); // 파일 없음

                File.WriteAllText(record, "x");
                Assert.Null(RecordArchive.ArchiveCopy(record, "aaaa", "aaaa"));
                Assert.False(Directory.Exists(Path.Combine(dir, "archive")), "보관할 것이 없으면 폴더도 만들지 않습니다.");
            });
        }

        private static void WithTempDir(Action<string> body)
        {
            string dir = Path.Combine(Path.GetTempPath(), "recordarchive-tests-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(dir);
            try
            {
                body(dir);
            }
            finally
            {
                try { Directory.Delete(dir, recursive: true); }
                catch (IOException) { }
            }
        }
    }
}
