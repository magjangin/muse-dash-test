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
        /// <summary>
        /// 스킨 기능과 Verbose 로그가 둘 다 켜진 실행에서만 겁니다. 둘 다 시작 때 정해지고 실행 중에는 바뀌지 않습니다
        /// (<see cref="ModConfig.Load"/>가 <see cref="PatchInstaller"/>보다 먼저 돕니다). <c>Awake</c>는 노트·캐릭터가 생길 때마다
        /// 불려서, 본문이 첫 줄에서 돌아가도 IL2CPP 트램펄린 비용이 노트마다 들었습니다.
        /// </summary>
        static bool Prepare()
        {
            return ModConfig.EnableSpineSkin && ModLogger.IsLevelEnabled(ModLogLevel.Verbose);
        }

        static void Postfix(SpineActionController __instance)
        {
            // 이 프로브는 커스텀 스킨 대상을 찾기 위한 것이라, 스킨 기능을 끄면 로그도 나오지 않아야 합니다.
            // Verbose가 꺼져 있을 때는 노트·캐릭터 스폰마다 일어나는 IL2CPP 문자열 마샬링(gameObject.name) 자체를 건너뜁니다.
            if (!ModConfig.EnableSpineSkin || !ModLogger.IsLevelEnabled(ModLogLevel.Verbose)) return;
            if (__instance == null || __instance.gameObject == null) return;

            try
            {
                var name = __instance.gameObject.name;
                // Awake는 노트·캐릭터 컨트롤러가 생길 때마다 불립니다. 소문자 사본을 만들지 않고 비교합니다.
                if (name != null && name.IndexOf("battle", System.StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    ModLogger.Verbose($"[SkinNameProbe] 배틀 오브젝트 이름 = \"{name}\"");
                }
            }
            catch (System.Exception ex)
            {
                ModLogger.Verbose($"[SkinNameProbe] 무시된 예외: {ex.Message}");
            }
        }
    }
}
