using MelonLoader;
using Il2CppGameLogic;

// 삽입된 BMS 노트의 더블 상태(지상+공중 동시) 판정과 showTick 기준 재정렬 로직.
public partial class DBStageInfo_SetRuntimeMusicData_Patch
{
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
}
