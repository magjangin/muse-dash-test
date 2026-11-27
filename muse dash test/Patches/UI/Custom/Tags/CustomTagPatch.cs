using MelonLoader;
using HarmonyLib;
using Il2Cpp;
using Il2CppAssets.Scripts.Database;
using System;

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
                // 스위치(EnableCustomTag)가 꺼져 있으면 태그·앨범·가상 곡을 아예 만들지 않아 곡 선택창이 바닐라와 같아집니다.
                if (!ModConfig.EnableCustomTag) return;

                // 가상 곡 주입 및 가상 앨범/태그 생성 비즈니스 로직을 모듈러 레지스트리로 이송하여 단 1줄로 초슬림 처리합니다.
                CustomTagRegistry.RegisterAll(__instance);
            }
        }
    }

    /// <summary>
    /// 커스텀 태그(1999)일 때 게임 내부의 RefreshStageDisplayMusics에서 발생하는 NullReferenceException을 우회합니다.
    /// -1(지금 태그)은 지금 태그가 커스텀일 때만 막습니다(판정은 <see cref="TagRefreshRules"/>).
    /// </summary>
    [HarmonyPatch(typeof(MusicTagManager), nameof(MusicTagManager.RefreshStageDisplayMusics), new Type[] { typeof(int) })]
    public class MusicTagManager_RefreshStageDisplayMusics_Patch
    {
        private static bool loggedError;

        public static bool Prefix(MusicTagManager __instance, int tagIndex)
        {
            try
            {
                ModLogger.Verbose($"[RefreshStageDisplayMusics Patch] Prefix called: tagIndex={tagIndex}");
                // -1(= 지금 태그)은 지금 태그가 커스텀일 때만 막습니다. 통째로 막으면 곡 선택 목록이 0곡으로 남습니다(TagRefreshRules).
                int? currentTag = null;
                if (tagIndex == TagRefreshRules.CurrentTag)
                {
                    var db = __instance?.m_DbMusicTag;
                    if (db != null) currentTag = db.m_CurSelectedTagIndex;
                }
                if (TagRefreshRules.ShouldSkipOriginal(tagIndex, currentTag))
                {
                    ModLogger.Verbose($"[RefreshStageDisplayMusics Patch] Bypassing: tagIndex={tagIndex}");
                    return false; // 원래 메서드 실행 차단
                }
                return true;
            }
            catch (Exception ex)
            {
                if (!loggedError)
                {
                    loggedError = true;
                    ModLogger.Warning($"[RefreshStageDisplayMusics Patch] 예외 (원본으로 복귀): {ex.Message}");
                }
                return true;
            }
        }
    }
}
