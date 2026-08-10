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
    /// 오프라인 샌드박스 및 세부 후킹 패치를 개별적으로 활성화/비활성화합니다.
    ///
    /// 파일 내용 예시:
    ///   오프라인_샌드박스=활성화 (마스터 스위치)
    ///   스팀_BIsDlcInstalled=활성화
    ///   스팀_DLCVerify=활성화
    ///   콜라보_만료시각_2099=활성화
    ///   특수DLC_IsFreeToGet=활성화
    ///   콜라보_카운트다운_스킵=활성화
    /// </summary>
    public static class OfflineCustomSandbox
    {
        // ──────────────────────────────────────────────
        // 경로 및 키 상수
        // ──────────────────────────────────────────────
        private const string FlagFileName = "OFFLINE_SANDBOX.txt";
        
        public const string KeyMaster               = "오프라인_샌드박스";
        public const string KeyBIsDlcInstalled      = "스팀_BIsDlcInstalled";
        public const string KeyDlcVerify            = "스팀_DLCVerify";
        public const string KeyDlcUIExtension       = "콜라보_만료시각_2099";
        public const string KeySpecialDlc           = "특수DLC_IsFreeToGet";
        public const string KeyActiveTimeTimer      = "콜라보_카운트다운_스킵";

        private const string ValueOn  = "활성화";
        private const string ValueOff = "비활성화";

        private static string FlagFilePath =>
            Path.Combine(
                MelonLoader.Utils.MelonEnvironment.GameRootDirectory,
                "save custom key",
                FlagFileName);

        // ──────────────────────────────────────────────
        // 개별 후킹 토글 상태
        // ──────────────────────────────────────────────
        public static bool IsEnabled { get; private set; } = false;
        public static bool IsBIsDlcInstalledEnabled { get; private set; } = false;
        public static bool IsDlcVerifyEnabled { get; private set; } = false;
        public static bool IsDlcUIExtensionEnabled { get; private set; } = false;
        public static bool IsSpecialDlcEnabled { get; private set; } = false;
        public static bool IsActiveTimeTimerEnabled { get; private set; } = false;

        // ──────────────────────────────────────────────
        // 초기화 및 플래그 파일 읽기
        // ──────────────────────────────────────────────
        public static void Initialize()
        {
            EnsureFlagFile();
            Reload();
        }

        public static void Reload()
        {
            try
            {
                var flags = ReadFlags();
                
                IsEnabled                 = flags.GetValueOrDefault(KeyMaster, false);
                IsBIsDlcInstalledEnabled  = flags.GetValueOrDefault(KeyBIsDlcInstalled, IsEnabled);
                IsDlcVerifyEnabled        = flags.GetValueOrDefault(KeyDlcVerify, IsEnabled);
                IsDlcUIExtensionEnabled   = flags.GetValueOrDefault(KeyDlcUIExtension, IsEnabled);
                IsSpecialDlcEnabled       = flags.GetValueOrDefault(KeySpecialDlc, IsEnabled);
                IsActiveTimeTimerEnabled  = flags.GetValueOrDefault(KeyActiveTimeTimer, IsEnabled);

                MelonLogger.Msg($"[OfflineSandbox] 마스터: {(IsEnabled ? "활성" : "비활성")} | BIsDlc: {(IsBIsDlcInstalledEnabled ? "ON" : "OFF")} | DLCVerify: {(IsDlcVerifyEnabled ? "ON" : "OFF")} | 콜라보2099: {(IsDlcUIExtensionEnabled ? "ON" : "OFF")} | 특수DLC: {(IsSpecialDlcEnabled ? "ON" : "OFF")} | 카운트다운스킵: {(IsActiveTimeTimerEnabled ? "ON" : "OFF")}");
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[OfflineSandbox] 플래그 파일 읽기 실패: {ex.Message}");
                IsEnabled = false;
            }
        }

        private static Dictionary<string, bool> ReadFlags()
        {
            var dict = new Dictionary<string, bool>(StringComparer.OrdinalIgnoreCase);

            if (!File.Exists(FlagFilePath))
                return dict;

            foreach (string rawLine in File.ReadAllLines(FlagFilePath, System.Text.Encoding.UTF8))
            {
                string line = rawLine.Trim();
                if (string.IsNullOrEmpty(line) || line.StartsWith("#") || line.StartsWith("//"))
                    continue;

                int eqIdx = line.IndexOf('=');
                if (eqIdx > 0)
                {
                    string key = line.Substring(0, eqIdx).Trim();
                    string val = line.Substring(eqIdx + 1).Trim();
                    dict[key] = val.Equals(ValueOn, StringComparison.OrdinalIgnoreCase);
                }
            }

            return dict;
        }

        private static void EnsureFlagFile()
        {
            if (!File.Exists(FlagFilePath))
            {
                MelonLogger.Msg($"[OfflineSandbox] 플래그 파일이 없어 기본 세분화 파일로 생성합니다: {FlagFilePath}");
                try
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(FlagFilePath)!);
                    string defaultContent = 
                        $"{KeyMaster}={ValueOff}\n" +
                        $"{KeyBIsDlcInstalled}={ValueOff}\n" +
                        $"{KeyDlcVerify}={ValueOff}\n" +
                        $"{KeyDlcUIExtension}={ValueOff}\n" +
                        $"{KeySpecialDlc}={ValueOff}\n" +
                        $"{KeyActiveTimeTimer}={ValueOff}\n";
                    File.WriteAllText(FlagFilePath, defaultContent, System.Text.Encoding.UTF8);
                }
                catch (Exception ex)
                {
                    MelonLogger.Error($"[OfflineSandbox] 플래그 파일 기본 생성 실패: {ex.Message}");
                }
            }
        }
    }

    // ──────────────────────────────────────────────────────────────────────────
    // Harmony 패치: BIsDlcInstalled
    // ──────────────────────────────────────────────────────────────────────────
    [HarmonyPatch(typeof(SteamApps), nameof(SteamApps.BIsDlcInstalled))]
    public class OfflineCustomSandboxPatch
    {
        private static HashSet<uint> loggedDLCs = new HashSet<uint>();

        static bool Prefix(ref bool __result, AppId_t appID)
        {
            if (!OfflineCustomSandbox.IsBIsDlcInstalledEnabled)
                return true; // 원본 로직 실행

            if (loggedDLCs.Add(appID.m_AppId))
            {
                MelonLogger.Msg($"[OfflineSandbox] DLC {appID.m_AppId} → BIsDlcInstalled 허용");
            }

            __result = true;
            return false; // 원본 로직 스킵
        }
    }

    // ──────────────────────────────────────────────────────────────────────────
    // Harmony 패치: DLCVerify
    // ──────────────────────────────────────────────────────────────────────────
    [HarmonyPatch(typeof(SteamManager), nameof(SteamManager.DLCVerify))]
    public class OfflineVerifyPatch
    {
        static bool Prefix(SteamManager __instance)
        {
            if (!OfflineCustomSandbox.IsDlcVerifyEnabled)
                return true; // 원본 로직 실행

            MelonLogger.Msg("[OfflineSandbox] DLCVerify 바이패스 활성화");
            __instance.m_DoSomething1 = true;
            __instance.m_DoSomething3 = true;
            return true;
        }
    }
}
