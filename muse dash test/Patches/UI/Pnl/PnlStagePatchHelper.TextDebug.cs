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
            ModLogger.Error($"IsCustomAlbumContext 예외: {ex}");
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
            ModLogger.Error($"GetCurrentSelectedMusicUid 예외: {ex}");
        }
        return null;
    }

    private static readonly string[] ExperimentModeTitles = { "\uc2e4\ud5d8 \ubaa8\ub4dc", "Experiment Mod", "\u5b9e\u9a8c\u6a21\u5f0f", "\u5be6\u9a57\u6a21\u5f0f", "\u5b9f\u9a13\u30e2\u30fc\u30c9" };

    public static bool IsExperimentModeTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            return false;
        }

        string normalized = NormalizeExperimentModeTitle(title);
        if (string.IsNullOrEmpty(normalized))
        {
            return false;
        }

        foreach (var candidate in ExperimentModeTitles)
        {
            if (NormalizeExperimentModeTitle(candidate) == normalized)
            {
                return true;
            }
        }

        return false;
    }

    private static string NormalizeExperimentModeTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            return string.Empty;
        }

        var sb = new StringBuilder(title.Length);
        foreach (char ch in title.Trim())
        {
            if (char.IsWhiteSpace(ch))
            {
                continue;
            }

            sb.Append(char.ToLowerInvariant(ch));
        }

        return sb.ToString();
    }

    public static void SyncExperimentModeFromStage(PnlStage stage)
    {
        try
        {
            if (stage == null) return;
            var titleText = stage.titleOwn;
            if (titleText == null) return;
            string text = titleText.text ?? string.Empty;
            bool isExp = IsExperimentModeTitle(text);

            bool previous = CustomPlaySession.Current.IsExperimentModeActive;
            if (isExp != previous)
            {
                CustomPlaySession.Current.IsExperimentModeActive = isExp;
            }

            ModLogger.Verbose($"[PnlStage.ExperimentMode] title='{text}', detected={isExp}, previous={previous}, current={CustomPlaySession.Current.IsExperimentModeActive}");
        }
        catch (Exception ex)
        {
            ModLogger.Error($"SyncExperimentModeFromStage \uc608\uc678: {ex}");
        }
    }

    public static void LogButtons(string source, PnlStage stage)
    {
    }
}
