# -*- coding: utf-8 -*-
import io, sys

ROOT = r"H:\source\repos\muse dash test\muse dash test"

def apply(path, pairs):
    with io.open(path, "r", encoding="utf-8", newline="") as f:
        content = f.read()
    nl = "\r\n" if "\r\n" in content else "\n"
    for old, new in pairs:
        old_n = old.replace("\r\n", "\n").replace("\n", nl)
        new_n = new.replace("\r\n", "\n").replace("\n", nl)
        c = content.count(old_n)
        if c != 1:
            print(f"ABORT {path}: expected 1 match, found {c} for block starting:\n  {old.strip().splitlines()[0][:70]}")
            sys.exit(1)
        content = content.replace(old_n, new_n)
    with io.open(path, "w", encoding="utf-8", newline="") as f:
        f.write(content)
    print(f"OK   {path}  ({len(pairs)} edit(s), newline={'CRLF' if nl=='\\r\\n' else 'LF'})")


# ---------------- 1) PnlMusicUtils.Log.cs : TryLogCompact ----------------
log_old = '''            string title = null, artist = null, album = null;
            foreach (var f in t.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                try
                {
                    var v = f.GetValue(pnlInstance); if (v == null) continue; var lname = f.Name.ToLowerInvariant();
                    if (title == null && (lname.Contains("song") || lname.Contains("title") || lname.Contains("music") || lname.Contains("name"))) title = SafeGetProp(v, "text") ?? SafeGetProp(v, "m_Text") ?? SafeGetProp(v, "name") ?? (v as string);
                    if (artist == null && lname.Contains("artist")) artist = SafeGetProp(v, "text") ?? SafeGetProp(v, "m_Text") ?? SafeGetProp(v, "name") ?? (v as string);
                    if (album == null && lname.Contains("album")) album = SafeGetProp(v, "text") ?? SafeGetProp(v, "m_Text") ?? SafeGetProp(v, "name") ?? (v as string);
                }
                catch { }
            }
            foreach (var p in t.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                try
                {
                    if (p.GetIndexParameters().Length > 0) continue; var v = p.GetValue(pnlInstance); if (v == null) continue; var lname = p.Name.ToLowerInvariant();
                    if (title == null && (lname.Contains("song") || lname.Contains("title") || lname.Contains("music") || lname.Contains("name"))) title = SafeGetProp(v, "text") ?? SafeGetProp(v, "m_Text") ?? SafeGetProp(v, "name") ?? (v as string);
                    if (artist == null && lname.Contains("artist")) artist = SafeGetProp(v, "text") ?? SafeGetProp(v, "m_Text") ?? SafeGetProp(v, "name") ?? (v as string);
                    if (album == null && lname.Contains("album")) album = SafeGetProp(v, "text") ?? SafeGetProp(v, "m_Text") ?? SafeGetProp(v, "name") ?? (v as string);
                }
                catch { }
            }
            if (string.IsNullOrEmpty(title) && string.IsNullOrEmpty(artist) && string.IsNullOrEmpty(album)) return false;
            MelonLogger.Msg($"NowPlaying: {(string.IsNullOrEmpty(title) ? "(unknown)" : title)} - {(string.IsNullOrEmpty(artist) ? "(unknown)" : artist)} - {(string.IsNullOrEmpty(album) ? "(unknown)" : album)}");
            return true;
        }
    }'''

log_new = '''            string title = null, artist = null, album = null;
            foreach (var f in t.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                try
                {
                    var v = f.GetValue(pnlInstance); if (v == null) continue;
                    MatchNowPlayingMember(f.Name, v, ref title, ref artist, ref album);
                }
                catch { }
            }
            foreach (var p in t.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                try
                {
                    if (p.GetIndexParameters().Length > 0) continue; var v = p.GetValue(pnlInstance); if (v == null) continue;
                    MatchNowPlayingMember(p.Name, v, ref title, ref artist, ref album);
                }
                catch { }
            }
            if (string.IsNullOrEmpty(title) && string.IsNullOrEmpty(artist) && string.IsNullOrEmpty(album)) return false;
            MelonLogger.Msg($"NowPlaying: {(string.IsNullOrEmpty(title) ? "(unknown)" : title)} - {(string.IsNullOrEmpty(artist) ? "(unknown)" : artist)} - {(string.IsNullOrEmpty(album) ? "(unknown)" : album)}");
            return true;
        }

        // 멤버 이름(필드/프로퍼티 공용)으로 title/artist/album을 채운다. 이미 채워진 값은
        // 덮어쓰지 않는다. 필드 루프와 프로퍼티 루프가 똑같이 쓰던 매칭 로직을 한곳으로 모은 것.
        private static void MatchNowPlayingMember(string memberName, object value, ref string title, ref string artist, ref string album)
        {
            var lname = memberName.ToLowerInvariant();
            if (title == null && (lname.Contains("song") || lname.Contains("title") || lname.Contains("music") || lname.Contains("name"))) title = SafeGetProp(value, "text") ?? SafeGetProp(value, "m_Text") ?? SafeGetProp(value, "name") ?? (value as string);
            if (artist == null && lname.Contains("artist")) artist = SafeGetProp(value, "text") ?? SafeGetProp(value, "m_Text") ?? SafeGetProp(value, "name") ?? (value as string);
            if (album == null && lname.Contains("album")) album = SafeGetProp(value, "text") ?? SafeGetProp(value, "m_Text") ?? SafeGetProp(value, "name") ?? (value as string);
        }
    }'''

apply(ROOT + r"\Patches\UI\Common\Pnl\PnlMusicUtils.Log.cs", [(log_old, log_new)])


# ---------------- 2) BmsWavParser.cs : ParseWavName (3 surgical edits) ----------------
wav_a_old = '''                // 4. 보스 발사체 (xx=06/07/08, Type 1) 및 보스 톱니 (xx=09, Type 2) 처리
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
                }'''

wav_a_new = '''                // 4. 보스 발사체 (xx=06/07/08, Type 1) 및 보스 톱니 (xx=09, Type 2) 처리
                ApplyBossProjectileAction(info, lowerName, xx, xxyy);'''

wav_b_old = '''            // String-based pattern matching and overrides for fallbacks
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
            }'''

wav_b_new = '''            // String-based pattern matching and overrides for fallbacks
            ApplyFallbackNoteType(info, lowerName, nameWithoutExt);'''

wav_c_old = '''            return info;
        }

        private static void ApplyBossTargetFromName(BmsWavInfo info, string nameWithoutExt)'''

wav_c_new = '''            return info;
        }

        // xx=06/07/08(보스 발사체) 또는 xx=09(보스 톱니)일 때만 보스 공격 액션/Dt를 설정한다.
        private static void ApplyBossProjectileAction(BmsWavInfo info, string lowerName, string xx, string xxyy)
        {
            bool isBossProjectile = xx == "06" || xx == "07" || xx == "08";
            bool isBossGear = xx == "09";

            if (!isBossProjectile && !isBossGear)
            {
                return;
            }

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

        // 파일명 문자열/UID 기반 폴백: 위에서 결정되지 않은 노트 타입·보스 전환을 보정한다.
        private static void ApplyFallbackNoteType(BmsWavInfo info, string lowerName, string nameWithoutExt)
        {
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
        }

        private static void ApplyBossTargetFromName(BmsWavInfo info, string nameWithoutExt)'''

apply(ROOT + r"\Bms\BmsWavParser.cs", [(wav_a_old, wav_a_new), (wav_b_old, wav_b_new), (wav_c_old, wav_c_new)])


# ---------------- 3) HwaMenuBgmController.cs : FindMenuAudioSource ----------------
menu_old = '''                // 0단계: GameObject 이름이 "BGM"인 AudioSource 탐색 (재생 여부, 클립 여부 무관)
                foreach (AudioSource source in sources)
                {
                    if (source == null || source.gameObject == null || !source.gameObject.activeInHierarchy) continue;
                    if (source.gameObject.name.Equals("BGM", StringComparison.OrdinalIgnoreCase))
                    {
                        MelonLogger.Msg($"[MenuBGM] 0단계(이름 매칭) 성공: GO={source.gameObject.name}");
                        return source;
                    }
                }

                // 1단계: 재생 중이며 이름이 music, bgm, demo 등 사운드 트랙 계열의 클립이나 오브젝트명을 갖는 AudioSource 탐색
                foreach (AudioSource source in sources)
                {
                    if (source == null || source.gameObject == null || !source.gameObject.activeInHierarchy) continue;
                    if (source.clip == null) continue;

                    string clipName = source.clip.name.ToLower();
                    string goName = source.gameObject.name.ToLower();

                    if (source.isPlaying && (clipName.Contains("demo") || clipName.Contains("music") || clipName.Contains("bgm") || goName.Contains("music") || goName.Contains("bgm")))
                    {
                        MelonLogger.Msg($"[MenuBGM] 1단계 매칭 성공: GO={goName}, Clip={source.clip.name}");
                        return source;
                    }
                }

                // 2단계: 효과음(click, SFX) 등을 제외한 나머지 재생 중인 BGM AudioSource 탐색
                foreach (AudioSource source in sources)
                {
                    if (source == null || source.gameObject == null || !source.gameObject.activeInHierarchy) continue;
                    if (source.clip == null) continue;

                    string clipName = source.clip.name.ToLower();
                    string goName = source.gameObject.name.ToLower();
                    if (source.isPlaying && !clipName.Contains("click") && !clipName.Contains("sfx") && !clipName.Contains("button"))
                    {
                        MelonLogger.Msg($"[MenuBGM] 2단계 매칭 성공: GO={goName}, Clip={source.clip.name}");
                        return source;
                    }
                }'''

menu_new = '''                // 활성 상태(GameObject 존재 + activeInHierarchy)인 AudioSource만 순서대로 검사한다.
                // 0/1/2단계가 똑같이 반복하던 null·active 가드를 한곳으로 모은 헬퍼.
                AudioSource FindActive(System.Func<AudioSource, bool> predicate)
                {
                    foreach (AudioSource source in sources)
                    {
                        if (source == null || source.gameObject == null || !source.gameObject.activeInHierarchy) continue;
                        if (predicate(source)) return source;
                    }
                    return null;
                }

                // 0단계: GameObject 이름이 "BGM"인 AudioSource 탐색 (재생 여부, 클립 여부 무관)
                AudioSource byName = FindActive(s => s.gameObject.name.Equals("BGM", StringComparison.OrdinalIgnoreCase));
                if (byName != null)
                {
                    MelonLogger.Msg($"[MenuBGM] 0단계(이름 매칭) 성공: GO={byName.gameObject.name}");
                    return byName;
                }

                // 1단계: 재생 중이며 이름이 music, bgm, demo 등 사운드 트랙 계열의 클립이나 오브젝트명을 갖는 AudioSource 탐색
                AudioSource bySoundtrack = FindActive(s =>
                {
                    if (s.clip == null || !s.isPlaying) return false;
                    string clipName = s.clip.name.ToLower();
                    string goName = s.gameObject.name.ToLower();
                    return clipName.Contains("demo") || clipName.Contains("music") || clipName.Contains("bgm") || goName.Contains("music") || goName.Contains("bgm");
                });
                if (bySoundtrack != null)
                {
                    MelonLogger.Msg($"[MenuBGM] 1단계 매칭 성공: GO={bySoundtrack.gameObject.name.ToLower()}, Clip={bySoundtrack.clip.name}");
                    return bySoundtrack;
                }

                // 2단계: 효과음(click, SFX) 등을 제외한 나머지 재생 중인 BGM AudioSource 탐색
                AudioSource byElimination = FindActive(s =>
                {
                    if (s.clip == null || !s.isPlaying) return false;
                    string clipName = s.clip.name.ToLower();
                    return !clipName.Contains("click") && !clipName.Contains("sfx") && !clipName.Contains("button");
                });
                if (byElimination != null)
                {
                    MelonLogger.Msg($"[MenuBGM] 2단계 매칭 성공: GO={byElimination.gameObject.name.ToLower()}, Clip={byElimination.clip.name}");
                    return byElimination;
                }'''

apply(ROOT + r"\Patches\UI\Common\Hwa\HwaMenuBgmController.cs", [(menu_old, menu_new)])

print("ALL DONE")
