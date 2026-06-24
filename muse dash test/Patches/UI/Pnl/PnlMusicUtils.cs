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

    // 패널 버전마다 멤버명이 달라 후보를 여러 개 두고 일괄 주입합니다.
    private static readonly string[] TitleMemberNames = { "musicNameTitle", "songNameTitle", "titleText", "musicTitle" };
    private static readonly string[] ArtistMemberNames = { "artistNameTitle", "artistText", "artistName" };
    private static readonly string[] DesignerNameMemberNames = { "levelDesignerName", "designerName", "chartDesignerName", "stageDesignerName" };
    private static readonly string[] DesignerLabelMemberNames = { "levelDesignerText", "designerText" };

    public static IEnumerator ApplyAndLogMusicInfoAfterDelay(string source, object pnlInstance, float delaySeconds)
    {
        yield return new WaitForSeconds(delaySeconds);
        ApplyAndLogMusicInfo(source, pnlInstance);
    }

    public static IEnumerator DelayedApplyPrepMusicInfo(object pnlInstance, string source, float delaySeconds)
    {
        yield return new WaitForSeconds(delaySeconds);
        ApplyPrepMusicInfo(pnlInstance, source);
    }

    /// <summary>
    /// 곡 정보를 로그로 남기는 동시에, 커스텀곡일 때는 화면 텍스트(제목/아티스트/디자이너)를
    /// 커스텀 값으로 "덮어씁니다". 읽기 전용이 아님에 주의(ApplySongTitleExperiment 호출).
    /// </summary>
    public static void ApplyAndLogMusicInfo(string source, object pnlInstance)
    {
        try
        {
            string resolvedUid = ResolveCustomMusicUid(pnlInstance);
            ApplySongTitleExperiment(source, pnlInstance, resolvedUid);
            var info = ExtractMusicInfo(pnlInstance, resolvedUid);
            LogCompact(source, info);
        }
        catch (Exception ex) { MelonLogger.Error($"ApplyAndLogMusicInfo 예외: {ex}"); }
    }

    /// <summary>
    /// 준비화면의 곡 정보를 로그로 남기는 동시에, 커스텀곡일 때는 화면의 제목/아티스트/디자이너
    /// 텍스트를 커스텀 값으로 "덮어씁니다"(ApplySongTitleExperiment 호출). 이름만 보고 읽기 전용
    /// 진단으로 오해하기 쉬우니 주의. 현재 선택이 순정곡이면 ResolveCustomMusicUid가 null을 반환해
    /// 아무것도 쓰지 않습니다.
    /// </summary>
    public static void ApplyPrepMusicInfo(object pnlInstance, string source = "PnlPreparation.Awake")
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
        catch (Exception ex) { MelonLogger.Error($"ApplyPrepMusicInfo 예외: {ex}"); }
    }

    public static void DumpMusicInfo(object pnlInstance)
    {
        try
        {
            ApplyAndLogMusicInfo("MusicInfo", pnlInstance);
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

        if (MainMod.TryGetHwaPrimarySong(
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

        // ★주의: 이 함수는 진단 "로그"만 남기는 게 아니라 실제로 화면 텍스트를 덮어씁니다(쓰기).
        // 다음에 같은 잔상 버그를 빨리 잡을 수 있도록, 무엇을 어떤 uid 기준으로 쓰는지 명시적으로 남깁니다.
        // (resolvedUid가 순정곡이면 위에서 이미 early-return 되므로, 이 줄이 찍혔다 = 커스텀 오버라이드를 실제로 적용했다는 뜻)
        MelonLogger.Msg($"[SongTitleOverride] 화면 텍스트 적용(WRITE): source={source}, uid={resolvedUid}, title='{title}', artist='{artist}', designer='{designer}'");

        SetMemberTexts(pnlInstance, TitleMemberNames, title);
        SetMemberTexts(pnlInstance, ArtistMemberNames, artist);
        SetMemberTexts(pnlInstance, DesignerNameMemberNames, designer);
        SetMemberTexts(pnlInstance, DesignerLabelMemberNames, ExperimentLevelDesignerLabel);

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
            SetSceneTextsGlobally(title, artist, designer);
        }
    }

    private static string ResolveCustomMusicUid(object pnlInstance)
    {
        // 현재 실제 선택된 곡을 먼저 확정합니다. 선택이 잡히면 그 곡이 커스텀(1999-)일 때만
        // 커스텀 uid로 인정하고, 순정곡이면 즉시 null을 반환합니다.
        // (객체 그래프 스캔을 먼저 하면 현재 선택과 무관한 다른 커스텀곡 uid를 집어와,
        //  순정곡 준비화면에 직전 커스텀 제목이 잔상으로 박히는 버그가 발생했습니다.)
        string selected = CustomPlaySession.Current.SelectedMusicUid;
        if (string.IsNullOrEmpty(selected)) selected = PnlStagePatchHelper.GetCurrentSelectedMusicUid();
        if (!string.IsNullOrEmpty(selected))
        {
            return IsCustomMusicUid(selected) ? selected : null;
        }

        // 현재 선택을 전혀 알 수 없을 때만 객체 그래프/마지막 클릭으로 폴백합니다.
        string uid = TryFindCustomMusicUidInObject(pnlInstance, 0, new HashSet<object>());
        if (!string.IsNullOrEmpty(uid)) return uid;

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

    private static void SetMemberTexts(object obj, string[] memberNames, string value)
    {
        if (obj == null || memberNames == null) return;
        foreach (var memberName in memberNames)
        {
            SetMemberText(obj, memberName, value);
        }
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
