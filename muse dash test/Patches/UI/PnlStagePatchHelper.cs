using MelonLoader;
using System;
using System.Reflection;
using System.Text;
using Il2CppAssets.Scripts.UI.Panels;
using UnityEngine;
using UnityEngine.UI;

public static class PnlStagePatchHelper
{
    private const int CustomTagUid = 998;
    private const string CustomMusicUid = "0-0";
    private const string CustomTitle = "화영왕";
    private const string CustomArtist = "화영왕";

    private const BindingFlags InstanceMembers =
        BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

    public static bool ShouldApplyHwayoungwang()
    {
        // 1. 실험 모드가 활성화되어 있지 않으면 절대 치환하지 않음
        if (!MusicButtonAreaTitle_RefreshTxt_Patch.IsExperimentModActive)
        {
            return false;
        }

        // 2. 마지막으로 클릭된 Uid가 "999-0" 이거나 "0-0" 인 경우
        string lastClicked = muse_dash_test.MusicButtonCell_OnButtonClicked_Patch.LastClickedMusicUid;
        if (lastClicked == "999-0" || lastClicked == "0-0")
        {
            return true;
        }

        // 3. 혹은, 현재 선택된 Uid가 "999-0" 이거나 "0-0" 인 경우
        string selected = GetCurrentSelectedMusicUid();
        if (selected == "999-0" || selected == "0-0")
        {
            return true;
        }

        // 4. 혹은, 아직 아무 곡도 클릭하지 않은 초기 상태라면 허용함
        if (string.IsNullOrEmpty(lastClicked) || lastClicked == "(null)")
        {
            return true;
        }

        return false;
    }

    public static void ApplyCustomTagTitleAccessors(string source, PnlStage stage)
    {
        try
        {
            if (stage == null)
            {
                MelonLogger.Msg($"[{source}] 커스텀 태그 접근자 적용 건너뜀: stage=null");
                return;
            }

            // 통합 조건 검사 적용
            if (!ShouldApplyHwayoungwang())
            {
                MelonLogger.Msg($"[{source}] 커스텀 태그 접근자 적용 건너뜀: ShouldApplyHwayoungwang=false");
                return;
            }

            if (!IsCustomAlbumContext(CustomTagUid, CustomMusicUid))
            {
                MelonLogger.Msg($"[{source}] 커스텀 태그 접근자 적용 건너뜀: customContext=false");
                return;
            }

            var musicText = stage.musicNameTitle;
            var artistText = stage.artistNameTitle;

            if (musicText != null)
            {
                MelonLogger.Msg($"[{source}] musicNameTitle 접근자로 제목 변경: {CleanLogText(musicText.text)} -> {CustomTitle}");
                musicText.text = CustomTitle;
            }
            else
            {
                MelonLogger.Warning($"[{source}] musicNameTitle 접근자 결과가 null입니다.");
            }

            if (artistText != null)
            {
                MelonLogger.Msg($"[{source}] artistNameTitle 접근자로 아티스트 변경: {CleanLogText(artistText.text)} -> {CustomArtist}");
                artistText.text = CustomArtist;
            }
            else
            {
                MelonLogger.Warning($"[{source}] artistNameTitle 접근자 결과가 null입니다.");
            }
        }
        catch (Exception ex)
        {
            MelonLogger.Error($"{source} 커스텀 태그 접근자 적용 예외: {ex}");
        }
    }

    public static void LogPnlStageRefresh(string source, PnlStage stage)
    {
        try
        {
            if (stage == null)
            {
                MelonLogger.Msg($"[{source}] stage=null");
                return;
            }

            string selectedUid = GetCurrentSelectedMusicUid();
            string albumTitle = GetLongNameControllerText(stage.m_AlbumTitleTxt);
            string musicTitle = GetLongNameControllerText(stage.musicLongNameController);
            string artistTitle = GetLongNameControllerText(stage.artistLongNameController);
            string albumObjActive = stage.m_AlbumTitleObj != null ? stage.m_AlbumTitleObj.activeSelf.ToString() : "(null)";
            MelonLogger.Msg($"[{source}] selectedUid={selectedUid ?? "(null)"}, albumTitle={CleanLogText(albumTitle)}, musicTitle={CleanLogText(musicTitle)}, artistTitle={CleanLogText(artistTitle)}, albumTitleObjActive={albumObjActive}");
        }
        catch (Exception ex)
        {
            MelonLogger.Error($"{source} 예외: {ex}");
        }
    }

    public static void LogTextAccessor(string source, PnlStage stage, Text text)
    {
        try
        {
            string selectedUid = GetCurrentSelectedMusicUid();
            string textName = text != null ? text.name : "(null)";
            string gameObjectName = text != null && text.gameObject != null ? text.gameObject.name : "(null)";
            string value = text != null ? text.text : null;
            string active = text != null && text.gameObject != null ? text.gameObject.activeSelf.ToString() : "(null)";
            string stageName = stage != null ? stage.name : "(null)";
            MelonLogger.Msg($"[{source}] stage={stageName}, selectedUid={selectedUid ?? "(null)"}, TextName={textName}, GameObject={gameObjectName}, Active={active}, Text={CleanLogText(value)}");
        }
        catch (Exception ex)
        {
            MelonLogger.Error($"{source} 예외: {ex}");
        }
    }

    public static void LogPnlStageProperties(string source, PnlStage stage)
    {
        try
        {
            if (stage == null)
            {
                MelonLogger.Msg($"[{source}] stage=null");
                return;
            }

            var sb = new StringBuilder();
            int count = 0;
            foreach (var prop in typeof(PnlStage).GetProperties(InstanceMembers))
            {
                if (!prop.CanRead || prop.GetIndexParameters().Length != 0)
                {
                    continue;
                }

                string value = SafePropertyValue(stage, prop);
                if (string.IsNullOrEmpty(value))
                {
                    continue;
                }

                if (sb.Length > 0)
                {
                    sb.Append(" | ");
                }

                sb.Append(prop.Name);
                sb.Append('=');
                sb.Append(value);
                count++;

                if (count >= 80)
                {
                    sb.Append(" | ...");
                    break;
                }
            }

            MelonLogger.Msg($"[{source}] {sb}");
        }
        catch (Exception ex)
        {
            MelonLogger.Error($"{source} 프로퍼티 덤프 예외: {ex}");
        }
    }

    public static void LogMusicRootComponents(string source, PnlStage stage)
    {
        try
        {
            if (stage == null || stage.musicRoot == null)
            {
                MelonLogger.Msg($"[{source}] musicRoot=null");
                return;
            }

            var root = stage.musicRoot;
            MelonLogger.Msg($"[{source}] cover probe: root={root.name}, active={root.activeSelf}");

            var images = root.GetComponentsInChildren<UnityEngine.UI.Image>(true);
            int coverCount = 0;
            foreach (var image in images)
            {
                if (image == null)
                {
                    continue;
                }

                string spriteName = image.sprite != null ? image.sprite.name : "(null)";
                if (!LooksLikeCoverImage(image.name, spriteName))
                {
                    continue;
                }

                MelonLogger.Msg($"[{source}] CoverImage[{coverCount}] path={GetTransformPath(image.transform, root.transform)}, active={image.gameObject.activeSelf}, sprite={spriteName}, color={image.color}");
                coverCount++;
                if (coverCount >= 8) break;
            }

            var rawImages = root.GetComponentsInChildren<UnityEngine.UI.RawImage>(true);
            foreach (var rawImage in rawImages)
            {
                if (rawImage == null)
                {
                    continue;
                }

                string textureName = rawImage.texture != null ? rawImage.texture.name : "(null)";
                if (!LooksLikeCoverImage(rawImage.name, textureName))
                {
                    continue;
                }

                MelonLogger.Msg($"[{source}] CoverRawImage[{coverCount}] path={GetTransformPath(rawImage.transform, root.transform)}, active={rawImage.gameObject.activeSelf}, texture={textureName}, color={rawImage.color}");
                coverCount++;
                if (coverCount >= 8) break;
            }

            if (coverCount == 0)
            {
                MelonLogger.Msg($"[{source}] cover probe: no ImgCover/cover sprite found");
            }
        }
        catch (Exception ex)
        {
            MelonLogger.Error($"{source} MusicRoot 덤프 예외: {ex}");
        }
    }

    private static bool LooksLikeCoverImage(string objectName, string assetName)
    {
        return ContainsCoverToken(objectName) || ContainsCoverToken(assetName);
    }

    private static bool ContainsCoverToken(string value)
    {
        return !string.IsNullOrEmpty(value) &&
            (value.IndexOf("ImgCover", StringComparison.OrdinalIgnoreCase) >= 0 ||
             value.IndexOf("cover", StringComparison.OrdinalIgnoreCase) >= 0);
    }

    private static string GetTransformPath(Transform transform, Transform stopAt)
    {
        try
        {
            if (transform == null)
            {
                return "(null)";
            }

            var sb = new StringBuilder(transform.name);
            var current = transform.parent;
            while (current != null && current != stopAt)
            {
                sb.Insert(0, current.name + "/");
                current = current.parent;
            }

            if (stopAt != null)
            {
                sb.Insert(0, stopAt.name + "/");
            }

            return sb.ToString();
        }
        catch
        {
            return transform != null ? transform.name : "(null)";
        }
    }

    private static string SafePropertyValue(object target, PropertyInfo prop)
    {
        try
        {
            object value = prop.GetValue(target);
            if (value == null)
            {
                return "(null)";
            }

            if (value is string s)
            {
                return CleanLogText(s);
            }

            Type type = value.GetType();
            if (type.IsPrimitive || value is decimal)
            {
                return value.ToString();
            }

            if (value is Text text)
            {
                return $"Text(name={text.name ?? "(null)"}, text={CleanLogText(text.text)})";
            }

            if (value is Il2CppAssets.Scripts.Database.MusicInfo musicInfo)
            {
                return $"MusicInfo(uid={musicInfo.uid ?? "(null)"}, name={musicInfo.name ?? "(null)"}, cover={musicInfo.cover ?? "(null)"})";
            }

            if (value is UnityEngine.Object unityObject)
            {
                return $"{type.Name}(name={unityObject.name ?? "(null)"})";
            }

            return type.FullName;
        }
        catch (Exception ex)
        {
            return "(error: " + ex.Message + ")";
        }
    }

    public static string GetLongNameControllerText(Il2Cpp.LongSongNameController controller)
    {
        if (controller == null)
        {
            return null;
        }

        return FirstNonEmpty(
            controller.m_TxtSimpleName?.text,
            controller.m_MidSimpleName?.text,
            controller.m_TxtBackupName?.text);
    }

    private static string FirstNonEmpty(params string[] values)
    {
        if (values == null) return null;
        foreach (var value in values)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                return value;
            }
        }
        return null;
    }

    private static string CleanLogText(string value)
    {
        return string.IsNullOrWhiteSpace(value) ? "(null)" : value.Trim();
    }

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

    private static readonly string[] ExperimentModeTitles = { "\uc2e4\ud5d8 \ubaa8\ub4dc", "Experiment Mod", "\u5b9e\u9a8c\u6a21\u5f0f", "\u5be6\u9a57\u6a21\u5f0f", "\u5b9f\u9a13\u30e2\u30fc\u30c9" };

    public static void SyncExperimentModeFromStage(PnlStage stage)
    {
        try
        {
            if (stage == null) return;
            var titleText = stage.titleOwn;
            if (titleText == null) return;
            string text = titleText.text ?? string.Empty;
            bool isExp = false;
            foreach (var t in ExperimentModeTitles)
                if (text == t) { isExp = true; break; }
            if (isExp != MusicButtonAreaTitle_RefreshTxt_Patch.IsExperimentModActive)
            {
                MelonLogger.Msg($"[SyncExperimentMode] titleOwn.text='{text}' \u2192 IsExperimentModActive={isExp}");
                MusicButtonAreaTitle_RefreshTxt_Patch.IsExperimentModActive = isExp;
            }
        }
        catch (Exception ex)
        {
            MelonLogger.Error($"SyncExperimentModeFromStage \uc608\uc678: {ex}");
        }
    }

    public static void LogButtons(string source, PnlStage stage)
    {
        try
        {
            if (stage == null)
            {
                MelonLogger.Msg($"[{source}] stage=null");
                return;
            }

            int count = 0;
            foreach (var prop in typeof(PnlStage).GetProperties(InstanceMembers))
            {
                if (prop.PropertyType != typeof(Button) || !prop.CanRead || prop.GetIndexParameters().Length != 0) continue;
                try
                {
                    var btn = prop.GetValue(stage) as Button;
                    string goName = btn?.gameObject != null ? btn.gameObject.name : "(null)";
                    string active = btn?.gameObject != null ? btn.gameObject.activeSelf.ToString() : "(null)";
                    string interactable = btn != null ? btn.interactable.ToString() : "(null)";
                    MelonLogger.Msg($"[{source}] Button prop={prop.Name}, gameObject={goName}, active={active}, interactable={interactable}");
                    count++;
                }
                catch (Exception ex)
                {
                    MelonLogger.Msg($"[{source}] Button prop={prop.Name}: (예외: {ex.Message})");
                }
            }

            if (count == 0)
                MelonLogger.Msg($"[{source}] Button 프로퍼티 없음");
        }
        catch (Exception ex)
        {
            MelonLogger.Error($"{source} LogButtons 예외: {ex}");
        }
    }
}
