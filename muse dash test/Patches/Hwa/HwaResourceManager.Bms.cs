using MelonLoader;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace muse_dash_test
{
    /// <summary>
    /// BMS 차트 파일 탐색 및 로드.
    /// </summary>
    public static partial class HwaResourceManager
    {
        /// <summary>
        /// 이 곡의 이 난이도에서 쓸 채보를 찾습니다. 따로 지정한 채보가 없는 난이도는 곡의 기본 채보를 씁니다.
        /// 난이도를 생략하는 오버로드를 두지 않습니다. 부르는 쪽이 어느 난이도인지 빠뜨리면 다른 난이도 채보가 조용히 쓰이기 때문입니다.
        /// </summary>
        public static bool TryGetCachedHwaBmsChart(string uid, int difficulty, out BmsChart chart)
        {
            chart = null;
            if (uid == null) return false;

            lock (cachedBmsCharts)
            {
                if (!cachedBmsCharts.TryGetValue(uid, out BmsChartSet charts) || charts == null)
                {
                    return false;
                }

                chart = charts.ForDifficulty(difficulty);
                return chart != null;
            }
        }

        public static bool TryGetCachedHwaBmsChart(string uid, int difficulty, out BmsChart chart, out string description)
        {
            if (TryGetCachedHwaBmsChart(uid, difficulty, out chart))
            {
                description = HwaChartDiagnostics.DescribeBmsChart(chart);
                return true;
            }

            description = string.Empty;
            return false;
        }

        /// <summary>가상 곡을 만들 때 숙주 곡에 실제로 있는 난이도를 기억해 둡니다. 복제본의 난이도 칸은 info.txt 값으로 덮어써서 나중에는 알 수 없습니다.</summary>
        public static void RememberHostMaps(string uid, int[] availableDifficulties)
        {
            if (uid == null) return;
            lock (hostMapsByUid)
            {
                hostMapsByUid[uid] = availableDifficulties;
            }
        }

        /// <summary>이 곡의 이 난이도에서 숙주 곡의 어느 난이도 차트 에셋을 불러올지 돌려줍니다(<see cref="BmsHostMaps.PickMap"/>).</summary>
        public static int ResolveHostMap(string uid, int difficulty)
        {
            if (uid == null) return difficulty;
            lock (hostMapsByUid)
            {
                return hostMapsByUid.TryGetValue(uid, out int[] available)
                    ? BmsHostMaps.PickMap(available, difficulty)
                    : difficulty;
            }
        }

        public static bool TryGetSongDirectory(string uid, out string songDir)
        {
            songDir = null;
            if (uid != null && cachedManifests.TryGetValue(uid, out var manifest))
            {
                if (!string.IsNullOrEmpty(manifest.SourcePath))
                {
                    songDir = Path.GetDirectoryName(manifest.SourcePath);
                    return true;
                }
            }
            return false;
        }

        private static HwaManifest LoadHwaManifest(string folderPath)
        {
            return HwaManifestLoader.LoadHwaManifest(folderPath);
        }

        /// <summary>
        /// 임시 파일이라 채보 후보에서 제외할지 판정합니다.
        /// 판정 규칙 본체는 게임에 의존하지 않는 <see cref="BmsFileNames.IsTempFile"/>에 있고,
        /// 로직 테스트(<c>BmsFileNamesTests</c>)로 검증됩니다.
        /// </summary>
        public static bool IsTempBmsFile(string filePath)
        {
            return BmsFileNames.IsTempFile(filePath);
        }

        private static string FindPreferredBmsFile(string folderPath, SearchOption searchOption, ICollection<string> excludedPaths)
        {
            try
            {
                string[] rawBmsFiles = Directory.GetFiles(folderPath, "*.bms", searchOption);
                if (rawBmsFiles == null || rawBmsFiles.Length == 0)
                {
                    return null;
                }

                var bmsFilesList = new List<string>();
                foreach (var f in rawBmsFiles)
                {
                    if (!IsTempBmsFile(f) && !excludedPaths.Contains(Path.GetFullPath(f)))
                    {
                        bmsFilesList.Add(f);
                    }
                }

                if (bmsFilesList.Count == 0)
                {
                    return null;
                }

                string[] bmsFiles = bmsFilesList.ToArray();
                Array.Sort(bmsFiles, StringComparer.OrdinalIgnoreCase);
                foreach (string file in bmsFiles)
                {
                    string fileName = Path.GetFileName(file);
                    if (string.Equals(fileName, "chart.bms", StringComparison.OrdinalIgnoreCase)
                        || string.Equals(fileName, "main.bms", StringComparison.OrdinalIgnoreCase)
                        || string.Equals(fileName, "test.bms", StringComparison.OrdinalIgnoreCase))
                    {
                        return file;
                    }
                }

                return bmsFiles[0];
            }
            catch (Exception ex)
            {
                ModLogger.Error($"[HwaResourceManager.Bms] BMS 탐색 실패: {ex}");
                return null;
            }
        }

        /// <summary>
        /// 곡 폴더의 채보를 모두 읽습니다. info.txt가 난이도별로 지정한 파일과, 지정이 없는 난이도가 쓸 기본 채보입니다.
        /// 읽을 수 있는 채보가 하나도 없으면 null입니다.
        /// </summary>
        private static BmsChartSet LoadHwaBmsChartSet(string folderPath, HwaManifest manifest)
        {
            if (string.IsNullOrWhiteSpace(folderPath) || !Directory.Exists(folderPath))
            {
                return null;
            }

            ModLogger.Verbose($"[HwaResourceManager.Bms] BMS 탐색 시작: folder={folderPath}");

            string[] configuredPaths = ResolveDifficultyBmsPaths(manifest);
            var claimedPaths = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (string path in configuredPaths)
            {
                if (path != null) claimedPaths.Add(path);
            }

            // 기본 채보는 난이도에 지정되지 않은 파일에서 먼저 고릅니다. easy.bms를 난이도1에 지정해 두고 hwa2.bms를 기본으로
            // 둔 폴더에서, 이름순으로 앞선다고 easy.bms가 다른 난이도의 기본 채보까지 되면 안 됩니다.
            string defaultPath = FindDefaultBmsFile(folderPath, manifest, claimedPaths)
                ?? FindDefaultBmsFile(folderPath, manifest, Array.Empty<string>());
            if (defaultPath == null)
            {
                ModLogger.Warning($"[HwaResourceManager.Bms] BMS 파일이 없습니다: folder={folderPath}");
                return null;
            }

            // 여러 난이도가 같은 파일을 가리켜도 한 번만 읽습니다.
            var parsedCharts = new Dictionary<string, BmsChart>(StringComparer.OrdinalIgnoreCase);
            var charts = new BmsChartSet(ParseBmsFileOnce(defaultPath, parsedCharts));
            for (int difficulty = 1; difficulty <= BmsChartSet.MaxDifficulty; difficulty++)
            {
                string path = configuredPaths[difficulty];
                if (path == null) continue;

                BmsChart chart = ParseBmsFileOnce(path, parsedCharts);
                if (chart == null)
                {
                    ModLogger.Warning($"[HwaResourceManager.Bms] 난이도{difficulty} 채보를 읽지 못해 기본 채보를 씁니다: {path}");
                    continue;
                }

                charts.SetForDifficulty(difficulty, chart);
            }

            return charts.IsEmpty ? null : charts;
        }

        /// <summary>
        /// info.txt의 난이도별 채보 이름을 전체 경로로 바꿉니다. 칸 번호가 난이도 번호입니다.
        /// 쓸 수 없는 값은 이유를 경고로 남기고 비워 둡니다(그 난이도는 기본 채보를 씁니다).
        /// </summary>
        private static string[] ResolveDifficultyBmsPaths(HwaManifest manifest)
        {
            var paths = new string[BmsChartSet.MaxDifficulty + 1];
            if (manifest == null || string.IsNullOrWhiteSpace(manifest.SourcePath))
            {
                return paths;
            }

            string manifestDir = Path.GetDirectoryName(manifest.SourcePath);
            for (int difficulty = 1; difficulty <= BmsChartSet.MaxDifficulty; difficulty++)
            {
                string configured = manifest.DifficultyBmsFiles[difficulty];
                if (string.IsNullOrWhiteSpace(configured)) continue;

                if (!BmsFileNames.TryResolveInSongFolder(manifestDir, configured, out string path))
                {
                    ModLogger.Warning($"[HwaResourceManager.Bms] 난이도{difficulty} bms 값을 쓸 수 없어 기본 채보를 씁니다. 곡 폴더 안의 .bms 파일이어야 합니다: '{configured}' ({manifest.SourcePath})");
                    continue;
                }

                if (!File.Exists(path))
                {
                    ModLogger.Warning($"[HwaResourceManager.Bms] 난이도{difficulty} bms 파일이 없어 기본 채보를 씁니다: {path}");
                    continue;
                }

                if (!manifest.GetDifficultyLevel(difficulty).HasValue)
                {
                    ModLogger.Warning($"[HwaResourceManager.Bms] 난이도{difficulty} bms는 있지만 '난이도{difficulty}' 숫자가 없어 곡 선택 화면에 이 난이도가 나오지 않을 수 있습니다: {manifest.SourcePath}");
                }

                paths[difficulty] = path;
            }

            return paths;
        }

        private static string FindDefaultBmsFile(string folderPath, HwaManifest manifest, ICollection<string> excludedPaths)
        {
            string preferred = null;
            if (manifest != null && !string.IsNullOrWhiteSpace(manifest.SourcePath))
            {
                string manifestDir = Path.GetDirectoryName(manifest.SourcePath);
                if (!string.IsNullOrWhiteSpace(manifestDir) && Directory.Exists(manifestDir))
                {
                    preferred = FindPreferredBmsFile(manifestDir, SearchOption.TopDirectoryOnly, excludedPaths);
                }
            }

            if (string.IsNullOrWhiteSpace(preferred))
            {
                preferred = FindPreferredBmsFile(folderPath, SearchOption.AllDirectories, excludedPaths);
            }

            return !string.IsNullOrWhiteSpace(preferred) && File.Exists(preferred) ? preferred : null;
        }

        private static BmsChart ParseBmsFileOnce(string path, Dictionary<string, BmsChart> parsedCharts)
        {
            string key = Path.GetFullPath(path);
            if (parsedCharts.TryGetValue(key, out BmsChart parsed))
            {
                return parsed;
            }

            BmsChart chart = ParseBmsFile(path);
            parsedCharts[key] = chart;
            return chart;
        }

        private static BmsChart ParseBmsFile(string path)
        {
            try
            {
                ModLogger.Verbose($"[HwaResourceManager.Bms] BMS 읽기 대상: {path}");
                var parseTimer = Stopwatch.StartNew();
                var chart = BmsParser.ParseFile(path);
                parseTimer.Stop();
                if (ModLogger.IsLevelEnabled(ModLogLevel.Verbose))
                {
                    ModLogger.Verbose($"[HwaResourceManager.Bms] BMS 파싱 완료: elapsed={parseTimer.ElapsedMilliseconds}ms, {HwaChartDiagnostics.DescribeBmsChart(chart)}");
                    HwaChartDiagnostics.LogBmsWavMappingSummary(chart);
                }
                return chart;
            }
            catch (Exception ex)
            {
                ModLogger.Error($"[HwaResourceManager.Bms] BMS 읽기 실패: {path}, {ex}");
                return null;
            }
        }
    }
}
