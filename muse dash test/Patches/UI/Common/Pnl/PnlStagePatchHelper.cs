using MelonLoader;
using System;
using System.Collections.Generic;
using System.Reflection;
using Il2CppAssets.Scripts.Database;
using Il2CppAssets.Scripts.UI.Panels;
using muse_dash_test;

public static partial class PnlStagePatchHelper
{
    private const int CustomTagUid = muse_dash_test.CustomContentIds.TagIndex;
    private const string CustomMusicUid = muse_dash_test.CustomContentIds.FallbackSourceMusicUid;
    private const string CustomTitle = "화영왕 0";
    private const string CustomArtist = "화영왕 0";

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
        string uid = CustomPlaySession.Current.SelectedMusicUid;
        if (string.IsNullOrEmpty(uid))
        {
            uid = GetCurrentSelectedMusicUid() ?? CustomPlaySession.Current.LastClickedMusicUid;
        }
        return CustomContentIds.IsVirtualSong(uid);
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

            string uid = CustomPlaySession.Current.SelectedMusicUid;
            if (string.IsNullOrEmpty(uid))
            {
                uid = GetCurrentSelectedMusicUid() ?? CustomPlaySession.Current.LastClickedMusicUid;
            }
            if (string.IsNullOrEmpty(uid))
            {
                return;
            }

            var musicInfo = GlobalDataBase.dbMusicTag?.GetMusicInfoFromAll(uid);
            if (musicInfo == null)
            {
                return;
            }

            ApplyTitleTexts(stage, musicInfo);
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

            if (!CustomPlaySession.Current.IsExperimentModeActive)
            {
                return;
            }

            string uid = CustomPlaySession.Current.SelectedMusicUid;
            if (string.IsNullOrEmpty(uid))
            {
                uid = GetCurrentSelectedMusicUid() ?? CustomPlaySession.Current.LastClickedMusicUid;
            }
            if (string.IsNullOrEmpty(uid))
            {
                return;
            }

            var musicInfo = GlobalDataBase.dbMusicTag?.GetMusicInfoFromAll(uid);
            if (musicInfo == null)
            {
                return;
            }

            ApplyTitleTexts(stage, musicInfo);
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

            if (!CustomContentIds.IsVirtualSong(musicInfo.uid))
            {
                return false;
            }

            ApplyTitleTexts(stage, musicInfo);

            var musicText = stage.musicNameTitle;
            var artistText = stage.artistNameTitle;
            MelonLogger.Msg($"{source}: musicInfo.uid={musicInfo.uid} direct apply => musicText={CleanLogText(musicText != null ? musicText.text : null)}, artistText={CleanLogText(artistText != null ? artistText.text : null)}");
            return true;
        }
        catch (Exception ex)
        {
            MelonLogger.Error($"{source} musicInfo 직접 커스텀 태그 적용 예외: {ex}");
            return false;
        }
    }

    private static void ApplyTitleTexts(PnlStage stage, MusicInfo musicInfo)
    {
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

}
