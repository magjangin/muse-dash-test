using System;
using MelonLoader;
using Il2CppAssets.Scripts.Database;
using Il2CppPeroTools2.Commons;

namespace muse_dash_test
{
    /// <summary>
    /// 뮤즈대시 원본 내장 DiscordManager(Il2Cpp)를 활용하여 플레이 상태(곡 선택, 인게임 플레이, 결과 화면)를
    /// 디스코드 Rich Presence에 실시간 업데이트하는 고수준 매니저입니다.
    /// 외부 네이티브 DLL(discord-rpc.dll) 의존성 없이 게임 내장 DiscordManager 싱글톤을 제어합니다.
    /// </summary>
    public static class DiscordPresenceManager
    {
        private static bool isInitialized;
        private static string lastSentLevelInfo;
        private static bool? lastSentIsPlaying;

        public static void Initialize()
        {
            if (isInitialized || !ModConfig.EnableDiscordRPC) return;

            try
            {
                var discordManager = Singleton<Il2Cpp.DiscordManager>.instance;
                if (discordManager != null)
                {
                    isInitialized = true;
                    ModLogger.Msg("[DiscordRPC] 내장 Il2Cpp.DiscordManager 연동 활성화 성공!");
                    SetIdleState();
                }
                else
                {
                    // 게임 시작 극초기에는 아직 DiscordManager 인스턴스가 없을 수 있으므로 추후 갱신 시 재시도
                    isInitialized = true;
                    ModLogger.Msg("[DiscordRPC] Discord Presence 관리자 초기화 완료 (게임 내장 인스턴스 대기 중)");
                }
            }
            catch (Exception ex)
            {
                ModLogger.Warning($"[DiscordRPC] 초기화 경고: {ex.Message}");
            }
        }

        public static void Update()
        {
            // 게임 내장 DiscordManager가 자체 틱/콜백을 관리하므로 별도의 P/Invoke 폴링 불필요
        }

        public static void SetIdleState()
        {
            SendPresence(isPlaying: false, levelInfo: "In Menu");
        }

        public static void ResolveSongDetails(string uid, out string title, out string artist)
        {
            title = null;
            artist = null;

            if (string.IsNullOrEmpty(uid))
            {
                title = "뮤즈대시";
                artist = "PeroPeroGames";
                return;
            }

            // 1순위: HwaResourceManager 매니페스트 확인 (커스텀 곡)
            var manifest = HwaResourceManager.GetManifest(uid);
            if (manifest != null)
            {
                title = !string.IsNullOrWhiteSpace(manifest.CustomTitle) ? manifest.CustomTitle : manifest.Title;
                artist = !string.IsNullOrWhiteSpace(manifest.CustomArtist) ? manifest.CustomArtist : manifest.Artist;
            }

            // 2순위: 게임 DB (GlobalDataBase.dbMusicTag) 확인 (공식 곡 또는 등록된 가상 곡)
            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(artist))
            {
                try
                {
                    var dbTag = GlobalDataBase.dbMusicTag;
                    if (dbTag != null)
                    {
                        var musicInfo = dbTag.GetMusicInfoFromAll(uid);
                        if (musicInfo != null)
                        {
                            if (string.IsNullOrWhiteSpace(title)) title = musicInfo.name;
                            if (string.IsNullOrWhiteSpace(artist)) artist = musicInfo.author;
                        }
                    }
                }
                catch
                {
                    // DB 접근 실패 시 무시
                }
            }

            // 3순위: 폴백
            if (string.IsNullOrWhiteSpace(title)) title = $"곡 {uid}";
            if (string.IsNullOrWhiteSpace(artist)) artist = "Muse Dash";
        }

        public static void UpdateForSelection(string uid)
        {
            ResolveSongDetails(uid, out string title, out string artist);
            bool isCustom = CustomContentIds.IsVirtualSong(uid) || HwaResourceManager.IsRegisteredCustomHostUid(uid);
            string difficultyTag = isCustom ? "커스텀 차트" : "공식 차트";
            SetSelectingSong(title, artist, difficultyTag);
        }

        public static void SetSelectingSong(string title, string artist, string difficultyText = "")
        {
            string detailsText = !string.IsNullOrEmpty(artist) ? $"{title} - {artist}" : title;
            string stateText = !string.IsNullOrEmpty(difficultyText) ? $"{detailsText} (곡 선택 중 - {difficultyText})" : $"{detailsText} (곡 선택 중)";
            SendPresence(isPlaying: true, levelInfo: stateText);
        }

        public static void UpdateForPlaying(string uid)
        {
            ResolveSongDetails(uid, out string title, out string artist);
            bool isCustom = CustomContentIds.IsVirtualSong(uid) || HwaResourceManager.IsRegisteredCustomHostUid(uid);
            string difficultyTag = isCustom ? "커스텀 플레이" : "공식 플레이";
            SetPlayingSong(title, artist, difficultyTag);
        }

        public static void SetPlayingSong(string title, string artist, string difficultyText = "")
        {
            string detailsText = !string.IsNullOrEmpty(artist) ? $"{title} - {artist}" : title;
            string stateText = !string.IsNullOrEmpty(difficultyText) ? $"{detailsText} [플레이 중: {difficultyText}]" : $"{detailsText} (플레이 중)";
            SendPresence(isPlaying: true, levelInfo: stateText);
        }

        public static void SetResults(string title, int score, float accuracy, bool isFullCombo, bool isAllPerfect)
        {
            string statusText = isAllPerfect ? "ALL PERFECT!" : (isFullCombo ? "FULL COMBO!" : "CLEAR!");
            string stateText = $"{title} [{statusText} | Acc: {accuracy:F2}%]";
            SendPresence(isPlaying: true, levelInfo: stateText);
        }

        public static void Shutdown()
        {
            try
            {
                SetIdleState();
                isInitialized = false;
                ModLogger.Msg("[DiscordRPC] Discord Presence 관리자 종료.");
            }
            catch (Exception ex)
            {
                ModLogger.Error($"[DiscordRPC] Shutdown 예외: {ex.Message}");
            }
        }

        private static void SendPresence(bool isPlaying, string levelInfo)
        {
            if (!ModConfig.EnableDiscordRPC) return;

            // 동일한 내용 중복 호출 방지
            if (lastSentIsPlaying == isPlaying && lastSentLevelInfo == levelInfo)
                return;

            try
            {
                var discordManager = Singleton<Il2Cpp.DiscordManager>.instance;
                if (discordManager != null)
                {
                    lastSentIsPlaying = isPlaying;
                    lastSentLevelInfo = levelInfo;
                    discordManager.SetUpdateActivity(isPlaying, levelInfo);
                }
            }
            catch (Exception ex)
            {
                ModLogger.Error($"[DiscordRPC] SetUpdateActivity 호출 실패: {ex.Message}");
            }
        }
    }
}
