using muse_dash_test;

// 노트 속도(uid "yy"의 속도 자리)에 맞춘 dt.
// 공식 곡은 uid 속도 자리 → 프리팹 이름 끝자리 → Spine 등장 애니메이션 → dt가 한 세트로 움직입니다
// (2026-10-23 OBLIVION 실측, docs/experiments/NOTE_SPEED_DT.md). 예전 커스텀 채보는 속도 1만 썼기에
// dt만 바꾸면 1.467초짜리 애니메이션과 어긋나 렌더가 깨졌습니다.
// 실측 칸은 실측값, 못 잰 칸은 속도 1 값에 비율을 곱한 추정값입니다. 속도 1 노트는 건드리지 않습니다.
// 인게임 확인 기록: docs/experiments/NOTE_SPEED_DT.md 3절.
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
        if (xx == "15")
        {
            table = speed == 2 ? 0.7 : 0.6;
            measured = true;
        }
        else if (IsBossProjectileSeries(xx) || xx == "09")
        {
            // 속도 2는 07·08·09에서 실측(0.667, 20프레임). 06과 속도 3은 추정(WAV 관례 속도 1 = 0.8의 0.75배).
            table = speed == 2 ? 0.667 : 0.6;
            measured = speed == 2 && xx != "06";
        }
        else if (xx == "02")
        {
            // 홀드는 공식 곡 실측이 없어 일반 노트 값을 빌려 씁니다.
            table = speed == 2 ? 1.267 : (double?)null;
        }
        else if (xx != "16")
        {
            table = speed == 2 ? (upDown ? 0.867 : 1.267) : (double?)null;
            measured = table.HasValue;
        }

        dt = table ?? speed1Dt * (speed == 2 ? EstimatedSpeed2Ratio : EstimatedSpeed3Ratio);
        return true;
    }

    public static bool IsBossProjectileSeries(string xx) => xx == "06" || xx == "07" || xx == "08";

    private static void LogSpeedDtOnce(string uid, double dt, bool measured)
    {
        if (!LoggedSpeedDtUids.Add(uid)) return;

        ModLogger.Msg($"[ExperimentChart.Speed] uid={uid} 속도 {UidCode.Speed(uid)}: WAV에 _dt가 없어 표 값 dt={dt:0.###} " +
                      $"({(measured ? "실측" : "추정")}), 프리팹 끝자리 _{UidCode.Speed(uid)}");
    }
}
