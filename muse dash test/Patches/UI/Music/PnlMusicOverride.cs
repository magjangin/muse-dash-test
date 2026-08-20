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
        // 아래 이름의 "Experiment" 접두사는 초기 실험 코드의 잔재일 뿐, 현재는 정식 기능이다.
        // EnableSongTitleExperiment는 항상 true(상시 활성)이고, Experiment* 문자열들은
        // 매니페스트/musicInfo 조회가 모두 실패했을 때 쓰는 폴백 기본 표시 텍스트다.
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

        private sealed class CachedPanelBinding
        {
            public readonly List<Text> TitleTexts = new List<Text>();
            public readonly List<Text> ArtistTexts = new List<Text>();
            public readonly List<Text> DesignerLabelTexts = new List<Text>();
            public readonly List<Text> DesignerNameTexts = new List<Text>();

            public bool IsAlive()
            {
                if (TitleTexts.Count > 0 && TitleTexts[0] == null) return false;
                if (ArtistTexts.Count > 0 && ArtistTexts[0] == null) return false;
                return true;
            }
        }

        private static readonly Dictionary<int, CachedPanelBinding> BindingCache = new Dictionary<int, CachedPanelBinding>();

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

            if (HwaResourceManager.TryGetHwaPrimarySong(
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

            ModLogger.Msg($"[SongTitleOverride] 화면 텍스트 적용(WRITE): source={source}, uid={resolvedUid}, title='{title}', artist='{artist}', designer='{designer}'");

            SetMemberTexts(pnlInstance, TitleMemberNames, title);
            SetMemberTexts(pnlInstance, ArtistMemberNames, artist);
            SetMemberTexts(pnlInstance, DesignerNameMemberNames, designer);
            SetMemberTexts(pnlInstance, DesignerLabelMemberNames, ExperimentLevelDesignerLabel);

            var root = GetRootGameObject(pnlInstance);
            if (root != null)
            {
                ApplyCachedChildTexts(root, title, artist, designer, ExperimentLevelDesignerLabel);
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
            catch (Exception) { }
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
                catch (Exception) { }
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
            catch (Exception) { }
            return 0;
        }

        private static void ApplyCachedChildTexts(GameObject root, string title, string artist, string designer, string designerLabel)
        {
            if (root == null) return;
            int rootId = root.GetInstanceID();

            if (!BindingCache.TryGetValue(rootId, out var binding) || !binding.IsAlive())
            {
                binding = BuildBinding(root);
                BindingCache[rootId] = binding;
            }

            for (int i = 0; i < binding.TitleTexts.Count; i++)
            {
                var t = binding.TitleTexts[i];
                if (t != null) t.text = title;
            }

            for (int i = 0; i < binding.ArtistTexts.Count; i++)
            {
                var t = binding.ArtistTexts[i];
                if (t != null) t.text = artist;
            }

            for (int i = 0; i < binding.DesignerLabelTexts.Count; i++)
            {
                var t = binding.DesignerLabelTexts[i];
                if (t != null) t.text = designerLabel;
            }

            for (int i = 0; i < binding.DesignerNameTexts.Count; i++)
            {
                var t = binding.DesignerNameTexts[i];
                if (t != null) t.text = designer;
            }
        }

        private static CachedPanelBinding BuildBinding(GameObject root)
        {
            var binding = new CachedPanelBinding();
            try
            {
                var texts = root.GetComponentsInChildren<Text>(true);
                if (texts != null)
                {
                    foreach (var text in texts)
                    {
                        if (text == null) continue;
                        string goName = text.gameObject.name;

                        if (NameMatches(goName, TitleTextObjectNames))
                        {
                            binding.TitleTexts.Add(text);
                        }
                        else if (NameMatches(goName, ArtistTextObjectNames))
                        {
                            binding.ArtistTexts.Add(text);
                        }
                        else if (NameMatches(goName, LevelDesignerLabelTextObjectNames))
                        {
                            binding.DesignerLabelTexts.Add(text);
                        }
                        else if (NameMatches(goName, LevelDesignerNameTextObjectNames))
                        {
                            binding.DesignerNameTexts.Add(text);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ModLogger.Error($"[PnlMusicOverride] BuildBinding 예외: {ex.Message}");
            }
            return binding;
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
