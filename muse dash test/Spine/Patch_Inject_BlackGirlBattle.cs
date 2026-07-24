using System.Collections.Generic;
using HarmonyLib;
using MelonLoader;
using Il2Cpp;
using Il2CppSpine.Unity;

namespace muse_dash_test
{
    // 스테이지에서 특정 GameObject 이름의 캐릭터/고스트를 커스텀 스킨으로 교체한다.
    // 그림자는 대상에서 제외한다(사용자 요청으로 원래대로 유지).
    // (원래 spine skin 모드에서 이식: 주입 기능만 가져옴)
    internal static class InjectHelper
    {
        private static readonly Dictionary<string, string> TargetToBaseName = new Dictionary<string, string>
        {
            { "black_girl_battle(Clone)", "char_3_black" },
            { "black_girl_battle_ghost(Clone)", "char_3_black" },
            { "sleepy_girl_battle(Clone)", "char_1_sleepy" },
            { "sleepy_girl_battle_ghost(Clone)", "char_1_sleepy" },
            { "rock_girl_battle(Clone)", "char_1_rock" },
            { "rock_girl_battle_ghost(Clone)", "char_1_rock" },
            { "rampage_girl_battle(Clone)", "char_1_rampage" },
            { "rampage_girl_battle_ghost(Clone)", "char_1_rampage" },
            { "violin_girl_battle(Clone)", "char_3_violin" },
            { "violin_girl_battle_ghost(Clone)", "char_3_violin" },
        };

        public static string ResolveBaseName(string name)
        {
            if (string.IsNullOrEmpty(name)) return null;
            if (TargetToBaseName.TryGetValue(name, out var exact)) return exact;

            string lower = name.ToLowerInvariant();
            if (lower.Contains("black_girl") || lower.Contains("marija_black") || lower.Contains("black_marija") || lower.Contains("char_3_black") || lower.Contains("marija_3"))
                return "char_3_black";
            if (lower.Contains("sleepy_girl") || lower.Contains("sleepy"))
                return "char_1_sleepy";
            if (lower.Contains("rock_girl") || lower.Contains("rock"))
                return "char_1_rock";
            if (lower.Contains("rampage_girl") || lower.Contains("rampage"))
                return "char_1_rampage";
            if (lower.Contains("violin_girl") || lower.Contains("violin"))
                return "char_3_violin";

            return null;
        }

        public static void TryInject(SpineActionController instance, string sourceCaller)
        {
            if (instance == null || instance.gameObject == null) return;
            string goName = instance.gameObject.name;

            string baseName = ResolveBaseName(goName);
            if (baseName == null)
            {
                // battle 단어가 포함된 경우 디버그용 수집 출력
                if (goName.ToLowerInvariant().Contains("battle"))
                {
                    MelonLogger.Msg($"[SpineSkin.Debug] [{sourceCaller}] '{goName}' -> 등록되지 않은 배틀 오브젝트 감지");
                }
                return;
            }

            MelonLogger.Msg($"[SpineSkin.Debug] [{sourceCaller}] '{goName}' -> 타겟 베이스네임 매칭: '{baseName}'");

            var customAsset = CustomSkinInjector.GetOrBuild(baseName);
            if (customAsset == null)
            {
                MelonLogger.Warning($"[SpineSkin] [{sourceCaller}] '{goName}' -> {baseName} 커스텀 스킨 에셋이 null이므로 주입 스킵");
                return;
            }

            var ska = instance.skeletonAnimation;
            if (ska == null)
            {
                MelonLogger.Warning($"[SpineSkin] [{sourceCaller}] '{goName}' -> skeletonAnimation 컴포넌트가 null이라 주입 스킵");
                return;
            }

            ska.skeletonDataAsset = customAsset;
            ska.Initialize(true);
            MelonLogger.Msg($"[SpineSkin] 🎉 [{sourceCaller}] '{goName}'에 '{baseName}' 커스텀 스킨 적용 성공!");
        }
    }

    // 오브젝트가 처음 생성될 때(Init).
    [HarmonyPatch(typeof(SpineActionController), nameof(SpineActionController.Init), typeof(int), typeof(int))]
    internal static class Patch_Inject_Init
    {
        static void Postfix(SpineActionController __instance, int idx, int curScene)
        {
            InjectHelper.TryInject(__instance, "Init");
        }
    }

    // 스테이지 (재)시작마다 불릴 것으로 추정되는 지점.
    [HarmonyPatch(typeof(SpineActionController), nameof(SpineActionController.OnControllerStart))]
    internal static class Patch_Inject_OnControllerStart
    {
        static void Postfix(SpineActionController __instance)
        {
            InjectHelper.TryInject(__instance, "OnControllerStart");
        }
    }

    // 커스텀 스킨에 특정 애니메이션(예: atk_g_1, atk_p_2 등)이 누락되어 있을 때 Spine 예외(Animation not found)로 게임이 튕기는 것을 방지하는 안전 패치
    [HarmonyPatch(typeof(SpineActionController), nameof(SpineActionController.SetAnimation), typeof(string), typeof(bool))]
    internal static class Patch_SpineActionController_SetAnimation
    {
        private static readonly HashSet<string> LoggedMissingAnimations = new HashSet<string>();

        static bool Prefix(SpineActionController __instance, string n, bool isLoop)
        {
            if (__instance == null || string.IsNullOrEmpty(n)) return true;

            var ska = __instance.skeletonAnimation;
            if (ska == null || ska.Skeleton == null || ska.Skeleton.Data == null) return true;

            var anim = ska.Skeleton.Data.FindAnimation(n);
            if (anim == null)
            {
                if (LoggedMissingAnimations.Add(n))
                {
                    MelonLogger.Warning($"[SpineSkin] [{__instance.gameObject.name}] 누락된 애니메이션 '{n}' 재생 요청 감지 -> 예외 방지 조치 (스킵/폴백 적용)");
                }

                // 폴백 애니메이션 검색 (stand, idle, run)
                var fallbackAnim = ska.Skeleton.Data.FindAnimation("stand")
                                ?? ska.Skeleton.Data.FindAnimation("idle")
                                ?? ska.Skeleton.Data.FindAnimation("run");

                if (fallbackAnim != null && ska.AnimationState != null)
                {
                    try
                    {
                        ska.AnimationState.SetAnimation(0, fallbackAnim.Name, isLoop);
                    }
                    catch { }
                }

                return false;
            }

            return true;
        }
    }
}
