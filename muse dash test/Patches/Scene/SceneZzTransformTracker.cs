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

        // 최적화를 위한 렌더 값 기준 해시 사전 및 리스트 (O(1) 조회용)
        private static readonly Dictionary<string, OriginalIdentity> OriginalsByRenderUid = new Dictionary<string, OriginalIdentity>();
        private static readonly Dictionary<string, OriginalIdentity> OriginalsByRenderMirrorUid = new Dictionary<string, OriginalIdentity>();
        private static readonly Dictionary<string, OriginalIdentity> OriginalsByRenderConfigNoteUid = new Dictionary<string, OriginalIdentity>();
        private static readonly Dictionary<int, OriginalIdentity> OriginalsByRenderNoteUid = new Dictionary<int, OriginalIdentity>();
        private static readonly List<OriginalIdentity> OriginalsWithRenderPrefabName = new List<OriginalIdentity>();

        // 리플렉션 캐시
        private static readonly Dictionary<Type, FieldInfo[]> FieldsCache = new Dictionary<Type, FieldInfo[]>();
        private static readonly Dictionary<Type, PropertyInfo[]> PropertiesCache = new Dictionary<Type, PropertyInfo[]>();
        private static readonly Dictionary<Type, PropertyInfo> CountPropertyCache = new Dictionary<Type, PropertyInfo>();
        private static readonly Dictionary<Type, PropertyInfo> ItemPropertyCache = new Dictionary<Type, PropertyInfo>();

        private static FieldInfo[] GetFieldsCached(Type type)
        {
            if (!FieldsCache.TryGetValue(type, out var fields))
            {
                fields = type.GetFields(DefaultFlags);
                FieldsCache[type] = fields;
            }
            return fields;
        }

        private static PropertyInfo[] GetPropertiesCached(Type type)
        {
            if (!PropertiesCache.TryGetValue(type, out var props))
            {
                var list = new List<PropertyInfo>();
                foreach (var prop in type.GetProperties(DefaultFlags))
                {
                    if (prop.CanRead && prop.GetIndexParameters().Length == 0)
                    {
                        list.Add(prop);
                    }
                }
                props = list.ToArray();
                PropertiesCache[type] = props;
            }
            return props;
        }

        private static PropertyInfo GetCountProperty(Type type)
        {
            if (!CountPropertyCache.TryGetValue(type, out var prop))
            {
                prop = type.GetProperty("Count");
                CountPropertyCache[type] = prop;
            }
            return prop;
        }

        private static PropertyInfo GetItemProperty(Type type)
        {
            if (!ItemPropertyCache.TryGetValue(type, out var prop))
            {
                prop = type.GetProperty("Item");
                ItemPropertyCache[type] = prop;
            }
            return prop;
        }

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
            OriginalsByRenderUid.Clear();
            OriginalsByRenderMirrorUid.Clear();
            OriginalsByRenderConfigNoteUid.Clear();
            OriginalsByRenderNoteUid.Clear();
            OriginalsWithRenderPrefabName.Clear();
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
                catch (Exception)
                {
                    // IL2CPP 리스트 요소 획득 또는 데이터 접근 시 예외 무시 (네이티브 객체 해제 등 대비)
                }
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
            if (int.TryParse(renderUid, out int parsed))
            {
                renderNoteUid = parsed;
            }

            OriginalIdentity captured;
            if (BmsOriginalsByObjId.TryGetValue(note.objId, out var bmsOriginal))
            {
                bmsOriginal.RenderUid = renderUid;
                bmsOriginal.RenderMirrorUid = renderMirrorUid;
                bmsOriginal.RenderNoteUid = renderNoteUid;
                bmsOriginal.RenderConfigNoteUid = renderConfigNoteUid;
                bmsOriginal.RenderPrefabName = renderPrefabName;
                OriginalsByObjId[note.objId] = bmsOriginal;
                captured = bmsOriginal;
            }
            else
            {
                captured = CaptureIdentity(note);
                captured.RenderUid = renderUid;
                captured.RenderMirrorUid = renderMirrorUid;
                captured.RenderNoteUid = renderNoteUid;
                captured.RenderConfigNoteUid = renderConfigNoteUid;
                captured.RenderPrefabName = renderPrefabName;
                OriginalsByObjId[note.objId] = captured;
            }

            // O(1) 조회를 위한 인덱스 등록
            if (!string.IsNullOrEmpty(renderUid))
                OriginalsByRenderUid[renderUid] = captured;
            if (!string.IsNullOrEmpty(renderMirrorUid))
                OriginalsByRenderMirrorUid[renderMirrorUid] = captured;
            if (!string.IsNullOrEmpty(renderConfigNoteUid))
                OriginalsByRenderConfigNoteUid[renderConfigNoteUid] = captured;
            OriginalsByRenderNoteUid[renderNoteUid] = captured;
            if (!string.IsNullOrEmpty(renderPrefabName))
            {
                if (!OriginalsWithRenderPrefabName.Contains(captured))
                {
                    OriginalsWithRenderPrefabName.Add(captured);
                }
            }
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

                    try 
                    { 
                        note.noteData = noteData; 
                    } 
                    catch (Exception) 
                    { 
                        // Ignored: IL2CPP 내부 C++ 객체 메모리 해제 등 대비
                    }

                    try 
                    { 
                        list[i] = note; 
                    } 
                    catch (Exception) 
                    { 
                        // Ignored: 리스트 요소 변경 예외 무시
                    }

                    restored++;
                }
                catch (Exception)
                {
                    // Ignored: 개별 노트 정체 복구 실패 시 무시
                }
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

            var sb = new System.Text.StringBuilder();
            sb.AppendLine("[SceneZzTransformTracker] Starting non-07 runtime object diagnostics dump...");

            // 각 런타임 리스트를 단 한 번만 스캔해 objId→객체 인덱스를 만든다.
            var indexes = new (string Label, Dictionary<short, RuntimeHit> Map)[]
            {
                ("objCtrls", BuildObjIdIndex(SafeGet(() => scene.objCtrls))),
                ("preloads", BuildObjIdIndex(SafeGet(() => scene.preloads))),
                ("preloads1", BuildObjIdIndex(SafeGet(() => scene.preloads1))),
            };

            int trackedCount = 0;
            foreach (var pair in OriginalsByObjId)
            {
                var original = pair.Value;
                if (original == null || string.IsNullOrEmpty(original.Uid) || original.Uid.StartsWith("07"))
                {
                    continue;
                }

                trackedCount++;
                sb.AppendLine($"[SceneZzTransformTracker] non-07 original tracked: objId={pair.Key}, originalUid={original.Uid}, renderUid={original.RenderUid}, scene={original.Scene}, prefab={original.PrefabName}");

                foreach (var index in indexes)
                {
                    if (index.Map.TryGetValue(pair.Key, out var hit))
                    {
                        sb.AppendLine($"[SceneZzTransformTracker] runtime object dump: list={index.Label}, index={hit.Index}, type={hit.Item.GetType().FullName}");
                        DumpObjectScalars(hit.Item, 0, new HashSet<int>(), sb);
                    }
                }
            }

            sb.AppendLine($"[SceneZzTransformTracker] Diagnostics dump completed. Total non-07 tracked notes analyzed: {trackedCount}");
            MelonLogger.Msg(sb.ToString());
        }

        /// <summary>
        /// 런타임 리스트를 한 번 순회하며 각 객체가 품고 있는 MusicData의 objId를 추출해 인덱스를 구축합니다.
        /// 동일 objId가 여러 번 나오면 기존 동작(첫 매칭만 덤프)과 맞추기 위해 첫 항목만 보존합니다.
        /// </summary>
        private static Dictionary<short, RuntimeHit> BuildObjIdIndex(object listObj)
        {
            var map = new Dictionary<short, RuntimeHit>();
            if (listObj == null) return map;

            var listType = listObj.GetType();
            var countProp = GetCountProperty(listType);
            if (countProp == null) return map;

            int count = (int)countProp.GetValue(listObj);
            var itemProp = GetItemProperty(listType);
            if (itemProp == null) return map;

            var indexArgs = new object[1];
            for (int i = 0; i < count; i++)
            {
                indexArgs[0] = i;
                object item = itemProp.GetValue(listObj, indexArgs);
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
            foreach (var field in GetFieldsCached(type))
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
                catch (Exception)
                {
                    // Ignored: 필드 값 획득 실패 시 건너뜀
                }
            }

            // 3. 클래스 내부의 프로퍼티들을 리플렉션으로 검사
            foreach (var prop in GetPropertiesCached(type))
            {
                try
                {
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
                catch (Exception)
                {
                    // Ignored: 프로퍼티 값 획득 실패 시 건너뜀
                }
            }

            return false;
        }

        /// <summary>
        /// [재귀 탐색] 특정 객체 내부의 문자열이나 숫자 필드를 로깅하여 값을 분석합니다.
        /// </summary>
        private static void DumpObjectScalars(object obj, int depth, HashSet<int> inspectedObjects, System.Text.StringBuilder sb)
        {
            if (obj == null || depth > 1) return;

            int identity = System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(obj);
            if (!inspectedObjects.Add(identity)) return;

            var type = obj.GetType();

            foreach (var field in GetFieldsCached(type))
            {
                try
                {
                    object value = field.GetValue(obj);
                    if (IsInterestingScalar(value))
                    {
                        sb.AppendLine($"[SceneZzTransformTracker]   scalar field depth={depth}: {type.Name}.{field.Name}={value}");
                    }
                    else if (value is MusicData musicData)
                    {
                        sb.AppendLine($"[SceneZzTransformTracker]   MusicData field depth={depth}: {type.Name}.{field.Name}, objId={musicData.objId}, uid={musicData.noteData?.uid}, scene={musicData.noteData?.scene}, prefab={musicData.noteData?.prefab_name}, configUid={musicData.configData?.note_uid}");
                    }
                    else if (ShouldInspectNested(value))
                    {
                        DumpObjectScalars(value, depth + 1, inspectedObjects, sb);
                    }
                }
                catch (Exception)
                {
                    // Ignored: 진단 필드 조회 실패 시 건너뜀
                }
            }

            foreach (var prop in GetPropertiesCached(type))
            {
                try
                {
                    object value = prop.GetValue(obj);
                    if (IsInterestingScalar(value))
                    {
                        sb.AppendLine($"[SceneZzTransformTracker]   scalar prop depth={depth}: {type.Name}.{prop.Name}={value}");
                    }
                    else if (value is MusicData musicData)
                    {
                        sb.AppendLine($"[SceneZzTransformTracker]   MusicData prop depth={depth}: {type.Name}.{prop.Name}, objId={musicData.objId}, uid={musicData.noteData?.uid}, scene={musicData.noteData?.scene}, prefab={musicData.noteData?.prefab_name}, configUid={musicData.configData?.note_uid}");
                    }
                    else if (ShouldInspectNested(value))
                    {
                        DumpObjectScalars(value, depth + 1, inspectedObjects, sb);
                    }
                }
                catch (Exception)
                {
                    // Ignored: 진단 프로퍼티 조회 실패 시 건너뜀
                }
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
                            try { field.SetValue(obj, musicData); } catch (Exception) { }
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
                        if (RestoreMusicData(ref musicData))
                        {
                            if (prop.CanWrite)
                            {
                                try { prop.SetValue(obj, musicData); } catch (Exception) { }
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

        private static int GetListCount(object listObj)
        {
            try
            {
                var prop = listObj.GetType().GetProperty("Count");
                if (prop == null) return -1;
                object value = prop.GetValue(listObj);
                return value is int count ? count : -1;
            }
            catch (Exception) { return -1; }
        }

        private static object GetListItem(object listObj, int index)
        {
            try
            {
                var prop = listObj.GetType().GetProperty("Item");
                return prop != null ? prop.GetValue(listObj, new object[] { index }) : null;
            }
            catch (Exception) { return null; }
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
