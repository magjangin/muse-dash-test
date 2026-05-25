using HarmonyLib;
using Il2CppAssets.Scripts.UI.Panels.PnlMusicTag;
using MelonLoader;
using System;

namespace muse_dash_test
{
    internal static class PnlMusicTagPatchLogger
    {
        private const string CustomMusicUid = "0-0";
        private const string CustomCellTitle = "화영왕";

        public static void ApplyCustomCellTitle(PnlMusicTag panel)
        {
            if (!MusicButtonAreaTitle_RefreshTxt_Patch.IsExperimentModActive)
            {
                return;
            }
            try
            {
                var viewItems = panel?.viewItems;
                if (viewItems == null)
                {
                    MelonLogger.Msg("[PnlMusicTag] 커스텀 셀 제목 적용 스킵: viewItems=null");
                    return;
                }

                MelonLogger.Msg($"[PnlMusicTag] 커스텀 셀 제목 적용 검사 시작: viewItems.Count={viewItems.Count}");

                for (int i = 0; i < viewItems.Count; i++)
                {
                    var cell = viewItems[i]?.m_Cell;
                    var musicInfo = cell?.musicInfo;
                    if (cell == null)
                    {
                        MelonLogger.Msg($"[PnlMusicTag] viewItem[{i}] 스킵: cell=null");
                        continue;
                    }

                    if (musicInfo == null)
                    {
                        MelonLogger.Msg($"[PnlMusicTag] viewItem[{i}] 스킵: musicInfo=null, cellGo={cell.gameObject?.name ?? "(null)"}");
                        continue;
                    }

                    MelonLogger.Msg($"[PnlMusicTag] viewItem[{i}] 셀 검사: uid={musicInfo.uid ?? "(null)"}, name={musicInfo.name ?? "(null)"}, cellGo={cell.gameObject?.name ?? "(null)"}");

                    if (musicInfo.uid != CustomMusicUid && musicInfo.uid != "999-0")
                    {
                        MelonLogger.Msg($"[PnlMusicTag] viewItem[{i}] 스킵: uid mismatch expected={CustomMusicUid} or 999-0");
                        continue;
                    }

                    int writes = SetCellTitleText(cell, CustomCellTitle, musicInfo.name);
                    if (writes > 0)
                    {
                        MelonLogger.Msg($"[PnlMusicTag] 커스텀 셀 제목 적용: uid={musicInfo.uid}, title={CustomCellTitle}, writes={writes}");
                    }
                    else
                    {
                        MelonLogger.Msg($"[PnlMusicTag] 커스텀 셀 제목 적용 결과: uid={musicInfo.uid}, writes=0");
                    }
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[PnlMusicTag] 커스텀 셀 제목 적용 예외: {ex}");
            }
        }

        private static int SetCellTitleText(MusicButtonCell cell, string title, string originalTitle)
        {
            var go = cell?.gameObject;
            if (go == null)
                return 0;

            int writes = 0;
            var texts = go.GetComponentsInChildren<UnityEngine.UI.Text>(true);
            MelonLogger.Msg($"[PnlMusicTag] SetCellTitleText: cellGo={go.name}, textCount={texts.Length}, originalTitle={originalTitle ?? "(null)"}, targetTitle={title}");
            for (int i = 0; i < texts.Length; i++)
            {
                var text = texts[i];
                if (text == null)
                    continue;

                string objectName = text.gameObject?.name ?? string.Empty;
                bool looksLikeTitle =
                    objectName.IndexOf("SongTitle", StringComparison.OrdinalIgnoreCase) >= 0 ||
                    objectName.IndexOf("TxtTitle", StringComparison.OrdinalIgnoreCase) >= 0 ||
                    objectName.IndexOf("Name", StringComparison.OrdinalIgnoreCase) >= 0;

                MelonLogger.Msg($"[PnlMusicTag] Text 후보[{i}]: objectName={objectName}, current='{text.text ?? "(null)"}', looksLikeTitle={looksLikeTitle}");

                if (!looksLikeTitle && text.text != originalTitle)
                    continue;

                if (text.text != title)
                {
                    text.text = title;
                    writes++;
                }
            }

            return writes;
        }
    }

    [HarmonyPatch(typeof(PnlMusicTag), nameof(PnlMusicTag.RefreshScrollViewItem))]
    internal static class PnlMusicTag_RefreshScrollViewItem_Patch
    {
        private static void Postfix(PnlMusicTag __instance)
        {
            MelonLogger.Msg("[PnlMusicTag.RefreshScrollViewItem] Postfix 호출됨");
            PnlMusicTagPatchLogger.ApplyCustomCellTitle(__instance);
        }
    }
}
