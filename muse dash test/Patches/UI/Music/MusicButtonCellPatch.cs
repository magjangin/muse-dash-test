using MelonLoader;
using HarmonyLib;
using Il2CppAssets.Scripts.UI.Panels.PnlMusicTag;
using Il2CppAssets.Scripts.Database;
using System;
using System.Collections.Generic;
using System.IO;
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

        public static void Postfix(MusicButtonCell __instance)
        {
        }
    }

    // MusicButtonCell.InitMusicCell 후킹 - 텍스트 탐색 오버헤드는 제거하되 cover.png 이미지 주입 로직은 유지합니다.
    [HarmonyPatch(typeof(MusicButtonCell), nameof(MusicButtonCell.InitMusicCell), new Type[] { typeof(MusicInfo), typeof(int) })]
    public class MusicButtonCell_InitMusicCell_Patch
    {
        public static bool Prepare() => true;

        public static void Prefix(MusicButtonCell __instance, MusicInfo initMusicInfo, int tabIndex)
        {
        }

        public static void Postfix(MusicButtonCell __instance, MusicInfo initMusicInfo, int tabIndex)
        {
            try
            {
                if (__instance == null || initMusicInfo == null) return;
                if (!CustomContentIds.IsVirtualSong(initMusicInfo.uid)) return;

                // 곡 폴더의 cover.png를 셀의 ImgCover에 경량 주입
                ApplyCustomCover(__instance.gameObject, initMusicInfo.uid);
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"MusicButtonCell.InitMusicCell Postfix 예외: {ex}");
            }
        }

        private static void ApplyCustomCover(GameObject cellGo, string uid)
        {
            if (cellGo == null) return;
            if (!CoverImageManager.TryGetCoverSprite(uid, out var coverSprite) || coverSprite == null) return;

            var images = cellGo.GetComponentsInChildren<Image>(true);
            if (images == null) return;

            foreach (var img in images)
            {
                if (img == null || img.gameObject == null) continue;
                if (img.gameObject.name != "ImgCover") continue;

                if (img.sprite != coverSprite)
                {
                    img.sprite = coverSprite;
                }
                return;
            }
        }
    }

    /// <summary>
    /// 곡 폴더의 cover.png를 읽어 Sprite로 디코딩하고 UID별로 캐싱하는 매니저입니다.
    /// 파일이 없거나 디코딩에 실패한 UID는 재시도하지 않도록 별도로 기록해 I/O를 줄입니다.
    /// </summary>
    public static class CoverImageManager
    {
        private static readonly Dictionary<string, Sprite> cache = new Dictionary<string, Sprite>();
        private static readonly HashSet<string> missing = new HashSet<string>();

        public static bool TryGetCoverSprite(string uid, out Sprite sprite)
        {
            sprite = null;
            if (string.IsNullOrEmpty(uid)) return false;

            if (cache.TryGetValue(uid, out sprite)) return sprite != null;
            if (missing.Contains(uid)) return false;

            if (!MainMod.TryGetSongDirectory(uid, out string songDir) || string.IsNullOrEmpty(songDir))
            {
                missing.Add(uid);
                return false;
            }

            string coverPath = Path.Combine(songDir, "cover.png");
            if (!File.Exists(coverPath))
            {
                missing.Add(uid);
                return false;
            }

            try
            {
                byte[] data = File.ReadAllBytes(coverPath);
                var tex = new Texture2D(2, 2);
                if (!ImageConversion.LoadImage(tex, data))
                {
                    MelonLogger.Error($"[Cover] cover.png 디코딩 실패: {coverPath}");
                    UnityEngine.Object.Destroy(tex);
                    missing.Add(uid);
                    return false;
                }

                tex.name = $"CustomCoverTex_{uid}";
                tex.hideFlags |= HideFlags.DontUnloadUnusedAsset;

                var spr = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
                spr.name = $"CustomCoverSprite_{uid}";
                spr.hideFlags |= HideFlags.DontUnloadUnusedAsset;

                cache[uid] = spr;
                sprite = spr;
                return true;
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[Cover] cover.png 로드 예외 uid='{uid}': {ex}");
                missing.Add(uid);
                return false;
            }
        }
    }
}
