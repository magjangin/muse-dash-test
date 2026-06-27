using MelonLoader;
using System;
using muse_dash_test;

// Il2CppGameLogic.GameMusicScene.LoadScene(string sceneName) 후킹
[HarmonyLib.HarmonyPatch(typeof(Il2CppGameLogic.GameMusicScene), "LoadScene")]
public class GameMusicScene_LoadScene_Patch
{
    public static void Prefix(Il2CppGameLogic.GameMusicScene __instance, ref string sceneName)
    {
        try
        {
            if (!CustomPlaySession.Current.ShouldApplyExperimentChart)
            {
                MelonLogger.Msg($"[GameMusicScene.LoadScene] 스킵: 커스텀 씬 리다이렉션 비활성, scene={sceneName}, {CustomPlaySession.Current.DescribeApplyDecision()}");
                return;
            }

            string uid = CustomPlaySession.Current.SelectedMusicUid;
            if (string.IsNullOrEmpty(uid))
            {
                uid = PnlStagePatchHelper.GetCurrentSelectedMusicUid() ?? CustomPlaySession.Current.LastClickedMusicUid;
            }

            bool hasCachedScene = muse_dash_test.MainMod.TryGetCachedHwaScene(uid, out int scene);
            MelonLogger.Msg($"[GameMusicScene.LoadScene] uid={uid ?? "(null)"}, sceneName={sceneName}, hasCachedScene={hasCachedScene}, resolvedScene={scene}");

            if (!hasCachedScene)
            {
                ExperimentHitPointInstaller.RememberLoadSceneRedirect(sceneName, sceneName);
                MelonLogger.Msg($"[GameMusicScene.LoadScene] manifest scene이 없어 리다이렉션을 건너뜁니다: current={sceneName}");
                return;
            }

            string redirectedSceneName = $"scene_{scene:00}";
            ExperimentHitPointInstaller.RememberLoadSceneRedirect(sceneName, redirectedSceneName);
            MelonLogger.Msg($"[GameMusicScene.LoadScene] scene 리다이렉션: {sceneName} -> {redirectedSceneName}");
            sceneName = redirectedSceneName;
        }
        catch (Exception ex)
        {
            MelonLogger.Error($"GameMusicScene.LoadScene Prefix 예외: {ex}");
        }
    }
}
