using HarmonyLib;
using Il2CppAssets.Scripts.Database;

namespace muse_dash_test.Patches.Database
{
    /// <summary>
    /// 계정 레벨을 Lv.9999 만렙으로 항시 고정하여
    /// 곡 클리어 시 반복되는 레벨업 팝업(PnlLevelUpAward) 및 효과음 연출을 완전 차단합니다.
    /// </summary>
    [HarmonyPatch(typeof(DataHelper), nameof(DataHelper.Level), MethodType.Getter)]
    public static class MaxLevelFixPatch
    {
        public static void Postfix(ref int __result)
        {
            __result = 9999;
        }
    }
}
