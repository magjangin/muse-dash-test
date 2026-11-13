using MelonLoader;
using System;
using System.Linq;
using muse_dash_test;

// Il2Cpp.Boss 후킹: InitBossObject() 및 SceneBossChange() (Play() 동적 스왑은 BossPatch.DynamicSwap.cs 참고)
[HarmonyLib.HarmonyPatch(typeof(Il2Cpp.Boss), nameof(Il2Cpp.Boss.InitBossObject), new Type[] { typeof(string), typeof(int), typeof(bool) })]
public class Boss_InitBossObject_Patch
{
    /*
     보스 노트의 prefab_name=empty_000은 보스 액션을 실행하는 빈 트리거입니다.
     실제 화면에 나오는 보스 프리팹은 이 InitBossObject의 name/scene 값으로 결정됩니다.

     확인한 정답:
     - name: 0401_boss
     - scene: txt의 씬 번호를 그대로 사용

     다른 보스를 실험하려면 BossRewriteRules의 NewName/NewScene만 바꾸면 됩니다.
    */

    public class BossRule { public string OrigName; public int? OrigScene; public bool? OrigIsLast; public string NewName; public int NewScene; }

    // 간단한 매핑 규칙: (원래이름, 원래씬, 원래IsLast) -> (변경이름, 변경씬)
    // 기본값은 빈 배열입니다 (기본적으로 씬/차트가 요청한 보스를 유지하며, 하드코딩된 특정 보스로 강제 변경하지 않습니다).
    private static readonly BossRule[] BossRewriteRules = Array.Empty<BossRule>();

    private static bool TryGetFirstBmsBoss(out string bossName, out int bossScene)
    {
        bossName = null;
        bossScene = -1;

        try
        {
            string uid = CustomPlaySession.Current.LastKnownMusicUid;
            if (HwaResourceManager.TryGetCachedHwaBmsChart(uid, CustomRecordStore.ResolveBattleDifficulty(), out var chart))
            {
                if (chart != null && chart.Notes != null)
                {
                    // BMS 노트는 Tick/Time 순으로 이미 정렬되어 있으므로 첫 번째 'in' 노트를 순차 탐색
                    for (int i = 0; i < chart.Notes.Count; i++)
                    {
                        var n = chart.Notes[i];
                        var wavInfo = BmsBossSwapPlanner.ResolveWavInfo(chart, n);
                        if (wavInfo != null && string.Equals(wavInfo.BossTransition, "in", System.StringComparison.OrdinalIgnoreCase))
                        {
                            if (!string.IsNullOrWhiteSpace(wavInfo.BossName) && wavInfo.BossScene >= 0)
                            {
                                bossName = wavInfo.BossName;
                                bossScene = wavInfo.BossScene;
                                return true;
                            }
                            break;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ModLogger.Error($"TryGetFirstBmsBoss 예외: {ex}");
        }

        return false;
    }

    private static readonly System.Collections.Generic.HashSet<int> s_PrewarmedScenes = new System.Collections.Generic.HashSet<int>();

    public static void ResetPrewarmedScenes()
    {
        s_PrewarmedScenes.Clear();
    }

    private static void PrewarmBmsBosses(Il2Cpp.Boss boss)
    {
        if (boss == null) return;
        try
        {
            string uid = CustomPlaySession.Current.LastKnownMusicUid;
            if (HwaResourceManager.TryGetCachedHwaBmsChart(uid, CustomRecordStore.ResolveBattleDifficulty(), out var chart))
            {
                if (chart != null)
                {
                    var swapEvents = BmsBossSwapPlanner.BuildSwapEvents(chart);
                    if (swapEvents != null && swapEvents.Count > 0)
                    {
                        CustomPlaySession.Current.IsDynamicBossSwap = true;
                        try
                        {
                            foreach (var ev in swapEvents)
                            {
                                if (ev?.InWav != null && !string.IsNullOrWhiteSpace(ev.InWav.BossName) && ev.InWav.BossScene >= 0)
                                {
                                    string bossName = ev.InWav.BossName;
                                    int sceneId = ev.InWav.BossScene;
                                    if (ChristmasFestival.ShouldForceForCurrentSong(out _) && (bossName == ChristmasFestival.Boss05 || sceneId == 5))
                                    {
                                        bossName = ChristmasFestival.Boss05Christmas;
                                        sceneId = 5;
                                    }

                                    if (s_PrewarmedScenes.Add(sceneId))
                                    {
                                        if (boss.m_BossObjects == null || !boss.m_BossObjects.ContainsKey(sceneId))
                                        {
                                            ModLogger.Msg($"[DynamicSwap] BMS 보스 사전 로드(Pre-warm): name={bossName}, scene={sceneId}");
                                            boss.InitBossObject(bossName, sceneId, false);
                                            if (boss.m_CurBossObject != null)
                                            {
                                                boss.m_CurBossObject.SetActive(false);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        finally
                        {
                            CustomPlaySession.Current.IsDynamicBossSwap = false;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ModLogger.Warning($"[DynamicSwap] BMS 보스 프리워밍 중 예외: {ex.Message}");
        }
    }

    public static void Prefix(Il2Cpp.Boss __instance, ref string name, ref int scene, ref bool isLast)
    {
        try
        {
            ModLogger.Verbose($"Il2Cpp.Boss.InitBossObject 호출: name={name}, scene={scene}, isLast={isLast}, instance={__instance}");

            if (!CustomPlaySession.Current.ShouldApplyExperimentChart)
            {
                ModLogger.Verbose($"Il2Cpp.Boss.InitBossObject: 변경 건너뜀 ({CustomPlaySession.Current.DescribeApplyDecision()})");
                return;
            }

            if (CustomPlaySession.Current.IsDynamicBossSwap)
            {
                // 배틀 도중 swap마다 도는 자리라 Verbose입니다(바로 위 '호출' 줄도 같은 이유로 Verbose입니다).
                ModLogger.Verbose("[DynamicSwap] 실시간 보스 교체 중이므로 리디렉션 패스를 건너뜁니다.");
                return;
            }

            // 크리스마스 곡인 경우: 5번 보스(0501_boss) 또는 이미 크리스마스 보스로 지정된 경우 크리스마스 보스로 강제 고정
            bool isChristmasSong = ChristmasFestival.ShouldForceForCurrentSong(out string christmasReason);
            if (isChristmasSong && (name == ChristmasFestival.Boss05 || name == ChristmasFestival.Boss05Christmas || scene == 5))
            {
                // BMS 차트에서 첫 보스를 확인하되, 5번 씬 보스(0501_boss)라면 반드시 크리스마스 보스(0501_boss_christmas)로 매핑
                if (TryGetFirstBmsBoss(out string xmasBmsBossName, out int xmasBmsBossScene))
                {
                    PrewarmBmsBosses(__instance);
                    if (xmasBmsBossName == ChristmasFestival.Boss05 || xmasBmsBossScene == 5)
                    {
                        name = ChristmasFestival.Boss05Christmas;
                        scene = 5;
                    }
                    else
                    {
                        name = xmasBmsBossName;
                        scene = xmasBmsBossScene;
                    }
                }
                else
                {
                    name = ChristmasFestival.Boss05Christmas;
                    scene = 5;
                }

                ModLogger.Msg($"[ChristmasFestival] Il2Cpp.Boss.InitBossObject: 크리스마스 보스 확정 -> name={name}, scene={scene} ({christmasReason})");
                return;
            }

            // 1. BMS 차트의 첫 번째 'in' 노트를 검색하여 보스 동적 결정 시도
            if (TryGetFirstBmsBoss(out string firstBossName, out int firstBossScene))
            {
                PrewarmBmsBosses(__instance);

                ModLogger.Msg($"Il2Cpp.Boss.InitBossObject: BMS 첫 'in' 노트를 통한 동적 보스 매핑 적용 -> name={firstBossName}, scene={firstBossScene}");
                name = firstBossName;
                scene = firstBossScene;
                return;
            }

            // 2. BMS에서 찾지 못한 경우 기존 Static Rewrite Rules를 폴백으로 수행
            foreach (var r in BossRewriteRules)
            {
                bool nameMatch = (r.OrigName == "*") || (name == r.OrigName);
                bool sceneMatch = (!r.OrigScene.HasValue) || (scene == r.OrigScene.Value);
                bool isLastMatch = (!r.OrigIsLast.HasValue) || (isLast == r.OrigIsLast.Value);
                if (nameMatch && sceneMatch && isLastMatch)
                {
                    ModLogger.Msg($"Il2Cpp.Boss.InitBossObject: Static 폴백 변경 적용 -> name={r.NewName}, scene={r.NewScene}");
                    name = r.NewName;
                    scene = r.NewScene;
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            ModLogger.Error($"Boss.InitBossObject Prefix 예외: {ex}");
        }
    }

    public static void Postfix(Il2Cpp.Boss __instance, string name, int scene, bool isLast)
    {
        try
        {
            ModLogger.Msg($"Il2Cpp.Boss.InitBossObject 완료: name={name}, scene={scene}, isLast={isLast}");

            // (프리팹 교체 로직은 제거됨 — 간단한 매핑/로깅만 수행합니다)
        }
        catch (Exception ex)
        {
            ModLogger.Error($"Boss.InitBossObject Postfix 예외: {ex}");
        }
    }
}

// 씬 전환 시 보스 교체 제어 패치:
// 커스텀 차트에서는 단순 배경 씬 전환 노트를 마구 넣어도 현재 보스를 끄거나 교체하지 않고 유지합니다.
// (공식곡 및 dynamicSwap 도중에는 원래 동작 허용)
[HarmonyLib.HarmonyPatch(typeof(Il2Cpp.Boss), nameof(Il2Cpp.Boss.SceneBossChange), new Type[] { typeof(int) })]
public class Boss_SceneBossChange_Patch
{
    public static bool Prefix(Il2Cpp.Boss __instance, int idx)
    {
        try
        {
            if (!CustomPlaySession.Current.ShouldApplyExperimentChart)
            {
                ModLogger.Verbose($"Il2Cpp.Boss.SceneBossChange 호출: idx={idx}, instance={__instance}");
                return true;
            }

            if (CustomPlaySession.Current.IsDynamicBossSwap)
            {
                ModLogger.Verbose($"Il2Cpp.Boss.SceneBossChange (DynamicSwap) 호출: idx={idx}, instance={__instance}");
                return true;
            }

            ModLogger.Verbose($"[Boss.SceneBossChange] 커스텀 차트 씬 전환 무시 (보스 유지): idx={idx}, curBossIndex={__instance?.m_CurBossIndex}");
            return false;
        }
        catch (Exception ex)
        {
            ModLogger.Error($"Boss.SceneBossChange Prefix 예외: {ex}");
            return true;
        }
    }

    public static void Postfix(Il2Cpp.Boss __instance, int idx)
    {
        try
        {
            ModLogger.Verbose($"Il2Cpp.Boss.SceneBossChange 완료: idx={idx}");
        }
        catch (Exception ex)
        {
            ModLogger.Error($"Boss.SceneBossChange Postfix 예외: {ex}");
        }
    }
}
