using MelonLoader;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Video;

[assembly: MelonInfo(typeof(muse_dash_test.MainMod), "muse-dash-test", "0.1.0", "화영왕")]
[assembly: MelonGame("PeroPeroGames", "MuseDash")]

namespace muse_dash_test
{
    public class MainMod : MelonMod
    {

        private static readonly string HwaFolderPath = Path.Combine(MelonLoader.Utils.MelonEnvironment.GameRootDirectory, "hwa");
        private static HwaManifest cachedManifest;
        private static BmsChart cachedBmsChart;

        private static readonly HywStageManager hywStageManager = new HywStageManager();
        private static float hywCheckTimer = 0f;
        private const float HywCheckInterval = 0.1f;
        private static float syncCooldownTimer = 0f;

        private sealed class HwaManifest
        {
            public string SourcePath;
            public string Uid;
            public string Title;
            public string CustomTitle;
            public string Artist;
            public string CustomArtist;
            public string LevelDesigner;
            public int? Scene;
            public int? Difficulty1;
            public int? Difficulty2;
            public int? Difficulty3;
            public int? Difficulty4;
            public int? Difficulty5;
        }

        public override void OnInitializeMelon()
        {
            MelonLogger.Msg("모드가 로드되었습니다.");
            MelonLogger.Msg("HywHpTextMod - 체력바 텍스트 모드가 성공적으로 연동 활성화되었습니다!");

            try
            {
                Directory.CreateDirectory(HwaFolderPath);
                MelonLogger.Msg($"hwa 폴더를 확인/생성했습니다: {HwaFolderPath}");
                
                string albumDumpPath = Path.Combine(HwaFolderPath, "album_tag_dump.txt");
                if (File.Exists(albumDumpPath)) File.Delete(albumDumpPath);
                
                string albumDumpMdPath = Path.Combine(HwaFolderPath, "album_tag_dump.md");
                if (File.Exists(albumDumpMdPath)) File.Delete(albumDumpMdPath);
                
                string musicDumpPath = Path.Combine(HwaFolderPath, "music_info_dump.txt");
                if (File.Exists(musicDumpPath)) File.Delete(musicDumpPath);

                // hwa tag image 폴더 생성
                string hwaTagImageFolderPath = Path.Combine(MelonLoader.Utils.MelonEnvironment.GameRootDirectory, "hwa tag image");
                Directory.CreateDirectory(hwaTagImageFolderPath);
                MelonLogger.Msg($"hwa tag image 폴더를 확인/생성했습니다: {hwaTagImageFolderPath}");

                // tag_icon.png 추출
                string pngPath = Path.Combine(hwaTagImageFolderPath, "tag_icon.png");
                if (!File.Exists(pngPath))
                {
                    try
                    {
                        var assembly = System.Reflection.Assembly.GetExecutingAssembly();
                        string resourceName = "muse_dash_test.Resources.tag_icon.png";
                        using (var stream = assembly.GetManifestResourceStream(resourceName))
                        {
                            if (stream != null)
                            {
                                byte[] fileData = new byte[stream.Length];
                                stream.Read(fileData, 0, fileData.Length);
                                File.WriteAllBytes(pngPath, fileData);
                                MelonLogger.Msg($"[APMod.TagIcon] OnInitializeMelon: 내장 리소스 '{resourceName}'를 '{pngPath}'에 복사 및 추출 완료!");
                            }
                            else
                            {
                                MelonLogger.Error($"[APMod.TagIcon] OnInitializeMelon: 추출할 내장 리소스를 찾을 수 없습니다: {resourceName}");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MelonLogger.Error($"[APMod.TagIcon] OnInitializeMelon: 내장 리소스 추출 중 예외 발생: {ex}");
                    }
                }

                PreloadHwaManifest();
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"hwa 폴더 생성 및 초기화 중 예외: {ex}");
            }

            try
            {
                AlbumTagToggle_Init_Patch.PatchResourcesManager(this.HarmonyInstance);
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[APMod] ResourcesManager 패치 등록 실패: {ex}");
            }
        }



        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            MelonLogger.Msg($"씬이 로드되었습니다: {sceneName} (빌드 인덱스: {buildIndex})");
        }

        public override void OnUpdate()
        {
            try
            {
                var pnl = Il2CppAssets.Scripts.UI.Panels.PnlBattle.instance;
                if (pnl != null && pnl.CurrentBattleUIComp != null)
                {
                    var sld = pnl.CurrentBattleUIComp.sldProgress;
                    if (sld != null && sld.gameObject.activeInHierarchy)
                    {

                        if (ExperimentPlayContext.ShouldApplyExperimentChart)
                        {
                            if (syncCooldownTimer > 0f)
                            {
                                syncCooldownTimer -= Time.deltaTime;
                            }

                            AudioSource bgmSource = null;
                            GameObject bgmGo = GameObject.Find("HwaBattleBgmSource");
                            if (bgmGo != null)
                            {
                                bgmSource = bgmGo.GetComponent<AudioSource>();
                            }

                            VideoPlayer bgaPlayer = null;
                            Camera mainCam = Camera.main;
                            if (mainCam != null)
                            {
                                Transform quad = mainCam.transform.Find("VideoBackgroundQuad");
                                if (quad != null)
                                {
                                    bgaPlayer = quad.GetComponent<VideoPlayer>();
                                }
                            }

                            if (syncCooldownTimer <= 0f && Time.timeScale > 0f)
                            {
                                float progressRatio = sld.value;

                                if (bgmSource != null && bgmSource.clip != null && bgmSource.isPlaying)
                                {
                                    float totalDuration = bgmSource.clip.length;
                                    float expectedTime = progressRatio * totalDuration;
                                    float currentAudioTime = bgmSource.time;

                                    if (Mathf.Abs(currentAudioTime - expectedTime) > 0.2f)
                                    {
                                        MelonLogger.Msg($"[Sync.BGM] 싱크 보정 적용! 오차: {currentAudioTime - expectedTime:F3}초 | 기존: {currentAudioTime:F2}초 -> 목표: {expectedTime:F2}초");
                                        bgmSource.time = expectedTime;
                                        syncCooldownTimer = 0.5f; // 0.5초 쿨다운을 두어 동기화 루프 충돌 방지
                                    }
                                }

                                if (bgaPlayer != null && bgaPlayer.isPrepared && bgaPlayer.isPlaying)
                                {
                                    float totalDuration = (float)bgaPlayer.length;
                                    float expectedTime = progressRatio * totalDuration;
                                    float currentVideoTime = (float)bgaPlayer.time;

                                    if (Mathf.Abs(currentVideoTime - expectedTime) > 0.2f)
                                    {
                                        MelonLogger.Msg($"[Sync.BGA] 싱크 보정 적용! 오차: {currentVideoTime - expectedTime:F3}초 | 기존: {currentVideoTime:F2}초 -> 목표: {expectedTime:F2}초");
                                        bgaPlayer.time = expectedTime;
                                        syncCooldownTimer = 0.5f; // 0.5초 쿨다운을 두어 비디오 버퍼 정비 보장
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    syncCooldownTimer = 0f; // 배틀 중이 아닐 때는 타이머 초기화
                }
            }
            catch (Exception)
            {
            }


            if (!ExperimentPlayContext.ShouldApplyExperimentChart)
            {
                return;
            }

            try
            {
                hywCheckTimer += Time.deltaTime;
                if (hywCheckTimer >= HywCheckInterval)
                {
                    hywCheckTimer = 0f;
                    hywStageManager.CheckForStageAndModify();
                }

                if (hywStageManager.IsInStage)
                {
                    hywStageManager.CheckForNoteEvents();
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[HywHpTextMod] Update 오류: {ex}");
            }
        }

        public override void OnApplicationQuit()
        {
            MelonLogger.Msg("모드가 종료되었습니다.");
        }

        public static void PreloadHwaManifest()
        {
            try
            {
                MelonLogger.Msg("[MainMod.Hwa] 모드 로드 시 manifest 선읽기 시작");
                cachedManifest = LoadHwaManifest(HwaFolderPath);

                if (cachedManifest != null)
                {
                    MelonLogger.Msg($"[MainMod.Hwa] 모드 로드 시 manifest 캐시 완료: {DescribeManifest(cachedManifest)}");
                }
                else
                {
                    MelonLogger.Msg("[MainMod.Hwa] 모드 로드 시 manifest 캐시 실패 또는 비어 있음");
                }

                cachedBmsChart = LoadHwaBmsChart(HwaFolderPath, cachedManifest);
                if (cachedBmsChart != null)
                {
                    MelonLogger.Msg($"[MainMod.Hwa.Bms] 모드 로드 시 BMS 캐시 완료: {DescribeBmsChart(cachedBmsChart)}");
                }
                else
                {
                    MelonLogger.Msg("[MainMod.Hwa.Bms] 모드 로드 시 BMS 캐시 실패 또는 비어 있음");
                }
            }
            catch (Exception ex)
            {
                cachedManifest = null;
                cachedBmsChart = null;
                MelonLogger.Error($"[MainMod.Hwa] 모드 로드 시 manifest 선읽기 실패: {ex}");
            }
        }

        public static bool TryGetCachedHwaManifest(out string description)
        {
            if (cachedManifest != null)
            {
                description = DescribeManifest(cachedManifest);
                return true;
            }

            description = string.Empty;
            return false;
        }

        public static bool TryGetCachedHwaSearchTerms(out string uid, out string title, out string artist, out string description)
        {
            uid = null;
            title = null;
            artist = null;

            if (cachedManifest == null)
            {
                description = string.Empty;
                return false;
            }

            uid = cachedManifest.Uid;
            title = cachedManifest.Title;
            artist = cachedManifest.Artist;
            description = DescribeManifest(cachedManifest);
            return true;
        }

        public static bool TryGetCachedHwaPrimaryVirtualSong(out string title, out string artist, out string levelDesigner, out int diff1, out int diff2, out int diff3, out int diff4, out int diff5, out string description)
        {
            title = null;
            artist = null;
            levelDesigner = null;
            diff1 = 2;
            diff2 = 5;
            diff3 = 0;
            diff4 = 0;
            diff5 = 0;

            if (cachedManifest == null)
            {
                description = string.Empty;
                return false;
            }

            title = !string.IsNullOrWhiteSpace(cachedManifest.CustomTitle) ? cachedManifest.CustomTitle : cachedManifest.Title;
            artist = !string.IsNullOrWhiteSpace(cachedManifest.CustomArtist) ? cachedManifest.CustomArtist : cachedManifest.Artist;
            levelDesigner = cachedManifest.LevelDesigner;
            diff1 = cachedManifest.Difficulty1 ?? diff1;
            diff2 = cachedManifest.Difficulty2 ?? diff2;
            diff3 = cachedManifest.Difficulty3 ?? diff3;
            diff4 = cachedManifest.Difficulty4 ?? diff4;
            diff5 = cachedManifest.Difficulty5 ?? diff5;
            description = DescribeManifest(cachedManifest);
            return true;
        }

        public static bool TryGetCachedHwaScene(out int scene)
        {
            scene = default;

            if (cachedManifest == null || !cachedManifest.Scene.HasValue)
            {
                return false;
            }

            scene = cachedManifest.Scene.Value;
            return true;
        }

        public static bool TryGetCachedHwaBmsChart(out BmsChart chart, out string description)
        {
            chart = cachedBmsChart;
            if (chart == null)
            {
                description = string.Empty;
                return false;
            }

            description = DescribeBmsChart(chart);
            return true;
        }

        private static HwaManifest LoadHwaManifest(string folderPath)
        {
            try
            {
                MelonLogger.Msg($"[MainMod.Hwa] manifest 탐색 시작: folder={folderPath}");

                string[] txtFiles = Directory.GetFiles(folderPath, "*.txt", SearchOption.AllDirectories);
                if (txtFiles == null || txtFiles.Length == 0)
                {
                    MelonLogger.Msg($"[MainMod.Hwa] 하위 폴더까지 스캔했지만 txt 파일이 없습니다: folder={folderPath}");
                    return null;
                }

                MelonLogger.Msg($"[MainMod.Hwa] txt 파일 {txtFiles.Length}개 발견(하위 폴더 포함): {string.Join(", ", Array.ConvertAll(txtFiles, file => GetRelativeHwaPath(folderPath, file)))}");

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
                    MelonLogger.Msg($"[MainMod.Hwa] 선택할 txt 파일이 없습니다: folder={folderPath}");
                    return null;
                }

                MelonLogger.Msg($"[MainMod.Hwa] manifest 읽기 대상: {preferred}");

                var manifest = new HwaManifest { SourcePath = preferred };
                foreach (var rawLine in File.ReadAllLines(preferred))
                {
                    if (TryParseManifestLine(rawLine, out string key, out string value))
                    {
                        MelonLogger.Msg($"[MainMod.Hwa] manifest line parsed: key={key}, value={value}");
                        ApplyManifestValue(manifest, key, value);
                    }
                }

                if (string.IsNullOrWhiteSpace(manifest.Uid) && string.IsNullOrWhiteSpace(manifest.Title) && string.IsNullOrWhiteSpace(manifest.Artist))
                {
                    MelonLogger.Msg($"[MainMod.Hwa] manifest 파싱은 했지만 핵심 값이 비어 있습니다: {DescribeManifest(manifest)}");
                    return null;
                }

                MelonLogger.Msg($"[MainMod.Hwa] manifest 파싱 완료: {DescribeManifest(manifest)}");

                return manifest;
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[MainMod.Hwa] manifest 읽기 실패: {ex}");
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

                MelonLogger.Msg($"[MainMod.Hwa.Bms] BMS 탐색 시작: folder={folderPath}");

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
                    MelonLogger.Msg($"[MainMod.Hwa.Bms] BMS 파일이 없습니다: folder={folderPath}");
                    return null;
                }

                MelonLogger.Msg($"[MainMod.Hwa.Bms] BMS 읽기 대상: {preferred}");
                var chart = BmsParser.ParseFile(preferred);
                MelonLogger.Msg($"[MainMod.Hwa.Bms] BMS 파싱 완료: {DescribeBmsChart(chart)}");
                LogBmsWavMappingSummary(chart);
                return chart;
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[MainMod.Hwa.Bms] BMS 읽기 실패: {ex}");
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
                MelonLogger.Error($"[MainMod.Hwa.Bms] BMS 탐색 실패: {ex}");
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
                MelonLogger.Msg("[MainMod.Hwa.Bms] 노트가 없어 WAV 매핑 샘플을 건너뜁니다.");
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
                    MelonLogger.Msg($"[MainMod.Hwa.Bms] WAV 매핑 샘플: raw={note.RawValue}, wav=(null)");
                }
                else
                {
                    MelonLogger.Msg($"[MainMod.Hwa.Bms] WAV 매핑 샘플: raw={note.RawValue}, wav={wavInfo.RawWavName}, uid={wavInfo.Uid ?? "(null)"}, type={wavInfo.NoteType}, prefab={wavInfo.PrefabName ?? "(null)"}, dt={wavInfo.Dt}, keyAudio={wavInfo.KeyAudio ?? "(null)"}, bossAction={wavInfo.BossAction ?? "(null)"}");
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
                MelonLogger.Msg($"[MainMod.Hwa.Bms] 보스 스왑 후보 #{i + 1}: outTick={evt.OutNote?.Tick}, inTick={evt.InNote?.Tick}, delay={evt.DelaySeconds:0.###}s, action={evt.BossAction}");
            }
        }

        private static void ApplyManifestValue(HwaManifest manifest, string key, string value)
        {
            if (manifest == null || string.IsNullOrWhiteSpace(key) || string.IsNullOrWhiteSpace(value))
            {
                return;
            }

            string normalizedKey = NormalizeManifestKey(key);
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
                + ", scene=" + (manifest.Scene.HasValue ? manifest.Scene.Value.ToString() : "(null)")
                + ", diff1=" + (manifest.Difficulty1.HasValue ? manifest.Difficulty1.Value.ToString() : "(null)")
                + ", diff2=" + (manifest.Difficulty2.HasValue ? manifest.Difficulty2.Value.ToString() : "(null)")
                + ", diff3=" + (manifest.Difficulty3.HasValue ? manifest.Difficulty3.Value.ToString() : "(null)")
                + ", diff4=" + (manifest.Difficulty4.HasValue ? manifest.Difficulty4.Value.ToString() : "(null)")
                + ", diff5=" + (manifest.Difficulty5.HasValue ? manifest.Difficulty5.Value.ToString() : "(null)");
        }

    }
}
