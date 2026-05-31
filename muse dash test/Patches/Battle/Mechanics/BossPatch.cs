using MelonLoader;
using System;
using System.Reflection;

// Il2Cpp.Boss 후킹: Play(string key, bool playAnimator = true) 및 SetBoss()
[HarmonyLib.HarmonyPatch(typeof(Il2Cpp.Boss), "Play", new Type[] { typeof(string), typeof(bool) })]
public class Boss_Play_Patch
{
    public static bool isDynamicSwapping = false;

    private static void DumpBossFields(Il2Cpp.Boss boss)
    {
        try
        {
            MelonLogger.Msg($"=== Boss Fields & Properties Dump ===");
            var fields = typeof(Il2Cpp.Boss).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var f in fields)
            {
                try
                {
                    object val = f.GetValue(boss);
                    MelonLogger.Msg($"  [Field] {f.Name} = {val}");
                }
                catch (Exception ex)
                {
                    MelonLogger.Msg($"  [Field] {f.Name} = (Error: {ex.Message})");
                }
            }
            var properties = typeof(Il2Cpp.Boss).GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var p in properties)
            {
                try
                {
                    object val = p.GetValue(boss);
                    MelonLogger.Msg($"  [Property] {p.Name} = {val}");
                }
                catch (Exception ex)
                {
                    MelonLogger.Msg($"  [Property] {p.Name} = (Error: {ex.Message})");
                }
            }
            MelonLogger.Msg($"=====================================");
        }
        catch (Exception ex)
        {
            MelonLogger.Error($"DumpBossFields 예외: {ex}");
        }
    }

    public static bool Prefix(Il2Cpp.Boss __instance, string key, bool playAnimator)
    {
        try
        {
            if (!ExperimentPlayContext.ShouldApplyExperimentChart)
            {
                return true;
            }

            MelonLogger.Msg($"Il2Cpp.Boss.Play 호출: key={key}, playAnimator={playAnimator}, instance={__instance}");

            if (key != null && key.StartsWith("swap:"))
            {
                var parts = key.Split(':');
                if (parts.Length >= 3)
                {
                    string newName = parts[1];
                    if (int.TryParse(parts[2], out int newScene))
                    {
                        // 보스 오브젝트와 그 부모를 강제로 활성화 (out 등으로 비활성화되었을 가능성 방지)
                        try
                        {
                            var component = __instance.Cast<UnityEngine.Component>();
                            if (component != null && component.gameObject != null)
                            {
                                component.gameObject.SetActive(true);

                                if (component.transform != null && component.transform.parent != null)
                                {
                                    component.transform.parent.gameObject.SetActive(true);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MelonLogger.Warning($"[DynamicSwap] gameObject 활성화 시도 중 경고: {ex.Message}");
                        }

                        isDynamicSwapping = true;
                        try
                        {
                            __instance.InitBossObject(newName, newScene, true);
                        }
                        finally
                        {
                            isDynamicSwapping = false;
                        }

                        // 교체 후에도 다시 한번 강제 활성화
                        try
                        {
                            var component = __instance.Cast<UnityEngine.Component>();
                            if (component != null && component.gameObject != null)
                            {
                                component.gameObject.SetActive(true);
                            }
                        }
                        catch {}
                        
                        __instance.Play("in", playAnimator);
                    }
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

    public static void Postfix(Il2Cpp.Boss __instance, string key, bool playAnimator)
    {
        try { }
        catch (Exception ex) { MelonLogger.Error($"Boss.Play Postfix 예외: {ex}"); }
    }
}

[HarmonyLib.HarmonyPatch(typeof(Il2Cpp.Boss), "SetBoss")]
public class Boss_SetBoss_Patch
{
    public static void Prefix(Il2Cpp.Boss __instance)
    {
        try { }
        catch (Exception ex) { MelonLogger.Error($"Boss.SetBoss Prefix 예외: {ex}"); }
    }

    public static void Postfix(Il2Cpp.Boss __instance)
    {
        try { }
        catch (Exception ex) { MelonLogger.Error($"Boss.SetBoss Postfix 예외: {ex}"); }
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
    // 주의: 원래 씬을 무시하려면 OrigScene에 null을 사용하세요(이전의 -1 표식 대신).
    private static readonly BossRule[] BossRewriteRules = new[]
    {
        new BossRule { OrigName = "*", OrigScene = null, OrigIsLast = null, NewName = "0701_boss", NewScene = 7 },
    };

    public static void Prefix(Il2Cpp.Boss __instance, ref string name, ref int scene, ref bool isLast)
    {
        try
        {
            MelonLogger.Msg($"Il2Cpp.Boss.InitBossObject 호출: name={name}, scene={scene}, isLast={isLast}, instance={__instance}");

            if (!ExperimentPlayContext.ShouldApplyExperimentChart)
            {
                MelonLogger.Msg("Il2Cpp.Boss.InitBossObject: 변경 건너뜀 (실험 차트 아님)");
                return;
            }

            if (Boss_Play_Patch.isDynamicSwapping)
            {
                MelonLogger.Msg("[DynamicSwap] 실시간 보스 교체 중이므로 리디렉션 패스를 건너뜁니다.");
                return;
            }

            foreach (var r in BossRewriteRules)
            {
                bool nameMatch = (r.OrigName == "*") || (name == r.OrigName);
                bool sceneMatch = (!r.OrigScene.HasValue) || (scene == r.OrigScene.Value);
                bool isLastMatch = (!r.OrigIsLast.HasValue) || (isLast == r.OrigIsLast.Value);
                if (nameMatch && sceneMatch && isLastMatch)
                {
                    MelonLogger.Msg($"Il2Cpp.Boss.InitBossObject: 변경 적용 -> name={r.NewName}, scene={r.NewScene}");
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

[HarmonyLib.HarmonyPatch(typeof(Il2Cpp.Boss), "SceneBossChange", new Type[] { typeof(int) })]
public class Boss_SceneBossChange_Patch
{
    private static readonly bool EnableSceneBossChangeRewrite = false;

    public class SceneBossChangeRule
    {
        public int? OrigIdx;
        public int NewIdx;
    }

    private static readonly SceneBossChangeRule[] SceneBossChangeRules = new[]
    {
        new SceneBossChangeRule { OrigIdx = null, NewIdx = 7 },
    };

    public static void Prefix(Il2Cpp.Boss __instance, ref int idx)
    {
        try
        {
            MelonLogger.Msg($"Il2Cpp.Boss.SceneBossChange 호출: idx={idx}, instance={__instance}");

            if (!EnableSceneBossChangeRewrite) return;

            foreach (var rule in SceneBossChangeRules)
            {
                bool idxMatch = !rule.OrigIdx.HasValue || idx == rule.OrigIdx.Value;
                if (!idxMatch) continue;

                MelonLogger.Msg($"Il2Cpp.Boss.SceneBossChange: idx 변경 적용 -> {rule.NewIdx} (원본={idx})");
                idx = rule.NewIdx;
                break;
            }    
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
