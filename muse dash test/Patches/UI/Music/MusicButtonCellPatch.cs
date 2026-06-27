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
            try
            {
                MelonLogger.Msg($"[Postfix] SelectedMusicUid={CustomPlaySession.Current.SelectedMusicUid}, LastClickedMusicUid={CustomPlaySession.Current.LastClickedMusicUid}");
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"MusicButtonCell.OnButtonClicked Postfix 예외: {ex}");
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

                // [조사] cover.png 주입 대상 컴포넌트를 찾기 위해 셀의 이미지 후보를 UID당 1회 덤프합니다.
                MusicCellImageDiagnostics.LogCellImagesOnce(__instance, initMusicInfo.uid);

                string title = initMusicInfo.name;
                string author = initMusicInfo.author;

                // 캐시된 manifest 정보 조회 시도
                if (MainMod.TryGetHwaPrimarySong(initMusicInfo.uid,
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

                // 곡 폴더의 cover.png를 ImgCover에 주입 (있을 때만)
                ApplyCustomCover(go, initMusicInfo.uid);
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"MusicButtonCell.InitMusicCell Postfix 예외: {ex}");
            }
        }

        // 곡 셀의 ImgCover 컴포넌트를 커스텀 커버 스프라이트로 교체합니다.
        private static void ApplyCustomCover(GameObject cellGo, string uid)
        {
            if (!CoverImageManager.TryGetCoverSprite(uid, out var coverSprite) || coverSprite == null)
            {
                return;
            }

            var images = cellGo.GetComponentsInChildren<Image>(true);
            foreach (var img in images)
            {
                if (img == null || img.gameObject == null) continue;
                if (img.gameObject.name != "ImgCover") continue;

                if (img.sprite != coverSprite)
                {
                    img.sprite = coverSprite;
                    MelonLogger.Msg($"[Cover] 곡 셀 ImgCover 스프라이트를 커스텀 커버로 교체 uid='{uid}'");
                }
                return;
            }

            MelonLogger.Warning($"[Cover] ImgCover 컴포넌트를 찾지 못했습니다 uid='{uid}'");
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
                    UnityEngine.Object.Destroy(tex); // 디코딩 실패한 텍스처가 새지 않도록 즉시 해제
                    missing.Add(uid);
                    return false;
                }

                tex.name = $"CustomCoverTex_{uid}";
                tex.hideFlags |= HideFlags.DontUnloadUnusedAsset; // 유니티 GC 방지

                var spr = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
                spr.name = $"CustomCoverSprite_{uid}";
                spr.hideFlags |= HideFlags.DontUnloadUnusedAsset;

                cache[uid] = spr;
                sprite = spr;
                MelonLogger.Msg($"[Cover] cover.png 로드 성공 uid='{uid}' {tex.width}x{tex.height} path={coverPath}");
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

    /// <summary>
    /// 곡 셀(MusicButtonCell)의 ImgCover 현재 커버 스프라이트명을 UID당 1회만 간단히 로깅합니다.
    /// </summary>
    public static class MusicCellImageDiagnostics
    {
        // UID당 1회만 로깅해 스크롤 중 로그 폭발을 방지합니다.
        private static readonly HashSet<string> loggedUids = new HashSet<string>();

        public static void LogCellImagesOnce(MusicButtonCell cell, string uid)
        {
            try
            {
                if (cell == null) return;
                string key = uid ?? "(null)";
                if (loggedUids.Contains(key)) return;
                loggedUids.Add(key);

                var go = cell.gameObject;
                if (go == null) return;

                string coverName = "(없음)";
                var images = go.GetComponentsInChildren<Image>(true);
                foreach (var img in images)
                {
                    if (img == null || img.gameObject == null) continue;
                    if (img.gameObject.name != "ImgCover") continue;
                    coverName = img.sprite != null ? img.sprite.name : "(null)";
                    break;
                }

                MelonLogger.Msg($"[CoverDiag] uid='{key}' cover='{coverName}'");
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[CoverDiag] 커버 로깅 실패: {ex}");
            }
        }
    }
}
