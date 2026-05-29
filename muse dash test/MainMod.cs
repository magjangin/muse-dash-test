using MelonLoader;
using System;
using System.IO;

[assembly: MelonInfo(typeof(muse_dash_test.MainMod), "muse-dash-test", "0.1.0", "화영왕")]
[assembly: MelonGame("PeroPeroGames", "MuseDash")]

namespace muse_dash_test
{
    public class MainMod : MelonMod
    {
        private static readonly string HwaFolderPath = Path.Combine(MelonLoader.Utils.MelonEnvironment.GameRootDirectory, "hwa");
        private static HwaManifest cachedManifest;

        private sealed class HwaManifest
        {
            public string SourcePath;
            public string Uid;
            public string Title;
            public string Artist;
            public int? Scene;
            public int? Difficulty1;
            public int? Difficulty2;
            public int? Difficulty3;
            public int? Difficulty4;
            public int? Difficulty5;
        }

        public override void OnApplicationStart()
        {
            MelonLogger.Msg("모드가 로드되었습니다.");

            try
            {
                Directory.CreateDirectory(HwaFolderPath);
                MelonLogger.Msg($"hwa 폴더를 확인/생성했습니다: {HwaFolderPath}");
                PreloadHwaManifest();
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"hwa 폴더 생성 중 예외: {ex}");
            }
        }

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            MelonLogger.Msg($"씬이 로드되었습니다: {sceneName} (빌드 인덱스: {buildIndex})");
        }

        public override void OnUpdate()
        {
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
            }
            catch (Exception ex)
            {
                cachedManifest = null;
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

        private static HwaManifest LoadHwaManifest(string folderPath)
        {
            try
            {
                MelonLogger.Msg($"[MainMod.Hwa] manifest 탐색 시작: folder={folderPath}");

                string[] txtFiles = Directory.GetFiles(folderPath, "*.txt", SearchOption.TopDirectoryOnly);
                if (txtFiles == null || txtFiles.Length == 0)
                {
                    MelonLogger.Msg($"[MainMod.Hwa] txt 파일이 없습니다: folder={folderPath}");
                    return null;
                }

                MelonLogger.Msg($"[MainMod.Hwa] txt 파일 {txtFiles.Length}개 발견: {string.Join(", ", Array.ConvertAll(txtFiles, Path.GetFileName))}");

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

            if (normalizedKey.Contains("artist") || normalizedKey.Contains("아티스트") || normalizedKey.Contains("author"))
            {
                manifest.Artist = value;
                return;
            }

            if (normalizedKey.Contains("곡이름") || normalizedKey.Contains("곡명") || normalizedKey.Contains("가져올곡") || normalizedKey.Contains("song") || normalizedKey.Contains("title") || normalizedKey.Contains("music"))
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
                + ", artist=" + (manifest.Artist ?? "(null)")
                + ", scene=" + (manifest.Scene.HasValue ? manifest.Scene.Value.ToString() : "(null)")
                + ", diff1=" + (manifest.Difficulty1.HasValue ? manifest.Difficulty1.Value.ToString() : "(null)")
                + ", diff2=" + (manifest.Difficulty2.HasValue ? manifest.Difficulty2.Value.ToString() : "(null)")
                + ", diff3=" + (manifest.Difficulty3.HasValue ? manifest.Difficulty3.Value.ToString() : "(null)")
                + ", diff4=" + (manifest.Difficulty4.HasValue ? manifest.Difficulty4.Value.ToString() : "(null)")
                + ", diff5=" + (manifest.Difficulty5.HasValue ? manifest.Difficulty5.Value.ToString() : "(null)");
        }
    }
}
