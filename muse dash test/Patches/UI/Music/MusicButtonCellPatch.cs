using MelonLoader;
using HarmonyLib;
using Il2CppAssets.Scripts.UI.Panels.PnlMusicTag;
using Il2CppAssets.Scripts.Database;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace muse_dash_test
{
    // MusicButtonCell.OnButtonClicked 후킹
    [HarmonyPatch(typeof(MusicButtonCell), nameof(MusicButtonCell.OnButtonClicked))]
    public class MusicButtonCell_OnButtonClicked_Patch
    {
        public static bool Prepare() => true;

        public static void Prefix(MusicButtonCell __instance)
        {
            try
            {
                if (__instance != null)
                {
                    var musicInfo = __instance.musicInfo;
                    string uid = musicInfo != null ? musicInfo.uid : "(null)";
                    CustomPlaySession.Current.LastClickedMusicUid = uid;
                    CustomPlaySession.Current.RememberMusicSelection(uid);
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"MusicButtonCell.OnButtonClicked Prefix 예외: {ex}");
            }
        }
    }

    // MusicButtonCell.InitMusicCell 후킹
    [HarmonyPatch(typeof(MusicButtonCell), nameof(MusicButtonCell.InitMusicCell), new Type[] { typeof(MusicInfo), typeof(int) })]
    public class MusicButtonCell_InitMusicCell_Patch
    {
        public static bool Prepare() => true;

        public static void Prefix(MusicButtonCell __instance, MusicInfo initMusicInfo, int tabIndex)
        {
            try
            {
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"MusicButtonCell.InitMusicCell Prefix 예외: {ex}");
            }
        }

        public static void Postfix(MusicButtonCell __instance, MusicInfo initMusicInfo, int tabIndex)
        {
            try
            {
                if (__instance == null || initMusicInfo == null) return;
                
                // 가상 곡인지 여부 체크
                if (!CustomContentIds.IsVirtualSong(initMusicInfo.uid)) return;

                string title = initMusicInfo.name;
                string author = initMusicInfo.author;

                // 캐시된 manifest 정보 조회 시도
                if (MainMod.TryGetCachedHwaPrimaryVirtualSong(initMusicInfo.uid,
                    out string manifestTitle, out string manifestArtist, out _, out _, out _, out _, out _, out _, out _))
                {
                    if (!string.IsNullOrWhiteSpace(manifestTitle)) title = manifestTitle;
                    if (!string.IsNullOrWhiteSpace(manifestArtist)) author = manifestArtist;
                }

                // cell의 게임오브젝트 텍스트 컴포넌트들을 직접 업데이트
                var go = __instance.gameObject;
                if (go == null) return;

                var texts = go.GetComponentsInChildren<Text>(true);
                for (int i = 0; i < texts.Length; i++)
                {
                    var text = texts[i];
                    if (text == null) continue;

                    string objectName = text.gameObject.name;
                    bool isTitle = objectName.IndexOf("SongTitle", StringComparison.OrdinalIgnoreCase) >= 0 ||
                                   objectName.IndexOf("TxtTitle", StringComparison.OrdinalIgnoreCase) >= 0 ||
                                   objectName.IndexOf("Name", StringComparison.OrdinalIgnoreCase) >= 0;

                    bool isAuthor = objectName.IndexOf("Artist", StringComparison.OrdinalIgnoreCase) >= 0 ||
                                    objectName.IndexOf("TxtAuthor", StringComparison.OrdinalIgnoreCase) >= 0 ||
                                    objectName.IndexOf("Author", StringComparison.OrdinalIgnoreCase) >= 0;

                    if (isTitle)
                    {
                        if (text.text != title)
                        {
                            text.text = title;
                        }
                    }
                    else if (isAuthor)
                    {
                        if (text.text != author)
                        {
                            text.text = author;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"MusicButtonCell.InitMusicCell Postfix 예외: {ex}");
            }
        }
    }
}
