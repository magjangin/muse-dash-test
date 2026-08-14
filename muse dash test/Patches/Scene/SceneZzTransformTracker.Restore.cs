using MelonLoader;
using System;
using System.Collections.Generic;
using System.Reflection;
using Il2CppGameLogic;

namespace muse_dash_test
{
    /// <summary>
    /// 런타임 오브젝트 트리를 재귀적으로 순회하며 보관된 원본 MusicData/식별 값을 복구하는 로직.
    /// </summary>
    internal static partial class SceneZzTransformTracker
    {
        private static int RestoreObjectList(string label, object listObj)
        {
            if (listObj == null) return 0;

            int restored = 0;
            var inspectedObjects = new HashSet<int>();

            // 1. objCtrls (List<BaseSpineObjectController>) 초고속 다이렉트 패스
            if (listObj is Il2CppSystem.Collections.Generic.List<Il2Cpp.BaseSpineObjectController> spineList)
            {
                int count = spineList.Count;
                for (int i = 0; i < count; i++)
                {
                    var item = spineList[i];
                    if (item == null) continue;
                    restored += RestoreBaseSpineController(item, inspectedObjects);
                }

                if (SceneDiagnosticLogger.ShouldLog($"SceneZzTransformTracker.RestoreList.{label}", 20))
                {
                    MelonLogger.Msg($"[SceneZzTransformTracker] runtime list scan: {label}, count={count}, restored={restored}, itemTypes=[Il2Cpp.BaseSpineObjectController]");
                }
                return restored;
            }

            // 2. preloads (List<GameObject>) 초고속 다이렉트 패스
            if (listObj is Il2CppSystem.Collections.Generic.List<UnityEngine.GameObject> goList)
            {
                int count = goList.Count;
                for (int i = 0; i < count; i++)
                {
                    var go = goList[i];
                    if (go == null) continue;
                    restored += RestoreGameObject(go, inspectedObjects);
                }

                if (SceneDiagnosticLogger.ShouldLog($"SceneZzTransformTracker.RestoreList.{label}", 20))
                {
                    MelonLogger.Msg($"[SceneZzTransformTracker] runtime list scan: {label}, count={count}, restored={restored}, itemTypes=[UnityEngine.GameObject]");
                }
                return restored;
            }

            // 3. preloads1 (List<List<GameObject>>) 초고속 다이렉트 패스
            if (listObj is Il2CppSystem.Collections.Generic.List<Il2CppSystem.Collections.Generic.List<UnityEngine.GameObject>> nestedGoList)
            {
                int count = nestedGoList.Count;
                for (int i = 0; i < count; i++)
                {
                    var subList = nestedGoList[i];
                    if (subList == null) continue;
                    int subCount = subList.Count;
                    for (int j = 0; j < subCount; j++)
                    {
                        var go = subList[j];
                        if (go == null) continue;
                        restored += RestoreGameObject(go, inspectedObjects);
                    }
                }

                if (SceneDiagnosticLogger.ShouldLog($"SceneZzTransformTracker.RestoreList.{label}", 20))
                {
                    MelonLogger.Msg($"[SceneZzTransformTracker] runtime list scan: {label}, count={count}, restored={restored}, itemTypes=[Il2CppSystem.Collections.Generic.List`1[[UnityEngine.GameObject]]]");
                }
                return restored;
            }

            // 4. 일반 폴백 (리플렉션 + 단일 visitedSet 재사용)
            var listType = listObj.GetType();
            var countProp = GetCountProperty(listType);
            if (countProp == null) return 0;

            int fallbackCount = (int)countProp.GetValue(listObj);
            var itemProp = GetItemProperty(listType);
            if (itemProp == null) return 0;

            var itemTypes = new HashSet<string>();
            var indexArgs = new object[1];
            for (int i = 0; i < fallbackCount; i++)
            {
                indexArgs[0] = i;
                object item = itemProp.GetValue(listObj, indexArgs);
                if (item == null) continue;

                if (itemTypes.Count < 4)
                {
                    itemTypes.Add(item.GetType().FullName ?? item.GetType().Name);
                }

                restored += RestoreObjectMusicData(item, 0, inspectedObjects);
            }

            if (SceneDiagnosticLogger.ShouldLog($"SceneZzTransformTracker.RestoreList.{label}", 20))
            {
                var preview = itemTypes.Count == 0 ? "(none)" : string.Join(", ", itemTypes);
                MelonLogger.Msg($"[SceneZzTransformTracker] runtime list scan: {label}, count={fallbackCount}, restored={restored}, itemTypes=[{preview}]");
            }
            return restored;
        }

        private static readonly Dictionary<Type, FieldInfo[]> s_TypeMusicDataFields = new Dictionary<Type, FieldInfo[]>();

        private static FieldInfo[] GetTypeMusicDataFields(Type type)
        {
            if (!s_TypeMusicDataFields.TryGetValue(type, out var fields))
            {
                var list = new List<FieldInfo>();
                foreach (var f in GetFieldsCached(type))
                {
                    if (f.FieldType == typeof(MusicData))
                    {
                        list.Add(f);
                    }
                }
                fields = list.ToArray();
                s_TypeMusicDataFields[type] = fields;
            }
            return fields;
        }

        private static int RestoreBaseSpineController(object item, HashSet<int> inspectedObjects)
        {
            if (item == null) return 0;
            int identity = System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(item);
            if (!inspectedObjects.Add(identity)) return 0;

            int restored = 0;
            try
            {
                var fields = GetTypeMusicDataFields(item.GetType());
                for (int i = 0; i < fields.Length; i++)
                {
                    object val = fields[i].GetValue(item);
                    if (val is MusicData md)
                    {
                        if (RestoreMusicData(ref md))
                        {
                            try { fields[i].SetValue(item, md); restored++; } catch (Exception) { }
                        }
                    }
                }
            }
            catch (Exception) { }

            return restored;
        }

        private static int RestoreGameObject(UnityEngine.GameObject go, HashSet<int> inspectedObjects)
        {
            if (go == null) return 0;
            int identity = System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(go);
            if (!inspectedObjects.Add(identity)) return 0;

            int restored = 0;
            try
            {
                var comps = go.GetComponents<UnityEngine.Component>();
                if (comps != null)
                {
                    for (int i = 0; i < comps.Length; i++)
                    {
                        var comp = comps[i];
                        if (comp != null && ShouldInspectNested(comp))
                        {
                            restored += RestoreBaseSpineController(comp, inspectedObjects);
                        }
                    }
                }
            }
            catch (Exception) { }

            return restored;
        }

        /// <summary>
        /// [재귀 복구] 런타임 객체 트리의 필드를 타고 내려가며 보관된 MusicData 및 식별 값을 복원합니다.
        /// </summary>
        private static int RestoreObjectMusicData(object obj, int depth, HashSet<int> inspectedObjects)
        {
            if (obj == null || depth > 2) return 0;

            int identity = System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(obj);
            if (!inspectedObjects.Add(identity)) return 0;

            if (obj is MusicData direct)
            {
                return RestoreMusicData(ref direct) ? 1 : 0;
            }

            // preloads1은 List<List<GameObject>> 형태입니다. 일반 중첩 탐색에서는
            // Il2CppSystem.*을 차단하므로, 인덱스 컬렉션은 명시적으로 펼쳐서 내부
            // GameObject까지 전달합니다.
            if (TryRestoreIndexedCollection(obj, depth, inspectedObjects, out int collectionRestored))
            {
                return collectionRestored;
            }

            int restored = 0;
            var type = obj.GetType();

            // 1. 필드 탐색 및 복구
            foreach (var field in GetFieldsCached(type))
            {
                try
                {
                    object value = field.GetValue(obj);
                    if (value is MusicData musicData)
                    {
                        if (RestoreMusicData(ref musicData))
                        {
                            try { field.SetValue(obj, musicData); restored++; } catch (Exception) { }
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
                        restored += RestoreObjectMusicData(value, depth + 1, inspectedObjects);
                    }
                }
                catch (Exception)
                {
                    // Ignored: 런타임 객체 필드 탐색 중 예외 무시
                }
            }

            // 2. 프로퍼티 탐색 및 복구
            foreach (var prop in GetPropertiesCached(type))
            {
                try
                {
                    object value = prop.GetValue(obj);
                    if (value is MusicData musicData)
                    {
                        if (RestoreMusicData(ref musicData) && prop.CanWrite)
                        {
                            try { prop.SetValue(obj, musicData); restored++; } catch (Exception) { }
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
                        restored += RestoreObjectMusicData(value, depth + 1, inspectedObjects);
                    }
                }
                catch (Exception)
                {
                    // Ignored: 런타임 객체 프로퍼티 탐색 중 예외 무시
                }
            }

            return restored;
        }

        private static bool TryRestoreIndexedCollection(
            object obj,
            int depth,
            HashSet<int> inspectedObjects,
            out int restored)
        {
            restored = 0;
            var type = obj.GetType();
            var countProp = GetCountProperty(type);
            var itemProp = GetItemProperty(type);
            if (countProp == null || itemProp == null) return false;

            try
            {
                int count = (int)countProp.GetValue(obj);
                var indexArgs = new object[1];
                for (int i = 0; i < count; i++)
                {
                    indexArgs[0] = i;
                    object item = itemProp.GetValue(obj, indexArgs);
                    restored += RestoreObjectMusicData(item, depth + 1, inspectedObjects);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static bool TryRestoreScalarField(object obj, FieldInfo field, object value)
        {
            if (obj == null || field == null || value == null) return false;

            if (TryGetRestoredScalar(value, field.FieldType, out object restored))
            {
                try
                {
                    field.SetValue(obj, restored);
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return false;
        }

        private static bool TryRestoreScalarProperty(object obj, PropertyInfo prop, object value)
        {
            if (obj == null || prop == null || !prop.CanWrite || value == null) return false;

            if (TryGetRestoredScalar(value, prop.PropertyType, out object restored))
            {
                try
                {
                    prop.SetValue(obj, restored);
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return false;
        }

        private static bool TryGetRestoredScalar(object value, Type targetType, out object restored)
        {
            restored = null;
            if (value == null) return false;

            if (targetType == typeof(string) && value is string text)
            {
                // 1. 다이렉트 매칭 (O(1))
                if (OriginalsByRenderUid.TryGetValue(text, out var origUid))
                {
                    restored = origUid.Uid;
                    return true;
                }
                if (OriginalsByRenderMirrorUid.TryGetValue(text, out var origMirror))
                {
                    restored = origMirror.MirrorUid;
                    return true;
                }
                if (OriginalsByRenderConfigNoteUid.TryGetValue(text, out var origConfig))
                {
                    restored = origConfig.ConfigNoteUid;
                    return true;
                }

                // 2. 부분 일치 검색
                foreach (var orig in OriginalsWithRenderPrefabName)
                {
                    if (text.Contains(orig.RenderPrefabName))
                    {
                        restored = text.Replace(orig.RenderPrefabName, orig.PrefabName ?? orig.Uid);
                        return true;
                    }
                }
                foreach (var orig in OriginalsByRenderUid.Values)
                {
                    if (text.Contains(orig.RenderUid))
                    {
                        restored = text.Replace(orig.RenderUid, orig.Uid);
                        return true;
                    }
                }
            }
            else if (targetType == typeof(int) && value is int intValue)
            {
                if (OriginalsByRenderNoteUid.TryGetValue(intValue, out var orig))
                {
                    restored = orig.NoteUid;
                    return true;
                }
            }
            else if (targetType == typeof(short) && value is short shortValue)
            {
                if (OriginalsByRenderNoteUid.TryGetValue(shortValue, out var orig))
                {
                    restored = (short)orig.NoteUid;
                    return true;
                }
            }
            else if (targetType == typeof(uint) && value is uint uintValue)
            {
                if (OriginalsByRenderNoteUid.TryGetValue((int)uintValue, out var orig))
                {
                    restored = (uint)orig.NoteUid;
                    return true;
                }
            }

            return false;
        }

        private static bool RestoreMusicData(ref MusicData note)
        {
            if (note?.noteData == null) return false;
            if (!TryResolveOriginalIdentity(note, out var original)) return false;

            var noteData = note.noteData;
            bool changed = false;

            if (!string.Equals(noteData.uid, original.Uid, StringComparison.Ordinal))
            {
                noteData.uid = original.Uid;
                changed = true;
            }

            if (!string.Equals(noteData.mirror_uid, original.MirrorUid, StringComparison.Ordinal))
            {
                noteData.mirror_uid = original.MirrorUid;
                changed = true;
            }

            if (noteData.noteUid != original.NoteUid)
            {
                noteData.noteUid = original.NoteUid;
                changed = true;
            }

            if (!string.Equals(noteData.scene, original.Scene, StringComparison.Ordinal))
            {
                noteData.scene = original.Scene;
                changed = true;
            }

            if (!string.Equals(noteData.prefab_name, original.PrefabName, StringComparison.Ordinal))
            {
                noteData.prefab_name = original.PrefabName;
                changed = true;
            }

            if (!string.Equals(noteData.key_audio, original.KeyAudio, StringComparison.Ordinal))
            {
                noteData.key_audio = original.KeyAudio;
                changed = true;
            }

            note.noteData = noteData;

            if (note.configData != null)
            {
                var configData = note.configData;
                if (!string.Equals(configData.note_uid, original.ConfigNoteUid, StringComparison.Ordinal))
                {
                    configData.note_uid = original.ConfigNoteUid;
                    changed = true;
                }
                note.configData = configData;
            }

            return changed;
        }

        private static OriginalIdentity CaptureIdentity(MusicData note)
        {
            return new OriginalIdentity
            {
                Uid = note.noteData.uid,
                MirrorUid = note.noteData.mirror_uid,
                NoteUid = note.noteData.noteUid,
                ConfigNoteUid = note.configData?.note_uid,
                Scene = note.noteData.scene,
                PrefabName = note.noteData.prefab_name,
                KeyAudio = note.noteData.key_audio,
                RenderUid = note.noteData.uid,
                RenderMirrorUid = note.noteData.mirror_uid,
                RenderNoteUid = note.noteData.noteUid,
                RenderConfigNoteUid = note.configData?.note_uid,
                RenderPrefabName = note.noteData.prefab_name,
                RenderKeyAudio = note.noteData.key_audio
            };
        }

        /// <summary>
        /// 탐색 가치가 있는 유니티 내부 IL2CPP 타입 객체인지 필터링합니다. (순환 스캔 최적화)
        /// </summary>
        private static bool ShouldInspectNested(object value)
        {
            if (value == null) return false;

            string typeName = value.GetType().FullName ?? string.Empty;

            // Unity 엔진 및 시스템 기본 형식 검색 차단 (GC 및 네이티브 속성 탐색 속도 대폭 개선)
            return typeName.StartsWith("Il2Cpp", StringComparison.Ordinal)
                && !typeName.StartsWith("Il2CppUnityEngine.", StringComparison.Ordinal)
                && !typeName.StartsWith("Il2CppSystem.", StringComparison.Ordinal)
                && !typeName.StartsWith("UnityEngine.", StringComparison.Ordinal)
                && !typeName.StartsWith("System.", StringComparison.Ordinal)
                && !typeName.Contains("String");
        }

        private static object SafeGet(Func<object> getter)
        {
            try { return getter(); }
            catch (Exception) { return null; }
        }
    }
}
