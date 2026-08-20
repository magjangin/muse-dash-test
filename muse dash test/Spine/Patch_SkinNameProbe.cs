using HarmonyLib;
using MelonLoader;
using Il2Cpp;

namespace muse_dash_test
{
    // 새 캐릭터를 커스텀 스킨 대상에 추가하려면, 그 캐릭터의 배틀 오브젝트
    // GameObject 이름(예: "black_girl_battle(Clone)")을 정확히 알아야 한다.
    // 이 패치는 스테이지에서 생성되는 SpineActionController 중 이름에 "battle"이
    // 들어간 것만 골라 한 줄 로그로 찍어준다. 원하는 캐릭터로 한 판 돌린 뒤
    // 로그에서 이름을 확인해 InjectHelper.TargetToBaseName에 붙여넣으면 된다.
    // (이름 탐색이 끝나면 이 파일은 삭제하거나 그대로 둬도 무방하다.)
    [HarmonyPatch(typeof(SpineActionController), nameof(SpineActionController.Awake))]
    internal static class Patch_SkinNameProbe
    {
        static void Postfix(SpineActionController __instance)
        {
            // 이 프로브는 커스텀 스킨 대상을 찾기 위한 것이라, 스킨 기능을 끄면 로그도 나오지 않아야 합니다.
            if (!ModConfig.EnableSpineSkin) return;

            var name = __instance.gameObject.name;
            if (name.ToLowerInvariant().Contains("battle"))
            {
                ModLogger.Msg($"[SkinNameProbe] 배틀 오브젝트 이름 = \"{name}\"");
            }
        }
    }
}
