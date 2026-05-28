using HarmonyLib;
using Il2CppAssets.Scripts.Database;
using Il2CppAssets.Scripts.Database.DataClass;
using MelonLoader;
using System;
using System.Text;
using Il2CppStringList = Il2CppSystem.Collections.Generic.List<string>;

namespace muse_dash_test
{
    internal static class UidRewriteExperiment
    {
        public const string SourceUid = "0-0";
        public const string TargetUid = "999-0";

        public static string MusicInfo(MusicInfo info)
        {
            if (info == null)
            {
                return "(null)";
            }

            try
            {
                return $"uid={info.uid ?? "(null)"}, name={info.name ?? "(null)"}, cover={info.cover ?? "(null)"}, hexieCover={info.hexieCover ?? "(null)"}, coverName={info.coverName ?? "(null)"}";
            }
            catch (Exception ex)
            {
                return $"(music info read error: {ex.Message})";
            }
        }

        public static string StringList(Il2CppStringList list)
        {
            if (list == null)
            {
                return "(null)";
            }

            try
            {
                int take = Math.Min(list.Count, 6);
                var values = new string[take];
                for (int i = 0; i < take; i++)
                {
                    values[i] = list[i] ?? "(null)";
                }

                string suffix = list.Count > take ? ", ..." : string.Empty;
                return $"count={list.Count}, values=[{string.Join(", ", values)}{suffix}]";
            }
            catch (Exception ex)
            {
                return $"(list read error: {ex.Message})";
            }
        }

        public static string PropertyDump(MusicInfo info)
        {
            if (info == null)
            {
                return "(null)";
            }

            try
            {
                var sb = new StringBuilder();
                var props = info.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                foreach (var prop in props)
                {
                    if (!prop.CanRead || prop.GetIndexParameters().Length != 0)
                    {
                        continue;
                    }

                    Type propType = prop.PropertyType;
                    if (propType != typeof(string) && propType != typeof(int) && propType != typeof(bool) && propType != typeof(float) && propType != typeof(double))
                    {
                        continue;
                    }

                    object value;
                    try
                    {
                        value = prop.GetValue(info);
                    }
                    catch (Exception ex)
                    {
                        value = "(error: " + ex.Message + ")";
                    }

                    if (sb.Length > 0)
                    {
                        sb.Append(", ");
                    }
                    sb.Append(prop.Name);
                    sb.Append('=');
                    sb.Append(value ?? "(null)");
                }

                return sb.Length == 0 ? "(no scalar properties)" : sb.ToString();
            }
            catch (Exception ex)
            {
                return "(property dump error: " + ex.Message + ")";
            }
        }
    }

    [HarmonyPatch(typeof(DBMusicTag), nameof(DBMusicTag.GetMusicInfoFromShowMusicUids))]
    internal static class DBMusicTag_GetMusicInfoFromShowMusicUids_ObservePatch
    {
        private static void Postfix(int index, MusicInfo __result)
        {
            try
            {
                MelonLogger.Msg($"[DBMusicTag.GetMusicInfoFromShowMusicUids.Postfix] index={index}, result={UidRewriteExperiment.MusicInfo(__result)}");
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"DBMusicTag.GetMusicInfoFromShowMusicUids Postfix 예외: {ex}");
            }
        }
    }

}
