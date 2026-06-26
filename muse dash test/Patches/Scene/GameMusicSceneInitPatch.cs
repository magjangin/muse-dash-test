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
