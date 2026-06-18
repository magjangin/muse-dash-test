using Il2CppAssets.Scripts.Database;
using Il2CppGameLogic;

// UID 해석(프리팹/오디오/dt/pathway), MusicData/NoteConfigData 복제, 수치 정규화 헬퍼.
public partial class DBStageInfo_SetRuntimeMusicData_Patch
{
    public static bool IsUidMiddlePair(string uid, string middlePair)
    {
        if (string.IsNullOrEmpty(uid) || string.IsNullOrEmpty(middlePair) || uid.Length < 4 || middlePair.Length != 2)
        {
            return false;
        }

        return string.Equals(uid.Substring(2, 2), middlePair, System.StringComparison.OrdinalIgnoreCase);
    }

    public static string BuildPrefabName(string uid, int noteType, int pathway)
    {
        if (string.IsNullOrEmpty(uid) || uid.Length < 6)
            return uid;

        string xx = uid.Substring(2, 2);
        string yy = uid.Substring(4, 2);
        string lane = pathway == 1 ? "air" : "road";
        string motion = "nor";
        string variant = GetPrefabVariant(uid, noteType);

        if (xx == "15") motion = "down";
        else if (xx == "16") motion = "up";
        else if (yy == "07" || yy == "10") motion = "up";
        else if (yy == "13" || yy == "16") motion = "down";

        return $"{uid}_{lane}_{motion}_{variant}";
    }

    public static string GetPrefabVariant(string uid, int noteType)
    {
        if (string.Equals(uid, "000301", System.StringComparison.OrdinalIgnoreCase)
            || string.Equals(uid, "000304", System.StringComparison.OrdinalIgnoreCase))
        {
            return "1";
        }

        if (noteType == 7)
        {
            return "2";
        }

        return "1";
    }

    public static string GetPathwayLabel(int pathway)
    {
        return pathway == 1 ? "공중" : "지상";
    }

    public static bool ShouldUseEmptyBossActionPrefab(int noteType, string uid, string bossAction)
    {
        return !string.IsNullOrEmpty(bossAction) && noteType == 0 && !IsBossProjectileUid(uid);
    }

    public static bool IsBossProjectileUid(string uid)
    {
        if (string.IsNullOrEmpty(uid) || uid.Length < 6) return false;

        string xx = uid.Substring(2, 2);
        string yy = uid.Substring(4, 2);
        bool projectileSeries = xx == "06" || xx == "07" || xx == "08";
        bool normalLane = yy == "01" || yy == "04";
        return projectileSeries && normalLane;
    }

    public static string ResolvePrefabName(ExperimentNoteSpec spec, int noteType, string uid, int pathway)
    {
        if (!string.IsNullOrEmpty(spec.PrefabName)) return spec.PrefabName;
        if (ShouldUseEmptyBossActionPrefab(noteType, uid, spec.BossAction)) return "empty_000";
        if (!string.IsNullOrEmpty(uid)) return BuildPrefabName(uid, noteType, pathway);
        return null; // keep cloned value
    }

    public static string ResolveKeyAudio(ExperimentNoteSpec spec, int noteType, string uid)
    {
        if (!string.IsNullOrEmpty(spec.KeyAudio)) return spec.KeyAudio;
        if (!string.IsNullOrEmpty(spec.BossAction)) return "";
        if (noteType == 1) return "sfx_mezzo_1";
        if (noteType == 6 || (!string.IsNullOrEmpty(uid) && uid.StartsWith("0002"))) return "sfx_hp";
        if (noteType == 7 || (!string.IsNullOrEmpty(uid) && uid.StartsWith("0003"))) return "sfx_score";
        return null; // keep cloned value
    }

    public static MusicData CloneMusicData(MusicData originalNote)
    {
        var newNote = new MusicData();
        newNote.noteData = CloneNoteConfigData(originalNote.noteData);
        newNote.configData = CloneMusicConfigData(originalNote.configData);
        newNote.objId = originalNote.objId;
        newNote.tick = originalNote.tick;
        newNote.dt = originalNote.dt;
        newNote.showTick = originalNote.showTick;
        newNote.doubleIdx = originalNote.doubleIdx;
        newNote.sameTickNoteIdx = originalNote.sameTickNoteIdx;
        newNote.isDouble = originalNote.isDouble;
        newNote.isLongPressing = originalNote.isLongPressing;
        newNote.isLongPressEnd = originalNote.isLongPressEnd;
        newNote.longPressPTick = originalNote.longPressPTick;
        newNote.endIndex = originalNote.endIndex;
        newNote.longPressNum = originalNote.longPressNum;
        newNote.m_BattleScore = originalNote.m_BattleScore;
        return newNote;
    }

    public static NoteConfigData CloneNoteConfigData(NoteConfigData originalNoteData)
    {
        if (originalNoteData == null) return null;

        var clone = new NoteConfigData
        {
            id = originalNoteData.id,
            ibms_id = originalNoteData.ibms_id,
            uid = originalNoteData.uid,
            mirror_uid = originalNoteData.mirror_uid,
            scene = originalNoteData.scene,
            des = originalNoteData.des,
            prefab_name = originalNoteData.prefab_name,
            type = originalNoteData.type,
            effect = originalNoteData.effect,
            key_audio = originalNoteData.key_audio,
            boss_action = originalNoteData.boss_action,
            sceneChangeNames = originalNoteData.sceneChangeNames,
            left_perfect_range = originalNoteData.left_perfect_range,
            left_great_range = originalNoteData.left_great_range,
            right_perfect_range = originalNoteData.right_perfect_range,
            right_great_range = originalNoteData.right_great_range,
            damage = originalNoteData.damage,
            pathway = originalNoteData.pathway,
            speed = originalNoteData.speed,
            score = originalNoteData.score,
            fever = originalNoteData.fever,
            missCombo = originalNoteData.missCombo,
            addCombo = originalNoteData.addCombo,
            jumpNote = originalNoteData.jumpNote,
            isShowPlayEffect = originalNoteData.isShowPlayEffect,
            m_BmsUid = originalNoteData.m_BmsUid,
            noteUid = originalNoteData.noteUid
        };
        return clone;
    }

    public static MusicConfigData CloneMusicConfigData(MusicConfigData originalConfigData)
    {
        if (originalConfigData == null) return null;

        return new MusicConfigData
        {
            id = originalConfigData.id,
            time = originalConfigData.time,
            note_uid = originalConfigData.note_uid,
            length = originalConfigData.length,
            blood = originalConfigData.blood,
            pathway = originalConfigData.pathway
        };
    }

    public static bool IsSceneToggleNote(MusicData note)
    {
        if (note?.noteData == null) return false;

        string uid = note.noteData.uid ?? "";
        return note.noteData.type == 9 || uid.StartsWith("0004");
    }

    public static double NormalizeTimingValue(double value)
    {
        return System.Math.Round(value, 3, System.MidpointRounding.AwayFromZero);
    }

    public static double NormalizeChartValue(double value)
    {
        return System.Math.Round(value, 2, System.MidpointRounding.AwayFromZero);
    }

    public static double NormalizeShowTickValue(double value)
    {
        return System.Math.Round(value, 2, System.MidpointRounding.AwayFromZero);
    }

    public static double GetEffectiveDt(MusicData note)
    {
        return GetEffectiveDt(note, null);
    }

    public static double GetEffectiveDt(MusicData note, ExperimentNoteSpec spec)
    {
        if (spec != null && spec.Dt >= 0.0) return spec.Dt;

        string uid = !string.IsNullOrEmpty(spec?.Uid) ? spec.Uid : note.noteData?.uid;
        if (string.IsNullOrEmpty(uid) || uid.Length < 6) return BaseDt;

        if (IsBossProjectileUid(uid)) return BossProjectileDt;

        if (spec != null && !string.IsNullOrEmpty(spec.BossAction))
        {
            return 0.0;
        }

        string xx = uid.Substring(2, 2);
        string yy = uid.Substring(4, 2);
        if (xx == "15") return 0.8;
        if (xx == "16") return 1.25;
        if (yy == "07" || yy == "10" || yy == "13" || yy == "16") return 1.0;

        return BaseDt;
    }
}
