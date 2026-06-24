using MelonLoader;
using System.Reflection;

public static partial class PnlMusicUtils
{
    private static bool TryLogCompact(object pnlInstance)
    {
        try
        {
            if (pnlInstance == null) return false;
            var t = pnlInstance.GetType();
            string title = null, artist = null, album = null;
            foreach (var f in t.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                try
                {
                    var v = f.GetValue(pnlInstance);
                    if (v == null) continue;
                    MatchNowPlayingMember(f.Name, v, ref title, ref artist, ref album);
                }
                catch { }
            }
            foreach (var p in t.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                try
                {
                    if (p.GetIndexParameters().Length > 0) continue;
                    var v = p.GetValue(pnlInstance);
                    if (v == null) continue;
                    MatchNowPlayingMember(p.Name, v, ref title, ref artist, ref album);
                }
                catch { }
            }
            if (string.IsNullOrEmpty(title) && string.IsNullOrEmpty(artist) && string.IsNullOrEmpty(album)) return false;
            MelonLogger.Msg($"NowPlaying: {(string.IsNullOrEmpty(title) ? "(unknown)" : title)} - {(string.IsNullOrEmpty(artist) ? "(unknown)" : artist)} - {(string.IsNullOrEmpty(album) ? "(unknown)" : album)}");
            return true;
        }
        catch { return false; }
    }

    private static void MatchNowPlayingMember(string memberName, object value, ref string title, ref string artist, ref string album)
    {
        var lname = memberName.ToLowerInvariant();
        string text = null;

        if (title == null && (lname.Contains("song") || lname.Contains("title") || lname.Contains("music") || lname.Contains("name")))
        {
            text = ReadNowPlayingText(value);
            title = text;
        }

        if (artist == null && lname.Contains("artist"))
        {
            artist = text ?? ReadNowPlayingText(value);
        }

        if (album == null && lname.Contains("album"))
        {
            album = text ?? ReadNowPlayingText(value);
        }
    }

    private static string ReadNowPlayingText(object value)
    {
        return SafeGetProp(value, "text") ?? SafeGetProp(value, "m_Text") ?? SafeGetProp(value, "name") ?? (value as string);
    }
}
