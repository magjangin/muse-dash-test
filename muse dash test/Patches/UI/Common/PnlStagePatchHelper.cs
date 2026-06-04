using MelonLoader;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Il2CppAssets.Scripts.Database;
using Il2CppAssets.Scripts.UI.Panels;
using UnityEngine;
using UnityEngine.UI;

public static partial class PnlStagePatchHelper
{
    private const int CustomTagUid = 998;
    private const string CustomMusicUid = "0-0";
    private const string CustomTitle = "화영왕 0";
    private const string CustomArtist = "화영왕 0";

    public static string LastSelectedMusicUid = "";

    private const BindingFlags InstanceMembers =
        BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

    public static string DescribeMusicInfo(MusicInfo musicInfo)
    {
        if (musicInfo == null)
        {
            return "MusicInfo(null)";
        }

        try
        {
            return $"MusicInfo(uid={musicInfo.uid ?? "(null)"}, name={musicInfo.name ?? "(null)"}, author={musicInfo.author ?? "(null)"}, cover={musicInfo.cover ?? "(null)"})";
        }
        catch (Exception ex)
        {
            return $"MusicInfo({musicInfo.GetType().Name}, describe failed: {ex.Message})";
        }
    }



    public static bool ShouldApplyHwayoungwang()
    {
        string uid = LastSelectedMusicUid;
        if (string.IsNullOrEmpty(uid))
        {
            uid = GetCurrentSelectedMusicUid() ?? muse_dash_test.MusicButtonCell_OnButtonClicked_Patch.LastClickedMusicUid;
        }
        return uid != null && uid.StartsWith("999-");
    }

    public static void ApplyCustomTagTitleAccessors(string source, PnlStage stage)
    {
        try
        {
            if (stage == null)
            {
                return;
            }

            // 통합 조건 검사 적용
            if (!ShouldApplyHwayoungwang())
            {
                return;
            }

            if (!IsCustomAlbumContext(CustomTagUid, CustomMusicUid))
            {
                return;
            }

            string uid = LastSelectedMusicUid;
            if (string.IsNullOrEmpty(uid))
            {
                uid = GetCurrentSelectedMusicUid() ?? muse_dash_test.MusicButtonCell_OnButtonClicked_Patch.LastClickedMusicUid;
            }

            var musicInfo = GlobalDataBase.dbMusicTag?.GetMusicInfoFromAll(uid);
            if (musicInfo == null)
            {
                return;
            }

            var musicText = stage.musicNameTitle;
            var artistText = stage.artistNameTitle;

            if (musicText != null)
            {
                musicText.text = musicInfo.name;
            }

            if (artistText != null)
            {
                artistText.text = musicInfo.author;
            }
        }
        catch (Exception ex)
        {
            MelonLogger.Error($"{source} 커스텀 태그 접근자 적용 예외: {ex}");
        }
    }

    public static void ForceApplyCustomTagTitleAccessors(string source, PnlStage stage)
    {
        try
        {
            if (stage == null)
            {
                return;
            }

            if (!MusicButtonAreaTitle_RefreshTxt_Patch.IsExperimentModActive)
            {
                return;
            }

            string uid = LastSelectedMusicUid;
            if (string.IsNullOrEmpty(uid))
            {
                uid = GetCurrentSelectedMusicUid() ?? muse_dash_test.MusicButtonCell_OnButtonClicked_Patch.LastClickedMusicUid;
            }

            var musicInfo = GlobalDataBase.dbMusicTag?.GetMusicInfoFromAll(uid);
            if (musicInfo == null)
            {
                return;
            }

            var musicText = stage.musicNameTitle;
            var artistText = stage.artistNameTitle;

            if (musicText != null)
            {
                musicText.text = musicInfo.name;
            }

            if (artistText != null)
            {
                artistText.text = musicInfo.author;
            }
        }
        catch (Exception ex)
        {
            MelonLogger.Error($"{source} 강제 커스텀 태그 접근자 적용 예외: {ex}");
        }
    }

    public static bool ApplyCustomTagTitleAccessorsForMusicInfo(string source, PnlStage stage, MusicInfo musicInfo)
    {
        try
        {
            if (stage == null || musicInfo == null)
            {
                return false;
            }

            if (musicInfo.uid == null || !musicInfo.uid.StartsWith("999-"))
            {
                return false;
            }

            var musicText = stage.musicNameTitle;
            var artistText = stage.artistNameTitle;

            if (musicText != null)
            {
                musicText.text = musicInfo.name;
            }

            if (artistText != null)
            {
                artistText.text = musicInfo.author;
            }

            MelonLogger.Msg($"{source}: musicInfo.uid={musicInfo.uid} direct apply => musicText={CleanLogText(musicText != null ? musicText.text : null)}, artistText={CleanLogText(artistText != null ? artistText.text : null)}");
            return true;
        }
        catch (Exception ex)
        {
            MelonLogger.Error($"{source} musicInfo 직접 커스텀 태그 적용 예외: {ex}");
            return false;
        }
    }

    public static void LogStageTitleSnapshot(string source, PnlStage stage)
    {
    }

    public static void LogPnlStageRefresh(string source, PnlStage stage)
    {
    }

    public static void LogTextAccessor(string source, PnlStage stage, Text text)
    {
    }

    public static string DescribeTextAccessorSource(PnlStage stage, Text text)
    {
        try
        {
            if (stage == null || text == null)
            {
                return "(unresolved)";
            }

            string direct = FindTextReferenceSource(stage, text, "stage", 0, 4);
            if (!string.IsNullOrEmpty(direct))
            {
                return direct;
            }

            return "(unresolved)";
        }
        catch (Exception ex)
        {
            return $"(source error: {ex.Message})";
        }
    }

    private static string FindTextReferenceSource(object target, Text text, string path, int depth, int maxDepth)
    {
        try
        {
            if (target == null || text == null)
            {
                return null;
            }

            if (ReferenceEquals(target, text))
            {
                return path;
            }

            if (depth >= maxDepth)
            {
                return null;
            }

            foreach (var field in target.GetType().GetFields(InstanceMembers))
            {
                object value = field.GetValue(target);
                if (value == null)
                {
                    continue;
                }

                if (ReferenceEquals(value, text))
                {
                    return $"{path}.{field.Name}";
                }

                if (value is string || value is Text || value is UnityEngine.Object)
                {
                    continue;
                }

                string nestedPath = FindTextReferenceSource(value, text, $"{path}.{field.Name}", depth + 1, maxDepth);
                if (!string.IsNullOrEmpty(nestedPath))
                {
                    return nestedPath;
                }
            }

            return null;
        }
        catch
        {
            return null;
        }
    }

    public static void LogPnlStageProperties(string source, PnlStage stage)
    {
    }

    public static void LogMusicRootComponents(string source, PnlStage stage)
    {
    }

    private static bool LooksLikeCoverImage(string objectName, string assetName)
    {
        return ContainsCoverToken(objectName) || ContainsCoverToken(assetName);
    }

    private static bool ContainsCoverToken(string value)
    {
        return !string.IsNullOrEmpty(value) &&
            (value.IndexOf("ImgCover", StringComparison.OrdinalIgnoreCase) >= 0 ||
             value.IndexOf("cover", StringComparison.OrdinalIgnoreCase) >= 0);
    }

    private static string GetTransformPath(Transform transform, Transform stopAt)
    {
        try
        {
            if (transform == null)
            {
                return "(null)";
            }

            var sb = new StringBuilder(transform.name);
            var current = transform.parent;
            while (current != null && current != stopAt)
            {
                sb.Insert(0, current.name + "/");
                current = current.parent;
            }

            if (stopAt != null)
            {
                sb.Insert(0, stopAt.name + "/");
            }

            return sb.ToString();
        }
        catch
        {
            return transform != null ? transform.name : "(null)";
        }
    }

    private static string SafePropertyValue(object target, PropertyInfo prop)
    {
        try
        {
            object value = prop.GetValue(target);
            if (value == null)
            {
                return "(null)";
            }

            if (value is string s)
            {
                return CleanLogText(s);
            }

            Type type = value.GetType();
            if (type.IsPrimitive || value is decimal)
            {
                return value.ToString();
            }

            if (value is Text text)
            {
                return $"Text(name={text.name ?? "(null)"}, text={CleanLogText(text.text)})";
            }

            if (value is Il2CppAssets.Scripts.Database.MusicInfo musicInfo)
            {
                return $"MusicInfo(uid={musicInfo.uid ?? "(null)"}, name={musicInfo.name ?? "(null)"}, cover={musicInfo.cover ?? "(null)"})";
            }

            if (value is UnityEngine.Object unityObject)
            {
                return $"{type.Name}(name={unityObject.name ?? "(null)"})";
            }

            return type.FullName;
        }
        catch (Exception ex)
        {
            return "(error: " + ex.Message + ")";
        }
    }

}
