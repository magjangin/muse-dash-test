using MelonLoader;
using Il2CppGameLogic;
using muse_dash_test;

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
        // 정렬 키(showTick/tick)는 노트당 한 번만 뽑아 둔다. ParseMusicDecimal은 Il2Cpp 문자열
        // 마샬링을 동반하므로, 비교자 안에서 매번 호출하면 O(n log n)번 문자열 변환이 일어난다.
        var runtimeNotes = new System.Collections.Generic.List<SortEntry>(musicList.Count - startIndex);
        var references = new NoteReferenceSnapshot();

        for (int i = startIndex; i < musicList.Count; i++)
        {
            var note = musicList[i];
            runtimeNotes.Add(new SortEntry(note, ParseMusicDecimal(note.showTick), ParseMusicDecimal(note.tick)));
            references.Capture(note);
        }

        runtimeNotes.Sort((left, right) =>
        {
            int showTickCompare = left.ShowTick.CompareTo(right.ShowTick);
            if (showTickCompare != 0) return showTickCompare;

            int tickCompare = left.Tick.CompareTo(right.Tick);
            if (tickCompare != 0) return tickCompare;

            return left.ObjId.CompareTo(right.ObjId);
        });

        // 정렬 후 확정된 새 위치를 (옛 objId → 새 인덱스)로 등록.
        for (int i = 0; i < runtimeNotes.Count; i++)
        {
            references.MapNewIndex(runtimeNotes[i].ObjId, startIndex + i);
        }

        while (musicList.Count > startIndex)
        {
            musicList.RemoveAt(musicList.Count - 1);
        }

        for (int i = 0; i < runtimeNotes.Count; i++)
        {
            var note = runtimeNotes[i].Note;
            short oldObjId = runtimeNotes[i].ObjId;
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

        ModLogger.Msg($"[ExperimentChart.Bms] 공식 방식 showTick 정렬 완료: notes={runtimeNotes.Count}, bossOffset={BossEventTickOffset}");
        DumpSortedBmsBossContext(musicList, startIndex);
    }

    /// <summary>
    /// 정렬용 노트 항목. showTick/tick을 미리 double로 뽑아 두어 비교자가 Il2Cpp 문자열 변환을
    /// 반복하지 않도록 합니다. ObjId도 정렬 전 값으로 고정 보관해, 재연결 시 옛 키로 쓸 수 있게 합니다.
    /// </summary>
    private readonly struct SortEntry
    {
        public readonly MusicData Note;
        public readonly double ShowTick;
        public readonly double Tick;
        public readonly short ObjId;

        public SortEntry(MusicData note, double showTick, double tick)
        {
            Note = note;
            ShowTick = showTick;
            Tick = tick;
            ObjId = note.objId;
        }
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

    // ParseMusicDecimal이 두 로케일 모두에서 실패했을 때 로그가 노트 수만큼 쏟아지는 것을 막습니다.
    private static bool warnedDecimalParseFailure = false;

    /// <summary>
    /// Il2CppSystem.Decimal을 double로 변환합니다.
    /// <para>
    /// 주의: <c>Decimal.ToString()</c>은 인자가 없으므로 <b>CurrentCulture</b>를 따릅니다.
    /// 소수점 구분자가 쉼표인 로케일(de/fr/ru 등)에서 게임이 돌면 "15,5"가 나오는데,
    /// 이를 InvariantCulture로만 파싱하면 실패해서 0.0이 됩니다. 이 값은 정렬 비교뿐 아니라
    /// <see cref="ApplyBmsDoubleState"/>에서 dt/showTick으로 <b>되써지기 때문에</b>,
    /// 0.0 폴백은 더블 노트의 등장 타이밍을 통째로 망가뜨립니다.
    /// 그래서 ToString()과 같은 CurrentCulture로 먼저 시도하고 InvariantCulture로 재시도합니다.
    /// </para>
    /// </summary>
    public static double ParseMusicDecimal(Il2CppSystem.Decimal value)
    {
        string raw = value.ToString();

        // ToString()이 CurrentCulture를 썼으므로 같은 문화권으로 먼저 되돌립니다.
        if (double.TryParse(raw, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.CurrentCulture, out double parsed))
        {
            return parsed;
        }

        // 게임이 InvariantGlobalization으로 빌드된 경우 등을 위한 2차 시도.
        if (double.TryParse(raw, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out parsed))
        {
            return parsed;
        }

        // 여기까지 왔으면 차트 타이밍이 손상됩니다. 조용히 0을 넘기지 않고 반드시 알립니다.
        if (!warnedDecimalParseFailure)
        {
            warnedDecimalParseFailure = true;
            ModLogger.Error($"[ExperimentChart] Decimal 파싱 실패: raw='{raw}', culture={System.Globalization.CultureInfo.CurrentCulture.Name}. " +
                              "이 상태에서는 더블 노트의 dt/showTick이 0으로 덮어써져 채보 타이밍이 깨집니다. (이 경고는 1회만 출력)");
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

            int pairCount = System.Math.Min(roadIndices.Count, airIndices.Count);
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

        ModLogger.Msg($"[ExperimentChart.Bms] 더블 상태 적용 완료: pairs={doubleGroupCount}, notes={musicList.Count - startIndex}");
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
