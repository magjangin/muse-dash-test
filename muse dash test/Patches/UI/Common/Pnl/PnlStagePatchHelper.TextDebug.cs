using MelonLoader;
using System;
using System.Reflection;
using System.Text;
using Il2CppAssets.Scripts.Database;
using Il2CppAssets.Scripts.UI.Panels;
using UnityEngine;
using UnityEngine.UI;
using muse_dash_test;

public static partial class PnlStagePatchHelper
{
    public static string GetLongNameControllerText(Il2Cpp.LongSongNameController controller)
    {
        if (controller == null)
        {
            return null;
        }

        return FirstNonEmpty(
            controller.m_TxtSimpleName?.text,
            controller.m_MidSimpleName?.text,
            controller.m_TxtBackupName?.text);
    }

    private static string FirstNonEmpty(params string[] values)
    {
        if (values == null) return null;
        foreach (var value in values)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                return value;
            }
        }
        return null;
    }

    private static string CleanLogText(string value)
    {
        return string.IsNullOrWhiteSpace(value) ? "(null)" : value.Trim();
    }

    public static bool IsCustomAlbumContext(int tagUid, string musicUid)
    {
        try
        {
            var db = Il2CppAssets.Scripts.Database.GlobalDataBase.dbMusicTag;
            if (db == null || db.stageShowMusicList == null)
            {
                return false;
            }

            var tag = db.GetAlbumTagInfo(tagUid);
            if (tag?.albumsInfos == null || tag.albumsInfos.Count == 0)
            {
                return false;
            }

            bool hasExpectedAlbum = false;
            for (int i = 0; i < tag.albumsInfos.Count; i++)
            {
                var album = tag.albumsInfos[i];
                if (album != null && album.uid == CustomContentIds.AlbumUid && album.title == "실험 앨범")
                {
                    hasExpectedAlbum = true;
                    break;
                }
            }

            if (!hasExpectedAlbum)
            {
                return false;
            }

            for (int i = 0; i < db.stageShowMusicList.Count; i++)
            {
                if (db.stageShowMusicList[i] == musicUid)
                {
                    return true;
                }
            }
        }
        catch (Exception ex)
        {
            MelonLogger.Error($"IsCustomAlbumContext 예외: {ex}");
        }

        return false;
    }

    public static string GetCurrentSelectedMusicUid()
    {
        if (!string.IsNullOrEmpty(CustomPlaySession.Current.SelectedMusicUid))
        {
            return CustomPlaySession.Current.SelectedMusicUid;
        }
        try
        {
            var pnlStage = UnityEngine.Object.FindObjectOfType<PnlStage>();
            if (pnlStage != null)
            {
                foreach (var field in typeof(PnlStage).GetFields(InstanceMembers))
                {
                    if (field.FieldType == typeof(Il2CppAssets.Scripts.Database.MusicInfo))
                    {
                        var info = field.GetValue(pnlStage) as Il2CppAssets.Scripts.Database.MusicInfo;
                        if (info != null && !string.IsNullOrEmpty(info.uid))
                        {
                            return info.uid;
                        }
                    }
                }
                foreach (var prop in typeof(PnlStage).GetProperties(InstanceMembers))
                {
                    if (prop.PropertyType == typeof(Il2CppAssets.Scripts.Database.MusicInfo) && prop.GetIndexParameters().Length == 0 && prop.CanRead)
                    {
                        var info = prop.GetValue(pnlStage) as Il2CppAssets.Scripts.Database.MusicInfo;
                        if (info != null && !string.IsNullOrEmpty(info.uid))
                        {
                            return info.uid;
                        }
                    }
                }
            }
        }
        catch (System.Exception ex)
        {
            MelonLogger.Error($"GetCurrentSelectedMusicUid 예외: {ex}");
        }
        return null;
    }

    public static string GetTitleTextFromArea(object area)
    {
        try
        {
            if (area == null) return null;

            var type = area.GetType();

            foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                if (!prop.CanRead || prop.PropertyType != typeof(string)) continue;
                var pn = prop.Name ?? string.Empty;
                if (pn.Equals("tag", StringComparison.OrdinalIgnoreCase) ||
                    pn.Equals("name", StringComparison.OrdinalIgnoreCase) ||
                    pn.Equals("gameObject", StringComparison.OrdinalIgnoreCase) ||
                    pn.Equals("transform", StringComparison.OrdinalIgnoreCase) ||
                    pn.Equals("ItemPrefabName", StringComparison.OrdinalIgnoreCase))
                    continue;
                try
                {
                    var v = prop.GetValue(area) as string;
                    if (!string.IsNullOrEmpty(v)) return v;
                }
                catch { }
            }

            var goProp = type.GetProperty("gameObject", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            var go = goProp?.GetValue(area) as UnityEngine.GameObject;

            if (go != null)
            {
                var allTexts = go.GetComponentsInChildren<UnityEngine.UI.Text>(true);

                foreach (var t in allTexts)
                {
                    if (t == null) continue;
                    var goName = t.gameObject?.name ?? string.Empty;
                    if (goName.IndexOf("SongTitle", StringComparison.OrdinalIgnoreCase) >= 0 ||
                        goName.IndexOf("TxtTitle", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        if (!string.IsNullOrEmpty(t.text)) return t.text;
                    }
                }

                foreach (var t in allTexts)
                {
                    if (t == null) continue;
                    if (!string.IsNullOrEmpty(t.text)) return t.text;
                }
            }
        }
        catch (Exception ex)
        {
            MelonLogger.Error($"GetTitleTextFromArea 예외: {ex}");
        }
        return null;
    }

    public static string DumpStringMembersForDebug(object area, int maxComponents = 20, int maxMembersPerComp = 10)
    {
        try
        {
            if (area == null) return "(area null)";
            var sb = new System.Text.StringBuilder();
            var type = area.GetType();

            var goProp = type.GetProperty("gameObject", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            UnityEngine.GameObject go = null;
            if (goProp != null) go = goProp.GetValue(area) as UnityEngine.GameObject;

            if (go == null)
            {
                sb.AppendLine($"No GameObject on area type {type.Name}");
                int found = 0;
                foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
                {
                    if (found >= maxMembersPerComp) break;
                    if (prop.PropertyType == typeof(string) && prop.CanRead)
                    {
                        var v = prop.GetValue(area) as string;
                        sb.AppendLine($"Prop {prop.Name} = '{v}'");
                        found++;
                    }
                    else if (prop.PropertyType == typeof(Text) && prop.CanRead)
                    {
                        var v = prop.GetValue(area) as Text;
                        sb.AppendLine($"Prop {prop.Name} = {FormatDebugValue(v)}");
                        found++;
                    }
                }
                foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
                {
                    if (found >= maxMembersPerComp) break;
                    if (field.FieldType == typeof(string))
                    {
                        var v = field.GetValue(area) as string;
                        sb.AppendLine($"Field {field.Name} = '{v}'");
                        found++;
                    }
                    else if (field.FieldType == typeof(Text))
                    {
                        var v = field.GetValue(area) as Text;
                        sb.AppendLine($"Field {field.Name} = {FormatDebugValue(v)}");
                        found++;
                    }
                }
                return sb.ToString();
            }

            var comps = go.GetComponentsInChildren<UnityEngine.Component>(true);
            int compCount = 0;
            foreach (var c in comps)
            {
                if (compCount++ >= maxComponents) break;
                if (c == null) continue;
                var ct = c.GetType();
                sb.AppendLine($"Component: {ct.FullName}");
                int memberCount = 0;
                foreach (var prop in ct.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
                {
                    if (memberCount++ >= maxMembersPerComp) break;
                    if (!prop.CanRead) continue;
                    string val = null;
                    try { val = FormatDebugValue(prop.GetValue(c)); } catch { val = "(err)"; }
                    if (!string.IsNullOrEmpty(val)) sb.AppendLine($"  Prop {prop.Name} = '{val}'");
                }
                foreach (var field in ct.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
                {
                    if (memberCount++ >= maxMembersPerComp) break;
                    string val = null;
                    try { val = FormatDebugValue(field.GetValue(c)); } catch { val = "(err)"; }
                    if (!string.IsNullOrEmpty(val)) sb.AppendLine($"  Field {field.Name} = '{val}'");
                }
            }
            return sb.ToString();
        }
        catch (Exception ex)
        {
            return $"Dump error: {ex}";
        }
    }

    private static string FormatDebugValue(object value)
    {
        try
        {
            if (value == null)
            {
                return "(null)";
            }

            if (value is Text text)
            {
                return $"Text(name={text.name ?? "(null)"}, text={CleanLogText(text.text)})";
            }

            if (value is UnityEngine.Object unityObject)
            {
                return $"{value.GetType().Name}(name={unityObject.name ?? "(null)"})";
            }

            return value.ToString();
        }
        catch (Exception ex)
        {
            return $"(format err: {ex.Message})";
        }
    }

    private static readonly string[] ExperimentModeTitles = { "\uc2e4\ud5d8 \ubaa8\ub4dc", "Experiment Mod", "\u5b9e\u9a8c\u6a21\u5f0f", "\u5be6\u9a57\u6a21\u5f0f", "\u5b9f\u9a13\u30e2\u30fc\u30c9" };

    public static void SyncExperimentModeFromStage(PnlStage stage)
    {
        try
        {
            if (stage == null) return;
            var titleText = stage.titleOwn;
            if (titleText == null) return;
            string text = titleText.text ?? string.Empty;
            bool isExp = false;
            foreach (var t in ExperimentModeTitles)
                if (text == t) { isExp = true; break; }
            if (isExp != CustomPlaySession.Current.IsExperimentModeActive)
            {
                CustomPlaySession.Current.IsExperimentModeActive = isExp;
            }
        }
        catch (Exception ex)
        {
            MelonLogger.Error($"SyncExperimentModeFromStage \uc608\uc678: {ex}");
        }
    }

    public static void LogButtons(string source, PnlStage stage)
    {
    }
}
