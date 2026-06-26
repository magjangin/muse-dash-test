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
            var listType = listObj.GetType();
            var countProp = GetCountProperty(listType);
            if (countProp == null) return 0;

            int count = (int)countProp.GetValue(listObj);
            var itemProp = GetItemProperty(listType);
            if (itemProp == null) return 0;

            var itemTypes = new HashSet<string>();
            var indexArgs = new object[1];
            for (int i = 0; i < count; i++)
            {
                indexArgs[0] = i;
                object item = itemProp.GetValue(listObj, indexArgs);
                if (item == null) continue;

                if (itemTypes.Count < 4)
                {
                    itemTypes.Add(item.GetType().FullName ?? item.GetType().Name);
                }

                restored += RestoreObjectMusicData(item, 0, new HashSet<int>());
            }

            MelonLogger.Msg($"[SceneZzTransformTracker] runtime list scan: {label}, count={count}, restored={restored}, itemTypes=[{string.Join(", ", itemTypes)}]");
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
            if (!OriginalsByObjId.TryGetValue(note.objId, out var original)) return false;

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
