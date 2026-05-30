using MelonLoader;
using System;
using System.Text;
using Il2CppAssets.Scripts.Database;

public static partial class PnlStagePatchHelper
{
    public static bool TryFindMusicInfoByQuery(string query, out MusicInfo musicInfo, out string matchedUid)
    {
        musicInfo = null;
        matchedUid = null;

        try
        {
            var allMusicInfo = GlobalDataBase.dbMusicTag?.m_AllMusicInfo;
            if (allMusicInfo == null)
            {
                return false;
            }

            string normalizedQuery = NormalizeMusicSearchText(query);
            bool hasQuery = !string.IsNullOrEmpty(normalizedQuery);

            MusicInfo firstMusicInfo = null;
            string firstUid = null;
            int bestScore = -1;

            foreach (var entry in allMusicInfo)
            {
                if (entry.Value == null)
                {
                    continue;
                }

                if (firstMusicInfo == null)
                {
                    firstMusicInfo = entry.Value;
                    firstUid = entry.Key;
                }

                if (!hasQuery)
                {
                    continue;
                }

                int score = ScoreMusicSearchMatch(entry.Key, entry.Value, normalizedQuery);
                if (score > bestScore)
                {
                    bestScore = score;
                    musicInfo = entry.Value;
                    matchedUid = entry.Key;
                }
            }

            if (!hasQuery)
            {
                if (firstMusicInfo != null)
                {
                    musicInfo = firstMusicInfo;
                    matchedUid = firstUid;
                    return true;
                }

                return false;
            }

            if (bestScore < 0 || musicInfo == null)
            {
                return false;
            }

            return true;
        }
        catch (Exception ex)
        {
            MelonLogger.Error($"TryFindMusicInfoByQuery 예외: {ex}");
            musicInfo = null;
            matchedUid = null;
            return false;
        }
    }

    private static int ScoreMusicSearchMatch(string uid, MusicInfo musicInfo, string normalizedQuery)
    {
        if (musicInfo == null)
        {
            return -1;
        }

        string normalizedUid = NormalizeMusicSearchText(uid);
        if (!string.IsNullOrEmpty(normalizedUid) && normalizedUid == normalizedQuery)
        {
            return 300;
        }

        string normalizedName = NormalizeMusicSearchText(musicInfo.name);
        if (!string.IsNullOrEmpty(normalizedName) && normalizedName == normalizedQuery)
        {
            return 250;
        }

        string normalizedAuthor = NormalizeMusicSearchText(musicInfo.author);
        if (!string.IsNullOrEmpty(normalizedAuthor) && normalizedAuthor == normalizedQuery)
        {
            return 240;
        }

        string normalizedDesigner = NormalizeMusicSearchText(musicInfo.levelDesigner);
        if (!string.IsNullOrEmpty(normalizedDesigner) && normalizedDesigner == normalizedQuery)
        {
            return 230;
        }

        if (!string.IsNullOrEmpty(normalizedUid) && normalizedUid.IndexOf(normalizedQuery, StringComparison.OrdinalIgnoreCase) >= 0)
        {
            return 180;
        }

        if (!string.IsNullOrEmpty(normalizedName) && normalizedName.IndexOf(normalizedQuery, StringComparison.OrdinalIgnoreCase) >= 0)
        {
            return 170;
        }

        if (!string.IsNullOrEmpty(normalizedAuthor) && normalizedAuthor.IndexOf(normalizedQuery, StringComparison.OrdinalIgnoreCase) >= 0)
        {
            return 160;
        }

        if (!string.IsNullOrEmpty(normalizedDesigner) && normalizedDesigner.IndexOf(normalizedQuery, StringComparison.OrdinalIgnoreCase) >= 0)
        {
            return 150;
        }

        return -1;
    }

    private static string NormalizeMusicSearchText(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return string.Empty;
        }

        var builder = new StringBuilder(text.Length);
        foreach (char ch in text)
        {
            if (char.IsWhiteSpace(ch) || ch == '-' || ch == '_' || ch == '/' || ch == '·' || ch == '.')
            {
                continue;
            }

            builder.Append(char.ToLowerInvariant(ch));
        }

        return builder.ToString();
    }
}
