using MelonLoader;
using System.Collections.Generic;

namespace muse_dash_test
{
    internal static class SceneDiagnosticLogger
    {
        private static readonly Dictionary<string, int> LastLogFrameByKey = new Dictionary<string, int>();

        public static bool EnableLogs { get; set; } = true;

        public static bool ShouldLog(string key, int minIntervalFrames = 1)
        {
            if (!EnableLogs) return false;

            int frame = GetFrameCount();
            if (!LastLogFrameByKey.TryGetValue(key, out int lastFrame) || frame - lastFrame >= minIntervalFrames)
            {
                LastLogFrameByKey[key] = frame;
                return true;
            }

            return false;
        }

        public static void Log(string key, string message, int minIntervalFrames = 1)
        {
            if (!ShouldLog(key, minIntervalFrames)) return;
            if (string.IsNullOrEmpty(message)) return;
            MelonLogger.Msg(message);
        }

        private static int GetFrameCount()
        {
            try { return UnityEngine.Time.frameCount; }
            catch { return 0; }
        }
    }
}
