using System;
using System.Collections.Generic;
using System.IO;

namespace muse_dash_test.LogicTests
{
    public static class CoreAndHelperRulesTests
    {
        public static void CustomContentIds_DistinguishesAlbumAndVirtualSongs()
        {
            Assert.Equal("1999-1", CustomContentIds.CreateVirtualSongUid(0));
            Assert.Equal("1999-2", CustomContentIds.CreateVirtualSongUid(1));
            Assert.Throws<ArgumentOutOfRangeException>(() => CustomContentIds.CreateVirtualSongUid(-1));
            Assert.True(CustomContentIds.IsVirtualSong("1999-1"));
            Assert.True(CustomContentIds.IsVirtualSong("1999-42"));
            Assert.False(CustomContentIds.IsVirtualSong(CustomContentIds.AlbumUid));
            Assert.False(CustomContentIds.IsVirtualSong("0-0"));
            Assert.False(CustomContentIds.IsVirtualSong(null));

            Assert.True(CustomContentIds.IsVirtualContent(CustomContentIds.AlbumUid));
            Assert.True(CustomContentIds.IsVirtualContent("1999-1"));
            Assert.False(CustomContentIds.IsVirtualContent("0-0"));
        }

        public static void ScenePatchHelpers_DetectsGhostUidAndFormatsZzCounts()
        {
            Assert.True(ScenePatchHelpers.IsSixDigitUid("051701"));
            Assert.False(ScenePatchHelpers.IsSixDigitUid("05170"));
            Assert.False(ScenePatchHelpers.IsSixDigitUid("05170A"));
            Assert.False(ScenePatchHelpers.IsSixDigitUid(null));

            Assert.True(ScenePatchHelpers.IsGhostNoteUid("051701"));
            Assert.False(ScenePatchHelpers.IsGhostNoteUid("051001"));
            Assert.False(ScenePatchHelpers.IsGhostNoteUid("17"));

            var counts = new SortedDictionary<string, int>(StringComparer.Ordinal);
            ScenePatchHelpers.CountZz(counts, "051001");
            ScenePatchHelpers.CountZz(counts, "020401");
            ScenePatchHelpers.CountZz(counts, "051704");
            ScenePatchHelpers.CountZz(counts, "x");
            Assert.Equal("{02:1, 05:2}", ScenePatchHelpers.FormatZzCounts(counts));
            Assert.Equal("{}", ScenePatchHelpers.FormatZzCounts(null));
        }

        public static void SchemaUidCode_ParsesSpeedAndMotionWhileGuardingSceneAndBossUids()
        {
            Assert.Equal("05", DBStageInfo_SetRuntimeMusicData_Patch.UidCode.Scene("051004"));
            Assert.Equal("10", DBStageInfo_SetRuntimeMusicData_Patch.UidCode.Xx("051004"));
            Assert.Equal("04", DBStageInfo_SetRuntimeMusicData_Patch.UidCode.Yy("051004"));
            Assert.Null(DBStageInfo_SetRuntimeMusicData_Patch.UidCode.Yy("0510"));

            Assert.Equal(1, DBStageInfo_SetRuntimeMusicData_Patch.UidCode.Speed("051001"));
            Assert.Equal(2, DBStageInfo_SetRuntimeMusicData_Patch.UidCode.Speed("051005"));
            Assert.Equal(3, DBStageInfo_SetRuntimeMusicData_Patch.UidCode.Speed("051018"));
            Assert.Equal(0, DBStageInfo_SetRuntimeMusicData_Patch.UidCode.MotionGroup("051001"));
            Assert.Equal(1, DBStageInfo_SetRuntimeMusicData_Patch.UidCode.MotionGroup("051005"));
            Assert.Equal(5, DBStageInfo_SetRuntimeMusicData_Patch.UidCode.MotionGroup("051018"));

            // 0004yy(씬 전환)와 xx=01(보스 동작)은 속도 코드가 아닙니다.
            Assert.Equal(0, DBStageInfo_SetRuntimeMusicData_Patch.UidCode.Speed("000402"));
            Assert.Equal(-1, DBStageInfo_SetRuntimeMusicData_Patch.UidCode.MotionGroup("000402"));
            Assert.Equal(0, DBStageInfo_SetRuntimeMusicData_Patch.UidCode.Speed("050102"));
            Assert.Equal(0, DBStageInfo_SetRuntimeMusicData_Patch.UidCode.Speed("051019"));
        }

        public static void UnlockAllMasterGuard_CapturesOnlyFirstSessionValue()
        {
            try
            {
                UnlockAllMasterGuard.ResetForTests();
                Assert.False(UnlockAllMasterGuard.IsCaptured);
                Assert.False(UnlockAllMasterGuard.GetRestoreValue());

                UnlockAllMasterGuard.CaptureOnce(false);
                UnlockAllMasterGuard.CaptureOnce(true);
                Assert.True(UnlockAllMasterGuard.IsCaptured);
                Assert.False(UnlockAllMasterGuard.GetRestoreValue());

                UnlockAllMasterGuard.ResetForTests();
                UnlockAllMasterGuard.CaptureOnce(true);
                UnlockAllMasterGuard.CaptureOnce(false);
                Assert.True(UnlockAllMasterGuard.GetRestoreValue());
            }
            finally
            {
                UnlockAllMasterGuard.ResetForTests();
            }
        }

        public static void SafeAccess_ReturnsFallbackWhenNullOrThrowing()
        {
            Assert.Equal(42, SafeAccess.Get(() => 42, -1));
            Assert.Equal(-1, SafeAccess.Get<int>(null, -1));
            Assert.Equal("fallback", SafeAccess.Get<string>(() => throw new InvalidOperationException("boom"), "fallback", "unit-test"));
        }

        public static void ChartFingerprint_HashesOpenFilesAndInvalidatesOnChange()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "md_fp_test_" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);
            string bmsPath = Path.Combine(tempDir, "chart.bms");
            try
            {
                ChartFingerprint.ClearCache();
                Assert.Equal(ChartFingerprint.NoChart, ChartFingerprint.ForFile(null));
                Assert.Equal(ChartFingerprint.NoChart, ChartFingerprint.ForFile(bmsPath));

                File.WriteAllText(bmsPath, "#TITLE Test\n#00111:01\n");
                string fp1;
                using (var writerLock = new FileStream(bmsPath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite | FileShare.Delete))
                {
                    fp1 = ChartFingerprint.ForFile(bmsPath);
                }
                Assert.NotNull(fp1);
                Assert.Equal(16, fp1.Length);
                Assert.Equal(fp1, ChartFingerprint.ForFile(bmsPath));

                File.WriteAllText(bmsPath, "#TITLE Test Modified Content Longer\n#00111:0102\n");
                string fp2 = ChartFingerprint.ForFile(bmsPath);
                Assert.NotNull(fp2);
                Assert.False(string.Equals(fp1, fp2, StringComparison.Ordinal));
            }
            finally
            {
                ChartFingerprint.ClearCache();
                if (Directory.Exists(tempDir)) Directory.Delete(tempDir, recursive: true);
            }
        }
    }
}
