using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace muse_dash_test
{
    public static partial class CustomRecordStore
    {
        public class PlayRecord
        {
            public string uid = string.Empty;
            public int noteCount;
            public int standard;
            public int gears;
            public int hearts;
            public int blueNotes;
            public int perfect;
            public int great;
            public int miss;
            public int score;
            public int maxCombo;
            public float accuracy;
            public bool isFullCombo;
            public bool isAllPerfect;
            public int playCount = 1;
            public string savedAtUtc = string.Empty;

            /// <summary>이 기록을 만든 채보의 지문입니다. 지문을 적기 전(v0.10.1 이하)의 기록은 비어 있습니다.</summary>
            public string chartFingerprint = string.Empty;

            /// <summary>기록 파일의 키로 쓰인 곡 폴더 이름입니다(파일을 열었을 때 어느 곡인지 알아보기 위한 것).</summary>
            public string songFolder = string.Empty;

            internal PlayRecord Clone() => (PlayRecord)MemberwiseClone();
        }

        private static PlayRecord ReadRecordFile(string filePath)
        {
            var record = ParseJson(File.ReadAllText(filePath, Encoding.UTF8));
            if (record != null && record.playCount <= 0) record.playCount = 1;
            return record;
        }

        private static PlayRecord ParseJson(string json)
        {
            var record = new PlayRecord();
            var lines = json.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines)
            {
                var clean = line.Trim();
                if (!clean.Contains(":")) continue;
                var idx = clean.IndexOf(':');
                var key = clean.Substring(0, idx).Replace("\"", "").Trim();
                var val = clean.Substring(idx + 1).Trim().TrimEnd(',');

                switch (key)
                {
                    case "uid":
                        record.uid = val.Replace("\"", "").Trim();
                        break;
                    case "noteCount":
                        int.TryParse(val, out record.noteCount);
                        break;
                    case "standard":
                        int.TryParse(val, out record.standard);
                        break;
                    case "gears":
                        int.TryParse(val, out record.gears);
                        break;
                    case "hearts":
                        int.TryParse(val, out record.hearts);
                        break;
                    case "blueNotes":
                        int.TryParse(val, out record.blueNotes);
                        break;
                    case "perfect":
                        int.TryParse(val, out record.perfect);
                        break;
                    case "great":
                        int.TryParse(val, out record.great);
                        break;
                    case "miss":
                        int.TryParse(val, out record.miss);
                        break;
                    case "score":
                        int.TryParse(val, out record.score);
                        break;
                    case "maxCombo":
                        int.TryParse(val, out record.maxCombo);
                        break;
                    case "accuracy":
                        float.TryParse(val, NumberStyles.Float, CultureInfo.InvariantCulture, out record.accuracy);
                        break;
                    case "isFullCombo":
                        record.isFullCombo = val.Equals("true", StringComparison.OrdinalIgnoreCase);
                        break;
                    case "isAllPerfect":
                        record.isAllPerfect = val.Equals("true", StringComparison.OrdinalIgnoreCase);
                        break;
                    case "playCount":
                        int.TryParse(val, out record.playCount);
                        break;
                    case "savedAtUtc":
                        record.savedAtUtc = val.Replace("\"", "").Trim();
                        break;
                    case "chartFingerprint":
                        record.chartFingerprint = val.Replace("\"", "").Trim();
                        break;
                    case "songFolder":
                        record.songFolder = val.Replace("\"", "").Trim();
                        break;
                }
            }
            return record;
        }

        // 사람이 열어볼 수 있도록 들여쓰기된 평문 JSON을 직접 구성합니다.
        private static string BuildJson(
            string uid, int noteCount,
            int standard, int gears, int hearts, int blueNotes,
            int perfect, int great, int miss,
            int score, int maxCombo,
            float accuracy, bool isFullCombo, bool isAllPerfect,
            int playCount, string savedAtUtc, string chartFingerprint, string songFolder)
        {
            var ci = CultureInfo.InvariantCulture;
            var sb = new StringBuilder();
            sb.Append('{').Append('\n');
            sb.Append("  \"uid\": \"").Append(EscapeJson(uid)).Append("\",\n");
            sb.Append("  \"noteCount\": ").Append(noteCount).Append(",\n");
            sb.Append("  \"standard\": ").Append(standard).Append(",\n");
            sb.Append("  \"gears\": ").Append(gears).Append(",\n");
            sb.Append("  \"hearts\": ").Append(hearts).Append(",\n");
            sb.Append("  \"blueNotes\": ").Append(blueNotes).Append(",\n");
            sb.Append("  \"perfect\": ").Append(perfect).Append(",\n");
            sb.Append("  \"great\": ").Append(great).Append(",\n");
            sb.Append("  \"miss\": ").Append(miss).Append(",\n");
            sb.Append("  \"score\": ").Append(score).Append(",\n");
            sb.Append("  \"maxCombo\": ").Append(maxCombo).Append(",\n");
            sb.Append("  \"accuracy\": ").Append(accuracy.ToString("0.000000", ci)).Append(",\n");
            sb.Append("  \"isFullCombo\": ").Append(isFullCombo ? "true" : "false").Append(",\n");
            sb.Append("  \"isAllPerfect\": ").Append(isAllPerfect ? "true" : "false").Append(",\n");
            sb.Append("  \"playCount\": ").Append(playCount).Append(",\n");
            sb.Append("  \"savedAtUtc\": \"").Append(string.IsNullOrEmpty(savedAtUtc) ? DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ", ci) : savedAtUtc).Append("\",\n");
            sb.Append("  \"chartFingerprint\": \"").Append(EscapeJson(string.IsNullOrEmpty(chartFingerprint) ? ChartFingerprint.NoChart : chartFingerprint)).Append("\",\n");
            sb.Append("  \"songFolder\": \"").Append(EscapeJson(songFolder)).Append("\"\n");
            sb.Append('}').Append('\n');
            return sb.ToString();
        }

        private static string EscapeJson(string value)
        {
            if (string.IsNullOrEmpty(value)) return string.Empty;
            return value.Replace("\\", "\\\\").Replace("\"", "\\\"");
        }

        private static string SanitizeFileName(string uid)
        {
            foreach (char c in Path.GetInvalidFileNameChars())
            {
                uid = uid.Replace(c, '_');
            }
            return uid;
        }
    }
}
