using MelonLoader;
using System;

namespace muse_dash_test
{
    /// <summary>
    /// 모드 전반의 로그 출력 수준(Log Level)을 정의합니다.
    /// </summary>
    public enum ModLogLevel
    {
        /// <summary>모든 로그를 끕니다.</summary>
        Silent = 0,
        /// <summary>치명적인 오류 및 예외 로그만 출력합니다.</summary>
        Error = 1,
        /// <summary>경고 및 오류 로그만 출력합니다. (UMPC 기본값: 프레임 렉 최소화)</summary>
        Warning = 2,
        /// <summary>일반 안내 메시지 및 중요 상태 변경 로그를 출력합니다. (일반 PC 기본값)</summary>
        Info = 3,
        /// <summary>디버깅 및 상세 진단 로그까지 모두 출력합니다.</summary>
        Verbose = 4
    }

    /// <summary>
    /// 실행 환경(UMPC vs 데스크톱) 및 설정에 따라 로그 출력을 동적으로 제어하는 로거 클래스입니다.
    /// UMPC에서는 디스크 I/O 및 콘솔 렌더링 병목으로 인한 순간적인 렉(Stuttering)을 방지하기 위해 일반 정보 로그를 억제합니다.
    ///
    /// <para><b>이 모드의 로그는 전부 이 클래스를 거칩니다.</b> <c>MelonLogger</c>를 직접 부르지 마십시오.
    /// 예전에는 <c>MelonLogger.Msg/Warning/Error</c>를 Harmony Prefix로 전역 가로채 음소거했지만,
    /// MelonLoader 0.7.3의 HarmonyX가 순수 관리 메서드를 디투어하는 경로에서
    /// <c>ILHookExtensions.GetCurrentTarget</c> NRE로 터져 <c>PatchAll</c>이 통째로 중단됐습니다
    /// (IL2CPP 대상 패치는 Il2CppInterop의 네이티브 경로를 타므로 멀쩡했고, 관리 메서드 패치만 실패했습니다).
    /// 그래서 가로채기를 걷어내고 호출부에서 레벨을 판정하는 방식으로 되돌렸습니다.</para>
    /// </summary>
    public static class ModLogger
    {
        /// <summary>
        /// 현재 적용된 로그 레벨입니다.
        /// </summary>
        public static ModLogLevel CurrentLogLevel { get; set; } = ModLogLevel.Info;

        /// <summary>
        /// 특정 로그 레벨이 활성화되어 있는지 여부를 확인합니다.
        /// </summary>
        public static bool IsLevelEnabled(ModLogLevel level)
        {
            return CurrentLogLevel >= level;
        }

        /// <summary>
        /// 일반 정보 로그를 출력합니다. (LogLevel이 Info 이상일 때만 출력)
        /// </summary>
        public static void Msg(string msg)
        {
            if (CurrentLogLevel >= ModLogLevel.Info)
            {
                MelonLogger.Msg(msg);
            }
        }

        /// <summary>
        /// 태그가 포함된 일반 정보 로그를 출력합니다. (LogLevel이 Info 이상일 때만 출력)
        /// </summary>
        public static void Msg(string tag, string msg)
        {
            if (CurrentLogLevel >= ModLogLevel.Info)
            {
                MelonLogger.Msg($"[{tag}] {msg}");
            }
        }

        /// <summary>
        /// 경고 로그를 출력합니다. (LogLevel이 Warning 이상일 때만 출력)
        /// </summary>
        public static void Warning(string msg)
        {
            if (CurrentLogLevel >= ModLogLevel.Warning)
            {
                MelonLogger.Warning(msg);
            }
        }

        /// <summary>
        /// 오류 로그를 출력합니다. (LogLevel이 Error 이상일 때만 출력)
        /// </summary>
        public static void Error(string msg)
        {
            if (CurrentLogLevel >= ModLogLevel.Error)
            {
                MelonLogger.Error(msg);
            }
        }

        /// <summary>
        /// 예외와 함께 오류 로그를 출력합니다. (LogLevel이 Error 이상일 때만 출력)
        /// </summary>
        public static void Error(string msg, Exception ex)
        {
            if (CurrentLogLevel >= ModLogLevel.Error)
            {
                MelonLogger.Error(ex != null ? $"{msg}: {ex}" : msg);
            }
        }

        /// <summary>
        /// 상세 진단 로그를 출력합니다. (LogLevel이 Verbose일 때만 출력)
        /// </summary>
        public static void Verbose(string msg)
        {
            if (CurrentLogLevel >= ModLogLevel.Verbose)
            {
                MelonLogger.Msg(msg);
            }
        }

        /// <summary>
        /// 로그 레벨과 무관하게 최초 1회 필수 안내(예: UMPC 최적화 적용 알림)를 출력합니다.
        /// 레벨 판정을 거치지 않고 MelonLogger로 바로 내보냅니다.
        /// </summary>
        public static void LogAlways(string msg)
        {
            MelonLogger.Msg(msg);
        }
    }
}
