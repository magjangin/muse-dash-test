using MelonLoader;
using System;
using System.Collections.Generic;
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
        /// 마지막 변경 뒤 이 시간 동안 같은 곡에 새 변경이 없을 때 한 번 다시 읽습니다(디바운스).
        /// 예전에는 이 시간 안의 후속 이벤트를 버렸는데, 여러 번에 나눠 쓰는 저장에서는 마지막 완성본이 버려져
        /// 중간 상태가 캐시에 남았습니다. 지금은 마지막 이벤트 뒤에 읽으므로 최종 내용이 반영됩니다.
        /// </summary>
        private static readonly TimeSpan BmsReloadQuietPeriod = TimeSpan.FromMilliseconds(300);

        /// <summary>파일이 아직 완성되지 않아 읽지 못했을 때의 재시도 간격과 최대 횟수입니다.</summary>
        private static readonly TimeSpan BmsReloadRetryDelay = TimeSpan.FromMilliseconds(500);
        private const int BmsReloadMaxRetries = 3;

        // 워처 이벤트와 타이머는 스레드풀에서 옵니다. 아래 상태는 bmsReloadLock으로만 접근합니다.
        private static readonly object bmsReloadLock = new object();
        private static readonly HashSet<string> pendingReloadUids = new HashSet<string>(StringComparer.Ordinal);
        private static readonly Dictionary<string, int> reloadRetryCounts = new Dictionary<string, int>(StringComparer.Ordinal);
        private static System.Threading.Timer bmsReloadTimer;

        // 재로드 콜백이 도는 중인지. System.Threading.Timer는 콜백이 끝나기 전에 Change로 다시 맞추면 콜백을
        // 다른 스레드풀 스레드에서 겹쳐 부를 수 있습니다. 같은 곡을 두 스레드가 동시에 읽어 합치면 늦게 끝난 쪽
        // (더 옛 내용일 수 있음)이 캐시를 덮으므로, 도는 중에 온 예약은 콜백이 끝난 뒤 한 번 더 돌게 미룹니다.
        private static bool bmsReloadRunning;

        /// <summary>
        /// 감시 버퍼 크기(바이트). 기본 8KB는 에디터가 여러 파일을 한꺼번에 저장하거나 곡 폴더를 통째로 복사할 때
        /// 넘칠 수 있고, 넘치면 그 사이 변경 알림이 사라집니다. 최대값 64KB로 둡니다.
        /// </summary>
        private const int BmsWatcherBufferSize = 64 * 1024;

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
                    NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.Size,
                    InternalBufferSize = BmsWatcherBufferSize
                };

                bmsWatcher.Changed += OnBmsFileChanged;
                bmsWatcher.Created += OnBmsFileChanged;
                bmsWatcher.Deleted += OnBmsFileChanged;
                bmsWatcher.Renamed += OnBmsFileRenamed;
                bmsWatcher.Error += OnBmsWatcherError;

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
            // 이름을 바꾸면 옛 경로가 속한 곡도 바뀐 것입니다. 옛 경로와 새 경로를 모두 처리합니다.
            HandleBmsFileEvent(e.OldFullPath);
            HandleBmsFileEvent(e.FullPath);
        }

        /// <summary>
        /// 감시자 자체의 오류입니다. 예전에는 이 이벤트를 받지 않아, 버퍼가 넘치면(<see cref="InternalBufferOverflowException"/>)
        /// 그 사이의 채보 저장이 로그 한 줄 없이 반영되지 않았습니다. 어느 파일이 바뀌었는지 알 수 없으므로 모든 곡을 다시
        /// 읽도록 예약하고(<see cref="BmsChartSet.MergeReload"/>가 읽지 못한 칸은 옛 채보를 유지), 감시가 멈췄으면 다시 겁니다.
        /// </summary>
        private static void OnBmsWatcherError(object sender, ErrorEventArgs e)
        {
            try
            {
                Exception ex = e?.GetException();
                ModLogger.Warning($"[HwaResourceManager.BmsWatcher] 폴더 감시 오류({ex?.GetType().Name ?? "알 수 없음"}: {ex?.Message}). 모든 곡의 채보를 다시 읽도록 예약합니다.");

                string[] uidsSnapshot;
                lock (virtualUids)
                {
                    uidsSnapshot = virtualUids.ToArray();
                }
                foreach (var uid in uidsSnapshot)
                {
                    ScheduleBmsReload(uid, BmsReloadQuietPeriod);
                }

                // 버퍼 넘침은 감시를 멈추지 않지만, 폴더가 지워지거나 접근이 끊기면 감시가 꺼집니다. 그때만 다시 겁니다.
                var watcher = sender as FileSystemWatcher;
                if (watcher != null && ReferenceEquals(watcher, bmsWatcher) && !watcher.EnableRaisingEvents)
                {
                    ModLogger.Warning("[HwaResourceManager.BmsWatcher] 폴더 감시가 멈춰 다시 시작합니다.");
                    InitializeBmsWatcher();
                }
            }
            catch (Exception handlerEx)
            {
                ModLogger.Error($"[HwaResourceManager.BmsWatcher] 감시 오류 처리 실패: {handlerEx.Message}");
            }
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

                string matchedUid = null;
                string[] uidsSnapshot;
                lock (virtualUids)
                {
                    uidsSnapshot = virtualUids.ToArray();
                }

                foreach (var uid in uidsSnapshot)
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
                    ScheduleBmsReload(matchedUid, BmsReloadQuietPeriod);
                }
            }
            catch (Exception ex)
            {
                ModLogger.Error($"[HwaResourceManager.BmsWatcher] 파일 변경 처리 중 오류: {ex.Message}");
            }
        }

        /// <summary>
        /// 곡의 재로드를 예약합니다. 이미 예약된 곡에 이벤트가 또 오면 타이머를 다시 맞춰 마지막 이벤트 뒤에 한 번만 읽습니다.
        /// </summary>
        private static void ScheduleBmsReload(string uid, TimeSpan delay)
        {
            lock (bmsReloadLock)
            {
                pendingReloadUids.Add(uid);
                if (bmsReloadTimer == null)
                {
                    bmsReloadTimer = new System.Threading.Timer(OnBmsReloadTimer, null, delay, System.Threading.Timeout.InfiniteTimeSpan);
                }
                else
                {
                    bmsReloadTimer.Change(delay, System.Threading.Timeout.InfiniteTimeSpan);
                }
            }
        }

        private static void OnBmsReloadTimer(object state)
        {
            string[] uids;
            lock (bmsReloadLock)
            {
                if (bmsReloadRunning)
                {
                    // 앞 콜백이 아직 읽는 중입니다. 예약은 pendingReloadUids에 남겨 두고 조금 뒤 다시 봅니다.
                    bmsReloadTimer?.Change(BmsReloadQuietPeriod, System.Threading.Timeout.InfiniteTimeSpan);
                    return;
                }
                bmsReloadRunning = true;
                uids = new string[pendingReloadUids.Count];
                pendingReloadUids.CopyTo(uids);
                pendingReloadUids.Clear();
            }

            try
            {
                foreach (var uid in uids)
                {
                    try
                    {
                        ReloadBmsChartForUid(uid);
                    }
                    catch (Exception ex)
                    {
                        ModLogger.Error($"[HwaResourceManager.BmsWatcher] 재로드 중 오류: uid={uid}, {ex.Message}");
                    }
                }
            }
            finally
            {
                lock (bmsReloadLock)
                {
                    bmsReloadRunning = false;
                }
            }
        }

        /// <summary>
        /// 곡의 채보를 난이도별로 전부 다시 읽어 캐시를 갱신합니다.
        /// 칸마다 새 채보를 얻으면 교체하고, 채보 파일이 사라졌으면 비우고, 읽지 못했지만 파일은 남아 있으면
        /// 옛 채보를 유지한 채 잠시 뒤 다시 시도합니다(저장 도중일 수 있습니다). 합치는 규칙은 <see cref="BmsChartSet.MergeReload"/>에 있습니다.
        /// </summary>
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
            BmsChartSet freshCharts = LoadHwaBmsChartSet(songDir, manifest);

            BmsChartSet currentCharts;
            lock (cachedBmsCharts)
            {
                cachedBmsCharts.TryGetValue(uid, out currentCharts);
            }

            BmsChartSet merged = BmsChartSet.MergeReload(freshCharts, currentCharts, File.Exists, out bool keptOld);

            // 캐시에 있던 채보 파일이 모두 사라졌다면(삭제, 이름 변경) 옛 채보를 계속 쓰면 안 됩니다.
            if (merged.IsEmpty)
            {
                lock (cachedBmsCharts)
                {
                    cachedBmsCharts.Remove(uid);
                }
                ClearRetryCount(uid);
                ModLogger.Msg($"[HwaResourceManager.BmsWatcher] [{uid}] 채보 파일이 사라져 캐시에서 제거했습니다.");
                return false;
            }

            lock (cachedBmsCharts)
            {
                cachedBmsCharts[uid] = merged;
            }

            if (keptOld)
            {
                ModLogger.Warning($"[HwaResourceManager.BmsWatcher] [{uid}] 읽지 못한 채보가 있어 그 채보만 이전 것을 유지하고 잠시 뒤 다시 시도합니다: {merged.Describe()}");
                ScheduleRetry(uid);
                return false;
            }

            ClearRetryCount(uid);
            ModLogger.Msg($"[HwaResourceManager.BmsWatcher] ✅ [{uid}] BMS 실시간 재로드 성공: {merged.Describe()}");
            return true;
        }

        private static void ScheduleRetry(string uid)
        {
            bool giveUp;
            lock (bmsReloadLock)
            {
                reloadRetryCounts.TryGetValue(uid, out int attempts);
                giveUp = attempts >= BmsReloadMaxRetries;
                if (giveUp)
                {
                    reloadRetryCounts.Remove(uid);
                }
                else
                {
                    reloadRetryCounts[uid] = attempts + 1;
                }
            }

            if (giveUp)
            {
                ModLogger.Warning($"[HwaResourceManager.BmsWatcher] [{uid}] {BmsReloadMaxRetries}회 재시도 후에도 읽지 못해 이전 채보를 유지합니다.");
                return;
            }

            ScheduleBmsReload(uid, BmsReloadRetryDelay);
        }

        private static void ClearRetryCount(string uid)
        {
            lock (bmsReloadLock)
            {
                reloadRetryCounts.Remove(uid);
            }
        }
    }
}
