using HarmonyLib;
using MelonLoader;
using System;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using UnityEngine;

namespace muse_dash_test
{
    internal static class ChangeHealthValuePatchLogger
    {
        private static float lastLogTime = -999f;
        private const float LogCooldown = 10f;

        public static void Log(string message)
        {
            try
            {
                float currentTime = Time.time;
                if (currentTime - lastLogTime >= LogCooldown)
                {
                    MelonLogger.Msg(message);
                    lastLogTime = currentTime;
                }
            }
            catch
            {
                MelonLogger.Msg(message);
            }
        }
    }

    [HarmonyPatch(typeof(Il2Cpp.ChangeHealthValue), "OnGameStart", new Type[] { typeof(Il2CppSystem.Object), typeof(Il2CppSystem.Object), typeof(Il2CppReferenceArray<Il2CppSystem.Object>) })]
    public class ChangeHealthValue_OnGameStart_Patch
    {
        public static void Postfix(Il2Cpp.ChangeHealthValue __instance)
        {
            try
            {
                if (CustomPlaySession.Current.ShouldApplyExperimentChart)
                {
                    if (__instance != null && __instance.text != null)
                    {
                        HywTextStyler.ApplyMadeByHywStyle(__instance.text);
                        ChangeHealthValuePatchLogger.Log("[HywHpTextMod.Hook] OnGameStart: 체력바 텍스트를 'made in 화영왕'으로 변경했습니다.");
                    }
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[ChangeHealthValuePatch.OnGameStart] 예외 발생: {ex}");
            }
        }
    }

    [HarmonyPatch(typeof(Il2Cpp.ChangeHealthValue), "OnHpRateChange", new Type[] { typeof(Il2CppSystem.Object), typeof(Il2CppSystem.Object), typeof(Il2CppReferenceArray<Il2CppSystem.Object>) })]
    public class ChangeHealthValue_OnHpRateChange_Patch
    {
        public static void Postfix(Il2Cpp.ChangeHealthValue __instance)
        {
            try
            {
                if (CustomPlaySession.Current.ShouldApplyExperimentChart)
                {
                    if (__instance != null && __instance.text != null)
                    {
                        HywTextStyler.ApplyMadeByHywStyle(__instance.text);
                        ChangeHealthValuePatchLogger.Log("[HywHpTextMod.Hook] OnHpRateChange: 체력바 텍스트를 'made in 화영왕'으로 유지했습니다.");
                    }
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[ChangeHealthValuePatch.OnHpRateChange] 예외 발생: {ex}");
            }
        }
    }

    [HarmonyPatch(typeof(Il2Cpp.ChangeHealthValue), "OnHpDeduct", new Type[] { typeof(Il2CppSystem.Object), typeof(Il2CppSystem.Object), typeof(Il2CppReferenceArray<Il2CppSystem.Object>) })]
    public class ChangeHealthValue_OnHpDeduct_Patch
    {
        public static void Postfix(Il2Cpp.ChangeHealthValue __instance)
        {
            try
            {
                if (CustomPlaySession.Current.ShouldApplyExperimentChart)
                {
                    if (__instance != null && __instance.text != null)
                    {
                        HywTextStyler.ApplyMadeByHywStyle(__instance.text);
                        ChangeHealthValuePatchLogger.Log("[HywHpTextMod.Hook] OnHpDeduct: 체력바 텍스트를 'made in 화영왕'으로 유지했습니다.");
                    }
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[ChangeHealthValuePatch.OnHpDeduct] 예외 발생: {ex}");
            }
        }
    }

    [HarmonyPatch(typeof(Il2Cpp.ChangeHealthValue), "OnHpAdd", new Type[] { typeof(Il2CppSystem.Object), typeof(Il2CppSystem.Object), typeof(Il2CppReferenceArray<Il2CppSystem.Object>) })]
    public class ChangeHealthValue_OnHpAdd_Patch
    {
        public static void Postfix(Il2Cpp.ChangeHealthValue __instance)
        {
            try
            {
                if (CustomPlaySession.Current.ShouldApplyExperimentChart)
                {
                    if (__instance != null && __instance.text != null)
                    {
                        HywTextStyler.ApplyMadeByHywStyle(__instance.text);
                        ChangeHealthValuePatchLogger.Log("[HywHpTextMod.Hook] OnHpAdd: 체력바 텍스트를 'made in 화영왕'으로 유지했습니다.");
                    }
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[ChangeHealthValuePatch.OnHpAdd] 예외 발생: {ex}");
            }
        }
    }
}
