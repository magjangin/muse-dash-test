using System;
using System.Reflection;
using Il2CppAssets.Scripts.UI.Panels;

namespace muse_dash_test
{
    /// <summary>
    /// pnlInstance의 멤버를 리플렉션으로 훑어 곡 정보(MusicInfo)를 추출하는 로직.
    /// </summary>
    public static partial class PnlMusicDiagnostics
    {
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
                        info.LevelDesigner = "(unknown)";
                    }
                }
            }

            return info;
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
    }
}
