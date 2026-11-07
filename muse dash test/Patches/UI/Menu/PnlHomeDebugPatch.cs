using MelonLoader;
using System;
using UnityEngine;
using muse_dash_test;
using Il2CppAssets.Scripts.UI.Panels;

namespace muse_dash_test.Patches.UI.Menu
{
    /// <summary>
    /// 메인 메뉴(PnlMenu) 진입 시 BGM 상태 추적 패치 클래스입니다.
    /// </summary>
    [HarmonyLib.HarmonyPatch(typeof(PnlMenu), "OnEnable")]
    public class PnlMenu_OnEnable_DebugPatch
    {
        public static void Postfix(PnlMenu __instance)
        {
            MenuPanelDebugHelper.LogPanelState("PnlMenu.OnEnable", __instance != null ? __instance.gameObject : null);
        }
    }

    public static class MenuPanelDebugHelper
    {
        public static void LogPanelState(string eventName, GameObject sourceGo)
        {
            if (!ModLogger.IsLevelEnabled(ModLogLevel.Verbose))
            {
                return;
            }

            try
            {
                string selectedUid = CustomPlaySession.Current.LastKnownMusicUid;

                AudioSource bgmSource = GameBgmSource.TryGet(out _);
                if (bgmSource == null)
                {
                    GameObject bgmGo = GameObject.Find("BGM");
                    bgmSource = bgmGo != null ? bgmGo.GetComponent<AudioSource>() : null;
                }

                string clipName = bgmSource != null && bgmSource.clip != null ? bgmSource.clip.name : "(null)";
                bool isPlaying = bgmSource != null && bgmSource.isPlaying;
                float volume = bgmSource != null ? bgmSource.volume : 0f;

                ModLogger.Verbose($"🔍 [UI.PanelDebug] ===== {eventName} =====");
                ModLogger.Verbose($"  - Selected UID: {selectedUid ?? "(null)"} (VirtualSong: {CustomContentIds.IsVirtualSong(selectedUid)})");
                ModLogger.Verbose($"  - BGM State: clip='{clipName}', playing={isPlaying}, vol={volume:F2}");
                ModLogger.Verbose($"==============================================");
            }
            catch (Exception ex)
            {
                ModLogger.Error($"[UI.PanelDebug] {eventName} 추적 에러: {ex}");
            }
        }
    }
}
