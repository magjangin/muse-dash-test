using System;
using HarmonyLib;
using Il2Cpp;
using MelonLoader;

namespace muse_dash_test
{
    /// <summary>
    /// 뮤즈대시 원본 DiscordManager의 주요 메서드를 후킹하여 디스코드 상태 업데이트 파라미터와
    /// 호출 타이밍을 상세하게 로그로 추적하는 진단 패치입니다.
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
        public static void SetUpdateActivity_Prefix(DiscordManager __instance, bool isPlaying, ref string levelInfo)
        {
            string currentUid = CustomPlaySession.Current.LastKnownMusicUid;
            MelonLogger.Msg($"[DiscordHook.SetUpdateActivity.Prefix] ----------------------------------------");
            MelonLogger.Msg($"  - isPlaying: {isPlaying}");
            MelonLogger.Msg($"  - levelInfo (원본 전달값): '{levelInfo ?? "(null)"}'");
            MelonLogger.Msg($"  - Current Selected UID: '{currentUid}'");

            if (!string.IsNullOrEmpty(currentUid))
            {
                DiscordPresenceManager.ResolveSongDetails(currentUid, out string title, out string artist);
                MelonLogger.Msg($"  - ResolveSongDetails 해석 결과: Title='{title}', Artist='{artist}'");

                bool isCustom = CustomContentIds.IsVirtualSong(currentUid) || HwaResourceManager.IsRegisteredCustomHostUid(currentUid);
                MelonLogger.Msg($"  - IsCustomRelated: {isCustom}");

                // 만약 커스텀 곡이면 진단용으로 원본 levelInfo를 커스텀 곡 제목으로 덮어써서 테스트합니다.
                if (isCustom && !string.IsNullOrEmpty(title))
                {
                    string oldInfo = levelInfo;
                    string statusTag = isPlaying ? "플레이 중" : "곡 선택 중";
                    levelInfo = $"{title} - {artist} ({statusTag})";
                    MelonLogger.Msg($"  - [가로채기 성공] levelInfo 변조: '{oldInfo}' ➔ '{levelInfo}'");
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
    }
}
