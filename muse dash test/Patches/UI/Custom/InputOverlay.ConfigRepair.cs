using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using MelonLoader;

namespace muse_dash_test
{
    // config.txt가 같은 설정 줄로 부풀어 오른 것을 1회성으로 정리합니다.
    public static partial class InputOverlay
    {
        /// <summary>누락 보충이 append 하던 머리말. 이 줄이 여러 개면 중복 append가 있었다는 뜻입니다.</summary>
        private const string AutoAppendMarker = "# [자동 업데이트] 누락된 설정 항목 추가";

        /// <summary>
        /// 과거 버그(공백 있는 키를 공백 없는 키로 검사)로 인해 <see cref="EnsureMissingKeysAdded"/>가
        /// 매 폴링마다 같은 줄을 덧붙여 config.txt가 무한히 커졌습니다. 그 흔적을 1회 정리합니다.
        ///
        /// <para><b>동작이 바뀌지 않는 이유</b>: <c>ParseConfigFile</c>은 위에서 아래로 훑으며 같은 키를
        /// 만나면 뒤엣값으로 덮어씁니다(나중 승리). 그래서 각 키의 <i>마지막</i> 줄만 남기면
        /// 정리 전후의 최종 설정값이 정확히 같습니다.</para>
        ///
        /// <para>중복이 없으면 파일을 건드리지 않고 즉시 반환합니다.</para>
        /// </summary>
        private static void RepairDuplicatedAutoAppendedKeys()
        {
            try
            {
                if (!File.Exists(configPath)) return;

                string text = ReadConfigTextRobust();
                if (string.IsNullOrEmpty(text)) return;

                string[] lines = text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

                // 1) 키별 등장 횟수와 마지막 등장 위치를 셉니다.
                var lastIndexByKey = new Dictionary<string, int>(StringComparer.Ordinal);
                var countByKey = new Dictionary<string, int>(StringComparer.Ordinal);
                int markerCount = 0;

                for (int i = 0; i < lines.Length; i++)
                {
                    string trimmed = lines[i].Trim();
                    if (trimmed.Length == 0) continue;

                    if (trimmed.StartsWith("#", StringComparison.Ordinal))
                    {
                        if (string.Equals(trimmed, AutoAppendMarker, StringComparison.Ordinal)) markerCount++;
                        continue;
                    }

                    if (!TryGetNormalizedKey(trimmed, out string key)) continue;

                    lastIndexByKey[key] = i;
                    countByKey[key] = countByKey.TryGetValue(key, out int prev) ? prev + 1 : 1;
                }

                bool hasDuplicateKey = false;
                foreach (var pair in countByKey)
                {
                    if (pair.Value > 1) { hasDuplicateKey = true; break; }
                }

                if (!hasDuplicateKey && markerCount <= 1) return;

                // 2) 각 키의 마지막 줄만 남기고, 중복된 자동 추가 머리말과 연속 빈 줄을 걷어냅니다.
                var kept = new List<string>(lines.Length);
                bool lastKeptWasBlank = false;

                for (int i = 0; i < lines.Length; i++)
                {
                    string raw = lines[i];
                    string trimmed = raw.Trim();

                    if (trimmed.Length == 0)
                    {
                        if (lastKeptWasBlank) continue; // 연속 빈 줄 접기
                        kept.Add(string.Empty);
                        lastKeptWasBlank = true;
                        continue;
                    }

                    if (trimmed.StartsWith("#", StringComparison.Ordinal))
                    {
                        // 중복 append의 머리말은 전부 걷어냅니다(설정값에는 영향 없는 주석).
                        if (string.Equals(trimmed, AutoAppendMarker, StringComparison.Ordinal)) continue;

                        kept.Add(raw);
                        lastKeptWasBlank = false;
                        continue;
                    }

                    if (TryGetNormalizedKey(trimmed, out string key)
                        && lastIndexByKey.TryGetValue(key, out int lastIndex)
                        && lastIndex != i)
                    {
                        continue; // 같은 키의 더 뒤쪽 줄이 있으므로 이 줄은 버립니다.
                    }

                    kept.Add(raw);
                    lastKeptWasBlank = false;
                }

                // 꼬리의 빈 줄 정리
                while (kept.Count > 0 && kept[kept.Count - 1].Trim().Length == 0)
                {
                    kept.RemoveAt(kept.Count - 1);
                }

                int removed = lines.Length - kept.Count;
                if (removed <= 0) return;

                File.WriteAllText(configPath, string.Join(Environment.NewLine, kept) + Environment.NewLine, new UTF8Encoding(true));
                MelonLogger.Msg(
                    $"[InputOverlay] config.txt에 중복 누적된 설정 줄 {removed}개를 정리했습니다 " +
                    $"({lines.Length}줄 → {kept.Count}줄). 각 항목의 마지막 값만 남겨 설정값은 그대로입니다.");
            }
            catch (Exception ex)
            {
                // 정리는 부가 작업입니다. 실패해도 설정 읽기 자체는 계속되어야 하므로 쓰기만 중단합니다.
                hasFailedToWrite = true;
                MelonLogger.Error($"[InputOverlay] config.txt 중복 정리 중 예외 발생 (쓰기 시도가 중단됩니다): {ex.Message}");
            }
        }

        /// <summary>`키=값` 줄에서 비교용으로 정규화한 키(공백 제거 + 소문자)를 얻습니다.</summary>
        private static bool TryGetNormalizedKey(string trimmedLine, out string key)
        {
            key = null;

            int idx = trimmedLine.IndexOf('=');
            if (idx <= 0) return false;

            key = StripSpaces(trimmedLine.Substring(0, idx)).ToLowerInvariant();
            return key.Length > 0;
        }
    }
}
