using HarmonyLib;
using MelonLoader;
using System;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using UnityEngine;

namespace muse_dash_test
{
    /// <summary>
    /// 체력바 워터마크("made in 화영왕")를 적용해도 되는 상황인지 판정하고, 실제 적용까지 맡습니다.
    ///
    /// <para>아래 4개 훅(OnGameStart / OnHpRateChange / OnHpDeduct / OnHpAdd)이 완전히 같은 조건과
    /// 같은 동작을 각자 복사해 갖고 있었습니다. 그래서 <c>EnableHpTextMod</c> 설정을 추가할 때
    /// 네 곳 모두가 누락됐고, 설정을 꺼도 워터마크가 그대로 찍혔습니다.
    /// 조건을 여기 한 곳으로 모아 다시 갈라지지 않게 합니다.</para>
    /// </summary>
    internal static class HywHpText
    {
        /// <summary>설정이 켜져 있고, 커스텀 차트를 적용 중일 때만 참입니다.</summary>
        public static bool ShouldApply =>
            ModConfig.EnableHpTextMod && CustomPlaySession.Current.ShouldApplyExperimentChart;

        /// <summary>워터마크를 적용합니다. 조건을 만족하지 않거나 대상이 없으면 아무것도 하지 않습니다.</summary>
        public static void Apply(Il2Cpp.ChangeHealthValue instance, string hookName, string verb)
        {
            try
            {
                if (!ShouldApply) return;
                if (instance == null || instance.text == null) return;

                HywTextStyler.ApplyMadeByHywStyle(instance.text);
                ChangeHealthValuePatchLogger.Log($"[HywHpTextMod.Hook] {hookName}: 체력바 텍스트를 'made in 화영왕'으로 {verb}했습니다.");
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[ChangeHealthValuePatch.{hookName}] 예외 발생: {ex}");
            }
        }
    }

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

    [HarmonyPatch(typeof(Il2Cpp.ChangeHealthValue), GameBindings.ChangeHealthValue.OnGameStart, new Type[] { typeof(Il2CppSystem.Object), typeof(Il2CppSystem.Object), typeof(Il2CppReferenceArray<Il2CppSystem.Object>) })]
    public class ChangeHealthValue_OnGameStart_Patch
    {
        public static void Postfix(Il2Cpp.ChangeHealthValue __instance)
        {
            HywHpText.Apply(__instance, "OnGameStart", "변경");
        }
    }

    [HarmonyPatch(typeof(Il2Cpp.ChangeHealthValue), GameBindings.ChangeHealthValue.OnHpRateChange, new Type[] { typeof(Il2CppSystem.Object), typeof(Il2CppSystem.Object), typeof(Il2CppReferenceArray<Il2CppSystem.Object>) })]
    public class ChangeHealthValue_OnHpRateChange_Patch
    {
        public static void Postfix(Il2Cpp.ChangeHealthValue __instance)
        {
            HywHpText.Apply(__instance, "OnHpRateChange", "유지");
        }
    }

    [HarmonyPatch(typeof(Il2Cpp.ChangeHealthValue), GameBindings.ChangeHealthValue.OnHpDeduct, new Type[] { typeof(Il2CppSystem.Object), typeof(Il2CppSystem.Object), typeof(Il2CppReferenceArray<Il2CppSystem.Object>) })]
    public class ChangeHealthValue_OnHpDeduct_Patch
    {
        public static void Postfix(Il2Cpp.ChangeHealthValue __instance)
        {
            HywHpText.Apply(__instance, "OnHpDeduct", "유지");
        }
    }

    [HarmonyPatch(typeof(Il2Cpp.ChangeHealthValue), GameBindings.ChangeHealthValue.OnHpAdd, new Type[] { typeof(Il2CppSystem.Object), typeof(Il2CppSystem.Object), typeof(Il2CppReferenceArray<Il2CppSystem.Object>) })]
    public class ChangeHealthValue_OnHpAdd_Patch
    {
        public static void Postfix(Il2Cpp.ChangeHealthValue __instance)
        {
            HywHpText.Apply(__instance, "OnHpAdd", "유지");
        }
    }
}
