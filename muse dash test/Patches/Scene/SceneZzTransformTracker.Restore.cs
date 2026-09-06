using MelonLoader;
using System;
using System.Collections.Generic;
using Il2CppGameLogic;
using Il2CppInterop.Runtime.InteropTypes.Arrays;

namespace muse_dash_test
{
    /// <summary>
    /// 런타임 오브젝트 트리를 순회하며 보관된 원본 MusicData/식별 값을 복구하는 로직.
    /// </summary>
    internal static partial class SceneZzTransformTracker
    {
        /// <summary>
        /// 씬에 생성된 실제 런타임 컨트롤러 객체들을 탐색하여 원래의 원본 값(uid, scene 등)으로 되돌려놓습니다.
        ///
        /// <para><b>크래시 방지 핵심 (CHECKLIST.md):</b></para>
        /// <list type="bullet">
        /// <item><description>게임의 <c>scene.objCtrls</c>, <c>scene.preloads</c>, <c>scene.preloads1</c>은
        /// <c>List&lt;T&gt;</c>가 아니라 <b><c>Il2CppReferenceArray&lt;T&gt;</c></b>입니다.
        /// 예전에는 <c>is List&lt;...&gt;</c>만 검사해 모든 런타임 객체가 리플렉션 폴백으로 떨어졌습니다.</description></item>
        /// <item><description>리플렉션으로 IL2CPP 속성(<c>prop.GetValue()</c>)을 재귀 탐색하면
        /// 500노트 이상 대형 곡에서 193번째 노트 전후로 프로세스가 로그 없이 즉시 증발합니다.
        /// 따라서 심층 재귀 리플렉션을 완전히 배제하고, <c>BaseEnemyObjectController.m_MusicData</c> 및
        /// <c>BaseSpineObjectController.m_Sac.m_MusicData</c>로 직접 타입 캐스팅해 즉시 복원합니다.</description></item>
        /// <item><description>객체 중복 방문 방지는 매니지드 C# 래퍼 해시코드가 아닌
        /// 네이티브 포인터(<c>Il2CppObjectBase.Pointer</c>)를 기준으로 합니다.</description></item>
        /// </list>
        /// </summary>
        public static int RestoreRuntimeObjects(GameMusicScene scene)
        {
            if (scene == null || OriginalsByObjId.Count == 0) return 0;

            int restored = 0;
            restored += RestoreObjectList("objCtrls", SafeGet(() => scene.objCtrls));
            restored += RestoreObjectList("preloads", SafeGet(() => scene.preloads));
            restored += RestoreObjectList("preloads1", SafeGet(() => scene.preloads1));
            return restored;
        }

        private static int RestoreObjectList(string label, object listObj)
        {
            if (listObj == null) return 0;

            int restored = 0;
            var inspectedPointers = new HashSet<IntPtr>();

            // 1. objCtrls 초고속 다이렉트 패스 (Il2CppReferenceArray<BaseSpineObjectController> 또는 List<BaseSpineObjectController>)
            if (listObj is Il2CppReferenceArray<Il2Cpp.BaseSpineObjectController> spineArray)
            {
                int count = spineArray.Length;
                for (int i = 0; i < count; i++)
                {
                    var item = spineArray[i];
                    if (item == null) continue;
                    restored += RestoreBaseSpineController(item, inspectedPointers);
                }

                if (SceneDiagnosticLogger.ShouldLog($"SceneZzTransformTracker.RestoreList.{label}", 20))
                {
                    ModLogger.Msg($"[SceneZzTransformTracker] runtime list scan: {label}, count={count}, restored={restored}, itemTypes=[Il2CppReferenceArray<BaseSpineObjectController>]");
                }
                return restored;
            }
            if (listObj is Il2CppSystem.Collections.Generic.List<Il2Cpp.BaseSpineObjectController> spineList)
            {
                int count = spineList.Count;
                for (int i = 0; i < count; i++)
                {
                    var item = spineList[i];
                    if (item == null) continue;
                    restored += RestoreBaseSpineController(item, inspectedPointers);
                }

                if (SceneDiagnosticLogger.ShouldLog($"SceneZzTransformTracker.RestoreList.{label}", 20))
                {
                    ModLogger.Msg($"[SceneZzTransformTracker] runtime list scan: {label}, count={count}, restored={restored}, itemTypes=[Il2Cpp.BaseSpineObjectController]");
                }
                return restored;
            }

            // 2. preloads 초고속 다이렉트 패스 (Il2CppReferenceArray<GameObject> 또는 List<GameObject>)
            if (listObj is Il2CppReferenceArray<UnityEngine.GameObject> goArray)
            {
                int count = goArray.Length;
                for (int i = 0; i < count; i++)
                {
                    var go = goArray[i];
                    if (go == null) continue;
                    restored += RestoreGameObject(go, inspectedPointers);
                }

                if (SceneDiagnosticLogger.ShouldLog($"SceneZzTransformTracker.RestoreList.{label}", 20))
                {
                    ModLogger.Msg($"[SceneZzTransformTracker] runtime list scan: {label}, count={count}, restored={restored}, itemTypes=[Il2CppReferenceArray<GameObject>]");
                }
                return restored;
            }
            if (listObj is Il2CppSystem.Collections.Generic.List<UnityEngine.GameObject> goList)
            {
                int count = goList.Count;
                for (int i = 0; i < count; i++)
                {
                    var go = goList[i];
                    if (go == null) continue;
                    restored += RestoreGameObject(go, inspectedPointers);
                }

                if (SceneDiagnosticLogger.ShouldLog($"SceneZzTransformTracker.RestoreList.{label}", 20))
                {
                    ModLogger.Msg($"[SceneZzTransformTracker] runtime list scan: {label}, count={count}, restored={restored}, itemTypes=[UnityEngine.GameObject]");
                }
                return restored;
            }

            // 3. preloads1 초고속 다이렉트 패스 (Il2CppReferenceArray<List<GameObject>> 또는 List<List<GameObject>>)
            if (listObj is Il2CppReferenceArray<Il2CppSystem.Collections.Generic.List<UnityEngine.GameObject>> nestedGoArray)
            {
                int count = nestedGoArray.Length;
                for (int i = 0; i < count; i++)
                {
                    var subList = nestedGoArray[i];
                    if (subList == null) continue;
                    int subCount = subList.Count;
                    for (int j = 0; j < subCount; j++)
                    {
                        var go = subList[j];
                        if (go == null) continue;
                        restored += RestoreGameObject(go, inspectedPointers);
                    }
                }

                if (SceneDiagnosticLogger.ShouldLog($"SceneZzTransformTracker.RestoreList.{label}", 20))
                {
                    ModLogger.Msg($"[SceneZzTransformTracker] runtime list scan: {label}, count={count}, restored={restored}, itemTypes=[Il2CppReferenceArray<List<GameObject>>]");
                }
                return restored;
            }
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
                        restored += RestoreGameObject(go, inspectedPointers);
                    }
                }

                if (SceneDiagnosticLogger.ShouldLog($"SceneZzTransformTracker.RestoreList.{label}", 20))
                {
                    ModLogger.Msg($"[SceneZzTransformTracker] runtime list scan: {label}, count={count}, restored={restored}, itemTypes=[Il2CppSystem.Collections.Generic.List`1[[UnityEngine.GameObject]]]");
                }
                return restored;
            }

            // 4. 일반 폴백 (리플렉션 재귀 탐색 대신 알려진 컨트롤러/게임오브젝트 타입으로 직접 디스패치)
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

                if (item is Il2Cpp.BaseSpineObjectController bsoc)
                {
                    restored += RestoreBaseSpineController(bsoc, inspectedPointers);
                }
                else if (item is UnityEngine.GameObject go)
                {
                    restored += RestoreGameObject(go, inspectedPointers);
                }
                else if (item is Il2Cpp.SpineActionController sac)
                {
                    restored += RestoreSpineActionController(sac, inspectedPointers);
                }
                else if (item is MusicData directMd)
                {
                    if (RestoreMusicData(ref directMd))
                    {
                        try { itemProp.SetValue(listObj, directMd, indexArgs); restored++; } catch (Exception) { }
                    }
                }
            }

            if (SceneDiagnosticLogger.ShouldLog($"SceneZzTransformTracker.RestoreList.{label}", 20))
            {
                var preview = itemTypes.Count == 0 ? "(none)" : string.Join(", ", itemTypes);
                ModLogger.Msg($"[SceneZzTransformTracker] runtime list scan: {label}, count={fallbackCount}, restored={restored}, itemTypes=[{preview}]");
            }
            return restored;
        }

        private static int RestoreBaseSpineController(Il2Cpp.BaseSpineObjectController bsoc, HashSet<IntPtr> inspectedPointers)
        {
            if (bsoc == null) return 0;
            IntPtr ptr = bsoc.Pointer;
            if (ptr == IntPtr.Zero || !inspectedPointers.Add(ptr)) return 0;

            int restored = 0;
            try
            {
                // 1. BaseEnemyObjectController의 m_MusicData 직접 복구
                if (bsoc is Il2Cpp.BaseEnemyObjectController beoc)
                {
                    try
                    {
                        var md = beoc.m_MusicData;
                        if (RestoreMusicData(ref md))
                        {
                            beoc.m_MusicData = md;
                            restored++;
                        }
                    }
                    catch (Exception) { }
                }

                // 2. BaseSpineObjectController.m_Sac (SpineActionController)의 m_MusicData 직접 복구
                try
                {
                    var sac = bsoc.m_Sac;
                    if (sac != null)
                    {
                        restored += RestoreSpineActionController(sac, inspectedPointers);
                    }
                }
                catch (Exception) { }
            }
            catch (Exception) { }

            return restored;
        }

        private static int RestoreSpineActionController(Il2Cpp.SpineActionController sac, HashSet<IntPtr> inspectedPointers)
        {
            if (sac == null) return 0;
            IntPtr ptr = sac.Pointer;
            if (ptr == IntPtr.Zero || !inspectedPointers.Add(ptr)) return 0;

            int restored = 0;
            try
            {
                var md = sac.m_MusicData;
                if (RestoreMusicData(ref md))
                {
                    sac.m_MusicData = md;
                    restored++;
                }
            }
            catch (Exception) { }

            return restored;
        }

        private static int RestoreGameObject(UnityEngine.GameObject go, HashSet<IntPtr> inspectedPointers)
        {
            if (go == null) return 0;
            IntPtr ptr = go.Pointer;
            if (ptr == IntPtr.Zero || !inspectedPointers.Add(ptr)) return 0;

            int restored = 0;
            try
            {
                var beoc = go.GetComponent<Il2Cpp.BaseEnemyObjectController>();
                if (beoc != null)
                {
                    restored += RestoreBaseSpineController(beoc, inspectedPointers);
                }

                var bsoc = go.GetComponent<Il2Cpp.BaseSpineObjectController>();
                if (bsoc != null && (beoc == null || bsoc.Pointer != beoc.Pointer))
                {
                    restored += RestoreBaseSpineController(bsoc, inspectedPointers);
                }

                var sac = go.GetComponent<Il2Cpp.SpineActionController>();
                if (sac != null)
                {
                    restored += RestoreSpineActionController(sac, inspectedPointers);
                }
            }
            catch (Exception) { }

            return restored;
        }

        private static bool RestoreMusicData(ref MusicData note)
        {
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

        private static object SafeGet(Func<object> getter)
        {
            try { return getter(); }
            catch (Exception) { return null; }
        }
    }
}
