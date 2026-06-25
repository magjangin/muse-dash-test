using MelonLoader;
using System;
using System.Reflection;
using System.Collections.Generic;
using Il2CppAssets.Scripts.UI.Panels;
using UnityEngine;
using UnityEngine.UI;

namespace muse_dash_test
{
    /// <summary>
    /// 곡 제목, 아티스트, 레벨 디자이너 등의 UI 텍스트 오버라이드 및 쓰기 로직을 담당하는 클래스입니다.
    /// </summary>
    public static class PnlMusicOverride
    {
        private const bool EnableSongTitleExperiment = true;
        private const string ExperimentTitle = "Custom Chart";
        private const string ExperimentArtist = "Custom Artist";
        private const string ExperimentLevelDesignerLabel = "레벨 디자이너";
        private const string ExperimentLevelDesignerName = "Custom Designer";

        private static readonly string[] TitleTextObjectNames = { "TxtSongTitle", "TxtSongName", "TxtSongName_Simple", "TxtSongName_Backup", "TxtMusicTitle", "TxtMusicName", "TxtTitle" };
        private static readonly string[] ArtistTextObjectNames = { "TxtArtist", "TxtArtistName", "TxtSongAuthor", "TxtSongAuthor_Simple", "TxtSongAuthor_Backup" };
        private static readonly string[] LevelDesignerLabelTextObjectNames = { "TxtStageDesigner", "TxtLevelDesigner", "TxtDesigner", "TxtLevelDesign", "TxtChartDesigner" };
        private static readonly string[] LevelDesignerNameTextObjectNames = { "ImgStageDesignerMask", "TxtStageDesignerName", "TxtDesignerName", "TxtLevelDesignName", "TxtChartDesignerName" };

        private static readonly string[] TitleMemberNames = { "musicNameTitle", "songNameTitle", "titleText", "musicTitle" };
        private static readonly string[] ArtistMemberNames = { "artistNameTitle", "artistText", "artistName" };
        private static readonly string[] DesignerNameMemberNames = { "levelDesignerName", "designerName", "chartDesignerName", "stageDesignerName" };
        private static readonly string[] DesignerLabelMemberNames = { "levelDesignerText", "designerText" };

        /// <summary>
        /// 지정한 패널의 텍스트 멤버 및 자식 UI 컴포넌트들을 찾아서 커스텀 곡 정보로 오버라이드합니다.
        /// </summary>
        public static void ApplySongTitleOverride(string source, object pnlInstance, string resolvedUid)
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
        }

        private static GameObject GetRootGameObject(object obj)
        {
            try
            {
                if (obj is GameObject go) return go;
                if (obj is Component component) return component.gameObject;

                var memberGameObject = ModReflection.GetValue(obj, "gameObject", silent: true) as GameObject;
                if (memberGameObject != null) return memberGameObject;
            }
            catch { }
            return null;
        }

        private static void SetMemberTexts(object obj, string[] memberNames, string value)
        {
            if (obj == null || memberNames == null) return;
            foreach (var memberName in memberNames)
            {
                try
                {
                    object target = ModReflection.GetValue(obj, memberName, silent: true);
                    if (target != null)
                    {
                        SetTextValue(target, value);
                    }
                }
                catch { }
            }
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
                MelonLogger.Error($"[PnlMusicOverride] SetChildTextsBatch 중 예외: {ex.Message}");
            }
        }

        private static int SetAllTextUnder(GameObject root, string value)
        {
            int writes = 0;
            if (root == null) return writes;

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

            try
            {
                var transforms = root.GetComponentsInChildren<Transform>(true);
                if (transforms != null)
                {
                    foreach (var trans in transforms)
                    {
                        if (trans == null || trans.gameObject == null) continue;
                        var go = trans.gameObject;

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

        private static bool NameMatches(string name, string[] candidates)
        {
            if (string.IsNullOrEmpty(name) || candidates == null) return false;
            foreach (var candidate in candidates)
            {
                if (string.Equals(name, candidate, StringComparison.OrdinalIgnoreCase)) return true;
            }
            return false;
        }
    }
}
