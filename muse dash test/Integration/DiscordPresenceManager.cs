using System;
using System.Runtime.CompilerServices;
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
                if (IsDiscordManagerInstanceAvailable())
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

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static bool IsDiscordManagerInstanceAvailable()
        {
            return Singleton<Il2Cpp.DiscordManager>.instance != null;
        }

        // 매 프레임 부르는 Update()는 두지 않습니다. 게임 내장 DiscordManager가 자체 틱/콜백을 관리하므로
        // 이 클래스는 이벤트(곡 선택·플레이·결과)마다 한 번씩 보내기만 합니다.

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
                    TryReadSongDetailsFromDb(uid, ref title, ref artist);
                }
                catch
                {
                    // DB 접근 또는 게임 업데이트로 인한 JIT 실패 시 무시하고 폴백 사용
                }
            }

            // 3순위: 폴백
            if (string.IsNullOrWhiteSpace(title)) title = $"곡 {uid}";
            if (string.IsNullOrWhiteSpace(artist)) artist = "Muse Dash";
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void TryReadSongDetailsFromDb(string uid, ref string title, ref string artist)
        {
            var dbTag = GlobalDataBase.dbMusicTag;
            if (dbTag == null) return;
            var musicInfo = dbTag.GetMusicInfoFromAll(uid);
            if (musicInfo == null) return;
            if (string.IsNullOrWhiteSpace(title)) title = musicInfo.name;
            if (string.IsNullOrWhiteSpace(artist)) artist = musicInfo.author;
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
            float displayAcc = accuracy <= 1.0f ? accuracy * 100f : accuracy;
            string stateText = $"{title} [{statusText} | Acc: {displayAcc:F2}%]";
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
                if (TryInvokeSetUpdateActivity(isPlaying, levelInfo))
                {
                    lastSentIsPlaying = isPlaying;
                    lastSentLevelInfo = levelInfo;
                }
            }
            catch (Exception ex)
            {
                ModLogger.Error($"[DiscordRPC] SetUpdateActivity 호출 실패: {ex.Message}");
            }
        }

        /// <summary>
        /// 게임 업데이트로 <c>DiscordManager.SetUpdateActivity</c> 시그니처가 바뀌더라도
        /// 호출부 <see cref="SendPresence"/>의 try/catch가 JIT <c>MissingMethodException</c>을 잡을 수 있도록 분리합니다.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static bool TryInvokeSetUpdateActivity(bool isPlaying, string levelInfo)
        {
            var discordManager = Singleton<Il2Cpp.DiscordManager>.instance;
            if (discordManager == null) return false;
            discordManager.SetUpdateActivity(isPlaying, levelInfo);
            return true;
        }
    }
}
