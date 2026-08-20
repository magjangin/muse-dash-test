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
    /// 저장 직전 가상 기록을 제거합니다), 커스텀 곡 기록은 당사 전용 샌드박스로 직접 관리합니다.
    ///
    /// [최고 기록 갱신 & 플레이 횟수 누적 지원]
    /// 1. 이전 기록보다 점수/정확도/FC/AP 상태가 우수할 때만 최고 기록(High Score)을 갱신합니다.
    /// 2. 플레이할 때마다 플레이/클리어 횟수(playCount)가 누적 갱신됩니다.
    /// </summary>
    public static class CustomRecordStore
    {
        /// <summary>기록 파일이 저장되는 폴더입니다. 게임 루트의 record/ 입니다.</summary>
        public static readonly string RecordFolderPath =
            Path.Combine(MelonLoader.Utils.MelonEnvironment.GameRootDirectory, "record");

        /// <summary>
        /// 현재 선택/플레이 중인 난이도를 해석합니다.
        /// 기록 파일명의 키이므로, 저장(승리 시점)과 로드(패널)가 반드시 같은 출처를 쓰도록
        /// 이 메서드 하나로 단일화합니다. 값을 못 구하면 1로 폴백합니다.
        /// </summary>
        public static int ResolveCurrentDifficulty()
        {
            try
            {
                var stage = Il2CppAssets.Scripts.Database.GlobalDataBase.s_DbBattleStage;
                if (stage != null) return stage.selectedDifficulty;
            }
            catch (Exception ex)
            {
                ModLogger.Warning($"[CustomRecordStore] 난이도 해석 실패, 1로 폴백: {ex.Message}");
            }
            return 1;
        }

        /// <summary>
        /// 한 판의 플레이 결과를 record/{uid}_{difficulty}.json 에 기록합니다.
        /// (최고 점수 갱신 비교 및 플레이 횟수 누적을 수행합니다.)
        /// </summary>
        public static void SaveResult(
            string uid, int difficulty,
            int standard, int gears, int hearts, int blueNotes,
            int perfect, int great, int miss,
            int score, int maxCombo,
            float accuracy,
            bool isFullCombo, bool isAllPerfect)
        {
            try
            {
                if (string.IsNullOrEmpty(uid))
                {
                    ModLogger.Warning("[CustomRecordStore] uid가 비어 있어 기록 저장을 건너뜁니다.");
                    return;
                }

                Directory.CreateDirectory(RecordFolderPath);

                int noteCount = standard + gears + hearts + blueNotes;
                // 풀콤보면 최대 콤보는 정의상 전체 노트 수입니다. (게임 필드 읽기 실패 시 안전 보정)
                if (isFullCombo && maxCombo < noteCount) maxCombo = noteCount;

                // 1. 기존 기록을 읽어와 최고 점수 비교 및 플레이 횟수 누적을 수행합니다.
                var existing = LoadResult(uid, difficulty);

                int updatedPlayCount = (existing != null && existing.playCount > 0) ? existing.playCount + 1 : 1;

                // 2. 최고 기록(High Score) 판정:
                //    기존 기록이 없거나, 새 점수가 더 높거나, 점수가 같아도 정확도가 높거나, FC/AP 신규 달성 시 갱신
                bool isNewHighScore = false;
                if (existing == null)
                {
                    isNewHighScore = true;
                }
                else if (score > existing.score)
                {
                    isNewHighScore = true;
                }
                else if (score == existing.score && accuracy > existing.accuracy)
                {
                    isNewHighScore = true;
                }
                else if (isAllPerfect && !existing.isAllPerfect)
                {
                    isNewHighScore = true;
                }
                else if (isFullCombo && !existing.isFullCombo && !existing.isAllPerfect)
                {
                    isNewHighScore = true;
                }

                int finalScore = isNewHighScore ? score : existing.score;
                int finalMaxCombo = isNewHighScore ? maxCombo : existing.maxCombo;
                float finalAccuracy = isNewHighScore ? accuracy : existing.accuracy;
                bool finalIsFullCombo = isNewHighScore ? isFullCombo : (existing.isFullCombo || isFullCombo);
                bool finalIsAllPerfect = isNewHighScore ? isAllPerfect : (existing.isAllPerfect || isAllPerfect);

                int finalNoteCount = isNewHighScore ? noteCount : existing.noteCount;
                int finalStandard = isNewHighScore ? standard : existing.standard;
                int finalGears = isNewHighScore ? gears : existing.gears;
                int finalHearts = isNewHighScore ? hearts : existing.hearts;
                int finalBlueNotes = isNewHighScore ? blueNotes : existing.blueNotes;
                int finalPerfect = isNewHighScore ? perfect : existing.perfect;
                int finalGreat = isNewHighScore ? great : existing.great;
                int finalMiss = isNewHighScore ? miss : existing.miss;
                string finalSavedAt = isNewHighScore ? DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture) : existing.savedAtUtc;

                // 방금 플레이한 채보의 지문을 함께 남깁니다. 계산에 실패해도(null) 기록 저장 자체는 막지 않습니다.
                string fingerprint = ChartFingerprint.ForUid(uid) ?? ChartFingerprint.NoChart;

                string filePath = Path.Combine(RecordFolderPath, $"{SanitizeFileName(uid)}_{difficulty}.json");
                string json = BuildJson(uid, finalNoteCount, finalStandard, finalGears, finalHearts, finalBlueNotes,
                    finalPerfect, finalGreat, finalMiss, finalScore, finalMaxCombo, finalAccuracy,
                    finalIsFullCombo, finalIsAllPerfect, updatedPlayCount, finalSavedAt, fingerprint);

                File.WriteAllText(filePath, json, Encoding.UTF8);
                ModLogger.Msg($"[CustomRecordStore] 기록 저장 완료 (신규 최고기록: {isNewHighScore}) → {filePath} (playCount={updatedPlayCount}, score={finalScore}, maxCombo={finalMaxCombo}, acc={finalAccuracy:0.0000}, FC={finalIsFullCombo}, AP={finalIsAllPerfect})");

                try
                {
                    DiscordPresenceManager.ResolveSongDetails(uid, out string title, out _);
                    DiscordPresenceManager.SetResults(title, finalScore, finalAccuracy, finalIsFullCombo, finalIsAllPerfect);
                }
                catch (Exception ex)
                {
                    ModLogger.Error($"[CustomRecordStore] Discord Presence 갱신 에러: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                ModLogger.Error($"[CustomRecordStore] 기록 저장 중 예외: {ex}");
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
            public int score;
            public int maxCombo;
            public float accuracy;
            public bool isFullCombo;
            public bool isAllPerfect;
            public int playCount = 1;
            public string savedAtUtc = string.Empty;

            /// <summary>이 기록을 만든 채보의 지문입니다. 지문을 적기 전(v0.10.1 이하)의 기록은 비어 있습니다.</summary>
            public string chartFingerprint = string.Empty;
        }

        /// <summary>같은 사유를 매 패널 갱신마다 찍지 않도록, 슬롯별로 한 번만 경고합니다(실측 세션당 86회 로드).</summary>
        private static readonly System.Collections.Generic.HashSet<string> WarnedSlots =
            new System.Collections.Generic.HashSet<string>(StringComparer.Ordinal);

        /// <summary>
        /// 읽어온 기록이 <b>지금 그 슬롯에 들어 있는 채보</b>의 것인지 판정합니다.
        ///
        /// <para>기록 파일은 uid(= hwa 폴더 순번)로만 묶이므로, 폴더 안 BMS를 갈아끼우거나
        /// 곡 폴더를 추가·삭제해 순번이 밀리면 남의 기록이 그대로 표시됩니다. 그래서 채보 지문으로 걸러냅니다.
        /// 자세한 배경은 <see cref="ChartFingerprint"/> 참고.</para>
        ///
        /// <para>판정에 실패했을 때(지문 계산 예외)는 <b>통과시킵니다</b>. 일시적인 파일 읽기 실패로
        /// 멀쩡한 기록을 숨기는 쪽이 더 나쁘기 때문입니다.</para>
        /// </summary>
        private static bool BelongsToCurrentChart(PlayRecord record, string uid, int difficulty, string filePath)
        {
            if (record == null) return false;

            string current = ChartFingerprint.ForUid(uid);
            if (current == null) return true; // 지문 계산 실패 → 판정 불가, 통과

            if (string.IsNullOrEmpty(record.chartFingerprint))
            {
                WarnOnce(uid, difficulty,
                    $"[CustomRecordStore] 채보 지문이 없는 옛 기록이라 표시하지 않습니다 → {filePath} "
                    + "(v0.10.1 이하에서 저장된 기록입니다. 이 채보를 한 번 플레이하면 지문과 함께 새로 기록됩니다. 파일은 지우지 않았습니다.)");
                return false;
            }

            if (!string.Equals(record.chartFingerprint, current, StringComparison.OrdinalIgnoreCase))
            {
                WarnOnce(uid, difficulty,
                    $"[CustomRecordStore] 다른 채보의 기록이라 표시하지 않습니다 → {filePath} "
                    + $"(기록 지문={record.chartFingerprint}, 현재 채보 지문={current}, 기록 노트수={record.noteCount}). "
                    + "이 슬롯의 BMS가 바뀌었거나 곡 폴더 순번이 밀린 것입니다.");
                return false;
            }

            return true;
        }

        private static void WarnOnce(string uid, int difficulty, string message)
        {
            string key = uid + "_" + difficulty;
            lock (WarnedSlots)
            {
                if (!WarnedSlots.Add(key)) return;
            }
            ModLogger.Warning(message);
        }

        /// <summary>
        /// record/{uid}_{difficulty}.json 에서 플레이 기록을 로드합니다.
        /// 지금 채보의 기록이 아니면 null을 돌려줍니다(<see cref="BelongsToCurrentChart"/>).
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
                if (record != null && record.playCount <= 0) record.playCount = 1;

                if (!BelongsToCurrentChart(record, uid, difficulty, filePath)) return null;

                ModLogger.Msg($"[CustomRecordStore] 기록 로드 성공 → {filePath} (playCount={record?.playCount}, score={record?.score}, acc={record?.accuracy:0.0000}, FC={record?.isFullCombo})");
                return record;
            }
            catch (Exception ex)
            {
                ModLogger.Error($"[CustomRecordStore] 기록 로드 중 예외 (uid={uid}, diff={difficulty}): {ex}");
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
                    case "score":
                        int.TryParse(val, out record.score);
                        break;
                    case "maxCombo":
                        int.TryParse(val, out record.maxCombo);
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
                    case "playCount":
                        int.TryParse(val, out record.playCount);
                        break;
                    case "savedAtUtc":
                        record.savedAtUtc = val.Replace("\"", "").Trim();
                        break;
                    case "chartFingerprint":
                        record.chartFingerprint = val.Replace("\"", "").Trim();
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
            int playCount, string savedAtUtc, string chartFingerprint)
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
            sb.Append("  \"savedAtUtc\": \"").Append(string.IsNullOrEmpty(savedAtUtc) ? DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ", ci) : savedAtUtc).Append("\"\n");
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
