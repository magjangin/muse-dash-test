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
        var musicList = __instance._musicList_k__BackingField;
        if (musicList == null)
        {
            return;
        }

        int bossEventCount = 0;
        MelonLogger.Msg($"[OfficialBossContext] 원본 차트 보스 이벤트 주변 덤프 시작: total={musicList.Count}, neighbors=2");

        for (int i = 0; i < musicList.Count; i++)
        {
            var note = musicList[i];
            string bossAction = note.noteData?.boss_action ?? string.Empty;
            if (note.noteData?.type != 0
                || string.IsNullOrWhiteSpace(bossAction)
                || string.Equals(bossAction, "0", System.StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            bossEventCount++;
            MelonLogger.Msg($"[OfficialBossContext] === event#{bossEventCount}, index={i}, action={bossAction} ===");

            int firstIndex = System.Math.Max(0, i - 2);
            int lastIndex = System.Math.Min(musicList.Count - 1, i + 2);
            for (int contextIndex = firstIndex; contextIndex <= lastIndex; contextIndex++)
            {
                LogOfficialBossContextNote(contextIndex, i, musicList[contextIndex]);
            }
        }

        MelonLogger.Msg($"[OfficialBossContext] 원본 차트 보스 이벤트 주변 덤프 완료: events={bossEventCount}");
    }

    public static void LogOfficialBossContextNote(int index, int eventIndex, MusicData note)
    {
        if (note == null)
        {
            MelonLogger.Msg($"[OfficialBossContext] {(index == eventIndex ? "EVENT" : "NEIGHBOR")} index={index}, note=(null)");
            return;
        }

        string role = index == eventIndex ? "EVENT" : index < eventIndex ? "PREV" : "NEXT";
        MelonLogger.Msg(
            $"[OfficialBossContext] {role} index={index}, objId={SafeLogValue(() => note.objId)}, " +
            $"tick={SafeLogValue(() => note.tick)}, dt={SafeLogValue(() => note.dt)}, showTick={SafeLogValue(() => note.showTick)}, " +
            $"uid={SafeLogValue(() => note.noteData?.uid)}, type={SafeLogValue(() => note.noteData?.type)}, " +
            $"pathway={SafeLogValue(() => note.noteData?.pathway)}, boss_action={SafeLogValue(() => note.noteData?.boss_action)}, " +
            $"prefab={SafeLogValue(() => note.noteData?.prefab_name)}, key_audio={SafeLogValue(() => note.noteData?.key_audio)}, " +
            $"isDouble={SafeLogValue(() => note.isDouble)}, doubleIdx={SafeLogValue(() => note.doubleIdx)}, sameTickNoteIdx={SafeLogValue(() => note.sameTickNoteIdx)}, " +
            $"isLongPressing={SafeLogValue(() => note.isLongPressing)}, isLongPressEnd={SafeLogValue(() => note.isLongPressEnd)}, endIndex={SafeLogValue(() => note.endIndex)}, " +
            $"config.id={SafeLogValue(() => note.configData?.id)}, config.time={SafeLogValue(() => note.configData?.time)}, " +
            $"config.length={SafeLogValue(() => note.configData?.length)}, config.pathway={SafeLogValue(() => note.configData?.pathway)}");
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
            SortBmsRuntimeMusicListByShowTick(musicList, 1);
        }

        MelonLogger.Msg($"실험 차트 적용 완료: {musicList.Count}개 노트 ([0] 원본 유지, 원본 index {SourceNoteIndex} 복사 후 지정 노트로 변형)");
    }

    public static void SortBmsRuntimeMusicListByShowTick(Il2CppSystem.Collections.Generic.List<MusicData> musicList, int startIndex)
    {
        if (musicList == null || musicList.Count <= startIndex)
        {
            return;
        }

        var runtimeNotes = new System.Collections.Generic.List<MusicData>();
        var oldEndIndices = new System.Collections.Generic.Dictionary<short, int>();
        var oldDoubleIndices = new System.Collections.Generic.Dictionary<short, int>();
        var oldDoubleStates = new System.Collections.Generic.Dictionary<short, bool>();

        for (int i = startIndex; i < musicList.Count; i++)
        {
            var note = musicList[i];
            runtimeNotes.Add(note);
            oldEndIndices[note.objId] = note.endIndex;
            oldDoubleIndices[note.objId] = note.doubleIdx;
            oldDoubleStates[note.objId] = note.isDouble;
        }

        runtimeNotes.Sort((left, right) =>
        {
            int showTickCompare = ParseMusicDecimal(left.showTick).CompareTo(ParseMusicDecimal(right.showTick));
            if (showTickCompare != 0) return showTickCompare;

            int tickCompare = ParseMusicDecimal(left.tick).CompareTo(ParseMusicDecimal(right.tick));
            if (tickCompare != 0) return tickCompare;

            return left.objId.CompareTo(right.objId);
        });

        var newIndexByOldObjId = new System.Collections.Generic.Dictionary<short, int>();
        for (int i = 0; i < runtimeNotes.Count; i++)
        {
            newIndexByOldObjId[runtimeNotes[i].objId] = startIndex + i;
        }

        while (musicList.Count > startIndex)
        {
            musicList.RemoveAt(musicList.Count - 1);
        }

        for (int i = 0; i < runtimeNotes.Count; i++)
        {
            var note = runtimeNotes[i];
            short oldObjId = note.objId;
            int newIndex = startIndex + i;

            note.objId = (short)newIndex;
            note.isDouble = oldDoubleStates.TryGetValue(oldObjId, out bool wasDouble) && wasDouble;
            note.doubleIdx = note.noteData?.type == 0 ? -1 : 0;

            if (note.isDouble
                && oldDoubleIndices.TryGetValue(oldObjId, out int oldDoubleIndex)
                && newIndexByOldObjId.TryGetValue((short)oldDoubleIndex, out int newDoubleIndex))
            {
                note.doubleIdx = newDoubleIndex;
            }

            if (IsSceneToggleNote(note))
            {
                note.doubleIdx = -1;
            }

            if (oldEndIndices.TryGetValue(oldObjId, out int oldEndIndex)
                && oldEndIndex > 0
                && newIndexByOldObjId.TryGetValue((short)oldEndIndex, out int newEndIndex))
            {
                note.endIndex = newEndIndex;
            }

            if (note.noteData != null)
            {
                note.noteData.id = newIndex.ToString(System.Globalization.CultureInfo.InvariantCulture);
            }

            if (note.configData != null)
            {
                note.configData.id = newIndex;
            }

            musicList.Add(note);
        }

        MelonLogger.Msg($"[ExperimentChart.Bms] 공식 방식 showTick 정렬 완료: notes={runtimeNotes.Count}, bossOffset={BossEventTickOffset}");
        DumpSortedBmsBossContext(musicList, startIndex);
    }

    public static void DumpSortedBmsBossContext(Il2CppSystem.Collections.Generic.List<MusicData> musicList, int startIndex)
    {
        for (int i = startIndex; i < musicList.Count; i++)
        {
            var note = musicList[i];
            if (note.noteData?.type != 0 || string.IsNullOrWhiteSpace(note.noteData.boss_action))
            {
                continue;
            }

            MelonLogger.Msg($"[BmsSortedBossContext] === index={i}, action={note.noteData.boss_action} ===");
            int firstIndex = System.Math.Max(startIndex, i - 2);
            int lastIndex = System.Math.Min(musicList.Count - 1, i + 2);
            for (int contextIndex = firstIndex; contextIndex <= lastIndex; contextIndex++)
            {
                var contextNote = musicList[contextIndex];
                string role = contextIndex == i ? "EVENT" : contextIndex < i ? "PREV" : "NEXT";
                MelonLogger.Msg(
                    $"[BmsSortedBossContext] {role} index={contextIndex}, objId={contextNote.objId}, " +
                    $"tick={contextNote.tick}, dt={contextNote.dt}, showTick={contextNote.showTick}, " +
                    $"config.time={SafeLogValue(() => contextNote.configData?.time)}, uid={SafeLogValue(() => contextNote.noteData?.uid)}, " +
                    $"type={SafeLogValue(() => contextNote.noteData?.type)}, boss_action={SafeLogValue(() => contextNote.noteData?.boss_action)}");
            }
        }
    }

    public static double ParseMusicDecimal(Il2CppSystem.Decimal value)
    {
        if (double.TryParse(value.ToString(), System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out double parsed))
        {
            return parsed;
        }

        return 0.0;
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
            if (note?.noteData == null)
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
            var roadIndices = new System.Collections.Generic.List<int>();
            var airIndices = new System.Collections.Generic.List<int>();

            for (int i = 0; i < group.Count; i++)
            {
                int noteIndex = group[i];
                var note = musicList[noteIndex];
                string exclusionReason = GetDoubleExclusionReason(note);

                if (exclusionReason != null)
                {
                    continue;
                }

                if (note.noteData.pathway == 1)
                {
                    airIndices.Add(noteIndex);
                }
                else
                {
                    roadIndices.Add(noteIndex);
                }
            }

            int pairCount = group.Count == 2 && roadIndices.Count == 1 && airIndices.Count == 1 ? 1 : 0;
            for (int i = 0; i < pairCount; i++)
            {
                int roadIndex = roadIndices[i];
                int airIndex = airIndices[i];
                var roadNote = musicList[roadIndex];
                var airNote = musicList[airIndex];

                double sharedDt = System.Math.Max(ParseMusicDecimal(roadNote.dt), ParseMusicDecimal(airNote.dt));
                double roadTick = ParseMusicDecimal(roadNote.tick);
                double airTick = ParseMusicDecimal(airNote.tick);

                roadNote.dt = (Il2CppSystem.Decimal)NormalizeTimingValue(sharedDt);
                roadNote.showTick = (Il2CppSystem.Decimal)NormalizeChartValue(roadTick - sharedDt);
                roadNote.isDouble = true;
                roadNote.doubleIdx = airNote.objId;

                airNote.dt = (Il2CppSystem.Decimal)NormalizeTimingValue(sharedDt);
                airNote.showTick = (Il2CppSystem.Decimal)NormalizeChartValue(airTick - sharedDt);
                airNote.isDouble = true;
                airNote.doubleIdx = roadNote.objId;

                musicList[roadIndex] = roadNote;
                musicList[airIndex] = airNote;
                doubleGroupCount++;
            }
        }

        MelonLogger.Msg($"[ExperimentChart.Bms] 더블 상태 적용 완료: pairs={doubleGroupCount}, notes={musicList.Count - startIndex}");
    }

    public static string GetDoubleExclusionReason(MusicData note)
    {
        if (note?.noteData == null) return "missing-note-data";
        if (note.noteData.type != 1) return $"type-{note.noteData.type}";
        if (note.isLongPressing) return "long-press-middle";
        if (note.isLongPressEnd) return "long-press-end";
        return null;
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
            else
            {
                double length = spec.IsMul ? (spec.Length > 0.0 ? spec.Length : 1.0) : 0.0;
                AddSingleNote(outputList, template, tick, length, label, spec);
            }
        }
    }

    private static void AddSingleNote(Il2CppSystem.Collections.Generic.List<MusicData> musicList, MusicData template, double tick, double length, string label, ExperimentNoteSpec spec)
    {
        var note = CloneMusicData(template);
        MoveNote(ref note, musicList.Count, tick, length, spec);
        if (DebugExperimentNotes) LogNoteState($"[ExperimentDebug] {label} after MoveNote", note);
        ApplyNoteSpec(ref note, spec);
        if (DebugExperimentNotes) LogNoteState($"[ExperimentDebug] {label} after ApplyNoteSpec", note);
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

        string resolvedPrefab = ResolvePrefabName(spec, noteType, uid, pathway);
        if (resolvedPrefab != null) noteData.prefab_name = resolvedPrefab;

        string resolvedKeyAudio = ResolveKeyAudio(spec, noteType, uid);
        if (resolvedKeyAudio != null) noteData.key_audio = resolvedKeyAudio;

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
