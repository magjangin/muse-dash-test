using MelonLoader;
using System;

// Il2CppGameLogic.GameMusicScene.Run() 후킹 — 씬 슬롯 컬렉션과 musicList 상태를 관찰용으로 덤프한다.
[HarmonyLib.HarmonyPatch(typeof(Il2CppGameLogic.GameMusicScene), "Run")]
public class GameMusicScene_Run_Patch
{
    private static int _dumpCount;
    private static bool _musicDumped;

    public static void Postfix(Il2CppGameLogic.GameMusicScene __instance)
    {
        try
        {
            if (__instance == null) return;
            _dumpCount++;

            int frame = 0;
            try { frame = UnityEngine.Time.frameCount; } catch (Exception) { }
            MelonLogger.Msg($"[GameMusicScene.Run] === dump #{_dumpCount}, frame={frame} ===");

            // 핵심 컬렉션 내용 덤프 ─ 현재 로드된 씬 슬롯의 실체를 본다.
            DumpKeyCollections(__instance);

            // Run 시점의 뮤직리스트 상태를 1회 덤프 (zz 분포).
            if (!_musicDumped)
            {
                _musicDumped = true;
                DumpMusicList();
            }
        }
        catch (Exception ex)
        {
            MelonLogger.Error($"[GameMusicScene.Run] 덤프 예외: {ex}");
        }
    }

    // Run 시점의 DBStageInfo.musicList를 uid zz 분포로 1회 덤프.
    private static void DumpMusicList()
    {
        try
        {
            var db = Il2CppAssets.Scripts.Database.GlobalDataBase.s_StageInfo;
            var list = db != null ? db.musicList : null;
            if (list == null) { MelonLogger.Msg("[GameMusicScene.Run]   >> musicList = (null)"); return; }

            var hist = new System.Collections.Generic.SortedDictionary<string, int>();
            var firstNonNull = new System.Collections.Generic.List<string>();
            int nullCount = 0;
            for (int i = 0; i < list.Count; i++)
            {
                string uid = null;
                try { uid = list[i]?.noteData?.uid; } catch (Exception) { }
                if (string.IsNullOrEmpty(uid)) { nullCount++; continue; }
                string zz = uid.Length >= 2 ? uid.Substring(0, 2) : uid;
                hist[zz] = hist.TryGetValue(zz, out int c) ? c + 1 : 1;
                if (firstNonNull.Count < 6) firstNonNull.Add(uid);
            }
            var sb = new System.Text.StringBuilder();
            sb.Append($"Count={list.Count}, nullUid={nullCount}, zz분포={{");
            bool first = true;
            foreach (var kv in hist) { if (!first) sb.Append(", "); sb.Append($"{kv.Key}:{kv.Value}"); first = false; }
            sb.Append($"}}, 첫non-null=[{string.Join(", ", firstNonNull)}]");
            MelonLogger.Msg($"[GameMusicScene.Run]   >> musicList {sb}");
        }
        catch (Exception ex) { MelonLogger.Error($"[GameMusicScene.Run] musicList 덤프 예외: {ex}"); }
    }

    // 씬 슬롯 관련 핵심 컬렉션의 실제 내용(이름/키)을 풀어서 찍는다.
    private static void DumpKeyCollections(Il2CppGameLogic.GameMusicScene scene)
    {
        // scenes: 현재 로드된 씬 GameObject들의 이름
        try
        {
            var scenes = scene.scenes;
            if (scenes != null)
            {
                MelonLogger.Msg($"[GameMusicScene.Run]   >> scenes (Count={scenes.Count}):");
                for (int i = 0; i < scenes.Count; i++)
                {
                    string name = "(null)";
                    try { name = scenes[i] != null ? scenes[i].name : "(null)"; } catch (Exception ex) { name = $"(예외:{ex.GetType().Name})"; }
                    MelonLogger.Msg($"[GameMusicScene.Run]        [{i}] {name}");
                }
            }
        }
        catch (Exception ex) { MelonLogger.Error($"[GameMusicScene.Run] scenes 덤프 예외: {ex}"); }

        // scenesAnimas: 씬 이름(키) → 애니메이터 배열
        try
        {
            var sa = scene.scenesAnimas;
            if (sa != null)
            {
                MelonLogger.Msg($"[GameMusicScene.Run]   >> scenesAnimas keys (Count={sa.Count}):");
                var e = sa.Keys.GetEnumerator();
                while (e.MoveNext())
                {
                    MelonLogger.Msg($"[GameMusicScene.Run]        key='{e.Current}'");
                }
            }
        }
        catch (Exception ex) { MelonLogger.Error($"[GameMusicScene.Run] scenesAnimas 덤프 예외: {ex}"); }

        // SceneSubCtrls: 씬 인덱스(키) → SceneSubControl
        try
        {
            var subs = scene.SceneSubCtrls;
            if (subs != null)
            {
                MelonLogger.Msg($"[GameMusicScene.Run]   >> SceneSubCtrls keys (Count={subs.Count}):");
                var e = subs.Keys.GetEnumerator();
                while (e.MoveNext())
                {
                    MelonLogger.Msg($"[GameMusicScene.Run]        key={e.Current}");
                }
            }
        }
        catch (Exception ex) { MelonLogger.Error($"[GameMusicScene.Run] SceneSubCtrls 덤프 예외: {ex}"); }
    }
}
