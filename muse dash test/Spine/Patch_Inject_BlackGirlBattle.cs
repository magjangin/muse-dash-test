using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using HarmonyLib;
using MelonLoader;
using Il2Cpp;
using Il2CppSpine.Unity;
using UnityEngine;

namespace muse_dash_test
{
    // 스테이지에서 특정 GameObject 이름의 캐릭터/고스트를 커스텀 스킨으로 교체한다.
    // 그림자는 대상에서 제외한다(사용자 요청으로 원래대로 유지).
    // (원래 spine skin 모드에서 이식: 주입 기능만 가져옴)
    internal static class InjectHelper
    {
        // GameObject 이름 -> skin test/character 폴더의 파일 베이스 이름({baseName}.png/.atlas/.json)
        private static readonly Dictionary<string, string> TargetToBaseName = new Dictionary<string, string>(StringComparer.Ordinal)
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

        public static void TryInject(SpineActionController instance)
        {
            // EnableSpineSkin의 유일한 집행 지점입니다. 주입 훅이 Init/OnControllerStart 두 곳이라
            // 각 훅에 검사를 흩어 두면 하나를 빠뜨리기 쉬우므로, 두 훅이 공유하는 여기서 한 번만 봅니다.
            // (예전에는 MainMod의 'skin test' 폴더 생성만 이 설정에 묶여 있어서, 설정을 꺼도
            //  스킨 주입은 그대로 돌았습니다.)
            if (!ModConfig.EnableSpineSkin) return;
            // config.txt [8] '캐릭터 커스텀 스킨=false'면 모든 캐릭터를 원래 모습으로 둡니다.
            // 배틀 중에는 config.txt를 다시 읽지 않으므로 판 도중에 바뀌지 않습니다(LoadConfigIfNeeded).
            // 이 함수는 노트가 생길 때마다(SpineActionController.Init) 불리므로, 이름(IL2CPP 문자열 마샬링)을 읽기 전에 봅니다.
            if (!InputOverlay.enableCharacterSkin) return;
            if (instance is null) return;

            try
            {
                TryInjectCore(instance);
            }
            catch (Exception ex)
            {
                ModLogger.Warning($"[Inject] 캐릭터 스킨 주입 예외: {ex.Message}");
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void TryInjectCore(SpineActionController instance)
        {
            if (instance == null) return;
            var go = instance.gameObject;
            if (go == null) return;
            string objName = go.name;
            if (objName == null || !TargetToBaseName.TryGetValue(objName, out var baseName)) return;

            var ska = instance.skeletonAnimation;
            if (ska == null)
            {
                ModLogger.Msg("[Inject] skeletonAnimation이 null이라 주입 스킵");
                return;
            }

            // 원본 아틀라스 머티리얼을 그대로 복사해 텍스처만 바꿉니다. 분신(_ghost)은 전용 셰이더로 반투명 하늘색으로
            // 그려지는데, 이 머티리얼을 버리면 본체처럼 불투명하게 나옵니다(PrimaryMaterialOf 주석).
            // OnControllerStart에서 다시 불릴 때는 이미 우리 에셋이라 원본을 읽지 않고 캐시(역할별)를 씁니다.
            var current = ska.skeletonDataAsset;
            Material source = current != null && !CustomSkinInjector.IsCachedAsset(current)
                ? CustomSkinInjector.PrimaryMaterialOf(current)
                : null;
            string role = SkinFolderRules.CharacterRole(objName);

            var customAsset = CustomSkinInjector.GetOrBuild(baseName, role, source);
            if (customAsset == null)
            {
                ModLogger.Msg($"[Inject] {baseName} customAsset이 null이라 주입 스킵");
                return;
            }

            ska.skeletonDataAsset = customAsset;
            ska.Initialize(true);
            ModLogger.Msg($"[Inject] {objName}에 {baseName} 스킨 주입 완료 ({role}, {CustomSkinInjector.DescribeMaterial(CustomSkinInjector.PrimaryMaterialOf(customAsset))})");
        }
    }

    // 오브젝트가 처음 생성될 때(Init).
    [HarmonyPatch(typeof(SpineActionController), nameof(SpineActionController.Init), typeof(int), typeof(int))]
    internal static class Patch_Inject_Init
    {
        static bool Prepare() => ModConfig.EnableSpineSkin || SpineActionContract.IsDumpEnabled;

        static void Postfix(SpineActionController __instance, int idx, int curScene)
        {
            InjectHelper.TryInject(__instance);

            // 커스텀 곡의 노트 스킨(곡 폴더 note skin/). 같은 메서드에 패치 클래스를 따로 붙이지 않고 여기에 얹습니다.
            // OnControllerStart 쪽에는 넣지 않습니다(재생 중 Initialize가 상태를 지움). NoteSkinInjector 주석 참고.
            NoteSkinInjector.TryInject(__instance);

            // 액션 계약서 프로브(읽기 전용). 주입 직후라 커스텀 스켈레톤의 애니메이션 목록이 잡힙니다.
            // 아래 OnControllerStart 쪽에도 같은 호출이 있지만, 이름 단위로 1회만 덤프하므로 중복되지 않습니다.
            SpineActionContract.DumpSupply(__instance);
        }
    }

    // 스테이지 (재)시작마다 불릴 것으로 추정되는 지점. 재시도/재진입 시에도 주입되도록 커버.
    [HarmonyPatch(typeof(SpineActionController), nameof(SpineActionController.OnControllerStart))]
    internal static class Patch_Inject_OnControllerStart
    {
        static bool Prepare() => ModConfig.EnableSpineSkin || SpineActionContract.IsDumpEnabled;
        static void Postfix(SpineActionController __instance)
        {
            InjectHelper.TryInject(__instance);

            // 액션 계약서 프로브(읽기 전용). 같은 메서드에 별도 패치 클래스를 붙이면 그쪽 Postfix가
            // 실행되지 않아서, 이미 도는 이 지점에 얹었습니다. 자세한 내용은 SpineActionContract 주석 참고.
            // 주입 이후에 호출해야 커스텀 스켈레톤의 애니메이션 목록이 잡힙니다.
            SpineActionContract.DumpSupply(__instance);
        }
    }
}
