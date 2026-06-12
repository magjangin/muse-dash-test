using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

namespace muse_dash_test
{
    public sealed class BmsWavInfo
    {
        public string RawWavName { get; set; }
        public string Uid { get; set; }
        public string PrefabName { get; set; }
        public double Dt { get; set; } = -1.0;
        public int NoteType { get; set; } = 1; // Default to normal note
        public string KeyAudio { get; set; }
        public string BossAction { get; set; }
        public string BossTransition { get; set; }
        public string BossName { get; set; }
        public int BossScene { get; set; } = -1;
    }

    public static class BmsWavParser
    {
        private static readonly Regex DtRegex = new Regex(@"_dt([0-9]+(?:\.[0-9]+)?)(?:\.wav)?$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex UidRegex = new Regex(@"^([0-9]{6})", RegexOptions.Compiled);

        // UID 앞 4자리 → NoteType (접두사 우선 매핑)
        private static readonly Dictionary<string, (int noteType, string keyAudio)> UidPrefixNoteType =
            new Dictionary<string, (int, string)>(StringComparer.OrdinalIgnoreCase)
        {
            { "0002", (6, "sfx_hp") },    // HP / Heart
            { "0003", (7, "sfx_score") }, // Score Note
        };

        // UID xx(2~3번째 자리) → NoteType
        private static readonly Dictionary<string, int> XxNoteType =
            new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
        {
            { "02", 3 }, // Hold / Long note
            { "03", 2 }, // Obstacle / Boss Gear
            { "04", 8 }, // Sandbag / Multi-hit
            { "09", 2 }, // Boss Gear
            { "17", 4 }, // Ghost
        };

        // xxyy → (BossAction, BossTransition?) — xx=01 보스 전환 테이블
        private static readonly Dictionary<string, (string action, string transition)> XxyyTransitionMap =
            new Dictionary<string, (string, string)>(StringComparer.OrdinalIgnoreCase)
        {
            { "0101", ("in",  "in") },
            { "0102", ("out", "out") },
            { "0107", ("boss_far_atk_1_start", null) },
            { "0108", ("boss_far_atk_1_end",   null) },
            { "0109", ("boss_far_atk_2_start", null) },
            { "0110", ("boss_far_atk_2_end",   null) },
        };

        // xxyy → BossAction — 보스 발사체/톱니 자동 매핑 테이블
        private static readonly Dictionary<string, string> XxyyProjectileAction =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "0601", "boss_far_atk_1_R" }, { "0902", "boss_far_atk_1_R" }, { "0903", "boss_far_atk_1_R" },
            { "0604", "boss_far_atk_1_L" }, { "0906", "boss_far_atk_1_L" },
            { "0701", "boss_far_atk_2" }, { "0704", "boss_far_atk_2" },
            { "0801", "boss_far_atk_2" }, { "0804", "boss_far_atk_2" },
            { "0908", "boss_far_atk_2" }, { "0909", "boss_far_atk_2" },
            { "0911", "boss_far_atk_2" }, { "0912", "boss_far_atk_2" },
        };

        public static BmsWavInfo ParseWavName(string wavName)
        {
            if (string.IsNullOrWhiteSpace(wavName))
            {
                return null;
            }

            var info = new BmsWavInfo { RawWavName = wavName };
            string nameWithoutExt = Path.GetFileNameWithoutExtension(wavName);

            // 1. Parse UID (6 digits at the start of filename, e.g., 051001)
            var uidMatch = UidRegex.Match(nameWithoutExt);
            if (uidMatch.Success)
            {
                info.Uid = uidMatch.Groups[1].Value;
                info.PrefabName = NormalizePrefabName(nameWithoutExt, info.Uid);
            }

            // 2. Parse dt (e.g. _dt0.7 or _dt1.2)
            var dtMatch = DtRegex.Match(nameWithoutExt);
            if (dtMatch.Success)
            {
                if (double.TryParse(dtMatch.Groups[1].Value, NumberStyles.Float, CultureInfo.InvariantCulture, out double parsedDt))
                {
                    info.Dt = Math.Round(parsedDt, 3, MidpointRounding.AwayFromZero);
                }

                info.PrefabName = NormalizePrefabName(nameWithoutExt, info.Uid);
            }

            // 3. Skeleton mapping for special gameplay notes & audios
            string lowerName = nameWithoutExt.ToLowerInvariant();

            // Check UID xx structure (zzxxyy)
            if (info.Uid != null && info.Uid.Length == 6)
            {
                string xx = info.Uid.Substring(2, 2);
                string xxyy = info.Uid.Substring(2, 4);
                string prefix4 = info.Uid.Substring(0, 4);

                if (UidPrefixNoteType.TryGetValue(prefix4, out var prefixEntry))
                {
                    info.NoteType = prefixEntry.noteType;
                    info.KeyAudio = prefixEntry.keyAudio;
                }
                else if (XxNoteType.TryGetValue(xx, out int xxType))
                {
                    info.NoteType = xxType;
                }

                ApplyBossTransitionFromXxyy(info, xxyy);

                // 4. 보스 발사체 (xx=06/07/08, Type 1) 및 보스 톱니 (xx=09, Type 2) 처리
                bool isBossProjectile = xx == "06" || xx == "07" || xx == "08";
                bool isBossGear = xx == "09";

                if (isBossProjectile || isBossGear)
                {
                    if (lowerName.Contains("_boss") || lowerName.Contains("_atk"))
                    {
                        if (lowerName.Contains("boss_far_atk_1_r"))
                            info.BossAction = "boss_far_atk_1_R";
                        else if (lowerName.Contains("boss_far_atk_1_l"))
                            info.BossAction = "boss_far_atk_1_L";
                        else if (lowerName.Contains("boss_far_atk_2"))
                            info.BossAction = "boss_far_atk_2";
                        else if (XxyyProjectileAction.TryGetValue(xxyy, out string mappedAction))
                            info.BossAction = mappedAction;

                        info.Dt = 0.7;
                    }
                    else
                    {
                        info.BossAction = "";
                    }
                }
            }

            // String-based pattern matching and overrides for fallbacks
            if (lowerName.Contains("heart") || lowerName.Contains("hp") || (info.Uid != null && info.Uid.StartsWith("0002")))
            {
                info.NoteType = 6;
                info.KeyAudio = "sfx_hp";
            }
            else if (lowerName.Contains("score") || lowerName.Contains("note") || (info.Uid != null && info.Uid.StartsWith("0003")))
            {
                info.NoteType = 7;
                info.KeyAudio = "sfx_score";
            }
            else if (lowerName.Contains("boss_swap"))
            {
                info.NoteType = 0;
                info.PrefabName = "empty_000";
                info.BossAction = "swap:0401_boss:4"; // Skeleton default swap redirection
            }
            else if (lowerName.Contains("boss_out"))
            {
                info.NoteType = 0;
                info.PrefabName = "empty_000";
                info.BossAction = "out";
                info.BossTransition = "out";
            }
            else if (lowerName.Contains("boss_in"))
            {
                info.NoteType = 0;
                info.PrefabName = "empty_000";
                info.BossAction = "in";
                info.BossTransition = "in";
                ApplyBossTargetFromName(info, nameWithoutExt);
            }
            else if (lowerName.Contains("sandbag") || (info.Uid != null && info.Uid.Substring(2, 2) == "04"))
            {
                info.NoteType = 8;
            }
            else if (lowerName.Contains("hold") || lowerName.Contains("long") || (info.Uid != null && info.Uid.Substring(2, 2) == "02"))
            {
                info.NoteType = 3;
            }
            else if (info.Uid != null && info.Uid.Substring(2, 2) == "17")
            {
                info.NoteType = 4; // Ghost
            }

            return info;
        }

        private static void ApplyBossTargetFromName(BmsWavInfo info, string nameWithoutExt)
        {
            if (info == null || string.IsNullOrWhiteSpace(nameWithoutExt))
            {
                return;
            }

            var bossNameMatch = Regex.Match(nameWithoutExt, @"([0-9]{4}_boss)", RegexOptions.IgnoreCase);
            if (bossNameMatch.Success)
            {
                info.BossName = bossNameMatch.Groups[1].Value.ToLowerInvariant();
            }

            var sceneMatch = Regex.Match(nameWithoutExt, @"(?:scene|sc|s)([0-9]{1,2})", RegexOptions.IgnoreCase);
            if (sceneMatch.Success && int.TryParse(sceneMatch.Groups[1].Value, NumberStyles.Integer, CultureInfo.InvariantCulture, out int scene))
            {
                info.BossScene = scene;
            }

            if (string.IsNullOrWhiteSpace(info.BossName) && !string.IsNullOrWhiteSpace(info.Uid) && info.Uid.Length >= 2)
            {
                info.BossName = info.Uid.Substring(0, 2) + "01_boss";
            }

            if (info.BossScene < 0 && !string.IsNullOrWhiteSpace(info.Uid) && info.Uid.Length >= 2)
            {
                if (int.TryParse(info.Uid.Substring(0, 2), NumberStyles.Integer, CultureInfo.InvariantCulture, out int uidScene))
                {
                    info.BossScene = uidScene;
                }
            }
        }

        private static string NormalizePrefabName(string nameWithoutExt, string uid)
        {
            if (string.IsNullOrWhiteSpace(nameWithoutExt))
            {
                return uid;
            }

            string prefabName = nameWithoutExt;
            int dtIdx = prefabName.IndexOf("_dt", StringComparison.OrdinalIgnoreCase);
            if (dtIdx > 0)
            {
                prefabName = prefabName.Substring(0, dtIdx);
            }

            if (string.IsNullOrWhiteSpace(uid))
            {
                return prefabName;
            }

            if (ContainsHumanLabel(prefabName))
            {
                return uid;
            }

            return prefabName;
        }

        private static bool ContainsHumanLabel(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return false;
            }

            foreach (char ch in text)
            {
                if (ch > 127 || char.IsWhiteSpace(ch) || ch == '(' || ch == ')' || ch == '[' || ch == ']')
                {
                    return true;
                }
            }

            return false;
        }

        private static void ApplyBossTransitionFromXxyy(BmsWavInfo info, string xxyy)
        {
            if (info == null || string.IsNullOrWhiteSpace(xxyy)) return;
            if (!XxyyTransitionMap.TryGetValue(xxyy, out var entry)) return;

            info.NoteType = 0;
            info.PrefabName = "empty_000";
            info.BossAction = entry.action;
            if (entry.transition != null)
                info.BossTransition = entry.transition;
            ApplyBossTargetFromUid(info);
        }

        private static void ApplyBossTargetFromUid(BmsWavInfo info)
        {
            if (info == null || string.IsNullOrWhiteSpace(info.Uid) || info.Uid.Length < 2)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(info.BossName))
            {
                info.BossName = info.Uid.Substring(0, 2) + "01_boss";
            }

            if (info.BossScene < 0 && int.TryParse(info.Uid.Substring(0, 2), NumberStyles.Integer, CultureInfo.InvariantCulture, out int scene))
            {
                info.BossScene = scene;
            }
        }
    }
}
