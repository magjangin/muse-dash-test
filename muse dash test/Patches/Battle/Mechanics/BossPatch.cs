using MelonLoader;
using System;
using System.Linq;
using muse_dash_test;

// Il2Cpp.Boss 후킹: Play(string key, bool playAnimator = true) 및 SetBoss()
[HarmonyLib.HarmonyPatch(typeof(Il2Cpp.Boss), "Play", new Type[] { typeof(string), typeof(bool) })]
public class Boss_Play_Patch
{
    public static bool Prefix(Il2Cpp.Boss __instance, string key, bool playAnimator)
    {
        try
        {
            if (!CustomPlaySession.Current.ShouldApplyExperimentChart)
            {
                return true;
            }

            MelonLogger.Msg($"Il2Cpp.Boss.Play 호출: key={key}, playAnimator={playAnimator}, instance={__instance}");

            if (key != null && key.StartsWith("swap:"))
            {
                var parts = key.Split(':');
                if (parts.Length >= 3 && int.TryParse(parts[2], out int newScene))
                {
                    string newName = parts[1];

                    // 교체 전후 상태를 양 끝에서 찍어 둡니다. 스왑이 화면에 안 나올 때
                    // "어느 단계에서 꺼져 있었는지"를 로그만 보고 특정할 수 있어야 합니다.
                    LogBossState(__instance, "① swap 요청 직전");

                    // out으로 비활성화된 보스 오브젝트를 깨웁니다.
                    ForceActivateBossObjects(__instance, "InitBossObject 전");

                    CustomPlaySession.Current.IsDynamicBossSwap = true;
                    try
                    {
                        __instance.InitBossObject(newName, newScene, true);
                    }
                    finally
                    {
                        CustomPlaySession.Current.IsDynamicBossSwap = false;
                    }

                    LogBossState(__instance, "② InitBossObject 직후");

                    // InitBossObject가 m_CurBossObject를 새 보스로 갈아끼우므로, 그 새 오브젝트를 다시 깨웁니다.
                    ForceActivateBossObjects(__instance, "InitBossObject 후");

                    __instance.Play("in", playAnimator);

                    LogBossState(__instance, "③ Play(in) 직후");
                }
                else
                {
                    // 예전에는 여기서 아무 말 없이 Play를 통째로 삼켜 버려서, 오타가 나면
                    // 보스가 조용히 사라지기만 했습니다.
                    MelonLogger.Warning($"[DynamicSwap] swap 키를 해석하지 못해 보스 교체를 건너뜁니다: '{key}' (형식: swap:보스이름:씬번호)");
                }
                return false; // 원래 Play("swap:...") 호출은 무시 및 중단
            }
        }
        catch (Exception ex)
        {
            MelonLogger.Error($"Boss.Play Prefix 예외: {ex}");
        }
        return true;
    }

    /// <summary>
    /// 보스 오브젝트와 그 부모를 강제로 활성화합니다. <c>out</c> 액션으로 꺼진 상태를 되살리는 용도입니다.
    ///
    /// <para><b>왜 <c>Cast&lt;Component&gt;</c>가 아닌가</b>: <c>Il2Cpp.Boss</c>는 MonoBehaviour가 아니라
    /// <c>Il2CppSystem.Object</c>를 직접 상속합니다(디컴파일 확인: <c>public class Boss : Il2CppSystem.Object</c>).
    /// 그래서 예전 코드의 <c>__instance.Cast&lt;UnityEngine.Component&gt;()</c>는 항상
    /// <i>Can't cast object of type Boss to type UnityEngine.Component</i>로 던졌고,
    /// 바깥 catch가 경고만 남기는 바람에 <b>활성화가 한 번도 실행되지 않았습니다</b>.
    /// Boss가 실제로 내놓는 GameObject는 <c>go</c>(프로퍼티)와 <c>m_CurBossObject</c>(필드) 둘입니다.</para>
    /// </summary>
    private static void ForceActivateBossObjects(Il2Cpp.Boss boss, string phase)
    {
        if (boss == null) return;

        ActivateWithParent(SafeGet(() => boss.go, "go"), $"{phase}/go");
        ActivateWithParent(SafeGet(() => boss.m_CurBossObject, "m_CurBossObject"), $"{phase}/m_CurBossObject");
    }

    private static void ActivateWithParent(UnityEngine.GameObject target, string label)
    {
        if (target == null) return;

        try
        {
            bool wasActive = target.activeSelf;
            if (!wasActive)
            {
                target.SetActive(true);
                MelonLogger.Msg($"[DynamicSwap] {label}: '{target.name}'이(가) 꺼져 있어 강제로 켰습니다.");
            }

            var parent = target.transform != null ? target.transform.parent : null;
            if (parent != null && !parent.gameObject.activeSelf)
            {
                parent.gameObject.SetActive(true);
                MelonLogger.Msg($"[DynamicSwap] {label}: 부모 '{parent.gameObject.name}'이(가) 꺼져 있어 강제로 켰습니다.");
            }
        }
        catch (Exception ex)
        {
            MelonLogger.Warning($"[DynamicSwap] {label} 활성화 실패: {ex.Message}");
        }
    }

    /// <summary>보스 교체 각 단계의 실제 상태를 한 줄로 남깁니다. 스왑이 안 보일 때 원인 지점을 특정하기 위한 계측입니다.</summary>
    private static void LogBossState(Il2Cpp.Boss boss, string phase)
    {
        if (boss == null)
        {
            MelonLogger.Msg($"[DynamicSwap.State] {phase}: boss=(null)");
            return;
        }

        MelonLogger.Msg(
            $"[DynamicSwap.State] {phase}: " +
            $"go={Describe(SafeGet(() => boss.go, "go"))}, " +
            $"curBossObject={Describe(SafeGet(() => boss.m_CurBossObject, "m_CurBossObject"))}, " +
            $"curBossIndex={SafeGet(() => boss.m_CurBossIndex.ToString(), "m_CurBossIndex") ?? "?"}, " +
            $"bossObjects={DescribeBossObjects(boss)}, " +
            $"curBossIsIn={SafeGet(() => boss.curBossIsIn.ToString(), "curBossIsIn") ?? "?"}, " +
            $"hasEntered={SafeGet(() => boss.BossHasEntered.ToString(), "BossHasEntered") ?? "?"}, " +
            $"disappear={SafeGet(() => boss.m_DisappearBoss.ToString(), "m_DisappearBoss") ?? "?"}");
    }

    private static string Describe(UnityEngine.GameObject go)
    {
        if (go == null) return "(null)";
        try
        {
            string parentName = (go.transform != null && go.transform.parent != null)
                ? go.transform.parent.gameObject.name
                : "(root)";
            return $"'{go.name}'(self={go.activeSelf}, hierarchy={go.activeInHierarchy}, parent='{parentName}')";
        }
        catch (Exception ex)
        {
            return $"(읽기 실패: {ex.Message})";
        }
    }

    private static string DescribeBossObjects(Il2Cpp.Boss boss)
    {
        try
        {
            var map = boss.m_BossObjects;
            if (map == null) return "(null)";

            var keys = new System.Collections.Generic.List<string>();
            foreach (var key in map.Keys)
            {
                keys.Add(key.ToString());
            }
            return $"{map.Count}개[{string.Join(",", keys)}]";
        }
        catch (Exception ex)
        {
            return $"(읽기 실패: {ex.Message})";
        }
    }

    /// <summary>IL2CPP 멤버 접근이 던져도 진단 로그 전체가 죽지 않도록 감쌉니다.</summary>
    private static T SafeGet<T>(Func<T> getter, string memberName) where T : class
    {
        try
        {
            return getter();
        }
        catch (Exception ex)
        {
            MelonLogger.Warning($"[DynamicSwap] Boss.{memberName} 접근 실패: {ex.Message}");
            return null;
        }
    }
}

[HarmonyLib.HarmonyPatch(typeof(Il2Cpp.Boss), "InitBossObject", new Type[] { typeof(string), typeof(int), typeof(bool) })]
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
    // 편집 예시: 모든 보스 호출("*")을 0401_boss, scene 4로 리디렉션합니다.
    // 기본: 모든 보스 호출을 확인한 정답 보스(0401_boss, scene 4)로 리디렉션합니다.
    // 편집하려면 이 배열만 수정하세요.
    private static readonly BossRule[] BossRewriteRules = new[]
    {
        new BossRule { OrigName = "*", OrigScene = null, OrigIsLast = null, NewName = "0701_boss", NewScene = 7 },
    };

    private static bool TryGetFirstBmsBoss(out string bossName, out int bossScene)
    {
        bossName = null;
        bossScene = -1;

        try
        {
            string uid = CustomPlaySession.Current.SelectedMusicUid;
            if (string.IsNullOrEmpty(uid))
            {
                uid = PnlStagePatchHelper.GetCurrentSelectedMusicUid() ?? CustomPlaySession.Current.LastClickedMusicUid;
            }
            if (HwaResourceManager.TryGetCachedHwaBmsChart(uid, out var chart, out _))
            {
                if (chart != null && chart.Notes != null)
                {
                    // 시간 순서대로 정렬하여 첫 번째 'in' 노트를 검색
                    var firstInNote = chart.Notes
                        .OrderBy(n => n.Time)
                        .ThenBy(n => n.Tick)
                        .FirstOrDefault(n => 
                        {
                            var wavInfo = BmsBossSwapPlanner.ResolveWavInfo(chart, n);
                            return wavInfo != null && string.Equals(wavInfo.BossTransition, "in", System.StringComparison.OrdinalIgnoreCase);
                        });

                    if (firstInNote != null)
                    {
                        var wavInfo = BmsBossSwapPlanner.ResolveWavInfo(chart, firstInNote);
                        if (wavInfo != null && !string.IsNullOrWhiteSpace(wavInfo.BossName) && wavInfo.BossScene >= 0)
                        {
                            bossName = wavInfo.BossName;
                            bossScene = wavInfo.BossScene;
                            return true;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            MelonLogger.Error($"TryGetFirstBmsBoss 예외: {ex}");
        }

        return false;
    }

    public static void Prefix(Il2Cpp.Boss __instance, ref string name, ref int scene, ref bool isLast)
    {
        try
        {
            MelonLogger.Msg($"Il2Cpp.Boss.InitBossObject 호출: name={name}, scene={scene}, isLast={isLast}, instance={__instance}");

            if (!CustomPlaySession.Current.ShouldApplyExperimentChart)
            {
                MelonLogger.Msg($"Il2Cpp.Boss.InitBossObject: 변경 건너뜀 ({CustomPlaySession.Current.DescribeApplyDecision()})");
                return;
            }

            if (CustomPlaySession.Current.IsDynamicBossSwap)
            {
                MelonLogger.Msg("[DynamicSwap] 실시간 보스 교체 중이므로 리디렉션 패스를 건너뜁니다.");
                return;
            }

            // 1. BMS 차트의 첫 번째 'in' 노트를 검색하여 보스 동적 결정 시도
            if (TryGetFirstBmsBoss(out string firstBossName, out int firstBossScene))
            {
                MelonLogger.Msg($"Il2Cpp.Boss.InitBossObject: BMS 첫 'in' 노트를 통한 동적 보스 매핑 적용 -> name={firstBossName}, scene={firstBossScene}");
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
                    MelonLogger.Msg($"Il2Cpp.Boss.InitBossObject: Static 폴백 변경 적용 -> name={r.NewName}, scene={r.NewScene}");
                    name = r.NewName;
                    scene = r.NewScene;
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            MelonLogger.Error($"Boss.InitBossObject Prefix 예외: {ex}");
        }
    }

    public static void Postfix(Il2Cpp.Boss __instance, string name, int scene, bool isLast)
    {
        try
        {
            MelonLogger.Msg($"Il2Cpp.Boss.InitBossObject 완료: name={name}, scene={scene}, isLast={isLast}");

            // (프리팹 교체 로직은 제거됨 — 간단한 매핑/로깅만 수행합니다)
        }
        catch (Exception ex)
        {
            MelonLogger.Error($"Boss.InitBossObject Postfix 예외: {ex}");
        }
    }
}

// 진단용 로깅 패치: SceneBossChange 호출 흐름만 추적합니다.
// (idx 재작성 기믹은 사용되지 않아 제거했으며, 필요 시 Prefix에서 ref idx를 조정해 복원할 수 있습니다.)
[HarmonyLib.HarmonyPatch(typeof(Il2Cpp.Boss), "SceneBossChange", new Type[] { typeof(int) })]
public class Boss_SceneBossChange_Patch
{
    public static void Prefix(Il2Cpp.Boss __instance, int idx)
    {
        try
        {
            MelonLogger.Msg($"Il2Cpp.Boss.SceneBossChange 호출: idx={idx}, instance={__instance}");
        }
        catch (Exception ex)
        {
            MelonLogger.Error($"Boss.SceneBossChange Prefix 예외: {ex}");
        }
    }

    public static void Postfix(Il2Cpp.Boss __instance, int idx)
    {
        try
        {
            MelonLogger.Msg($"Il2Cpp.Boss.SceneBossChange 완료: idx={idx}");
        }
        catch (Exception ex)
        {
            MelonLogger.Error($"Boss.SceneBossChange Postfix 예외: {ex}");
        }
    }
}
