using MelonLoader;
using System;
using System.Collections.Generic;

/// <summary>
/// Il2CppGameLogic.GameMusicScene.InitTimer / InitSceneEvents 패치 클래스입니다.
/// 인게임 스테이지 진입 시 노트 UID의 렌더 씬(zz) 계층을 커스텀 채보에 맞게 변형(TransformSceneSegments)합니다.
/// 
/// [UID 구조 규약] (자세한 내용은 docs/BMS_PARSING.md 참조)
/// 6자리 숫자 UID = zzxxyy
///   - zz: 렌더 씬 번호 (배경/스테이지 계열)
///   - xx: 노트 타입
///   - yy: 슬롯/세부값
///   - "0004…": 씬 전환 토글 노트 (끝 2자리 yy = 전환할 씬 번호)
///   - zz == "00": 특정 씬에 묶이지 않는 무관 노트 (변형 대상 제외)
/// </summary>
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
        catch (Exception ex) 
        { 
            MelonLogger.Error($"[GameMusicScene.InitTimer] Prefix 예외: {ex}"); 
        }
    }

    /// <summary>
    /// 로드된 스테이지의 전체 노트 리스트를 순회하며 렌더 씬(zz) 세그먼트를 변환합니다.
    /// </summary>
    private static int TransformSceneSegments(Il2CppSystem.Collections.Generic.List<Il2CppGameLogic.MusicData> list, string initialZz)
    {
        if (list == null || string.IsNullOrEmpty(initialZz)) return 0;

        int changedCount = 0;
        var changedByOriginalZz = new SortedDictionary<string, int>();
        string activeRenderZz = initialZz;
        
        muse_dash_test.SceneZzTransformTracker.Clear();

        for (int i = 0; i < list.Count; i++)
        {
            try
            {
                var note = list[i];
                if (note == null || note.noteData == null) continue;

                if (ProcessSingleNote(note, i, ref activeRenderZz, changedByOriginalZz))
                {
                    list[i] = note;
                    changedCount++;
                }
            }
            catch (Exception) { }
        }

        MelonLogger.Msg($"[GameMusicScene.InitTimer] 구간 렌더 zz 변형 분포: {FormatZzCounts(changedByOriginalZz)}");
        return changedCount;
    }

    /// <summary>
    /// 개별 노트의 UID 및 렌더 씬 정보를 검출하고 필요 시 렌더 씬(zz) 필드를 갱신합니다.
    /// </summary>
    private static bool ProcessSingleNote(
        Il2CppGameLogic.MusicData note, 
        int index, 
        ref string activeRenderZz, 
        SortedDictionary<string, int> changedByOriginalZz)
    {
        var nd = note.noteData;
        string uid = nd.uid;

        if (!IsSixDigitUid(uid)) return false;

        // 씬 전환 토글 노트(0004xx) 처리
        if (IsSceneToggleUid(uid))
        {
            string nextRenderZz = uid.Substring(4, 2);
            MelonLogger.Msg($"[GameMusicScene.InitTimer] 씬 전환 구간 관찰: index={index}, uid={uid}, sceneInfo={nextRenderZz}, activeRenderZz={activeRenderZz}");
            return false;
        }

        string fromZz = uid.Substring(0, 2);
        if (fromZz == "00" || fromZz == activeRenderZz) return false;

        string renderZz = activeRenderZz;
        if (muse_dash_test.SceneZzTransformTracker.TryGetBmsOriginalUid(note.objId, out string bmsOriginalUid)
            && IsSixDigitUid(bmsOriginalUid))
        {
            renderZz = bmsOriginalUid.Substring(0, 2);
        }

        if (fromZz == renderZz) return false;

        // 변환될 새로운 UID 및 프리맵 이름 생성
        string newUid = renderZz + uid.Substring(2);
        string renderPrefabName = nd.prefab_name;
        if (!string.IsNullOrEmpty(nd.prefab_name) && nd.prefab_name.StartsWith(fromZz))
        {
            renderPrefabName = renderZz + nd.prefab_name.Substring(2);
        }

        // 추적 기록 및 카운트
        muse_dash_test.SceneZzTransformTracker.Record(note, newUid, renderPrefabName);
        CountZz(changedByOriginalZz, fromZz);

        // il2cpp 필드 갱신 (독립 예외 격리)
        ApplyTransformedFields(note, fromZz, renderZz, newUid, renderPrefabName);
        return true;
    }

    /// <summary>
    /// il2cpp 바인딩 객체의 각 필드 쓰기를 개별 try-catch로 격리하여 안전하게 대입합니다.
    /// </summary>
    private static void ApplyTransformedFields(
        Il2CppGameLogic.MusicData note,
        string fromZz,
        string renderZz,
        string newUid,
        string renderPrefabName)
    {
        var nd = note.noteData;
        nd.uid = newUid;

        try { if (IsSixDigitUid(nd.mirror_uid) && nd.mirror_uid.StartsWith(fromZz)) nd.mirror_uid = renderZz + nd.mirror_uid.Substring(2); } catch (Exception) { }
        try { nd.scene = "scene_" + renderZz; } catch (Exception) { }
        try { nd.prefab_name = renderPrefabName; } catch (Exception) { }
        try { if (int.TryParse(newUid, out int parsedNoteUid)) nd.noteUid = parsedNoteUid; } catch (Exception) { }
        try { if (note.configData != null && IsSixDigitUid(note.configData.note_uid) && note.configData.note_uid.StartsWith(fromZz)) note.configData.note_uid = renderZz + note.configData.note_uid.Substring(2); } catch (Exception) { }
        try { note.noteData = nd; } catch (Exception) { }
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

    private static void CountZz(SortedDictionary<string, int> counts, string zz)
    {
        if (string.IsNullOrEmpty(zz)) return;
        counts[zz] = counts.TryGetValue(zz, out int count) ? count + 1 : 1;
    }

    private static string FormatZzCounts(SortedDictionary<string, int> counts)
    {
        if (counts == null || counts.Count == 0)
        {
            return "{}";
        }

        var parts = new List<string>();
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
