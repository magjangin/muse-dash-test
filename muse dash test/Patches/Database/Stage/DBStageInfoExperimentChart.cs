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
}
