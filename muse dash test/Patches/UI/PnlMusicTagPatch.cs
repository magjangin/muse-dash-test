using HarmonyLib;
using Il2CppAssets.Scripts.UI.Panels.PnlMusicTag;
using MelonLoader;
using System;

namespace muse_dash_test
{
    internal static class PnlMusicTagPatchLogger
    {
        private const string CustomMusicUid = "0-0";
        private const string CustomCellTitle = "화영왕 0";

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
                    return;
                }

                for (int i = 0; i < viewItems.Count; i++)
                {
                    var cell = viewItems[i]?.m_Cell;
                    var musicInfo = cell?.musicInfo;
                    if (cell == null)
                    {
                        continue;
                    }

                    if (musicInfo == null)
                    {

                        continue;
                    }

                    if (musicInfo.uid != CustomMusicUid && musicInfo.uid != "999-0")
                    {
                        continue;
                    }

                    SetCellTitleText(cell, CustomCellTitle, musicInfo.name);
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
            PnlMusicTagPatchLogger.ApplyCustomCellTitle(__instance);
        }
    }
}
