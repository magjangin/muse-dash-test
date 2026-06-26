using MelonLoader;
using System;
using System.IO;

namespace muse_dash_test
{
    /// <summary>
    /// hwa/ 폴더의 BMS 파일 변경을 실시간으로 감지해 해당 곡의 캐시를 다시 로드합니다.
    /// </summary>
    public static partial class HwaResourceManager
    {
        private static FileSystemWatcher bmsWatcher = null;

        public static void InitializeBmsWatcher()
        {
            try
            {
                if (bmsWatcher != null)
                {
                    bmsWatcher.EnableRaisingEvents = false;
                    bmsWatcher.Dispose();
                    bmsWatcher = null;
                }

                if (!Directory.Exists(HwaFolderPath))
                {
                    return;
                }

                bmsWatcher = new FileSystemWatcher
                {
                    Path = HwaFolderPath,
                    Filter = "*.bms",
                    IncludeSubdirectories = true,
                    NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.Size
                };

                bmsWatcher.Changed += OnBmsFileChanged;
                bmsWatcher.Created += OnBmsFileChanged;
                bmsWatcher.Deleted += OnBmsFileChanged;
                bmsWatcher.Renamed += OnBmsFileRenamed;

                bmsWatcher.EnableRaisingEvents = true;
                MelonLogger.Msg($"[HwaResourceManager.BmsWatcher] BMS 실시간 폴더 감시 시작: {HwaFolderPath}");
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[HwaResourceManager.BmsWatcher] BMS 폴더 감시 설정 실패: {ex.Message}");
            }
        }

        private static void OnBmsFileChanged(object sender, FileSystemEventArgs e)
        {
            HandleBmsFileEvent(e.FullPath);
        }

        private static void OnBmsFileRenamed(object sender, RenamedEventArgs e)
        {
            HandleBmsFileEvent(e.FullPath);
        }

        private static void HandleBmsFileEvent(string filePath)
        {
            try
            {
                if (string.IsNullOrEmpty(filePath)) return;
                string ext = Path.GetExtension(filePath);
                if (!string.Equals(ext, ".bms", StringComparison.OrdinalIgnoreCase)) return;

                string fullPath = Path.GetFullPath(filePath);
                string matchedUid = null;

                foreach (var uid in virtualUids)
                {
                    if (TryGetSongDirectory(uid, out string songDir) && !string.IsNullOrEmpty(songDir))
                    {
                        string fullSongDir = Path.GetFullPath(songDir).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                        if (fullPath.StartsWith(fullSongDir + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase))
                        {
                            matchedUid = uid;
                            break;
                        }
                    }
                }

                if (matchedUid != null)
                {
                    ReloadBmsChartForUid(matchedUid);
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[HwaResourceManager.BmsWatcher] 파일 변경 처리 중 오류: {ex.Message}");
            }
        }

        public static bool ReloadBmsChartForUid(string uid)
        {
            if (uid == null || !cachedManifests.TryGetValue(uid, out var manifest))
            {
                return false;
            }

            if (!TryGetSongDirectory(uid, out string songDir) || string.IsNullOrEmpty(songDir))
            {
                return false;
            }

            MelonLogger.Msg($"[HwaResourceManager.BmsWatcher] BMS 실시간 감지 -> [{uid}] 다시 읽기 시도: {songDir}");
            BmsChart newChart = LoadHwaBmsChart(songDir, manifest);
            if (newChart != null)
            {
                lock (cachedBmsCharts)
                {
                    cachedBmsCharts[uid] = newChart;
                }
                MelonLogger.Msg($"[HwaResourceManager.BmsWatcher] ✅ [{uid}] BMS 실시간 재로드 성공!");
                return true;
            }
            else
            {
                MelonLogger.Warning($"[HwaResourceManager.BmsWatcher] ❌ [{uid}] BMS 실시간 재로드 실패");
                return false;
            }
        }
    }
}
