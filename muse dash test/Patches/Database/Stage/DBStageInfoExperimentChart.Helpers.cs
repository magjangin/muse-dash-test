using MelonLoader;
using Il2CppAssets.Scripts.Database;
using Il2CppGameLogic;
using System.Reflection;

public partial class DBStageInfo_SetRuntimeMusicData_Patch
{
    public static bool IsUidMiddlePair(string uid, string middlePair)
    {
        if (string.IsNullOrEmpty(uid) || string.IsNullOrEmpty(middlePair) || uid.Length < 4 || middlePair.Length != 2)
        {
            return false;
        }

        return string.Equals(uid.Substring(2, 2), middlePair, System.StringComparison.OrdinalIgnoreCase);
    }

    public static string BuildPrefabName(string uid, int noteType, int pathway)
    {
        if (string.IsNullOrEmpty(uid) || uid.Length < 6)
            return uid;

        string xx = uid.Substring(2, 2);
        string yy = uid.Substring(4, 2);
        string lane = pathway == 1 ? "air" : "road";
        string motion = "nor";
        string variant = GetPrefabVariant(uid, noteType);

        if (xx == "15") motion = "down";
        else if (xx == "16") motion = "up";
        else if (yy == "07" || yy == "10") motion = "up";
        else if (yy == "13" || yy == "16") motion = "down";

        return $"{uid}_{lane}_{motion}_{variant}";
    }

    public static string GetPrefabVariant(string uid, int noteType)
    {
        if (string.Equals(uid, "000301", System.StringComparison.OrdinalIgnoreCase)
            || string.Equals(uid, "000304", System.StringComparison.OrdinalIgnoreCase))
        {
            return "1";
        }

        if (noteType == 7)
        {
            return "2";
        }

        return "1";
    }

    public static string GetPathwayLabel(int pathway)
    {
        return pathway == 1 ? "공중" : "지상";
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

    public static System.Collections.Generic.List<ExperimentNoteSpec> BuildBmsExperimentNotes(muse_dash_test.BmsChart chart)
    {
        var specs = new System.Collections.Generic.List<ExperimentNoteSpec>();
        if (chart?.Notes == null || chart.Notes.Count == 0)
        {
            return specs;
        }

        var pairedNotes = new System.Collections.Generic.HashSet<muse_dash_test.BmsNote>();
        var matchedPairs = muse_dash_test.BmsNoteMatcher.MatchSpecialNotes(chart.Notes, chart);
        foreach (var pair in matchedPairs)
        {
            if (pair?.StartNote == null || pair.EndNote == null)
            {
                continue;
            }

            var wavInfo = muse_dash_test.BmsBossSwapPlanner.ResolveWavInfo(chart, pair.StartNote);
            if (wavInfo == null)
            {
                continue;
            }

            pairedNotes.Add(pair.StartNote);
            pairedNotes.Add(pair.EndNote);

            var spec = CreateExperimentNoteSpecFromBms(pair.StartNote, wavInfo);
            spec.Label = $"BMS {pair.Type} {pair.StartNote.RawValue}";
            spec.StartTick = pair.StartNote.Time;
            spec.Length = System.Math.Max(0.0, pair.Duration);
            spec.IsLong = pair.Type == muse_dash_test.BmsSpecialNoteType.Hold;
            spec.IsMul = pair.Type == muse_dash_test.BmsSpecialNoteType.Sandbag;
            specs.Add(spec);
        }

        foreach (var note in chart.Notes)
        {
            if (note == null || pairedNotes.Contains(note))
            {
                continue;
            }

            var wavInfo = muse_dash_test.BmsBossSwapPlanner.ResolveWavInfo(chart, note);
            if (wavInfo == null)
            {
                continue;
            }

            if (wavInfo.NoteType == 3 || wavInfo.NoteType == 8)
            {
                MelonLogger.Warning($"[ExperimentChart.Bms] 특수 노트가 짝 없이 남아 단일 주입을 건너뜁니다: raw={note.RawValue}, tick={note.Tick}, time={note.Time:0.###}, wav={wavInfo.RawWavName}");
                continue;
            }

            specs.Add(CreateExperimentNoteSpecFromBms(note, wavInfo));
        }

        specs.Sort((left, right) =>
        {
            int tickCompare = left.StartTick.CompareTo(right.StartTick);
            if (tickCompare != 0) return tickCompare;
            return string.Compare(left.Label, right.Label, System.StringComparison.OrdinalIgnoreCase);
        });

        MelonLogger.Msg($"[ExperimentChart.Bms] BMS 변환 완료: notes={chart.Notes.Count}, specs={specs.Count}, matchedPairs={matchedPairs.Count}");
        return specs;
    }

    public static ExperimentNoteSpec CreateExperimentNoteSpecFromBms(muse_dash_test.BmsNote note, muse_dash_test.BmsWavInfo wavInfo)
    {
        var spec = new ExperimentNoteSpec
        {
            Label = $"BMS {note.RawValue}",
            Uid = wavInfo.Uid ?? "",
            PrefabName = ShouldKeepBmsPrefabName(wavInfo) ? wavInfo.PrefabName : "",
            KeyAudio = wavInfo.KeyAudio ?? "",
            BossAction = wavInfo.BossAction ?? "",
            BossName = wavInfo.BossName ?? "",
            BossScene = wavInfo.BossScene,
            NoteType = wavInfo.NoteType,
            Pathway = ResolveBmsPathway(note, wavInfo),
            StartTick = System.Math.Round(note.Time, 2, System.MidpointRounding.AwayFromZero),
            Dt = wavInfo.Dt >= 0.0 ? NormalizeTimingValue(wavInfo.Dt) : wavInfo.Dt
        };

        if (!string.IsNullOrEmpty(spec.BossAction) && !string.IsNullOrEmpty(spec.Uid) && spec.Uid.Length >= 2)
        {
            spec.Scene = "scene_" + spec.Uid.Substring(0, 2);
        }

        if (!string.IsNullOrEmpty(spec.BossAction) && muse_dash_test.MainMod.TryGetCachedHwaScene(out int manifestScene))
        {
            spec.Scene = $"scene_{manifestScene:00}";
        }

        if (!string.IsNullOrWhiteSpace(spec.BossAction))
        {
            MelonLogger.Msg($"[ExperimentChart.Bms.BossFields] raw={note.RawValue}, uid={spec.Uid}, action={spec.BossAction}, BossName={spec.BossName}, BossScene={spec.BossScene}, Scene={spec.Scene}");
        }

        return spec;
    }

    public static bool ShouldKeepBmsPrefabName(muse_dash_test.BmsWavInfo wavInfo)
    {
        if (wavInfo == null)
        {
            return false;
        }

        if (!string.IsNullOrWhiteSpace(wavInfo.BossAction) && wavInfo.NoteType == 0)
        {
            return true;
        }

        if (string.IsNullOrWhiteSpace(wavInfo.PrefabName) || string.IsNullOrWhiteSpace(wavInfo.Uid))
        {
            return false;
        }

        return !string.Equals(wavInfo.PrefabName, wavInfo.Uid, System.StringComparison.OrdinalIgnoreCase);
    }

    public static int ResolveBmsPathway(muse_dash_test.BmsNote note, muse_dash_test.BmsWavInfo wavInfo)
    {
        if (note == null)
        {
            return 0;
        }

        if (note.Lane == muse_dash_test.BmsLane.Air)
        {
            return 1;
        }

        if (!string.IsNullOrWhiteSpace(wavInfo?.Uid) && wavInfo.Uid.Length >= 6)
        {
            string yy = wavInfo.Uid.Substring(4, 2);
            if (yy == "04" || yy == "10" || yy == "16")
            {
                return 1;
            }
        }

        return 0;
    }

    public static System.Collections.Generic.List<ExperimentNoteSpec> BuildRuntimeExperimentNotes(System.Collections.Generic.IReadOnlyList<ExperimentNoteSpec> specs)
    {
        var runtimeSpecs = new System.Collections.Generic.List<ExperimentNoteSpec>();
        if (specs == null || specs.Count == 0)
        {
            return runtimeSpecs;
        }

        bool waitingForSwapIn = false;
        double lastOutTick = 0.0;

        for (int i = 0; i < specs.Count; i++)
        {
            var spec = CloneExperimentNoteSpec(specs[i]);
            if (spec == null)
            {
                continue;
            }

            if (IsBossOutAction(spec))
            {
                waitingForSwapIn = true;
                lastOutTick = spec.StartTick;
                runtimeSpecs.Add(spec);
                continue;
            }

            if (waitingForSwapIn && IsBossInAction(spec) && spec.StartTick > lastOutTick)
            {
                string swapAction = BuildBossSwapAction(spec);
                if (!string.IsNullOrWhiteSpace(swapAction))
                {
                    MelonLogger.Msg($"[BossAutoSwap] out 이후 in 감지: label={spec.Label}, tick={spec.StartTick}, action={swapAction}");
                    spec.BossAction = swapAction;
                    spec.NoteType = 0;
                    spec.PrefabName = "empty_000";
                    spec.KeyAudio = "";
                }
                else
                {
                    MelonLogger.Warning($"[BossAutoSwap] out 이후 in을 감지했지만 BossName/BossScene을 결정하지 못했습니다: label={spec.Label}, uid={spec.Uid}");
                }

                waitingForSwapIn = false;
            }

            runtimeSpecs.Add(spec);
        }

        return runtimeSpecs;
    }

    public static ExperimentNoteSpec CloneExperimentNoteSpec(ExperimentNoteSpec source)
    {
        if (source == null) return null;

        return new ExperimentNoteSpec
        {
            Label = source.Label,
            Uid = source.Uid,
            Des = source.Des,
            PrefabName = source.PrefabName,
            KeyAudio = source.KeyAudio,
            BossAction = source.BossAction,
            BossName = source.BossName,
            BossScene = source.BossScene,
            Scene = source.Scene,
            IbmsId = source.IbmsId,
            NoteType = source.NoteType,
            Pathway = source.Pathway,
            IsLong = source.IsLong,
            IsMul = source.IsMul,
            StartTick = source.StartTick,
            Count = source.Count,
            Interval = source.Interval,
            Length = source.Length,
            Speed = source.Speed,
            Dt = source.Dt
        };
    }

    public static bool IsBossOutAction(ExperimentNoteSpec spec)
    {
        return string.Equals(spec?.BossAction, "out", System.StringComparison.OrdinalIgnoreCase);
    }

    public static bool IsBossInAction(ExperimentNoteSpec spec)
    {
        return string.Equals(spec?.BossAction, "in", System.StringComparison.OrdinalIgnoreCase);
    }

    public static string BuildBossSwapAction(ExperimentNoteSpec spec)
    {
        if (spec == null)
        {
            return string.Empty;
        }

        string bossName = !string.IsNullOrWhiteSpace(spec.BossName) ? spec.BossName : TryInferBossNameFromUid(spec.Uid);
        int bossScene = spec.BossScene >= 0 ? spec.BossScene : TryInferBossSceneFromUid(spec.Uid);
        if (string.IsNullOrWhiteSpace(bossName) || bossScene < 0)
        {
            return string.Empty;
        }

        return $"swap:{bossName}:{bossScene}";
    }

    public static string TryInferBossNameFromUid(string uid)
    {
        if (string.IsNullOrWhiteSpace(uid) || uid.Length < 2)
        {
            return string.Empty;
        }

        if (!int.TryParse(uid.Substring(0, 2), out int scene))
        {
            return string.Empty;
        }

        return $"{scene:00}01_boss";
    }

    public static int TryInferBossSceneFromUid(string uid)
    {
        if (string.IsNullOrWhiteSpace(uid) || uid.Length < 2)
        {
            return -1;
        }

        if (int.TryParse(uid.Substring(0, 2), out int scene))
        {
            return scene;
        }

        return -1;
    }

    public static void MoveNote(ref MusicData note, int objId, double tick, double length, ExperimentNoteSpec spec)
    {
        double normalizedTick = NormalizeTimingValue(tick);
        double normalizedDt = NormalizeTimingValue(GetEffectiveDt(note, spec));
        double normalizedShowTick = NormalizeShowTickValue(normalizedTick - normalizedDt);

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
            configData.time = (Il2CppSystem.Decimal)normalizedTick;
            configData.length = (Il2CppSystem.Decimal)NormalizeTimingValue(length);
            note.configData = configData;
        }
    }

    public static double NormalizeTimingValue(double value)
    {
        return System.Math.Round(value, 3, System.MidpointRounding.AwayFromZero);
    }

    public static double NormalizeShowTickValue(double value)
    {
        return System.Math.Round(value, 2, System.MidpointRounding.AwayFromZero);
    }

    public static double GetEffectiveDt(MusicData note)
    {
        return GetEffectiveDt(note, null);
    }

    public static double GetEffectiveDt(MusicData note, ExperimentNoteSpec spec)
    {
        if (spec != null && spec.Dt >= 0.0) return spec.Dt;

        string uid = !string.IsNullOrEmpty(spec?.Uid) ? spec.Uid : note.noteData?.uid;
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
        bool isBmsNote = label != null && label.StartsWith("BMS ", System.StringComparison.OrdinalIgnoreCase);
        if (!isBmsNote && !IsUidMiddlePair(uid, "09")) return;
        string prefab = note.noteData?.prefab_name ?? "(null)";
        string type = note.noteData != null ? note.noteData.type.ToString() : "(null)";
        string pathway = note.noteData != null ? note.noteData.pathway.ToString() : "(null)";
        string pathwayLabel = note.noteData != null ? GetPathwayLabel(note.noteData.pathway) : "(null)";
        string speed = note.noteData != null ? note.noteData.speed.ToString() : "(null)";
        string scene = note.noteData?.scene ?? "(null)";
        string bossAction = note.noteData?.boss_action ?? "(null)";
        MelonLogger.Msg($"실험 노트 추가: {label}, objId={note.objId}, tick={note.tick}, dt={note.dt}, showTick={note.showTick}, speed={speed}, uid={uid}, type={type}, pathway={pathway}({pathwayLabel}), scene={scene}, prefab={prefab}, boss_action={bossAction}");
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

        return uid.Substring(2, 4);
    }
}
