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
            try
            {
                string selectedUid = CustomPlaySession.Current.SelectedMusicUid;
                if (string.IsNullOrEmpty(selectedUid)) selectedUid = PnlStagePatchHelper.GetCurrentSelectedMusicUid();

                GameObject bgmGo = GameObject.Find("BGM");
                AudioSource bgmSource = bgmGo != null ? bgmGo.GetComponent<AudioSource>() : null;

                string clipName = bgmSource != null && bgmSource.clip != null ? bgmSource.clip.name : "(null)";
                bool isPlaying = bgmSource != null && bgmSource.isPlaying;
                float volume = bgmSource != null ? bgmSource.volume : 0f;

                var pnlMenu = UnityEngine.Object.FindObjectOfType<PnlMenu>();
                bool menuActive = pnlMenu != null && pnlMenu.gameObject != null && pnlMenu.gameObject.activeInHierarchy;

                ModLogger.Msg($"🔍 [UI.PanelDebug] ===== {eventName} =====");
                ModLogger.Msg($"  - Selected UID: {selectedUid ?? "(null)"} (VirtualSong: {CustomContentIds.IsVirtualSong(selectedUid)})");
                ModLogger.Msg($"  - BGM State: clip='{clipName}', playing={isPlaying}, vol={volume:F2}");
                ModLogger.Msg($"==============================================");
            }
            catch (Exception ex)
            {
                ModLogger.Error($"[UI.PanelDebug] {eventName} 추적 에러: {ex}");
            }
        }
    }
}
