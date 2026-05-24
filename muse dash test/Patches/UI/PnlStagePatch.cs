using MelonLoader;
using System;
using System.Reflection;
using System.Collections;
using Il2CppAssets.Scripts.UI.Panels;
using UnityEngine;
using UnityEngine.UI;

// PnlStage.Start 후킹
[HarmonyLib.HarmonyPatch(typeof(PnlStage), "Start")]
public class PnlStage_Start_Patch
{
    public static void Prefix(PnlStage __instance)
    {
        try { }
        catch (Exception ex) { MelonLogger.Error($"PnlStage.Start Prefix 예외: {ex}"); }
    }

    public static void Postfix(PnlStage __instance)
    {
        try
        {
            PnlMusicUtils.LogMusicInfo("PnlStage.Start", __instance);
            MelonCoroutines.Start(PnlMusicUtils.LogMusicInfoAfterDelay("PnlStage.Start.Delay", __instance, 0.5f));
            
            // 실시간 리스트 확인을 위해 코루틴 다시 구동 (지연: 2.0초)
            MelonCoroutines.Start(LogTagItemsAfterDelay(2.0f));
        }
        catch (Exception ex) { MelonLogger.Error($"PnlStage.Start Postfix 예외: {ex}"); }
    }

    public static IEnumerator LogTagItemsAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        try
        {
            MusicButtonCellInstanceTracker.LogSummary("PnlStage.Start.Delay");

            var scrollView = UnityEngine.Object.FindObjectOfType<Il2Cpp.PnlMusicTagScrollView>();
            if (scrollView == null)
            {
                MelonLogger.Msg("[★] PnlMusicTagScrollView를 찾을 수 없습니다.");
                yield break;
            }

            MelonLogger.Msg($"[★] PnlMusicTagScrollView 발견! Name={scrollView.name} | ItemTotalCount={scrollView.ItemTotalCount} | ShownCount={scrollView.ShownItemCount}");
            
            var itemList = scrollView.mItemList;
            if (itemList == null)
            {
                MelonLogger.Msg("[★] mItemList가 null입니다.");
                yield break;
            }

            MelonLogger.Msg($"[★] mItemList 크기: {itemList.Count}");
            for (int i = 0; i < itemList.Count; i++)
            {
                var item = itemList[i];
                if (item == null) continue;

                string treeTypeStr = item.m_TreeType.ToString();
                string nameStr = item.name;
                int itemIndex = item.ItemIndex;
                int itemId = item.ItemId;

                MelonLogger.Msg($"[★ 태그 아이템 {i}] GameObject={nameStr} | TreeType={treeTypeStr} | ItemIndex={itemIndex} | ItemId={itemId}");

                var titleObj = item.m_Title;
                if (titleObj != null)
                {
                    string extracted = PnlStagePatchHelper.GetTitleTextFromArea(titleObj);
                    MelonLogger.Msg($"    - [Title] Type={titleObj.GetType().Name} | Name={titleObj.name} | Active={titleObj.gameObject.activeSelf} | Text={(string.IsNullOrEmpty(extracted)?"(null)":extracted)}");
                }

                var cellObj = item.m_Cell;
                if (cellObj != null)
                {
                    string cellText = PnlStagePatchHelper.GetTitleTextFromArea(cellObj);
                    MelonLogger.Msg($"    - [Cell] Name={cellObj.name} | Active={cellObj.gameObject.activeSelf} | Text={(string.IsNullOrEmpty(cellText)?"(null)":cellText)}");
                    if (string.IsNullOrEmpty(cellText) || cellText == "Untagged")
                    {
                        var dump = PnlStagePatchHelper.DumpStringMembersForDebug(cellObj);
                        MelonLogger.Msg("    - [Cell Debug Dump]\n" + dump);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            MelonLogger.Error($"[★] LogTagItemsAfterDelay 예외: {ex}");
        }
    }
}

// PnlStage.ChangeMusic(int) 후킹
[HarmonyLib.HarmonyPatch(typeof(PnlStage), "ChangeMusic", new Type[] { typeof(int) })]
public class PnlStage_ChangeMusic_Patch
{
    public static void Prefix(PnlStage __instance, int i)
    {
        try { }
        catch (Exception ex) { MelonLogger.Error($"PnlStage.ChangeMusic Prefix 예외: {ex}"); }
    }

    public static void Postfix(PnlStage __instance, int i)
    {
        try
        {
            PnlStagePatchHelper.ApplyCustomTagTitleAccessors("PnlStage.ChangeMusic", __instance);
            PnlMusicUtils.LogMusicInfo("PnlStage.ChangeMusic", __instance);
        }
        catch (Exception ex) { MelonLogger.Error($"PnlStage.ChangeMusic Postfix 예외: {ex}"); }
    }
}

// PnlStage.ChangeFinalMusic(int) 후킹
[HarmonyLib.HarmonyPatch(typeof(PnlStage), "ChangeFinalMusic", new Type[] { typeof(int) })]
public class PnlStage_ChangeFinalMusic_Patch
{
    public static void Prefix(PnlStage __instance, int i)
    {
        try { }
        catch (Exception ex) { MelonLogger.Error($"PnlStage.ChangeFinalMusic Prefix 예외: {ex}"); }
    }

    public static void Postfix(PnlStage __instance, int i)
    {
        try
        {
            PnlMusicUtils.LogMusicInfo("PnlStage.ChangeFinalMusic", __instance);
        }
        catch (Exception ex) { MelonLogger.Error($"PnlStage.ChangeFinalMusic Postfix 예외: {ex}"); }
    }
}

// PnlStage.RefreshTagTitle 후킹
[HarmonyLib.HarmonyPatch(typeof(PnlStage), "RefreshTagTitle")]
public class PnlStage_RefreshTagTitle_Patch
{
    public static void Prefix(PnlStage __instance)
    {
        PnlStagePatchHelper.LogPnlStageRefresh("PnlStage.RefreshTagTitle.Prefix", __instance);
    }

    public static void Postfix(PnlStage __instance)
    {
        PnlStagePatchHelper.LogPnlStageRefresh("PnlStage.RefreshTagTitle.Postfix", __instance);
    }
}

// PnlStage.musicNameTitle getter 후킹
[HarmonyLib.HarmonyPatch(typeof(PnlStage), nameof(PnlStage.musicNameTitle), HarmonyLib.MethodType.Getter)]
public class PnlStage_GetMusicNameTitle_Patch
{
    public static bool Prepare()
    {
        MelonLogger.Msg("[PnlStage.get_musicNameTitle] 접근자 후킹 준비 완료");
        return true;
    }

    public static void Postfix(PnlStage __instance, ref Text __result)
    {
        PnlStagePatchHelper.LogTextAccessor("PnlStage.get_musicNameTitle", __instance, __result);
    }
}

// PnlStage.artistNameTitle getter 후킹
[HarmonyLib.HarmonyPatch(typeof(PnlStage), nameof(PnlStage.artistNameTitle), HarmonyLib.MethodType.Getter)]
public class PnlStage_GetArtistNameTitle_Patch
{
    public static bool Prepare()
    {
        MelonLogger.Msg("[PnlStage.get_artistNameTitle] 접근자 후킹 준비 완료");
        return true;
    }

    public static void Postfix(PnlStage __instance, ref Text __result)
    {
        PnlStagePatchHelper.LogTextAccessor("PnlStage.get_artistNameTitle", __instance, __result);
    }
}
