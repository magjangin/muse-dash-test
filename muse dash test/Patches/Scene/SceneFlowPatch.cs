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
        try { frame = UnityEngine.Time.frameCount; time = UnityEngine.Time.time; } catch (Exception) { }
        return $"#{++_seq} frame={frame} t={time:F3}";
    }

    // curScene은 il2cpp 바인딩상 static(전역 현재 씬)이라 타입명으로 접근한다.
    // 실패 시 반환하는 -888은 실제 씬 번호가 아니라 "curScene 읽기 실패"를 뜻하는
    // 센티널 값이다. 로그에서 -888이 보이면 게임 상태가 아니라 접근 실패로 해석한다.
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
    public static bool Prefix(Il2Cpp.SceneChangeController __instance, ref int sceneInfo)
    {
        if (!muse_dash_test.CustomPlaySession.Current.ShouldApplyExperimentChart)
        {
            return true; // 순정곡이면 원본 ChangeNote 실행 허용
        }

        try
        {
            MelonLogger.Msg($"[SceneFlow.ChangeNote]  PRE(SKIP) {SceneFlowLog.Stamp()}, sceneInfo={sceneInfo}, curScene={SceneFlowLog.SafeCurScene(__instance)}");
        }
        catch (Exception ex) { MelonLogger.Error($"[SceneFlow.ChangeNote] Prefix 예외: {ex}"); }
        return false; // 커스텀곡일때만 원본 실행 차단
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
