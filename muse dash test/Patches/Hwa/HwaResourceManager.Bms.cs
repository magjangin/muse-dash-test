using MelonLoader;
using System;
using System.Diagnostics;
using System.IO;

namespace muse_dash_test
{
    /// <summary>
    /// BMS 차트 파일 탐색 및 로드.
    /// </summary>
    public static partial class HwaResourceManager
    {
        public static bool TryGetCachedHwaBmsChart(string uid, out BmsChart chart, out string description)
        {
            chart = null;
            description = string.Empty;

            bool found = false;
            lock (cachedBmsCharts)
            {
                if (uid != null)
                {
                    found = cachedBmsCharts.TryGetValue(uid, out chart);
                }
            }

            if (found && chart != null)
            {
                description = HwaChartDiagnostics.DescribeBmsChart(chart);
                return true;
            }

            return false;
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

        public static bool IsTempBmsFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) return true;
            string fileName = Path.GetFileName(filePath);
            return fileName.StartsWith("~", StringComparison.OrdinalIgnoreCase)
                || fileName.StartsWith("___", StringComparison.OrdinalIgnoreCase)
                || fileName.IndexOf("temp", StringComparison.OrdinalIgnoreCase) >= 0
                || fileName.IndexOf("tmp", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static string FindPreferredBmsFile(string folderPath, SearchOption searchOption)
        {
            try
            {
                string[] rawBmsFiles = Directory.GetFiles(folderPath, "*.bms", searchOption);
                if (rawBmsFiles == null || rawBmsFiles.Length == 0)
                {
                    return null;
                }

                var bmsFilesList = new System.Collections.Generic.List<string>();
                foreach (var f in rawBmsFiles)
                {
                    if (!IsTempBmsFile(f))
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
                MelonLogger.Error($"[HwaResourceManager.Bms] BMS 탐색 실패: {ex}");
                return null;
            }
        }

        private static BmsChart LoadHwaBmsChart(string folderPath, HwaManifest manifest)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(folderPath) || !Directory.Exists(folderPath))
                {
                    return null;
                }

                MelonLogger.Msg($"[HwaResourceManager.Bms] BMS 탐색 시작: folder={folderPath}");

                string preferred = null;
                if (manifest != null && !string.IsNullOrWhiteSpace(manifest.SourcePath))
                {
                    string manifestDir = Path.GetDirectoryName(manifest.SourcePath);
                    if (!string.IsNullOrWhiteSpace(manifestDir) && Directory.Exists(manifestDir))
                    {
                        preferred = FindPreferredBmsFile(manifestDir, SearchOption.TopDirectoryOnly);
                    }
                }

                if (string.IsNullOrWhiteSpace(preferred))
                {
                    preferred = FindPreferredBmsFile(folderPath, SearchOption.AllDirectories);
                }

                if (string.IsNullOrWhiteSpace(preferred) || !File.Exists(preferred))
                {
                    MelonLogger.Msg($"[HwaResourceManager.Bms] BMS 파일이 없습니다: folder={folderPath}");
                    return null;
                }

                MelonLogger.Msg($"[HwaResourceManager.Bms] BMS 읽기 대상: {preferred}");
                var parseTimer = Stopwatch.StartNew();
                var chart = BmsParser.ParseFile(preferred);
                parseTimer.Stop();
                MelonLogger.Msg($"[HwaResourceManager.Bms] BMS 파싱 완료: elapsed={parseTimer.ElapsedMilliseconds}ms, {HwaChartDiagnostics.DescribeBmsChart(chart)}");
                HwaChartDiagnostics.LogBmsWavMappingSummary(chart);
                return chart;
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[HwaResourceManager.Bms] BMS 읽기 실패: {ex}");
                return null;
            }
        }
    }
}
