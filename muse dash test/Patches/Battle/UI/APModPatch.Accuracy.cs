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
            // 음표(블루노트)는 먹은 개수가 m_MusicCount에 쌓입니다(공식 곡과 같은 경로).
            // 예전엔 m_BluePoint를 더했는데, 커스텀 음표가 PERFECT로 잘못 집계되던 동안엔 분자에 이미 들어가 있어
            // 드러나지 않았습니다. 판정을 바로잡자 neon이 863/888 = 97.2%로 떨어졌습니다(m_MusicCount=25).
            int musicCount = ModReflection.GetInt(instance, "MusicCount");

            int totalStandard = CustomPlaySession.Current.TotalStandard;
            int totalGears = CustomPlaySession.Current.TotalGears;
            int totalHearts = CustomPlaySession.Current.TotalHearts;
            int totalBlueNotes = CustomPlaySession.Current.TotalBlueNotes;

            int denominator = totalStandard + totalGears + totalHearts + totalBlueNotes;
            if (denominator > 0)
            {
                float numerator = perfect + (great * 0.5f) + jumpOver + energy + musicCount;
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
