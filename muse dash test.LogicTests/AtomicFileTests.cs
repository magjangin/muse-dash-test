using System.Text;

namespace muse_dash_test.LogicTests
{
    // AtomicFile 검증. 저장이 끝나면 새 내용만 남고 임시 파일이 없어야 하며,
    // 임시 쓰기가 실패하면 옛 파일이 그대로 남아야 합니다(기록 파일이 잘리지 않게 하려고 넣은 헬퍼입니다).
    public static class AtomicFileTests
    {
        public static void WriteAllText_CreatesNewFileWithoutTempLeftover()
        {
            WithTempDir(dir =>
            {
                string path = Path.Combine(dir, "record.json");

                AtomicFile.WriteAllText(path, "{\"score\": 1}", Encoding.UTF8);

                Assert.Equal("{\"score\": 1}", File.ReadAllText(path, Encoding.UTF8));
                Assert.False(File.Exists(path + ".tmp"), "임시 파일이 남으면 안 됩니다.");
            });
        }

        public static void WriteAllText_ReplacesLongerExistingContent()
        {
            WithTempDir(dir =>
            {
                string path = Path.Combine(dir, "record.json");
                File.WriteAllText(path, "old contents that are much longer than the new one", Encoding.UTF8);

                AtomicFile.WriteAllText(path, "new", Encoding.UTF8);

                Assert.Equal("new", File.ReadAllText(path, Encoding.UTF8));
                Assert.False(File.Exists(path + ".tmp"), "임시 파일이 남으면 안 됩니다.");
            });
        }

        public static void WriteAllText_KeepsOldFileWhenTempWriteFails()
        {
            WithTempDir(dir =>
            {
                string path = Path.Combine(dir, "record.json");
                File.WriteAllText(path, "old", Encoding.UTF8);
                // 임시 파일 자리에 폴더를 만들어 임시 쓰기를 실패시킵니다. 원본은 건드려지면 안 됩니다.
                Directory.CreateDirectory(path + ".tmp");

                bool threw = false;
                try
                {
                    AtomicFile.WriteAllText(path, "new", Encoding.UTF8);
                }
                catch (Exception)
                {
                    threw = true;
                }

                Assert.True(threw, "임시 쓰기가 실패하면 예외가 호출자에게 전달돼야 합니다.");
                Assert.Equal("old", File.ReadAllText(path, Encoding.UTF8));
            });
        }

        public static void ReadAllBytesShared_CanReadWhileFileIsOpenedWithReadWriteShare()
        {
            WithTempDir(dir =>
            {
                string path = Path.Combine(dir, "shared_read.bin");
                byte[] expected = new byte[] { 0x01, 0x02, 0x03, 0x04 };
                File.WriteAllBytes(path, expected);

                // 외부 프로세스가 쓰기 가능한 공유 모드로 열고 있는 상황 시뮬레이션
                using (var fs = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    byte[] read = AtomicFile.ReadAllBytesShared(path);
                    Assert.Equal(expected.Length, read.Length);
                    for (int i = 0; i < expected.Length; i++)
                    {
                        Assert.Equal(expected[i], read[i]);
                    }
                }
            });
        }

        public static void ReadAllTextShared_CanReadWhileFileIsOpenedWithReadWriteShare()
        {
            WithTempDir(dir =>
            {
                string path = Path.Combine(dir, "shared_read.txt");
                string expected = "동시 접근 테스트 텍스트 1234";
                File.WriteAllText(path, expected, Encoding.UTF8);

                using (var fs = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    string read = AtomicFile.ReadAllTextShared(path, Encoding.UTF8);
                    Assert.Equal(expected, read);
                }
            });
        }

        private static void WithTempDir(Action<string> body)
        {
            string dir = Path.Combine(Path.GetTempPath(), "atomicfile-tests-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(dir);
            try
            {
                body(dir);
            }
            finally
            {
                try
                {
                    Directory.Delete(dir, recursive: true);
                }
                catch (IOException)
                {
                    // 테스트 임시 폴더 정리 실패는 결과에 영향을 주지 않습니다.
                }
            }
        }
    }
}
