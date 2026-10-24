using MelonLoader;
using muse_dash_test;
using System;
using System.Collections.Generic;

/// <summary>
/// 커스텀 차트 플레이 시 씬 전환(`m_IsSceneChange`)이 활성화되더라도 게임 엔진(`StageBattleComponent.LoadMusicData` 및
/// `GameMusicScene.PreLoad`)이 노트의 `uid`/`prefab_name`/`configData.note_uid`를 씬 번호(`zz`)로 덮어쓰지 못하도록
/// `IsIgnoreCurrentNoteChecking`에서 차단하고, `InitTimer`의 총 시간(`total`)을 커스텀 채보 길이에 맞게 확장합니다.
///
/// [결합도 분리 (Decoupled Design)]
/// 기존에는 `StageBattleComponent.LoadMusicData()`가 씬 전환 구간 이후 노트들의 `noteData`를 인플레이스로 덮어썼기 때문에
/// `InitTimer`에서 `TransformSceneSegments`로 변형하고 `PreLoadEnemy` 직후 `RestoreIdentities`/`RestoreRuntimeObjects`로
/// 복구하는 2단 우회 구조를 썼습니다.
/// Cpp2IL/ISIL 역공학 결과, `StageBattleComponent.IsIgnoreCurrentNoteChecking(string)`과
/// `GameMusicScene.IsIgnoreCurrentNoteChecking(int, string)`이 `true`를 반환하면:
///   1) `StageBattleComponent.LoadMusicData()`가 배경 씬 목록(`curSceneChangeType`/`m_Scenes`)과 `isSceneChangeType`은 그대로 등록하되,
///      개별 노트의 `noteData`(`uid`, `prefab_name` 등) 및 `configData.note_uid` 덮어쓰기와 `configData.scenes` 생성을 건너뜁니다.
///   2) `GameMusicScene.PreLoad`가 각 노트를 BMS 원본 `prefab_name` 그대로 단일 프리팹으로 인스턴스화합니다.
/// 따라서 `musicList`를 인플레이스로 변형·복구할 필요가 완전히 사라집니다.
/// </summary>
[HarmonyLib.HarmonyPatch(typeof(Il2CppFormulaBase.StageBattleComponent), "IsIgnoreCurrentNoteChecking",
    new Type[] { typeof(string) })]
public class StageBattleComponent_IsIgnoreCurrentNoteChecking_Patch
{
    public static bool Prefix(string prefabName, ref bool __result)
    {
        if (!CustomPlaySession.Current.ShouldApplyExperimentChart)
        {
            return true;
        }

        __result = true;
        return false;
    }
}

[HarmonyLib.HarmonyPatch(typeof(Il2CppGameLogic.GameMusicScene), "IsIgnoreCurrentNoteChecking",
    new Type[] { typeof(int), typeof(string) })]
public class GameMusicScene_IsIgnoreCurrentNoteChecking_Patch
{
    public static bool Prefix(int curScene, string notePrefabName, ref bool __result)
    {
        if (!CustomPlaySession.Current.ShouldApplyExperimentChart)
        {
            return true;
        }

        __result = true;
        return false;
    }
}

[HarmonyLib.HarmonyPatch(typeof(Il2CppGameLogic.GameMusicScene), "InitTimer",
    new Type[] { typeof(Il2CppSystem.Decimal) })]
public class GameMusicScene_InitTimer_Patch
{
    private static bool EnableDebugLogs => true;
    private static readonly HashSet<string> loggedGhostUids = new HashSet<string>();

    private static void LogDebug(string message)
    {
        if (!EnableDebugLogs) return;
        SceneDiagnosticLogger.Log("GameMusicScene.InitTimer", message, 20);
    }

    public static void Prefix(Il2CppGameLogic.GameMusicScene __instance, ref Il2CppSystem.Decimal total)
    {
        try
        {
            if (!CustomPlaySession.Current.ShouldApplyExperimentChart)
            {
                return;
            }

            // FixUpdateTimer는 total에서 카운트다운하며 (total - md.showTick) 시점에 노트를 스폰합니다.
            // 커스텀 채보나 음원이 기본 total(360초)보다 길면 후반 노트의 (total - showTick)이 음수가 되어 잘리므로 확장합니다.
            float requiredTotal = ProgressBarPatch.GetEffectiveDuration() + 30f;
            float currentTotal = (float)total;
            if (requiredTotal > currentTotal)
            {
                total = (Il2CppSystem.Decimal)requiredTotal;
                ModLogger.Msg($"[GameMusicScene.InitTimer] 장시간 커스텀 곡 타이머 한도 확장: {currentTotal:F1}s -> {requiredTotal:F1}s");
            }

            if (EnableDebugLogs)
            {
                int frame = 0;
                try { frame = UnityEngine.Time.frameCount; } catch (Exception) { }
                SceneDiagnosticLogger.Log("GameMusicScene.InitTimer.Prefix", $"[GameMusicScene.InitTimer] PRE frame={frame}, total={total}", 20);
            }

            var db = Il2CppAssets.Scripts.Database.GlobalDataBase.s_StageInfo;
            InspectPureSceneSegments(db != null ? db.musicList : null);
        }
        catch (Exception ex)
        {
            ModLogger.Error($"[GameMusicScene.InitTimer] Prefix 예외: {ex}");
        }
    }

    /// <summary>
    /// 변형 없이 보존된 BMS 원본 노트들의 zz 분포와 고스트 노트 상태를 관찰 로깅합니다.
    /// </summary>
    private static void InspectPureSceneSegments(Il2CppSystem.Collections.Generic.List<Il2CppGameLogic.MusicData> list)
    {
        if (list == null) return;

        var zzCounts = new SortedDictionary<string, int>();
        loggedGhostUids.Clear();

        for (int i = 0; i < list.Count; i++)
        {
            try
            {
                var note = list[i];
                if (note == null || note.noteData == null) continue;

                string uid = note.noteData.uid;
                ScenePatchHelpers.CountZz(zzCounts, uid);
                LogGhostNoteIfNeeded(note, uid, note.noteData.prefab_name);
            }
            catch (Exception ex) { ModLogger.Verbose($"[GameMusicScene.GhostLog] 무시된 예외: {ex.Message}"); }
        }

        LogDebug($"[GameMusicScene.InitTimer] 무변형 순수 BMS zz 분포: {ScenePatchHelpers.FormatZzCounts(zzCounts)}");
    }

    private static void LogGhostNoteIfNeeded(Il2CppGameLogic.MusicData note, string uid, string prefab)
    {
        try
        {
            if (!ScenePatchHelpers.IsSixDigitUid(uid)) return;
            if (uid.Substring(2, 2) != "17") return;
            if (!loggedGhostUids.Add(uid)) return;

            var nd = note.noteData;
            ModLogger.Msg($"[GameMusicScene.InitTimer.Ghost] uid={uid}, prefab={prefab ?? "(null)"}, scene={nd.scene ?? "(null)"}, " +
                          $"type={nd.type}, pathway={nd.pathway}, tick={note.tick}, showTick={note.showTick}, dt={note.dt}");
        }
        catch (Exception ex) { ModLogger.Verbose($"[GameMusicScene.GhostLog] 무시된 예외: {ex.Message}"); }
    }
}

[HarmonyLib.HarmonyPatch(typeof(Il2CppGameLogic.GameMusicScene), "InitSceneEvents")]
public class GameMusicScene_InitSceneEvents_Patch
{
    public static void Prefix(Il2CppGameLogic.GameMusicScene __instance)
    {
        try
        {
            if (!CustomPlaySession.Current.ShouldApplyExperimentChart) return;

            int frame = 0;
            try { frame = UnityEngine.Time.frameCount; } catch (Exception) { }
            SceneDiagnosticLogger.Log("GameMusicScene.InitSceneEvents", $"[GameMusicScene.InitSceneEvents] PRE frame={frame}, curSceneName={SafeCurSceneName(__instance)}", 20);
        }
        catch (Exception ex) { ModLogger.Error($"[GameMusicScene.InitSceneEvents] Prefix 예외: {ex}"); }
    }

    public static void Postfix(Il2CppGameLogic.GameMusicScene __instance)
    {
        try
        {
            if (!CustomPlaySession.Current.ShouldApplyExperimentChart) return;

            int sceneCount = -1;
            try { sceneCount = __instance != null && __instance.scenes != null ? __instance.scenes.Count : -1; } catch (Exception) { }
            SceneDiagnosticLogger.Log("GameMusicScene.InitSceneEvents", $"[GameMusicScene.InitSceneEvents] POST scenes.Count={sceneCount}, curSceneName={SafeCurSceneName(__instance)}", 20);
        }
        catch (Exception ex) { ModLogger.Error($"[GameMusicScene.InitSceneEvents] Postfix 예외: {ex}"); }
    }

    private static string SafeCurSceneName(Il2CppGameLogic.GameMusicScene scene)
    {
        try { return scene != null ? (scene.curSceneName ?? "(null)") : "(null)"; }
        catch { return "(예외)"; }
    }
}
