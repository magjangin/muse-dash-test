using MelonLoader;
using Il2CppAssets.Scripts.Database;
using Il2CppGameLogic;

// 실험 차트 디버깅용 덤프/로깅 헬퍼. 게임 로직에는 영향을 주지 않습니다.
public partial class DBStageInfo_SetRuntimeMusicData_Patch
{
    // DebugExperimentNotes 플래그를 내부에서 검사하는 게이트 래퍼들.
    // 호출부에서 매번 'if (DebugExperimentNotes)'를 반복하지 않도록 해 파이프라인 가독성을 높입니다.
    private static void DebugMsg(string message)
    {
        if (DebugExperimentNotes) MelonLogger.Msg(message);
    }

    private static void DebugNote(string label, MusicData note)
    {
        if (DebugExperimentNotes) LogNoteState(label, note);
    }

    private static void DebugSpec(string label, ExperimentNoteSpec spec)
    {
        if (DebugExperimentNotes) LogSpec(label, spec);
    }

    public static void DumpMusicList(DBStageInfo __instance)
    {
        var musicList = __instance._musicList_k__BackingField;
        if (musicList == null)
        {
            return;
        }

        // 1. 원본 차트 전체 노트 중 모드 표준 노트를 제외한 신규/미등록 노트(UID 및 NoteType > 17) 서치 로그
        LogUnregisteredOriginalChartNotes(musicList);

        // 2. 샌드백/연타 노트(zz04yy & Type 8) 전용 별도 분석 로그
        LogSandbagAnalysisOriginalChartNotes(musicList);
    }

    /// <summary>
    /// 원본 차트(공식 곡) 덤프 시, 모드 기본 노트 UID 및 표준 NoteType(0~17)을 제외한
    /// 새로운/미등록 노트(UID 및 NoteType > 17)를 감지하여 덤프 출력합니다.
    /// </summary>
    public static void LogUnregisteredOriginalChartNotes(Il2CppSystem.Collections.Generic.List<MusicData> musicList)
    {
        if (musicList == null || musicList.Count == 0) return;

        var loggedUids = new System.Collections.Generic.HashSet<string>(System.StringComparer.OrdinalIgnoreCase);
        int newNoteTotalCount = 0;

        MelonLogger.Msg($"[OfficialSceneContext] ★ 원본 차트 신규/미등록 노트(UID & NoteType) 서치 시작 (total={musicList.Count}) ★");

        for (int i = 0; i < musicList.Count; i++)
        {
            var note = musicList[i];
            if (note?.noteData == null) continue;

            string uid = note.noteData.uid ?? string.Empty;
            uint noteTypeVal = (uint)note.noteData.type;

            // 표준 NoteType (0:None ~ 17:SceneHideOff) 범위 및 모드 표준 UID 패턴인지 검사
            bool isStandardType = noteTypeVal <= 17;
            bool isKnownUidPrefix = isStandardType && (
                uid.StartsWith("0001") || uid.StartsWith("0002") || uid.StartsWith("0003") ||
                uid.StartsWith("0004") || uid.StartsWith("0017") ||
                (uid.Length >= 6 && (uid.StartsWith("01") || uid.StartsWith("02") || uid.StartsWith("03") ||
                                     uid.StartsWith("04") || uid.StartsWith("05") || uid.StartsWith("06") ||
                                     uid.StartsWith("07") || uid.StartsWith("08") || uid.StartsWith("09")))
            );

            // 표준 노트(기본 몬스터/블록/홀드/보스/HP/점수/씬체인지)가 아닌 새로운 노트인 경우
            bool isUnregisteredOrNew = !isStandardType || (!string.IsNullOrEmpty(uid) && !isKnownUidPrefix);

            if (isUnregisteredOrNew)
            {
                newNoteTotalCount++;
                string key = $"{uid}_type{noteTypeVal}";

                // 콘솔 중복 출력을 방지하며 고유한 신규 노트 타입/UID만 상세 출력
                if (!loggedUids.Contains(key))
                {
                    loggedUids.Add(key);

                    string ibmsId = SafeLogValue(() => note.noteData.ibms_id);
                    string prefab = SafeLogValue(() => note.noteData.prefab_name);
                    string bossAction = SafeLogValue(() => note.noteData.boss_action);
                    string pathway = SafeLogValue(() => note.noteData.pathway);
                    string keyAudio = SafeLogValue(() => note.noteData.key_audio);
                    string scene = SafeLogValue(() => note.noteData.scene);
                    string tick = SafeLogValue(() => note.tick);
                    string dt = SafeLogValue(() => note.dt);
                    string showTick = SafeLogValue(() => note.showTick);

                    MelonLogger.Msg(
                        $"[OfficialSceneContext] ★신규/미등록 노트 감지★ index={i}, objId={note.objId}, tick={tick}, dt={dt}, showTick={showTick}, " +
                        $"uid={uid}, ibms_id={ibmsId}, type={noteTypeVal}, scene={scene}, pathway={pathway}, " +
                        $"prefab={prefab}, key_audio={keyAudio}, boss_action={bossAction}"
                    );
                }
            }
        }

        MelonLogger.Msg($"[OfficialSceneContext] ★ 원본 차트 신규/미등록 노트 서치 완료: 감지 노트={newNoteTotalCount}개, 고유 타입={loggedUids.Count}개 ★");
    }

    /// <summary>
    /// 원본 차트(공식 곡) 덤프 시, UID/프리팹의 중간 2자리 xx가 "01" (보스 액션/전환) 또는 "04" (샌드백/연타)인
    /// zz01yy 및 zz04yy 전용 노트만을 덤프 출력합니다.
    /// </summary>
    public static void LogSandbagAnalysisOriginalChartNotes(Il2CppSystem.Collections.Generic.List<MusicData> musicList)
    {
        if (musicList == null || musicList.Count == 0) return;

        var loggedKeys = new System.Collections.Generic.HashSet<string>(System.StringComparer.OrdinalIgnoreCase);
        int matchedTotalCount = 0;

        MelonLogger.Msg($"[Zz01Zz04Analysis] ★ 원본 차트 zz01yy & zz04yy 노트 서치 시작 (total={musicList.Count}) ★");

        for (int i = 0; i < musicList.Count; i++)
        {
            var note = musicList[i];
            if (note?.noteData == null) continue;

            string uid = note.noteData.uid ?? string.Empty;
            string prefabStr = note.noteData.prefab_name ?? string.Empty;
            uint noteTypeVal = (uint)note.noteData.type;

            string xxFromUid = (uid.Length >= 4) ? UidCode.Xx(uid) : string.Empty;
            string xxFromPrefab = (prefabStr.Length >= 4) ? prefabStr.Substring(2, 2) : string.Empty;

            // zz01yy (xx="01") 또는 zz04yy (xx="04") 패턴인 노트 필터링
            bool isZz01OrZz04 = xxFromUid == "01" || xxFromUid == "04" || xxFromPrefab == "01" || xxFromPrefab == "04" || noteTypeVal == 8;

            if (isZz01OrZz04)
            {
                matchedTotalCount++;
                string bossAction = SafeLogValue(() => note.noteData.boss_action);
                string key = $"{i}_{uid}_type{noteTypeVal}_{prefabStr}_{bossAction}_{note.tick}";

                if (!loggedKeys.Contains(key))
                {
                    loggedKeys.Add(key);

                    string ibmsId = SafeLogValue(() => note.noteData.ibms_id);
                    string prefab = SafeLogValue(() => note.noteData.prefab_name);
                    string pathway = SafeLogValue(() => note.noteData.pathway);
                    string keyAudio = SafeLogValue(() => note.noteData.key_audio);
                    string scene = SafeLogValue(() => note.noteData.scene);
                    string tick = SafeLogValue(() => note.tick);
                    string dt = SafeLogValue(() => note.dt);
                    string showTick = SafeLogValue(() => note.showTick);

                    MelonLogger.Msg(
                        $"[Zz01Zz04Analysis] ★zz01yy/zz04yy 노트 감지★ index={i}, objId={note.objId}, tick={tick}, dt={dt}, showTick={showTick}, " +
                        $"uid={uid}, ibms_id={ibmsId}, type={noteTypeVal}, scene={scene}, pathway={pathway}, " +
                        $"prefab={prefab}, key_audio={keyAudio}, boss_action={bossAction}"
                    );
                }
            }
        }

        MelonLogger.Msg($"[Zz01Zz04Analysis] ★ 원본 차트 zz01yy & zz04yy 서치 완료: 감지 노트={matchedTotalCount}개, 출력 고유 노트={loggedKeys.Count}개 ★");
    }

    public static void DumpSortedBmsBossContext(Il2CppSystem.Collections.Generic.List<MusicData> musicList, int startIndex)
    {
        for (int i = startIndex; i < musicList.Count; i++)
        {
            var note = musicList[i];
            if (note.noteData?.type != NoteTypes.Boss || string.IsNullOrWhiteSpace(note.noteData.boss_action))
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

                string scene = UidCode.Scene(uid);
                string xxyy = UidCode.Xx(uid);
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

    public static void LogInsertedNote(string label, MusicData note)
    {
        string uid = note.noteData?.uid ?? "(null)";
        bool isBmsNote = label != null && label.StartsWith("BMS ", System.StringComparison.OrdinalIgnoreCase);
        if (!isBmsNote && !IsUidMiddlePair(uid, "09")) return;
        string prefab = note.noteData?.prefab_name ?? "(null)";
        string type = note.noteData != null ? note.noteData.type.ToString() : "(null)";
        string pathway = note.noteData != null ? note.noteData.pathway.ToString() : "(null)";
        string pathwayLabel = note.noteData != null ? GetPathwayLabel(note.noteData.pathway) : "(null)";
        string speed = note.noteData != null ? note.noteData.speed.ToString() : "(null)";
        string scene = note.noteData?.scene ?? "(null)";
        string bossAction = note.noteData?.boss_action ?? "(null)";
        string configTime = note.configData != null ? note.configData.time.ToString() : "(null)";
        string effect = note.noteData != null ? note.noteData.effect.ToString() : "(null)";
        string isShowPlayEffect = note.noteData != null ? note.noteData.isShowPlayEffect.ToString() : "(null)";
        string sceneChangeNames = note.noteData != null ? FormatSceneChangeNames(note.noteData.sceneChangeNames) : "(null)";
        string ibmsId = note.noteData?.ibms_id ?? "(null)";
        MelonLogger.Msg($"실험 노트 추가: {label}, objId={note.objId}, tick={note.tick}, dt={note.dt}, showTick={note.showTick}, config.time={configTime}, speed={speed}, uid={uid}, ibms_id={ibmsId}, type={type}, pathway={pathway}({pathwayLabel}), scene={scene}, sceneChangeNames={sceneChangeNames}, prefab={prefab}, boss_action={bossAction}, effect={effect}, isShowPlayEffect={isShowPlayEffect}");
    }

    // note.noteData.sceneChangeNames(Il2Cpp List<string>)를 사람이 읽기 좋은 형태로 변환합니다.
    private static string FormatSceneChangeNames(Il2CppSystem.Collections.Generic.List<string> names)
    {
        if (names == null) return "(null)";
        try
        {
            var values = new System.Collections.Generic.List<string>();
            for (int i = 0; i < names.Count; i++)
            {
                values.Add(names[i] ?? "(null)");
            }
            return "[" + string.Join(",", values) + "]";
        }
        catch
        {
            return "(error)";
        }
    }

    public static void LogSpec(string label, ExperimentNoteSpec spec)
    {
        string sceneChangeNames = spec.SceneChangeNames != null ? string.Join(",", spec.SceneChangeNames) : "(null)";
        MelonLogger.Msg($"{label}: Label={spec.Label}, Uid={spec.Uid}, NoteType={spec.NoteType}, Pathway={spec.Pathway}, PrefabName={spec.PrefabName}, KeyAudio={spec.KeyAudio}, BossAction={spec.BossAction}, BossName={spec.BossName}, BossScene={spec.BossScene}, Scene={spec.Scene}, IbmsId={spec.IbmsId}, SceneChangeNames={sceneChangeNames}, IsLong={spec.IsLong}, IsMul={spec.IsMul}, StartTick={spec.StartTick}, Count={spec.Count}, Interval={spec.Interval}, Length={spec.Length}, Speed={spec.Speed}, Dt={spec.Dt}");
    }

    public static void LogNoteState(string label, MusicData note)
    {
        string noteUid = SafeLogValue(() => note.noteData?.uid);
        bool isExperimentDebug = label != null && label.StartsWith("[ExperimentDebug]", System.StringComparison.OrdinalIgnoreCase);
        if (!isExperimentDebug && !IsUidMiddlePair(noteUid, "09")) return;
        string noteType = SafeLogValue(() => note.noteData?.type);
        string notePathway = SafeLogValue(() => note.noteData?.pathway);
        string notePathwayLabel = SafeLogValue(() => GetPathwayLabel(note.noteData?.pathway ?? 0));
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

        MelonLogger.Msg($"{label}: objId={note.objId}, tick={note.tick}, dt={note.dt}, showTick={note.showTick}, note.uid={noteUid}, note.type={noteType}, note.pathway={notePathway}({notePathwayLabel}), note.noteUid={noteUidValue}, note.m_BmsUid={bmsUid}, note.prefab={prefab}, note.speed={speed}, note.key_audio={keyAudio}, note.boss_action={bossAction}, config.id={configId}, config.time={configTime}, config.note_uid={configUid}, config.length={configLength}, config.pathway={configPathway}, isLongPressing={note.isLongPressing}, isLongPressEnd={note.isLongPressEnd}, longPressPTick={note.longPressPTick}, endIndex={note.endIndex}, longPressNum={note.longPressNum}");
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

    public static string GetBossActionGroup(string bossAction)
    {
        if (string.IsNullOrWhiteSpace(bossAction))
        {
            return "none";
        }

        if (bossAction == "boss_far_atk_2")
        {
            return "boss_far_atk_2";
        }

        if (bossAction == "boss_far_atk_1_R")
        {
            return "boss_far_atk_1_R";
        }

        return "other";
    }

    public static bool IsTargetDebugUid(string uid)
    {
        return uid == "090908" || uid == "090902" || uid == "090911";
    }

    public static bool IsTargetDebugXxyy(string uid)
    {
        string xxyy = GetUidXxyy(uid);
        return xxyy == "0908" || xxyy == "0902" || xxyy == "0911";
    }

    public static string GetUidXxyy(string uid)
    {
        if (string.IsNullOrEmpty(uid) || uid.Length < 6)
        {
            return string.Empty;
        }

        return UidCode.Xx(uid) + UidCode.Yy(uid);
    }
}
