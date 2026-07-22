using System;
using HarmonyLib;
using Il2Cpp;
using Il2CppInterop.Runtime.InteropTypes.Arrays;

namespace muse_dash_test.Patches.Database
{
    /// <summary>
    /// 레벨 수치(9999 조작)는 변경하지 않고,
    /// 곡 클리어 후 발생하는 레벨업 보상 팝업(PnlLevelUpAward) 및 효과음 연출("띠리링~")만 깔끔하게 차단합니다.
    /// </summary>
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
