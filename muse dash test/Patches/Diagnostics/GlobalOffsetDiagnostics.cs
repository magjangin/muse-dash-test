using Il2CppAssets.Scripts.Database;

namespace muse_dash_test
{
    /// <summary>
    /// 전역 오프셋 설정값(<c>DataHelper.offset</c>, 단위 ms)을 진단용으로 남깁니다.
    ///
    /// <para>예전에는 이 값을 getter에 Harmony 훅을 걸어 읽었습니다. 아무것도 주입하지 않으면서
    /// 게임이 그 프로퍼티를 읽을 때마다 훅이 하나 끼어들었고, 플레이 중에도 그대로 돌았습니다.
    /// 정작 필요한 정보는 "지금 전역 오프셋이 몇 ms인가" 하나뿐이라 씬이 바뀔 때 한 번 읽으면
    /// 충분합니다. 그래서 훅을 걷어내고 이 진단만 남겼습니다.</para>
    ///
    /// <para>로그 수준이 Verbose일 때만 값을 읽습니다(기본값은 꺼짐 — config의 <c>LogLevel</c> 또는
    /// <c>EnableVerboseLog</c>). 값이 그대로면 다시 남기지 않아 씬을 오갈 때 같은 줄이 쌓이지 않습니다.</para>
    /// </summary>
    public static class GlobalOffsetDiagnostics
    {
        private const int Unknown = int.MinValue;

        private static int lastLoggedOffsetMs = Unknown;

        /// <summary>
        /// 전역 오프셋을 읽어 직전에 남긴 값과 다르면 로그로 남깁니다.
        /// 예외는 부르는 쪽(<c>FeatureGuard</c>)이 격리합니다.
        /// </summary>
        public static void LogGlobalOffsetIfChanged()
        {
            if (!ModLogger.IsLevelEnabled(ModLogLevel.Verbose))
            {
                return;
            }

            int offsetMs = DataHelper.offset;
            if (offsetMs == lastLoggedOffsetMs)
            {
                return;
            }

            lastLoggedOffsetMs = offsetMs;
            ModLogger.Msg($"[OffsetInject] 전역 오프셋 설정값(DataHelper.offset): {offsetMs}ms");
        }
    }
}
