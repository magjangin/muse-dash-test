using System;

namespace muse_dash_test
{
    /// <summary>
    /// 진단 로깅이나 방어적 프로퍼티 조회 중 발생하는 예외를 안전하게 처리합니다.
    /// context를 넘기면 Verbose 로그가 켜졌을 때 예외 원인을 남깁니다.
    /// </summary>
    internal static class SafeAccess
    {
        public static T Get<T>(Func<T> getter, T fallback = default, string context = null)
        {
            if (getter == null) return fallback;
            try
            {
                return getter();
            }
            catch (Exception ex)
            {
                if (!string.IsNullOrEmpty(context))
                {
                    ModLogger.Verbose($"[SafeAccess] {context}: {ex.GetType().Name}: {ex.Message}");
                }
                return fallback;
            }
        }
    }
}
