using MelonLoader;
using System;
using System.Reflection;
using System.Collections.Generic;
using Il2CppGameLogic;

namespace muse_dash_test
{
    /// <summary>
    /// 커스텀 씬 전환 과정에서 변경된 MusicData의 식별자(Uid, noteUid 등)를 기록하고,
    /// 게임 런타임 중에 다시 원본 값으로 복구하거나 상태를 비교하는 역할을 담당하는 추적기입니다.
    /// </summary>
    internal static class SceneZzTransformTracker
    {
        // 리플렉션 탐색 시 사용하는 기본 바인딩 플래그 상수
        private const BindingFlags DefaultFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

        /// <summary>
        /// 런타임 오브젝트 복구를 위해 보존하는 원본 MusicData의 식별 정보 구조체입니다.
        /// </summary>
        private sealed class OriginalIdentity
        {
            public string Uid;
            public string MirrorUid;
            public int NoteUid;
            public string ConfigNoteUid;
            public string Scene;
            public string PrefabName;

            // 변환을 적용한 렌더러용 식별 정보
            public string RenderUid;
            public string RenderMirrorUid;
            public int RenderNoteUid;
            public string RenderConfigNoteUid;
            public string RenderPrefabName;
        }

        // 오브젝트 고유 ID(objId) 기준 원본 식별자 매핑 정보
        private static readonly Dictionary<short, OriginalIdentity> OriginalsByObjId = new Dictionary<short, OriginalIdentity>();
        private static readonly Dictionary<short, OriginalIdentity> BmsOriginalsByObjId = new Dictionary<short, OriginalIdentity>();

        public static int Count => OriginalsByObjId.Count;
        public static int BmsOriginalCount => BmsOriginalsByObjId.Count;

        /// <summary>
        /// BMS 파일 원본의 UID를 objId로 조회합니다.
        /// </summary>
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

        /// <summary>
        /// BMS 로드 시점에 원본 오브젝트 ID 정보들을 캐싱 등록합니다.
        /// </summary>
        public static void RegisterBmsOriginalIdentities(Il2CppSystem.Collections.Generic.List<MusicData> list, int startIndex)
        {
            BmsOriginalsByObjId.Clear();
            if (list == null) return;

            var counts = new SortedDictionary<string, int>();
            for (int i = Math.Max(0, startIndex); i < list.Count; i++)
            {
                try
                {
                    var note = list[i];
                    if (note?.noteData == null) continue;
                    if (string.IsNullOrEmpty(note.noteData.uid)) continue;

                    BmsOriginalsByObjId[note.objId] = CaptureIdentity(note);
                    CountZz(counts, note.noteData.uid);
                }
                catch { }
            }

            MelonLogger.Msg($"[SceneZzTransformTracker] BMS 원본 UID 등록: count={BmsOriginalsByObjId.Count}, zz분포={FormatZzCounts(counts)}");
        }

        /// <summary>
        /// 특정 노트를 새로운 UID와 PrefabName으로 변형하여 기록해 둡니다.
        /// </summary>
        public static void Record(MusicData note, string renderUid, string renderPrefabName)
        {
            if (note?.noteData == null) return;

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

        /// <summary>
        /// MusicData 목록 전체를 돌며 기록해 두었던 원본 식별 정보로 일괄 복구합니다.
        /// </summary>
        public static int RestoreIdentities(Il2CppSystem.Collections.Generic.List<MusicData> list)
        {
            if (list == null || OriginalsByObjId.Count == 0) return 0;

            int restored = 0;
            var counts = new SortedDictionary<string, int>();
            for (int i = 0; i < list.Count; i++)
            {
                try
                {
                    var note = list[i];
                    if (note?.noteData == null) continue;
                    if (!OriginalsByObjId.TryGetValue(note.objId, out var original)) continue;

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

        /// <summary>
        /// 씬에 생성된 실제 런타임 컨트롤러 객체들을 깊게 탐색하여 원래의 원본 값(uid, scene 등)으로 되돌려놓습니다.
        /// </summary>
        public static int RestoreRuntimeObjects(GameMusicScene scene)
        {
            if (scene == null || OriginalsByObjId.Count == 0) return 0;

            int restored = 0;
            restored += RestoreObjectList("objCtrls", SafeGet(() => scene.objCtrls));
            restored += RestoreObjectList("preloads", SafeGet(() => scene.preloads));
            restored += RestoreObjectList("preloads1", SafeGet(() => scene.preloads1));

            DumpNonDefaultOriginalRuntimeObjects(scene);
            return restored;
        }

        // 런타임 리스트에서 찾아낸 객체의 위치 정보(인덱스 + 실제 객체)
        private readonly struct RuntimeHit
        {
            public readonly int Index;
            public readonly object Item;

            public RuntimeHit(int index, object item)
            {
                Index = index;
                Item = item;
            }
        }

        private static void DumpNonDefaultOriginalRuntimeObjects(GameMusicScene scene)
        {
            if (scene == null) return;

            // 각 런타임 리스트를 단 한 번만 스캔해 objId→객체 인덱스를 만든다.
            // (기존엔 추적 원본마다 리스트 전체를 재귀 리플렉션으로 다시 훑어 O(원본×리스트) 였다.)
            var indexes = new (string Label, Dictionary<short, RuntimeHit> Map)[]
            {
                ("objCtrls", BuildObjIdIndex(SafeGet(() => scene.objCtrls))),
                ("preloads", BuildObjIdIndex(SafeGet(() => scene.preloads))),
                ("preloads1", BuildObjIdIndex(SafeGet(() => scene.preloads1))),
            };

            foreach (var pair in OriginalsByObjId)
            {
                var original = pair.Value;
                if (original == null || string.IsNullOrEmpty(original.Uid) || original.Uid.StartsWith("07"))
                {
                    continue;
                }

                MelonLogger.Msg($"[SceneZzTransformTracker] non-07 original tracked: objId={pair.Key}, originalUid={original.Uid}, renderUid={original.RenderUid}, scene={original.Scene}, prefab={original.PrefabName}");

                foreach (var index in indexes)
                {
                    if (index.Map.TryGetValue(pair.Key, out var hit))
                    {
                        MelonLogger.Msg($"[SceneZzTransformTracker] runtime object dump: list={index.Label}, index={hit.Index}, type={hit.Item.GetType().FullName}");
                        DumpObjectScalars(hit.Item, 0, new HashSet<int>());
                    }
                }
            }
        }

        /// <summary>
        /// 런타임 리스트를 한 번 순회하며 각 객체가 품고 있는 MusicData의 objId를 추출해 인덱스를 구축합니다.
        /// 동일 objId가 여러 번 나오면 기존 동작(첫 매칭만 덤프)과 맞추기 위해 첫 항목만 보존합니다.
        /// </summary>
        private static Dictionary<short, RuntimeHit> BuildObjIdIndex(object listObj)
        {
            var map = new Dictionary<short, RuntimeHit>();
            int count = GetListCount(listObj);
            if (count < 0) return map;

            for (int i = 0; i < count; i++)
            {
                object item = GetListItem(listObj, i);
                if (item == null) continue;

                if (TryGetObjId(item, 0, new HashSet<int>(), out short objId) && !map.ContainsKey(objId))
                {
                    map[objId] = new RuntimeHit(i, item);
                }
            }

            return map;
        }

        /// <summary>
        /// [재귀 탐색] 런타임 객체 트리 내부에서 첫 번째로 발견되는 MusicData의 objId를 추출합니다.
        /// </summary>
        /// <param name="obj">검사할 객체</param>
        /// <param name="depth">재귀 탐색 깊이 (최대 2단계 제한)</param>
        /// <param name="inspectedObjects">순환 참조 방지용 방문 기록 집합</param>
        /// <param name="objId">발견된 MusicData의 objId</param>
        private static bool TryGetObjId(object obj, int depth, HashSet<int> inspectedObjects, out short objId)
        {
            objId = 0;
            if (obj == null || depth > 2) return false;

            // 순환 참조 방지
            int identity = System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(obj);
            if (!inspectedObjects.Add(identity)) return false;

            // 1. 직접 찾으려는 MusicData 형식일 때
            if (obj is MusicData musicData)
            {
                objId = musicData.objId;
                return true;
            }

            var type = obj.GetType();

            // 2. 클래스 내부의 필드들을 리플렉션으로 검사
            foreach (var field in type.GetFields(DefaultFlags))
            {
                try
                {
                    object value = field.GetValue(obj);
                    if (value is MusicData fieldMusicData)
                    {
                        objId = fieldMusicData.objId;
                        return true;
                    }

                    if (ShouldInspectNested(value) && TryGetObjId(value, depth + 1, inspectedObjects, out objId))
                    {
                        return true;
                    }
                }
                catch { }
            }

            // 3. 클래스 내부의 프로퍼티들을 리플렉션으로 검사
            foreach (var prop in type.GetProperties(DefaultFlags))
            {
                try
                {
                    if (!prop.CanRead || prop.GetIndexParameters().Length != 0) continue;

                    object value = prop.GetValue(obj);
                    if (value is MusicData propMusicData)
                    {
                        objId = propMusicData.objId;
                        return true;
                    }

                    if (ShouldInspectNested(value) && TryGetObjId(value, depth + 1, inspectedObjects, out objId))
                    {
                        return true;
                    }
                }
                catch { }
            }

            return false;
        }

        /// <summary>
        /// [재귀 탐색] 특정 객체 내부의 문자열이나 숫자 필드를 로깅하여 값을 분석합니다.
        /// </summary>
        private static void DumpObjectScalars(object obj, int depth, HashSet<int> inspectedObjects)
        {
            if (obj == null || depth > 1) return;

            int identity = System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(obj);
            if (!inspectedObjects.Add(identity)) return;

            var type = obj.GetType();

            foreach (var field in type.GetFields(DefaultFlags))
            {
                try
                {
                    object value = field.GetValue(obj);
                    if (IsInterestingScalar(value))
                    {
                        MelonLogger.Msg($"[SceneZzTransformTracker]   scalar field depth={depth}: {type.Name}.{field.Name}={value}");
                    }
                    else if (value is MusicData musicData)
                    {
                        MelonLogger.Msg($"[SceneZzTransformTracker]   MusicData field depth={depth}: {type.Name}.{field.Name}, objId={musicData.objId}, uid={musicData.noteData?.uid}, scene={musicData.noteData?.scene}, prefab={musicData.noteData?.prefab_name}, configUid={musicData.configData?.note_uid}");
                    }
                    else if (ShouldInspectNested(value))
                    {
                        DumpObjectScalars(value, depth + 1, inspectedObjects);
                    }
                }
                catch { }
            }

            foreach (var prop in type.GetProperties(DefaultFlags))
            {
                try
                {
                    if (!prop.CanRead || prop.GetIndexParameters().Length != 0) continue;

                    object value = prop.GetValue(obj);
                    if (IsInterestingScalar(value))
                    {
                        MelonLogger.Msg($"[SceneZzTransformTracker]   scalar prop depth={depth}: {type.Name}.{prop.Name}={value}");
                    }
                    else if (value is MusicData musicData)
                    {
                        MelonLogger.Msg($"[SceneZzTransformTracker]   MusicData prop depth={depth}: {type.Name}.{prop.Name}, objId={musicData.objId}, uid={musicData.noteData?.uid}, scene={musicData.noteData?.scene}, prefab={musicData.noteData?.prefab_name}, configUid={musicData.configData?.note_uid}");
                    }
                    else if (ShouldInspectNested(value))
                    {
                        DumpObjectScalars(value, depth + 1, inspectedObjects);
                    }
                }
                catch { }
            }
        }

        private static bool IsInterestingScalar(object value)
        {
            if (value == null) return false;

            if (value is string text)
            {
                return text.Contains("071304") || text.Contains("121304") || text.Contains("71304");
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
            if (listObj == null) return 0;

            int restored = 0;
            int count = GetListCount(listObj);
            if (count < 0) return 0;

            var itemTypes = new HashSet<string>();
            for (int i = 0; i < count; i++)
            {
                object item = GetListItem(listObj, i);
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
            foreach (var field in type.GetFields(DefaultFlags))
            {
                try
                {
                    object value = field.GetValue(obj);
                    if (value is MusicData musicData)
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
                        restored += RestoreObjectMusicData(value, depth + 1, inspectedObjects);
                    }
                }
                catch { }
            }

            // 2. 프로퍼티 탐색 및 복구
            foreach (var prop in type.GetProperties(DefaultFlags))
            {
                try
                {
                    if (!prop.CanRead || prop.GetIndexParameters().Length != 0) continue;

                    object value = prop.GetValue(obj);
                    if (value is MusicData musicData)
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
                        restored += RestoreObjectMusicData(value, depth + 1, inspectedObjects);
                    }
                }
                catch { }
            }

            return restored;
        }

        private static bool TryRestoreScalarField(object obj, FieldInfo field, object value)
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

        private static bool TryRestoreScalarProperty(object obj, PropertyInfo prop, object value)
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
            if (original == null) return false;

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
            return typeName.StartsWith("Il2Cpp", StringComparison.Ordinal)
                && !typeName.StartsWith("Il2CppSystem.Collections", StringComparison.Ordinal)
                && !typeName.Contains("String");
        }

        private static object SafeGet(Func<object> getter)
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

        private static void CountZz(SortedDictionary<string, int> counts, string uid)
        {
            if (string.IsNullOrEmpty(uid) || uid.Length < 2) return;
            string zz = uid.Substring(0, 2);
            counts[zz] = counts.TryGetValue(zz, out int count) ? count + 1 : 1;
        }

        private static string FormatZzCounts(SortedDictionary<string, int> counts)
        {
            if (counts == null || counts.Count == 0) return "{}";

            var parts = new List<string>();
            foreach (var pair in counts)
            {
                parts.Add($"{pair.Key}:{pair.Value}");
            }
            return "{" + string.Join(", ", parts) + "}";
        }
    }
}
