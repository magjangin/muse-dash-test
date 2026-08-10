using MelonLoader;
using HarmonyLib;
using System;
using System.IO;
using Il2CppAssets.Scripts.Database;

namespace muse_dash_test
{
    /// <summary>
    /// [실험 모듈] 게임 내장 콜라보 만료 날짜 판정 로직 테스트용 패치.
    ///
    /// "save custom key/EXP_HIDE_COLLAB.txt" 파일의 내용이 "콜라보_강제만료=활성화"일 때,
    /// DBConfigDlcUIExtension의 모든 콜라보 팩 만료 시각(dlcEndTime)을 
    /// 이미 지나버린 과거 날짜(2020년 1월 1일)로 설정하여 
    /// 오프라인 샌드박스가 비활성화된 상태에서 게임 원본 만료 로직이 
    /// 콜라보 팩을 어떻게 숨기거나 락(Lock)거치는지 실험합니다.
    /// </summary>
    public static class ExperimentCollabExpire
    {
        private const string FlagFileName = "EXP_HIDE_COLLAB.txt";
        private const string FlagKey      = "콜라보_강제만료";
        private const string ValueOn      = "활성화";
        private const string ValueOff     = "비활성화";

        private static string FlagFilePath =>
            Path.Combine(
                MelonLoader.Utils.MelonEnvironment.GameRootDirectory,
                "save custom key",
                FlagFileName);

        public static bool IsExperimentActive { get; private set; } = false;

        public static void Initialize()
        {
            EnsureFlagFile();
            Reload();
        }

        public static void Reload()
        {
            try
            {
                IsExperimentActive = ReadFlag();
                if (IsExperimentActive)
                {
                    MelonLogger.Msg("[Experiment] 🧪 콜라보 과거 날짜(2020년) 강제 만료 실험이 활성화되었습니다.");
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[Experiment] 플래그 파일 읽기 실패: {ex.Message}");
                IsExperimentActive = false;
            }
        }

        private static bool ReadFlag()
        {
            if (!File.Exists(FlagFilePath))
                return false;

            foreach (string rawLine in File.ReadAllLines(FlagFilePath, System.Text.Encoding.UTF8))
            {
                string line = rawLine.Trim();
                if (line.StartsWith(FlagKey + "="))
                {
                    string val = line.Substring((FlagKey + "=").Length).Trim();
                    return val.Equals(ValueOn, StringComparison.OrdinalIgnoreCase);
                }
            }
            return false;
        }

        private static void EnsureFlagFile()
        {
            if (!File.Exists(FlagFilePath))
            {
                try
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(FlagFilePath)!);
                    File.WriteAllText(FlagFilePath, $"{FlagKey}={ValueOn}\n", System.Text.Encoding.UTF8);
                }
                catch { }
            }
        }

        [HarmonyPatch(typeof(DBConfigDlcUIExtension), nameof(DBConfigDlcUIExtension.Deserialize))]
        public static class DBConfigDlcUIExtensionExperimentPatch
        {
            static void Postfix(DBConfigDlcUIExtension __instance)
            {
                Reload();
                if (!IsExperimentActive)
                    return;

                try
                {
                    var list = __instance.list;
                    if (list == null) return;

                    // 2020년 1월 1일 (이미 지난 과거 날짜)
                    var pastTime = new Il2CppSystem.DateTime(2020, 1, 1, 0, 0, 0);
                    int count = 0;

                    for (int i = 0; i < list.Count; i++)
                    {
                        var info = list[i];
                        if (info != null)
                        {
                            info.dlcEndTime = pastTime;
                            count++;
                        }
                    }

                    MelonLogger.Msg($"[Experiment] 🧪 {count}개 콜라보 팩의 dlcEndTime을 2020-01-01(과거)로 설정 완료. (게임 내장 만료 동작 관찰)");
                }
                catch (Exception ex)
                {
                    MelonLogger.Error($"[Experiment] 콜라보 만료 실험 패치 오류: {ex.Message}");
                }
            }
        }
    }
}
