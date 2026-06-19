using MelonLoader;
using muse_dash_test;
using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using Il2CppAssets.Scripts.UI.Panels;
using UnityEngine;
using UnityEngine.UI;

public static partial class PnlMusicUtils
{
    // 곡 제목 실험 모드: 여기만 수정하면 됩니다.
    // EnableSongTitleExperiment=false로 바꾸면 원본 UI를 건드리지 않고 로그만 출력합니다.
    private const bool EnableSongTitleExperiment = true;
    private const string ExperimentTitle = "화영왕 0";
    private const string ExperimentArtist = "화영왕 0";
    private const string ExperimentLevelDesignerLabel = "레벨 디자이너";
    private const string ExperimentLevelDesignerName = "화영왕 0";
    private static readonly bool ApplySongTitleExperimentGlobally = false;

    // 화면마다 텍스트 오브젝트 이름이 다를 수 있어서 후보를 여러 개 둡니다.
    private static readonly string[] TitleTextObjectNames = { "TxtSongTitle", "TxtSongName", "TxtSongName_Simple", "TxtSongName_Backup", "TxtMusicTitle", "TxtMusicName", "TxtTitle" };
    private static readonly string[] ArtistTextObjectNames = { "TxtArtist", "TxtArtistName", "TxtSongAuthor", "TxtSongAuthor_Simple", "TxtSongAuthor_Backup" };
    private static readonly string[] LevelDesignerLabelTextObjectNames = { "TxtStageDesigner", "TxtLevelDesigner", "TxtDesigner", "TxtLevelDesign", "TxtChartDesigner" };
    private static readonly string[] LevelDesignerNameTextObjectNames = { "ImgStageDesignerMask", "TxtStageDesignerName", "TxtDesignerName", "TxtLevelDesignName", "TxtChartDesignerName" };

    public static IEnumerator LogMusicInfoAfterDelay(string source, object pnlInstance, float delaySeconds)
    {
        yield return new WaitForSeconds(delaySeconds);
        LogMusicInfo(source, pnlInstance);
    }

    public static IEnumerator LogPreparationMusicInfoAfterDelay(object pnlInstance, string source, float delaySeconds)
    {
        yield return new WaitForSeconds(delaySeconds);
        LogPreparationMusicInfo(pnlInstance, source);
    }

    public static void LogMusicInfo(string source, object pnlInstance)
    {
        try
        {
            string resolvedUid = ResolveCustomMusicUid(pnlInstance);
            ApplySongTitleExperiment(source, pnlInstance, resolvedUid);
            var info = ExtractMusicInfo(pnlInstance, resolvedUid);
            LogCompact(source, info);
        }
        catch (Exception ex) { MelonLogger.Error($"LogMusicInfo 예외: {ex}"); }
    }

    public static void LogPreparationMusicInfo(object pnlInstance, string source = "PnlPreparation.Awake")
    {
        try
        {
            string resolvedUid = ResolveCustomMusicUid(pnlInstance);
            ApplySongTitleExperiment(source, pnlInstance, resolvedUid);
            var info = ExtractMusicInfo(pnlInstance, resolvedUid);
            if (!IsUsefulTitle(info.Title))
            {
                var stage = FindLivePnlStage();
                if (stage != null)
                {
                    ApplySongTitleExperiment(source + "->PnlStage", stage, resolvedUid);
                    var stageInfo = ExtractMusicInfo(stage);
                    if (IsUsefulTitle(stageInfo.Title)) info.Title = stageInfo.Title;
                    if (!string.IsNullOrWhiteSpace(stageInfo.Clip)) info.Clip = stageInfo.Clip;
                    if (!string.IsNullOrWhiteSpace(stageInfo.Artist)) info.Artist = stageInfo.Artist;
                    if (!string.IsNullOrWhiteSpace(stageInfo.LevelDesigner)) info.LevelDesigner = stageInfo.LevelDesigner;
                    if (string.IsNullOrWhiteSpace(info.ClipReason) || info.ClipReason == "AudioClip 후보 없음")
                        info.ClipReason = stageInfo.ClipReason;
                }
            }
            LogCompact(source, info);
        }
        catch (Exception ex) { MelonLogger.Error($"LogPreparationMusicInfo 예외: {ex}"); }
    }

    public static void DumpMusicInfo(object pnlInstance)
    {
        try
        {
            LogMusicInfo("MusicInfo", pnlInstance);
        }
        catch (Exception ex) { MelonLogger.Error($"DumpMusicInfo 예외: {ex}"); }
    }

    private class MusicInfo
    {
        public string Title;
        public string Clip;
        public string Artist;
        public string LevelDesigner;
        public string ClipReason;
    }

    private static void ApplySongTitleExperiment(string source, object pnlInstance, string resolvedUid)
    {
        if (!EnableSongTitleExperiment || pnlInstance == null) return;
        if (string.IsNullOrEmpty(resolvedUid)) return;

        string title = ExperimentTitle;
        string artist = ExperimentArtist;
        string designer = ExperimentLevelDesignerName;

        if (MainMod.TryGetCachedHwaPrimaryVirtualSong(
                resolvedUid,
                out string manifestTitle,
                out string manifestArtist,
                out string manifestLevelDesigner,
                out _, out _, out _, out _, out _, out _))
        {
            if (!string.IsNullOrWhiteSpace(manifestTitle)) title = manifestTitle;
            if (!string.IsNullOrWhiteSpace(manifestArtist)) artist = manifestArtist;
            if (!string.IsNullOrWhiteSpace(manifestLevelDesigner)) designer = manifestLevelDesigner;
        }
        else
        {
            var musicInfo = Il2CppAssets.Scripts.Database.GlobalDataBase.dbMusicTag?.GetMusicInfoFromAll(resolvedUid);
            if (musicInfo != null)
            {
                title = musicInfo.name;
                artist = musicInfo.author;
                designer = musicInfo.levelDesigner;
            }
        }

        SetMemberText(pnlInstance, "musicNameTitle", title);
        SetMemberText(pnlInstance, "songNameTitle", title);
        SetMemberText(pnlInstance, "titleText", title);
        SetMemberText(pnlInstance, "musicTitle", title);

        SetMemberText(pnlInstance, "artistNameTitle", artist);
        SetMemberText(pnlInstance, "artistText", artist);
        SetMemberText(pnlInstance, "artistName", artist);

        SetMemberText(pnlInstance, "levelDesignerName", designer);
        SetMemberText(pnlInstance, "levelDesignerText", ExperimentLevelDesignerLabel);
        SetMemberText(pnlInstance, "designerName", designer);
        SetMemberText(pnlInstance, "designerText", ExperimentLevelDesignerLabel);
        SetMemberText(pnlInstance, "chartDesignerName", designer);
        SetMemberText(pnlInstance, "stageDesignerName", designer);

        var root = GetRootGameObject(pnlInstance);
        if (root != null)
        {
            SetChildTextsBatch(root,
                (TitleTextObjectNames,                title),
                (ArtistTextObjectNames,               artist),
                (LevelDesignerLabelTextObjectNames,   ExperimentLevelDesignerLabel),
                (LevelDesignerNameTextObjectNames,    designer));
        }

        if (ApplySongTitleExperimentGlobally)
        {
            SetSceneTextByNameOrCurrentValue(TitleTextObjectNames, title, true);
            SetSceneTextByNameOrCurrentValue(ArtistTextObjectNames, artist, false);
            SetSceneTextByNameOrCurrentValue(LevelDesignerLabelTextObjectNames, ExperimentLevelDesignerLabel, false);
            SetSceneTextByNameOrCurrentValue(LevelDesignerNameTextObjectNames, designer, false);
        }
    }

    private static string ResolveCustomMusicUid(object pnlInstance)
    {
        string uid = TryFindCustomMusicUidInObject(pnlInstance, 0, new HashSet<object>());
        if (!string.IsNullOrEmpty(uid)) return uid;

        uid = CustomPlaySession.Current.SelectedMusicUid;
        if (IsCustomMusicUid(uid)) return uid;

        uid = PnlStagePatchHelper.GetCurrentSelectedMusicUid();
        if (IsCustomMusicUid(uid)) return uid;

        uid = CustomPlaySession.Current.LastClickedMusicUid;
        if (IsCustomMusicUid(uid)) return uid;

        return null;
    }

    private static string TryFindCustomMusicUidInObject(object obj, int depth, HashSet<object> visited)
    {
        if (obj == null || depth > 2) return null;
        if (obj is string text) return IsCustomMusicUid(text) ? text : null;
        if (obj is UnityEngine.Object unityObject && !unityObject) return null;
        if (!visited.Add(obj)) return null;

        try
        {
            if (obj is Il2CppAssets.Scripts.Database.MusicInfo musicInfo && IsCustomMusicUid(musicInfo.uid))
            {
                return musicInfo.uid;
            }

            Type type = obj.GetType();
            foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                string uid = TryReadCustomMusicUid(field.FieldType, () => field.GetValue(obj), depth, visited);
                if (!string.IsNullOrEmpty(uid)) return uid;
            }

            foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                if (!property.CanRead || property.GetIndexParameters().Length != 0) continue;
                string uid = TryReadCustomMusicUid(property.PropertyType, () => property.GetValue(obj), depth, visited);
                if (!string.IsNullOrEmpty(uid)) return uid;
            }
        }
        catch { }

        return null;
    }

    private static string TryReadCustomMusicUid(Type memberType, Func<object> read, int depth, HashSet<object> visited)
    {
        try
        {
            bool promisingType = memberType == typeof(string)
                || memberType == typeof(Il2CppAssets.Scripts.Database.MusicInfo)
                || IsMusicLike(memberType);
            if (!promisingType) return null;

            object value = read();
            if (value == null) return null;

            if (value is string text)
            {
                return IsCustomMusicUid(text) ? text : null;
            }

            if (value is Il2CppAssets.Scripts.Database.MusicInfo musicInfo)
            {
                return IsCustomMusicUid(musicInfo.uid) ? musicInfo.uid : null;
            }

            return TryFindCustomMusicUidInObject(value, depth + 1, visited);
        }
        catch
        {
            return null;
        }
    }

    private static bool IsCustomMusicUid(string uid)
    {
        return CustomContentIds.IsVirtualSong(uid);
    }

    private static int SetMemberText(object obj, string memberName, string value)
    {
        try
        {
            if (obj == null || string.IsNullOrEmpty(memberName)) return 0;
            object target = ModReflection.GetValue(obj, memberName, silent: true);
            if (target != null)
            {
                return SetTextValue(target, value);
            }
        }
        catch { }
        return 0;
    }

    private static int SetTextValue(object target, string value)
    {
        try
        {
            if (target == null) return 0;

            if (target is Text unityText)
            {
                unityText.text = value;
                return 1;
            }

            if (ModReflection.SetValue(target, "text", value, silent: true))
            {
                return 1;
            }
        }
        catch { }
        return 0;
    }

    private static GameObject GetRootGameObject(object obj)
    {
        try
        {
            if (obj is GameObject go) return go;
            if (obj is Component component) return component.gameObject;

            var memberGameObject = GetMemberObject(obj, "gameObject") as GameObject;
            if (memberGameObject != null) return memberGameObject;
        }
        catch { }
        return null;
    }

    private static object GetMemberObject(object obj, string memberName)
    {
        return ModReflection.GetValue(obj, memberName, silent: true);
    }

    // 4번의 GetComponentsInChildren 호출을 1번으로 줄인 배치 버전
    private static void SetChildTextsBatch(GameObject root, params (string[] names, string value)[] pairs)
    {
        if (root == null || pairs == null || pairs.Length == 0) return;
        try
        {
            var transforms = root.GetComponentsInChildren<Transform>(true);
            if (transforms == null) return;
            foreach (var trans in transforms)
            {
                try
                {
                    if (trans == null || trans.gameObject == null) continue;
                    var go = trans.gameObject;
                    foreach (var (names, value) in pairs)
                    {
                        if (!NameMatches(go.name, names)) continue;
                        var components = go.GetComponents<Component>();
                        if (components != null)
                        {
                            foreach (var comp in components)
                            {
                                if (comp == null || comp is Transform || comp.GetType().Name == "CanvasRenderer") continue;
                                SetTextValue(comp, value);
                            }
                        }
                        SetAllTextUnder(go, value);
                    }
                }
                catch { }
            }
        }
        catch (Exception ex)
        {
            MelonLogger.Error($"[PnlMusicUtils] SetChildTextsBatch 중 예외: {ex.Message}");
        }
    }

    private static int SetChildTextByNames(GameObject root, string[] objectNames, string value)
    {
        int writes = 0;
        if (root == null || objectNames == null) return writes;

        try
        {
            // 1. 하위의 모든 Transform(오브젝트 노드)을 수집하여 이름이 일치하는 타겟 오브젝트만 1차 초고속 선별
            var transforms = root.GetComponentsInChildren<Transform>(true);
            if (transforms != null)
            {
                foreach (var trans in transforms)
                {
                    try
                    {
                        if (trans == null || trans.gameObject == null) continue;

                        // GameObject 이름이 찾고자 하는 텍스트 오브젝트 후보와 일치하는 경우에만 핀포인트 진입
                        if (NameMatches(trans.gameObject.name, objectNames))
                        {
                            var go = trans.gameObject;

                            // 해당 오브젝트에 존재하는 컴포넌트들만 조회 (전체 트리 스캔 배제)
                            var components = go.GetComponents<Component>();
                            if (components != null)
                            {
                                foreach (var comp in components)
                                {
                                    if (comp == null || comp is Transform || comp.GetType().Name == "CanvasRenderer") continue;
                                    writes += SetTextValue(comp, value);
                                }
                            }

                            // 하위 모든 텍스트 강제 동기화
                            writes += SetAllTextUnder(go, value);
                        }
                    }
                    catch { }
                }
            }
        }
        catch (Exception ex)
        {
            MelonLogger.Error($"[PnlMusicUtils] SetChildTextByNames 최적화 스캔 중 예외: {ex.Message}");
        }

        return writes;
    }

    private static int SetAllTextUnder(GameObject root, string value)
    {
        int writes = 0;
        if (root == null) return writes;

        // 1. 표준 Text 컴포넌트는 즉시 처리
        try
        {
            var texts = root.GetComponentsInChildren<Text>(true);
            if (texts != null)
            {
                foreach (var text in texts)
                {
                    if (text == null) continue;
                    text.text = value;
                    writes++;
                }
            }
        }
        catch { }

        // 2. 기타 텍스트 오브젝트 후보군에 대해 핀포인트 처리
        try
        {
            var transforms = root.GetComponentsInChildren<Transform>(true);
            if (transforms != null)
            {
                foreach (var trans in transforms)
                {
                    if (trans == null || trans.gameObject == null) continue;
                    var go = trans.gameObject;

                    // 표준 Text 컴포넌트는 이미 처리했으므로 제외하고 나머지 컴포넌트만 탐색
                    var components = go.GetComponents<Component>();
                    if (components != null)
                    {
                        foreach (var comp in components)
                        {
                            if (comp == null || comp is Text || comp is Transform || comp.GetType().Name == "CanvasRenderer") continue;
                            writes += SetTextValue(comp, value);
                        }
                    }
                }
            }
        }
        catch { }

        return writes;
    }

}
