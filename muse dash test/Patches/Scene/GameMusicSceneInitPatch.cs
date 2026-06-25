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
            int frame = 0;
            try { frame = UnityEngine.Time.frameCount; } catch { }
            MelonLogger.Msg($"[GameMusicScene.InitTimer] PRE frame={frame}, total={total}");

            // [실험] musicList의 zz=01 노트를 07로 변형 (InitTimer 시점으로 이동).
            var db = Il2CppAssets.Scripts.Database.GlobalDataBase.s_StageInfo;
            int changed = TransformZz(db != null ? db.musicList : null, "01", "07");
            MelonLogger.Msg($"[GameMusicScene.InitTimer] 변형(01->07): {changed}개");
        }
        catch (Exception ex) { MelonLogger.Error($"[GameMusicScene.InitTimer] Prefix 예외: {ex}"); }
    }

    // 리스트에서 uid가 fromZz로 시작하는 노트의 zz(앞2자리)를 toZz로 바꾼다. uid/mirror_uid/scene/prefab 동기화.
    private static int TransformZz(Il2CppSystem.Collections.Generic.List<Il2CppGameLogic.MusicData> list, string fromZz, string toZz)
    {
        int changed = 0;
        if (list == null) return 0;
        for (int i = 0; i < list.Count; i++)
        {
            try
            {
                var note = list[i];
                if (note == null) continue;
                var nd = note.noteData;
                if (nd == null) continue;
                string uid = nd.uid;
                if (string.IsNullOrEmpty(uid) || uid.Length < 2 || !uid.StartsWith(fromZz)) continue;

                nd.uid = toZz + uid.Substring(2);
                try { if (!string.IsNullOrEmpty(nd.mirror_uid) && nd.mirror_uid.StartsWith(fromZz)) nd.mirror_uid = toZz + nd.mirror_uid.Substring(2); } catch { }
                try { nd.scene = "scene_" + toZz; } catch { }
                try { if (!string.IsNullOrEmpty(nd.prefab_name) && nd.prefab_name.StartsWith(fromZz)) nd.prefab_name = toZz + nd.prefab_name.Substring(2); } catch { }
                try { note.noteData = nd; } catch { }
                try { list[i] = note; } catch { }
                changed++;
            }
            catch { }
        }
        return changed;
    }
}

[HarmonyLib.HarmonyPatch(typeof(Il2CppGameLogic.GameMusicScene), "InitSceneEvents")]
public class GameMusicScene_InitSceneEvents_Patch
{
    public static void Prefix(Il2CppGameLogic.GameMusicScene __instance)
    {
        try
        {
            int frame = 0;
            try { frame = UnityEngine.Time.frameCount; } catch { }
            MelonLogger.Msg($"[GameMusicScene.InitSceneEvents] PRE frame={frame}, curSceneName={SafeCurSceneName(__instance)}");
        }
        catch (Exception ex) { MelonLogger.Error($"[GameMusicScene.InitSceneEvents] Prefix 예외: {ex}"); }
    }

    public static void Postfix(Il2CppGameLogic.GameMusicScene __instance)
    {
        try
        {
            int sceneCount = -1;
            try { sceneCount = __instance != null && __instance.scenes != null ? __instance.scenes.Count : -1; } catch { }
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
