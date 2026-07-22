using HarmonyLib;
using Il2CppAssets.Scripts.Database;
using Il2CppAssets.Scripts.UI;

namespace muse_dash_test.Patches.Database
{
    /// <summary>
    /// 로컬 EXP 기반 레벨 계산 함수(AccountSaveUtils.CacularLevel)와
    /// DataHelper.Level 프로퍼티를 동시에 9999(만렙)로 고정하여
    /// 로컬 데이터 및 UI 레벨 표기를 9999로 변경하고 레벨업 연출/효과음을 완벽히 차단합니다.
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
}
