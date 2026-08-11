using MelonLoader;
using System;
using System.Collections.Generic;
using Il2Cpp;
using Il2CppAssets.Scripts.UI.Panels;
using muse_dash_test;

/// <summary>
/// 곡 선택 화면에서 커버 아래에 붙는 앨범(음악 팩) 이름을 커스텀 곡에 맞게 고칩니다.
///
/// 커스텀 곡은 원본 곡의 얇은 복제본이라, 앨범 이름 자리에 숙주가 속한 팩 이름('기본 패키지')이 그대로 나옵니다.
/// <see cref="CustomTagRegistry.CustomAlbumInfo"/>의 제목은 이미 '실험 앨범'으로 등록해 두었지만
/// 이 라벨은 그 경로로 채워지지 않아서, 출력 지점에서 마지막에 덮어씁니다.
/// </summary>
public static partial class PnlStagePatchHelper
{
    /// <summary>같은 (곡, 이전 값) 조합은 한 번만 로그로 남깁니다. 이 경로는 프레임마다 돌 수 있습니다.</summary>
    private static readonly HashSet<string> loggedAlbumTitles = new HashSet<string>();

    public static void ApplyAlbumTitle(string source, PnlStage stage)
    {
        try
        {
            if (stage == null) return;

            string uid = CustomPlaySession.Current.SelectedMusicUid;
            if (string.IsNullOrEmpty(uid))
            {
                uid = GetCurrentSelectedMusicUid() ?? CustomPlaySession.Current.LastClickedMusicUid;
            }
            if (!CustomContentIds.IsVirtualSong(uid)) return;

            var controller = stage.m_AlbumTitleTxt;
            if (controller == null) return;

            string before = ReadAlbumTitle(controller);
            if (string.Equals(before, CustomTagRegistry.AlbumTitle, StringComparison.Ordinal)) return;

            controller.RefreshText(CustomTagRegistry.AlbumTitle);

            if (loggedAlbumTitles.Add(uid + "|" + (before ?? "(null)")))
            {
                MelonLogger.Msg($"[{source}] 앨범 이름 교체: uid={uid}, '{before ?? "(null)"}' → '{CustomTagRegistry.AlbumTitle}'");
            }
        }
        catch (Exception ex)
        {
            MelonLogger.Error($"{source} 앨범 이름 적용 예외: {ex}");
        }
    }

    /// <summary>지금 걸려 있는 앨범 이름을 읽습니다. 긴 이름은 흐르는 텍스트라 표시용 Text가 여럿입니다.</summary>
    private static string ReadAlbumTitle(LongSongNameController controller)
    {
        try
        {
            var simple = controller.m_TxtSimpleName;
            if (simple != null && !string.IsNullOrEmpty(simple.text)) return simple.text;

            var mid = controller.m_MidSimpleName;
            if (mid != null && !string.IsNullOrEmpty(mid.text)) return mid.text;

            var backup = controller.m_TxtBackupName;
            if (backup != null && !string.IsNullOrEmpty(backup.text)) return backup.text;
        }
        catch (Exception) { }

        return null;
    }
}
