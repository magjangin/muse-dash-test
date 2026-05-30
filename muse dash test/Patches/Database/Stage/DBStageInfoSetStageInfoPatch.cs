using MelonLoader;
using System;
using Il2CppAssets.Scripts.Database;
using System.Reflection;
using System.Collections;

// DBStageInfo.SetStageInfo(StageInfo info) 후킹
[HarmonyLib.HarmonyPatch(typeof(DBStageInfo), "SetStageInfo")]
public class DBStageInfo_SetStageInfo_Patch
{
    public static void Prefix(DBStageInfo __instance, object info)
    {
        try { }
        catch (Exception ex) { MelonLogger.Error($"DBStageInfo.SetStageInfo Prefix 예외: {ex}"); }
    }

    public static void Postfix(DBStageInfo __instance, object info)
    {
        try { }
        catch (Exception ex) { MelonLogger.Error($"DBStageInfo.SetStageInfo Postfix 예외: {ex}"); }
    }

    private static void DumpStageInfo(object info)
    {
        if (info == null)
        {
            MelonLogger.Msg("DumpStageInfo: info is null");
            return;
        }

        try
        {
            var t = info.GetType();
            MelonLogger.Msg($"DumpStageInfo: Type={t.FullName}");

            // Fields
            var fields = t.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var f in fields)
            {
                try
                {
                    var val = f.GetValue(info);
                    MelonLogger.Msg($"  Field {f.Name}: {SafeVal(val)}");
                }
                catch (Exception ex)
                {
                    MelonLogger.Msg($"  Field {f.Name}: (예외: {ex.Message})");
                }
            }

            // Properties
            var props = t.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var p in props)
            {
                if (p.GetIndexParameters().Length > 0) continue; // skip indexers
                try
                {
                    var val = p.GetValue(info);
                    MelonLogger.Msg($"  Prop  {p.Name}: {SafeVal(val)}");
                }
                catch (Exception ex)
                {
                    MelonLogger.Msg($"  Prop  {p.Name}: (예외: {ex.Message})");
                }
            }
        }
        catch (Exception ex)
        {
            MelonLogger.Error($"DumpStageInfo 예외: {ex}");
        }
    }

    private static string SafeVal(object val)
    {
        try
        {
            if (val == null) return "(null)";
            if (val is string) return (string)val;
            if (val.GetType().IsPrimitive) return val.ToString();
            if (val is IEnumerable && !(val is string))
            {
                var sb = new System.Text.StringBuilder();
                sb.Append("[Enumerable]");
                int i = 0;
                foreach (var e in (IEnumerable)val)
                {
                    if (i >= 5) { sb.Append(", ..."); break; }
                    sb.Append(" "); sb.Append(e != null ? e.ToString() : "(null)");
                    i++;
                }
                return sb.ToString();
            }
            return val.ToString();
        }
        catch (Exception ex)
        {
            return $"(SafeVal 예외: {ex.Message})";
        }
    }
}
