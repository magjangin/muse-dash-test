using MelonLoader;
using Il2CppGameLogic;

// 삽입된 BMS 노트의 더블 상태(지상+공중 동시) 판정과 showTick 기준 재정렬 로직.
public partial class DBStageInfo_SetRuntimeMusicData_Patch
{
    public static void SortBmsNotesByShowTick(Il2CppSystem.Collections.Generic.List<MusicData> musicList, int startIndex)
    {
        if (musicList == null || musicList.Count <= startIndex)
        {
            return;
        }

        // 정렬은 노트들의 배열 위치를 바꾸는데, MusicData는 서로를 "정수 인덱스"로 가리킨다
        // (endIndex=롱노트 끝, doubleIdx=더블 짝). 따라서 정렬 전 상호참조를 objId 기준으로
        // 스냅샷해 두고, 정렬 후 새 위치로 재연결(Relink)해야 채보가 깨지지 않는다.
        var runtimeNotes = new System.Collections.Generic.List<MusicData>();
        var references = new NoteReferenceSnapshot();

        for (int i = startIndex; i < musicList.Count; i++)
        {
            var note = musicList[i];
            runtimeNotes.Add(note);
            references.Capture(note);
        }

        runtimeNotes.Sort((left, right) =>
        {
            int showTickCompare = ParseMusicDecimal(left.showTick).CompareTo(ParseMusicDecimal(right.showTick));
            if (showTickCompare != 0) return showTickCompare;

            int tickCompare = ParseMusicDecimal(left.tick).CompareTo(ParseMusicDecimal(right.tick));
            if (tickCompare != 0) return tickCompare;

            return left.objId.CompareTo(right.objId);
        });

        // 정렬 후 확정된 새 위치를 (옛 objId → 새 인덱스)로 등록.
        for (int i = 0; i < runtimeNotes.Count; i++)
        {
            references.MapNewIndex(runtimeNotes[i].objId, startIndex + i);
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
            references.Relink(note, oldObjId);

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

    /// <summary>
    /// 정렬 전 노트 간 상호참조(endIndex=롱노트 끝, doubleIdx=더블 짝, isDouble)를 objId 기준으로
    /// 스냅샷해 두었다가, 정렬로 인덱스가 바뀐 뒤 각 노트를 새 위치로 재연결합니다.
    /// MusicData가 서로를 정수 인덱스로 가리키기 때문에, 이 재연결이 없으면 정렬 후 채보가 깨집니다.
    /// 동작은 기존 인라인 로직과 동일하며, 단지 "스냅샷→재연결" 책임을 한곳에 모은 것입니다.
    /// </summary>
    private sealed class NoteReferenceSnapshot
    {
        private readonly System.Collections.Generic.Dictionary<short, int> _oldEndIndices = new System.Collections.Generic.Dictionary<short, int>();
        private readonly System.Collections.Generic.Dictionary<short, int> _oldDoubleIndices = new System.Collections.Generic.Dictionary<short, int>();
        private readonly System.Collections.Generic.Dictionary<short, bool> _oldDoubleStates = new System.Collections.Generic.Dictionary<short, bool>();
        private readonly System.Collections.Generic.Dictionary<short, int> _newIndexByOldObjId = new System.Collections.Generic.Dictionary<short, int>();

        /// <summary>정렬 전 노트의 상호참조 상태를 옛 objId 기준으로 저장합니다.</summary>
        public void Capture(MusicData note)
        {
            _oldEndIndices[note.objId] = note.endIndex;
            _oldDoubleIndices[note.objId] = note.doubleIdx;
            _oldDoubleStates[note.objId] = note.isDouble;
        }

        /// <summary>정렬 후 확정된 (옛 objId → 새 인덱스) 매핑을 등록합니다.</summary>
        public void MapNewIndex(short oldObjId, int newIndex)
        {
            _newIndexByOldObjId[oldObjId] = newIndex;
        }

        /// <summary>옛 objId로 보관한 참조를 새 인덱스로 변환해 note에 다시 채워 넣습니다.</summary>
        public void Relink(MusicData note, short oldObjId)
        {
            note.isDouble = _oldDoubleStates.TryGetValue(oldObjId, out bool wasDouble) && wasDouble;
            note.doubleIdx = note.noteData?.type == NoteTypes.Boss ? -1 : 0;

            if (note.isDouble
                && _oldDoubleIndices.TryGetValue(oldObjId, out int oldDoubleIndex)
                && _newIndexByOldObjId.TryGetValue((short)oldDoubleIndex, out int newDoubleIndex))
            {
                note.doubleIdx = newDoubleIndex;
            }

            if (IsSceneToggleNote(note))
            {
                note.doubleIdx = -1;
            }

            if (_oldEndIndices.TryGetValue(oldObjId, out int oldEndIndex)
                && oldEndIndex > 0
                && _newIndexByOldObjId.TryGetValue((short)oldEndIndex, out int newEndIndex))
            {
                note.endIndex = newEndIndex;
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
        if (note.noteData.type != NoteTypes.Normal) return $"type-{note.noteData.type}";
        if (note.isLongPressing) return "long-press-middle";
        if (note.isLongPressEnd) return "long-press-end";
        return null;
    }
}
