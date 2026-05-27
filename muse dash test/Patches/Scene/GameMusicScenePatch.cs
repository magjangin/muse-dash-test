using MelonLoader;
using System;

// Il2CppGameLogic.GameMusicScene.LoadScene(string sceneName) 후킹
[HarmonyLib.HarmonyPatch(typeof(Il2CppGameLogic.GameMusicScene), "LoadScene")]
public class GameMusicScene_LoadScene_Patch
{
    private static readonly bool EnableLoadSceneRewrite = true;

    public class LoadSceneRule
    {
        public string OrigSceneName;
        public string NewSceneName;
    }

    private static readonly LoadSceneRule[] LoadSceneRewriteRules = new[]
    {
        new LoadSceneRule { OrigSceneName = "*", NewSceneName = "scene_10" },
    };

    public static void Prefix(Il2CppGameLogic.GameMusicScene __instance, ref string sceneName)
    {
        try
        {
            if (!EnableLoadSceneRewrite) return;
            if (!ExperimentPlayContext.ShouldApplyExperimentChart) return;

            foreach (var rule in LoadSceneRewriteRules)
            {
                bool sceneMatch = rule.OrigSceneName == "*" || sceneName == rule.OrigSceneName;
                if (sceneMatch)
                {
                    sceneName = rule.NewSceneName;
                    break;
                }
            }
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
