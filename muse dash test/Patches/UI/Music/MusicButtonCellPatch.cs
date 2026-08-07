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

    // MusicButtonCell.InitMusicCell 후킹 - 초고속 Zero-Allocation 커버 교체 최적화
    [HarmonyPatch(typeof(MusicButtonCell), nameof(MusicButtonCell.InitMusicCell), new Type[] { typeof(MusicInfo), typeof(int) })]
    public class MusicButtonCell_InitMusicCell_Patch
    {
        // 스크롤 시 GetComponentsInChildren 배열 할당 및 유니티 계층 탐색 렉을 방지하기 위한 컴포넌트 캐시
        private static readonly Dictionary<int, Image> cellImageCache = new Dictionary<int, Image>();

        public static bool Prepare() => true;

        public static void Prefix(MusicButtonCell __instance, MusicInfo initMusicInfo, int tabIndex)
        {
        }

        public static void Postfix(MusicButtonCell __instance, MusicInfo initMusicInfo, int tabIndex)
        {
            try
            {
                if (__instance == null || initMusicInfo == null) return;

                // 가상 곡이 아니면 처리 건너뜀
                if (!CustomContentIds.IsVirtualSong(initMusicInfo.uid)) return;

                // 곡 폴더의 cover.png를 셀의 ImgCover에 초고속 주입
                ApplyCustomCoverOptimized(__instance.gameObject, initMusicInfo.uid);
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"MusicButtonCell.InitMusicCell Postfix 예외: {ex}");
            }
        }

        private static void ApplyCustomCoverOptimized(GameObject cellGo, string uid)
        {
            if (cellGo == null) return;
            if (!CoverImageManager.TryGetCoverSprite(uid, out var coverSprite) || coverSprite == null) return;

            int instanceId = cellGo.GetInstanceID();
            if (!cellImageCache.TryGetValue(instanceId, out Image targetImg) || targetImg == null)
            {
                // GetComponentsInChildren 대신 Direct Child Find로 1회만 단일 컴포넌트 탐색
                Transform coverTransform = cellGo.transform.Find("ImgCover");
                if (coverTransform != null)
                {
                    targetImg = coverTransform.GetComponent<Image>();
                }
                
                // Fallback: 자식 탐색 실패 시 1회만 Single GetComponentInChildren
                if (targetImg == null)
                {
                    targetImg = cellGo.GetComponentInChildren<Image>();
                }

                if (targetImg != null)
                {
                    cellImageCache[instanceId] = targetImg;
                }
            }

            if (targetImg != null && targetImg.sprite != coverSprite)
            {
                targetImg.sprite = coverSprite;
                MelonLogger.Msg($"⚡ [Cover.Fast] 곡 셀 ImgCover 커버 스프라이트 초고속 교체 완료: uid='{uid}'");
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
                MelonLogger.Msg($"[Cover] cover.png 파일 로드 성공: uid='{uid}', path='{coverPath}'");
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
