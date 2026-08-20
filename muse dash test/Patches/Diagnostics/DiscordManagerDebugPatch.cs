using System;
using HarmonyLib;
using Il2Cpp;
using MelonLoader;
using Il2CppAssets.Scripts.UI.Panels;

namespace muse_dash_test
{
    /// <summary>
    /// 뮤즈대시 원본 DiscordManager의 주요 메서드를 후킹하여 디스코드 상태 업데이트 파라미터를 변조하고,
    /// 홈 메뉴(패널 홈) 복귀 시 디스코드 프로필을 "In Menu" 상태로 명시적 갱신하는 패치입니다.
    /// </summary>
    [HarmonyPatch]
    public static class DiscordManagerDebugPatch
    {
        [HarmonyPatch(typeof(DiscordManager), nameof(DiscordManager.InitDiscord))]
        [HarmonyPrefix]
        public static void InitDiscord_Prefix(DiscordManager __instance)
        {
            ModLogger.Msg($"[DiscordHook.InitDiscord] ⚓ DiscordManager.InitDiscord() 호출됨! Instance={__instance?.Pointer ?? IntPtr.Zero}");
        }

        [HarmonyPatch(typeof(DiscordManager), nameof(DiscordManager.SetUpdateActivity))]
        [HarmonyPrefix]
        public static void SetUpdateActivity_Prefix(DiscordManager __instance, ref bool isPlaying, ref string levelInfo)
        {
            bool isSelectionActive = IsStageSelectionContextActive();
            bool isInBattle = IsInBattleStageContext();

            // 곡 선택 패널도 아니고 배틀 중도 아니면 (홈 메뉴 복귀 시) "In Menu"로 명시적 덮어쓰기 전송
            if (!isSelectionActive && !isInBattle)
            {
                levelInfo = "In Menu";
                isPlaying = false;
                ModLogger.Msg("[DiscordHook.SetUpdateActivity.Prefix] 홈 메뉴 복귀 감지 ➔ 'In Menu' (isPlaying=false) 명시적 갱신 전송");
                return;
            }

            string currentUid = CustomPlaySession.Current.LastKnownMusicUid;

            ModLogger.Msg($"[DiscordHook.SetUpdateActivity.Prefix] ----------------------------------------");
            ModLogger.Msg($"  - isPlaying (입력값): {isPlaying}");
            ModLogger.Msg($"  - isSelectionActive (곡선택 패널): {isSelectionActive}");
            ModLogger.Msg($"  - isInBattle (배틀 여부): {isInBattle}");
            ModLogger.Msg($"  - levelInfo (원본 전달값): '{levelInfo ?? "(null)"}'");
            ModLogger.Msg($"  - Current Selected UID: '{currentUid}'");

            if (!string.IsNullOrEmpty(currentUid))
            {
                DiscordPresenceManager.ResolveSongDetails(currentUid, out string title, out string artist);
                ModLogger.Msg($"  - ResolveSongDetails 해석 결과: Title='{title}', Artist='{artist}'");

                bool isCustom = CustomContentIds.IsVirtualSong(currentUid) || HwaResourceManager.IsRegisteredCustomHostUid(currentUid);
                ModLogger.Msg($"  - IsCustomRelated: {isCustom}");

                if (isCustom && !string.IsNullOrEmpty(title))
                {
                    string oldInfo = levelInfo;
                    string statusTag = isInBattle ? "플레이 중" : "곡 선택 중";
                    levelInfo = $"{title} - {artist} ({statusTag})";

                    // isPlaying이 false면 게임 내부 C++ 코드에서 'In Menu'로 강제 덮어쓰므로 true로 전환
                    isPlaying = true;
                    ModLogger.Msg($"  - [가로채기 성공] levelInfo: '{oldInfo}' ➔ '{levelInfo}', isPlaying -> true 전환 (상태: {statusTag})");
                }
            }
            ModLogger.Msg($"[DiscordHook.SetUpdateActivity.Prefix] ----------------------------------------");
        }

        [HarmonyPatch(typeof(DiscordManager), nameof(DiscordManager.SetUpdateActivity))]
        [HarmonyPostfix]
        public static void SetUpdateActivity_Postfix(DiscordManager __instance, bool isPlaying, string levelInfo)
        {
            ModLogger.Msg($"[DiscordHook.SetUpdateActivity.Postfix] SetUpdateActivity 처리 완료. 최종 levelInfo: '{levelInfo}'");
        }

        [HarmonyPatch(typeof(DiscordManager), nameof(DiscordManager.Destroy))]
        [HarmonyPrefix]
        public static void Destroy_Prefix()
        {
            ModLogger.Msg("[DiscordHook.Destroy] 🛑 DiscordManager.Destroy() 호출됨");
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
                ModLogger.Error($"[DiscordHook.IsStageSelectionContextActive] 예외 발생: {ex.Message}");
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
                ModLogger.Error($"[DiscordHook.IsInBattleStageContext] 예외 발생: {ex.Message}");
            }

            return false;
        }
    }
}
