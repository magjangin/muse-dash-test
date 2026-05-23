using MelonLoader;
using Il2CppAssets.Scripts.Database;
using Il2CppGameLogic;
using System.Reflection;

public partial class DBStageInfo_SetRuntimeMusicData_Patch
{
    public static void DumpMusicList(DBStageInfo __instance)
    {
        if (__instance._musicList_k__BackingField != null)
        {
            int count = System.Math.Min(__instance._musicList_k__BackingField.Count, 5);
            for (int i = 0; i < count; i++)
            {
                var musicData = __instance._musicList_k__BackingField[i];
                MelonLogger.Msg($"MusicData {i}: {musicData}");
                foreach (var prop in musicData.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    try
                    {
                        var value = prop.GetValue(musicData);
                        MelonLogger.Msg($"  {prop.Name}: {value}");
                        if (prop.Name == "noteData" && value != null)
                        {
                            MelonLogger.Msg($"    noteData:");
                            foreach (var noteProp in value.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
                            {
                                try
                                {
                                    var noteValue = noteProp.GetValue(value);
                                    MelonLogger.Msg($"      {noteProp.Name}: {noteValue}");
                                }
                                catch (System.Exception ex)
                                {
                                    MelonLogger.Msg($"      {noteProp.Name}: (예외 발생: {ex.Message})");
                                }
                            }
                        }
                        else if (prop.Name == "configData" && value != null)
                        {
                            MelonLogger.Msg($"    configData:");
                            foreach (var configProp in value.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
                            {
                                try
                                {
                                    var configValue = configProp.GetValue(value);
                                    MelonLogger.Msg($"      {configProp.Name}: {configValue}");
                                }
                                catch (System.Exception ex)
                                {
                                    MelonLogger.Msg($"      {configProp.Name}: (예외 발생: {ex.Message})");
                                }
                            }
                        }
                    }
                    catch (System.Exception ex)
                    {
                        MelonLogger.Msg($"  {prop.Name}: (예외 발생: {ex.Message})");
                    }
                }
            }
        }
    }

    public static void ApplyExperimentChart(DBStageInfo __instance)
    {
        var musicList = __instance._musicList_k__BackingField;
        if (musicList == null || musicList.Count <= SourceNoteIndex)
        {
            MelonLogger.Warning("실험 차트 적용 실패: 원본 노트가 부족합니다.");
            return;
        }

        var sourceNotes = new MusicData[musicList.Count];
        for (int i = 0; i < musicList.Count; i++)
        {
            sourceNotes[i] = CloneMusicData(musicList[i]);
        }



        var anchor = CloneMusicData(sourceNotes[0]);
        var sourceNote = CloneMusicData(sourceNotes[SourceNoteIndex]);

        if (DebugExperimentNotes)
        {
            MelonLogger.Msg($"[ExperimentDebug] source count={sourceNotes.Length}, SourceNoteIndex={SourceNoteIndex}");
            LogNoteState("[ExperimentDebug] anchor original [0]", anchor);
            LogNoteState($"[ExperimentDebug] source original [{SourceNoteIndex}]", sourceNote);
        }

        musicList.Clear();
        musicList.Add(anchor);

        foreach (var spec in ExperimentNotes)
        {
            AddExperimentNotes(musicList, sourceNote, spec);
        }

        MelonLogger.Msg($"실험 차트 적용 완료: {musicList.Count}개 노트 ([0] 원본 유지, 원본 index {SourceNoteIndex} 복사 후 지정 노트로 변형)");
    }

    public static void AddExperimentNotes(Il2CppSystem.Collections.Generic.List<MusicData> outputList, MusicData sourceNote, ExperimentNoteSpec spec)
    {
        int count = System.Math.Max(1, spec.Count);
        var template = CloneMusicData(sourceNote);

        if (DebugExperimentNotes)
        {
            LogSpec("[ExperimentDebug] spec", spec);
            LogNoteState("[ExperimentDebug] template before spec", template);
        }

        ApplyNoteSpec(ref template, spec);

        if (DebugExperimentNotes)
        {
            LogNoteState("[ExperimentDebug] template after spec", template);
        }

        for (int i = 0; i < count; i++)
        {
            double tick = spec.StartTick + (spec.Interval * i);
            string label = $"{spec.Label} #{i + 1}/{count}";

            if (spec.IsLong)
            {
                AddLongNote(outputList, template, tick, spec.Length > 0.0 ? spec.Length : 1.0, label, spec);
            }
            else if (spec.IsMul)
            {
                AddMulNote(outputList, template, tick, spec.Length > 0.0 ? spec.Length : 1.0, label, spec);
            }
            else
            {
                AddMovedNote(outputList, template, tick, label, spec);
            }
        }
    }

    public static void AddMovedNote(Il2CppSystem.Collections.Generic.List<MusicData> musicList, MusicData template, double tick, string label, ExperimentNoteSpec spec)
    {
        var note = CloneMusicData(template);
        if (DebugExperimentNotes)
        {
            LogNoteState($"[ExperimentDebug] {label} before ApplyNoteSpec", note);
        }
        ApplyNoteSpec(ref note, spec);
        if (DebugExperimentNotes)
        {
            LogNoteState($"[ExperimentDebug] {label} after ApplyNoteSpec", note);
        }
        ResetRuntimeFlags(ref note);
        MoveNote(ref note, musicList.Count, tick, 0.0, spec);
        if (DebugExperimentNotes)
        {
            LogNoteState($"[ExperimentDebug] {label} after MoveNote", note);
        }
        musicList.Add(note);
        LogInsertedNote(label, note);
    }

    public static void AddMulNote(Il2CppSystem.Collections.Generic.List<MusicData> musicList, MusicData template, double tick, double length, string label, ExperimentNoteSpec spec)
    {
        var note = CloneMusicData(template);
        if (DebugExperimentNotes)
        {
            LogNoteState($"[ExperimentDebug] {label} before ApplyNoteSpec", note);
        }
        ApplyNoteSpec(ref note, spec);
        if (DebugExperimentNotes)
        {
            LogNoteState($"[ExperimentDebug] {label} after ApplyNoteSpec", note);
        }
        ResetRuntimeFlags(ref note);
        if (note.configData != null)
        {
            note.configData.length = (Il2CppSystem.Decimal)length;
        }
        MoveNote(ref note, musicList.Count, tick, length, spec);
        if (DebugExperimentNotes)
        {
            LogNoteState($"[ExperimentDebug] {label} after MoveNote", note);
        }
        musicList.Add(note);
        LogInsertedNote(label, note);
    }

    public static void AddLongNote(Il2CppSystem.Collections.Generic.List<MusicData> musicList, MusicData template, double startTick, double length, string label, ExperimentNoteSpec spec)
    {
        int startIndex = musicList.Count;
        int longPressCount = (int)System.Math.Ceiling(length / LongMiddleStep);
        int middleCount = System.Math.Max(0, longPressCount - 1);
        int endIndex = startIndex + middleCount + 1;

        var start = CloneMusicData(template);
        if (DebugExperimentNotes)
        {
            LogNoteState($"[ExperimentDebug] {label} start before ApplyNoteSpec", start);
        }
        ApplyNoteSpec(ref start, spec);
        if (DebugExperimentNotes)
        {
            LogNoteState($"[ExperimentDebug] {label} start after ApplyNoteSpec", start);
        }
        ResetRuntimeFlags(ref start);
        MoveNote(ref start, startIndex, startTick, length, spec);
        start.longPressPTick = (Il2CppSystem.Decimal)startTick;
        start.endIndex = endIndex;
        start.longPressNum = longPressCount;
        if (DebugExperimentNotes)
        {
            LogNoteState($"[ExperimentDebug] {label} start after MoveNote", start);
        }
        musicList.Add(start);
        LogInsertedNote(label + " start", start);

        for (int i = 1; i <= middleCount; i++)
        {
            double middleTick = startTick + (LongMiddleStep * i);
            var middle = CloneMusicData(template);
            ApplyNoteSpec(ref middle, spec);
            ResetRuntimeFlags(ref middle);
            MoveNote(ref middle, musicList.Count, middleTick, 0.0, spec);
            middle.isLongPressing = true;
            middle.longPressPTick = (Il2CppSystem.Decimal)startTick;
            middle.endIndex = endIndex;
            middle.longPressNum = longPressCount;
            musicList.Add(middle);
        }

        var end = CloneMusicData(template);
        if (DebugExperimentNotes)
        {
            LogNoteState($"[ExperimentDebug] {label} end before ApplyNoteSpec", end);
        }
        ApplyNoteSpec(ref end, spec);
        if (DebugExperimentNotes)
        {
            LogNoteState($"[ExperimentDebug] {label} end after ApplyNoteSpec", end);
        }
        ResetRuntimeFlags(ref end);
        MoveNote(ref end, musicList.Count, startTick + length, 0.0, spec);
        end.isLongPressEnd = true;
        end.longPressPTick = (Il2CppSystem.Decimal)startTick;
        end.endIndex = endIndex;
        end.longPressNum = longPressCount;
        if (DebugExperimentNotes)
        {
            LogNoteState($"[ExperimentDebug] {label} end after MoveNote", end);
        }
        musicList.Add(end);
        LogInsertedNote(label + " end", end);
    }

    public static void ApplyNoteSpec(ref MusicData note, ExperimentNoteSpec spec)
    {
        if (DebugExperimentNotes)
        {
            MelonLogger.Msg($"[ExperimentDebug] ApplyNoteSpec begin label={spec.Label}");
            LogNoteState("[ExperimentDebug] ApplyNoteSpec input", note);
        }

        var noteData = CloneNoteConfigData(note.noteData) ?? new NoteConfigData();
        var configData = CloneMusicConfigData(note.configData);

        string uid = !string.IsNullOrEmpty(spec.Uid) ? spec.Uid : noteData.uid;
        int noteType = spec.NoteType >= 0 ? spec.NoteType : (int)noteData.type;
        int pathway = spec.Pathway >= 0 ? spec.Pathway : noteData.pathway;

        if (spec.IsLong) noteType = 3;
        if (spec.IsMul) noteType = 8;

        noteData.uid = uid;
        // mirror_uid를 uid와 동일하게 설정
        noteData.mirror_uid = uid;
        if (!string.IsNullOrEmpty(spec.IbmsId))
        {
            noteData.ibms_id = spec.IbmsId;
        }
        noteData.type = (uint)noteType;
        noteData.pathway = pathway;

        if (!string.IsNullOrEmpty(uid))
        {
            if (int.TryParse(uid, out int parsedNoteUid))
            {
                noteData.noteUid = parsedNoteUid;
            }
            if (uid.Length >= 2)
            {
                noteData.scene = "scene_" + uid.Substring(0, 2);
            }
        }

        if (!string.IsNullOrEmpty(spec.Scene))
        {
            noteData.scene = spec.Scene;
        }

        if (!string.IsNullOrEmpty(spec.PrefabName))
        {
            noteData.prefab_name = spec.PrefabName;
        }
        else if (ShouldUseEmptyBossActionPrefab(noteType, uid, spec.BossAction))
        {
            noteData.prefab_name = "empty_000";
        }
        else if (!string.IsNullOrEmpty(uid))
        {
            noteData.prefab_name = BuildPrefabName(uid, noteType, pathway);
        }

        if (!string.IsNullOrEmpty(spec.KeyAudio))
        {
            noteData.key_audio = spec.KeyAudio;
        }
        else if (!string.IsNullOrEmpty(spec.BossAction))
        {
            noteData.key_audio = "";
        }
        else
        {
            ApplyDefaultKeyAudio(noteData, noteType, uid);
        }

        if (!string.IsNullOrEmpty(spec.BossAction))
        {
            noteData.boss_action = spec.BossAction;
        }

        if (spec.Speed >= 0.0)
        {
            noteData.speed = (int)System.Math.Round(spec.Speed);
        }

        // des is intentionally not overridden by spec

        if (configData != null && !string.IsNullOrEmpty(uid))
        {
            configData.note_uid = uid;
            configData.pathway = pathway;
        }

        note.noteData = noteData;
        note.configData = configData;

        if (DebugExperimentNotes)
        {
            MelonLogger.Msg($"[ExperimentDebug] ApplyNoteSpec resolved uid={uid}, noteType={noteType}, pathway={pathway}, scene={noteData.scene}, prefab={noteData.prefab_name}, keyAudio={noteData.key_audio}");
            LogNoteState("[ExperimentDebug] ApplyNoteSpec output", note);
        }
    }

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
