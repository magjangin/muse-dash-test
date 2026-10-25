using System;
using System.Globalization;

namespace muse_dash_test
{
    /// <summary>노트 dt 권장표가 묶는 노트 계열입니다.</summary>
    public enum DtFamily
    {
        /// <summary>권장표에 없는 계열(샌드백·보스 톱니·속도 자리가 없는 uid 등). WAV <c>_dt</c>를 그대로 둡니다.</summary>
        None,
        /// <summary>일반 / 트랩 / 음표 / Up·Down (하트·고스트 포함).</summary>
        Normal,
        /// <summary>홀드(롱노트). 막대 길이를 속도별 고정 시간으로 그리기 때문에 일반과 값이 다릅니다.</summary>
        Hold,
        /// <summary>보스 발사체(xx=06/07/08, 기본 레인).</summary>
        BossProjectile,
        /// <summary>씬 전환(0004yy)·보스 액션처럼 속도가 없는 빈 트리거. dt=0.</summary>
        FixedEvent
    }

    /// <summary>
    /// 노트 계열·속도별 dt 권장표(2026-10-25 실기 확정)와 차터 허용 범위입니다.
    /// <para>모드는 기본적으로 WAV <c>_dt</c>를 그대로 씁니다. <c>hwa info.txt</c>의
    /// 'dt를 권장 표로 고치기 = 활성화'일 때만 이 표로 덮어씁니다(docs/guides/DT_RECOMMENDED_TABLE.md).</para>
    /// </summary>
    public static class BmsDtTable
    {
        // [속도 1, 속도 2, 속도 3]
        private static readonly double[] NormalDt = { 1.48, 1.28, 1.18 };
        private static readonly double[] HoldDt = { 1.47, 1.26, 1.10 };
        private static readonly double[] BossProjectileDt = { 0.8, 0.76, 0.6 };

        // 일반 계열에서 차터가 권장표를 벗어나 고를 수 있는 범위(사용자 지정, 2026-10-25).
        // 속도 1은 범위 없이 1.48 고정입니다.
        private static readonly double[] NormalMin = { 1.48, 1.18, 0.98 };
        private static readonly double[] NormalMax = { 1.48, 1.38, 1.28 };

        // WAV 이름의 dt는 소수 3자리로 반올림돼 들어옵니다.
        private const double Epsilon = 0.0005;

        public static DtFamily Classify(string uid, int noteType, string bossAction)
        {
            if (uid != null && uid.StartsWith("0004", StringComparison.Ordinal)) return DtFamily.FixedEvent;
            if (noteType == 9) return DtFamily.FixedEvent;

            string xx = uid != null && uid.Length >= 4 ? uid.Substring(2, 2) : null;
            int motionGroup = MotionGroup(uid);
            bool projectile = (xx == "06" || xx == "07" || xx == "08") && (motionGroup == 0 || motionGroup == 1);
            if (projectile) return DtFamily.BossProjectile;

            // 보스 등장·퇴장·원거리 시작/끝(xx=01, boss_in/out/swap)은 dt=0인 빈 트리거입니다.
            if (noteType == 0 && !string.IsNullOrWhiteSpace(bossAction)) return DtFamily.FixedEvent;
            if (xx == "01") return DtFamily.FixedEvent;

            if (Speed(uid) == 0) return DtFamily.None;

            switch (noteType)
            {
                case 3: return DtFamily.Hold;
                case 8: return DtFamily.None; // 샌드백: 실측값 없음
            }

            // 보스 톱니(xx=09)는 공식 곡 dt가 발사체와 같아 보이지만(0.667, 속도 2) 실기 확정값이 없습니다.
            if (xx == "09") return DtFamily.None;

            return DtFamily.Normal;
        }

        /// <summary>uid <c>yy</c>의 속도 1·2·3. 속도 자리가 없는 uid(0004yy, xx=01, 범위 밖)는 0.</summary>
        public static int Speed(string uid)
            => TryParseYy(uid, out int yy) ? (yy - 1) % 3 + 1 : 0;

        private static int MotionGroup(string uid)
            => TryParseYy(uid, out int yy) ? (yy - 1) / 3 : -1;

        private static bool TryParseYy(string uid, out int yy)
        {
            yy = 0;
            if (uid == null || uid.Length < 6) return false;
            if (uid.StartsWith("0004", StringComparison.Ordinal) || uid.Substring(2, 2) == "01") return false;
            return int.TryParse(uid.Substring(4, 2), NumberStyles.Integer, CultureInfo.InvariantCulture, out yy) && yy >= 1 && yy <= 18;
        }

        /// <summary>권장 dt. 권장표에 없는 계열이면 false.</summary>
        public static bool TryGetRecommended(DtFamily family, int speed, out double dt)
        {
            dt = -1.0;
            if (family == DtFamily.FixedEvent)
            {
                dt = 0.0;
                return true;
            }

            double[] table = TableFor(family);
            if (table == null || speed < 1 || speed > 3) return false;

            dt = table[speed - 1];
            return true;
        }

        /// <summary>
        /// 차터가 적은 dt가 허용되는지 봅니다. 일반 계열은 범위(속도 1 = 1.48, 속도 2 = 1.18~1.38,
        /// 속도 3 = 0.98~1.28), 홀드·보스 발사체·고정 이벤트는 권장값 그대로만 허용합니다.
        /// 권장표에 없는 계열이나 dt를 적지 않은 노트(음수)는 판정하지 않고 true입니다.
        /// </summary>
        public static bool IsAllowed(DtFamily family, int speed, double dt, out string expected)
        {
            expected = null;
            if (dt < 0.0) return true;
            if (!TryGetRecommended(family, speed, out double recommended)) return true;

            if (family == DtFamily.Normal)
            {
                double min = NormalMin[speed - 1];
                double max = NormalMax[speed - 1];
                expected = min == max ? Format(min) : Format(min) + "~" + Format(max);
                return dt >= min - Epsilon && dt <= max + Epsilon;
            }

            expected = Format(recommended);
            return Math.Abs(dt - recommended) <= Epsilon;
        }

        public static string Describe(DtFamily family)
        {
            switch (family)
            {
                case DtFamily.Normal: return "일반";
                case DtFamily.Hold: return "홀드";
                case DtFamily.BossProjectile: return "보스 발사체";
                case DtFamily.FixedEvent: return "고정 이벤트";
                default: return "권장표 밖";
            }
        }

        private static double[] TableFor(DtFamily family)
        {
            switch (family)
            {
                case DtFamily.Normal: return NormalDt;
                case DtFamily.Hold: return HoldDt;
                case DtFamily.BossProjectile: return BossProjectileDt;
                default: return null;
            }
        }

        private static string Format(double value) => value.ToString("0.###", CultureInfo.InvariantCulture);
    }
}
