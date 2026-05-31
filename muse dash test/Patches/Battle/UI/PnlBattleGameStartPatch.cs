using MelonLoader;
using System;
using System.Reflection;
using Il2CppAssets.Scripts.UI.Panels;

// PnlBattle.GameStart 호출 로그만 남기는 보조 패치
[HarmonyLib.HarmonyPatch]
public class PnlBattle_GameStart_Patch
{
    private static MethodBase TargetMethod()
    {
        Type battleType = FindBattleType();
        if (battleType == null)
        {
            MelonLogger.Warning("[PnlBattle.GameStart] PnlBattle 타입을 찾지 못했습니다.");
            return null;
        }

        return battleType.GetMethod("GameStart", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
    }

    public static void Postfix(object __instance)
    {
        MelonLogger.Msg($"[PnlBattle.GameStart] 호출됨: {__instance}");
    }

    private static Type FindBattleType()
    {
        try
        {
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                Type[] types = null;
                try
                {
                    types = assembly.GetTypes();
                }
                catch (ReflectionTypeLoadException ex)
                {
                    types = ex.Types;
                }
                catch
                {
                    continue;
                }

                if (types == null)
                {
                    continue;
                }

                foreach (Type type in types)
                {
                    if (type == null)
                    {
                        continue;
                    }

                    if (string.Equals(type.Name, "PnlBattle", StringComparison.OrdinalIgnoreCase) ||
                        string.Equals(type.FullName, "Il2Cpp.PnlBattle", StringComparison.OrdinalIgnoreCase) ||
                        (type.FullName != null && type.FullName.EndsWith(".PnlBattle", StringComparison.OrdinalIgnoreCase)))
                    {
                        return type;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            MelonLogger.Error($"[PnlBattle.GameStart] PnlBattle 타입 탐색 실패: {ex}");
        }

        return null;
    }
}