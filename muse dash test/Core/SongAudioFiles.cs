using System;
using System.IO;

namespace muse_dash_test
{
    /// <summary>
    /// 곡 폴더에서 BGM으로 쓸 .ogg 하나를 고르는 규칙입니다.
    /// 곡 선택 화면 미리듣기(<c>HwaMenuBgmController</c>)와 배틀 BGM(<c>HwaBattleMediaController</c>)이
    /// 이 한 곳에서만 고르므로, 두 화면은 항상 같은 파일을 틉니다.
    ///
    /// <para>예전에는 두 곳에 규칙이 따로 있었습니다. 메뉴 쪽만 <c>demo</c>를 키워드로 보고
    /// 설정 txt와의 이름 일치는 보지 않아서, 곡 폴더에 ogg가 여러 개면 미리듣기와 배틀이
    /// 서로 다른 파일을 틀 수 있었습니다.</para>
    ///
    /// <para>우선순위(이름 비교는 대소문자 무시):
    /// 1) 설정 txt와 이름(확장자 제외)이 같은 ogg,
    /// 2) 이름에 bgm/battle/music/song이 들어간 ogg,
    /// 3) 이름순 첫 ogg.</para>
    /// </summary>
    public static class SongAudioFiles
    {
        private static readonly string[] BgmKeywords = { "bgm", "battle", "music", "song" };

        /// <summary>
        /// 같은 곡 폴더를 다시 탐색하지 않는 유예 시간(ms). 곡 선택 휠을 오가거나 미리듣기→배틀로 넘어갈 때마다
        /// <c>.ogg</c>/<c>.txt</c> 재귀 스캔 2번이 반복되던 것을 줄입니다. 캐시된 경로는 쓰기 전에 파일이 아직
        /// 있는지 확인하고, 유예가 지나면 다시 스캔하므로 폴더를 고쳐도 곧 반영됩니다.
        /// </summary>
        internal const long CacheTtlMs = 30_000;

        private static readonly System.Collections.Generic.Dictionary<string, (string path, long tickMs)> ResolveCache =
            new System.Collections.Generic.Dictionary<string, (string path, long tickMs)>(StringComparer.OrdinalIgnoreCase);

        /// <summary>캐시를 비웁니다(차트 폴더 재로드 등에서 즉시 반영하고 싶을 때).</summary>
        public static void ClearCache()
        {
            lock (ResolveCache) ResolveCache.Clear();
        }

        /// <summary>곡 폴더(하위 폴더 포함)에서 BGM ogg 경로를 찾습니다. 없거나 읽지 못하면 null입니다.</summary>
        public static string ResolveBgmOggPath(string songDir)
        {
            try
            {
                if (string.IsNullOrEmpty(songDir) || !Directory.Exists(songDir))
                {
                    return null;
                }

                long now = Environment.TickCount64;
                lock (ResolveCache)
                {
                    if (ResolveCache.TryGetValue(songDir, out var hit)
                        && now - hit.tickMs < CacheTtlMs
                        && hit.path != null && File.Exists(hit.path))
                    {
                        return hit.path;
                    }
                }

                string[] oggFiles = Directory.GetFiles(songDir, "*.ogg", SearchOption.AllDirectories);
                if (oggFiles.Length == 0)
                {
                    return null;
                }

                string[] txtFiles = Directory.GetFiles(songDir, "*.txt", SearchOption.AllDirectories);
                string picked = PickBgmOgg(oggFiles, txtFiles);
                lock (ResolveCache) ResolveCache[songDir] = (picked, now);
                return picked;
            }
            catch (Exception ex)
            {
                ModLogger.Error($"[SongAudioFiles] ogg 탐색 실패: folder={songDir}, {ex}");
                return null;
            }
        }

        /// <summary>
        /// 후보 목록에서 BGM ogg를 고릅니다(파일 시스템을 건드리지 않는 순수 규칙).
        /// 넘겨받은 배열은 수정하지 않습니다.
        /// </summary>
        public static string PickBgmOgg(string[] oggFiles, string[] txtFiles)
        {
            if (oggFiles == null || oggFiles.Length == 0)
            {
                return null;
            }

            string[] sortedOgg = SortedCopy(oggFiles);

            if (txtFiles != null && txtFiles.Length > 0)
            {
                foreach (string txtFile in SortedCopy(txtFiles))
                {
                    string stem = Path.GetFileNameWithoutExtension(txtFile);
                    foreach (string oggFile in sortedOgg)
                    {
                        if (string.Equals(Path.GetFileNameWithoutExtension(oggFile), stem, StringComparison.OrdinalIgnoreCase))
                        {
                            return oggFile;
                        }
                    }
                }
            }

            foreach (string oggFile in sortedOgg)
            {
                string lower = Path.GetFileNameWithoutExtension(oggFile).ToLowerInvariant();
                foreach (string keyword in BgmKeywords)
                {
                    if (lower.Contains(keyword))
                    {
                        return oggFile;
                    }
                }
            }

            return sortedOgg[0];
        }

        private static string[] SortedCopy(string[] source)
        {
            var copy = (string[])source.Clone();
            Array.Sort(copy, StringComparer.OrdinalIgnoreCase);
            return copy;
        }
    }
}
