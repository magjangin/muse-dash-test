using System;
using HarmonyLib;
using Il2Cpp;
using MelonLoader;
using Il2CppAssets.Scripts.UI.Panels;

namespace muse_dash_test
{
    /// <summary>
    /// 뮤즈대시 원본 DiscordManager의 주요 메서드를 후킹하여 디스코드 상태 업데이트 파라미터와
    /// 호출 타이밍을 추적하고, isPlaying이 false일 때 게임이 "In Menu"로 덮어쓰는 버그를 차단하는 진단 패치입니다.
    /// </summary>
    [HarmonyPatch]
    public static class DiscordManagerDebugPatch
    {
        [HarmonyPatch(typeof(DiscordManager), nameof(DiscordManager.InitDiscord))]
        [HarmonyPrefix]
        public static void InitDiscord_Prefix(DiscordManager __instance)
        {
            MelonLogger.Msg($"[DiscordHook.InitDiscord] ⚓ DiscordManager.InitDiscord() 호출됨! Instance={__instance?.Pointer ?? IntPtr.Zero}");
        }

        [HarmonyPatch(typeof(DiscordManager), nameof(DiscordManager.SetUpdateActivity))]
        [HarmonyPrefix]
        public static void SetUpdateActivity_Prefix(DiscordManager __instance, ref bool isPlaying, ref string levelInfo)
        {
            string currentUid = CustomPlaySession.Current.LastKnownMusicUid;
            bool isInBattle = IsInBattleStageContext();

            MelonLogger.Msg($"[DiscordHook.SetUpdateActivity.Prefix] ----------------------------------------");
            MelonLogger.Msg($"  - isPlaying (입력값): {isPlaying}");
            MelonLogger.Msg($"  - isInBattle (배틀 여부): {isInBattle}");
            MelonLogger.Msg($"  - levelInfo (원본 전달값): '{levelInfo ?? "(null)"}'");
            MelonLogger.Msg($"  - Current Selected UID: '{currentUid}'");

            if (!string.IsNullOrEmpty(currentUid))
            {
                DiscordPresenceManager.ResolveSongDetails(currentUid, out string title, out string artist);
                MelonLogger.Msg($"  - ResolveSongDetails 해석 결과: Title='{title}', Artist='{artist}'");

                bool isCustom = CustomContentIds.IsVirtualSong(currentUid) || HwaResourceManager.IsRegisteredCustomHostUid(currentUid);
                MelonLogger.Msg($"  - IsCustomRelated: {isCustom}");

                if (isCustom && !string.IsNullOrEmpty(title))
                {
                    string oldInfo = levelInfo;
                    string statusTag = isInBattle ? "플레이 중" : "곡 선택 중";
                    levelInfo = $"{title} - {artist} ({statusTag})";

                    // [핵심 해결] isPlaying이 false면 게임 내부 C++ 코드에서 'In Menu'로 강제 덮어쓰므로 true로 승환
                    isPlaying = true;
                    MelonLogger.Msg($"  - [가로채기 대성공] levelInfo: '{oldInfo}' ➔ '{levelInfo}', isPlaying -> true 강제 전환 (상태: {statusTag})");
                }
            }
            MelonLogger.Msg($"[DiscordHook.SetUpdateActivity.Prefix] ----------------------------------------");
        }

        [HarmonyPatch(typeof(DiscordManager), nameof(DiscordManager.SetUpdateActivity))]
        [HarmonyPostfix]
        public static void SetUpdateActivity_Postfix(DiscordManager __instance, bool isPlaying, string levelInfo)
        {
            MelonLogger.Msg($"[DiscordHook.SetUpdateActivity.Postfix] SetUpdateActivity 처리 완료. 최종 levelInfo: '{levelInfo}'");
        }

        [HarmonyPatch(typeof(DiscordManager), nameof(DiscordManager.Destroy))]
        [HarmonyPrefix]
        public static void Destroy_Prefix()
        {
            MelonLogger.Msg("[DiscordHook.Destroy] 🛑 DiscordManager.Destroy() 호출됨");
        }

        /// <summary>
        /// 곡 선택 UI/준비 화면 패널의 활성화 여부를 점검하여 실제로 인게임 배틀 중인지 판별합니다.
        /// </summary>
        private static bool IsInBattleStageContext()
        {
            try
            {
                // 곡 선택 패널(PnlStage)이 활성화되어 있으면 곡 선택 중
                var pnlStage = UnityEngine.Object.FindObjectOfType<PnlStage>();
                if (pnlStage != null && pnlStage.gameObject != null && pnlStage.gameObject.activeInHierarchy)
                {
                    return false;
                }

                // 곡 준비 패널(PnlPreparation)이 활성화되어 있으면 곡 선택 중
                var pnlPreparation = UnityEngine.Object.FindObjectOfType<PnlPreparation>();
                if (pnlPreparation != null && pnlPreparation.gameObject != null && pnlPreparation.gameObject.activeInHierarchy)
                {
                    return false;
                }

                // 배틀 패널(PnlBattle)이나 배틀 스테이지가 있으면 플레이 중
                var pnlBattle = UnityEngine.Object.FindObjectOfType<PnlBattle>();
                if (pnlBattle != null && pnlBattle.gameObject != null && pnlBattle.gameObject.activeInHierarchy)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[DiscordHook.IsInBattleStageContext] 예외 발생: {ex.Message}");
            }

            return false;
        }
    }
}
