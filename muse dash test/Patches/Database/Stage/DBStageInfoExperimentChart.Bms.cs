using MelonLoader;

// BMS 차트(BmsChart) → 실험 노트 스펙(ExperimentNoteSpec) 변환 및 보스 등장/퇴장 자동 스왑 로직.
public partial class DBStageInfo_SetRuntimeMusicData_Patch
{
    public static System.Collections.Generic.List<ExperimentNoteSpec> BuildBmsExperimentNotes(muse_dash_test.BmsChart chart, string activeUid)
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

            var spec = CreateExperimentNoteSpecFromBms(pair.StartNote, wavInfo, activeUid);
            spec.Label = $"BMS {pair.Type} {pair.StartNote.RawValue}";
            spec.StartTick = NormalizeChartValue(pair.StartNote.Time);
            spec.Length = NormalizeChartValue(System.Math.Max(0.0, pair.Duration));
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

            if (wavInfo.NoteType == NoteTypes.Long || wavInfo.NoteType == NoteTypes.Sandbag)
            {
                MelonLogger.Warning($"[ExperimentChart.Bms] 특수 노트가 짝 없이 남아 단일 주입을 건너뜁니다: raw={note.RawValue}, tick={note.Tick}, time={note.Time:0.###}, wav={wavInfo.RawWavName}");
                continue;
            }

            specs.Add(CreateExperimentNoteSpecFromBms(note, wavInfo, activeUid));
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

    public static void ApplySceneChangeNamesToBmsSpecs(System.Collections.Generic.List<ExperimentNoteSpec> specs)
    {
        if (specs == null || specs.Count == 0)
        {
            return;
        }

        string activeSceneChangeName = null;
        int applied = 0;
        for (int i = 0; i < specs.Count; i++)
        {
            var spec = specs[i];
            if (spec == null)
            {
                continue;
            }

            if (spec.NoteType == NoteTypes.SceneToggle)
            {
                spec.SceneChangeNames = null;
                if (!string.IsNullOrWhiteSpace(spec.IbmsId))
                {
                    activeSceneChangeName = spec.IbmsId;
                    MelonLogger.Msg($"[ExperimentChart.Bms.SceneChangeNames] 활성 씬 체인지 키 갱신: uid={spec.Uid}, ibms_id={activeSceneChangeName}, tick={spec.StartTick}");
                }
                continue;
            }

            if (!string.IsNullOrWhiteSpace(activeSceneChangeName))
            {
                spec.SceneChangeNames = new System.Collections.Generic.List<string> { activeSceneChangeName };
                applied++;
            }
        }

        MelonLogger.Msg($"[ExperimentChart.Bms.SceneChangeNames] 적용 완료: applied={applied}, active={activeSceneChangeName ?? "(none)"}");
    }

    public static ExperimentNoteSpec CreateExperimentNoteSpecFromBms(muse_dash_test.BmsNote note, muse_dash_test.BmsWavInfo wavInfo, string activeUid)
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
            StartTick = wavInfo.NoteType == NoteTypes.Boss && !string.IsNullOrWhiteSpace(wavInfo.BossAction)
                ? note.Time
                : NormalizeChartValue(note.Time),
            Dt = wavInfo.Dt >= 0.0 ? NormalizeTimingValue(wavInfo.Dt) : wavInfo.Dt
        };

        if (!string.IsNullOrEmpty(spec.BossAction) && !string.IsNullOrEmpty(spec.Uid) && spec.Uid.Length >= 2)
        {
            spec.Scene = "scene_" + UidCode.Scene(spec.Uid);
        }

        if (!string.IsNullOrEmpty(spec.BossAction) && muse_dash_test.MainMod.TryGetCachedHwaScene(activeUid, out int manifestScene))
        {
            spec.Scene = $"scene_{manifestScene:00}";
        }

        if (!string.IsNullOrWhiteSpace(spec.BossAction))
        {
            MelonLogger.Msg($"[ExperimentChart.Bms.BossFields] raw={note.RawValue}, uid={spec.Uid}, action={spec.BossAction}, BossName={spec.BossName}, BossScene={spec.BossScene}, Scene={spec.Scene}");
        }

        // 씬 전환 노트(type 9): SceneChangeController.ChangeScene가 호출되려면 ibms_id가 게임의
        // sceneInfo 키와 맞아야 합니다(BMS 경로는 기존에 ibms_id를 채우지 않아 전환이 안 됐습니다).
        if (spec.NoteType == NoteTypes.SceneToggle)
        {
            spec.PrefabName = spec.Uid;   // 보이지 않는 트리거 → 자동 프리팹명 생성 방지(원시 UID 사용)
            spec.KeyAudio = "0";          // 복제 원본의 타격음 상속 방지
            spec.IbmsId = ResolveSceneToggleIbmsId(spec.Uid); // 000401 → "1O"
            // Scene/BossAction은 의도적으로 미설정 → 복제된 원본 노트의 실재 scene을 상속하여
            // 존재하지 않는 scene_00 배경 등록(보라색 화면)을 원천 차단합니다.
            MelonLogger.Msg($"[ExperimentChart.Bms.SceneToggle] uid={spec.Uid}, ibms_id={spec.IbmsId ?? "(none)"}, prefab={spec.PrefabName}, tick={note.Tick}, time={note.Time:0.###}");
        }

        return spec;
    }

    // UID 끝 2자리(yy) = 전환할 sceneInfo 번호 → 게임 내부 ibms_id 매핑.
    // (docs/NOTE_EXPERIMENTS.md의 검증된 표. scene 11은 표에 없어 제외.)
    private static readonly System.Collections.Generic.Dictionary<int, string> SceneToggleIbmsIdByScene =
        new System.Collections.Generic.Dictionary<int, string>
    {
        { 1, "1O" }, { 2, "1P" }, { 3, "1Q" }, { 4, "1R" }, { 5, "1S" }, { 6, "1T" },
        { 7, "1U" }, { 8, "1V" }, { 9, "1W" }, { 10, "1X" }, { 12, "1Y" },
    };

    // 씬 전환 노트 UID(000401 등) → ibms_id("1O" 등) 해석. 미지원 번호면 경고 후 null.
    public static string ResolveSceneToggleIbmsId(string uid)
    {
        string yy = UidCode.Yy(uid);
        if (yy != null && int.TryParse(yy, out int sceneInfo)
            && SceneToggleIbmsIdByScene.TryGetValue(sceneInfo, out string ibmsId))
        {
            return ibmsId;
        }

        MelonLogger.Warning($"[ExperimentChart.Bms] 씬 전환 ibms_id 매핑을 찾지 못했습니다: uid={uid}, yy={yy}. 씬 전환이 동작하지 않을 수 있습니다.");
        return null;
    }

    public static bool ShouldKeepBmsPrefabName(muse_dash_test.BmsWavInfo wavInfo)
    {
        if (wavInfo == null)
        {
            return false;
        }

        if (!string.IsNullOrWhiteSpace(wavInfo.BossAction) && wavInfo.NoteType == NoteTypes.Boss)
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

        if (!string.IsNullOrWhiteSpace(wavInfo?.Uid) && wavInfo.Uid.Length >= 6)
        {
            string yy = UidCode.Yy(wavInfo.Uid);
            if (yy == "04" || yy == "10" || yy == "16")
            {
                return 1;
            }
        }

        return 0;
    }

    /// <summary>
    /// 보스 등장/퇴장 자동 스왑 패스. 스펙 목록을 순서대로 훑으며 "out 이후 처음 나오는 in"을
    /// 한 쌍으로 보고, 그 in 노트를 실제 보스 교체 이벤트(swap 액션 + empty_000 placeholder)로 변형합니다.
    /// <para>상태기계: <c>waitingForSwapIn</c>(out을 만나 in을 기다리는 중) + <c>lastOutTick</c>(짝이 될 in은
    /// 반드시 이 tick 이후여야 함). out을 만나면 대기 ON, 유효한 in을 처리하면 대기 OFF로 1:1 소비됩니다.</para>
    /// <para><b>실행 순서 계약</b>: 이 패스는 입력 스펙이 tick 순으로 정렬된 상태에서, 노트가 게임 리스트에
    /// 삽입(AddExperimentNotes)되기 <i>전에</i> 돌아야 합니다. out→in 판정이 순서에 의존하기 때문입니다.</para>
    /// 입력은 변경하지 않고 복제본(CloneExperimentNoteSpec) 위에서만 작업합니다.
    /// </summary>
    public static System.Collections.Generic.List<ExperimentNoteSpec> BuildRuntimeExperimentNotes(System.Collections.Generic.IReadOnlyList<ExperimentNoteSpec> specs)
    {
        var runtimeSpecs = new System.Collections.Generic.List<ExperimentNoteSpec>();
        if (specs == null || specs.Count == 0)
        {
            return runtimeSpecs;
        }

        // out을 만나 그 짝이 될 in을 기다리는 중인가, 그리고 그 out의 tick은 언제였나.
        bool waitingForSwapIn = false;
        double lastOutTick = 0.0;

        for (int i = 0; i < specs.Count; i++)
        {
            var spec = CloneExperimentNoteSpec(specs[i]);
            if (spec == null)
            {
                continue;
            }

            // 보스 퇴장(out): 대기 상태 ON, 이후 등장(in)의 하한 tick 기록.
            if (IsBossOutAction(spec))
            {
                waitingForSwapIn = true;
                lastOutTick = spec.StartTick;
                runtimeSpecs.Add(spec);
                continue;
            }

            // out 이후 처음 나오는, out보다 뒤(tick)인 in을 실제 보스 교체로 변형.
            if (waitingForSwapIn && IsBossInAction(spec) && spec.StartTick > lastOutTick)
            {
                string swapAction = BuildBossSwapAction(spec);
                if (!string.IsNullOrWhiteSpace(swapAction))
                {
                    MelonLogger.Msg($"[BossAutoSwap] out 이후 in 감지: label={spec.Label}, tick={spec.StartTick}, action={swapAction}");
                    spec.BossAction = swapAction;
                    spec.NoteType = NoteTypes.Boss;
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
            SceneChangeNames = source.SceneChangeNames != null
                ? new System.Collections.Generic.List<string>(source.SceneChangeNames)
                : null,
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

        if (!int.TryParse(UidCode.Scene(uid), out int scene))
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

        if (int.TryParse(UidCode.Scene(uid), out int scene))
        {
            return scene;
        }

        return -1;
    }
}
