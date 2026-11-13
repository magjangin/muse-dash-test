using System;
using muse_dash_test;

// Il2Cpp.Boss 후킹: Play(string key, bool playAnimator = true) — 동적 보스 교체(swap) 처리
[HarmonyLib.HarmonyPatch(typeof(Il2Cpp.Boss), nameof(Il2Cpp.Boss.Play), new Type[] { typeof(string), typeof(bool) })]
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

            ModLogger.Verbose($"Il2Cpp.Boss.Play 호출: key={key}, playAnimator={playAnimator}");

            if (key != null && key.StartsWith("swap:"))
            {
                var parts = key.Split(':');
                if (parts.Length >= 3 && int.TryParse(parts[2], out int newScene))
                {
                    string newName = parts[1];
                    if (ChristmasFestival.ShouldForceForCurrentSong(out _) && (newName == ChristmasFestival.Boss05 || newScene == 5))
                    {
                        newName = ChristmasFestival.Boss05Christmas;
                        newScene = 5;
                    }

                    // 교체 전후 상태를 디버그 로그용으로 남깁니다 (VerboseLog 활성화 시에만).
                    if (ModConfig.EnableVerboseLog)
                    {
                        LogBossState(__instance, "① swap 요청 직전");
                    }

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

                    if (ModConfig.EnableVerboseLog)
                    {
                        LogBossState(__instance, "② InitBossObject 직후");
                    }

                    // InitBossObject가 m_CurBossObject를 새 보스로 갈아끼우므로, 그 새 오브젝트를 다시 깨웁니다.
                    ForceActivateBossObjects(__instance, "InitBossObject 후");

                    __instance.Play("in", playAnimator);

                    if (ModConfig.EnableVerboseLog)
                    {
                        LogBossState(__instance, "③ Play(in) 직후");
                    }
                }
                else
                {
                    // 예전에는 여기서 아무 말 없이 Play를 통째로 삼켜 버려서, 오타가 나면
                    // 보스가 조용히 사라지기만 했습니다.
                    ModLogger.Warning($"[DynamicSwap] swap 키를 해석하지 못해 보스 교체를 건너뜁니다: '{key}' (형식: swap:보스이름:씬번호)");
                }
                return false; // 원래 Play("swap:...") 호출은 무시 및 중단
            }
        }
        catch (Exception ex)
        {
            ModLogger.Error($"Boss.Play Prefix 예외: {ex}");
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
    /// Boss가 내놓는 GameObject는 <c>go</c>(프로퍼티)와 <c>m_CurBossObject</c>(필드) 둘이지만,
    /// 아래 이유로 <b><c>m_CurBossObject</c>만</b> 씁니다.</para>
    ///
    /// <para><b><c>go</c>를 쓰지 않는 이유(실측)</b>: 교체 시점에 <c>boss.go</c>는
    /// <c>KeyNotFoundException(Dictionary.get_Item)</c>을 던집니다. Boss의 유일한 딕셔너리는
    /// <c>m_BossObjects</c>(키 = 씬 번호)인데, 실측 로그에서 키는 <c>[9, 8]</c>인 반면
    /// <c>m_CurBossIndex</c>는 <c>2</c>라 조회가 실패합니다. 즉 <c>get_go</c>는 이 상태에서
    /// 구조적으로 성립하지 않습니다. 게다가 실제로 보스를 되살린 것은 <c>m_CurBossObject</c>
    /// 하나였습니다(로그: "InitBossObject 후/m_CurBossObject: '0801_boss'이(가) 꺼져 있어 강제로 켰습니다").
    /// 부모(<c>SceneObjectController</c>)는 아래 <see cref="ActivateWithParent"/>가 함께 켭니다.</para>
    /// </summary>
    private static void ForceActivateBossObjects(Il2Cpp.Boss boss, string phase)
    {
        if (boss == null) return;

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
                if (ModConfig.EnableVerboseLog)
                {
                    ModLogger.Msg($"[DynamicSwap] {label}: '{target.name}'이(가) 꺼져 있어 강제로 켰습니다.");
                }
            }

            var parent = target.transform != null ? target.transform.parent : null;
            if (parent != null && !parent.gameObject.activeSelf)
            {
                parent.gameObject.SetActive(true);
                if (ModConfig.EnableVerboseLog)
                {
                    ModLogger.Msg($"[DynamicSwap] {label}: 부모 '{parent.gameObject.name}'이(가) 꺼져 있어 강제로 켰습니다.");
                }
            }
        }
        catch (Exception ex)
        {
            ModLogger.Warning($"[DynamicSwap] {label} 활성화 실패: {ex.Message}");
        }
    }

    /// <summary>보스 교체 각 단계의 실제 상태를 한 줄로 남깁니다. 스왑이 안 보일 때 원인 지점을 특정하기 위한 계측입니다.</summary>
    private static void LogBossState(Il2Cpp.Boss boss, string phase)
    {
        if (!ModConfig.EnableVerboseLog) return;

        if (boss == null)
        {
            ModLogger.Msg($"[DynamicSwap.State] {phase}: boss=(null)");
            return;
        }

        // boss.go는 여기서 항상 KeyNotFoundException을 던지므로 찍지 않습니다
        // (사유는 ForceActivateBossObjects 주석 참고). 대신 그 원인이 되는
        // curBossIndex와 bossObjects 키를 나란히 남겨 불일치가 눈에 보이게 합니다.
        ModLogger.Msg(
            $"[DynamicSwap.State] {phase}: " +
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
            ModLogger.Warning($"[DynamicSwap] Boss.{memberName} 접근 실패: {ex.Message}");
            return null;
        }
    }
}
