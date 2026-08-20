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

        /// <summary>
        /// 같은 파일에 대해 이 시간 안에 다시 들어온 이벤트는 무시합니다.
        /// FileSystemWatcher는 저장 한 번에 LastWrite와 Size 알림을 따로 올리고, 에디터에 따라
        /// 쓰기 자체가 여러 번 일어나기도 합니다. 그때마다 파일을 다시 읽고 파싱하면
        /// 저장 1회에 재파싱이 여러 번 도는 낭비가 생깁니다.
        /// </summary>
        private static readonly TimeSpan BmsEventDebounceWindow = TimeSpan.FromMilliseconds(300);

        /// <summary>경로별 마지막 처리 시각. 워처 이벤트는 스레드풀에서 오므로 접근을 잠급니다.</summary>
        private static readonly System.Collections.Generic.Dictionary<string, DateTime> lastHandledByPath =
            new System.Collections.Generic.Dictionary<string, DateTime>(StringComparer.OrdinalIgnoreCase);

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
                ModLogger.Msg($"[HwaResourceManager.BmsWatcher] BMS 실시간 폴더 감시 시작: {HwaFolderPath}");
            }
            catch (Exception ex)
            {
                ModLogger.Error($"[HwaResourceManager.BmsWatcher] BMS 폴더 감시 설정 실패: {ex.Message}");
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
                if (IsTempBmsFile(filePath)) return;
                string ext = Path.GetExtension(filePath);
                if (!string.Equals(ext, ".bms", StringComparison.OrdinalIgnoreCase)) return;

                string fullPath = Path.GetFullPath(filePath);

                // 저장 1회에 여러 알림이 몰려 오는 경우 첫 건만 처리합니다.
                lock (lastHandledByPath)
                {
                    DateTime now = DateTime.UtcNow;
                    if (lastHandledByPath.TryGetValue(fullPath, out DateTime last)
                        && now - last < BmsEventDebounceWindow)
                    {
                        return;
                    }
                    lastHandledByPath[fullPath] = now;
                }

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
                ModLogger.Error($"[HwaResourceManager.BmsWatcher] 파일 변경 처리 중 오류: {ex.Message}");
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

            ModLogger.Msg($"[HwaResourceManager.BmsWatcher] BMS 실시간 감지 -> [{uid}] 다시 읽기 시도: {songDir}");
            BmsChart newChart = LoadHwaBmsChart(songDir, manifest);
            if (newChart != null)
            {
                lock (cachedBmsCharts)
                {
                    cachedBmsCharts[uid] = newChart;
                }
                ModLogger.Msg($"[HwaResourceManager.BmsWatcher] ✅ [{uid}] BMS 실시간 재로드 성공!");
                return true;
            }
            else
            {
                ModLogger.Warning($"[HwaResourceManager.BmsWatcher] ❌ [{uid}] BMS 실시간 재로드 실패");
                return false;
            }
        }
    }
}
