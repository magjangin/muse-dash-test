using HarmonyLib;
using MelonLoader;
using System;
using UnityEngine.UI;

namespace muse_dash_test
{
    internal static class SetSelectedMusicNameTxtPatchHelper
    {
        public static void Apply(Il2Cpp.SetSelectedMusicNameTxt instance, string source)
        {
            try
            {
                if (instance == null) return;

                string uid = CustomPlaySession.Current.LastKnownMusicUid;
                if (!CustomContentIds.IsVirtualSong(uid))
                {
                    uid = PnlStagePatchHelper.GetCurrentSelectedMusicUid()
                        ?? CustomPlaySession.Current.LastClickedMusicUid;
                }

                if (!CustomContentIds.IsVirtualSong(uid)) return;
                if (!MainMod.TryGetCachedHwaPrimaryVirtualSong(
                        uid,
                        out string title,
                        out string artist,
                        out _, out _, out _, out _, out _, out _, out _))
                {
                    return;
                }

                string value = null;
                if (instance.isMusicName && !string.IsNullOrWhiteSpace(title))
                {
                    value = title;
                }
                else if (instance.isMusicAuthor && !string.IsNullOrWhiteSpace(artist))
                {
                    value = artist;
                }

                if (string.IsNullOrWhiteSpace(value)) return;

                int writes = 0;
                if (instance.txt != null)
                {
                    instance.txt.text = value;
                    writes++;
                }

                // 긴 제목 컨트롤러는 simple/backup 텍스트를 별도로 보유합니다.
                // 해당 컨트롤러가 활성화되는 화면에서도 같은 값으로 맞춥니다.
                if (instance.m_LongCtrl != null && instance.m_LongCtrl.gameObject != null)
                {
                    var texts = instance.m_LongCtrl.gameObject.GetComponentsInChildren<Text>(true);
                    if (texts != null)
                    {
                        foreach (var text in texts)
                        {
                            if (text == null || text.text == value) continue;
                            text.text = value;
                            writes++;
                        }
                    }
                }

                MelonLogger.Msg($"[SetSelectedMusicNameTxt.{source}] uid={uid}, value={value}, writes={writes}");
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[SetSelectedMusicNameTxt.{source}] 커스텀 곡 정보 적용 예외: {ex}");
            }
        }
    }

    [HarmonyPatch(typeof(Il2Cpp.SetSelectedMusicNameTxt), "Awake")]
    internal static class SetSelectedMusicNameTxt_Awake_Patch
    {
        private static void Postfix(Il2Cpp.SetSelectedMusicNameTxt __instance)
        {
            SetSelectedMusicNameTxtPatchHelper.Apply(__instance, "Awake");
        }
    }

    [HarmonyPatch(typeof(Il2Cpp.SetSelectedMusicNameTxt), "OnEnable")]
    internal static class SetSelectedMusicNameTxt_OnEnable_Patch
    {
        private static void Postfix(Il2Cpp.SetSelectedMusicNameTxt __instance)
        {
            SetSelectedMusicNameTxtPatchHelper.Apply(__instance, "OnEnable");
        }
    }
}
