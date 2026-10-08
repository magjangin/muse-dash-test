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
        /// 기록 파일의 키를 해석합니다. <b>uid가 아니라 곡 폴더 이름</b>입니다.
        ///
        /// <para>uid(<c>1999-N</c>)는 <c>hwa</c> 폴더를 이름순 정렬한 <b>순번</b>이라서
        /// (<c>HwaResourceManager.PreloadHwaManifest</c>), 곡 폴더를 추가·삭제·개명하면 통째로 밀립니다.
        /// uid를 키로 쓰면 그때마다 <b>남의 곡 기록이 붙습니다.</b> 폴더 이름은 순번과 무관하므로
        /// 폴더를 어디에 끼워 넣어도 기록이 따라다니지 않습니다.</para>
        ///
        /// <para>폴더를 해석할 수 없거나(테스트 슬롯 등) 해석 결과가 <c>hwa</c> 루트 자체이면
        /// 슬롯을 구분할 수 없으므로 예전처럼 uid로 돌아갑니다.</para>
        /// </summary>
        public static string ResolveRecordKey(string uid)
        {
            try
            {
                if (string.IsNullOrEmpty(uid)) return uid;
                if (!HwaResourceManager.TryGetSongDirectory(uid, out string dir) || string.IsNullOrEmpty(dir)) return uid;

                // hwa 루트 자체는 테스트 슬롯 3개가 전부 같은 이름으로 해석되므로 키가 될 수 없습니다.
                if (string.Equals(TrimSeparators(dir), TrimSeparators(HwaResourceManager.HwaFolderPath), StringComparison.OrdinalIgnoreCase))
                {
                    return uid;
                }

                string folderName = Path.GetFileName(TrimSeparators(dir));
                return string.IsNullOrWhiteSpace(folderName) ? uid : folderName;
            }
            catch (Exception ex)
            {
                ModLogger.Warning($"[CustomRecordStore] uid={uid} 곡 폴더 해석 실패, uid를 키로 씁니다: {ex.Message}");
                return uid;
            }
        }

        private static string TrimSeparators(string path)
        {
            return string.IsNullOrEmpty(path)
                ? path
                : path.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        }

        private static string RecordPathFor(string key, int difficulty)
        {
            return Path.Combine(RecordFolderPath, $"{SanitizeFileName(key)}_{difficulty}.json");
        }

        /// <summary>
        /// 읽을 기록 파일을 찾습니다. 폴더 이름 키 → (구버전) uid 키 → (더 옛날) uid.json 순으로 훑습니다.
        /// </summary>
        private static string FindExistingRecordPath(string uid, int difficulty)
        {
            string keyPath = RecordPathFor(ResolveRecordKey(uid), difficulty);
            if (File.Exists(keyPath)) return keyPath;

            string uidPath = RecordPathFor(uid, difficulty);
            if (File.Exists(uidPath)) return uidPath;

            string legacyPath = Path.Combine(RecordFolderPath, SanitizeFileName(uid) + ".json");
            return File.Exists(legacyPath) ? legacyPath : null;
        }

        /// <summary>
        /// 한 판의 플레이 결과를 record/{곡 폴더 이름}_{difficulty}.json 에 기록합니다.
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

                // 2. 최고 기록 / 달성 배지 / 최고 콤보 판정.
                //    셋은 서로 다른 개념이라 규칙을 PlayRecordMerge로 분리했습니다.
                //    (한 플래그로 겸하다가 낮은 점수의 AP가 최고점을 덮고, 반대로 점수만 높은 판이
                //     이미 딴 FC/AP를 지우는 사고가 있었습니다. 자세한 배경은 PlayRecordMerge 참고)
                var merged = PlayRecordMerge.Merge(
                    existing != null,
                    existing?.score ?? 0, existing?.maxCombo ?? 0, existing?.accuracy ?? 0f,
                    existing?.isFullCombo ?? false, existing?.isAllPerfect ?? false,
                    score, maxCombo, accuracy, isFullCombo, isAllPerfect);

                bool isNewHighScore = merged.IsNewHighScore;
                int finalScore = merged.Score;
                int finalMaxCombo = merged.MaxCombo;
                float finalAccuracy = merged.Accuracy;
                bool finalIsFullCombo = merged.IsFullCombo;
                bool finalIsAllPerfect = merged.IsAllPerfect;

                int finalNoteCount = isNewHighScore || existing == null ? noteCount : existing.noteCount;
                int finalStandard = isNewHighScore || existing == null ? standard : existing.standard;
                int finalGears = isNewHighScore || existing == null ? gears : existing.gears;
                int finalHearts = isNewHighScore || existing == null ? hearts : existing.hearts;
                int finalBlueNotes = isNewHighScore || existing == null ? blueNotes : existing.blueNotes;
                int finalPerfect = isNewHighScore || existing == null ? perfect : existing.perfect;
                int finalGreat = isNewHighScore || existing == null ? great : existing.great;
                int finalMiss = isNewHighScore || existing == null ? miss : existing.miss;
                string finalSavedAt = isNewHighScore || existing == null ? DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture) : existing.savedAtUtc;

                // 지문은 배틀 시작 때 고정한 값을 씁니다. 고정된 값이 없을 때만 지금 파일을 읽습니다.
                // 계산에 실패해도(null) 기록 저장 자체는 막지 않습니다.
                string fingerprint = CustomPlaySession.Current.TakeBattleChartFingerprint(uid)
                    ?? ChartFingerprint.ForUid(uid)
                    ?? ChartFingerprint.NoChart;

                string recordKey = ResolveRecordKey(uid);
                string filePath = RecordPathFor(recordKey, difficulty);
                string json = BuildJson(uid, finalNoteCount, finalStandard, finalGears, finalHearts, finalBlueNotes,
                    finalPerfect, finalGreat, finalMiss, finalScore, finalMaxCombo, finalAccuracy,
                    finalIsFullCombo, finalIsAllPerfect, updatedPlayCount, finalSavedAt, fingerprint, recordKey);

                KeepBackupBeforeOverwrite(filePath);

                // 쓰는 도중 크래시가 나도 옛 기록이 잘리지 않도록 임시 파일을 거쳐 교체합니다(AtomicFile 참고).
                AtomicFile.WriteAllText(filePath, json, Encoding.UTF8);
                RecordCache.Invalidate(filePath);
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

        /// <summary>
        /// 덮어쓰기 전에 기존 기록 파일을 <c>.bak</c>으로 한 번 남깁니다. 옛 버전(지문 없는 기록 등)은 화면에 나오지 않지만
        /// 파일이 사라지면 복구할 수 없으므로, 처음 덮어쓸 때의 원본을 보관합니다. 코드는 이 파일을 읽지 않습니다.
        /// 백업 실패가 기록 저장을 막지 않도록 예외는 경고로만 남깁니다.
        /// </summary>
        private static void KeepBackupBeforeOverwrite(string filePath)
        {
            try
            {
                string backupPath = filePath + ".bak";
                if (File.Exists(filePath) && !File.Exists(backupPath))
                {
                    File.Copy(filePath, backupPath);
                }
            }
            catch (Exception ex)
            {
                ModLogger.Warning($"[CustomRecordStore] 기존 기록의 .bak 백업에 실패했습니다(저장은 계속합니다): {filePath}, {ex.Message}");
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

            /// <summary>기록 파일의 키로 쓰인 곡 폴더 이름입니다(파일을 열었을 때 어느 곡인지 알아보기 위한 것).</summary>
            public string songFolder = string.Empty;

            internal PlayRecord Clone() => (PlayRecord)MemberwiseClone();
        }

        /// <summary>
        /// 기록 파일을 파싱한 결과입니다. 곡 선택/준비 패널이 갱신될 때마다(준비 화면은 즉시 + 0.25초 + 1초 지연 적용)
        /// 같은 파일을 다시 열어 파싱하지 않도록, 파일이 그대로인 동안 재사용합니다.
        ///
        /// <para>키는 uid가 아니라 <b>파일 경로</b>입니다. uid는 폴더 순번이라 밀릴 수 있고, 경로는
        /// <see cref="FindExistingRecordPath"/>가 매번 지금의 곡 폴더 이름으로 다시 해석합니다.
        /// 보관하는 것은 파싱 결과뿐이고, 채보 지문 대조(<see cref="BelongsToCurrentChart"/>)는 매번 다시 합니다.</para>
        /// </summary>
        private static readonly FileStampCache<PlayRecord> RecordCache = new FileStampCache<PlayRecord>();

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

                string filePath = FindExistingRecordPath(uid, difficulty);
                if (filePath == null) return null;

                // 보관본을 호출자가 고쳐도 다음 조회에 번지지 않도록 사본을 돌려줍니다.
                var record = RecordCache.GetOrLoad(filePath, ReadRecordFile, out bool readFromDisk)?.Clone();

                if (!BelongsToCurrentChart(record, uid, difficulty, filePath)) return null;

                // 파일을 실제로 읽었을 때만 남깁니다. 패널 갱신마다 찍으면 이 로그 자체가 콘솔·디스크 I/O가 됩니다.
                if (readFromDisk)
                {
                    ModLogger.Msg($"[CustomRecordStore] 기록 로드 성공 → {filePath} (playCount={record?.playCount}, score={record?.score}, acc={record?.accuracy:0.0000}, FC={record?.isFullCombo})");
                }
                return record;
            }
            catch (Exception ex)
            {
                ModLogger.Error($"[CustomRecordStore] 기록 로드 중 예외 (uid={uid}, diff={difficulty}): {ex}");
                return null;
            }
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
