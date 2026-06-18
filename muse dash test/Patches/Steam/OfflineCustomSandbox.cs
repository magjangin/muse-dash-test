using MelonLoader;
using HarmonyLib;
using Il2CppSteamworks;
using Il2Cpp;
using System;
using System.Collections.Generic;
using System.IO;

namespace muse_dash_test
{
    /// <summary>
    /// "save custom key/OFFLINE_SANDBOX.txt" 파일의 내용에 따라
    /// 오프라인 샌드박스 패치를 동적으로 활성화/비활성화합니다.
    ///
    /// 파일 내용:
    ///   오프라인_샌드박스=활성화  →  패치 ON  (DLC 전체 허용, DLCVerify 바이패스)
    ///   오프라인_샌드박스=비활성화 →  패치 OFF (원본 게임 로직 그대로)
    /// </summary>
    public static class OfflineCustomSandbox
    {
        // ──────────────────────────────────────────────
        // 경로 상수
        // ──────────────────────────────────────────────
        private const string FlagFileName = "OFFLINE_SANDBOX.txt";
        private const string FlagKey      = "오프라인_샌드박스";
        private const string ValueOn      = "활성화";
        private const string ValueOff     = "비활성화";

        private static string FlagFilePath =>
            Path.Combine(
                MelonLoader.Utils.MelonEnvironment.GameRootDirectory,
                "save custom key",
                FlagFileName);

        // ──────────────────────────────────────────────
        // 공개 상태
        // ──────────────────────────────────────────────

        /// <summary>현재 오프라인 샌드박스가 활성화 상태인지 여부</summary>
        public static bool IsEnabled { get; private set; } = false;

        // ──────────────────────────────────────────────
        // 초기화: 게임 시작 시 1회 호출
        // ──────────────────────────────────────────────
        public static void Initialize()
        {
            EnsureFlagFile();   // 파일이 없으면 기본값(비활성화)으로 생성
            Reload();           // 파일을 읽어 IsEnabled 갱신
        }

        // ──────────────────────────────────────────────
        // 재로드: 런타임 중 언제든 호출해 상태를 갱신
        // ──────────────────────────────────────────────
        public static void Reload()
        {
            try
            {
                bool prev = IsEnabled;
                IsEnabled = ReadFlag();

                if (IsEnabled != prev)
                {
                    MelonLogger.Msg(IsEnabled
                        ? "[OfflineSandbox] ✅ 오프라인 샌드박스가 활성화되었습니다."
                        : "[OfflineSandbox] ⛔ 오프라인 샌드박스가 비활성화되었습니다.");
                }
                else
                {
                    MelonLogger.Msg($"[OfflineSandbox] 현재 상태: {(IsEnabled ? "활성화" : "비활성화")}");
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[OfflineSandbox] 플래그 파일 읽기 실패: {ex.Message}");
                IsEnabled = false;
            }
        }

        // ──────────────────────────────────────────────
        // 플래그 켜기 / 끄기 (코드에서 직접 제어)
        // ──────────────────────────────────────────────
        public static void Enable()  => WriteFlag(true);
        public static void Disable() => WriteFlag(false);

        public static void Toggle()
        {
            Reload();           // 파일의 최신 상태를 먼저 읽고
            WriteFlag(!IsEnabled);
        }

        // ──────────────────────────────────────────────
        // 내부 유틸
        // ──────────────────────────────────────────────
        private static bool ReadFlag()
        {
            if (!File.Exists(FlagFilePath))
            {
                MelonLogger.Warning($"[OfflineSandbox] 플래그 파일 없음 → 비활성화 처리: {FlagFilePath}");
                return false;
            }

            foreach (string rawLine in File.ReadAllLines(FlagFilePath, System.Text.Encoding.UTF8))
            {
                string line = rawLine.Trim();
                if (line.StartsWith(FlagKey + "="))
                {
                    string val = line.Substring((FlagKey + "=").Length).Trim();
                    return val.Equals(ValueOn, StringComparison.OrdinalIgnoreCase);
                }
            }

            MelonLogger.Warning($"[OfflineSandbox] '{FlagKey}' 키를 찾지 못했습니다 → 비활성화 처리");
            return false;
        }

        private static void WriteFlag(bool enable)
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(FlagFilePath)!);
                File.WriteAllText(FlagFilePath,
                    $"{FlagKey}={( enable ? ValueOn : ValueOff )}\n",
                    System.Text.Encoding.UTF8);

                IsEnabled = enable;
                MelonLogger.Msg(enable
                    ? "[OfflineSandbox] ✅ 플래그 파일 → 활성화로 저장되었습니다."
                    : "[OfflineSandbox] ⛔ 플래그 파일 → 비활성화로 저장되었습니다.");
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[OfflineSandbox] 플래그 파일 쓰기 실패: {ex.Message}");
            }
        }

        private static void EnsureFlagFile()
        {
            if (!File.Exists(FlagFilePath))
            {
                MelonLogger.Msg($"[OfflineSandbox] 플래그 파일이 없어 기본값(비활성화)으로 생성합니다: {FlagFilePath}");
                WriteFlag(false);
            }
        }
    }

    // ──────────────────────────────────────────────────────────────────────────
    // Harmony 패치: BIsDlcInstalled
    //   → IsEnabled=true 일 때만 모든 DLC를 허용
    // ──────────────────────────────────────────────────────────────────────────
    [HarmonyPatch(typeof(SteamApps), nameof(SteamApps.BIsDlcInstalled))]
    public class OfflineCustomSandboxPatch
    {
        private static HashSet<uint> loggedDLCs = new HashSet<uint>();

        static bool Prefix(ref bool __result, AppId_t appID)
        {
            if (!OfflineCustomSandbox.IsEnabled)
                return true; // 원본 로직 실행

            if (loggedDLCs.Add(appID.m_AppId))
            {
                MelonLogger.Msg($"[OfflineSandbox] DLC {appID.m_AppId} → 오프라인 샌드박스 허용");
            }

            __result = true;
            return false; // 원본 로직 스킵
        }
    }

    // ──────────────────────────────────────────────────────────────────────────
    // Harmony 패치: DLCVerify
    //   → IsEnabled=true 일 때만 검증 바이패스
    // ──────────────────────────────────────────────────────────────────────────
    [HarmonyPatch(typeof(SteamManager), nameof(SteamManager.DLCVerify))]
    public class OfflineVerifyPatch
    {
        static bool Prefix(SteamManager __instance)
        {
            if (!OfflineCustomSandbox.IsEnabled)
                return true; // 원본 로직 실행

            MelonLogger.Msg("[OfflineSandbox] DLCVerify 바이패스 (오프라인 샌드박스 활성 중)");
            __instance.m_DoSomething1 = true;
            __instance.m_DoSomething3 = true;
            return true;
        }
    }
}
