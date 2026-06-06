using MelonLoader;
using System;
using System.Collections.Generic;
using UnityEngine;

// LongSongNameController.Refresh 후킹 패치 (진단 및 후킹용)
[HarmonyLib.HarmonyPatch(typeof(Il2Cpp.LongSongNameController), "Refresh", new Type[] { typeof(string), typeof(bool), typeof(float) })]
public class LongSongNameController_Refresh_Patch
{
    private const int CustomTagUid = 9998;
    private const string CustomMusicUid = "0-0";
    private const string CustomAlbumTitle = "실험 앨범";

    // 태그 뷰 아이템의 LongSongNameController 인스턴스 → 커스텀 텍스트 맵
    private static readonly Dictionary<IntPtr, string> _customTextMap = new Dictionary<IntPtr, string>();

    public static void RegisterCustomText(Il2Cpp.LongSongNameController ctrl, string customText)
    {
        if (ctrl != null)
            _customTextMap[ctrl.Pointer] = customText;
    }

    private static readonly Dictionary<string, string> CustomTitles = new Dictionary<string, string>
    {
        { "9999-0", "화영왕 0" },
        { "9999-1", "화영왕 1" },
        { "9999-2", "화영왕 2" },
        { "0-100", "화영왕1" },
        { "0-101", "화영왕2" },
        { "0-102", "화영왕3" }
    };

    private static readonly Dictionary<string, string> CustomArtists = new Dictionary<string, string>
    {
        { "9999-0", "화영왕 0" },
        { "9999-1", "화영왕 1" },
        { "9999-2", "화영왕 2" },
        { "0-100", "화영왕1" },
        { "0-101", "화영왕2" },
        { "0-102", "화영왕3" }
    };

    public static void Prefix(Il2Cpp.LongSongNameController __instance, ref string text, bool isSpecialFont, float delay)
    {
        if (!MusicButtonAreaTitle_RefreshTxt_Patch.IsExperimentModActive)
        {
            return;
        }
        try
        {
            // 등록된 커스텀 텍스트 우선 적용 (Refresh 시에도 유지)
            if (_customTextMap.TryGetValue(__instance.Pointer, out var mapped))
            {
                text = mapped;
                return;
            }
            
            // 현재 이 Controller가 속한 곡의 UID를 구한다.
            // 우선 parent MusicButtonCell을 찾아보고, 있으면 그 셀의 곡 UID를 사용한다.
            string targetUid = null;
            var cell = __instance.GetComponentInParent<Il2CppAssets.Scripts.UI.Panels.PnlMusicTag.MusicButtonCell>();
            if (cell != null && cell.musicInfo != null)
            {
                targetUid = cell.musicInfo.uid;
            }
            else
            {
                // 없으면 현재 선택된 곡 UID를 사용한다.
                targetUid = PnlStagePatchHelper.GetCurrentSelectedMusicUid();
                if (string.IsNullOrEmpty(targetUid) || targetUid == "(null)")
                {
                    targetUid = muse_dash_test.MusicButtonCell_OnButtonClicked_Patch.LastClickedMusicUid;
                }
            }

            if (string.IsNullOrEmpty(targetUid) || targetUid == "(null)")
            {
                return;
            }

            // 앨범 제목 처리 (ImgAlbumTittle)
            if (__instance.gameObject.name == "ImgAlbumTittle")
            {
                bool isCustomAlbumContext = PnlStagePatchHelper.IsCustomAlbumContext(CustomTagUid, CustomMusicUid);
                if (isCustomAlbumContext)
                {
                    text = CustomAlbumTitle;
                }
                return;
            }

            // 커스텀 제목/아티스트 매핑 처리
            if (__instance.gameObject.name == "ImgSongTitleMask" || __instance.gameObject.name == "ImgSongNameMask")
            {
                if (CustomTitles.TryGetValue(targetUid, out var customTitle))
                {
                    text = customTitle;
                }
                else if (targetUid != null && targetUid.StartsWith("9999-"))
                {
                    var musicInfo = Il2CppAssets.Scripts.Database.GlobalDataBase.dbMusicTag?.GetMusicInfoFromAll(targetUid);
                    if (musicInfo != null)
                    {
                        text = musicInfo.name;
                    }
                }
            }
            else if (__instance.gameObject.name == "ImgArtistMask" || __instance.gameObject.name == "ImgSongAuthorMask")
            {
                if (CustomArtists.TryGetValue(targetUid, out var customArtist))
                {
                    text = customArtist;
                }
                else if (targetUid != null && targetUid.StartsWith("9999-"))
                {
                    var musicInfo = Il2CppAssets.Scripts.Database.GlobalDataBase.dbMusicTag?.GetMusicInfoFromAll(targetUid);
                    if (musicInfo != null)
                    {
                        text = musicInfo.author;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            MelonLogger.Error($"LongSongNameController.Refresh Prefix 예외: {ex}");
        }
    }
}
