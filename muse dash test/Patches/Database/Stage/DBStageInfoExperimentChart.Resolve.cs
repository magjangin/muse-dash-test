using Il2CppAssets.Scripts.Database;
using Il2CppGameLogic;
using muse_dash_test;

// UID 해석(프리팹/오디오/dt/pathway), MusicData/NoteConfigData 복제, 수치 정규화 헬퍼.
public partial class DBStageInfo_SetRuntimeMusicData_Patch
{
    public static bool IsUidMiddlePair(string uid, string middlePair)
    {
        if (string.IsNullOrEmpty(uid) || string.IsNullOrEmpty(middlePair) || uid.Length < 4 || middlePair.Length != 2)
        {
            return false;
        }

        return string.Equals(UidCode.Xx(uid), middlePair, System.StringComparison.OrdinalIgnoreCase);
    }

    public static string BuildPrefabName(string uid, int noteType, int pathway)
    {
        if (string.IsNullOrEmpty(uid) || uid.Length < 6)
            return uid;

        string xx = UidCode.Xx(uid);
        string lane = pathway == 1 ? "air" : "road";
        string motion = "nor";
        string variant = GetPrefabVariant(uid, noteType);
        int motionGroup = UidCode.MotionGroup(uid);

        if (xx == "15") motion = "down";
        else if (xx == "16") motion = "up";
        else if (motionGroup == 2 || motionGroup == 3) motion = "up";
        else if (motionGroup == 4 || motionGroup == 5) motion = "down";

        return $"{uid}_{lane}_{motion}_{variant}";
    }

    public static string GetPrefabVariant(string uid, int noteType)
    {
        // 공식 프리팹 이름 끝자리는 노트 속도입니다(091002_road_nor_2, 000302_road_nor_2).
        int speed = UidCode.Speed(uid);
        if (speed >= 2) return speed.ToString(System.Globalization.CultureInfo.InvariantCulture);

        if (string.Equals(uid, "000301", System.StringComparison.OrdinalIgnoreCase)
            || string.Equals(uid, "000304", System.StringComparison.OrdinalIgnoreCase))
        {
            return "1";
        }

        if (noteType == NoteTypes.Blue)
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
        return !string.IsNullOrEmpty(bossAction) && noteType == NoteTypes.Boss && !IsBossProjectileUid(uid);
    }

    public static bool IsBossProjectileUid(string uid)
    {
        if (string.IsNullOrEmpty(uid) || uid.Length < 6) return false;

        string xx = UidCode.Xx(uid);
        string yy = UidCode.Yy(uid);
        bool projectileSeries = IsBossProjectileSeries(xx);
        // 지상·공중 기본 레인의 모든 속도(01~06). 속도 2·3 발사체도 발사체로 다룹니다.
        int motionGroup = UidCode.MotionGroup(uid);
        bool normalLane = motionGroup == 0 || motionGroup == 1;
        return projectileSeries && normalLane;
    }

    public static string ResolvePrefabName(ExperimentNoteSpec spec, int noteType, string uid, int pathway)
    {
        if (!string.IsNullOrEmpty(spec.PrefabName)) return spec.PrefabName;
        if (ShouldUseEmptyBossActionPrefab(noteType, uid, spec.BossAction)) return "empty_000";
        if (!string.IsNullOrEmpty(uid)) return BuildPrefabName(uid, noteType, pathway);
        return null; // keep cloned value
    }

    public static string ResolveKeyAudio(ExperimentNoteSpec spec, int noteType, string uid)
    {
        if (!string.IsNullOrEmpty(spec.KeyAudio)) return spec.KeyAudio;

        // 명시적으로 override하지 않는 한, 원본 noteData.key_audio를 그대로 유지한다.
        // 이렇게 해야 실험 노트도 원래 키음 세팅을 유지할 수 있다.
        return null; // keep cloned value
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

    private static readonly System.Collections.Generic.HashSet<string> LoggedOfficialNoteDiffs =
        new System.Collections.Generic.HashSet<string>();

    public static bool IsItemNoteType(int noteType)
        => noteType == NoteTypes.Blue || noteType == NoteTypes.Heart;

    /// <summary>
    /// 게임 공식 노트 표(NoteDataMananger)에서 uid 행을 찾아 복제합니다. 못 찾으면 false.
    /// uid별 첫 1회는 복제 원본(일반 노트)과 다른 판정 필드를 남깁니다.
    /// </summary>
    public static bool TryCloneOfficialNoteConfig(string uid, NoteConfigData sourceNoteData, out NoteConfigData official)
    {
        official = null;
        if (string.IsNullOrEmpty(uid)) return false;

        try
        {
            var manager = Il2CppAssets.Scripts.GameCore.Managers.NoteDataMananger.instance;
            var row = manager != null ? manager.GetNoteByUid(uid) : null;
            if (row == null || !string.Equals(row.uid, uid, System.StringComparison.OrdinalIgnoreCase)) return false;

            official = CloneNoteConfigData(row);
            if (LoggedOfficialNoteDiffs.Add(uid))
            {
                ModLogger.Msg($"[ExperimentChart] 아이템 노트 uid={uid}를 공식 노트 표 기준으로 생성합니다. " +
                              $"복제 원본과 다른 필드: {DescribeNoteConfigDiff(sourceNoteData, row)}");
            }
            return official != null;
        }
        catch (System.Exception ex)
        {
            ModLogger.Warning($"[ExperimentChart] 공식 노트 표 조회 실패(uid={uid}), 복제 원본을 그대로 씁니다: {ex.Message}");
            return false;
        }
    }

    private static string DescribeNoteConfigDiff(NoteConfigData a, NoteConfigData b)
    {
        if (a == null || b == null) return "(비교 불가)";

        var diffs = new System.Collections.Generic.List<string>();
        void Check(string name, object x, object y)
        {
            string sx = x?.ToString() ?? "null";
            string sy = y?.ToString() ?? "null";
            if (sx != sy) diffs.Add($"{name} {sx}->{sy}");
        }

        Check("type", a.type, b.type);
        Check("des", a.des, b.des);
        Check("effect", a.effect, b.effect);
        Check("left_perfect_range", a.left_perfect_range, b.left_perfect_range);
        Check("left_great_range", a.left_great_range, b.left_great_range);
        Check("right_perfect_range", a.right_perfect_range, b.right_perfect_range);
        Check("right_great_range", a.right_great_range, b.right_great_range);
        Check("damage", a.damage, b.damage);
        Check("score", a.score, b.score);
        Check("fever", a.fever, b.fever);
        Check("missCombo", a.missCombo, b.missCombo);
        Check("addCombo", a.addCombo, b.addCombo);
        Check("jumpNote", a.jumpNote, b.jumpNote);
        Check("isShowPlayEffect", a.isShowPlayEffect, b.isShowPlayEffect);
        return diffs.Count > 0 ? string.Join(", ", diffs) : "(없음)";
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

    public static bool IsSceneToggleNote(MusicData note)
    {
        if (note?.noteData == null) return false;

        string uid = note.noteData.uid ?? "";
        return note.noteData.type == NoteTypes.SceneToggle || uid.StartsWith("0004");
    }

    // 게임 노트 수치의 두 가지 반올림 정밀도 도메인:
    //  - TimingDecimals(3자리): 판정 타이밍 계열(tick, dt). 판정 일관성을 위해 더 촘촘하게.
    //  - ChartDecimals (2자리): 차트 배치 계열(config time, length, showTick).
    // showTick = tick - dt 로 3자리 값에서 파생되지만, 배치값이므로 2자리로 맞춥니다.
    private const int TimingDecimals = 3;
    private const int ChartDecimals = 2;

    public static double NormalizeTimingValue(double value)
    {
        return System.Math.Round(value, TimingDecimals, System.MidpointRounding.AwayFromZero);
    }

    public static double NormalizeChartValue(double value)
    {
        return System.Math.Round(value, ChartDecimals, System.MidpointRounding.AwayFromZero);
    }

    public static double GetEffectiveDt(MusicData note)
    {
        return GetEffectiveDt(note, null);
    }

    public static double GetEffectiveDt(MusicData note, ExperimentNoteSpec spec)
    {
        string uid = !string.IsNullOrEmpty(spec?.Uid) ? spec.Uid : note.noteData?.uid;
        bool isBossEvent = spec != null && !string.IsNullOrEmpty(spec.BossAction)
            && !IsBossProjectileUid(uid) && UidCode.Xx(uid) != "09";

        // 속도 2·3 uid는 그 속도의 애니메이션이 재생되므로 dt도 그에 맞춰야 합니다. WAV 이름의 _dt보다 우선합니다.
        if (!isBossEvent && TryGetSpeedDt(uid, GetSpeed1Dt(uid, spec), out double speedDt, out bool measured))
        {
            LogSpeedDtOnce(uid, speedDt, measured, spec?.Dt ?? -1.0);
            return speedDt;
        }

        if (spec != null && spec.Dt >= 0.0) return spec.Dt;
        return GetSpeed1Dt(uid, spec);
    }

    /// <summary>속도 1 기준의 자동 dt입니다(속도 자리를 보기 전부터 있던 규칙).</summary>
    private static double GetSpeed1Dt(string uid, ExperimentNoteSpec spec)
    {
        if (string.IsNullOrEmpty(uid) || uid.Length < 6) return BaseDt;

        if (IsBossProjectileUid(uid)) return BossProjectileDt;

        if (spec != null && !string.IsNullOrEmpty(spec.BossAction))
        {
            return 0.0;
        }

        string xx = UidCode.Xx(uid);
        if (xx == "15") return 0.8;
        if (xx == "16") return 1.25;
        // Up·Down 묶음(속도 1이면 yy 07/10/13/16)
        if (UidCode.MotionGroup(uid) >= 2) return 1.0;

        return BaseDt;
    }
}
