using MelonLoader;

namespace muse_dash_test
{
    /// <summary>
    /// FixLocksPatch(PnlStagePatch)가 화면 표시용으로 강제 true 처리하는 IsUnlockAllMaster의
    /// 원래 값을 세션당 한 번만 캡처해두고, 세이브 시 SaveDataManagerPatch가 이 값으로 원복할 수 있게 합니다.
    /// 강제 true 값이 그대로 디스크에 영구 기록되는 걸 막기 위한 목적입니다.
    /// </summary>
    public static class UnlockAllMasterGuard
    {
        private static bool s_captured;
        private static bool s_originalValue;

        public static void CaptureOnce(bool currentValue)
        {
            if (s_captured) return;
            s_captured = true;
            s_originalValue = currentValue;
            MelonLogger.Msg($"[UnlockAllMasterGuard] 원본 IsUnlockAllMaster 값 캡처: {s_originalValue}");
        }

        public static bool GetRestoreValue()
        {
            return s_captured ? s_originalValue : false;
        }
    }
}
