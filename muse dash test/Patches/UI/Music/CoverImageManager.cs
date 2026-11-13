using Il2CppAssets.Scripts.UI.Panels.PnlMusicTag;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace muse_dash_test
{
    /// <summary>
    /// 곡 폴더의 cover.png를 읽어 Sprite로 디코딩하고 UID별로 LRU(Least Recently Used) 캐싱하는 매니저입니다.
    /// 커스텀 곡이 수백 개일 때 비압축 Texture2D가 무제한 누적되어 VRAM/RAM이 고갈되는 것을 방지하기 위해
    /// 최대 <see cref="MaxCacheEntries"/>개까지만 유지하고 가장 오래 쓰이지 않은 Sprite/Texture2D를 명시적으로 Destroy합니다.
    /// 파일이 없거나 디코딩에 실패한 UID는 재시도하지 않도록 별도로 기록해 I/O를 줄입니다.
    /// </summary>
    public static class CoverImageManager
    {
        private const int MaxCacheEntries = 64;

        private sealed class CacheEntry
        {
            public string Uid;
            public Sprite Sprite;
            public Texture2D Texture;
            public LinkedListNode<CacheEntry> Node;
        }

        private static readonly Dictionary<string, CacheEntry> cache = new Dictionary<string, CacheEntry>(StringComparer.Ordinal);
        private static readonly LinkedList<CacheEntry> lruList = new LinkedList<CacheEntry>();
        private static readonly HashSet<string> missing = new HashSet<string>(StringComparer.Ordinal);

        public static bool TryGetCoverSprite(string uid, out Sprite sprite)
        {
            sprite = null;
            if (string.IsNullOrEmpty(uid)) return false;

            if (cache.TryGetValue(uid, out var existing))
            {
                if (existing.Sprite != null && existing.Texture != null)
                {
                    if (existing.Node != null && existing.Node.List == lruList && lruList.First != existing.Node)
                    {
                        lruList.Remove(existing.Node);
                        lruList.AddFirst(existing.Node);
                    }
                    sprite = existing.Sprite;
                    return true;
                }

                // 외부에서 파괴된 경우 캐시에서 제거 후 재로드 허용
                if (existing.Node != null && existing.Node.List == lruList)
                {
                    lruList.Remove(existing.Node);
                }
                cache.Remove(uid);
            }

            if (missing.Contains(uid)) return false;

            if (!HwaResourceManager.TryGetSongDirectory(uid, out string songDir) || string.IsNullOrEmpty(songDir))
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
                    ModLogger.Error($"[Cover] cover.png 디코딩 실패: {coverPath}");
                    UnityEngine.Object.Destroy(tex); // 디코딩 실패한 텍스처가 새지 않도록 즉시 해제
                    missing.Add(uid);
                    return false;
                }

                EvictOldestIfNeeded();

                tex.name = $"CustomCoverTex_{uid}";
                tex.hideFlags |= HideFlags.DontUnloadUnusedAsset; // 유니티 GC 방지

                var spr = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
                spr.name = $"CustomCoverSprite_{uid}";
                spr.hideFlags |= HideFlags.DontUnloadUnusedAsset;

                var entry = new CacheEntry
                {
                    Uid = uid,
                    Sprite = spr,
                    Texture = tex
                };
                entry.Node = lruList.AddFirst(entry);
                cache[uid] = entry;

                sprite = spr;
                ModLogger.Verbose($"[Cover] cover.png 로드 성공 uid='{uid}' {tex.width}x{tex.height} (캐시={cache.Count}/{MaxCacheEntries}) path={coverPath}");
                return true;
            }
            catch (Exception ex)
            {
                ModLogger.Error($"[Cover] cover.png 로드 예외 uid='{uid}': {ex}");
                missing.Add(uid);
                return false;
            }
        }

        private static void EvictOldestIfNeeded()
        {
            while (cache.Count >= MaxCacheEntries && lruList.Last != null)
            {
                var oldest = lruList.Last.Value;
                lruList.RemoveLast();

                if (oldest == null) continue;
                cache.Remove(oldest.Uid);

                try
                {
                    if (oldest.Sprite != null)
                    {
                        UnityEngine.Object.Destroy(oldest.Sprite);
                    }
                    if (oldest.Texture != null)
                    {
                        UnityEngine.Object.Destroy(oldest.Texture);
                    }
                    ModLogger.Verbose($"[Cover.LRU] 캐시 상한({MaxCacheEntries}) 도달로 오래된 커버 해제: uid='{oldest.Uid}'");
                }
                catch (Exception ex)
                {
                    ModLogger.Verbose($"[Cover.LRU] 커버 해제 중 무시된 예외: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// 커버 이미지 캐시(LRU)와 실패 목록을 모두 비우고, 생성된 모든 Sprite 및 Texture2D 네이티브 리소스를 파괴합니다.
        /// </summary>
        public static void ClearCache()
        {
            try
            {
                while (lruList.First != null)
                {
                    var entry = lruList.First.Value;
                    lruList.RemoveFirst();
                    if (entry == null) continue;

                    if (entry.Sprite != null) UnityEngine.Object.Destroy(entry.Sprite);
                    if (entry.Texture != null) UnityEngine.Object.Destroy(entry.Texture);
                }
                cache.Clear();
                missing.Clear();
                MusicCellImageDiagnostics.Reset();
                ModLogger.Verbose("[Cover.LRU] 커버 이미지 캐시 및 네이티브 텍스처 리소스를 전면 초기화했습니다.");
            }
            catch (Exception ex)
            {
                ModLogger.Warning($"[Cover.LRU] 캐시 초기화 중 예외: {ex.Message}");
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
        public static void Reset() => loggedUids.Clear();

        public static void LogCellImagesOnce(MusicButtonCell cell, string uid)
        {
            // 순수 관찰용입니다. 아래 GetComponentsInChildren는 셀마다 배열을 새로 할당하는데,
            // 곡 목록을 스크롤하면 셀이 재활용되며 계속 불립니다. 찍지 않을 거면 훑지도 않습니다.
            if (!ModLogger.IsLevelEnabled(ModLogLevel.Verbose)) return;

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

                ModLogger.Verbose($"[CoverDiag] uid='{key}' cover='{coverName}'");
            }
            catch (Exception ex)
            {
                ModLogger.Error($"[CoverDiag] 커버 로깅 실패: {ex}");
            }
        }
    }
}
