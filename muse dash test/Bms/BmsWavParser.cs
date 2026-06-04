using System;
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
                if (info.Uid.StartsWith("0002"))
                {
                    info.NoteType = 6; // HP / Heart
                    info.KeyAudio = "sfx_hp";
                }
                else if (info.Uid.StartsWith("0003"))
                {
                    info.NoteType = 7; // Score Note
                    info.KeyAudio = "sfx_score";
                }
                else if (xx == "02")
                {
                    info.NoteType = 3; // Hold / Long note
                }
                else if (xx == "04")
                {
                    info.NoteType = 8; // Sandbag / Multi-hit
                }
                else if (xx == "03" || xx == "09")
                {
                    info.NoteType = 2; // Obstacle / Gear / Boss Gear
                }
                else if (xx == "17")
                {
                    info.NoteType = 4; // Ghost
                }

                ApplyBossTransitionFromXxyy(info, xxyy);

                // 4. 보스 발사체 (xx=06/07/08, Type 1) 및 보스 톱니 (xx=09, Type 2) 처리
                bool isBossProjectile = xx == "06" || xx == "07" || xx == "08";
                bool isBossGear = xx == "09";

                if (isBossProjectile || isBossGear)
                {
                    // 영어 예약 키워드(_boss, _atk) 감지 시 '보스 있는 버전'으로 분류
                    if (lowerName.Contains("_boss") || lowerName.Contains("_atk"))
                    {
                        // 파일명에 구체적인 액션명이 직접 명시되어 있는지 우선 확인
                        if (lowerName.Contains("boss_far_atk_1_r"))
                        {
                            info.BossAction = "boss_far_atk_1_R";
                        }
                        else if (lowerName.Contains("boss_far_atk_1_l"))
                        {
                            info.BossAction = "boss_far_atk_1_L";
                        }
                        else if (lowerName.Contains("boss_far_atk_2"))
                        {
                            info.BossAction = "boss_far_atk_2";
                        }
                        // 명시되지 않은 경우 UID 패턴에 기반하여 가장 어울리는 액션 자동 매핑
                        else
                        {
                            if (xxyy == "0601" || xxyy == "0902" || xxyy == "0903")
                            {
                                info.BossAction = "boss_far_atk_1_R";
                            }
                            else if (xxyy == "0604" || xxyy == "0906")
                            {
                                info.BossAction = "boss_far_atk_1_L";
                            }
                            else if (xxyy == "0701" || xxyy == "0704" || xxyy == "0801" || xxyy == "0804" || 
                                     xxyy == "0908" || xxyy == "0909" || xxyy == "0911" || xxyy == "0912")
                            {
                                info.BossAction = "boss_far_atk_2";
                            }
                        }

                        // 보스 모션 싱크용 권장 dt인 0.7초 적용
                        info.Dt = 0.7;
                    }
                    else
                    {
                        // 보스 없는 버전이므로 BossAction을 비움
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
            if (info == null || string.IsNullOrWhiteSpace(xxyy))
            {
                return;
            }

            if (string.Equals(xxyy, "0101", StringComparison.OrdinalIgnoreCase))
            {
                info.NoteType = 0;
                info.PrefabName = "empty_000";
                info.BossAction = "in";
                info.BossTransition = "in";
                ApplyBossTargetFromUid(info);
            }
            else if (string.Equals(xxyy, "0102", StringComparison.OrdinalIgnoreCase))
            {
                info.NoteType = 0;
                info.PrefabName = "empty_000";
                info.BossAction = "out";
                info.BossTransition = "out";
                ApplyBossTargetFromUid(info);
            }
            else if (string.Equals(xxyy, "0107", StringComparison.OrdinalIgnoreCase))
            {
                ApplyBossActionTrigger(info, "boss_far_atk_1_start");
            }
            else if (string.Equals(xxyy, "0108", StringComparison.OrdinalIgnoreCase))
            {
                ApplyBossActionTrigger(info, "boss_far_atk_1_end");
            }
            else if (string.Equals(xxyy, "0109", StringComparison.OrdinalIgnoreCase))
            {
                ApplyBossActionTrigger(info, "boss_far_atk_2_start");
            }
            else if (string.Equals(xxyy, "0110", StringComparison.OrdinalIgnoreCase))
            {
                ApplyBossActionTrigger(info, "boss_far_atk_2_end");
            }
        }

        private static void ApplyBossActionTrigger(BmsWavInfo info, string bossAction)
        {
            if (info == null)
            {
                return;
            }

            info.NoteType = 0;
            info.PrefabName = "empty_000";
            info.BossAction = bossAction;
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
