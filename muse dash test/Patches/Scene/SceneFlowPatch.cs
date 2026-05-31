using MelonLoader;
using System;

[HarmonyLib.HarmonyPatch(typeof(Il2Cpp.SceneChangeController), "ChangeScene", new Type[] { typeof(int) })]
public class SceneChangeController_ChangeScene_Patch
{
    public static void Prefix(Il2Cpp.SceneChangeController __instance, ref int sceneInfo)
    {
        try
        {
            MelonLogger.Msg($"SceneChangeController.ChangeScene Prefix: sceneInfo={sceneInfo}, instance={__instance}");
        }
        catch (Exception ex) { MelonLogger.Error($"SceneChangeController.ChangeScene Prefix 예외: {ex}"); }
    }

    public static void Postfix(Il2Cpp.SceneChangeController __instance, int sceneInfo)
    {
        try
        {
            MelonLogger.Msg($"SceneChangeController.ChangeScene Postfix: sceneInfo={sceneInfo}, instance={__instance}");
        }
        catch (Exception ex) { MelonLogger.Error($"SceneChangeController.ChangeScene Postfix 예외: {ex}"); }
    }
}

[HarmonyLib.HarmonyPatch(typeof(Il2Cpp.SceneChangeController), "ChangeNote", new Type[] { typeof(int) })]
public class SceneChangeController_ChangeNote_Patch
{
    public static void Prefix(Il2Cpp.SceneChangeController __instance, ref int sceneInfo)
    {
        try
        {
            MelonLogger.Msg($"SceneChangeController.ChangeNote Prefix: sceneInfo={sceneInfo}, instance={__instance}");
        }
        catch (Exception ex) { MelonLogger.Error($"SceneChangeController.ChangeNote Prefix 예외: {ex}"); }
    }

    public static void Postfix(Il2Cpp.SceneChangeController __instance, int sceneInfo)
    {
        try
        {
            MelonLogger.Msg($"SceneChangeController.ChangeNote Postfix: sceneInfo={sceneInfo}, instance={__instance}");
        }
        catch (Exception ex) { MelonLogger.Error($"SceneChangeController.ChangeNote Postfix 예외: {ex}"); }
    }
}
