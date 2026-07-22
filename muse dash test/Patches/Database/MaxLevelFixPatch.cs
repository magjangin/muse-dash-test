using System;
using HarmonyLib;
using Il2Cpp;
using Il2CppAssets.Scripts.Database;
using Il2CppAssets.Scripts.UI;
using Il2CppInterop.Runtime.InteropTypes.Arrays;

namespace muse_dash_test.Patches.Database
{
    /// <summary>
    /// 글로벌/로컬 레벨 및 경험치 계산, 레벨업 검증 메서드를 정밀 지정하여
    /// 표시 레벨을 9999(만렙)로 고정하고 레벨업 팝업 및 효과음("띠리링~")을 완벽 차단합니다.
    /// </summary>
    [HarmonyPatch(typeof(DataHelper), nameof(DataHelper.Level), MethodType.Getter)]
    public static class DataHelper_Level_Patch
    {
        public static void Postfix(ref int __result)
        {
            __result = 9999;
        }
    }

    [HarmonyPatch(typeof(AccountSaveUtils), nameof(AccountSaveUtils.CacularLevel))]
    public static class AccountSaveUtils_CacularLevel_Patch
    {
        public static void Postfix(ref int __result)
        {
            __result = 9999;
        }
    }

    [HarmonyPatch(typeof(AccountSaveUtils), nameof(AccountSaveUtils.CacularCurExp))]
    public static class AccountSaveUtils_CacularCurExp_Patch
    {
        public static void Postfix(ref int __result)
        {
            __result = 999999;
        }
    }

    [HarmonyPatch(typeof(PnlUnlock), "CheckHaveEnoughExpToLevelUp")]
    public static class PnlUnlock_CheckHaveEnoughExpToLevelUp_Patch
    {
        public static bool Prefix(ref bool __result)
        {
            __result = false;
            return false;
        }
    }

    [HarmonyPatch(typeof(PnlUnlock), "OnLevelUp", new Type[] { typeof(Il2CppSystem.Object), typeof(Il2CppSystem.Object), typeof(Il2CppReferenceArray<Il2CppSystem.Object>) })]
    public static class PnlUnlock_OnLevelUp_Patch
    {
        public static bool Prefix()
        {
            return false;
        }
    }

    [HarmonyPatch(typeof(PnlLevelUpAward), "OnEnable")]
    public static class PnlLevelUpAward_OnEnable_Patch
    {
        public static bool Prefix()
        {
            return false;
        }
    }
}
