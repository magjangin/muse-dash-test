using MelonLoader;
using HarmonyLib;
using Il2Cpp;
using Il2CppAssets.Scripts.Database;

namespace muse_dash_test
{
    /// <summary>
    /// MusicTagManager 패치를 통해 커스텀 태그 주입 흐름을 안전하게 가로챕니다.
    /// </summary>
    internal partial class CustomTagPatch
    {
        /// <summary>
        /// 런타임에 게임 데이터베이스에 우리의 "실험 모드" 커스텀 태그 카테고리를 동적으로 주입합니다.
        /// </summary>
        [HarmonyPatch(typeof(MusicTagManager), nameof(MusicTagManager.InitAlbumTagInfo))]
        internal partial class MusicTagPatch
        {
            private static void Postfix(MusicTagManager __instance)
            {
                // 가상 곡 주입 및 가상 앨범/태그 생성 비즈니스 로직을 모듈러 레지스트리로 이송하여 단 1줄로 초슬림 처리합니다.
                CustomTagRegistry.RegisterAll(__instance);
            }
        }
    }
}
