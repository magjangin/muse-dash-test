using System;
using System.Globalization;
using System.IO;
using System.Text;
using MelonLoader;

namespace muse_dash_test
{
    /// <summary>
    /// 커스텀 곡(1999-*, 1998-*)의 플레이 기록을 게임 세이브와 분리된 별도 폴더(record/)에 저장합니다.
    ///
    /// 게임 세이브 시스템은 "최고 기록만 유지"하고 가상 곡 데이터를 오염시키므로(그래서 SaveDataManagerPatch가
    /// 저장 직전 가상 기록을 제거합니다), 커스텀 곡 기록은 게임과 싸우지 않고 우리가 직접 관리합니다.
    ///
    /// [1단계] 승리 시점에 record/{uid}.json 파일을 생성/갱신하는 것까지만 담당합니다.
    ///         (이 파일을 읽어서 곡 선택/준비 패널에 띄우는 것은 2단계에서 진행합니다.)
    /// </summary>
    public static class CustomRecordStore
    {
        /// <summary>기록 파일이 저장되는 폴더입니다. 게임 루트의 record/ 입니다.</summary>
        public static readonly string RecordFolderPath =
            Path.Combine(MelonLoader.Utils.MelonEnvironment.GameRootDirectory, "record");

        /// <summary>
        /// 한 판의 플레이 결과를 record/{uid}_{difficulty}.json 에 기록합니다.
        /// </summary>
        public static void SaveResult(
            string uid, int difficulty,
            int standard, int gears, int hearts, int blueNotes,
            int perfect, int great, int miss,
            float accuracy,
            bool isFullCombo, bool isAllPerfect)
        {
            try
            {
                if (string.IsNullOrEmpty(uid))
                {
                    MelonLogger.Warning("[CustomRecordStore] uid가 비어 있어 기록 저장을 건너뜁니다.");
                    return;
                }

                Directory.CreateDirectory(RecordFolderPath);

                int noteCount = standard + gears + hearts + blueNotes;
                string filePath = Path.Combine(RecordFolderPath, $"{SanitizeFileName(uid)}_{difficulty}.json");
                string json = BuildJson(uid, noteCount, standard, gears, hearts, blueNotes,
                    perfect, great, miss, accuracy, isFullCombo, isAllPerfect);

                File.WriteAllText(filePath, json, Encoding.UTF8);
                MelonLogger.Msg($"[CustomRecordStore] 기록 저장 완료 → {filePath} (notes={noteCount}, acc={accuracy:0.0000}, FC={isFullCombo}, AP={isAllPerfect})");
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[CustomRecordStore] 기록 저장 중 예외: {ex}");
            }
        }

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
            public float accuracy;
            public bool isFullCombo;
            public bool isAllPerfect;
            public string savedAtUtc = string.Empty;
        }

        /// <summary>
        /// record/{uid}_{difficulty}.json 에서 플레이 기록을 로드합니다.
        /// </summary>
        public static PlayRecord LoadResult(string uid, int difficulty)
        {
            try
            {
                if (string.IsNullOrEmpty(uid)) return null;

                string filename = $"{SanitizeFileName(uid)}_{difficulty}.json";
                string filePath = Path.Combine(RecordFolderPath, filename);
                if (!File.Exists(filePath))
                {
                    // Fallback to legacy {uid}.json if {uid}_{difficulty}.json doesn't exist
                    string fallbackPath = Path.Combine(RecordFolderPath, SanitizeFileName(uid) + ".json");
                    if (File.Exists(fallbackPath))
                    {
                        filePath = fallbackPath;
                    }
                    else
                    {
                        return null;
                    }
                }

                string content = File.ReadAllText(filePath, Encoding.UTF8);
                var record = ParseJson(content);
                MelonLogger.Msg($"[CustomRecordStore] 기록 로드 성공 → {filePath} (acc={record.accuracy:0.0000}, FC={record.isFullCombo})");
                return record;
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[CustomRecordStore] 기록 로드 중 예외 (uid={uid}, diff={difficulty}): {ex}");
                return null;
            }
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
                    case "accuracy":
                        float.TryParse(val, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out record.accuracy);
                        break;
                    case "isFullCombo":
                        record.isFullCombo = val.Equals("true", StringComparison.OrdinalIgnoreCase);
                        break;
                    case "isAllPerfect":
                        record.isAllPerfect = val.Equals("true", StringComparison.OrdinalIgnoreCase);
                        break;
                    case "savedAtUtc":
                        record.savedAtUtc = val.Replace("\"", "").Trim();
                        break;
                }
            }
            return record;
        }

        // 사람이 열어볼 수 있도록 들여쓰기된 평문 JSON을 직접 구성합니다. (외부 의존성 없이 1단계용)
        private static string BuildJson(
            string uid, int noteCount,
            int standard, int gears, int hearts, int blueNotes,
            int perfect, int great, int miss,
            float accuracy, bool isFullCombo, bool isAllPerfect)
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
            sb.Append("  \"accuracy\": ").Append(accuracy.ToString("0.000000", ci)).Append(",\n");
            sb.Append("  \"isFullCombo\": ").Append(isFullCombo ? "true" : "false").Append(",\n");
            sb.Append("  \"isAllPerfect\": ").Append(isAllPerfect ? "true" : "false").Append(",\n");
            sb.Append("  \"savedAtUtc\": \"").Append(DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ", ci)).Append("\"\n");
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
