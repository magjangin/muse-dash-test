using System;
using System.IO;

namespace muse_dash_test.LogicTests
{
    public static class CustomRecordStoreSerializationTests
    {
        public static void RoundTrip_PreservesAllFields()
        {
            string json = CustomRecordStore.BuildJson(
                uid: "hwa_001_0",
                noteCount: 500,
                standard: 450, gears: 20, hearts: 10, blueNotes: 20,
                perfect: 440, great: 8, miss: 2,
                score: 123456, maxCombo: 350,
                accuracy: 0.985f, isFullCombo: false, isAllPerfect: false,
                playCount: 7,
                savedAtUtc: "2026-11-18T00:00:00Z",
                chartFingerprint: "abc123",
                songFolder: "Test \"Song\" Folder");

            var parsed = CustomRecordStore.ParseJson(json);

            Assert.Equal("hwa_001_0", parsed.uid);
            Assert.Equal(500, parsed.noteCount);
            Assert.Equal(450, parsed.standard);
            Assert.Equal(20, parsed.gears);
            Assert.Equal(10, parsed.hearts);
            Assert.Equal(20, parsed.blueNotes);
            Assert.Equal(440, parsed.perfect);
            Assert.Equal(8, parsed.great);
            Assert.Equal(2, parsed.miss);
            Assert.Equal(123456, parsed.score);
            Assert.Equal(350, parsed.maxCombo);
            Assert.True(Math.Abs(parsed.accuracy - 0.985f) < 0.0001f, $"Expected 0.985, got {parsed.accuracy}");
            Assert.False(parsed.isFullCombo);
            Assert.False(parsed.isAllPerfect);
            Assert.Equal(7, parsed.playCount);
            Assert.Equal("2026-11-18T00:00:00Z", parsed.savedAtUtc);
            Assert.Equal("abc123", parsed.chartFingerprint);
        }

        public static void BuildJson_EmptyFingerprintDefaultsToNoChart()
        {
            string json = CustomRecordStore.BuildJson(
                uid: "hwa_001",
                noteCount: 0,
                standard: 0, gears: 0, hearts: 0, blueNotes: 0,
                perfect: 0, great: 0, miss: 0,
                score: 0, maxCombo: 0,
                accuracy: 0f, isFullCombo: false, isAllPerfect: false,
                playCount: 1,
                savedAtUtc: "2026-11-18T00:00:00Z",
                chartFingerprint: "",
                songFolder: "Empty");

            var parsed = CustomRecordStore.ParseJson(json);
            Assert.Equal(CustomRecordStore.NoChartFingerprint, parsed.chartFingerprint);
        }

        public static void ReadRecordFile_DefaultsNonPositivePlayCountToOne()
        {
            string tempFile = Path.GetTempFileName();
            try
            {
                string json = CustomRecordStore.BuildJson(
                    uid: "hwa_002",
                    noteCount: 10,
                    standard: 10, gears: 0, hearts: 0, blueNotes: 0,
                    perfect: 10, great: 0, miss: 0,
                    score: 1000, maxCombo: 10,
                    accuracy: 1.0f, isFullCombo: true, isAllPerfect: true,
                    playCount: 0,
                    savedAtUtc: "2026-11-18T00:00:00Z",
                    chartFingerprint: "fp",
                    songFolder: "Song");
                File.WriteAllText(tempFile, json);

                var parsed = CustomRecordStore.ReadRecordFile(tempFile);
                Assert.Equal(1, parsed.playCount);
                Assert.True(parsed.isFullCombo);
                Assert.True(parsed.isAllPerfect);
            }
            finally
            {
                if (File.Exists(tempFile)) File.Delete(tempFile);
            }
        }

        public static void SanitizeFileName_ReplacesInvalidChars()
        {
            string sanitized = CustomRecordStore.SanitizeFileName("a:b/c\\d*e?");
            foreach (char invalid in Path.GetInvalidFileNameChars())
            {
                Assert.False(sanitized.Contains(invalid), $"Contains invalid char: {invalid}");
            }
        }
    }
}
