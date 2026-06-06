using MelonLoader;
using System;
using System.Collections.Generic;
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

        public class HwaManifest
        {
            public string SourcePath;
            public string Uid;
            public string Title;
            public string CustomTitle;
            public string Artist;
            public string CustomArtist;
            public string LevelDesigner;
            public string Album;
            public int? Scene;
            public int? Difficulty1;
            public int? Difficulty2;
            public int? Difficulty3;
            public int? Difficulty4;
            public int? Difficulty5;
        }

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
                        string uid = $"1000-{i}";

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
                            MelonLogger.Msg($"[HwaResourceManager] [{uid}] BMS 로드 성공: {DescribeBmsChart(bmsChart)}");
                        }
                        else
                        {
                            MelonLogger.Warning($"[HwaResourceManager] [{uid}] BMS 로드 실패 또는 파일 없음");
                        }

                        virtualUids.Add(uid);
                        MelonLogger.Msg($"[HwaResourceManager] [{uid}] 등록 완료: {DescribeManifest(manifest)}");
                    }
                }
                else
                {
                    MelonLogger.Msg("[HwaResourceManager] 하위 폴더가 발견되지 않았습니다. 테스트용 3개 슬롯(1000-0~2)을 기본 생성합니다.");
                    for (int i = 0; i < 3; i++)
                    {
                        string uid = $"1000-{i}";
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
                description = DescribeManifest(manifest);
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
            description = DescribeManifest(manifest);
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
            description = DescribeManifest(manifest);
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
                description = DescribeBmsChart(chart);
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
            try
            {
                MelonLogger.Msg($"[HwaResourceManager] manifest 탐색 시작: folder={folderPath}");

                string[] txtFiles = Directory.GetFiles(folderPath, "*.txt", SearchOption.AllDirectories);
                if (txtFiles == null || txtFiles.Length == 0)
                {
                    MelonLogger.Msg($"[HwaResourceManager] 하위 폴더까지 스캔했지만 txt 파일이 없습니다: folder={folderPath}");
                    return null;
                }

                MelonLogger.Msg($"[HwaResourceManager] txt 파일 {txtFiles.Length}개 발견(하위 폴더 포함): {string.Join(", ", Array.ConvertAll(txtFiles, file => GetRelativeHwaPath(folderPath, file)))}");

                Array.Sort(txtFiles, StringComparer.OrdinalIgnoreCase);
                string preferred = null;
                foreach (var file in txtFiles)
                {
                    string fileName = Path.GetFileName(file);
                    if (string.Equals(fileName, "info.txt", StringComparison.OrdinalIgnoreCase) || string.Equals(fileName, "info1.txt", StringComparison.OrdinalIgnoreCase))
                    {
                        preferred = file;
                        break;
                    }

                    if (preferred == null)
                    {
                        preferred = file;
                    }
                }

                if (string.IsNullOrWhiteSpace(preferred) || !File.Exists(preferred))
                {
                    MelonLogger.Msg($"[HwaResourceManager] 선택할 txt 파일이 없습니다: folder={folderPath}");
                    return null;
                }

                MelonLogger.Msg($"[HwaResourceManager] manifest 읽기 대상: {preferred}");

                var manifest = new HwaManifest { SourcePath = preferred };
                foreach (var rawLine in File.ReadAllLines(preferred))
                {
                    if (TryParseManifestLine(rawLine, out string key, out string value))
                    {
                        MelonLogger.Msg($"[HwaResourceManager] manifest line parsed: key={key}, value={value}");
                        ApplyManifestValue(manifest, key, value);
                    }
                }

                if (string.IsNullOrWhiteSpace(manifest.Uid) && string.IsNullOrWhiteSpace(manifest.Title) && string.IsNullOrWhiteSpace(manifest.Artist))
                {
                    MelonLogger.Msg($"[HwaResourceManager] manifest 파싱은 했지만 핵심 값이 비어 있습니다: {DescribeManifest(manifest)}");
                    return null;
                }

                MelonLogger.Msg($"[HwaResourceManager] manifest 파싱 완료: {DescribeManifest(manifest)}");

                return manifest;
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[HwaResourceManager] manifest 읽기 실패: {ex}");
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
                var chart = BmsParser.ParseFile(preferred);
                MelonLogger.Msg($"[HwaResourceManager.Bms] BMS 파싱 완료: {DescribeBmsChart(chart)}");
                LogBmsWavMappingSummary(chart);
                return chart;
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[HwaResourceManager.Bms] BMS 읽기 실패: {ex}");
                return null;
            }
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

        private static bool TryParseManifestLine(string rawLine, out string key, out string value)
        {
            key = null;
            value = null;

            if (string.IsNullOrWhiteSpace(rawLine))
            {
                return false;
            }

            string line = rawLine.Trim();
            if (line.StartsWith("//"))
            {
                line = line.Substring(2).Trim();
            }

            if (string.IsNullOrWhiteSpace(line))
            {
                return false;
            }

            int separatorIndex = line.IndexOf(':');
            if (separatorIndex < 0)
            {
                separatorIndex = line.IndexOf('=');
            }
            if (separatorIndex < 0)
            {
                separatorIndex = line.IndexOf('：');
            }

            if (separatorIndex < 0)
            {
                return false;
            }

            key = line.Substring(0, separatorIndex).Trim();
            value = line.Substring(separatorIndex + 1).Trim();
            return !string.IsNullOrWhiteSpace(key) && !string.IsNullOrWhiteSpace(value);
        }

        private static string GetRelativeHwaPath(string rootPath, string filePath)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(rootPath) || string.IsNullOrWhiteSpace(filePath))
                {
                    return Path.GetFileName(filePath);
                }

                string root = Path.GetFullPath(rootPath).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar) + Path.DirectorySeparatorChar;
                string full = Path.GetFullPath(filePath);
                if (full.StartsWith(root, StringComparison.OrdinalIgnoreCase))
                {
                    return full.Substring(root.Length);
                }
            }
            catch
            {
            }

            return Path.GetFileName(filePath);
        }

        private static string DescribeBmsChart(BmsChart chart)
        {
            if (chart == null)
            {
                return "(null)";
            }

            int metadataCount = chart.Metadata != null ? chart.Metadata.Count : 0;
            int wavCount = CountBmsWavMetadata(chart);
            int noteCount = chart.Notes != null ? chart.Notes.Count : 0;
            int bpmCount = chart.BpmChanges != null ? chart.BpmChanges.Count : 0;
            int swapCount = 0;
            try
            {
                swapCount = BmsBossSwapPlanner.BuildSwapEvents(chart).Count;
            }
            catch
            {
                swapCount = -1;
            }

            return "path=" + (chart.SourcePath ?? "(null)")
                + ", title=" + (chart.Title ?? "(null)")
                + ", artist=" + (chart.Artist ?? "(null)")
                + ", defaultBpm=" + chart.DefaultBpm.ToString("0.###", System.Globalization.CultureInfo.InvariantCulture)
                + ", notes=" + noteCount
                + ", bpmChanges=" + bpmCount
                + ", metadata=" + metadataCount
                + ", wav=" + wavCount
                + ", bossSwapCandidates=" + swapCount;
        }

        private static int CountBmsWavMetadata(BmsChart chart)
        {
            if (chart?.Metadata == null)
            {
                return 0;
            }

            int count = 0;
            foreach (var key in chart.Metadata.Keys)
            {
                if (!string.IsNullOrWhiteSpace(key) && key.StartsWith("WAV", StringComparison.OrdinalIgnoreCase))
                {
                    count++;
                }
            }

            return count;
        }

        private static void LogBmsWavMappingSummary(BmsChart chart)
        {
            if (chart?.Notes == null || chart.Notes.Count == 0)
            {
                MelonLogger.Msg("[HwaResourceManager.Bms] 노트가 없어 WAV 매핑 샘플을 건너뜁니다.");
                return;
            }

            int logged = 0;
            var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var note in chart.Notes)
            {
                if (note == null || string.IsNullOrWhiteSpace(note.RawValue) || !seen.Add(note.RawValue))
                {
                    continue;
                }

                var wavInfo = BmsBossSwapPlanner.ResolveWavInfo(chart, note);
                if (wavInfo == null)
                {
                    MelonLogger.Msg($"[HwaResourceManager.Bms] WAV 매핑 샘플: raw={note.RawValue}, wav=(null)");
                }
                else
                {
                    MelonLogger.Msg($"[HwaResourceManager.Bms] WAV 매핑 샘플: raw={note.RawValue}, wav={wavInfo.RawWavName}, uid={wavInfo.Uid ?? "(null)"}, type={wavInfo.NoteType}, prefab={wavInfo.PrefabName ?? "(null)"}, dt={wavInfo.Dt}, keyAudio={wavInfo.KeyAudio ?? "(null)"}, bossAction={wavInfo.BossAction ?? "(null)"}");
                }

                logged++;
                if (logged >= 12)
                {
                    break;
                }
            }

            var swapEvents = BmsBossSwapPlanner.BuildSwapEvents(chart);
            for (int i = 0; i < swapEvents.Count && i < 5; i++)
            {
                var evt = swapEvents[i];
                MelonLogger.Msg($"[HwaResourceManager.Bms] 보스 스왑 후보 #{i + 1}: outTick={evt.OutNote?.Tick}, inTick={evt.InNote?.Tick}, delay={evt.DelaySeconds:0.###}s, action={evt.BossAction}");
            }
        }

        private static void ApplyManifestValue(HwaManifest manifest, string key, string value)
        {
            if (manifest == null || string.IsNullOrWhiteSpace(key) || string.IsNullOrWhiteSpace(value))
            {
                return;
            }

            string normalizedKey = NormalizeManifestKey(key);
            if (normalizedKey.Contains("가져올앨범") || normalizedKey.Contains("앨범") || normalizedKey.Contains("album"))
            {
                manifest.Album = value;
                return;
            }

            if (normalizedKey.Contains("uid"))
            {
                manifest.Uid = value;
                return;
            }

            if (normalizedKey.Contains("커스텀아티스트") || normalizedKey.Contains("customartist") || normalizedKey.Contains("customauthor"))
            {
                manifest.CustomArtist = value;
                return;
            }

            if (normalizedKey.Contains("레벨디자이너") || normalizedKey.Contains("leveldesigner"))
            {
                manifest.LevelDesigner = value;
                return;
            }

            if (normalizedKey.Contains("artist") || normalizedKey.Contains("아티스트") || normalizedKey.Contains("author"))
            {
                manifest.Artist = value;
                return;
            }

            if (normalizedKey.Contains("커스텀곡제목") || normalizedKey.Contains("customsongtitle") || normalizedKey.Contains("customtitle"))
            {
                manifest.CustomTitle = value;
                return;
            }

            if (normalizedKey.Contains("곡이름") || normalizedKey.Contains("곡명") || normalizedKey.Contains("곡제목") || normalizedKey.Contains("가져올곡") || normalizedKey.Contains("song") || normalizedKey.Contains("title") || normalizedKey.Contains("music"))
            {
                manifest.Title = value;
                return;
            }

            if (normalizedKey.Contains("씬번호") || normalizedKey.Contains("scene"))
            {
                manifest.Scene = TryParseNullableInt(value);
                return;
            }

            if (normalizedKey.Contains("난이도1") || normalizedKey.Contains("difficulty1"))
            {
                manifest.Difficulty1 = TryParseNullableInt(value);
                return;
            }

            if (normalizedKey.Contains("난이도2") || normalizedKey.Contains("difficulty2"))
            {
                manifest.Difficulty2 = TryParseNullableInt(value);
                return;
            }

            if (normalizedKey.Contains("난이도3") || normalizedKey.Contains("difficulty3"))
            {
                manifest.Difficulty3 = TryParseNullableInt(value);
                return;
            }

            if (normalizedKey.Contains("난이도4") || normalizedKey.Contains("difficulty4"))
            {
                manifest.Difficulty4 = TryParseNullableInt(value);
                return;
            }

            if (normalizedKey.Contains("난이도5") || normalizedKey.Contains("difficulty5"))
            {
                manifest.Difficulty5 = TryParseNullableInt(value);
                return;
            }
        }

        private static string NormalizeManifestKey(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return string.Empty;
            }

            var builder = new System.Text.StringBuilder(text.Length);
            foreach (char ch in text)
            {
                if (char.IsWhiteSpace(ch) || ch == '-' || ch == '_' || ch == '/' || ch == '·' || ch == '.' || ch == '(' || ch == ')' || ch == '[' || ch == ']')
                {
                    continue;
                }

                builder.Append(char.ToLowerInvariant(ch));
            }

            return builder.ToString();
        }

        private static int? TryParseNullableInt(string value)
        {
            if (string.IsNullOrWhiteSpace(value) || value.Trim().Equals("null", StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            if (int.TryParse(value.Trim(), out int parsed))
            {
                return parsed;
            }

            return null;
        }

        private static string DescribeManifest(HwaManifest manifest)
        {
            if (manifest == null)
            {
                return "(null)";
            }

            return "path=" + (manifest.SourcePath ?? "(null)")
                + ", uid=" + (manifest.Uid ?? "(null)")
                + ", title=" + (manifest.Title ?? "(null)")
                + ", customTitle=" + (manifest.CustomTitle ?? "(null)")
                + ", artist=" + (manifest.Artist ?? "(null)")
                + ", customArtist=" + (manifest.CustomArtist ?? "(null)")
                + ", levelDesigner=" + (manifest.LevelDesigner ?? "(null)")
                + ", album=" + (manifest.Album ?? "(null)")
                + ", scene=" + (manifest.Scene.HasValue ? manifest.Scene.Value.ToString() : "(null)")
                + ", diff1=" + (manifest.Difficulty1.HasValue ? manifest.Difficulty1.Value.ToString() : "(null)")
                + ", diff2=" + (manifest.Difficulty2.HasValue ? manifest.Difficulty2.Value.ToString() : "(null)")
                + ", diff3=" + (manifest.Difficulty3.HasValue ? manifest.Difficulty3.Value.ToString() : "(null)")
                + ", diff4=" + (manifest.Difficulty4.HasValue ? manifest.Difficulty4.Value.ToString() : "(null)")
                + ", diff5=" + (manifest.Difficulty5.HasValue ? manifest.Difficulty5.Value.ToString() : "(null)");
        }
    }
}
