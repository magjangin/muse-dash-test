using System;

namespace muse_dash_test.LogicTests
{
    public static class AccuracyMathTests
    {
        public static void ComputeTrueAccuracy_UsesTotalStandardWhenPositive()
        {
            float acc = AccuracyMath.ComputeTrueAccuracy(perfect: 80, great: 10, miss: 10, totalStandard: 100);
            Assert.True(Math.Abs(acc - 0.85f) < 0.0001f, $"Expected 0.85, got {acc}");
        }

        public static void ComputeTrueAccuracy_ClampsToOne()
        {
            float acc = AccuracyMath.ComputeTrueAccuracy(perfect: 120, great: 0, miss: 0, totalStandard: 100);
            Assert.True(Math.Abs(acc - 1.0f) < 0.0001f, $"Expected 1.0, got {acc}");
        }

        public static void ComputeTrueAccuracy_FallsBackToJudgedTotalWhenTotalStandardIsZero()
        {
            float acc = AccuracyMath.ComputeTrueAccuracy(perfect: 3, great: 2, miss: 5, totalStandard: 0);
            Assert.True(Math.Abs(acc - 0.4f) < 0.0001f, $"Expected 0.4, got {acc}");
        }

        public static void ComputeTrueAccuracy_ReturnsOneWhenEverythingIsZero()
        {
            float acc = AccuracyMath.ComputeTrueAccuracy(perfect: 0, great: 0, miss: 0, totalStandard: 0);
            Assert.True(Math.Abs(acc - 1.0f) < 0.0001f, $"Expected 1.0, got {acc}");
        }

        public static void ComputeTrueAccuracyNew_IncludesGearsHeartsAndBlueNotes()
        {
            // (800 + 20*0.5 + 10 + 5 + 25) / (850 + 10 + 5 + 25) = 850 / 890
            float acc = AccuracyMath.ComputeTrueAccuracyNew(
                perfect: 800, great: 20, miss: 30,
                jumpOver: 10, energy: 5, musicCount: 25,
                totalStandard: 850, totalGears: 10, totalHearts: 5, totalBlueNotes: 25);
            float expected = 850f / 890f;
            Assert.True(Math.Abs(acc - expected) < 0.0001f, $"Expected {expected}, got {acc}");
        }

        public static void ComputeTrueAccuracyNew_FallsBackToStandardNotesWhenDenominatorIsZero()
        {
            float acc = AccuracyMath.ComputeTrueAccuracyNew(
                perfect: 6, great: 2, miss: 2,
                jumpOver: 0, energy: 0, musicCount: 0,
                totalStandard: 0, totalGears: 0, totalHearts: 0, totalBlueNotes: 0);
            Assert.True(Math.Abs(acc - 0.7f) < 0.0001f, $"Expected 0.7, got {acc}");
        }
    }
}
