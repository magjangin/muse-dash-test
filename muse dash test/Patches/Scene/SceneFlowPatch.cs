using MelonLoader;
using System;

// SceneChangeController 후킹/로깅.
// 씬 전환 실행부(ChangeScene/ChangeNote/SceneAnimationReset)를 가로채 호출 타이밍·순번·curScene 전후를
// 기록한다. "투 톱 연타"(같은 tick에 여러 씬 토글)가 하드컷인지 ChangeNote로 고스트를 만드는지 실측용.
internal static class SceneFlowLog
{
    private static int _seq;

    // frameCount/time을 같이 찍어 같은 프레임 내 연타인지, 프레임 분산인지 구분한다.
    public static string Stamp()
    {
        int frame = 0;
        float time = 0f;
        try { frame = UnityEngine.Time.frameCount; time = UnityEngine.Time.time; } catch { }
        return $"#{++_seq} frame={frame} t={time:F3}";
    }

    // curScene은 il2cpp 바인딩상 static(전역 현재 씬)이라 타입명으로 접근한다.
    public static int SafeCurScene(Il2Cpp.SceneChangeController inst)
    {
        try { return Il2Cpp.SceneChangeController.curScene; }
        catch { return -888; }
    }
}

[HarmonyLib.HarmonyPatch(typeof(Il2Cpp.SceneChangeController), "ChangeScene", new Type[] { typeof(int) })]
public class SceneChangeController_ChangeScene_Patch
{
    public static void Prefix(Il2Cpp.SceneChangeController __instance, ref int sceneInfo)
    {
        try
        {
            MelonLogger.Msg($"[SceneFlow.ChangeScene] PRE  {SceneFlowLog.Stamp()}, sceneInfo={sceneInfo}, curScene={SceneFlowLog.SafeCurScene(__instance)}");
        }
        catch (Exception ex) { MelonLogger.Error($"[SceneFlow.ChangeScene] Prefix 예외: {ex}"); }
    }

    public static void Postfix(Il2Cpp.SceneChangeController __instance, int sceneInfo)
    {
        try
        {
            MelonLogger.Msg($"[SceneFlow.ChangeScene] POST {SceneFlowLog.Stamp()}, sceneInfo={sceneInfo}, curScene={SceneFlowLog.SafeCurScene(__instance)}");
        }
        catch (Exception ex) { MelonLogger.Error($"[SceneFlow.ChangeScene] Postfix 예외: {ex}"); }
    }
}

[HarmonyLib.HarmonyPatch(typeof(Il2Cpp.SceneChangeController), "ChangeNote", new Type[] { typeof(int) })]
public class SceneChangeController_ChangeNote_Patch
{
    // [실험] false 반환 → 원본 ChangeNote 미실행. 노트 세트 교체를 막아본다.
    public static bool Prefix(Il2Cpp.SceneChangeController __instance, ref int sceneInfo)
    {
        try
        {
            MelonLogger.Msg($"[SceneFlow.ChangeNote]  PRE(SKIP) {SceneFlowLog.Stamp()}, sceneInfo={sceneInfo}, curScene={SceneFlowLog.SafeCurScene(__instance)}");
        }
        catch (Exception ex) { MelonLogger.Error($"[SceneFlow.ChangeNote] Prefix 예외: {ex}"); }
        return false; // 원본 실행 안 함
    }

    public static void Postfix(Il2Cpp.SceneChangeController __instance, int sceneInfo)
    {
        try
        {
            MelonLogger.Msg($"[SceneFlow.ChangeNote]  POST {SceneFlowLog.Stamp()}, sceneInfo={sceneInfo}, curScene={SceneFlowLog.SafeCurScene(__instance)}");
        }
        catch (Exception ex) { MelonLogger.Error($"[SceneFlow.ChangeNote] Postfix 예외: {ex}"); }
    }
}

[HarmonyLib.HarmonyPatch(typeof(Il2Cpp.SceneChangeController), "SceneAnimationReset", new Type[] { typeof(int) })]
public class SceneChangeController_SceneAnimationReset_Patch
{
    public static void Prefix(Il2Cpp.SceneChangeController __instance, ref int sceneInfo)
    {
        try
        {
            MelonLogger.Msg($"[SceneFlow.AnimReset]   PRE  {SceneFlowLog.Stamp()}, sceneInfo={sceneInfo}, curScene={SceneFlowLog.SafeCurScene(__instance)}");
        }
        catch (Exception ex) { MelonLogger.Error($"[SceneFlow.SceneAnimationReset] Prefix 예외: {ex}"); }
    }
}
