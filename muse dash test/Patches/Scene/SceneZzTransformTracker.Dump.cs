using MelonLoader;
using System;
using System.Collections.Generic;
using Il2CppGameLogic;

namespace muse_dash_test
{
    /// <summary>
    /// non-07 런타임 오브젝트 진단 덤프 (프레임 분산 코루틴 기반).
    /// </summary>
    internal static partial class SceneZzTransformTracker
    {
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

        // 진단 덤프 시 한 프레임에서 처리할 작업량 상한. 이만큼 처리하면 다음 프레임으로 양보(yield)한다.
        private const int DumpListScanPerFrame = 200; // 리스트 인덱싱: 항목(인터롭 읽기) 수 기준
        private const int DumpNotesPerFrame = 40;      // 노트 덤프: 노트당 인터롭 읽기가 더 무거우므로 더 작게

        // 현재 진행 중인 진단 덤프 코루틴 토큰. 씬 재진입 시 이전 덤프를 중단하는 데 사용한다.
        private static object _dumpCoroutine;

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

            // 진단 덤프는 대량의 IL2CPP 인터롭 읽기 + 대량 로그 출력을 동반하므로, 곡 시작 한 프레임에
            // 몰리면 히치(렉)를 유발한다. 코루틴으로 여러 프레임에 나눠 실행해 부하를 분산한다.
            // (씬 재진입 시 이전 덤프가 아직 돌고 있으면 중단하고 새로 시작)
            if (_dumpCoroutine != null)
            {
                MelonCoroutines.Stop(_dumpCoroutine);
                _dumpCoroutine = null;
            }
            _dumpCoroutine = MelonCoroutines.Start(DumpNonDefaultOriginalRuntimeObjectsRoutine(scene));
            return restored;
        }

        /// <summary>
        /// non-07 런타임 오브젝트 진단 덤프를 여러 프레임에 나눠 수행하는 코루틴입니다.
        /// 출력 내용·순서는 기존 동기 버전과 동일하며, 작업량과 로그 플러시만 프레임 단위로 분산됩니다.
        /// </summary>
        private static System.Collections.IEnumerator DumpNonDefaultOriginalRuntimeObjectsRoutine(GameMusicScene scene)
        {
            if (scene == null) { _dumpCoroutine = null; yield break; }

            var sb = new System.Text.StringBuilder();
            sb.AppendLine("[SceneZzTransformTracker] Starting non-07 runtime object diagnostics dump...");

            // 1) 각 런타임 리스트를 단 한 번만 스캔해 objId→객체 인덱스를 만든다. (프레임 분산)
            var listObjs = new (string Label, object List)[]
            {
                ("objCtrls", SafeGet(() => scene.objCtrls)),
                ("preloads", SafeGet(() => scene.preloads)),
                ("preloads1", SafeGet(() => scene.preloads1)),
            };
            var indexes = new (string Label, Dictionary<short, RuntimeHit> Map)[listObjs.Length];

            int scanned = 0;
            for (int li = 0; li < listObjs.Length; li++)
            {
                var map = new Dictionary<short, RuntimeHit>();
                object listObj = listObjs[li].List;
                if (listObj != null)
                {
                    var listType = listObj.GetType();
                    var countProp = GetCountProperty(listType);
                    var itemProp = GetItemProperty(listType);
                    if (countProp != null && itemProp != null)
                    {
                        int count = (int)countProp.GetValue(listObj);
                        var indexArgs = new object[1];
                        for (int i = 0; i < count; i++)
                        {
                            indexArgs[0] = i;
                            object item = itemProp.GetValue(listObj, indexArgs);
                            if (item != null && TryGetObjIdOptimized(item, out short objId) && !map.ContainsKey(objId))
                            {
                                map[objId] = new RuntimeHit(i, item);
                            }

                            if (++scanned >= DumpListScanPerFrame)
                            {
                                scanned = 0;
                                FlushDumpBuffer(sb);
                                yield return null;
                            }
                        }
                    }
                }
                indexes[li] = (listObjs[li].Label, map);
            }

            // 2) non-07 추적 노트를 스냅샷으로 떠서 프레임에 나눠 덤프한다.
            //    (덤프 도중 OriginalsByObjId가 갱신돼도 안전하도록 미리 복사한다.)
            var tracked = new List<KeyValuePair<short, OriginalIdentity>>();
            foreach (var pair in OriginalsByObjId)
            {
                var original = pair.Value;
                if (original == null || string.IsNullOrEmpty(original.Uid) || original.Uid.StartsWith("07"))
                {
                    continue;
                }
                tracked.Add(pair);
            }

            int trackedCount = 0;
            int dumpedNotes = 0;
            foreach (var pair in tracked)
            {
                var original = pair.Value;
                trackedCount++;
                sb.AppendLine($"[SceneZzTransformTracker] non-07 original tracked: objId={pair.Key}, originalUid={original.Uid}, renderUid={original.RenderUid}, scene={original.Scene}, prefab={original.PrefabName}");

                foreach (var index in indexes)
                {
                    if (index.Map.TryGetValue(pair.Key, out var hit))
                    {
                        sb.AppendLine($"[SceneZzTransformTracker] runtime object dump: list={index.Label}, index={hit.Index}, type={hit.Item.GetType().FullName}");
                        DumpObjectScalarsOptimized(hit.Item, sb);
                    }
                }

                if (++dumpedNotes >= DumpNotesPerFrame)
                {
                    dumpedNotes = 0;
                    FlushDumpBuffer(sb);
                    yield return null;
                }
            }

            sb.AppendLine($"[SceneZzTransformTracker] Diagnostics dump completed. Total non-07 tracked notes analyzed: {trackedCount}");
            FlushDumpBuffer(sb);
            _dumpCoroutine = null;
        }

        // 누적된 진단 로그 버퍼를 한 번에 출력하고 비운다. (프레임당 1회 플러시로 콘솔/파일 I/O를 분산)
        private static void FlushDumpBuffer(System.Text.StringBuilder sb)
        {
            if (sb.Length == 0) return;
            MelonLogger.Msg(sb.ToString().TrimEnd('\r', '\n'));
            sb.Clear();
        }

        private static bool TryGetObjIdOptimized(object obj, out short objId)
        {
            objId = 0;
            if (obj == null) return false;

            if (obj is MusicData musicData)
            {
                objId = musicData.objId;
                return true;
            }

            var type = obj.GetType();
            var schema = GetOrCreateDumpSchema(type);
            foreach (var path in schema.MusicDataPaths)
            {
                try
                {
                    var val = path.Evaluate(obj);
                    if (val is MusicData md)
                    {
                        objId = md.objId;
                        return true;
                    }
                }
                catch (Exception) { }
            }

            return false;
        }

        private static void DumpObjectScalarsOptimized(object obj, System.Text.StringBuilder sb)
        {
            if (obj == null) return;
            var type = obj.GetType();
            var schema = GetOrCreateDumpSchema(type);

            // 1. MusicData 프로퍼티들 덤프
            foreach (var path in schema.MusicDataPaths)
            {
                try
                {
                    var val = path.Evaluate(obj);
                    if (val is MusicData musicData)
                    {
                        var lastMember = path.Members[path.Members.Length - 1];
                        string declaringTypeName = lastMember.DeclaringType?.Name ?? type.Name;
                        sb.AppendLine($"[SceneZzTransformTracker]   MusicData prop depth={path.Members.Length}: {declaringTypeName}.{lastMember.Name}, objId={musicData.objId}, uid={musicData.noteData?.uid}, scene={musicData.noteData?.scene}, prefab={musicData.noteData?.prefab_name}, configUid={musicData.configData?.note_uid}");
                    }
                }
                catch (Exception) { }
            }

            // 2. 스칼라 프로퍼티들 덤프 (IsInterestingScalar에 부합할 경우에만)
            foreach (var path in schema.ScalarPaths)
            {
                try
                {
                    var val = path.Evaluate(obj);
                    if (IsInterestingScalar(val))
                    {
                        var lastMember = path.Members[path.Members.Length - 1];
                        string declaringTypeName = lastMember.DeclaringType?.Name ?? type.Name;
                        sb.AppendLine($"[SceneZzTransformTracker]   scalar prop depth={path.Members.Length - 1}: {declaringTypeName}.{lastMember.Name}={val}");
                    }
                }
                catch (Exception) { }
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
