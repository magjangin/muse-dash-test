using MelonLoader;
using System;

// Il2CppGameLogic.GameMusicScene.InitTimer(Decimal) / InitSceneEvents() 후킹 로깅.
// 씬 타이머·씬 이벤트 초기화가 언제 일어나는지(씬 풀 구성 타이밍) 관찰용.
[HarmonyLib.HarmonyPatch(typeof(Il2CppGameLogic.GameMusicScene), "InitTimer",
    new Type[] { typeof(Il2CppSystem.Decimal) })]
public class GameMusicScene_InitTimer_Patch
{
    public static void Prefix(Il2CppGameLogic.GameMusicScene __instance, Il2CppSystem.Decimal total)
    {
        try
        {
            if (!muse_dash_test.CustomPlaySession.Current.ShouldApplyExperimentChart)
            {
                return;
            }

            int frame = 0;
            try { frame = UnityEngine.Time.frameCount; } catch { }
            MelonLogger.Msg($"[GameMusicScene.InitTimer] PRE frame={frame}, total={total}");

            string initialZz = ResolveInitialRenderZz();
            var db = Il2CppAssets.Scripts.Database.GlobalDataBase.s_StageInfo;
            int changed = TransformSceneSegments(db != null ? db.musicList : null, initialZz);
            MelonLogger.Msg($"[GameMusicScene.InitTimer] 구간 렌더 zz 변형(initial={initialZz}): {changed}개");
        }
        catch (Exception ex) { MelonLogger.Error($"[GameMusicScene.InitTimer] Prefix 예외: {ex}"); }
    }

    private static int TransformSceneSegments(Il2CppSystem.Collections.Generic.List<Il2CppGameLogic.MusicData> list, string initialZz)
    {
        int changed = 0;
        if (list == null || string.IsNullOrEmpty(initialZz)) return 0;

        var changedByOriginalZz = new System.Collections.Generic.SortedDictionary<string, int>();
        string activeRenderZz = initialZz;
        SceneZzTransformTracker.Clear();
        for (int i = 0; i < list.Count; i++)
        {
            try
            {
                var note = list[i];
                if (note == null) continue;

                var nd = note.noteData;
                if (nd == null) continue;

                string uid = nd.uid;
                if (!IsSixDigitUid(uid)) continue;

                if (IsSceneToggleUid(uid))
                {
                    string nextRenderZz = uid.Substring(4, 2);
                    MelonLogger.Msg($"[GameMusicScene.InitTimer] 씬 전환 구간 관찰: index={i}, uid={uid}, sceneInfo={nextRenderZz}, activeRenderZz={activeRenderZz}");
                    continue;
                }

                string fromZz = uid.Substring(0, 2);
                if (fromZz == "00" || fromZz == activeRenderZz) continue;

                string renderZz = activeRenderZz;
                if (SceneZzTransformTracker.TryGetBmsOriginalUid(note.objId, out string bmsOriginalUid)
                    && IsSixDigitUid(bmsOriginalUid))
                {
                    renderZz = bmsOriginalUid.Substring(0, 2);
                }

                if (fromZz == renderZz) continue;

                string newUid = renderZz + uid.Substring(2);
                string renderPrefabName = nd.prefab_name;
                try
                {
                    if (!string.IsNullOrEmpty(nd.prefab_name) && nd.prefab_name.StartsWith(fromZz))
                    {
                        renderPrefabName = renderZz + nd.prefab_name.Substring(2);
                    }
                }
                catch { }

                SceneZzTransformTracker.Record(note, newUid, renderPrefabName);
                CountZz(changedByOriginalZz, fromZz);
                nd.uid = newUid;
                try { if (IsSixDigitUid(nd.mirror_uid) && nd.mirror_uid.StartsWith(fromZz)) nd.mirror_uid = renderZz + nd.mirror_uid.Substring(2); } catch { }
                try { nd.scene = "scene_" + renderZz; } catch { }
                try { nd.prefab_name = renderPrefabName; } catch { }
                try { if (int.TryParse(newUid, out int parsedNoteUid)) nd.noteUid = parsedNoteUid; } catch { }
                try { if (note.configData != null && IsSixDigitUid(note.configData.note_uid) && note.configData.note_uid.StartsWith(fromZz)) note.configData.note_uid = renderZz + note.configData.note_uid.Substring(2); } catch { }
                try { note.noteData = nd; } catch { }
                try { list[i] = note; } catch { }
                changed++;
            }
            catch { }
        }

        MelonLogger.Msg($"[GameMusicScene.InitTimer] 구간 렌더 zz 변형 분포: {FormatZzCounts(changedByOriginalZz)}");
        return changed;
    }

    private static string ResolveInitialRenderZz()
    {
        string uid = muse_dash_test.CustomPlaySession.Current.SelectedMusicUid;
        if (string.IsNullOrEmpty(uid))
        {
            uid = PnlStagePatchHelper.GetCurrentSelectedMusicUid() ?? muse_dash_test.CustomPlaySession.Current.LastClickedMusicUid;
        }

        if (muse_dash_test.MainMod.TryGetCachedHwaScene(uid, out int manifestScene))
        {
            return manifestScene.ToString("00");
        }

        MelonLogger.Msg($"[GameMusicScene.InitTimer] manifest scene 없음, initialRenderZz=07 사용: uid={uid ?? "(null)"}");
        return "07";
    }

    private static bool IsSceneToggleUid(string uid)
    {
        return IsSixDigitUid(uid) && uid.StartsWith("0004", StringComparison.OrdinalIgnoreCase);
    }

    private static bool IsSixDigitUid(string uid)
    {
        if (string.IsNullOrEmpty(uid) || uid.Length != 6) return false;
        for (int i = 0; i < uid.Length; i++)
        {
            if (uid[i] < '0' || uid[i] > '9') return false;
        }

        return true;
    }

    private static void CountZz(System.Collections.Generic.SortedDictionary<string, int> counts, string zz)
    {
        if (string.IsNullOrEmpty(zz)) return;
        counts[zz] = counts.TryGetValue(zz, out int count) ? count + 1 : 1;
    }

    private static string FormatZzCounts(System.Collections.Generic.SortedDictionary<string, int> counts)
    {
        if (counts == null || counts.Count == 0)
        {
            return "{}";
        }

        var parts = new System.Collections.Generic.List<string>();
        foreach (var pair in counts)
        {
            parts.Add($"{pair.Key}:{pair.Value}");
        }

        return "{" + string.Join(", ", parts) + "}";
    }
}

internal static class SceneZzTransformTracker
{
    private sealed class OriginalIdentity
    {
        public string Uid;
        public string MirrorUid;
        public int NoteUid;
        public string ConfigNoteUid;
        public string Scene;
        public string PrefabName;
        public string RenderUid;
        public string RenderMirrorUid;
        public int RenderNoteUid;
        public string RenderConfigNoteUid;
        public string RenderPrefabName;
    }

    private static readonly System.Collections.Generic.Dictionary<short, OriginalIdentity> OriginalsByObjId =
        new System.Collections.Generic.Dictionary<short, OriginalIdentity>();

    private static readonly System.Collections.Generic.Dictionary<short, OriginalIdentity> BmsOriginalsByObjId =
        new System.Collections.Generic.Dictionary<short, OriginalIdentity>();

    public static int Count => OriginalsByObjId.Count;

    public static int BmsOriginalCount => BmsOriginalsByObjId.Count;

    public static bool TryGetBmsOriginalUid(short objId, out string uid)
    {
        uid = null;
        if (!BmsOriginalsByObjId.TryGetValue(objId, out var original))
        {
            return false;
        }

        uid = original.Uid;
        return !string.IsNullOrEmpty(uid);
    }

    public static void Clear()
    {
        OriginalsByObjId.Clear();
    }

    public static void ClearBmsOriginalIdentities()
    {
        BmsOriginalsByObjId.Clear();
    }

    public static void RegisterBmsOriginalIdentities(Il2CppSystem.Collections.Generic.List<Il2CppGameLogic.MusicData> list, int startIndex)
    {
        BmsOriginalsByObjId.Clear();
        if (list == null)
        {
            return;
        }

        var counts = new System.Collections.Generic.SortedDictionary<string, int>();
        for (int i = System.Math.Max(0, startIndex); i < list.Count; i++)
        {
            try
            {
                var note = list[i];
                if (note?.noteData == null)
                {
                    continue;
                }

                if (string.IsNullOrEmpty(note.noteData.uid))
                {
                    continue;
                }

                BmsOriginalsByObjId[note.objId] = CaptureIdentity(note);
                CountZz(counts, note.noteData.uid);
            }
            catch { }
        }

        MelonLogger.Msg($"[SceneZzTransformTracker] BMS 원본 UID 등록: count={BmsOriginalsByObjId.Count}, zz분포={FormatZzCounts(counts)}");
    }

    public static void Record(Il2CppGameLogic.MusicData note, string renderUid, string renderPrefabName)
    {
        if (note?.noteData == null)
        {
            return;
        }

        string renderZz = IsSixDigitUid(renderUid) ? renderUid.Substring(0, 2) : null;
        string renderMirrorUid = IsSixDigitUid(note.noteData.mirror_uid) && !string.IsNullOrEmpty(renderZz)
            ? renderZz + note.noteData.mirror_uid.Substring(2)
            : note.noteData.mirror_uid;
        string renderConfigNoteUid = note.configData != null && IsSixDigitUid(note.configData.note_uid) && !string.IsNullOrEmpty(renderZz)
            ? renderZz + note.configData.note_uid.Substring(2)
            : note.configData?.note_uid;
        int renderNoteUid = note.noteData.noteUid;
        try
        {
            if (int.TryParse(renderUid, out int parsed))
            {
                renderNoteUid = parsed;
            }
        }
        catch { }

        if (BmsOriginalsByObjId.TryGetValue(note.objId, out var bmsOriginal))
        {
            bmsOriginal.RenderUid = renderUid;
            bmsOriginal.RenderMirrorUid = renderMirrorUid;
            bmsOriginal.RenderNoteUid = renderNoteUid;
            bmsOriginal.RenderConfigNoteUid = renderConfigNoteUid;
            bmsOriginal.RenderPrefabName = renderPrefabName;
            OriginalsByObjId[note.objId] = bmsOriginal;
            return;
        }

        var captured = CaptureIdentity(note);
        captured.RenderUid = renderUid;
        captured.RenderMirrorUid = renderMirrorUid;
        captured.RenderNoteUid = renderNoteUid;
        captured.RenderConfigNoteUid = renderConfigNoteUid;
        captured.RenderPrefabName = renderPrefabName;
        OriginalsByObjId[note.objId] = captured;
    }

    public static int RestoreIdentities(Il2CppSystem.Collections.Generic.List<Il2CppGameLogic.MusicData> list)
    {
        if (list == null || OriginalsByObjId.Count == 0)
        {
            return 0;
        }

        int restored = 0;
        var counts = new System.Collections.Generic.SortedDictionary<string, int>();
        for (int i = 0; i < list.Count; i++)
        {
            try
            {
                var note = list[i];
                if (note?.noteData == null)
                {
                    continue;
                }

                if (!OriginalsByObjId.TryGetValue(note.objId, out var original))
                {
                    continue;
                }

                var noteData = note.noteData;
                noteData.uid = original.Uid;
                CountZz(counts, original.Uid);
                noteData.mirror_uid = original.MirrorUid;
                noteData.noteUid = original.NoteUid;
                noteData.scene = original.Scene;
                noteData.prefab_name = original.PrefabName;
                if (note.configData != null)
                {
                    var configData = note.configData;
                    configData.note_uid = original.ConfigNoteUid;
                    note.configData = configData;
                }

                try { note.noteData = noteData; } catch { }
                try { list[i] = note; } catch { }
                restored++;
            }
            catch { }
        }

        MelonLogger.Msg($"[SceneZzTransformTracker] 복구 UID zz분포: {FormatZzCounts(counts)}");
        return restored;
    }

    public static int RestoreRuntimeObjects(Il2CppGameLogic.GameMusicScene scene)
    {
        if (scene == null || OriginalsByObjId.Count == 0)
        {
            return 0;
        }

        int restored = 0;
        restored += RestoreObjectList("objCtrls", SafeGet(() => scene.objCtrls));
        restored += RestoreObjectList("preloads", SafeGet(() => scene.preloads));
        restored += RestoreObjectList("preloads1", SafeGet(() => scene.preloads1));
        DumpNonDefaultOriginalRuntimeObjects(scene);
        return restored;
    }

    private static void DumpNonDefaultOriginalRuntimeObjects(Il2CppGameLogic.GameMusicScene scene)
    {
        if (scene == null)
        {
            return;
        }

        foreach (var pair in OriginalsByObjId)
        {
            var original = pair.Value;
            if (original == null || string.IsNullOrEmpty(original.Uid) || original.Uid.StartsWith("07"))
            {
                continue;
            }

            MelonLogger.Msg($"[SceneZzTransformTracker] non-07 original tracked: objId={pair.Key}, originalUid={original.Uid}, renderUid={original.RenderUid}, scene={original.Scene}, prefab={original.PrefabName}");
            DumpRuntimeObjectForObjId("objCtrls", SafeGet(() => scene.objCtrls), pair.Key);
            DumpRuntimeObjectForObjId("preloads", SafeGet(() => scene.preloads), pair.Key);
            DumpRuntimeObjectForObjId("preloads1", SafeGet(() => scene.preloads1), pair.Key);
        }
    }

    private static void DumpRuntimeObjectForObjId(string label, object listObj, short objId)
    {
        int count = GetListCount(listObj);
        if (count < 0)
        {
            return;
        }

        for (int i = 0; i < count; i++)
        {
            object item = GetListItem(listObj, i);
            if (item == null)
            {
                continue;
            }

            if (ObjectContainsObjId(item, objId, 0, new System.Collections.Generic.HashSet<int>()))
            {
                MelonLogger.Msg($"[SceneZzTransformTracker] runtime object dump: list={label}, index={i}, type={item.GetType().FullName}");
                DumpObjectScalars(item, 0, new System.Collections.Generic.HashSet<int>());
                return;
            }
        }
    }

    private static bool ObjectContainsObjId(object obj, short objId, int depth, System.Collections.Generic.HashSet<int> visited)
    {
        if (obj == null || depth > 2)
        {
            return false;
        }

        int identity = System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(obj);
        if (!visited.Add(identity))
        {
            return false;
        }

        if (obj is Il2CppGameLogic.MusicData musicData)
        {
            return musicData.objId == objId;
        }

        var type = obj.GetType();
        const System.Reflection.BindingFlags flags =
            System.Reflection.BindingFlags.Public |
            System.Reflection.BindingFlags.NonPublic |
            System.Reflection.BindingFlags.Instance;

        foreach (var field in type.GetFields(flags))
        {
            try
            {
                object value = field.GetValue(obj);
                if (value is Il2CppGameLogic.MusicData fieldMusicData && fieldMusicData.objId == objId)
                {
                    return true;
                }

                if (ShouldInspectNested(value) && ObjectContainsObjId(value, objId, depth + 1, visited))
                {
                    return true;
                }
            }
            catch { }
        }

        foreach (var prop in type.GetProperties(flags))
        {
            try
            {
                if (!prop.CanRead || prop.GetIndexParameters().Length != 0)
                {
                    continue;
                }

                object value = prop.GetValue(obj);
                if (value is Il2CppGameLogic.MusicData propMusicData && propMusicData.objId == objId)
                {
                    return true;
                }

                if (ShouldInspectNested(value) && ObjectContainsObjId(value, objId, depth + 1, visited))
                {
                    return true;
                }
            }
            catch { }
        }

        return false;
    }

    private static void DumpObjectScalars(object obj, int depth, System.Collections.Generic.HashSet<int> visited)
    {
        if (obj == null || depth > 1)
        {
            return;
        }

        int identity = System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(obj);
        if (!visited.Add(identity))
        {
            return;
        }

        var type = obj.GetType();
        const System.Reflection.BindingFlags flags =
            System.Reflection.BindingFlags.Public |
            System.Reflection.BindingFlags.NonPublic |
            System.Reflection.BindingFlags.Instance;

        foreach (var field in type.GetFields(flags))
        {
            try
            {
                object value = field.GetValue(obj);
                if (IsInterestingScalar(value))
                {
                    MelonLogger.Msg($"[SceneZzTransformTracker]   scalar field depth={depth}: {type.Name}.{field.Name}={value}");
                }
                else if (value is Il2CppGameLogic.MusicData musicData)
                {
                    MelonLogger.Msg($"[SceneZzTransformTracker]   MusicData field depth={depth}: {type.Name}.{field.Name}, objId={musicData.objId}, uid={musicData.noteData?.uid}, scene={musicData.noteData?.scene}, prefab={musicData.noteData?.prefab_name}, configUid={musicData.configData?.note_uid}");
                }
                else if (ShouldInspectNested(value))
                {
                    DumpObjectScalars(value, depth + 1, visited);
                }
            }
            catch { }
        }

        foreach (var prop in type.GetProperties(flags))
        {
            try
            {
                if (!prop.CanRead || prop.GetIndexParameters().Length != 0)
                {
                    continue;
                }

                object value = prop.GetValue(obj);
                if (IsInterestingScalar(value))
                {
                    MelonLogger.Msg($"[SceneZzTransformTracker]   scalar prop depth={depth}: {type.Name}.{prop.Name}={value}");
                }
                else if (value is Il2CppGameLogic.MusicData musicData)
                {
                    MelonLogger.Msg($"[SceneZzTransformTracker]   MusicData prop depth={depth}: {type.Name}.{prop.Name}, objId={musicData.objId}, uid={musicData.noteData?.uid}, scene={musicData.noteData?.scene}, prefab={musicData.noteData?.prefab_name}, configUid={musicData.configData?.note_uid}");
                }
                else if (ShouldInspectNested(value))
                {
                    DumpObjectScalars(value, depth + 1, visited);
                }
            }
            catch { }
        }
    }

    private static bool IsInterestingScalar(object value)
    {
        if (value == null)
        {
            return false;
        }

        if (value is string text)
        {
            return text.Contains("071304") || text.Contains("121304") || text.Contains("71304") || text.Contains("121304");
        }

        if (value is int intValue)
        {
            return intValue == 71304 || intValue == 121304;
        }

        if (value is uint uintValue)
        {
            return uintValue == 71304 || uintValue == 121304;
        }

        return false;
    }

    private static int RestoreObjectList(string label, object listObj)
    {
        if (listObj == null)
        {
            return 0;
        }

        int restored = 0;
        int count = GetListCount(listObj);
        if (count < 0)
        {
            return 0;
        }

        var itemTypes = new System.Collections.Generic.HashSet<string>();
        for (int i = 0; i < count; i++)
        {
            object item = GetListItem(listObj, i);
            if (item == null)
            {
                continue;
            }

            if (itemTypes.Count < 4)
            {
                itemTypes.Add(item.GetType().FullName ?? item.GetType().Name);
            }

            restored += RestoreObjectMusicData(item, 0, new System.Collections.Generic.HashSet<int>());
        }

        MelonLogger.Msg($"[SceneZzTransformTracker] runtime list scan: {label}, count={count}, restored={restored}, itemTypes=[{string.Join(", ", itemTypes)}]");
        return restored;
    }

    private static int RestoreObjectMusicData(object obj, int depth, System.Collections.Generic.HashSet<int> visited)
    {
        if (obj == null || depth > 2)
        {
            return 0;
        }

        int identity = System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(obj);
        if (!visited.Add(identity))
        {
            return 0;
        }

        if (obj is Il2CppGameLogic.MusicData direct)
        {
            return RestoreMusicData(ref direct) ? 1 : 0;
        }

        int restored = 0;
        var type = obj.GetType();
        const System.Reflection.BindingFlags flags =
            System.Reflection.BindingFlags.Public |
            System.Reflection.BindingFlags.NonPublic |
            System.Reflection.BindingFlags.Instance;

        foreach (var field in type.GetFields(flags))
        {
            try
            {
                object value = field.GetValue(obj);
                if (value is Il2CppGameLogic.MusicData musicData)
                {
                    if (RestoreMusicData(ref musicData))
                    {
                        try { field.SetValue(obj, musicData); } catch { }
                        restored++;
                    }
                    continue;
                }

                if (TryRestoreScalarField(obj, field, value))
                {
                    restored++;
                    continue;
                }

                if (ShouldInspectNested(value))
                {
                    restored += RestoreObjectMusicData(value, depth + 1, visited);
                }
            }
            catch { }
        }

        foreach (var prop in type.GetProperties(flags))
        {
            try
            {
                if (!prop.CanRead || prop.GetIndexParameters().Length != 0)
                {
                    continue;
                }

                object value = prop.GetValue(obj);
                if (value is Il2CppGameLogic.MusicData musicData)
                {
                    if (RestoreMusicData(ref musicData))
                    {
                        if (prop.CanWrite)
                        {
                            try { prop.SetValue(obj, musicData); } catch { }
                        }
                        restored++;
                    }
                    continue;
                }

                if (TryRestoreScalarProperty(obj, prop, value))
                {
                    restored++;
                    continue;
                }

                if (ShouldInspectNested(value))
                {
                    restored += RestoreObjectMusicData(value, depth + 1, visited);
                }
            }
            catch { }
        }

        return restored;
    }

    private static bool TryRestoreScalarField(object obj, System.Reflection.FieldInfo field, object value)
    {
        if (obj == null || field == null || value == null) return false;

        foreach (var original in OriginalsByObjId.Values)
        {
            if (TryGetRestoredScalar(value, field.FieldType, original, out object restored))
            {
                try
                {
                    field.SetValue(obj, restored);
                    return true;
                }
                catch { return false; }
            }
        }

        return false;
    }

    private static bool TryRestoreScalarProperty(object obj, System.Reflection.PropertyInfo prop, object value)
    {
        if (obj == null || prop == null || !prop.CanWrite || value == null) return false;

        foreach (var original in OriginalsByObjId.Values)
        {
            if (TryGetRestoredScalar(value, prop.PropertyType, original, out object restored))
            {
                try
                {
                    prop.SetValue(obj, restored);
                    return true;
                }
                catch { return false; }
            }
        }

        return false;
    }

    private static bool TryGetRestoredScalar(object value, Type targetType, OriginalIdentity original, out object restored)
    {
        restored = null;
        if (original == null)
        {
            return false;
        }

        if (targetType == typeof(string) && value is string text)
        {
            if (!string.IsNullOrEmpty(original.RenderPrefabName) && text.Contains(original.RenderPrefabName))
            {
                restored = text.Replace(original.RenderPrefabName, original.PrefabName ?? original.Uid);
                return true;
            }

            if (!string.IsNullOrEmpty(original.RenderUid) && text.Contains(original.RenderUid))
            {
                restored = text.Replace(original.RenderUid, original.Uid);
                return true;
            }

            if (!string.IsNullOrEmpty(original.RenderUid) && text == original.RenderUid)
            {
                restored = original.Uid;
                return true;
            }

            if (!string.IsNullOrEmpty(original.RenderMirrorUid) && text == original.RenderMirrorUid)
            {
                restored = original.MirrorUid;
                return true;
            }

            if (!string.IsNullOrEmpty(original.RenderConfigNoteUid) && text == original.RenderConfigNoteUid)
            {
                restored = original.ConfigNoteUid;
                return true;
            }
        }

        if (targetType == typeof(int) && value is int intValue && intValue == original.RenderNoteUid)
        {
            restored = original.NoteUid;
            return true;
        }

        if (targetType == typeof(short) && value is short shortValue && shortValue == original.RenderNoteUid)
        {
            restored = (short)original.NoteUid;
            return true;
        }

        if (targetType == typeof(uint) && value is uint uintValue && uintValue == (uint)original.RenderNoteUid)
        {
            restored = (uint)original.NoteUid;
            return true;
        }

        return false;
    }

    private static bool RestoreMusicData(ref Il2CppGameLogic.MusicData note)
    {
        if (note?.noteData == null)
        {
            return false;
        }

        if (!OriginalsByObjId.TryGetValue(note.objId, out var original))
        {
            return false;
        }

        var noteData = note.noteData;
        noteData.uid = original.Uid;
        noteData.mirror_uid = original.MirrorUid;
        noteData.noteUid = original.NoteUid;
        noteData.scene = original.Scene;
        noteData.prefab_name = original.PrefabName;
        note.noteData = noteData;

        if (note.configData != null)
        {
            var configData = note.configData;
            configData.note_uid = original.ConfigNoteUid;
            note.configData = configData;
        }

        return true;
    }

    private static OriginalIdentity CaptureIdentity(Il2CppGameLogic.MusicData note)
    {
        return new OriginalIdentity
        {
            Uid = note.noteData.uid,
            MirrorUid = note.noteData.mirror_uid,
            NoteUid = note.noteData.noteUid,
            ConfigNoteUid = note.configData?.note_uid,
            Scene = note.noteData.scene,
            PrefabName = note.noteData.prefab_name,
            RenderUid = note.noteData.uid,
            RenderMirrorUid = note.noteData.mirror_uid,
            RenderNoteUid = note.noteData.noteUid,
            RenderConfigNoteUid = note.configData?.note_uid,
            RenderPrefabName = note.noteData.prefab_name
        };
    }

    private static bool IsSixDigitUid(string uid)
    {
        if (string.IsNullOrEmpty(uid) || uid.Length != 6) return false;
        for (int i = 0; i < uid.Length; i++)
        {
            if (uid[i] < '0' || uid[i] > '9') return false;
        }

        return true;
    }

    private static bool ShouldInspectNested(object value)
    {
        if (value == null)
        {
            return false;
        }

        string typeName = value.GetType().FullName ?? string.Empty;
        return typeName.StartsWith("Il2Cpp", System.StringComparison.Ordinal)
            && !typeName.StartsWith("Il2CppSystem.Collections", System.StringComparison.Ordinal)
            && !typeName.Contains("String");
    }

    private static object SafeGet(System.Func<object> getter)
    {
        try { return getter(); }
        catch { return null; }
    }

    private static int GetListCount(object listObj)
    {
        try
        {
            var prop = listObj.GetType().GetProperty("Count");
            if (prop == null) return -1;
            object value = prop.GetValue(listObj);
            return value is int count ? count : -1;
        }
        catch { return -1; }
    }

    private static object GetListItem(object listObj, int index)
    {
        try
        {
            var prop = listObj.GetType().GetProperty("Item");
            return prop != null ? prop.GetValue(listObj, new object[] { index }) : null;
        }
        catch { return null; }
    }

    private static void CountZz(System.Collections.Generic.SortedDictionary<string, int> counts, string uid)
    {
        if (string.IsNullOrEmpty(uid) || uid.Length < 2) return;
        string zz = uid.Substring(0, 2);
        counts[zz] = counts.TryGetValue(zz, out int count) ? count + 1 : 1;
    }

    private static string FormatZzCounts(System.Collections.Generic.SortedDictionary<string, int> counts)
    {
        if (counts == null || counts.Count == 0)
        {
            return "{}";
        }

        var parts = new System.Collections.Generic.List<string>();
        foreach (var pair in counts)
        {
            parts.Add($"{pair.Key}:{pair.Value}");
        }

        return "{" + string.Join(", ", parts) + "}";
    }
}

[HarmonyLib.HarmonyPatch(typeof(Il2CppGameLogic.GameMusicScene), "InitSceneEvents")]
public class GameMusicScene_InitSceneEvents_Patch
{
    public static void Prefix(Il2CppGameLogic.GameMusicScene __instance)
    {
        try
        {
            if (!muse_dash_test.CustomPlaySession.Current.ShouldApplyExperimentChart) return;

            int frame = 0;
            try { frame = UnityEngine.Time.frameCount; } catch { }
            MelonLogger.Msg($"[GameMusicScene.InitSceneEvents] PRE frame={frame}, curSceneName={SafeCurSceneName(__instance)}");
        }
        catch (Exception ex) { MelonLogger.Error($"[GameMusicScene.InitSceneEvents] Prefix 예외: {ex}"); }
    }

    public static void Postfix(Il2CppGameLogic.GameMusicScene __instance)
    {
        try
        {
            if (!muse_dash_test.CustomPlaySession.Current.ShouldApplyExperimentChart) return;

            int sceneCount = -1;
            try { sceneCount = __instance != null && __instance.scenes != null ? __instance.scenes.Count : -1; } catch { }
            MelonLogger.Msg($"[GameMusicScene.InitSceneEvents] POST scenes.Count={sceneCount}, curSceneName={SafeCurSceneName(__instance)}");
        }
        catch (Exception ex) { MelonLogger.Error($"[GameMusicScene.InitSceneEvents] Postfix 예외: {ex}"); }
    }

    private static string SafeCurSceneName(Il2CppGameLogic.GameMusicScene scene)
    {
        try { return scene != null ? (scene.curSceneName ?? "(null)") : "(null)"; }
        catch { return "(예외)"; }
    }
}
