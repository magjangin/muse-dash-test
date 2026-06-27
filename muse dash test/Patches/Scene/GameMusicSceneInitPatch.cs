using MelonLoader;
using System;

// Il2CppGameLogic.GameMusicScene.InitTimer(Decimal) / InitSceneEvents() 후킹.
// InitSceneEvents는 관찰 로깅용이지만, InitTimer Prefix는 단순 관찰이 아니라
// 노트 UID의 "렌더 씬(zz)"을 실제로 다시 쓰는 변형 작업(TransformSceneSegments)을 수행한다.
//
// UID 구조 규약(자세한 내용은 docs/BMS_PARSING.md): 6자리 숫자 UID = zzxxyy
//   zz = 렌더 씬 번호(배경/스테이지 계열), xx = 노트 타입, yy = 슬롯/세부값
//   접두사 "0004…" = 씬 전환 토글 노트(xx=04지만 0004가 우선 분류), 끝 2자리 yy = 전환할 씬 번호
//   zz == "00" = 특정 씬에 묶이지 않는 씬 무관 노트(렌더 씬 변형 대상에서 제외)
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
            try { frame = UnityEngine.Time.frameCount; } catch (Exception) { }
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
        muse_dash_test.SceneZzTransformTracker.Clear();
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

                // fromZz: 이 노트가 원래 갖고 있던 렌더 씬. "00"(씬 무관)이거나 이미 현재
                // 활성 렌더 씬과 같으면 다시 쓸 필요가 없으므로 건너뛴다.
                string fromZz = uid.Substring(0, 2);
                if (fromZz == "00" || fromZz == activeRenderZz) continue;

                string renderZz = activeRenderZz;
                if (muse_dash_test.SceneZzTransformTracker.TryGetBmsOriginalUid(note.objId, out string bmsOriginalUid)
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
                catch (Exception) { }

                muse_dash_test.SceneZzTransformTracker.Record(note, newUid, renderPrefabName);
                CountZz(changedByOriginalZz, fromZz);
                nd.uid = newUid;
                // 아래 필드 쓰기들을 한 줄씩 개별 try/catch로 격리하는 이유:
                // il2cpp 바인딩에서 일부 필드 접근만 던질 수 있어, 한 필드 실패가
                // 나머지 필드 갱신까지 막지 않도록 각각 독립적으로 적용한다.
                try { if (IsSixDigitUid(nd.mirror_uid) && nd.mirror_uid.StartsWith(fromZz)) nd.mirror_uid = renderZz + nd.mirror_uid.Substring(2); } catch (Exception) { }
                try { nd.scene = "scene_" + renderZz; } catch (Exception) { }
                try { nd.prefab_name = renderPrefabName; } catch (Exception) { }
                try { if (int.TryParse(newUid, out int parsedNoteUid)) nd.noteUid = parsedNoteUid; } catch (Exception) { }
                try { if (note.configData != null && IsSixDigitUid(note.configData.note_uid) && note.configData.note_uid.StartsWith(fromZz)) note.configData.note_uid = renderZz + note.configData.note_uid.Substring(2); } catch (Exception) { }
                try { note.noteData = nd; } catch (Exception) { }
                try { list[i] = note; } catch (Exception) { }
                changed++;
            }
            catch (Exception) { }
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

        // manifest에 씬 번호가 지정되지 않은 커스텀 곡의 기본 렌더 씬.
        // 07은 대부분의 곡에 존재하는 표준 배경 씬이라 안전한 기본값으로 사용한다.
        MelonLogger.Msg($"[GameMusicScene.InitTimer] manifest scene 없음, 기본 렌더 씬 07 사용: uid={uid ?? "(null)"}");
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

[HarmonyLib.HarmonyPatch(typeof(Il2CppGameLogic.GameMusicScene), "InitSceneEvents")]
public class GameMusicScene_InitSceneEvents_Patch
{
    public static void Prefix(Il2CppGameLogic.GameMusicScene __instance)
    {
        try
        {
            if (!muse_dash_test.CustomPlaySession.Current.ShouldApplyExperimentChart) return;

            int frame = 0;
            try { frame = UnityEngine.Time.frameCount; } catch (Exception) { }
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
            try { sceneCount = __instance != null && __instance.scenes != null ? __instance.scenes.Count : -1; } catch (Exception) { }
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
