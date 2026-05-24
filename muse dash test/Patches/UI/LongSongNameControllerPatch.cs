using MelonLoader;
using System;
using System.Collections.Generic;
using UnityEngine;

// LongSongNameController.Refresh 후킹 패치 (진단 및 후킹용)
[HarmonyLib.HarmonyPatch(typeof(Il2Cpp.LongSongNameController), "Refresh", new Type[] { typeof(string), typeof(bool), typeof(float) })]
public class LongSongNameController_Refresh_Patch
{
    private const int CustomTagUid = 998;
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
        { "0-100", "화영왕1" },
        { "0-101", "화영왕2" },
        { "0-102", "화영왕3" }
    };

    private static readonly Dictionary<string, string> CustomArtists = new Dictionary<string, string>
    {
        { "0-100", "화영왕1" },
        { "0-101", "화영왕2" },
        { "0-102", "화영왕3" }
    };

    public static void Prefix(Il2Cpp.LongSongNameController __instance, ref string text, bool isSpecialFont, float delay)
    {
        try
        {
            MelonLogger.Msg($"[LongSongNameController.Refresh] 호출됨: 원본 text='{text}' | GameObject={__instance.gameObject.name}");

            // 등록된 커스텀 텍스트 우선 적용 (Refresh 시에도 유지)
            if (_customTextMap.TryGetValue(__instance.Pointer, out var mapped))
            {
                MelonLogger.Msg($"[LongSongNameController.Refresh] 커스텀 텍스트 유지: '{text}' → '{mapped}'");
                text = mapped;
                return;
            }
            
            // 현재 선택된 곡 Uid 가져오기 (로컬 헬퍼 사용)
            string selectedUid = PnlStagePatchHelper.GetCurrentSelectedMusicUid();
            if (__instance.gameObject.name == "ImgAlbumTittle")
            {
                bool isCustomAlbumContext = PnlStagePatchHelper.IsCustomAlbumContext(CustomTagUid, CustomMusicUid);
                MelonLogger.Msg($"[LongSongNameController.Refresh] 앨범 제목 검사: selectedUid='{selectedUid ?? "(null)"}', isCustomAlbumContext={isCustomAlbumContext}");

                if (isCustomAlbumContext)
                {
                    text = CustomAlbumTitle;
                    MelonLogger.Msg($"[LongSongNameController.Refresh] 앨범 제목 강제 변경 -> {CustomAlbumTitle} (UID: {selectedUid ?? "(unknown)"})");
                }

                return;
            }

            if (!string.IsNullOrEmpty(selectedUid))
            {
                if (__instance.gameObject.name == "ImgSongTitleMask")
                {
                    if (CustomTitles.TryGetValue(selectedUid, out var customTitle))
                    {
                        text = customTitle;
                        MelonLogger.Msg($"[LongSongNameController.Refresh] 곡 제목 강제 변경 -> {customTitle} (UID: {selectedUid})");
                    }
                }
                else if (__instance.gameObject.name == "ImgArtistMask")
                {
                    if (CustomArtists.TryGetValue(selectedUid, out var customArtist))
                    {
                        text = customArtist;
                        MelonLogger.Msg($"[LongSongNameController.Refresh] 아티스트 강제 변경 -> {customArtist} (UID: {selectedUid})");
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
