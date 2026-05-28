using MelonLoader;
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
            ApplySongTitleExperiment(source, pnlInstance);
            var info = ExtractMusicInfo(pnlInstance);
            LogCompact(source, info);
        }
        catch (Exception ex) { MelonLogger.Error($"LogMusicInfo 예외: {ex}"); }
    }

    public static void LogPreparationMusicInfo(object pnlInstance, string source = "PnlPreparation.Awake")
    {
        try
        {
            ApplySongTitleExperiment(source, pnlInstance);
            var info = ExtractMusicInfo(pnlInstance);
            if (!IsUsefulTitle(info.Title))
            {
                var stage = FindLivePnlStage();
                if (stage != null)
                {
                    ApplySongTitleExperiment(source + "->PnlStage", stage);
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

    private static void ApplySongTitleExperiment(string source, object pnlInstance)
    {
        if (!EnableSongTitleExperiment || pnlInstance == null) return;

        // 통합 조건 검사 적용
        if (!PnlStagePatchHelper.ShouldApplyHwayoungwang())
        {
            return;
        }

        string selectedUid = PnlStagePatchHelper.GetCurrentSelectedMusicUid();
        if (string.IsNullOrEmpty(selectedUid) || selectedUid == "(null)")
        {
            selectedUid = muse_dash_test.MusicButtonCell_OnButtonClicked_Patch.LastClickedMusicUid;
        }

        string title = ExperimentTitle;
        string artist = ExperimentArtist;
        string designer = ExperimentLevelDesignerName;

        if (!string.IsNullOrEmpty(selectedUid))
        {
            var musicInfo = Il2CppAssets.Scripts.Database.GlobalDataBase.dbMusicTag?.GetMusicInfoFromAll(selectedUid);
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
            SetChildTextByNames(root, TitleTextObjectNames, title);
            SetChildTextByNames(root, ArtistTextObjectNames, artist);
            SetChildTextByNames(root, LevelDesignerLabelTextObjectNames, ExperimentLevelDesignerLabel);
            SetChildTextByNames(root, LevelDesignerNameTextObjectNames, designer);
        }

        if (ApplySongTitleExperimentGlobally)
        {
            SetSceneTextByNameOrCurrentValue(TitleTextObjectNames, title, true);
            SetSceneTextByNameOrCurrentValue(ArtistTextObjectNames, artist, false);
            SetSceneTextByNameOrCurrentValue(LevelDesignerLabelTextObjectNames, ExperimentLevelDesignerLabel, false);
            SetSceneTextByNameOrCurrentValue(LevelDesignerNameTextObjectNames, designer, false);
        }
    }

    private static int SetMemberText(object obj, string memberName, string value)
    {
        try
        {
            if (obj == null || string.IsNullOrEmpty(memberName)) return 0;
            var ty = obj.GetType();

            var p = ty.GetProperty(memberName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (p != null && p.GetIndexParameters().Length == 0)
                return SetTextValue(p.GetValue(obj), value);

            var f = ty.GetField(memberName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (f != null)
                return SetTextValue(f.GetValue(obj), value);
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

            var ty = target.GetType();
            var textProp = ty.GetProperty("text", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (textProp != null && textProp.CanWrite)
            {
                textProp.SetValue(target, value);
                return 1;
            }

            var mTextProp = ty.GetProperty("m_Text", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (mTextProp != null && mTextProp.CanWrite)
            {
                mTextProp.SetValue(target, value);
                return 1;
            }

            var mTextField = ty.GetField("m_Text", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (mTextField != null)
            {
                mTextField.SetValue(target, value);
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
        try
        {
            if (obj == null) return null;
            var ty = obj.GetType();
            var p = ty.GetProperty(memberName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (p != null && p.GetIndexParameters().Length == 0) return p.GetValue(obj);
            var f = ty.GetField(memberName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (f != null) return f.GetValue(obj);
        }
        catch { }
        return null;
    }

    private static int SetChildTextByNames(GameObject root, string[] objectNames, string value)
    {
        int writes = 0;
        if (root == null || objectNames == null) return writes;

        try
        {
            var texts = root.GetComponentsInChildren<Text>(true);
            foreach (var text in texts)
            {
                try
                {
                    if (text == null || text.gameObject == null) continue;
                    if (!NameMatches(text.name, objectNames) && !NameMatches(text.gameObject.name, objectNames)) continue;
                    text.text = value;
                    writes++;
                    writes += SetAllTextUnder(text.gameObject, value);
                }
                catch { }
            }
        }
        catch { }

        try
        {
            var components = root.GetComponentsInChildren<Component>(true);
            foreach (var component in components)
            {
                try
                {
                    if (component == null || component.gameObject == null) continue;
                    if (!NameMatches(component.name, objectNames) && !NameMatches(component.gameObject.name, objectNames)) continue;
                    writes += SetTextValue(component, value);
                    writes += SetAllTextUnder(component.gameObject, value);
                }
                catch { }
            }
        }
        catch { }

        return writes;
    }

    private static int SetAllTextUnder(GameObject root, string value)
    {
        int writes = 0;
        if (root == null) return writes;

        try
        {
            var texts = root.GetComponentsInChildren<Text>(true);
            foreach (var text in texts)
            {
                try
                {
                    if (text == null) continue;
                    text.text = value;
                    writes++;
                }
                catch { }
            }
        }
        catch { }

        try
        {
            var components = root.GetComponentsInChildren<Component>(true);
            foreach (var component in components)
            {
                try
                {
                    if (component == null) continue;
                    writes += SetTextValue(component, value);
                }
                catch { }
            }
        }
        catch { }

        return writes;
    }

}
