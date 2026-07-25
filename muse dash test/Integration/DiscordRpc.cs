using System;
using System.Runtime.InteropServices;

namespace muse_dash_test
{
    /// <summary>
    /// discord-rpc.dll 또는 discord_game_sdk Native C API P/Invoke 바인딩 클래스입니다.
    /// </summary>
    public static class DiscordRpc
    {
        private const string DllName = "discord-rpc";

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct DiscordRichPresence
        {
            public string state;            // max 128 bytes
            public string details;          // max 128 bytes
            public long startTimestamp;
            public long endTimestamp;
            public string largeImageKey;     // max 32 bytes
            public string largeImageText;    // max 128 bytes
            public string smallImageKey;     // max 32 bytes
            public string smallImageText;    // max 128 bytes
            public string partyId;          // max 128 bytes
            public int partySize;
            public int partyMax;
            public string matchSecret;      // max 128 bytes
            public string joinSecret;       // max 128 bytes
            public string spectateSecret;   // max 128 bytes
            public bool instance;
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void ReadyCallback(ref DiscordUser connectedUser);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void DisconnectedCallback(int errorCode, string message);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void ErroredCallback(int errorCode, string message);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void JoinGameCallback(string secret);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void SpectateGameCallback(string secret);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void JoinRequestCallback(ref DiscordUser request);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct DiscordEventHandlers
        {
            public ReadyCallback readyCallback;
            public DisconnectedCallback disconnectedCallback;
            public ErroredCallback erroredCallback;
            public JoinGameCallback joinGameCallback;
            public SpectateGameCallback spectateGameCallback;
            public JoinRequestCallback joinRequestCallback;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct DiscordUser
        {
            public string userId;
            public string username;
            public string discriminator;
            public string avatar;
        }

        [DllImport(DllName, EntryPoint = "Discord_Initialize", CallingConvention = CallingConvention.Cdecl)]
        public static extern void Initialize(string applicationId, ref DiscordEventHandlers handlers, int autoRegister, string optionalSteamId);

        [DllImport(DllName, EntryPoint = "Discord_Shutdown", CallingConvention = CallingConvention.Cdecl)]
        public static extern void Shutdown();

        [DllImport(DllName, EntryPoint = "Discord_RunCallbacks", CallingConvention = CallingConvention.Cdecl)]
        public static extern void RunCallbacks();

        [DllImport(DllName, EntryPoint = "Discord_UpdatePresence", CallingConvention = CallingConvention.Cdecl)]
        public static extern void UpdatePresence(ref DiscordRichPresence presence);

        [DllImport(DllName, EntryPoint = "Discord_ClearPresence", CallingConvention = CallingConvention.Cdecl)]
        public static extern void ClearPresence();
    }
}
