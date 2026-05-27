using MelonLoader;
using Il2CppAssets.Scripts.Database;
using Il2CppGameLogic;
using System.Reflection;

// DBStageInfo.SetRuntimeMusicData 하모니 패치
[HarmonyLib.HarmonyPatch(typeof(DBStageInfo), "SetRuntimeMusicData")]
public partial class DBStageInfo_SetRuntimeMusicData_Patch
{
    private const double BaseDt = 1.47;

    private const double BossProjectileDt = 0.7;

    private const double LongMiddleStep = 0.1;

    private const int SourceNoteIndex = 1;

    private static readonly bool DebugExperimentNotes = true;

    // 여기만 수정하면 됩니다. 자세한 규칙은 docs/NOTE_EXPERIMENTS.md를 참고하세요.
    // NoteType: 1=일반, 2=톱니, 3=롱, 6=하트, 7=음표, 8=샌드백
    // Pathway: 0=지상, 1=공중
    // Speed, Dt는 -1이면 자동/원본 로직을 사용합니다.
    private static readonly ExperimentNoteSpec[] ExperimentNotes =
    {
        // 이 배열만 수정하면 됩니다.
        // 줄을 복사/삭제하면서 원하는 노트만 남기세요.
        new ExperimentNoteSpec { Label = "보스1 등장", Uid = "050101", NoteType = 0, Pathway = 0, StartTick = 15.0, BossAction = "in" },
        new ExperimentNoteSpec { Label = "보스1 공격", Uid = "050107", NoteType = 0, Pathway = 0, StartTick = 17.5, BossAction = "boss_far_atk_1_start" },
        new ExperimentNoteSpec { Label = "보스1 퇴장", Uid = "050102", NoteType = 0, Pathway = 0, StartTick = 22.0, BossAction = "out" },
        new ExperimentNoteSpec { Label = "보스2 교체", Uid = "050101", NoteType = 0, Pathway = 0, StartTick = 24.0, BossAction = "swap:0401_boss:4" },
        new ExperimentNoteSpec { Label = "보스2 공격", Uid = "050107", NoteType = 0, Pathway = 0, StartTick = 26.5, BossAction = "boss_far_atk_1_start" },
        new ExperimentNoteSpec { Label = "보스2 퇴장", Uid = "050108", NoteType = 0, Pathway = 0, StartTick = 31.0, BossAction = "boss_far_atk_1_end" },


        // 원하는 실험은 아래 예시를 복사해서 주석을 해제하세요.
        // new ExperimentNoteSpec { Label = "지상 일반 8개", Uid = "051001", NoteType = 1, Pathway = 0, StartTick = 15.0, Count = 8, Interval = 0.25 },
        // new ExperimentNoteSpec { Label = "공중 일반 4개", Uid = "051004", NoteType = 1, Pathway = 1, StartTick = 17.5, Count = 4, Interval = 0.5 },
        // new ExperimentNoteSpec { Label = "속도/dt 직접 지정", Uid = "051001", NoteType = 1, Pathway = 0, StartTick = 15.0, Speed = 12.0, Dt = 0.75 },
        // new ExperimentNoteSpec { Label = "롱노트 1개", Uid = "050201", NoteType = 3, Pathway = 0, IsLong = true, StartTick = 20.0, Length = 1.6 },
        // new ExperimentNoteSpec { Label = "샌드백 1개", Uid = "020401", NoteType = 8, Pathway = 0, IsMul = true, StartTick = 24.0, Length = 1.2 },
        // new ExperimentNoteSpec { Label = "하트 1개", Uid = "000201", NoteType = 6, Pathway = 0, KeyAudio = "sfx_hp", StartTick = 28.0 },
        // new ExperimentNoteSpec { Label = "음표 1개", Uid = "000301", NoteType = 7, Pathway = 0, KeyAudio = "sfx_score", StartTick = 30.0 },

        // 보스 액션 트리거(type 0)는 empty_000, 보스 발사체(type 1)는 일반 노트 프리팹을 씁니다.
        // 보스 발사체만 만들 때는 BossAction을 비우면 됩니다.
        // 070601 => boss_far_atk_1_R, 070604 => boss_far_atk_1_L, 070701 => boss_far_atk_2, Dt=0.7 권장.
        // new ExperimentNoteSpec { Label = "보스 등장", Uid = "050101", NoteType = 0, Pathway = 0, BossAction = "in", StartTick = 15.0 },
        // new ExperimentNoteSpec { Label = "보스 원거리1 시작", Uid = "050107", NoteType = 0, Pathway = 0, BossAction = "boss_far_atk_1_start", StartTick = 16.0 },
        // new ExperimentNoteSpec { Label = "보스 원거리1 후속(종료)", Uid = "050108", NoteType = 0, Pathway = 0, BossAction = "boss_far_atk_1_end", StartTick = 17.0 },
        // new ExperimentNoteSpec { Label = "보스 원거리2 시작", Uid = "050109", NoteType = 0, Pathway = 0, BossAction = "boss_far_atk_2_start", StartTick = 18.0 },
        // new ExperimentNoteSpec { Label = "보스 원거리2 후속(종료)", Uid = "050110", NoteType = 0, Pathway = 0, BossAction = "boss_far_atk_2_end", StartTick = 19.0 },
        // new ExperimentNoteSpec { Label = "보스 퇴장", Uid = "050102", NoteType = 0, Pathway = 0, BossAction = "out", StartTick = 22.0 },
    };

    public class ExperimentNoteSpec
    {
        public string Label = "note";
        public string Uid = "";
        public string Des = "";
        public string PrefabName = "";
        public string KeyAudio = "";
        public string BossAction = "";
        public string Scene = "";
        public string IbmsId = "";
        public int NoteType = -1;
        public int Pathway = -1;
        public bool IsLong = false;
        public bool IsMul = false;
        public double StartTick = 15.0;
        public int Count = 1;
        public double Interval = 1.0;
        public double Length = 0.0;
        public double Speed = -1.0;
        public double Dt = -1.0;
    }

    public static void Postfix(DBStageInfo __instance)
    {
        try
        {
            if (!ExperimentPlayContext.ShouldApplyExperimentChart)
            {
                MelonLogger.Msg("[ExperimentChart] 적용 건너뜀: 실험 모드 선택이 아님");
                return;
            }

            ApplyExperimentChart(__instance);
        }
        catch (System.Exception ex)
        {
            MelonLogger.Error($"실험 차트 적용 중 예외 발생: {ex}");
        }

        // 삽입 후 덤프 헬퍼 메서드 호출
        //MelonLogger.Msg("노트 삽입 후 덤프:");
        //DumpMusicList(__instance);
    }
}

public static class ExperimentPlayContext
{
    public static bool ShouldApplyExperimentChart { get; private set; }

    public static void RememberMusicSelection(string uid)
    {
        bool isExperimentTag = MusicButtonAreaTitle_RefreshTxt_Patch.IsExperimentModActive;
        bool isCustomUid = uid == "0-0" || uid == "999-0";
        bool isCustomAlbum = PnlStagePatchHelper.IsCustomAlbumContext(998, "0-0");
        ShouldApplyExperimentChart = isExperimentTag && isCustomUid && isCustomAlbum;

        MelonLogger.Msg($"[ExperimentChart] selection uid={uid ?? "(null)"}, isExperimentTag={isExperimentTag}, isCustomAlbum={isCustomAlbum}, apply={ShouldApplyExperimentChart}");
    }
}
