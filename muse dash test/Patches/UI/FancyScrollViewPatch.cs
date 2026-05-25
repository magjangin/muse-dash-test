using HarmonyLib;
using Il2CppAssets.Scripts.PeroTools.Nice.Components;
using MelonLoader;
using System;

namespace muse_dash_test
{
    [HarmonyPatch(typeof(FancyScrollView), nameof(FancyScrollView.InitData))]
    internal static class FancyScrollView_InitData_Patch
    {
        private static void Prefix(FancyScrollView __instance)
        {
            try { }
            catch (Exception ex) { MelonLogger.Error($"FancyScrollView.InitData Prefix 예외: {ex}"); }
        }

        private static void Postfix(FancyScrollView __instance)
        {
            try { }
            catch (Exception ex) { MelonLogger.Error($"FancyScrollView.InitData Postfix 예외: {ex}"); }
        }

        private static string Describe(FancyScrollView instance)
        {
            if (instance == null)
                return "instance=null";

            string typeName = instance.GetType().FullName;
            string name = instance.name ?? "(null)";
            string gameObjectName = instance.gameObject != null ? instance.gameObject.name : "(null)";
            return $"type={typeName}, name={name}, gameObject={gameObjectName}";
        }
    }
}
