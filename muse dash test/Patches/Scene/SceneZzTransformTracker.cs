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
    /// 책임별로 다음 partial 파일들로 분리되어 있습니다:
    ///   - SceneZzTransformTracker.ReflectionCache.cs : 리플렉션 스키마 탐색 및 캐시
    ///   - SceneZzTransformTracker.Dump.cs             : 런타임 진단 덤프
    ///   - SceneZzTransformTracker.Restore.cs          : 런타임 오브젝트 복구 로직
    /// </summary>
    internal static partial class SceneZzTransformTracker
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
        // prefab_name 부분일치 검색용 후보 목록. distinct prefab_name 기준으로만 채워(아래 SeenRenderPrefabNames)
        // 노트 수가 아니라 프리팹 종류 수로 크기가 제한됩니다. 리스트인 이유는 첫 매치 우선 순회를 유지하기 위함입니다.
        private static readonly List<OriginalIdentity> OriginalsWithRenderPrefabName = new List<OriginalIdentity>();
        // 위 리스트 중복 등록 방지용 O(1) 집합(노트마다 List.Contains O(N) 스캔하던 것을 대체).
        private static readonly HashSet<string> SeenRenderPrefabNames = new HashSet<string>(StringComparer.Ordinal);

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
            SeenRenderPrefabNames.Clear();
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
                // distinct prefab_name 당 한 번만 등록(O(1) HashSet). 같은 프리팹을 쓰는 노트가
                // 수천 개여도 후보 목록은 프리팹 종류 수만큼만 커지므로 Restore의 부분일치 루프가 가벼워집니다.
                if (SeenRenderPrefabNames.Add(renderPrefabName))
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
    }
}
