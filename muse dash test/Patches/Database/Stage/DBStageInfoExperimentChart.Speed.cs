using muse_dash_test;

// 노트 속도(uid "yy"의 속도 자리)에 맞춘 dt.
// 공식 곡은 uid 속도 자리 → 프리팹 이름 끝자리 → Spine 등장 애니메이션 → dt가 한 세트로 움직입니다
// (2026-10-23 OBLIVION 실측, docs/experiments/NOTE_SPEED_DT.md). 예전 커스텀 채보는 속도 1만 썼기에
// dt만 바꾸면 1.467초짜리 애니메이션과 어긋나 렌더가 깨졌습니다.
// 임시 구현입니다: 실측 칸은 실측값, 못 잰 칸은 속도 1 값에 비율을 곱한 추정값입니다.
public partial class DBStageInfo_SetRuntimeMusicData_Patch
{
    // 실측 비율: 일반 1.267/1.467, Up·Down 0.867/1.0, xx=15 0.7/0.8 → 0.86~0.88. 속도 3은 xx=15의 0.6/0.8.
    private const double EstimatedSpeed2Ratio = 0.865;
    private const double EstimatedSpeed3Ratio = 0.75;

    private static readonly System.Collections.Generic.HashSet<string> LoggedSpeedDtUids =
        new System.Collections.Generic.HashSet<string>();

    /// <summary>
    /// uid의 속도 자리가 2·3이면 그 속도의 dt를 돌려줍니다. 속도 1이거나 판단할 수 없으면 false
    /// (기존 dt 로직과 BMS WAV 이름의 _dt 값을 그대로 씁니다).
    /// </summary>
    public static bool TryGetSpeedDt(string uid, double speed1Dt, out double dt, out bool measured)
    {
        dt = 0.0;
        measured = false;

        int speed = UidCode.Speed(uid);
        if (speed < 2) return false;

        string xx = UidCode.Xx(uid);
        int group = UidCode.MotionGroup(uid);
        bool upDown = group >= 2;

        double? table = null;
        if (xx == "15") table = speed == 2 ? 0.7 : 0.6;
        else if (xx == "07" || xx == "08") table = speed == 2 ? 0.667 : (double?)null;
        else if (xx == "09") table = speed == 2 ? 0.667 : (double?)null;
        else if (xx != "16") table = speed == 2 ? (upDown ? 0.867 : 1.267) : (double?)null;

        measured = table.HasValue;
        dt = table ?? speed1Dt * (speed == 2 ? EstimatedSpeed2Ratio : EstimatedSpeed3Ratio);
        return true;
    }

    private static int _forcedSpeedCount;

    /// <summary>
    /// [임시 실험] <c>ForceCustomNoteSpeed</c>가 2·3이면 속도 1 노트의 uid 끝자리를 그 속도로 바꿉니다.
    /// WAV를 고치지 않고 속도별 렌더를 확인하려는 용도입니다. 보스 이벤트, 롱노트(xx=02, 공식 곡 실측 없음),
    /// WAV 이름이 프리팹을 직접 지정한 노트는 건드리지 않습니다.
    /// </summary>
    public static void ApplyForcedNoteSpeed(ExperimentNoteSpec spec)
    {
        int forced = ModConfig.ForceCustomNoteSpeed;
        if (forced != 2 && forced != 3) return;
        if (spec == null || string.IsNullOrEmpty(spec.Uid) || spec.Uid.Length != 6) return;
        if (UidCode.Speed(spec.Uid) != 1) return;
        if (!string.IsNullOrEmpty(spec.BossAction) || spec.NoteType == NoteTypes.Boss) return;
        if (UidCode.Xx(spec.Uid) == "02" || !string.IsNullOrEmpty(spec.PrefabName)) return;

        int yy = int.Parse(UidCode.Yy(spec.Uid)) + (forced - 1);
        string forcedUid = spec.Uid.Substring(0, 4) + yy.ToString("00", System.Globalization.CultureInfo.InvariantCulture);
        if (_forcedSpeedCount++ == 0)
        {
            ModLogger.Msg($"[ExperimentChart.Speed] ForceCustomNoteSpeed={forced}: 속도 1 노트를 속도 {forced}로 바꿔 주입합니다 (예: {spec.Uid} → {forcedUid}).");
        }
        spec.Uid = forcedUid;
    }

    private static void LogSpeedDtOnce(string uid, double dt, bool measured, double wavDt)
    {
        if (!LoggedSpeedDtUids.Add(uid)) return;

        string wavPart = wavDt >= 0.0 ? $", WAV 이름의 dt {wavDt}는 무시" : "";
        ModLogger.Msg($"[ExperimentChart.Speed] uid={uid} 속도 {UidCode.Speed(uid)} → dt={dt:0.###} " +
                      $"({(measured ? "실측" : "추정")}{wavPart}), 프리팹 끝자리 _{UidCode.Speed(uid)}");
    }
}
