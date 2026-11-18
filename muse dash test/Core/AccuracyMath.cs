using System;

namespace muse_dash_test
{
    /// <summary>
    /// 커스텀 곡의 정확도(TrueAccuracy / TrueAccuracyNew)를 계산하는 순수 수식 헬퍼입니다.
    /// 게임 객체(<c>TaskStageTarget</c>)와 분리해 로직 테스트에서 직접 검증할 수 있도록 둡니다.
    /// </summary>
    public static class AccuracyMath
    {
        public static float ComputeTrueAccuracy(int perfect, int great, int miss, int totalStandard)
        {
            if (totalStandard > 0)
            {
                float numerator = perfect + great * 0.5f;
                return Math.Min(1.0f, numerator / totalStandard);
            }

            float total = perfect + great + miss;
            if (total > 0f)
            {
                return (perfect + great * 0.5f) / total;
            }
            return 1.0f;
        }

        public static float ComputeTrueAccuracyNew(
            int perfect, int great, int miss,
            int jumpOver, int energy, int musicCount,
            int totalStandard, int totalGears, int totalHearts, int totalBlueNotes)
        {
            int denominator = totalStandard + totalGears + totalHearts + totalBlueNotes;
            if (denominator > 0)
            {
                float numerator = perfect + (great * 0.5f) + jumpOver + energy + musicCount;
                return Math.Min(1.0f, numerator / denominator);
            }

            float total = perfect + great + miss;
            if (total > 0f)
            {
                return (perfect + great * 0.5f) / total;
            }
            return 1.0f;
        }
    }
}
