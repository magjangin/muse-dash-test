using MelonLoader;
using Il2CppAssets.Scripts.Database;
using Il2CppGameLogic;

// 실험 차트 디버깅용 덤프/로깅 헬퍼. 게임 로직에는 영향을 주지 않습니다.
public partial class DBStageInfo_SetRuntimeMusicData_Patch
{
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
            if (note.noteData?.type != NoteTypes.Boss
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
        MelonLogger.Msg($"실험 노트 추가: {label}, objId={note.objId}, tick={note.tick}, dt={note.dt}, showTick={note.showTick}, config.time={configTime}, speed={speed}, uid={uid}, type={type}, pathway={pathway}({pathwayLabel}), scene={scene}, prefab={prefab}, boss_action={bossAction}");
    }

    public static void LogSpec(string label, ExperimentNoteSpec spec)
    {
        MelonLogger.Msg($"{label}: Label={spec.Label}, Uid={spec.Uid}, NoteType={spec.NoteType}, Pathway={spec.Pathway}, PrefabName={spec.PrefabName}, KeyAudio={spec.KeyAudio}, BossAction={spec.BossAction}, BossName={spec.BossName}, BossScene={spec.BossScene}, Scene={spec.Scene}, IbmsId={spec.IbmsId}, IsLong={spec.IsLong}, IsMul={spec.IsMul}, StartTick={spec.StartTick}, Count={spec.Count}, Interval={spec.Interval}, Length={spec.Length}, Speed={spec.Speed}, Dt={spec.Dt}");
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
