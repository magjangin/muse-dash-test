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

        return spec;
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
