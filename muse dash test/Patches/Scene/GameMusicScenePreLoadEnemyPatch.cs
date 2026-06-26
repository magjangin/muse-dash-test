using MelonLoader;
using System;
using muse_dash_test;


// Il2CppGameLogic.GameMusicScene.PreLoadEnemy() 후킹 — 노트 오브젝트 풀(preloads/objCtrls)을 빌드하는 지점.
// 이 풀이 어느 소스 리스트에서 만들어지는지 판별: musicList(우리가 비운 것, 68) vs oriMusicList(원본, 501?).
[HarmonyLib.HarmonyPatch(typeof(Il2CppGameLogic.GameMusicScene), "PreLoadEnemy")]
public class GameMusicScene_PreLoadEnemy_Patch
{
    public static void Prefix(Il2CppGameLogic.GameMusicScene __instance)
    {
        try
        {
            if (!muse_dash_test.CustomPlaySession.Current.ShouldApplyExperimentChart) return;

            int frame = 0;
            try { frame = UnityEngine.Time.frameCount; } catch (Exception) { }

            var db = Il2CppAssets.Scripts.Database.GlobalDataBase.s_StageInfo;
            int musicCount = -1, oriCount = -1;
            string musicHead = "?", oriHead = "?";
            if (db != null)
            {
                try { musicCount = db.musicList != null ? db.musicList.Count : -1; } catch (Exception) { }
                try { oriCount = db.oriMusicList != null ? db.oriMusicList.Count : -1; } catch (Exception) { }
                musicHead = Describe(db.musicList);
                oriHead = Describe(db.oriMusicList);
            }

            // 변형은 SetRuntimeMusicData(더 이른 시점)로 옮겼다. 여기선 그 결과만 확인한다.
            MelonLogger.Msg($"[PreLoadEnemy] PRE frame={frame}: musicList.Count={musicCount}, oriMusicList.Count={oriCount}");
            MelonLogger.Msg($"[PreLoadEnemy]   musicList    {musicHead}");
            MelonLogger.Msg($"[PreLoadEnemy]   oriMusicList {oriHead}");
        }
        catch (Exception ex) { MelonLogger.Error($"[PreLoadEnemy] Prefix 예외: {ex}"); }
    }

    // 빌드 후 풀 크기 → 어느 리스트로 만들었는지 판별.
    public static void Postfix(Il2CppGameLogic.GameMusicScene __instance)
    {
        try
        {
            if (!muse_dash_test.CustomPlaySession.Current.ShouldApplyExperimentChart) return;

            int preloadCount = -1, objCtrlCount = -1, preloads1Count = -1;
            try { preloadCount = __instance.preloads != null ? __instance.preloads.Count : -1; } catch (Exception) { }
            try { objCtrlCount = __instance.objCtrls != null ? __instance.objCtrls.Count : -1; } catch (Exception) { }
            try { preloads1Count = __instance.preloads1 != null ? __instance.preloads1.Count : -1; } catch (Exception) { }
            MelonLogger.Msg($"[PreLoadEnemy] POST 풀 크기: preloads={preloadCount}, objCtrls={objCtrlCount}, preloads1={preloads1Count}");

            var db = Il2CppAssets.Scripts.Database.GlobalDataBase.s_StageInfo;
            int restored = SceneZzTransformTracker.RestoreIdentities(db != null ? db.musicList : null);
            MelonLogger.Msg($"[PreLoadEnemy] POST BMS 정체 복구: restored={restored}, tracked={SceneZzTransformTracker.Count}, bmsOriginals={SceneZzTransformTracker.BmsOriginalCount}");
            int runtimeRestored = SceneZzTransformTracker.RestoreRuntimeObjects(__instance);
            MelonLogger.Msg($"[PreLoadEnemy] POST 런타임 객체 BMS 정체 복구: restored={runtimeRestored}");
            MelonLogger.Msg($"[PreLoadEnemy] POST 복구 후 musicList {Describe(db != null ? db.musicList : null)}");
        }
        catch (Exception ex) { MelonLogger.Error($"[PreLoadEnemy] Postfix 예외: {ex}"); }
    }

    // 리스트 전체를 스캔: uid zz(앞2자리) 프리픽스 분포 + 첫 non-null uid 몇 개.
    private static string Describe(Il2CppSystem.Collections.Generic.List<Il2CppGameLogic.MusicData> list)
    {
        try
        {
            if (list == null) return "(null)";
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
            sb.Append($"nullUid={nullCount}, zz분포={{");
            bool first = true;
            foreach (var kv in hist) { if (!first) sb.Append(", "); sb.Append($"{kv.Key}:{kv.Value}"); first = false; }
            sb.Append($"}}, 첫non-null=[{string.Join(", ", firstNonNull)}]");
            return sb.ToString();
        }
        catch (Exception ex) { return $"(예외:{ex.GetType().Name})"; }
    }
}
