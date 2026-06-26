using MelonLoader;
using HarmonyLib;
using System;

namespace muse_dash_test
{
    /// <summary>
    /// 곡 디스크 셀(MusicStageCell)이 커버를 세팅하는 SetCoverLogic 직후에 후킹하여,
    /// 그 셀의 곡이 가상곡이고 cover.png가 있으면 해당 셀의 커버 이미지(m_StageImg)를
    /// 커스텀 커버로 교체합니다. 인자(cellInfo.musicUid)가 셀별 곡 uid를 직접 들고 있으므로
    /// 인접 카드까지 각자 자기 곡의 커버가 정확히 적용됩니다(sprite 이름 매칭 불필요).
    /// </summary>
    [HarmonyPatch(typeof(Il2Cpp.MusicStageCell), "SetCoverLogic")]
    public class MusicStageCell_SetCoverLogic_Patch
    {
        public static void Postfix(Il2Cpp.MusicStageCell __instance, Il2Cpp.MusicStageCell.MusicStageCellInfo cellInfo)
        {
            try
            {
                if (__instance == null || cellInfo == null) return;

                string uid = cellInfo.musicUid;
                if (string.IsNullOrEmpty(uid) || !CustomContentIds.IsVirtualSong(uid)) return;
                if (!CoverImageManager.TryGetCoverSprite(uid, out var sprite) || sprite == null) return;

                var img = __instance.m_StageImg;
                if (img != null && img.sprite != sprite)
                {
                    img.sprite = sprite;
                    MelonLogger.Msg($"[Cover.Disc] MusicStageCell 커버 교체 uid='{uid}'");
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[Cover.Disc] SetCoverLogic Postfix 예외: {ex}");
            }
        }
    }
}
