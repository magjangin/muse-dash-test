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

    [HarmonyPatch(typeof(AlbumTagInfo), nameof(AlbumTagInfo.GetMusicUids))]
    internal static class AlbumTagInfo_GetMusicUids_ObservePatch
    {
        private static void Postfix(AlbumTagInfo __instance, Il2CppStringList __0, bool __1)
        {
            if (__instance?.tagUid == "tag-muse-dash-test")
            {
                MelonLogger.Msg($"[UidObserve] AlbumTagInfo.GetMusicUids tagUid={__instance.tagUid}, clear={__1}, result={UidRewriteExperiment.StringList(__0)}");
            }
        }
    }

    [HarmonyPatch(typeof(DBMusicTag), nameof(DBMusicTag.GetMusicInfoFromShowMusicUids))]
    internal static class DBMusicTag_GetMusicInfoFromShowMusicUids_ObservePatch
    {
        private static void Postfix(int index, MusicInfo __result)
        {
            bool shouldLog = index <= 3 || (__result != null && (__result.uid == UidRewriteExperiment.SourceUid || __result.uid == UidRewriteExperiment.TargetUid));

            if (shouldLog)
            {
                MelonLogger.Msg($"[UidObserve] DBMusicTag.GetMusicInfoFromShowMusicUids index={index}, result={UidRewriteExperiment.MusicInfo(__result)}");
                MelonLogger.Msg($"[MusicInfoProps] index={index}, props={UidRewriteExperiment.PropertyDump(__result)}");
            }
        }
    }

    [HarmonyPatch(typeof(MusicInfo), nameof(MusicInfo.uid), MethodType.Getter)]
    internal static class MusicInfo_Uid_Patch
    {
        public static bool Prepare()
        {
            MelonLogger.Msg("[MusicInfo.get_uid] 접근자 후킹 준비 완료");
            return true;
        }

        [HarmonyPostfix]
        private static void Postfix(MusicInfo __instance, ref string __result)
        {
            if (__instance == null) return;
            MelonLogger.Msg($"[MusicInfo.uid Getter Hook] Instance: {__instance.name} ({__instance.musicName}), uid: {__result ?? "(null)"}");
        }
    }

    [HarmonyPatch(typeof(MusicInfo), nameof(MusicInfo.uid), MethodType.Setter)]
    internal static class MusicInfo_UidSetter_Patch
    {
        public static bool Prepare()
        {
            MelonLogger.Msg("[MusicInfo.set_uid] 접근자 후킹 준비 완료");
            return true;
        }

        [HarmonyPrefix]
        private static bool Prefix(MusicInfo __instance, ref string __0)
        {
            if (__instance == null) return true;
            MelonLogger.Msg($"[MusicInfo.uid Setter Hook] Instance: {__instance.name} ({__instance.musicName}), setting uid to: {__0 ?? "(null)"}");
            return true;
        }
    }
}
