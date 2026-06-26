using MelonLoader;
using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

namespace muse_dash_test
{
    /// <summary>
    /// UI 패널 객체나 음악 관련 오브젝트의 멤버 정보를 사람이 읽기 쉬운 형태로 출력(덤프)해 주는 디버깅 전용 유틸리티 클래스입니다.
    /// </summary>
    public static class PnlMusicDumper
    {
        private const BindingFlags DefaultFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

        /// <summary>
        /// 객체의 멤버(텍스트, 타이틀 등)를 읽어서 간략한 문자열 묘사 정보를 반환합니다.
        /// </summary>
        public static string DescribeMusicObject(object o)
        {
            if (o == null) return "(null)";
            try
            {
                var ty = o.GetType();
                try
                {
                    var tprop = ty.GetProperty("text", DefaultFlags);
                    if (tprop != null)
                    {
                        var txt = tprop.GetValue(o) as string;
                        if (!string.IsNullOrEmpty(txt)) return $"{ty.Name}: title={txt}";
                    }
                }
                catch (Exception) { }

                string title = SafeGetProp(o, "title") ?? SafeGetProp(o, "name") ?? SafeGetProp(o, "musicName") ?? SafeGetProp(o, "songName");
                string album = SafeGetProp(o, "album") ?? SafeGetProp(o, "albumName");
                string bms = SafeGetProp(o, "m_BmsUid") ?? SafeGetProp(o, "bmsUid") ?? SafeGetProp(o, "ibms_id");
                
                if (!string.IsNullOrEmpty(title) || !string.IsNullOrEmpty(album) || !string.IsNullOrEmpty(bms))
                {
                    return $"{ty.Name}: title={title ?? "(null)"}, album={album ?? "(null)"}, bms={bms ?? "(null)"}";
                }

                var candidates = new List<string>();
                CollectStringValues(o, candidates, 0, 2);
                string best = null;
                foreach (var s in candidates)
                {
                    if (string.IsNullOrWhiteSpace(s)) continue;
                    var sTrim = s.Trim();
                    if (sTrim.Length <= 1) continue;
                    if (sTrim.StartsWith("Img") || sTrim.StartsWith("Pnl") || sTrim.StartsWith("sfx_")) continue;
                    
                    if (best == null) best = sTrim;
                    else if (sTrim.Length > best.Length) best = sTrim;
                }
                if (!string.IsNullOrEmpty(best)) return $"{ty.Name}: title={best}";
                return ty.FullName + " - " + o.ToString();
            }
            catch (Exception ex) { return "(DescribeMusicObject 예외: " + ex.Message + ")"; }
        }

        /// <summary>
        /// 객체 내부의 모든 필드와 프로퍼티 정보를 매우 상세하게 디버그 로그에 출력합니다. (재귀 1단계)
        /// </summary>
        public static void DumpMusicInfoVerbose(object obj)
        {
            try
            {
                if (obj == null)
                {
                    MelonLogger.Msg("DumpMusicInfoVerbose: null");
                    return;
                }
                var t = obj.GetType();
                MelonLogger.Msg($"--- DumpMusicInfoVerbose: {t.FullName} ---");
                
                foreach (var f in t.GetFields(DefaultFlags))
                {
                    try
                    {
                        var v = f.GetValue(obj);
                        MelonLogger.Msg($"Field: {f.Name} (Type={f.FieldType.Name}) = {(v != null ? v.ToString() : "(null)")}");
                        if (v != null) LogStringProps(v, "  ");
                    }
                    catch (Exception ex) { MelonLogger.Msg($"Field {f.Name} read error: {ex.Message}"); }
                }

                foreach (var p in t.GetProperties(DefaultFlags))
                {
                    try
                    {
                        if (p.GetIndexParameters().Length > 0)
                        {
                            MelonLogger.Msg($"Property: {p.Name} (indexed) skipped");
                            continue;
                        }
                        var v = p.GetValue(obj);
                        MelonLogger.Msg($"Property: {p.Name} (Type={p.PropertyType.Name}) = {(v != null ? v.ToString() : "(null)")}");
                        if (v != null) LogStringProps(v, "  ");
                    }
                    catch (Exception ex) { MelonLogger.Msg($"Property {p.Name} read error: {ex.Message}"); }
                }

                int childCount = 0;
                foreach (var f in t.GetFields(DefaultFlags))
                {
                    try
                    {
                        var v = f.GetValue(obj);
                        if (v == null) continue;
                        var vt = v.GetType();
                        if (vt == typeof(string) || vt.IsPrimitive) continue;
                        if (++childCount > 6) break;
                        
                        MelonLogger.Msg($"-- Child field {f.Name} type {vt.FullName} --");
                        foreach (var p in vt.GetProperties(DefaultFlags))
                        {
                            try
                            {
                                if (p.GetIndexParameters().Length > 0) continue;
                                var pv = p.GetValue(v);
                                if (pv is string) MelonLogger.Msg($"   {p.Name} = {pv}");
                            }
                            catch (Exception) { }
                        }
                    }
                    catch (Exception) { }
                }
                MelonLogger.Msg($"--- End DumpMusicInfoVerbose ---");
            }
            catch (Exception ex) { MelonLogger.Msg($"DumpMusicInfoVerbose exception: {ex}"); }
        }

        /// <summary>
        /// 곡 선택 UI 패널 등 현재 재생 중인 음악 관련 클래스 내부의 타이틀, 아티스트, 앨범 필드를 탐색하여 출력합니다.
        /// </summary>
        public static bool TryLogCompact(object pnlInstance)
        {
            try
            {
                if (pnlInstance == null) return false;
                var t = pnlInstance.GetType();
                string title = null, artist = null, album = null;
                
                foreach (var f in t.GetFields(DefaultFlags))
                {
                    try
                    {
                        var v = f.GetValue(pnlInstance);
                        if (v == null) continue;
                        MatchNowPlayingMember(f.Name, v, ref title, ref artist, ref album);
                    }
                    catch (Exception) { }
                }
                
                foreach (var p in t.GetProperties(DefaultFlags))
                {
                    try
                    {
                        if (p.GetIndexParameters().Length > 0) continue;
                        var v = p.GetValue(pnlInstance);
                        if (v == null) continue;
                        MatchNowPlayingMember(p.Name, v, ref title, ref artist, ref album);
                    }
                    catch (Exception) { }
                }

                if (string.IsNullOrEmpty(title) && string.IsNullOrEmpty(artist) && string.IsNullOrEmpty(album))
                {
                    return false;
                }

                MelonLogger.Msg($"NowPlaying: {(string.IsNullOrEmpty(title) ? "(unknown)" : title)} - {(string.IsNullOrEmpty(artist) ? "(unknown)" : artist)} - {(string.IsNullOrEmpty(album) ? "(unknown)" : album)}");
                return true;
            }
            catch { return false; }
        }

        private static void CollectStringValues(object obj, List<string> outList, int depth, int maxDepth)
        {
            if (obj == null || depth > maxDepth) return;
            try
            {
                var t = obj.GetType();
                foreach (var p in t.GetProperties(DefaultFlags))
                {
                    try
                    {
                        if (p.GetIndexParameters().Length > 0) continue;
                        var val = p.GetValue(obj);
                        if (val == null) continue;
                        
                        if (val is string s) outList.Add(s);
                        else if (!(val is ValueType) && !(val is IEnumerable)) CollectStringValues(val, outList, depth + 1, maxDepth);
                        else if (val is IEnumerable ie && !(val is string))
                        {
                            int i = 0;
                            foreach (var it in ie)
                            {
                                if (i++ > 10) break;
                                if (it is string ss) outList.Add(ss);
                            }
                        }
                    }
                    catch (Exception) { }
                }
                foreach (var f in t.GetFields(DefaultFlags))
                {
                    try
                    {
                        var val = f.GetValue(obj);
                        if (val == null) continue;
                        if (val is string s) outList.Add(s);
                        else if (!(val is ValueType) && !(val is IEnumerable)) CollectStringValues(val, outList, depth + 1, maxDepth);
                    }
                    catch (Exception) { }
                }
            }
            catch (Exception) { }
        }

        private static void LogStringProps(object v, string indent)
        {
            try
            {
                var vt = v.GetType();
                var textProp = vt.GetProperty("text", DefaultFlags);
                if (textProp != null)
                {
                    try
                    {
                        var tv = textProp.GetValue(v) as string;
                        if (!string.IsNullOrEmpty(tv)) MelonLogger.Msg($"{indent}text: {tv}");
                    }
                    catch (Exception) { }
                }

                try
                {
                    var nameProp = vt.GetProperty("name", DefaultFlags);
                    if (nameProp != null)
                    {
                        var nv = nameProp.GetValue(v) as string;
                        if (!string.IsNullOrEmpty(nv)) MelonLogger.Msg($"{indent}name: {nv}");
                    }
                }
                catch (Exception) { }

                var tname = vt.Name.ToLowerInvariant();
                if (tname.Contains("textmeshpro") || tname.Contains("textmesh") || tname.Contains("textui") || tname.Contains("text"))
                {
                    foreach (var p in vt.GetProperties(DefaultFlags))
                    {
                        try
                        {
                            if (p.GetIndexParameters().Length > 0) continue;
                            if (p.PropertyType == typeof(string))
                            {
                                var sval = p.GetValue(v) as string;
                                if (!string.IsNullOrEmpty(sval)) MelonLogger.Msg($"{indent}{p.Name}: {sval}");
                            }
                        }
                        catch (Exception) { }
                    }
                }

                int found = 0;
                foreach (var p in vt.GetProperties(DefaultFlags))
                {
                    try
                    {
                        if (p.GetIndexParameters().Length > 0) continue;
                        var pv = p.GetValue(v);
                        if (pv is string s && !string.IsNullOrEmpty(s))
                        {
                            MelonLogger.Msg($"{indent}{p.Name}: {s}");
                            if (++found > 6) break;
                        }
                    }
                    catch (Exception) { }
                }
            }
            catch (Exception ex) { MelonLogger.Msg($"LogStringProps exception: {ex.Message}"); }
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

        private static string SafeGetProp(object obj, string propName)
        {
            try
            {
                var p = obj.GetType().GetProperty(propName, DefaultFlags);
                if (p != null && p.CanRead) return p.GetValue(obj)?.ToString();
            }
            catch (Exception) { }
            return null;
        }
    }
}
