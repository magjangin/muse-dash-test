using MelonLoader;
using System;

// Il2CppGameLogic.GameMusicScene.LoadScene(string sceneName) 후킹
[HarmonyLib.HarmonyPatch(typeof(Il2CppGameLogic.GameMusicScene), "LoadScene")]
public class GameMusicScene_LoadScene_Patch
{
    public static void Prefix(Il2CppGameLogic.GameMusicScene __instance, ref string sceneName)
    {
        try
        {
            if (!ExperimentPlayContext.ShouldApplyExperimentChart) return;

            if (!muse_dash_test.MainMod.TryGetCachedHwaScene(out int scene))
            {
                MelonLogger.Msg($"[GameMusicScene.LoadScene] manifest scene이 없어 리다이렉션을 건너뜁니다: current={sceneName}");
                return;
            }

            string redirectedSceneName = $"scene_{scene:00}";
            MelonLogger.Msg($"[GameMusicScene.LoadScene] scene 리다이렉션: {sceneName} -> {redirectedSceneName}");
            sceneName = redirectedSceneName;
        }
        catch (Exception ex)
        {
            MelonLogger.Error($"GameMusicScene.LoadScene Prefix 예외: {ex}");
        }
    }

    public static void Postfix(Il2CppGameLogic.GameMusicScene __instance, string sceneName)
    {
        try { }
        catch (Exception ex) { MelonLogger.Error($"GameMusicScene.LoadScene Postfix 예외: {ex}"); }
    }
}
