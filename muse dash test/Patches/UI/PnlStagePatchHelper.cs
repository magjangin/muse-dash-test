using MelonLoader;
using System;
using System.Reflection;
using Il2CppAssets.Scripts.UI.Panels;
using UnityEngine;
using UnityEngine.UI;

public static class PnlStagePatchHelper
{
    private const int CustomTagUid = 998;
    private const string CustomMusicUid = "0-0";
    private const string CustomTitle = "화영왕";
    private const string CustomArtist = "화영왕";

    private const BindingFlags InstanceMembers =
        BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

    public static void ApplyCustomTagTitleAccessors(string source, PnlStage stage)
    {
        try
        {
            if (stage == null)
            {
                MelonLogger.Msg($"[{source}] 커스텀 태그 접근자 적용 건너뜀: stage=null");
                return;
            }

            if (!IsCustomAlbumContext(CustomTagUid, CustomMusicUid))
            {
                MelonLogger.Msg($"[{source}] 커스텀 태그 접근자 적용 건너뜀: customContext=false");
                return;
            }

            var musicText = stage.musicNameTitle;
            var artistText = stage.artistNameTitle;

            if (musicText != null)
            {
                MelonLogger.Msg($"[{source}] musicNameTitle 접근자로 제목 변경: {CleanLogText(musicText.text)} -> {CustomTitle}");
                musicText.text = CustomTitle;
            }
            else
            {
                MelonLogger.Warning($"[{source}] musicNameTitle 접근자 결과가 null입니다.");
            }

            if (artistText != null)
            {
                MelonLogger.Msg($"[{source}] artistNameTitle 접근자로 아티스트 변경: {CleanLogText(artistText.text)} -> {CustomArtist}");
                artistText.text = CustomArtist;
            }
            else
            {
                MelonLogger.Warning($"[{source}] artistNameTitle 접근자 결과가 null입니다.");
            }
        }
        catch (Exception ex)
        {
            MelonLogger.Error($"{source} 커스텀 태그 접근자 적용 예외: {ex}");
        }
    }

    public static void LogPnlStageRefresh(string source, PnlStage stage)
    {
        try
        {
            if (stage == null)
            {
                MelonLogger.Msg($"[{source}] stage=null");
                return;
            }

            string selectedUid = GetCurrentSelectedMusicUid();
            string albumTitle = GetLongNameControllerText(stage.m_AlbumTitleTxt);
            string musicTitle = GetLongNameControllerText(stage.musicLongNameController);
            string artistTitle = GetLongNameControllerText(stage.artistLongNameController);
            string albumObjActive = stage.m_AlbumTitleObj != null ? stage.m_AlbumTitleObj.activeSelf.ToString() : "(null)";
            MelonLogger.Msg($"[{source}] selectedUid={selectedUid ?? "(null)"}, albumTitle={CleanLogText(albumTitle)}, musicTitle={CleanLogText(musicTitle)}, artistTitle={CleanLogText(artistTitle)}, albumTitleObjActive={albumObjActive}");
        }
        catch (Exception ex)
        {
            MelonLogger.Error($"{source} 예외: {ex}");
        }
    }

    public static void LogTextAccessor(string source, PnlStage stage, Text text)
    {
        try
        {
            string selectedUid = GetCurrentSelectedMusicUid();
            string textName = text != null ? text.name : "(null)";
            string gameObjectName = text != null && text.gameObject != null ? text.gameObject.name : "(null)";
            string value = text != null ? text.text : null;
            string active = text != null && text.gameObject != null ? text.gameObject.activeSelf.ToString() : "(null)";
            string stageName = stage != null ? stage.name : "(null)";
            MelonLogger.Msg($"[{source}] stage={stageName}, selectedUid={selectedUid ?? "(null)"}, TextName={textName}, GameObject={gameObjectName}, Active={active}, Text={CleanLogText(value)}");
        }
        catch (Exception ex)
        {
            MelonLogger.Error($"{source} 예외: {ex}");
        }
    }

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
                if (album != null && album.uid == "998-0" && album.title == "실험 앨범")
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

            // Step 1: own plain string properties (excluding Unity meta names)
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

            // Step 2: get the GameObject and use typed GetComponentsInChildren (no Reflection for text read)
            var goProp = type.GetProperty("gameObject", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            var go = goProp?.GetValue(area) as UnityEngine.GameObject;

            if (go != null)
            {
                var allTexts = go.GetComponentsInChildren<UnityEngine.UI.Text>(true);

                // Prefer UI.Text on child GameObjects with a song-title name
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

                // Fall back to any UI.Text child
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

            // If has gameObject, inspect its components
            var goProp = type.GetProperty("gameObject", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            UnityEngine.GameObject go = null;
            if (goProp != null) go = goProp.GetValue(area) as UnityEngine.GameObject;

            if (go == null)
            {
                sb.AppendLine($"No GameObject on area type {type.Name}");
                // also list string props/fields on the area itself
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
                    try { var obj = prop.GetValue(c); if (obj != null) val = obj.ToString(); } catch { val = "(err)"; }
                    if (!string.IsNullOrEmpty(val)) sb.AppendLine($"  Prop {prop.Name} = '{val}'");
                }
                foreach (var field in ct.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
                {
                    if (memberCount++ >= maxMembersPerComp) break;
                    string val = null;
                    try { var obj = field.GetValue(c); if (obj != null) val = obj.ToString(); } catch { val = "(err)"; }
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
}
