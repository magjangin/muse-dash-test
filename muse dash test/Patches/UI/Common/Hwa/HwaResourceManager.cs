using MelonLoader;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace muse_dash_test
{
    /// <summary>
    /// hwa/ 폴더의 하위 폴더별 커스텀 곡 설정(Manifest) 및 BMS 차트를 스캔하고 관리하는 리소스 캐시 매니저입니다.
    /// </summary>
    public static class HwaResourceManager
    {
        public static readonly string HwaFolderPath = Path.Combine(MelonLoader.Utils.MelonEnvironment.GameRootDirectory, "hwa");
        private static readonly Dictionary<string, HwaManifest> cachedManifests = new Dictionary<string, HwaManifest>();
        private static readonly Dictionary<string, BmsChart> cachedBmsCharts = new Dictionary<string, BmsChart>();
        private static readonly List<string> virtualUids = new List<string>();

        public static void PreloadHwaManifest()
        {
            try
            {
                MelonLogger.Msg("[HwaResourceManager] 모드 로드 시 manifest 선읽기 시작");
                cachedManifests.Clear();
                cachedBmsCharts.Clear();
                virtualUids.Clear();

                List<string> songDirs = new List<string>();

                if (Directory.Exists(HwaFolderPath))
                {
                    string[] subDirs = Directory.GetDirectories(HwaFolderPath);
                    if (subDirs != null && subDirs.Length > 0)
                    {
                        Array.Sort(subDirs, StringComparer.OrdinalIgnoreCase);
                        songDirs.AddRange(subDirs);
                    }

                    if (songDirs.Count == 0)
                    {
                        string[] rootTxts = Directory.GetFiles(HwaFolderPath, "*.txt", SearchOption.TopDirectoryOnly);
                        string[] rootBms = Directory.GetFiles(HwaFolderPath, "*.bms", SearchOption.TopDirectoryOnly);
                        if ((rootTxts != null && rootTxts.Length > 0) || (rootBms != null && rootBms.Length > 0))
                        {
                            songDirs.Add(HwaFolderPath);
                        }
                    }
                }

                if (songDirs.Count > 0)
                {
                    MelonLogger.Msg($"[HwaResourceManager] 총 {songDirs.Count}개의 하위 폴더/곡 폴더를 발견했습니다.");
                    for (int i = 0; i < songDirs.Count; i++)
                    {
                        string dir = songDirs[i];
                        string uid = CustomContentIds.CreateVirtualSongUid(i);

                        MelonLogger.Msg($"[HwaResourceManager] [{uid}] 매핑 시도: folder={dir}");
                        HwaManifest manifest = LoadHwaManifest(dir);
                        if (manifest == null)
                        {
                            string dirName = Path.GetFileName(dir);
                            if (string.Equals(dir, HwaFolderPath, StringComparison.OrdinalIgnoreCase))
                            {
                                dirName = "HwaRoot";
                            }
                            manifest = new HwaManifest
                            {
                                SourcePath = Path.Combine(dir, "info.txt"),
                                Title = dirName,
                                Artist = "Unknown",
                                LevelDesigner = "Hwa"
                            };
                            MelonLogger.Msg($"[HwaResourceManager] [{uid}] 설정 파일(info.txt)이 없어 폴백 설정을 생성했습니다.");
                        }

                        if (string.IsNullOrEmpty(manifest.SourcePath))
                        {
                            manifest.SourcePath = Path.Combine(dir, "info.txt");
                        }

                        BmsChart bmsChart = LoadHwaBmsChart(dir, manifest);

                        cachedManifests[uid] = manifest;
                        if (bmsChart != null)
                        {
                            cachedBmsCharts[uid] = bmsChart;
                            MelonLogger.Msg($"[HwaResourceManager] [{uid}] BMS 로드 성공: {HwaChartDiagnostics.DescribeBmsChart(bmsChart)}");
                        }
                        else
                        {
                            MelonLogger.Warning($"[HwaResourceManager] [{uid}] BMS 로드 실패 또는 파일 없음");
                        }

                        virtualUids.Add(uid);
                        MelonLogger.Msg($"[HwaResourceManager] [{uid}] 등록 완료: {HwaManifestLoader.DescribeManifest(manifest)}");
                    }
                }
                else
                {
                    MelonLogger.Msg("[HwaResourceManager] 하위 폴더가 발견되지 않았습니다. 테스트용 3개 슬롯(1999-0~2)을 기본 생성합니다.");
                    for (int i = 0; i < 3; i++)
                    {
                        string uid = CustomContentIds.CreateVirtualSongUid(i);
                        HwaManifest manifest = new HwaManifest
                        {
                            SourcePath = Path.Combine(HwaFolderPath, $"info.txt"),
                            Title = $"화영왕 {i}",
                            Artist = $"화영왕 {i}",
                            LevelDesigner = $"화영왕 {i}",
                            Difficulty1 = 2 + i,
                            Difficulty2 = 5 + i
                        };
                        cachedManifests[uid] = manifest;
                        virtualUids.Add(uid);
                    }
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[HwaResourceManager] 모드 로드 시 manifest 선읽기 실패: {ex}");
            }
        }

        public static bool TryGetCachedHwaManifest(string uid, out string description)
        {
            if (uid != null && cachedManifests.TryGetValue(uid, out var manifest))
            {
                description = HwaManifestLoader.DescribeManifest(manifest);
                return true;
            }

            description = string.Empty;
            return false;
        }

        public static bool TryGetCachedHwaSearchTerms(string uid, out string sourceUid, out string sourceTitle, out string sourceArtist, out string sourceAlbum, out string description)
        {
            sourceUid = null;
            sourceTitle = null;
            sourceArtist = null;
            sourceAlbum = null;

            if (uid == null || !cachedManifests.TryGetValue(uid, out var manifest))
            {
                description = string.Empty;
                return false;
            }

            sourceUid = manifest.Uid;
            sourceTitle = manifest.Title;
            sourceArtist = manifest.Artist;
            sourceAlbum = manifest.Album;
            description = HwaManifestLoader.DescribeManifest(manifest);
            return true;
        }

        public static bool TryGetCachedHwaPrimaryVirtualSong(string uid, out string title, out string artist, out string levelDesigner, out int diff1, out int diff2, out int diff3, out int diff4, out int diff5, out string description)
        {
            title = null;
            artist = null;
            levelDesigner = null;
            diff1 = 2;
            diff2 = 5;
            diff3 = 0;
            diff4 = 0;
            diff5 = 0;

            if (uid == null || !cachedManifests.TryGetValue(uid, out var manifest))
            {
                description = string.Empty;
                return false;
            }

            title = !string.IsNullOrWhiteSpace(manifest.CustomTitle) ? manifest.CustomTitle : manifest.Title;
            artist = !string.IsNullOrWhiteSpace(manifest.CustomArtist) ? manifest.CustomArtist : manifest.Artist;
            levelDesigner = manifest.LevelDesigner;
            diff1 = manifest.Difficulty1 ?? diff1;
            diff2 = manifest.Difficulty2 ?? diff2;
            diff3 = manifest.Difficulty3 ?? diff3;
            diff4 = manifest.Difficulty4 ?? diff4;
            diff5 = manifest.Difficulty5 ?? diff5;
            description = HwaManifestLoader.DescribeManifest(manifest);
            return true;
        }

        public static bool TryGetCachedHwaScene(string uid, out int scene)
        {
            scene = default;

            if (uid == null || !cachedManifests.TryGetValue(uid, out var manifest) || !manifest.Scene.HasValue)
            {
                return false;
            }

            scene = manifest.Scene.Value;
            return true;
        }

        public static bool TryGetCachedHwaBmsChart(string uid, out BmsChart chart, out string description)
        {
            chart = null;
            description = string.Empty;

            if (uid != null && cachedBmsCharts.TryGetValue(uid, out chart))
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

        public static List<string> GetVirtualUids()
        {
            return new List<string>(virtualUids);
        }

        private static HwaManifest LoadHwaManifest(string folderPath)
        {
            return HwaManifestLoader.LoadHwaManifest(folderPath);
        }

        private static string FindPreferredBmsFile(string folderPath, SearchOption searchOption)
        {
            try
            {
                string[] bmsFiles = Directory.GetFiles(folderPath, "*.bms", searchOption);
                if (bmsFiles == null || bmsFiles.Length == 0)
                {
                    return null;
                }

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
