using MelonLoader;
using Il2CppAssets.Scripts.Database;
using Il2CppGameLogic;
using System.Reflection;

public partial class DBStageInfo_SetRuntimeMusicData_Patch
{
    public static string BuildPrefabName(string uid, int noteType, int pathway)
    {
        if (string.IsNullOrEmpty(uid) || uid.Length < 6)
            return uid;

        string xx = uid.Substring(2, 2);
        string yy = uid.Substring(4, 2);
        string lane = pathway == 1 ? "air" : "road";
        string motion = "nor";

        if (xx == "15") motion = "down";
        else if (xx == "16") motion = "up";
        else if (yy == "07" || yy == "10") motion = "up";
        else if (yy == "13" || yy == "16") motion = "down";

        return $"{uid}_{lane}_{motion}_1";
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

    public static void ApplyDefaultKeyAudio(NoteConfigData noteData, int noteType, string uid)
    {
        if (noteType == 6 || (!string.IsNullOrEmpty(uid) && uid.StartsWith("0002")))
            noteData.key_audio = "sfx_hp";
        else if (noteType == 7 || (!string.IsNullOrEmpty(uid) && uid.StartsWith("0003")))
            noteData.key_audio = "sfx_score";
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

    public static void ResetRuntimeFlags(ref MusicData note)
    {
        note.doubleIdx = 0;
        note.isDouble = false;
        note.isLongPressing = false;
        note.isLongPressEnd = false;
        note.longPressPTick = (Il2CppSystem.Decimal)0.0;
        note.endIndex = 0;
        note.longPressNum = 0;

        if (IsSceneToggleNote(note))
        {
            note.doubleIdx = -1;
        }
    }

    public static bool IsSceneToggleNote(MusicData note)
    {
        if (note?.noteData == null) return false;

        string uid = note.noteData.uid ?? "";
        return note.noteData.type == 9 || uid.StartsWith("0004");
    }

    public static void MoveNote(ref MusicData note, int objId, double tick, double length, ExperimentNoteSpec spec)
    {
        note.objId = (short)objId;
        note.tick = (Il2CppSystem.Decimal)tick;
        note.dt = (Il2CppSystem.Decimal)GetEffectiveDt(note, spec);
        note.showTick = note.tick - note.dt;

        if (note.configData != null)
        {
            var configData = CloneMusicConfigData(note.configData);
            configData.id = objId;
            configData.time = (Il2CppSystem.Decimal)tick;
            configData.length = (Il2CppSystem.Decimal)length;
            note.configData = configData;
        }
    }

    public static double GetEffectiveDt(MusicData note)
    {
        return GetEffectiveDt(note, null);
    }

    public static double GetEffectiveDt(MusicData note, ExperimentNoteSpec spec)
    {
        if (spec != null && spec.Dt >= 0.0) return spec.Dt;

        string uid = note.noteData?.uid;
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

    public static void LogInsertedNote(string label, MusicData note)
    {
        string uid = note.noteData?.uid ?? "(null)";
        string prefab = note.noteData?.prefab_name ?? "(null)";
        string type = note.noteData != null ? note.noteData.type.ToString() : "(null)";
        string pathway = note.noteData != null ? note.noteData.pathway.ToString() : "(null)";
        string speed = note.noteData != null ? note.noteData.speed.ToString() : "(null)";
        string scene = note.noteData?.scene ?? "(null)";
        string bossAction = note.noteData?.boss_action ?? "(null)";
        MelonLogger.Msg($"실험 노트 추가: {label}, objId={note.objId}, tick={note.tick}, dt={note.dt}, showTick={note.showTick}, speed={speed}, uid={uid}, type={type}, pathway={pathway}, scene={scene}, prefab={prefab}, boss_action={bossAction}");
    }

    public static void LogSpec(string label, ExperimentNoteSpec spec)
    {
        MelonLogger.Msg($"{label}: Label={spec.Label}, Uid={spec.Uid}, NoteType={spec.NoteType}, Pathway={spec.Pathway}, PrefabName={spec.PrefabName}, KeyAudio={spec.KeyAudio}, BossAction={spec.BossAction}, Scene={spec.Scene}, IbmsId={spec.IbmsId}, IsLong={spec.IsLong}, IsMul={spec.IsMul}, StartTick={spec.StartTick}, Count={spec.Count}, Interval={spec.Interval}, Length={spec.Length}, Speed={spec.Speed}, Dt={spec.Dt}");
    }

    public static void LogNoteState(string label, MusicData note)
    {
        string noteUid = SafeLogValue(() => note.noteData?.uid);
        string noteType = SafeLogValue(() => note.noteData?.type);
        string notePathway = SafeLogValue(() => note.noteData?.pathway);
        string noteScene = SafeLogValue(() => note.noteData?.scene);
        string prefab = SafeLogValue(() => note.noteData?.prefab_name);
        string keyAudio = SafeLogValue(() => note.noteData?.key_audio);
        string bossAction = SafeLogValue(() => note.noteData?.boss_action);
        string speed = SafeLogValue(() => note.noteData?.speed);
        string noteUidValue = SafeLogValue(() => note.noteData?.noteUid);
        string bmsUid = SafeLogValue(() => note.noteData?.m_BmsUid);

        string configId = SafeLogValue(() => note.configData?.id);
        string configTime = SafeLogValue(() => note.configData?.time);
        string configUid = SafeLogValue(() => note.configData?.note_uid);
        string configLength = SafeLogValue(() => note.configData?.length);
        string configPathway = SafeLogValue(() => note.configData?.pathway);

        MelonLogger.Msg($"{label}: objId={note.objId}, tick={note.tick}, dt={note.dt}, showTick={note.showTick}, note.uid={noteUid}, note.type={noteType}, note.pathway={notePathway}, note.scene={noteScene}, note.noteUid={noteUidValue}, note.m_BmsUid={bmsUid}, note.prefab={prefab}, note.speed={speed}, note.key_audio={keyAudio}, note.boss_action={bossAction}, config.id={configId}, config.time={configTime}, config.note_uid={configUid}, config.length={configLength}, config.pathway={configPathway}, isLongPressing={note.isLongPressing}, isLongPressEnd={note.isLongPressEnd}, longPressPTick={note.longPressPTick}, endIndex={note.endIndex}, longPressNum={note.longPressNum}");
    }

    public static string SafeLogValue(System.Func<object> getter)
    {
        try
        {
            object value = getter();
            return value != null ? value.ToString() : "(null)";
        }
        catch (System.Exception ex)
        {
            return $"(예외: {ex.GetType().Name})";
        }
    }
}