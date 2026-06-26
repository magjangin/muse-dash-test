using MelonLoader;
using System;
using System.IO;

namespace muse_dash_test
{
    internal static class HwaManifestLoader
    {
        internal static HwaManifest LoadHwaManifest(string folderPath)
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

        internal static string GetRelativeHwaPath(string rootPath, string filePath)
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
            catch (Exception) { }

            return Path.GetFileName(filePath);
        }

        internal static bool TryParseManifestLine(string rawLine, out string key, out string value)
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

        internal static void ApplyManifestValue(HwaManifest manifest, string key, string value)
        {
            if (manifest == null || string.IsNullOrWhiteSpace(key) || string.IsNullOrWhiteSpace(value))
            {
                return;
            }

            string normalizedKey = NormalizeManifestKey(key);
            if (TryApplyString(normalizedKey, value, v => manifest.Album = v, "가져올앨범", "앨범", "album")) return;
            if (TryApplyString(normalizedKey, value, v => manifest.Uid = v, "uid")) return;
            if (TryApplyString(normalizedKey, value, v => manifest.CustomArtist = v, "커스텀아티스트", "customartist", "customauthor")) return;
            if (TryApplyString(normalizedKey, value, v => manifest.LevelDesigner = v, "레벨디자이너", "leveldesigner")) return;
            if (TryApplyString(normalizedKey, value, v => manifest.Artist = v, "artist", "아티스트", "author")) return;
            if (TryApplyString(normalizedKey, value, v => manifest.CustomTitle = v, "커스텀곡제목", "customsongtitle", "customtitle")) return;
            if (TryApplyString(normalizedKey, value, v => manifest.Title = v, "곡이름", "곡명", "곡제목", "가져올곡", "song", "title", "music")) return;
            if (TryApplyInt(normalizedKey, value, v => manifest.Scene = v, "씬번호", "scene")) return;
            if (TryApplyInt(normalizedKey, value, v => manifest.Difficulty1 = v, "난이도1", "difficulty1")) return;
            if (TryApplyInt(normalizedKey, value, v => manifest.Difficulty2 = v, "난이도2", "difficulty2")) return;
            if (TryApplyInt(normalizedKey, value, v => manifest.Difficulty3 = v, "난이도3", "difficulty3")) return;
            if (TryApplyInt(normalizedKey, value, v => manifest.Difficulty4 = v, "난이도4", "difficulty4")) return;
            if (TryApplyInt(normalizedKey, value, v => manifest.Difficulty5 = v, "난이도5", "difficulty5")) return;
            if (TryApplyDouble(normalizedKey, value, v => manifest.Delay = v, "delay", "지연")) return;
            TryApplyDouble(normalizedKey, value, v => manifest.Offset = v, "offset", "오프셋", "싱크");
        }

        private static bool TryApplyString(string normalizedKey, string value, Action<string> apply, params string[] tokens)
        {
            if (!ContainsAny(normalizedKey, tokens)) return false;
            apply(value);
            return true;
        }

        private static bool TryApplyInt(string normalizedKey, string value, Action<int?> apply, params string[] tokens)
        {
            if (!ContainsAny(normalizedKey, tokens)) return false;
            apply(TryParseNullableInt(value));
            return true;
        }

        private static bool TryApplyDouble(string normalizedKey, string value, Action<double?> apply, params string[] tokens)
        {
            if (!ContainsAny(normalizedKey, tokens)) return false;
            apply(TryParseNullableDouble(value));
            return true;
        }

        private static bool ContainsAny(string normalizedKey, params string[] tokens)
        {
            foreach (string token in tokens)
            {
                if (normalizedKey.Contains(token)) return true;
            }

            return false;
        }

        internal static string NormalizeManifestKey(string text)
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

        internal static int? TryParseNullableInt(string value)
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

        internal static double? TryParseNullableDouble(string value)
        {
            if (string.IsNullOrWhiteSpace(value) || value.Trim().Equals("null", StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            if (double.TryParse(value.Trim(), out double parsed))
            {
                return parsed;
            }

            return null;
        }

        internal static string DescribeManifest(HwaManifest manifest)
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
                + ", diff5=" + (manifest.Difficulty5.HasValue ? manifest.Difficulty5.Value.ToString() : "(null)")
                + ", delay=" + (manifest.Delay.HasValue ? manifest.Delay.Value.ToString("F7") : "(null)")
                + ", offset=" + (manifest.Offset.HasValue ? manifest.Offset.Value.ToString("F7") : "(null)");
        }
    }
}
