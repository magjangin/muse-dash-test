using System;
using HarmonyLib;
using Il2Cpp;
using MelonLoader;
using Il2CppAssets.Scripts.UI.Panels;

namespace muse_dash_test
{
    /// <summary>
    /// 뮤즈대시 원본 DiscordManager의 주요 메서드를 후킹하여 디스코드 상태 업데이트 파라미터를 변조하고,
    /// 홈 메뉴(패널 홈) 이탈 시 게임 본래의 "In Menu" 상태로 깔끔하게 돌아가도록 제어하는 패치입니다.
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
            bool isSelectionActive = IsStageSelectionContextActive();
            bool isInBattle = IsInBattleStageContext();

            // 패널 홈(메인 메뉴 등)으로 돌아와 곡 선택 패널도 아니고 배틀 중도 아니면, 게임 본래의 "In Menu" 갱신을 허용하고 건너뜁니다.
            if (!isSelectionActive && !isInBattle)
            {
                MelonLogger.Msg("[DiscordHook.SetUpdateActivity.Prefix] 곡 선택/배틀 컨텍스트 밖(홈 메뉴) 감지 -> 게임 원본 In Menu 상태 허용");
                return;
            }

            string currentUid = CustomPlaySession.Current.LastKnownMusicUid;

            MelonLogger.Msg($"[DiscordHook.SetUpdateActivity.Prefix] ----------------------------------------");
            MelonLogger.Msg($"  - isPlaying (입력값): {isPlaying}");
            MelonLogger.Msg($"  - isSelectionActive (곡선택 패널): {isSelectionActive}");
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

                    // isPlaying이 false면 게임 내부 C++ 코드에서 'In Menu'로 강제 덮어쓰므로 true로 전환
                    isPlaying = true;
                    MelonLogger.Msg($"  - [가로채기 성공] levelInfo: '{oldInfo}' ➔ '{levelInfo}', isPlaying -> true 전환 (상태: {statusTag})");
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
        /// 곡 선택 패널(PnlStage) 또는 곡 준비 패널(PnlPreparation)이 실제로 활성화되어 있는지 확인합니다.
        /// </summary>
        private static bool IsStageSelectionContextActive()
        {
            try
            {
                var pnlStage = UnityEngine.Object.FindObjectOfType<PnlStage>();
                if (pnlStage != null && pnlStage.gameObject != null && pnlStage.gameObject.activeInHierarchy)
                {
                    return true;
                }

                var pnlPreparation = UnityEngine.Object.FindObjectOfType<Il2Cpp.PnlPreparation>();
                if (pnlPreparation != null && pnlPreparation.gameObject != null && pnlPreparation.gameObject.activeInHierarchy)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[DiscordHook.IsStageSelectionContextActive] 예외 발생: {ex.Message}");
            }

            return false;
        }

        /// <summary>
        /// 실제 인게임 배틀 화면(PnlBattle)이 활성화되어 있는지 확인합니다.
        /// </summary>
        private static bool IsInBattleStageContext()
        {
            try
            {
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
