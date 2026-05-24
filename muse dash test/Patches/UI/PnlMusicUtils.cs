using MelonLoader;
using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using Il2CppAssets.Scripts.UI.Panels;
using UnityEngine;
using UnityEngine.UI;

public static class PnlMusicUtils
{
    // 곡 제목 실험 모드: 여기만 수정하면 됩니다.
    // EnableSongTitleExperiment=false로 바꾸면 원본 UI를 건드리지 않고 로그만 출력합니다.
    private const bool EnableSongTitleExperiment = false;
    private const string ExperimentTitle = "화영왕";
    private const string ExperimentArtist = "화영왕";
    private const string ExperimentLevelDesignerLabel = "레벨 디자이너";
    private const string ExperimentLevelDesignerName = "화영왕";
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

        SetMemberText(pnlInstance, "musicNameTitle", ExperimentTitle);
        SetMemberText(pnlInstance, "songNameTitle", ExperimentTitle);
        SetMemberText(pnlInstance, "titleText", ExperimentTitle);
        SetMemberText(pnlInstance, "musicTitle", ExperimentTitle);

        SetMemberText(pnlInstance, "artistNameTitle", ExperimentArtist);
        SetMemberText(pnlInstance, "artistText", ExperimentArtist);
        SetMemberText(pnlInstance, "artistName", ExperimentArtist);

        SetMemberText(pnlInstance, "levelDesignerName", ExperimentLevelDesignerName);
        SetMemberText(pnlInstance, "levelDesignerText", ExperimentLevelDesignerLabel);
        SetMemberText(pnlInstance, "designerName", ExperimentLevelDesignerName);
        SetMemberText(pnlInstance, "designerText", ExperimentLevelDesignerLabel);
        SetMemberText(pnlInstance, "chartDesignerName", ExperimentLevelDesignerName);
        SetMemberText(pnlInstance, "stageDesignerName", ExperimentLevelDesignerName);

        var root = GetRootGameObject(pnlInstance);
        if (root != null)
        {
            SetChildTextByNames(root, TitleTextObjectNames, ExperimentTitle);
            SetChildTextByNames(root, ArtistTextObjectNames, ExperimentArtist);
            SetChildTextByNames(root, LevelDesignerLabelTextObjectNames, ExperimentLevelDesignerLabel);
            SetChildTextByNames(root, LevelDesignerNameTextObjectNames, ExperimentLevelDesignerName);
        }

        if (ApplySongTitleExperimentGlobally)
        {
            SetSceneTextByNameOrCurrentValue(TitleTextObjectNames, ExperimentTitle, true);
            SetSceneTextByNameOrCurrentValue(ArtistTextObjectNames, ExperimentArtist, false);
            SetSceneTextByNameOrCurrentValue(LevelDesignerLabelTextObjectNames, ExperimentLevelDesignerLabel, false);
            SetSceneTextByNameOrCurrentValue(LevelDesignerNameTextObjectNames, ExperimentLevelDesignerName, false);
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

    private static int SetSceneTextByNameOrCurrentValue(string[] objectNames, string value, bool titleMode)
    {
        int writes = 0;
        try
        {
            var texts = UnityEngine.Object.FindObjectsOfType<Text>();
            foreach (var text in texts)
            {
                try
                {
                    if (text == null || text.gameObject == null) continue;
                    if (!ShouldRewriteSceneText(text, objectNames, titleMode)) continue;
                    text.text = value;
                    writes++;
                }
                catch { }
            }
        }
        catch { }
        return writes;
    }

    private static bool ShouldRewriteSceneText(Text text, string[] objectNames, bool titleMode)
    {
        string objectName = text.gameObject != null ? text.gameObject.name : text.name;
        string lowerName = (objectName ?? "").ToLowerInvariant();
        string currentText = text.text ?? "";

        if (NameMatches(objectName, objectNames) || NameMatches(text.name, objectNames)) return true;

        if (titleMode)
        {
            if (lowerName.Contains("song") || lowerName.Contains("music") || lowerName.Contains("title")) return true;
            return IsLikelyCurrentSongTitle(currentText);
        }

        if (lowerName.Contains("artist")) return true;
        return IsLikelyCurrentArtist(currentText);
    }

    private static bool IsLikelyCurrentSongTitle(string text)
    {
        if (string.IsNullOrWhiteSpace(text)) return false;
        string trimmed = text.Trim();
        if (trimmed == ExperimentTitle || trimmed == ExperimentArtist) return false;
        if (trimmed.Length > 60) return false;
        if (trimmed.Contains("%") || trimmed.Contains(":")) return false;
        if (trimmed.StartsWith("Lv", StringComparison.OrdinalIgnoreCase)) return false;
        return trimmed == "Iyaiya" || trimmed == "Wonderful Pain";
    }

    private static bool IsLikelyCurrentArtist(string text)
    {
        if (string.IsNullOrWhiteSpace(text)) return false;
        string trimmed = text.Trim();
        if (trimmed == ExperimentTitle || trimmed == ExperimentArtist) return false;
        if (trimmed.Length > 60) return false;
        return trimmed == "小野道ono" || trimmed == "Haloweak";
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

    private static MusicInfo ExtractMusicInfo(object pnlInstance)
    {
        var info = new MusicInfo();
        if (pnlInstance == null) return info;

        info.Title = FirstNonEmpty(
            GetMemberText(pnlInstance, "musicNameTitle"),
            GetMemberText(pnlInstance, "songNameTitle"),
            GetMemberText(pnlInstance, "titleText"),
            GetMemberText(pnlInstance, "musicTitle"));

        info.Artist = FirstNonEmpty(
            GetMemberText(pnlInstance, "artistNameTitle"),
            GetMemberText(pnlInstance, "artistText"),
            GetMemberText(pnlInstance, "artistName"));

        info.LevelDesigner = FirstNonEmpty(
            GetMemberText(pnlInstance, "levelDesignerName"),
            GetMemberText(pnlInstance, "levelDesignerText"),
            GetMemberText(pnlInstance, "designerName"),
            GetMemberText(pnlInstance, "designerText"),
            GetMemberText(pnlInstance, "chartDesignerName"),
            GetMemberText(pnlInstance, "stageDesignerName"));

        info.Clip = FirstNonEmpty(
            FindAudioClipName(pnlInstance, out string clipReason),
            FindSceneMusicAudioClipName(out string sceneClipReason),
            GetMemberText(pnlInstance, "musicClip"),
            GetMemberText(pnlInstance, "demoMusic"),
            GetMemberText(pnlInstance, "clipName"),
            GetMemberText(pnlInstance, "audioClip"));
        info.ClipReason = string.IsNullOrWhiteSpace(info.Clip) ? FirstNonEmpty(clipReason, sceneClipReason) : null;

        if (string.IsNullOrEmpty(info.Title) || string.IsNullOrEmpty(info.Artist) || string.IsNullOrEmpty(info.LevelDesigner) || string.IsNullOrEmpty(info.Clip))
            FillByNamedMembers(pnlInstance, info);

        if (EnableSongTitleExperiment && (string.IsNullOrWhiteSpace(info.LevelDesigner) || info.LevelDesigner == ExperimentLevelDesignerLabel || IsUiObjectName(info.LevelDesigner)))
            info.LevelDesigner = ExperimentLevelDesignerName;

        return info;
    }

    private static void LogCompact(string source, MusicInfo info)
    {
        string clip = Clean(info.Clip);
        string reason = string.IsNullOrWhiteSpace(info.Clip) ? $", 클립 사유={Clean(info.ClipReason)}" : "";
        MelonLogger.Msg($"{source}: 곡 이름={Clean(info.Title)}, 음악 클립={clip}, 아티스트 이름={Clean(info.Artist)}, 레벨 디자이너={ExperimentLevelDesignerLabel}, 실제 이름={Clean(info.LevelDesigner)}{reason}");
    }

    private static string Clean(string value)
    {
        return string.IsNullOrWhiteSpace(value) ? "(unknown)" : value.Trim();
    }

    private static string FirstNonEmpty(params string[] values)
    {
        foreach (var value in values)
        {
            if (!string.IsNullOrWhiteSpace(value)) return value.Trim();
        }
        return null;
    }

    private static bool IsUsefulTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title)) return false;
        title = title.Trim();
        if (title.StartsWith("Pnl", StringComparison.OrdinalIgnoreCase)) return false;
        if (title.StartsWith("Img", StringComparison.OrdinalIgnoreCase)) return false;
        if (title.StartsWith("Txt", StringComparison.OrdinalIgnoreCase)) return false;
        if (title.Equals("MusicRoot", StringComparison.OrdinalIgnoreCase)) return false;
        if (title.Equals("FancyScrollViewMusic", StringComparison.OrdinalIgnoreCase)) return false;
        return true;
    }

    private static string GetMemberText(object obj, string memberName)
    {
        try
        {
            if (obj == null) return null;
            var ty = obj.GetType();
            var p = ty.GetProperty(memberName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (p != null && p.GetIndexParameters().Length == 0) return ValueToUsefulText(p.GetValue(obj));
            var f = ty.GetField(memberName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (f != null) return ValueToUsefulText(f.GetValue(obj));
        }
        catch { }
        return null;
    }

    private static string ValueToUsefulText(object value)
    {
        if (value == null) return null;
        if (value is string s) return string.IsNullOrWhiteSpace(s) ? null : s.Trim();

        string text = SafeGetProp(value, "text") ?? SafeGetProp(value, "m_Text");
        if (!string.IsNullOrWhiteSpace(text)) return text.Trim();

        string name = SafeGetProp(value, "name");
        if (!string.IsNullOrWhiteSpace(name)) return name.Trim();

        return null;
    }

    private static PnlStage FindLivePnlStage()
    {
        try { return UnityEngine.Object.FindObjectOfType<PnlStage>(); }
        catch { return null; }
    }

    private static string FindAudioClipName(object obj, out string reason)
    {
        reason = "AudioClip 후보 없음";
        if (obj == null)
        {
            reason = "대상 인스턴스 없음";
            return null;
        }
        var t = obj.GetType();
        int audioClipMembers = 0;
        int excludedSfxMembers = 0;

        foreach (var f in t.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
        {
            try
            {
                if (IsAudioClipType(f.FieldType)) audioClipMembers++;
                if (IsExcludedAudioMember(f.Name))
                {
                    if (IsAudioClipType(f.FieldType)) excludedSfxMembers++;
                    continue;
                }
                if (!LooksLikeMusicClipMember(f.Name, f.FieldType)) continue;
                var name = ValueToUsefulText(f.GetValue(obj));
                if (!string.IsNullOrWhiteSpace(name)) return name;
                reason = $"후보 {f.Name} 값 비어있음";
            }
            catch (Exception ex) { reason = $"후보 {f.Name} 읽기 실패: {ex.GetType().Name}"; }
        }

        foreach (var p in t.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
        {
            try
            {
                if (p.GetIndexParameters().Length > 0) continue;
                if (IsAudioClipType(p.PropertyType)) audioClipMembers++;
                if (IsExcludedAudioMember(p.Name))
                {
                    if (IsAudioClipType(p.PropertyType)) excludedSfxMembers++;
                    continue;
                }
                if (!LooksLikeMusicClipMember(p.Name, p.PropertyType)) continue;
                var name = ValueToUsefulText(p.GetValue(obj));
                if (!string.IsNullOrWhiteSpace(name)) return name;
                reason = $"후보 {p.Name} 값 비어있음";
            }
            catch (Exception ex) { reason = $"후보 {p.Name} 읽기 실패: {ex.GetType().Name}"; }
        }

        if (audioClipMembers > 0 && audioClipMembers == excludedSfxMembers)
            reason = "효과음 AudioClip만 있음";
        else if (audioClipMembers == 0)
            reason = "AudioClip 멤버 없음";

        return null;
    }

    private static string FindSceneMusicAudioClipName(out string reason)
    {
        reason = "씬 AudioSource 없음";
        try
        {
            var sources = UnityEngine.Object.FindObjectsOfType<AudioSource>();
            if (sources == null || sources.Length == 0) return null;

            int clipCount = 0;
            int excludedCount = 0;
            string firstMusicClip = null;
            string firstPausedMusicClip = null;

            foreach (var source in sources)
            {
                try
                {
                    var clip = source != null ? source.clip : null;
                    if (clip == null) continue;

                    clipCount++;
                    string clipName = clip.name;
                    if (IsExcludedAudioMember(clipName))
                    {
                        excludedCount++;
                        continue;
                    }

                    if (source.isPlaying) return clipName;
                    if (firstMusicClip == null && source.gameObject != null && LooksLikeMusicObjectName(source.gameObject.name))
                        firstMusicClip = clipName;
                    if (firstPausedMusicClip == null)
                        firstPausedMusicClip = clipName;
                }
                catch { }
            }

            if (!string.IsNullOrWhiteSpace(firstMusicClip)) return firstMusicClip;
            if (!string.IsNullOrWhiteSpace(firstPausedMusicClip)) return firstPausedMusicClip;

            if (clipCount > 0 && clipCount == excludedCount)
                reason = "씬 AudioSource에 효과음 클립만 있음";
            else if (clipCount == 0)
                reason = "씬 AudioSource에 clip 없음";
            return null;
        }
        catch (Exception ex)
        {
            reason = $"씬 AudioSource 검색 실패: {ex.GetType().Name}";
            return null;
        }
    }

    private static bool LooksLikeMusicClipMember(string memberName, Type memberType)
    {
        string name = (memberName ?? "").ToLowerInvariant();
        if (IsExcludedAudioMember(name)) return false;

        string type = memberType != null ? memberType.Name.ToLowerInvariant() : "";
        if (type.Contains("audioclip")) return true;
        return name.Contains("musicclip") || name.Contains("demomusic") || name.Contains("bgm") || name.Contains("audio");
    }

    private static bool IsAudioClipType(Type memberType)
    {
        return memberType != null && memberType.Name.ToLowerInvariant().Contains("audioclip");
    }

    private static bool IsExcludedAudioMember(string memberName)
    {
        string name = (memberName ?? "").ToLowerInvariant();
        return name.Contains("click") || name.Contains("sfx") || name.Contains("button");
    }

    private static bool LooksLikeMusicObjectName(string objectName)
    {
        string name = (objectName ?? "").ToLowerInvariant();
        return name.Contains("music") || name.Contains("bgm") || name.Contains("song") || name.Contains("demo");
    }

    private static void FillByNamedMembers(object obj, MusicInfo info)
    {
        if (obj == null) return;
        var t = obj.GetType();

        foreach (var p in t.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
        {
            try
            {
                if (p.GetIndexParameters().Length > 0) continue;
                ApplyNamedValue(p.Name, p.GetValue(obj), info);
            }
            catch { }
        }

        foreach (var f in t.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
        {
            try { ApplyNamedValue(f.Name, f.GetValue(obj), info); }
            catch { }
        }
    }

    private static void ApplyNamedValue(string memberName, object value, MusicInfo info)
    {
        string name = (memberName ?? "").ToLowerInvariant();
        string text = ValueToUsefulText(value);
        if (string.IsNullOrWhiteSpace(text)) return;
        if (IsUiObjectName(text)) return;

        if (string.IsNullOrEmpty(info.Title) && (name.Contains("song") || name.Contains("title") || name.Contains("musicname")))
            info.Title = text;
        else if (string.IsNullOrEmpty(info.Artist) && name.Contains("artist"))
            info.Artist = text;
        else if (string.IsNullOrEmpty(info.LevelDesigner) && (name.Contains("designer") || name.Contains("design") || name.Contains("chart")))
            info.LevelDesigner = text;
        else if (string.IsNullOrEmpty(info.Clip) && (name.Contains("clip") || name.Contains("audio") || name.Contains("bgm")))
            info.Clip = text;
    }

    private static bool IsUiObjectName(string text)
    {
        if (string.IsNullOrWhiteSpace(text)) return true;
        string trimmed = text.Trim();
        return trimmed.StartsWith("Img", StringComparison.OrdinalIgnoreCase)
            || trimmed.StartsWith("Txt", StringComparison.OrdinalIgnoreCase)
            || trimmed.StartsWith("Pnl", StringComparison.OrdinalIgnoreCase)
            || trimmed.StartsWith("Btn", StringComparison.OrdinalIgnoreCase)
            || trimmed.Contains("Mask")
            || trimmed.Contains("(Clone)");
    }

    private static bool IsMusicLike(Type t)
    {
        var name = t.Name.ToLowerInvariant();
        if (name.Contains("music") || name.Contains("song") || name.Contains("track") || name.Contains("bms") || name.Contains("audio") || name.Contains("bgm") || name.Contains("album")) return true;
        return false;
    }

    private static string DescribeMusicObject(object o)
    {
        if (o == null) return "(null)";
        try
        {
            var ty = o.GetType();
            // 1) If the object is a Unity UI text/TMP component, prefer its text property
            try { var tprop = ty.GetProperty("text", BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic); if (tprop != null) { var txt = tprop.GetValue(o) as string; if (!string.IsNullOrEmpty(txt)) return $"{ty.Name}: title={txt}"; } } catch { }

            // 2) Try common direct props/fields
            string title = SafeGetProp(o, "title") ?? SafeGetProp(o, "name") ?? SafeGetProp(o, "musicName") ?? SafeGetProp(o, "songName");
            string album = SafeGetProp(o, "album") ?? SafeGetProp(o, "albumName");
            string bms = SafeGetProp(o, "m_BmsUid") ?? SafeGetProp(o, "bmsUid") ?? SafeGetProp(o, "ibms_id");
            if (!string.IsNullOrEmpty(title) || !string.IsNullOrEmpty(album) || !string.IsNullOrEmpty(bms)) return $"{ty.Name}: title={title ?? "(null)"}, album={album ?? "(null)"}, bms={bms ?? "(null)"}";

            // 3) Deep scan
            var candidates = new List<string>(); CollectStringValues(o, candidates, 0, 2);
            string best = null;
            foreach (var s in candidates)
            {
                if (string.IsNullOrWhiteSpace(s)) continue; var sTrim = s.Trim(); if (sTrim.Length <= 1) continue; if (sTrim.StartsWith("Img") || sTrim.StartsWith("Pnl") || sTrim.StartsWith("sfx_")) continue; if (best == null) best = sTrim; else if (sTrim.Length > best.Length) best = sTrim;
            }
            if (!string.IsNullOrEmpty(best)) return $"{ty.Name}: title={best}";
            return ty.FullName + " - " + o.ToString();
        }
        catch (Exception ex) { return "(DescribeMusicObject 예외: " + ex.Message + ")"; }
    }

    private static void CollectStringValues(object obj, List<string> outList, int depth, int maxDepth)
    {
        if (obj == null || depth > maxDepth) return;
        try
        {
            var t = obj.GetType();
            foreach (var p in t.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                try
                {
                    if (p.GetIndexParameters().Length > 0) continue;
                    var val = p.GetValue(obj); if (val == null) continue;
                    if (val is string s) outList.Add(s);
                    else if (!(val is ValueType) && !(val is IEnumerable)) CollectStringValues(val, outList, depth + 1, maxDepth);
                    else if (val is IEnumerable ie && !(val is string)) { int i = 0; foreach (var it in ie) { if (i++ > 10) break; if (it is string ss) outList.Add(ss); } }
                }
                catch { }
            }
            foreach (var f in t.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                try { var val = f.GetValue(obj); if (val == null) continue; if (val is string s) outList.Add(s); else if (!(val is ValueType) && !(val is IEnumerable)) CollectStringValues(val, outList, depth + 1, maxDepth); } catch { }
            }
        }
        catch { }
    }

    private static string SafeGetProp(object o, string propName)
    {
        try
        {
            if (o == null) return null; var ty = o.GetType();
            var p = ty.GetProperty(propName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (p != null) { var v = p.GetValue(o); if (v is string) return v as string; var tv = v?.GetType().GetProperty("text", BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(v) as string; if (!string.IsNullOrEmpty(tv)) return tv; if (v != null) return v.ToString(); }
            var f = ty.GetField(propName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (f != null) { var v = f.GetValue(o); if (v is string) return v as string; var tv = v?.GetType().GetProperty("text", BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(v) as string; if (!string.IsNullOrEmpty(tv)) return tv; if (v != null) return v.ToString(); }
        }
        catch { }
        return null;
    }

    private static void DumpMusicInfoVerbose(object obj)
    {
        try
        {
            if (obj == null) { MelonLogger.Msg("DumpMusicInfoVerbose: null"); return; }
            var t = obj.GetType(); MelonLogger.Msg($"--- DumpMusicInfoVerbose: {t.FullName} ---");
            foreach (var f in t.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                try { var v = f.GetValue(obj); MelonLogger.Msg($"Field: {f.Name} (Type={f.FieldType.Name}) = {(v != null ? v.ToString() : "(null)")}"); if (v != null) LogStringProps(v, "  "); } catch (Exception ex) { MelonLogger.Msg($"Field {f.Name} read error: {ex.Message}"); }
            }
            foreach (var p in t.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                try { if (p.GetIndexParameters().Length > 0) { MelonLogger.Msg($"Property: {p.Name} (indexed) skipped"); continue; } var v = p.GetValue(obj); MelonLogger.Msg($"Property: {p.Name} (Type={p.PropertyType.Name}) = {(v != null ? v.ToString() : "(null)")}"); if (v != null) LogStringProps(v, "  "); } catch (Exception ex) { MelonLogger.Msg($"Property {p.Name} read error: {ex.Message}"); }
            }
            int childCount = 0;
            foreach (var f in t.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                try
                {
                    var v = f.GetValue(obj); if (v == null) continue; var vt = v.GetType(); if (vt == typeof(string) || vt.IsPrimitive) continue; if (++childCount > 6) break; MelonLogger.Msg($"-- Child field {f.Name} type {vt.FullName} --");
                    foreach (var p in vt.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
                    {
                        try { if (p.GetIndexParameters().Length > 0) continue; var pv = p.GetValue(v); if (pv is string) MelonLogger.Msg($"   {p.Name} = {pv}"); } catch { }
                    }
                }
                catch { }
            }
            MelonLogger.Msg($"--- End DumpMusicInfoVerbose ---");
        }
        catch (Exception ex) { MelonLogger.Msg($"DumpMusicInfoVerbose exception: {ex}"); }
    }

    private static void LogStringProps(object v, string indent)
    {
        try
        {
            var vt = v.GetType();
            var textProp = vt.GetProperty("text", BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
            if (textProp != null) { try { var tv = textProp.GetValue(v) as string; if (!string.IsNullOrEmpty(tv)) MelonLogger.Msg($"{indent}text: {tv}"); } catch { } }
            try { var nameProp = vt.GetProperty("name", BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic); if (nameProp != null) { var nv = nameProp.GetValue(v) as string; if (!string.IsNullOrEmpty(nv)) MelonLogger.Msg($"{indent}name: {nv}"); } } catch { }
            var tname = vt.Name.ToLowerInvariant();
            if (tname.Contains("textmeshpro") || tname.Contains("textmesh") || tname.Contains("textui") || tname.Contains("text"))
            {
                foreach (var p in vt.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
                {
                    try { if (p.GetIndexParameters().Length > 0) continue; if (p.PropertyType == typeof(string)) { var sval = p.GetValue(v) as string; if (!string.IsNullOrEmpty(sval)) MelonLogger.Msg($"{indent}{p.Name}: {sval}"); } } catch { }
                }
            }
            int found = 0;
            foreach (var p in vt.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                try { if (p.GetIndexParameters().Length > 0) continue; var pv = p.GetValue(v); if (pv is string s && !string.IsNullOrEmpty(s)) { MelonLogger.Msg($"{indent}{p.Name}: {s}"); if (++found > 6) break; } } catch { }
            }
        }
        catch (Exception ex) { MelonLogger.Msg($"LogStringProps exception: {ex.Message}"); }
    }

    private static bool TryLogCompact(object pnlInstance)
    {
        try
        {
            if (pnlInstance == null) return false;
            var t = pnlInstance.GetType();
            string title = null, artist = null, album = null;
            foreach (var f in t.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                try
                {
                    var v = f.GetValue(pnlInstance); if (v == null) continue; var lname = f.Name.ToLowerInvariant();
                    if (title == null && (lname.Contains("song") || lname.Contains("title") || lname.Contains("music") || lname.Contains("name"))) title = SafeGetProp(v, "text") ?? SafeGetProp(v, "m_Text") ?? SafeGetProp(v, "name") ?? (v as string);
                    if (artist == null && lname.Contains("artist")) artist = SafeGetProp(v, "text") ?? SafeGetProp(v, "m_Text") ?? SafeGetProp(v, "name") ?? (v as string);
                    if (album == null && lname.Contains("album")) album = SafeGetProp(v, "text") ?? SafeGetProp(v, "m_Text") ?? SafeGetProp(v, "name") ?? (v as string);
                }
                catch { }
            }
            foreach (var p in t.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                try
                {
                    if (p.GetIndexParameters().Length > 0) continue; var v = p.GetValue(pnlInstance); if (v == null) continue; var lname = p.Name.ToLowerInvariant();
                    if (title == null && (lname.Contains("song") || lname.Contains("title") || lname.Contains("music") || lname.Contains("name"))) title = SafeGetProp(v, "text") ?? SafeGetProp(v, "m_Text") ?? SafeGetProp(v, "name") ?? (v as string);
                    if (artist == null && lname.Contains("artist")) artist = SafeGetProp(v, "text") ?? SafeGetProp(v, "m_Text") ?? SafeGetProp(v, "name") ?? (v as string);
                    if (album == null && lname.Contains("album")) album = SafeGetProp(v, "text") ?? SafeGetProp(v, "m_Text") ?? SafeGetProp(v, "name") ?? (v as string);
                }
                catch { }
            }
            if (string.IsNullOrEmpty(title) && string.IsNullOrEmpty(artist) && string.IsNullOrEmpty(album)) return false;
            MelonLogger.Msg($"NowPlaying: {(string.IsNullOrEmpty(title) ? "(unknown)" : title)} - {(string.IsNullOrEmpty(artist) ? "(unknown)" : artist)} - {(string.IsNullOrEmpty(album) ? "(unknown)" : album)}");
            return true;
        }
        catch { return false; }
    }
}
