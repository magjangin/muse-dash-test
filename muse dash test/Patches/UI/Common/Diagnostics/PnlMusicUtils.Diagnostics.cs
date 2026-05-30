using MelonLoader;
using muse_dash_test;
using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class PnlMusicUtils
{
    private static string DescribeMusicObject(object o)
    {
        if (o == null) return "(null)";
        try
        {
            var ty = o.GetType();
            try { var tprop = ty.GetProperty("text", BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic); if (tprop != null) { var txt = tprop.GetValue(o) as string; if (!string.IsNullOrEmpty(txt)) return $"{ty.Name}: title={txt}"; } } catch { }

            string title = SafeGetProp(o, "title") ?? SafeGetProp(o, "name") ?? SafeGetProp(o, "musicName") ?? SafeGetProp(o, "songName");
            string album = SafeGetProp(o, "album") ?? SafeGetProp(o, "albumName");
            string bms = SafeGetProp(o, "m_BmsUid") ?? SafeGetProp(o, "bmsUid") ?? SafeGetProp(o, "ibms_id");
            if (!string.IsNullOrEmpty(title) || !string.IsNullOrEmpty(album) || !string.IsNullOrEmpty(bms)) return $"{ty.Name}: title={title ?? "(null)"}, album={album ?? "(null)"}, bms={bms ?? "(null)"}";

            var candidates = new List<string>(); CollectStringValues(o, candidates, 0, 2);
            string best = null;
            foreach (var s in candidates)
            {
                if (string.IsNullOrWhiteSpace(s)) continue; var sTrim = s.Trim(); if (sTrim.Length <= 1) continue; if (sTrim.StartsWith("Img") || sTrim.StartsWith("Pnl") || sTrim.StartsWith("sfx_")) continue; if (best == null) best = sTrim; else if (sTrim.Length > best.Length) best = sTrim;
            }
            if (!string.IsNullOrEmpty(best)) return $"{ty.Name}: title={best}";
            return ty.FullName + " - " + o.ToString();
        }
        catch (Exception ex) { return "(DescribeMusicObject 예외: " + ex.Message + ")"; }
    }

    private static void CollectStringValues(object obj, List<string> outList, int depth, int maxDepth)
    {
        if (obj == null || depth > maxDepth) return;
        try
        {
            var t = obj.GetType();
            foreach (var p in t.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                try
                {
                    if (p.GetIndexParameters().Length > 0) continue;
                    var val = p.GetValue(obj); if (val == null) continue;
                    if (val is string s) outList.Add(s);
                    else if (!(val is ValueType) && !(val is IEnumerable)) CollectStringValues(val, outList, depth + 1, maxDepth);
                    else if (val is IEnumerable ie && !(val is string)) { int i = 0; foreach (var it in ie) { if (i++ > 10) break; if (it is string ss) outList.Add(ss); } }
                }
                catch { }
            }
            foreach (var f in t.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                try { var val = f.GetValue(obj); if (val == null) continue; if (val is string s) outList.Add(s); else if (!(val is ValueType) && !(val is IEnumerable)) CollectStringValues(val, outList, depth + 1, maxDepth); } catch { }
            }
        }
        catch { }
    }

    private static void DumpMusicInfoVerbose(object obj)
    {
        try
        {
            if (obj == null) { MelonLogger.Msg("DumpMusicInfoVerbose: null"); return; }
            var t = obj.GetType(); MelonLogger.Msg($"--- DumpMusicInfoVerbose: {t.FullName} ---");
            foreach (var f in t.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                try { var v = f.GetValue(obj); MelonLogger.Msg($"Field: {f.Name} (Type={f.FieldType.Name}) = {(v != null ? v.ToString() : "(null)")}"); if (v != null) LogStringProps(v, "  "); } catch (Exception ex) { MelonLogger.Msg($"Field {f.Name} read error: {ex.Message}"); }
            }
            foreach (var p in t.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                try { if (p.GetIndexParameters().Length > 0) { MelonLogger.Msg($"Property: {p.Name} (indexed) skipped"); continue; } var v = p.GetValue(obj); MelonLogger.Msg($"Property: {p.Name} (Type={p.PropertyType.Name}) = {(v != null ? v.ToString() : "(null)")}"); if (v != null) LogStringProps(v, "  "); } catch (Exception ex) { MelonLogger.Msg($"Property {p.Name} read error: {ex.Message}"); }
            }
            int childCount = 0;
            foreach (var f in t.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                try
                {
                    var v = f.GetValue(obj); if (v == null) continue; var vt = v.GetType(); if (vt == typeof(string) || vt.IsPrimitive) continue; if (++childCount > 6) break; MelonLogger.Msg($"-- Child field {f.Name} type {vt.FullName} --");
                    foreach (var p in vt.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
                    {
                        try { if (p.GetIndexParameters().Length > 0) continue; var pv = p.GetValue(v); if (pv is string) MelonLogger.Msg($"   {p.Name} = {pv}"); } catch { }
                    }
                }
                catch { }
            }
            MelonLogger.Msg($"--- End DumpMusicInfoVerbose ---");
        }
        catch (Exception ex) { MelonLogger.Msg($"DumpMusicInfoVerbose exception: {ex}"); }
    }

    private static void LogStringProps(object v, string indent)
    {
        try
        {
            var vt = v.GetType();
            var textProp = vt.GetProperty("text", BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
            if (textProp != null) { try { var tv = textProp.GetValue(v) as string; if (!string.IsNullOrEmpty(tv)) MelonLogger.Msg($"{indent}text: {tv}"); } catch { } }
            try { var nameProp = vt.GetProperty("name", BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic); if (nameProp != null) { var nv = nameProp.GetValue(v) as string; if (!string.IsNullOrEmpty(nv)) MelonLogger.Msg($"{indent}name: {nv}"); } } catch { }
            var tname = vt.Name.ToLowerInvariant();
            if (tname.Contains("textmeshpro") || tname.Contains("textmesh") || tname.Contains("textui") || tname.Contains("text"))
            {
                foreach (var p in vt.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
                {
                    try { if (p.GetIndexParameters().Length > 0) continue; if (p.PropertyType == typeof(string)) { var sval = p.GetValue(v) as string; if (!string.IsNullOrEmpty(sval)) MelonLogger.Msg($"{indent}{p.Name}: {sval}"); } } catch { }
                }
            }
            int found = 0;
            foreach (var p in vt.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                try { if (p.GetIndexParameters().Length > 0) continue; var pv = p.GetValue(v); if (pv is string s && !string.IsNullOrEmpty(s)) { MelonLogger.Msg($"{indent}{p.Name}: {s}"); if (++found > 6) break; } } catch { }
            }
        }
        catch (Exception ex) { MelonLogger.Msg($"LogStringProps exception: {ex.Message}"); }
    }

    private static string SafeGetProp(object o, string propName)
    {
        try
        {
            if (o == null) return null;
            object v = ModReflection.GetValue(o, propName, silent: true);
            if (v == null) return null;
            if (v is string s) return s;

            string tv = ModReflection.GetValue(v, "text", silent: true) as string;
            if (!string.IsNullOrEmpty(tv)) return tv;
            return v.ToString();
        }
        catch { }
        return null;
    }
}
