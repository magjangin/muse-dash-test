using System;
using HarmonyLib;
using Il2CppAssets.Scripts.GameCore.GameObjectLogics.GameObjectControl;
using UnityEngine;

namespace muse_dash_test.Patches.Battle.Mechanics
{
    /// <summary>
    /// 고스트 노트(유령 노트)의 알파값을 항상 1.0(완전 불투명)으로 고정하여
    /// 투명하게 연출되는 노트를 실체화하여 명확히 보이도록 보정하는 테스트용 패치입니다.
    /// </summary>
    [HarmonyPatch(typeof(NormalNoteVisibleController), nameof(NormalNoteVisibleController.OnAppear))]
    public static class GhostNoteVisiblePatch
    {
        public static void Postfix(NormalNoteVisibleController __instance)
        {
            if (__instance == null) return;

            try
            {
                // Spine 애니메이션 렌더러 알파값 100% 고정
                if (__instance.m_SkeletonAnimation != null && __instance.m_SkeletonAnimation.skeleton != null)
                {
                    __instance.m_SkeletonAnimation.skeleton.A = 1.0f;
                }

                // SpriteRenderer 알파값 100% 고정
                var spriteRenderer = __instance.GetComponent<SpriteRenderer>();
                if (spriteRenderer != null)
                {
                    Color color = spriteRenderer.color;
                    color.a = 1.0f;
                    spriteRenderer.color = color;
                }
            }
            catch (Exception ex)
            {
                MelonLoader.MelonLogger.Error($"[GhostNoteVisiblePatch] Error in Postfix: {ex}");
            }
        }
    }
}
