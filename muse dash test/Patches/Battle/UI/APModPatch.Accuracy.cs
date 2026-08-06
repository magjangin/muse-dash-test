using System;

namespace muse_dash_test.Patches
{
    public static class AccuracyCalculator
    {
        public static float CalculateTrueAccuracy(Il2CppAssets.Scripts.GameCore.HostComponent.TaskStageTarget instance)
        {
            // 게임 업데이트로 필드명이 바뀌어도 예외 대신 0 fallback으로 안전하게 degrade되도록 리플렉션 경유로 읽습니다.
            int perfect = ModReflection.GetInt(instance, "PerfectResult");
            int great = ModReflection.GetInt(instance, "GreatResult");
            int miss = ModReflection.GetInt(instance, "MissResult");

            int totalStandard = CustomPlaySession.Current.TotalStandard;
            if (totalStandard > 0)
            {
                float numerator = perfect + great * 0.5f;
                return Math.Min(1.0f, numerator / totalStandard);
            }
            else
            {
                float total = perfect + great + miss;
                if (total > 0f)
                {
                    return (perfect + great * 0.5f) / total;
                }
                return 1.0f;
            }
        }

        public static float CalculateTrueAccuracyNew(Il2CppAssets.Scripts.GameCore.HostComponent.TaskStageTarget instance)
        {
            int perfect = ModReflection.GetInt(instance, "PerfectResult");
            int great = ModReflection.GetInt(instance, "GreatResult");
            int miss = ModReflection.GetInt(instance, "MissResult");
            int jumpOver = ModReflection.GetInt(instance, "JumpOverResult");
            int energy = ModReflection.GetInt(instance, "EnergyCount");
            int bluePoint = ModReflection.GetInt(instance, "BluePoint");

            int totalStandard = CustomPlaySession.Current.TotalStandard;
            int totalGears = CustomPlaySession.Current.TotalGears;
            int totalHearts = CustomPlaySession.Current.TotalHearts;
            int totalBlueNotes = CustomPlaySession.Current.TotalBlueNotes;

            int denominator = totalStandard + totalGears + totalHearts + totalBlueNotes;
            if (denominator > 0)
            {
                float numerator = perfect + (great * 0.5f) + jumpOver + energy + bluePoint;
                return Math.Min(1.0f, numerator / denominator);
            }
            else
            {
                float total = perfect + great + miss;
                if (total > 0f)
                {
                    return (perfect + great * 0.5f) / total;
                }
                return 1.0f;
            }
        }
    }
}
