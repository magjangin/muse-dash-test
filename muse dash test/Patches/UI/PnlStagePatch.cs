using MelonLoader;
using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
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
            if (!string.IsNullOrEmpty(selectedUid))
            {
                if (__instance.gameObject.name == "ImgAlbumTittle")
                {
                    if (selectedUid == CustomMusicUid && PnlStagePatchHelper.IsCustomAlbumContext(CustomTagUid, CustomMusicUid))
                    {
                        text = CustomAlbumTitle;
                        MelonLogger.Msg($"[LongSongNameController.Refresh] 앨범 제목 강제 변경 -> {CustomAlbumTitle} (UID: {selectedUid})");
                    }
                }
                else if (__instance.gameObject.name == "ImgSongTitleMask")
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

// PnlMusicTagScrollView.InitListView 테스트 후킹 패치
[HarmonyLib.HarmonyPatch(typeof(Il2Cpp.PnlMusicTagScrollView), nameof(Il2Cpp.PnlMusicTagScrollView.InitListView))]
public class PnlMusicTagScrollView_InitListView_Patch
{
    public static void Prefix(Il2Cpp.PnlMusicTagScrollView __instance, int itemTotalCount)
    {
        try
        {
            MelonLogger.Msg($"[★ PnlMusicTagScrollView.InitListView 후킹 성공! ★] 호출됨 | itemTotalCount={itemTotalCount}");
        }
        catch (Exception ex)
        {
            MelonLogger.Error($"PnlMusicTagScrollView.InitListView Prefix 예외: {ex}");
        }
    }
}

// 로컬 PnlStage 정보 조회 헬퍼
public static class PnlStagePatchHelper
{
    private const BindingFlags InstanceMembers =
        BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

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
                if (album != null && album.uid == "998-0" && album.title == "실험 앨범")
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
            MelonLogger.Error($"IsCustomAlbumContext 예외: {ex}");
        }

        return false;
    }

    public static string GetCurrentSelectedMusicUid()
    {
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
            MelonLogger.Error($"GetCurrentSelectedMusicUid 예외: {ex}");
        }
        return null;
    }

    public static string GetTitleTextFromArea(object area)
    {
        try
        {
            if (area == null) return null;

            var type = area.GetType();

            // Step 1: own plain string properties (excluding Unity meta names)
            foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                if (!prop.CanRead || prop.PropertyType != typeof(string)) continue;
                var pn = prop.Name ?? string.Empty;
                if (pn.Equals("tag", StringComparison.OrdinalIgnoreCase) ||
                    pn.Equals("name", StringComparison.OrdinalIgnoreCase) ||
                    pn.Equals("gameObject", StringComparison.OrdinalIgnoreCase) ||
                    pn.Equals("transform", StringComparison.OrdinalIgnoreCase) ||
                    pn.Equals("ItemPrefabName", StringComparison.OrdinalIgnoreCase))
                    continue;
                try
                {
                    var v = prop.GetValue(area) as string;
                    if (!string.IsNullOrEmpty(v)) return v;
                }
                catch { }
            }

            // Step 2: get the GameObject and use typed GetComponentsInChildren (no Reflection for text read)
            var goProp = type.GetProperty("gameObject", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            var go = goProp?.GetValue(area) as UnityEngine.GameObject;

            if (go != null)
            {
                var allTexts = go.GetComponentsInChildren<UnityEngine.UI.Text>(true);

                // Prefer UI.Text on child GameObjects with a song-title name
                foreach (var t in allTexts)
                {
                    if (t == null) continue;
                    var goName = t.gameObject?.name ?? string.Empty;
                    if (goName.IndexOf("SongTitle", StringComparison.OrdinalIgnoreCase) >= 0 ||
                        goName.IndexOf("TxtTitle", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        if (!string.IsNullOrEmpty(t.text)) return t.text;
                    }
                }

                // Fall back to any UI.Text child
                foreach (var t in allTexts)
                {
                    if (t == null) continue;
                    if (!string.IsNullOrEmpty(t.text)) return t.text;
                }
            }
        }
        catch (Exception ex)
        {
            MelonLogger.Error($"GetTitleTextFromArea 예외: {ex}");
        }
        return null;
    }

    public static string DumpStringMembersForDebug(object area, int maxComponents = 20, int maxMembersPerComp = 10)
    {
        try
        {
            if (area == null) return "(area null)";
            var sb = new System.Text.StringBuilder();
            var type = area.GetType();

            // If has gameObject, inspect its components
            var goProp = type.GetProperty("gameObject", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            UnityEngine.GameObject go = null;
            if (goProp != null) go = goProp.GetValue(area) as UnityEngine.GameObject;

            if (go == null)
            {
                sb.AppendLine($"No GameObject on area type {type.Name}");
                // also list string props/fields on the area itself
                int found = 0;
                foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
                {
                    if (found >= maxMembersPerComp) break;
                    if (prop.PropertyType == typeof(string) && prop.CanRead)
                    {
                        var v = prop.GetValue(area) as string;
                        sb.AppendLine($"Prop {prop.Name} = '{v}'");
                        found++;
                    }
                }
                foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
                {
                    if (found >= maxMembersPerComp) break;
                    if (field.FieldType == typeof(string))
                    {
                        var v = field.GetValue(area) as string;
                        sb.AppendLine($"Field {field.Name} = '{v}'");
                        found++;
                    }
                }
                return sb.ToString();
            }

            var comps = go.GetComponentsInChildren<UnityEngine.Component>(true);
            int compCount = 0;
            foreach (var c in comps)
            {
                if (compCount++ >= maxComponents) break;
                if (c == null) continue;
                var ct = c.GetType();
                sb.AppendLine($"Component: {ct.FullName}");
                int memberCount = 0;
                foreach (var prop in ct.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
                {
                    if (memberCount++ >= maxMembersPerComp) break;
                    if (!prop.CanRead) continue;
                    string val = null;
                    try { var obj = prop.GetValue(c); if (obj != null) val = obj.ToString(); } catch { val = "(err)"; }
                    if (!string.IsNullOrEmpty(val)) sb.AppendLine($"  Prop {prop.Name} = '{val}'");
                }
                foreach (var field in ct.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
                {
                    if (memberCount++ >= maxMembersPerComp) break;
                    string val = null;
                    try { var obj = field.GetValue(c); if (obj != null) val = obj.ToString(); } catch { val = "(err)"; }
                    if (!string.IsNullOrEmpty(val)) sb.AppendLine($"  Field {field.Name} = '{val}'");
                }
            }
            return sb.ToString();
        }
        catch (Exception ex)
        {
            return $"Dump error: {ex}";
        }
    }
}
