using MelonLoader;
using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using Il2CppAssets.Scripts.UI.Panels;
using UnityEngine;
using UnityEngine.UI;

namespace muse_dash_test
{
    /// <summary>
    /// 곡 정보의 추출(Read-only), 로깅, 디버그 덤프 등의 진단 로직을 담당하는 클래스입니다.
    /// </summary>
    public static class PnlMusicDiagnostics
    {
        public class MusicInfo
        {
            public string Title;
            public string Clip;
            public string Artist;
            public string LevelDesigner;
            public string ClipReason;
        }

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

        public static void ApplyAndLogMusicInfo(string source, object pnlInstance)
        {
            try
            {
                string resolvedUid = ResolveCustomMusicUid(pnlInstance);
                if (!string.IsNullOrEmpty(resolvedUid))
                {
                    PnlMusicOverride.ApplySongTitleOverride(source, pnlInstance, resolvedUid);
                }
                var info = ExtractMusicInfo(pnlInstance, resolvedUid);
                LogCompact(source, info);
            }
            catch (Exception ex) { MelonLogger.Error($"ApplyAndLogMusicInfo 예외: {ex}"); }
        }

        public static void ApplyPrepMusicInfo(object pnlInstance, string source = "PnlPreparation.Awake")
        {
            try
            {
                string resolvedUid = ResolveCustomMusicUid(pnlInstance);
                if (!string.IsNullOrEmpty(resolvedUid))
                {
                    PnlMusicOverride.ApplySongTitleOverride(source, pnlInstance, resolvedUid);
                }
                var info = ExtractMusicInfo(pnlInstance, resolvedUid);
                if (!IsUsefulTitle(info.Title))
                {
                    var stage = FindLivePnlStage();
                    if (stage != null)
                    {
                        string stageResolvedUid = ResolveCustomMusicUid(stage);
                        if (!string.IsNullOrEmpty(stageResolvedUid))
                        {
                            PnlMusicOverride.ApplySongTitleOverride(source + "->PnlStage", stage, stageResolvedUid);
                        }
                        var stageInfo = ExtractMusicInfo(stage, stageResolvedUid);
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

        public static string ResolveCustomMusicUid(object pnlInstance)
        {
            string selected = CustomPlaySession.Current.SelectedMusicUid;
            if (string.IsNullOrEmpty(selected)) selected = PnlStagePatchHelper.GetCurrentSelectedMusicUid();
            if (!string.IsNullOrEmpty(selected))
            {
                return CustomContentIds.IsVirtualSong(selected) ? selected : null;
            }

            string uid = TryFindCustomMusicUidInObject(pnlInstance, 0, new HashSet<object>());
            if (!string.IsNullOrEmpty(uid)) return uid;

            uid = CustomPlaySession.Current.LastClickedMusicUid;
            if (!string.IsNullOrEmpty(uid) && CustomContentIds.IsVirtualSong(uid)) return uid;

            return null;
        }

        private static string TryFindCustomMusicUidInObject(object obj, int depth, HashSet<object> visited)
        {
            if (obj == null || depth > 2) return null;
            if (obj is string text) return CustomContentIds.IsVirtualSong(text) ? text : null;
            if (obj is UnityEngine.Object unityObject && !unityObject) return null;
            if (!visited.Add(obj)) return null;

            try
            {
                if (obj is Il2CppAssets.Scripts.Database.MusicInfo musicInfo && CustomContentIds.IsVirtualSong(musicInfo.uid))
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
            catch (Exception) { }

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
                    return CustomContentIds.IsVirtualSong(text) ? text : null;
                }

                if (value is Il2CppAssets.Scripts.Database.MusicInfo musicInfo)
                {
                    return CustomContentIds.IsVirtualSong(musicInfo.uid) ? musicInfo.uid : null;
                }

                return TryFindCustomMusicUidInObject(value, depth + 1, visited);
            }
            catch
            {
                return null;
            }
        }

        private static MusicInfo ExtractMusicInfo(object pnlInstance, string resolvedUid = null)
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

            if (string.IsNullOrWhiteSpace(info.LevelDesigner) || info.LevelDesigner == "레벨 디자이너" || IsUiObjectName(info.LevelDesigner))
            {
                string selectedUid = resolvedUid ?? ResolveCustomMusicUid(pnlInstance);
                if (!string.IsNullOrEmpty(selectedUid)
                    && MainMod.TryGetHwaPrimarySong(selectedUid, out _, out _, out string manifestLevelDesigner, out _, out _, out _, out _, out _, out _)
                    && !string.IsNullOrWhiteSpace(manifestLevelDesigner))
                {
                    info.LevelDesigner = manifestLevelDesigner;
                }
                else
                {
                    var musicInfo = !string.IsNullOrEmpty(selectedUid)
                        ? Il2CppAssets.Scripts.Database.GlobalDataBase.dbMusicTag?.GetMusicInfoFromAll(selectedUid)
                        : null;
                    if (musicInfo != null)
                    {
                        info.LevelDesigner = musicInfo.levelDesigner;
                    }
                    else
                    {
                        info.LevelDesigner = "Custom Designer";
                    }
                }
            }

            return info;
        }

        private static void LogCompact(string source, MusicInfo info)
        {
            string uid = PnlStagePatchHelper.GetCurrentSelectedMusicUid() ?? CustomPlaySession.Current.LastClickedMusicUid ?? "(unknown)";
            string clip = Clean(info.Clip);
            string reason = string.IsNullOrWhiteSpace(info.Clip) ? $", 클립 사유={Clean(info.ClipReason)}" : "";
            MelonLogger.Msg($"{source}: 곡 이름={Clean(info.Title)}, UID={uid}, 음악 클립={clip}, 아티스트 이름={Clean(info.Artist)}, 레벨 디자이너=레벨 디자이너, 실제 이름={Clean(info.LevelDesigner)}{reason}");
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
                object val = ModReflection.GetValue(obj, memberName, silent: true);
                return ValueToUsefulText(val);
            }
            catch (Exception) { }
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

        private static string SafeGetProp(object obj, string propName)
        {
            try
            {
                var p = obj.GetType().GetProperty(propName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if (p != null && p.CanRead) return p.GetValue(obj)?.ToString();
            }
            catch (Exception) { }
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
                GameObject bgmGo = GameObject.Find("BGM");
                if (bgmGo != null)
                {
                    AudioSource source = bgmGo.GetComponent<AudioSource>();
                    if (source != null && source.clip != null)
                    {
                        reason = null;
                        return source.clip.name;
                    }
                }

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
                    catch (Exception) { }
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
                catch (Exception) { }
            }

            foreach (var f in t.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                try { ApplyNamedValue(f.Name, f.GetValue(obj), info); }
                catch (Exception) { }
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
    }
}
