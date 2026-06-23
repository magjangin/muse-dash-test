using MelonLoader;
using Il2CppAssets.Scripts.Database;
using Il2CppGameLogic;
using muse_dash_test;

// 실험 차트 핵심 파이프라인: 원본 노트 복제 → 스펙 적용 → 노트 삽입.
// (보조 로직은 같은 partial 클래스의 .Bms / .Resolve / .Sorting / .Diagnostics 파일로 분리되어 있습니다.)
public partial class DBStageInfo_SetRuntimeMusicData_Patch
{
    // true: BMS 파일이 있으면 BMS 차트를 주입, false: ExperimentNotes 배열만 사용
    private static readonly bool UseBmsInjection = true;

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
        var fallbackSourceNote = CloneMusicData(sourceNotes[SourceNoteIndex]);

        DebugMsg($"[ExperimentDebug] source count={sourceNotes.Length}, SourceNoteIndex={SourceNoteIndex}");
        DebugNote("[ExperimentDebug] anchor original [0]", anchor);
        DebugNote($"[ExperimentDebug] source original [{SourceNoteIndex}]", fallbackSourceNote);
        if (DebugExperimentNotes) LogOriginalUidMatches(sourceNotes, "05", "05");

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
            var sourceNote = PickSourceNote(sourceNotes, spec, fallbackSourceNote);
            AddExperimentNotes(musicList, sourceNote, spec);
        }

        if (UseBmsInjection && muse_dash_test.MainMod.TryGetCachedHwaBmsChart(activeUid, out _, out _) && musicList.Count > 1)
        {
            ApplyBmsDoubleState(musicList, 1);
            SortBmsRuntimeMusicListByShowTick(musicList, 1);
        }

        // 인게임 정확도 오버라이드를 위해 노트 타입별 개수를 집계해 세션에 저장합니다.
        RecountNoteTypes(musicList);

        MelonLogger.Msg($"실험 차트 적용 완료: {musicList.Count}개 노트 ([0] 원본 유지, 원본 index {SourceNoteIndex} 복사 후 지정 노트로 변형)");
    }

    /// <summary>
    /// musicList의 노트 타입별 개수(Standard/Gear/Heart/Blue)를 집계해 <see cref="CustomPlaySession"/>에 기록합니다.
    /// 보스/씬토글 노트는 정확도 분모에서 제외하며, 롱노트는 시작 노트 1개만 표준 노트로 셉니다.
    /// </summary>
    private static void RecountNoteTypes(Il2CppSystem.Collections.Generic.List<MusicData> musicList)
    {
        CustomPlaySession.Current.ResetCounts();
        int totalStandard = 0;
        int totalGears = 0;
        int totalHearts = 0;
        int totalBlueNotes = 0;

        for (int i = 1; i < musicList.Count; i++)
        {
            var note = musicList[i];
            if (note?.noteData == null) continue;

            int type = (int)note.noteData.type;

            if (type == NoteTypes.Boss || type == NoteTypes.SceneToggle || IsSceneToggleNote(note))
            {
                continue;
            }
            else if (type == NoteTypes.Gear)
            {
                totalGears++;
            }
            else if (type == NoteTypes.Heart)
            {
                totalHearts++;
            }
            else if (type == NoteTypes.Blue)
            {
                totalBlueNotes++;
            }
            else if (type == NoteTypes.Long)
            {
                // 롱노트는 시작 노트만 분모에 포함하고 중간/끝 마디는 제외합니다.
                if (!note.isLongPressing && !note.isLongPressEnd)
                {
                    totalStandard++;
                }
            }
            else
            {
                totalStandard++;
            }
        }

        CustomPlaySession.Current.TotalStandard = totalStandard;
        CustomPlaySession.Current.TotalGears = totalGears;
        CustomPlaySession.Current.TotalHearts = totalHearts;
        CustomPlaySession.Current.TotalBlueNotes = totalBlueNotes;

        MelonLogger.Msg($"[APMod.Accuracy] Custom chart note counts: Standard={totalStandard}, Gears={totalGears}, Hearts={totalHearts}, BlueNotes={totalBlueNotes}");
    }

    /// <summary>
    /// spec의 uid에서 씬 번호(앞 2자리)를 추출하고, 홀드노트이면 xx=10(예: zz1001), 일반 노트이면 xx=11(예: zz1101)에
    /// 해당하는 원본 노트를 sourceNotes에서 탐색합니다. 매칭되는 노트가 없으면 fallback을 반환합니다.
    /// </summary>
    private static MusicData PickSourceNote(MusicData[] sourceNotes, ExperimentNoteSpec spec, MusicData fallback)
    {
        if (string.IsNullOrEmpty(spec?.Uid) || spec.Uid.Length < 2)
        {
            return fallback;
        }

        string scenePrefix = UidCode.Scene(spec.Uid);
        string targetXx = spec.IsLong ? "10" : "11";

        for (int i = 0; i < sourceNotes.Length; i++)
        {
            string uid = sourceNotes[i]?.noteData?.uid;
            if (string.IsNullOrEmpty(uid) || uid.Length < 6) continue;
            if (UidCode.Scene(uid) == scenePrefix && UidCode.Xx(uid) == targetXx)
            {
                MelonLogger.Msg($"[ExperimentChart] sourceNote 선택: uid={uid} (spec={spec.Label}, isLong={spec.IsLong}, scene={scenePrefix}, xx={targetXx})");
                return sourceNotes[i];
            }
        }

        MelonLogger.Msg($"[ExperimentChart] sourceNote 폴백: spec={spec.Label}, isLong={spec.IsLong}, scene={scenePrefix}, targetXx={targetXx} → sourceNotes[{SourceNoteIndex}] 사용");
        return fallback;
    }

    public static void AddExperimentNotes(Il2CppSystem.Collections.Generic.List<MusicData> outputList, MusicData sourceNote, ExperimentNoteSpec spec)
    {
        int count = System.Math.Max(1, spec.Count);
        var template = CloneMusicData(sourceNote);

        DebugSpec("[ExperimentDebug] spec", spec);
        DebugNote("[ExperimentDebug] template before spec", template);

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
        DebugNote($"[ExperimentDebug] {label} after MoveNote", note);
        ApplyNoteSpec(ref note, spec);
        DebugNote($"[ExperimentDebug] {label} after ApplyNoteSpec", note);
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
        DebugNote($"[ExperimentDebug] {label} start after MoveNote", start);
        ApplyNoteSpec(ref start, spec);
        DebugNote($"[ExperimentDebug] {label} start after ApplyNoteSpec", start);
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
        DebugNote($"[ExperimentDebug] {label} end after MoveNote", end);
        ApplyNoteSpec(ref end, spec);
        DebugNote($"[ExperimentDebug] {label} end after ApplyNoteSpec", end);
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
        DebugMsg($"[ExperimentDebug] ApplyNoteSpec begin label={spec.Label}");
        DebugNote("[ExperimentDebug] ApplyNoteSpec input", note);

        var noteData = CloneNoteConfigData(note.noteData) ?? new NoteConfigData();
        var configData = CloneMusicConfigData(note.configData);

        string uid = !string.IsNullOrEmpty(spec.Uid) ? spec.Uid : noteData.uid;
        int noteType = spec.NoteType >= 0 ? spec.NoteType : (int)noteData.type;
        int pathway = spec.Pathway >= 0 ? spec.Pathway : noteData.pathway;

        if (spec.IsLong) noteType = NoteTypes.Long;
        if (spec.IsMul) noteType = NoteTypes.Sandbag;

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
                noteData.scene = "scene_" + UidCode.Scene(uid);
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

        DebugMsg($"[ExperimentDebug] ApplyNoteSpec resolved uid={uid}, noteType={noteType}, pathway={pathway}, scene={noteData.scene}, prefab={noteData.prefab_name}, keyAudio={noteData.key_audio}");
        DebugNote("[ExperimentDebug] ApplyNoteSpec output", note);
    }

    public static void MoveNote(ref MusicData note, int objId, double tick, double length, ExperimentNoteSpec spec)
    {
        bool isBossEvent = spec?.NoteType == NoteTypes.Boss && !string.IsNullOrWhiteSpace(spec.BossAction);
        double normalizedConfigTime = isBossEvent ? tick : NormalizeChartValue(tick);
        double normalizedTick = NormalizeTimingValue(isBossEvent ? tick + BossEventTickOffset : tick);
        double normalizedDt = NormalizeTimingValue(GetEffectiveDt(note, spec));
        double normalizedShowTick = NormalizeChartValue(normalizedTick - normalizedDt);

        note.objId = (short)objId;
        note.tick = (Il2CppSystem.Decimal)normalizedTick;
        note.dt = (Il2CppSystem.Decimal)normalizedDt;
        note.showTick = (Il2CppSystem.Decimal)normalizedShowTick;

        if (note.noteData != null)
        {
            note.noteData.id = objId.ToString(System.Globalization.CultureInfo.InvariantCulture);
        }

        if (note.configData != null)
        {
            var configData = CloneMusicConfigData(note.configData);
            configData.id = objId;
            configData.time = (Il2CppSystem.Decimal)(isBossEvent ? normalizedConfigTime : normalizedTick);
            configData.length = (Il2CppSystem.Decimal)NormalizeChartValue(length);
            note.configData = configData;
        }
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

        if (note.noteData?.type == NoteTypes.Boss && !string.IsNullOrWhiteSpace(note.noteData.boss_action))
        {
            note.doubleIdx = -1;
        }
    }
}
