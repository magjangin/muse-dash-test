using MelonLoader;
using System;
using System.Collections.Generic;
using System.IO;

namespace muse_dash_test
{
    /// <summary>
    /// hwa/ 폴더의 하위 폴더별 커스텀 곡 설정(Manifest) 및 BMS 차트를 스캔하고 관리하는 리소스 캐시 매니저입니다.
    /// 책임별로 다음 partial 파일들로 분리되어 있습니다:
    ///   - HwaResourceManager.Bms.cs     : BMS 차트 탐색/로드
    ///   - HwaResourceManager.Watcher.cs : BMS 파일 변경 실시간 감지/재로드
    /// </summary>
    public static partial class HwaResourceManager
    {
        public static readonly string HwaFolderPath = Path.Combine(MelonLoader.Utils.MelonEnvironment.GameRootDirectory, "hwa");
        private static readonly Dictionary<string, HwaManifest> cachedManifests = new Dictionary<string, HwaManifest>();
        private static readonly Dictionary<string, BmsChart> cachedBmsCharts = new Dictionary<string, BmsChart>();
        private static readonly List<string> virtualUids = new List<string>();

        // 커스텀으로 "주장된" 모든 uid 집합. 가상 uid(1999-N)와, 매니페스트가 숙주로 지정한
        // 순정 uid(info.txt의 uid: 값, 예: 66-0)를 함께 담아 곡 단위 커스텀 판정의 단일 출처로 씁니다.
        private static readonly HashSet<string> customClaimedUids = new HashSet<string>(StringComparer.Ordinal);

        public static void PreloadHwaManifest()
        {
            try
            {
                MelonLogger.Msg("[HwaResourceManager] 모드 로드 시 manifest 선읽기 시작");
                ClearCaches();

                List<string> songDirs = DiscoverSongDirectories();
                if (songDirs.Count > 0)
                {
                    MelonLogger.Msg($"[HwaResourceManager] 총 {songDirs.Count}개의 하위 폴더/곡 폴더를 발견했습니다.");
                    for (int i = 0; i < songDirs.Count; i++)
                    {
                        string uid = CustomContentIds.CreateVirtualSongUid(i);
                        RegisterSongDirectory(uid, songDirs[i]);
                    }
                }
                else
                {
                    GenerateTestSlots();
                }

                InitializeBmsWatcher();
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[HwaResourceManager] 모드 로드 시 manifest 선읽기 실패: {ex}");
            }
        }

        private static void ClearCaches()
        {
            cachedManifests.Clear();
            lock (cachedBmsCharts)
            {
                cachedBmsCharts.Clear();
            }
            virtualUids.Clear();
            customClaimedUids.Clear();
        }

        /// <summary>
        /// hwa/ 아래 곡 폴더 목록을 찾습니다. 하위 폴더가 없고 루트에 직접 txt/bms가 있으면 hwa/ 자체를 단일 곡으로 취급합니다.
        /// </summary>
        private static List<string> DiscoverSongDirectories()
        {
            var songDirs = new List<string>();
            if (!Directory.Exists(HwaFolderPath))
            {
                return songDirs;
            }

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

            return songDirs;
        }

        /// <summary>
        /// 단일 곡 폴더의 manifest(없으면 폴백)와 BMS 차트를 로드해 캐시에 등록합니다.
        /// </summary>
        private static void RegisterSongDirectory(string uid, string dir)
        {
            MelonLogger.Msg($"[HwaResourceManager] [{uid}] 매핑 시도: folder={dir}");

            HwaManifest manifest = LoadHwaManifest(dir) ?? CreateFallbackManifest(uid, dir);
            if (string.IsNullOrEmpty(manifest.SourcePath))
            {
                manifest.SourcePath = Path.Combine(dir, "info.txt");
            }

            BmsChart bmsChart = LoadHwaBmsChart(dir, manifest);

            cachedManifests[uid] = manifest;
            if (bmsChart != null)
            {
                lock (cachedBmsCharts)
                {
                    cachedBmsCharts[uid] = bmsChart;
                }
                MelonLogger.Msg($"[HwaResourceManager] [{uid}] BMS 로드 성공: {HwaChartDiagnostics.DescribeBmsChart(bmsChart)}");
            }
            else
            {
                MelonLogger.Warning($"[HwaResourceManager] [{uid}] BMS 로드 실패 또는 파일 없음");
            }

            virtualUids.Add(uid);
            customClaimedUids.Add(uid);

            // 매니페스트가 순정 슬롯을 숙주로 지정했다면(uid: 66-0 등) 그 uid도 커스텀으로 인식되게 등록.
            if (!string.IsNullOrWhiteSpace(manifest.Uid))
            {
                customClaimedUids.Add(manifest.Uid.Trim());
                MelonLogger.Msg($"[HwaResourceManager] [{uid}] 숙주 uid '{manifest.Uid.Trim()}'를 커스텀 곡으로 등록했습니다.");
            }

            MelonLogger.Msg($"[HwaResourceManager] [{uid}] 등록 완료: {HwaManifestLoader.DescribeManifest(manifest)}");
        }

        private static HwaManifest CreateFallbackManifest(string uid, string dir)
        {
            string dirName = Path.GetFileName(dir);
            if (string.Equals(dir, HwaFolderPath, StringComparison.OrdinalIgnoreCase))
            {
                dirName = "HwaRoot";
            }

            MelonLogger.Msg($"[HwaResourceManager] [{uid}] 설정 파일(info.txt)이 없어 폴백 설정을 생성했습니다.");
            return new HwaManifest
            {
                SourcePath = Path.Combine(dir, "info.txt"),
                Title = dirName,
                Artist = "Unknown",
                LevelDesigner = "Hwa"
            };
        }

        /// <summary>
        /// hwa/ 폴더가 비어 있을 때 동작 확인용 가상 곡 3개(1999-1~3)를 생성합니다.
        /// </summary>
        private static void GenerateTestSlots()
        {
            MelonLogger.Msg("[HwaResourceManager] 하위 폴더가 발견되지 않았습니다. 테스트용 3개 슬롯(1999-1~3)을 기본 생성합니다.");
            for (int i = 0; i < 3; i++)
            {
                string uid = CustomContentIds.CreateVirtualSongUid(i);
                cachedManifests[uid] = new HwaManifest
                {
                    SourcePath = Path.Combine(HwaFolderPath, "info.txt"),
                    Title = $"화영왕 {i + 1}",
                    Artist = $"화영왕 {i + 1}",
                    LevelDesigner = $"화영왕 {i + 1}",
                    Difficulty1 = 2 + i,
                    Difficulty2 = 5 + i
                };
                virtualUids.Add(uid);
                customClaimedUids.Add(uid);
            }
        }

        /// <summary>
        /// 가상 uid(1999-N) 또는 매니페스트가 숙주로 지정한 순정 uid(예: 66-0)면 true를 반환합니다.
        /// 이 값은 "등록 여부"만 뜻합니다. 실제 플레이에 커스텀 차트를 적용할지는 선택 문맥까지
        /// 포함해서 <see cref="ShouldApplyCustomChartForSelection"/>로 판단해야 합니다.
        /// </summary>
        public static bool IsCustomSong(string uid)
        {
            if (string.IsNullOrEmpty(uid)) return false;
            // 매니페스트가 아직 로드되기 전이라도 1999- 가상곡은 항상 커스텀으로 간주(하위 호환).
            return CustomContentIds.IsVirtualSong(uid) || customClaimedUids.Contains(uid);
        }

        public static bool IsRegisteredCustomHostUid(string uid)
        {
            if (string.IsNullOrEmpty(uid)) return false;
            return !CustomContentIds.IsVirtualSong(uid) && customClaimedUids.Contains(uid);
        }

        public static bool ShouldApplyCustomChartForSelection(string uid, bool isExperimentModeActive)
        {
            if (string.IsNullOrEmpty(uid)) return false;
            if (CustomContentIds.IsVirtualSong(uid)) return true;

            // 순정 uid를 숙주로 쓰는 경우에도 공식 앨범에서 같은 uid를 고르면 원본 채보가 살아야 합니다.
            // 그래서 숙주 uid는 실험 모드/커스텀 태그 문맥일 때만 커스텀 차트 적용 대상으로 봅니다.
            return isExperimentModeActive && IsRegisteredCustomHostUid(uid);
        }

        public static HwaManifest GetManifest(string uid)
        {
            if (uid != null && cachedManifests.TryGetValue(uid, out var manifest))
            {
                return manifest;
            }
            return null;
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

        public static bool TryGetHwaPrimarySong(string uid, out string title, out string artist, out string levelDesigner, out int diff1, out int diff2, out int diff3, out int diff4, out int diff5, out string description)
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
            bool anyDiffSpecified = manifest.Difficulty1.HasValue ||
                                    manifest.Difficulty2.HasValue ||
                                    manifest.Difficulty3.HasValue ||
                                    manifest.Difficulty4.HasValue ||
                                    manifest.Difficulty5.HasValue;

            if (anyDiffSpecified)
            {
                diff1 = manifest.Difficulty1 ?? 0;
                diff2 = manifest.Difficulty2 ?? 0;
                diff3 = manifest.Difficulty3 ?? 0;
                diff4 = manifest.Difficulty4 ?? 0;
                diff5 = manifest.Difficulty5 ?? 0;
            }
            else
            {
                diff1 = 2;
                diff2 = 5;
                diff3 = 0;
                diff4 = 0;
                diff5 = 0;
            }
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

        public static List<string> GetVirtualUids()
        {
            return new List<string>(virtualUids);
        }
    }
}
