using System;
using MelonLoader;

namespace muse_dash_test
{
    /// <summary>
    /// 뮤즈대시 모드의 플레이 상태(곡 선택, 인게임 플레이, 결과 화면)를 디스코드 Rich Presence에 동동 업데이트하는 매니저입니다.
    /// </summary>
    public static class DiscordPresenceManager
    {
        // Muse Dash 기본 디스코드 Application ID (또는 모드 커스텀 ID)
        private const string DefaultAppId = "1180000000000000000";

        private static bool isInitialized;
        private static bool isAvailable = true;
        private static DiscordRpc.DiscordEventHandlers handlers;
        private static DiscordRpc.DiscordRichPresence presence;
        private static float updateTimer;
        private const float UpdateInterval = 2.0f;

        public static void Initialize()
        {
            if (isInitialized || !isAvailable) return;

            try
            {
                handlers = new DiscordRpc.DiscordEventHandlers
                {
                    readyCallback = OnReady,
                    disconnectedCallback = OnDisconnected,
                    erroredCallback = OnErrored
                };

                DiscordRpc.Initialize(DefaultAppId, ref handlers, 1, null);
                isInitialized = true;
                MelonLogger.Msg("[DiscordRPC] Discord Rich Presence 초기화 성공!");

                SetIdleState();
            }
            catch (DllNotFoundException)
            {
                isAvailable = false;
                MelonLogger.Warning("[DiscordRPC] discord-rpc.dll을 찾을 수 없어 Discord Rich Presence 기능이 비활성화됩니다.");
            }
            catch (Exception ex)
            {
                isAvailable = false;
                MelonLogger.Error($"[DiscordRPC] Discord Rich Presence 초기화 중 예외 발생: {ex.Message}");
            }
        }

        public static void Update()
        {
            if (!isInitialized || !isAvailable) return;

            try
            {
                updateTimer += UnityEngine.Time.deltaTime;
                if (updateTimer >= UpdateInterval)
                {
                    updateTimer = 0f;
                    DiscordRpc.RunCallbacks();
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[DiscordRPC] Update 틱 중 예외 발생: {ex.Message}");
            }
        }

        public static void SetIdleState()
        {
            if (!isInitialized || !isAvailable) return;

            presence = new DiscordRpc.DiscordRichPresence
            {
                details = "메인 메뉴",
                state = "대기 중...",
                largeImageKey = "icon_main",
                largeImageText = "Muse Dash Custom Chart",
                startTimestamp = GetCurrentUnixTimestamp()
            };

            SendPresence();
        }

        public static void SetSelectingSong(string title, string artist, string difficultyText = "")
        {
            if (!isInitialized || !isAvailable) return;

            string detailsText = !string.IsNullOrEmpty(artist) ? $"{title} - {artist}" : title;
            string stateText = !string.IsNullOrEmpty(difficultyText) ? $"곡 선택 중 ({difficultyText})" : "곡 선택 중";

            presence = new DiscordRpc.DiscordRichPresence
            {
                details = TruncateString(detailsText, 128),
                state = TruncateString(stateText, 128),
                largeImageKey = "icon_main",
                largeImageText = "Muse Dash Custom Chart",
                startTimestamp = GetCurrentUnixTimestamp()
            };

            SendPresence();
        }

        public static void SetPlayingSong(string title, string artist, string difficultyText = "")
        {
            if (!isInitialized || !isAvailable) return;

            string detailsText = !string.IsNullOrEmpty(artist) ? $"{title} - {artist}" : title;
            string stateText = !string.IsNullOrEmpty(difficultyText) ? $"플레이 중 [{difficultyText}]" : "플레이 중";

            presence = new DiscordRpc.DiscordRichPresence
            {
                details = TruncateString(detailsText, 128),
                state = TruncateString(stateText, 128),
                largeImageKey = "icon_main",
                largeImageText = "Muse Dash Custom Chart",
                startTimestamp = GetCurrentUnixTimestamp()
            };

            SendPresence();
        }

        public static void SetResults(string title, int score, float accuracy, bool isFullCombo, bool isAllPerfect)
        {
            if (!isInitialized || !isAvailable) return;

            string statusText = isAllPerfect ? "ALL PERFECT!" : (isFullCombo ? "FULL COMBO!" : "CLEAR!");
            string stateText = $"{statusText} | Acc: {accuracy:F2}% (Score: {score})";

            presence = new DiscordRpc.DiscordRichPresence
            {
                details = TruncateString($"결과: {title}", 128),
                state = TruncateString(stateText, 128),
                largeImageKey = "icon_main",
                largeImageText = "Muse Dash Custom Chart",
                startTimestamp = GetCurrentUnixTimestamp()
            };

            SendPresence();
        }

        public static void Shutdown()
        {
            if (!isInitialized || !isAvailable) return;

            try
            {
                DiscordRpc.Shutdown();
                isInitialized = false;
                MelonLogger.Msg("[DiscordRPC] Discord Rich Presence 종료 완료.");
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[DiscordRPC] Shutdown 예외 발생: {ex.Message}");
            }
        }

        private static void SendPresence()
        {
            try
            {
                DiscordRpc.UpdatePresence(ref presence);
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[DiscordRPC] Presence 전송 예외 발생: {ex.Message}");
            }
        }

        private static long GetCurrentUnixTimestamp()
        {
            return (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
        }

        private static string TruncateString(string str, int maxLength)
        {
            if (string.IsNullOrEmpty(str)) return string.Empty;
            return str.Length <= maxLength ? str : str.Substring(0, maxLength - 3) + "...";
        }

        private static void OnReady(ref DiscordRpc.DiscordUser connectedUser)
        {
            MelonLogger.Msg($"[DiscordRPC] 디스코드 연결 완료! 사용자: {connectedUser.username}#{connectedUser.discriminator}");
        }

        private static void OnDisconnected(int errorCode, string message)
        {
            MelonLogger.Warning($"[DiscordRPC] 디스코드 연결 해제됨: ({errorCode}) {message}");
        }

        private static void OnErrored(int errorCode, string message)
        {
            MelonLogger.Error($"[DiscordRPC] 디스코드 오류 발생: ({errorCode}) {message}");
        }
    }
}
