using MelonLoader;
using Il2CppAssets.Scripts.Database;
using Il2CppGameLogic;
using System.Reflection;

public partial class DBStageInfo_SetRuntimeMusicData_Patch
{
    // true: BMS 파일이 있으면 BMS 차트를 주입, false: ExperimentNotes 배열만 사용
    private static readonly bool UseBmsInjection = true;

    public static void DumpMusicList(DBStageInfo __instance)
    {
        if (__instance._musicList_k__BackingField != null)
        {
            int totalCount = __instance._musicList_k__BackingField.Count;
            int count0908 = 0;
            int count0902 = 0;
            int count0911 = 0;
            int other09yyCount = 0;
            int road09yyCount = 0;
            int air09yyCount = 0;
            var seenOther09yy = new System.Collections.Generic.HashSet<string>();
            var seenXxyy = new System.Collections.Generic.HashSet<string>();
            MelonLogger.Msg($"[ExperimentChart.Debug] DumpMusicList 시작: total={totalCount}, filter=xxyy in [0908, 0902, 0911], unique only");

            for (int i = 0; i < totalCount; i++)
            {
                var musicData = __instance._musicList_k__BackingField[i];
                string uid = musicData.noteData?.uid;
                string xxyy = GetUidXxyy(uid);
                string pathwayLabel = GetPathwayLabel(musicData.noteData?.pathway ?? 0);
                string noteType = musicData.noteData != null ? musicData.noteData.type.ToString() : "(null)";
                string scene = musicData.noteData?.scene ?? "(null)";
                string prefab = musicData.noteData?.prefab_name ?? "(null)";
                string bossAction = musicData.noteData?.boss_action ?? "(null)";
                string configPathway = musicData.configData != null ? musicData.configData.pathway.ToString() : "(null)";
                if (!IsTargetDebugXxyy(uid) || !seenXxyy.Add(xxyy))
                {
                    if (!string.IsNullOrEmpty(xxyy) && xxyy.StartsWith("09"))
                    {
                        other09yyCount++;
                        if (string.Equals(pathwayLabel, "공중", System.StringComparison.Ordinal))
                        {
                            air09yyCount++;
                        }
                        else
                        {
                            road09yyCount++;
                        }
                        if (seenOther09yy.Add(xxyy))
                        {
                            MelonLogger.Msg($"[ExperimentChart.Debug] 발견된 추가 09yy: xxyy={xxyy}, idx={i}, uid={uid}, pathway={musicData.noteData?.pathway ?? -1}({pathwayLabel}), type={noteType}, scene={scene}, prefab={prefab}, config_pathway={configPathway}, boss_action={bossAction}, tick={musicData.tick}, dt={musicData.dt}, showTick={musicData.showTick}");
                        }
                    }

                    continue;
                }

                if (xxyy == "0908") count0908++;
                else if (xxyy == "0902") count0902++;
                else if (xxyy == "0911") count0911++;

                MelonLogger.Msg($"[ExperimentChart.Debug] xxyy={xxyy}: idx={i}, uid={uid}, pathway={musicData.noteData?.pathway ?? -1}({pathwayLabel}), type={noteType}, scene={scene}, prefab={prefab}, config_pathway={configPathway}, boss_action={bossAction}, tick={musicData.tick}, dt={musicData.dt}, showTick={musicData.showTick}");
                LogNoteState($"[ExperimentChart.Debug] note[{i}]", musicData);
            }

            MelonLogger.Msg($"[ExperimentChart.Debug] xxyy summary: 0908={count0908}, 0902={count0902}, 0911={count0911}, other09yy={other09yyCount}, road09yy={road09yyCount}, air09yy={air09yyCount}");
        }
    }

    public static void ApplyExperimentChart(DBStageInfo __instance, string activeUid)
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
            LogOriginalUidMatches(sourceNotes, "05", "05");
        }

        musicList.Clear();
        musicList.Add(anchor);

        var runtimeSpecs = BuildRuntimeExperimentNotes(ExperimentNotes);
        if (UseBmsInjection && muse_dash_test.MainMod.TryGetCachedHwaBmsChart(activeUid, out var bmsChart, out string bmsDescription))
        {
            var bmsSpecs = BuildBmsExperimentNotes(bmsChart, activeUid);
            if (bmsSpecs.Count > 0)
            {
                runtimeSpecs = BuildRuntimeExperimentNotes(bmsSpecs);
                MelonLogger.Msg($"[ExperimentChart.Bms] BMS 차트 주입 사용: specs={runtimeSpecs.Count}, {bmsDescription}");
            }
            else
            {
                MelonLogger.Warning($"[ExperimentChart.Bms] BMS 차트를 찾았지만 변환된 노트가 없어 기존 ExperimentNotes를 사용합니다: {bmsDescription}");
            }
        }

        foreach (var spec in runtimeSpecs)
        {
            AddExperimentNotes(musicList, sourceNote, spec);
        }

        if (UseBmsInjection && muse_dash_test.MainMod.TryGetCachedHwaBmsChart(activeUid, out _, out _) && musicList.Count > 1)
        {
            ApplyBmsDoubleState(musicList, 1);
        }

        MelonLogger.Msg($"실험 차트 적용 완료: {musicList.Count}개 노트 ([0] 원본 유지, 원본 index {SourceNoteIndex} 복사 후 지정 노트로 변형)");
    }

    public static void ApplyBmsDoubleState(Il2CppSystem.Collections.Generic.List<MusicData> musicList, int startIndex)
    {
        if (musicList == null || musicList.Count <= startIndex)
        {
            return;
        }

        var groupsByTick = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<int>>(System.StringComparer.Ordinal);

        for (int i = startIndex; i < musicList.Count; i++)
        {
            var note = musicList[i];
            if (note.noteData == null)
            {
                continue;
            }

            string tickKey = note.tick.ToString();
            if (!groupsByTick.TryGetValue(tickKey, out var indices))
            {
                indices = new System.Collections.Generic.List<int>();
                groupsByTick[tickKey] = indices;
            }

            indices.Add(i);
        }

        int doubleGroupCount = 0;
        foreach (var group in groupsByTick.Values)
        {
            if (group == null || group.Count < 2)
            {
                continue;
            }

            group.Sort();
            doubleGroupCount++;

            for (int i = 0; i < group.Count; i++)
            {
                int currentIndex = group[i];
                int partnerIndex = group[(i + 1) % group.Count];
                if (currentIndex == partnerIndex)
                {
                    continue;
                }

                var currentNote = musicList[currentIndex];
                var partnerNote = musicList[partnerIndex];

                currentNote.isDouble = true;
                currentNote.doubleIdx = partnerNote.objId;

                musicList[currentIndex] = currentNote;
            }
        }

        MelonLogger.Msg($"[ExperimentChart.Bms] 더블 상태 적용 완료: groups={doubleGroupCount}, notes={musicList.Count - startIndex}");
    }

    public static void LogOriginalUidMatches(MusicData[] sourceNotes, string targetScene, string targetXx)
    {
        if (sourceNotes == null || string.IsNullOrWhiteSpace(targetScene) || string.IsNullOrWhiteSpace(targetXx))
        {
            return;
        }

        int count = 0;
        for (int i = 0; i < sourceNotes.Length; i++)
        {
            try
            {
                var note = sourceNotes[i];
                if (note == null)
                {
                    continue;
                }

                var noteData = note.noteData;
                if (noteData == null)
                {
                    continue;
                }

                string uid = noteData.uid;
                if (string.IsNullOrEmpty(uid) || uid.Length < 4)
                {
                    continue;
                }

                string scene = uid.Substring(0, 2);
                string xxyy = uid.Substring(2, 2);
                if (!string.Equals(scene, targetScene, System.StringComparison.OrdinalIgnoreCase) || !string.Equals(xxyy, targetXx, System.StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                count++;
                string type = SafeLogValue(() => noteData.type);
                string pathway = SafeLogValue(() => noteData.pathway);
                string pathwayLabel = SafeLogValue(() => GetPathwayLabel(noteData.pathway));
                string sceneName = SafeLogValue(() => noteData.scene);
                string prefab = SafeLogValue(() => noteData.prefab_name);
                string keyAudio = SafeLogValue(() => noteData.key_audio);
                string bossAction = SafeLogValue(() => noteData.boss_action);
                string configTime = SafeLogValue(() => note.configData?.time);
                string configPathway = SafeLogValue(() => note.configData?.pathway);
                string doubleIdx = SafeLogValue(() => note.doubleIdx);
                string sameTickNoteIdx = SafeLogValue(() => note.sameTickNoteIdx);
                string isDouble = SafeLogValue(() => note.isDouble);
                string jumpNote = SafeLogValue(() => noteData.jumpNote);
                string score = SafeLogValue(() => noteData.score);
                string objId = SafeLogValue(() => note.objId);
                string tick = SafeLogValue(() => note.tick);
                string dt = SafeLogValue(() => note.dt);
                string showTick = SafeLogValue(() => note.showTick);

                MelonLogger.Msg($"[ExperimentDebug.Search] 원본 UID 발견: uid={uid}, idx={i}, objId={objId}, tick={tick}, dt={dt}, showTick={showTick}, type={type}, pathway={pathway}({pathwayLabel}), scene={sceneName}, prefab={prefab}, keyAudio={keyAudio}, bossAction={bossAction}, doubleIdx={doubleIdx}, sameTickNoteIdx={sameTickNoteIdx}, isDouble={isDouble}, jumpNote={jumpNote}, score={score}, config.time={configTime}, config.pathway={configPathway}");
            }
            catch (System.Exception ex)
            {
                MelonLogger.Warning($"[ExperimentDebug.Search] 원본 UID 검사 중 예외 발생: idx={i}, error={ex.Message}");
            }
        }

        MelonLogger.Msg($"[ExperimentDebug.Search] 원본 UID 검색 완료: scene={targetScene}, xx={targetXx}, count={count}");
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
        MoveNote(ref note, musicList.Count, tick, 0.0, spec);
        if (DebugExperimentNotes)
        {
            LogNoteState($"[ExperimentDebug] {label} after MoveNote", note);
        }
        ApplyNoteSpec(ref note, spec);
        if (DebugExperimentNotes)
        {
            LogNoteState($"[ExperimentDebug] {label} after ApplyNoteSpec", note);
        }
        ResetRuntimeFlags(ref note);
        musicList.Add(note);
        LogInsertedNote(label, note);
    }

    public static void AddMulNote(Il2CppSystem.Collections.Generic.List<MusicData> musicList, MusicData template, double tick, double length, string label, ExperimentNoteSpec spec)
    {
        var note = CloneMusicData(template);
        MoveNote(ref note, musicList.Count, tick, length, spec);
        if (DebugExperimentNotes)
        {
            LogNoteState($"[ExperimentDebug] {label} after MoveNote", note);
        }
        ApplyNoteSpec(ref note, spec);
        if (DebugExperimentNotes)
        {
            LogNoteState($"[ExperimentDebug] {label} after ApplyNoteSpec", note);
        }
        ResetRuntimeFlags(ref note);
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
        MoveNote(ref start, startIndex, startTick, length, spec);
        if (DebugExperimentNotes)
        {
            LogNoteState($"[ExperimentDebug] {label} start after MoveNote", start);
        }
        ApplyNoteSpec(ref start, spec);
        if (DebugExperimentNotes)
        {
            LogNoteState($"[ExperimentDebug] {label} start after ApplyNoteSpec", start);
        }
        ResetRuntimeFlags(ref start);
        start.longPressPTick = (Il2CppSystem.Decimal)startTick;
        start.endIndex = endIndex;
        start.longPressNum = longPressCount;
        musicList.Add(start);
        LogInsertedNote(label + " start", start);

        for (int i = 1; i <= middleCount; i++)
        {
            double middleTick = startTick + (LongMiddleStep * i);
            var middle = CloneMusicData(template);
            MoveNote(ref middle, musicList.Count, middleTick, 0.0, spec);
            ApplyNoteSpec(ref middle, spec);
            ResetRuntimeFlags(ref middle);
            middle.isLongPressing = true;
            middle.longPressPTick = (Il2CppSystem.Decimal)startTick;
            middle.endIndex = endIndex;
            middle.longPressNum = longPressCount;
            musicList.Add(middle);
        }

        var end = CloneMusicData(template);
        MoveNote(ref end, musicList.Count, startTick + length, 0.0, spec);
        if (DebugExperimentNotes)
        {
            LogNoteState($"[ExperimentDebug] {label} end after MoveNote", end);
        }
        ApplyNoteSpec(ref end, spec);
        if (DebugExperimentNotes)
        {
            LogNoteState($"[ExperimentDebug] {label} end after ApplyNoteSpec", end);
        }
        ResetRuntimeFlags(ref end);
        end.isLongPressEnd = true;
        end.longPressPTick = (Il2CppSystem.Decimal)startTick;
        end.endIndex = endIndex;
        end.longPressNum = longPressCount;
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
            if (!string.IsNullOrEmpty(spec.BossAction) && uid.Length >= 2)
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
