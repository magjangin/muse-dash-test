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
                    var v = f.GetValue(pnlInstance); if (v == null) continue; var lname = f.Name.ToLowerInvariant();
                    if (title == null && (lname.Contains("song") || lname.Contains("title") || lname.Contains("music") || lname.Contains("name"))) title = SafeGetProp(v, "text") ?? SafeGetProp(v, "m_Text") ?? SafeGetProp(v, "name") ?? (v as string);
                    if (artist == null && lname.Contains("artist")) artist = SafeGetProp(v, "text") ?? SafeGetProp(v, "m_Text") ?? SafeGetProp(v, "name") ?? (v as string);
                    if (album == null && lname.Contains("album")) album = SafeGetProp(v, "text") ?? SafeGetProp(v, "m_Text") ?? SafeGetProp(v, "name") ?? (v as string);
                }
                catch { }
            }
            foreach (var p in t.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                try
                {
                    if (p.GetIndexParameters().Length > 0) continue; var v = p.GetValue(pnlInstance); if (v == null) continue; var lname = p.Name.ToLowerInvariant();
                    if (title == null && (lname.Contains("song") || lname.Contains("title") || lname.Contains("music") || lname.Contains("name"))) title = SafeGetProp(v, "text") ?? SafeGetProp(v, "m_Text") ?? SafeGetProp(v, "name") ?? (v as string);
                    if (artist == null && lname.Contains("artist")) artist = SafeGetProp(v, "text") ?? SafeGetProp(v, "m_Text") ?? SafeGetProp(v, "name") ?? (v as string);
                    if (album == null && lname.Contains("album")) album = SafeGetProp(v, "text") ?? SafeGetProp(v, "m_Text") ?? SafeGetProp(v, "name") ?? (v as string);
                }
                catch { }
            }
            if (string.IsNullOrEmpty(title) && string.IsNullOrEmpty(artist) && string.IsNullOrEmpty(album)) return false;
            MelonLogger.Msg($"NowPlaying: {(string.IsNullOrEmpty(title) ? "(unknown)" : title)} - {(string.IsNullOrEmpty(artist) ? "(unknown)" : artist)} - {(string.IsNullOrEmpty(album) ? "(unknown)" : album)}");
            return true;
        }
        catch { return false; }
    }
}