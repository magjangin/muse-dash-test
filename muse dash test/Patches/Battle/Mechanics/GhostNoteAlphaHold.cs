using System;
using System.Collections.Generic;
using MelonLoader;
using Il2Cpp;

namespace muse_dash_test
{
    // === 고스트 노트가 사라지지 않게 하기 ===
    //
    // 고스트 노트(UID zzxxyy의 xx=17, type 4)는 판정선에 가까워질수록 투명해집니다.
    // 원인은 C# 코드가 아니라 **Spine 애니메이션 데이터**입니다.
    //
    // 액션 계약서(SpineActionContract 덤프)로 확인한 고스트 노트의 액션은 셋뿐입니다.
    //   in          → in_nor_44     ← 비행 1.5초(dt=1.48) 동안 재생되는 유일한 애니메이션
    //   note_out_g  → out_g         ← 판정 시점(Great)
    //   note_out_p  → out_p         ← 판정 시점(Perfect)
    // 이 중 in_nor_44가 알파를 깎습니다. 그래서 `in`이 깔린 직후 그 애니메이션의 컬러 타임라인에서
    // 알파 키만 1로 덮어씁니다. 이동·스케일·회전 타임라인은 건드리지 않으므로 등장 모션은 원본 그대로입니다.
    //
    // 확인된 막다른 길(같은 곳을 다시 파지 않기 위해 남깁니다):
    //   - SpineActionController.SetAlpha(float), SpineActionController.OnNoteDisappear,
    //     BaseEnemyObjectController.NoteDisappearLogic → 고스트 노트에 대해 한 번도 호출되지 않습니다.
    //   - 애니메이션을 standby로 통째 교체하면 노트가 화면 중앙에 멈춥니다. 비행 이동도 in_nor_44에 들어 있습니다.
    //   - 알파/투명도 필드는 NoteConfigData·MusicData 어디에도 없어 BMS 주입·zz 복구 레이어에서는 손댈 수 없습니다.
    //
    // 왜 이렇게 고쳐지는지는 값으로 확인할 수 있습니다. 덮어쓰기 직전에 GhostNoteAlphaDiagnostics가
    // 원본 컬러 타임라인을 전부 찍고(키별 시간·rgba·커브), 덮어쓴 뒤 다시 읽어 검증까지 남깁니다.

    internal static class GhostNoteIdentity
    {
        private const uint GhostType = 4;
        private const string GhostXx = "17";

        /// <summary>
        /// 고스트 노트인지 판정합니다. `m_MusicData`가 비어 있는 컨트롤러도 있어서,
        /// 노트 데이터가 없으면 오브젝트 이름(`071701_road_nor_1(Clone)`)의 앞 6자리를 UID로 읽습니다.
        /// </summary>
        internal static bool IsGhost(SpineActionController controller)
        {
            if (controller == null) return false;

            try
            {
                var note = controller.m_MusicData?.noteData;
                if (note != null)
                {
                    return note.type == GhostType || IsGhostUid(note.uid);
                }
            }
            catch (Exception) { }

            try
            {
                string name = controller.gameObject != null ? controller.gameObject.name : null;
                return name != null && name.Length >= 6 && IsGhostUid(name.Substring(0, 6));
            }
            catch (Exception) { }

            return false;
        }

        private static bool IsGhostUid(string uid)
        {
            if (uid == null || uid.Length < 6) return false;
            for (int i = 0; i < 6; i++)
            {
                if (!char.IsDigit(uid[i])) return false;
            }
            return uid.Substring(2, 2) == GhostXx;
        }
    }

    /// <summary>
    /// 고스트 노트의 비행 애니메이션에서 알파 키를 걷어냅니다.
    /// `PlayByKey`는 `public` + `string`/`bool`이라 이 프로젝트에서 안전이 확인된 훅 모양입니다.
    /// </summary>
    [HarmonyLib.HarmonyPatch(typeof(SpineActionController), nameof(SpineActionController.PlayByKey))]
    public class SpineActionController_PlayByKey_GhostNote_Patch
    {
        /// <summary>노트가 날아오는 동안 재생되는 액션 키.</summary>
        private const string FlightActionKey = "in";

        /// <summary>SkeletonData는 같은 에셋을 쓰는 모든 노트가 공유하므로 애니메이션당 1회만 손봅니다.</summary>
        private static readonly HashSet<string> patchedAnimations = new HashSet<string>();

        public static void Postfix(SpineActionController __instance, string actionKey)
        {
            try
            {
                // 게이트 순서가 곧 성능입니다. PlayByKey는 캐릭터·모든 노트가 공유하는 뜨거운 경로라
                // 대부분의 호출이 첫 줄에서 끝나야 합니다. 설정을 끄면 진단 덤프도 같이 멈춥니다.
                if (!InputOverlay.showGhostNotes) return;
                if (!string.Equals(actionKey, FlightActionKey, StringComparison.Ordinal)) return;
                if (!GhostNoteIdentity.IsGhost(__instance)) return;

                StripAlphaFromCurrentAnimation(__instance);
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[GhostNote.AlphaTimeline] 알파 타임라인 처리 중 예외 발생: {ex}");
            }
        }

        /// <summary>지금 재생 중인 애니메이션에서 컬러 타임라인의 알파 키를 전부 1로 만듭니다.</summary>
        private static void StripAlphaFromCurrentAnimation(SpineActionController controller)
        {
            var skeleton = controller.skeletonAnimation != null ? controller.skeletonAnimation.skeleton : null;
            var data = skeleton != null ? skeleton.Data : null;
            if (data == null) return;

            string animationName = controller.currentAnimationName;
            if (string.IsNullOrEmpty(animationName)) return;
            if (!patchedAnimations.Add(animationName)) return;

            var animation = data.FindAnimation(animationName);
            if (animation == null)
            {
                MelonLogger.Msg($"[GhostNote.AlphaTimeline] 애니메이션을 찾지 못했습니다: {animationName}");
                return;
            }

            // 덮어쓰기 전에 원본을 찍어 둡니다. 한 번 덮으면 SkeletonData가 공유 데이터라 원본을 다시 볼 수 없습니다.
            GhostNoteAlphaDiagnostics.DumpOriginal(controller, data, animation, animationName);

            // 컬러 타임라인의 rgb는 텍스처에 곱하는 값이라, 실제로 보이는 색은 텍스처를 봐야 압니다.
            GhostNoteTextureProbe.Dump(controller);

            var timelines = animation.timelines;
            // ExposedList는 인덱서가 없습니다. 내부 배열(Items)이 Count보다 클 수 있어 둘 다 봅니다.
            var items = timelines != null ? timelines.Items : null;
            int count = timelines == null ? 0 : timelines.Count;
            if (items == null) return;
            if (count > items.Length) count = items.Length;

            int colorTimelines = 0;
            int alphaKeys = 0;

            for (int i = 0; i < count; i++)
            {
                if (!SpineColorFrames.TryDescribe(items[i], out var view)) continue;

                colorTimelines++;
                alphaKeys += ForceOpaque(view);
            }

            MelonLogger.Msg($"[GhostNote.AlphaTimeline] '{animationName}' 처리 완료: 타임라인 {count}개 중 컬러 {colorTimelines}개, " +
                            $"알파 키 {alphaKeys}개를 1로 고정 (이동/스케일 타임라인은 그대로)");

            GhostNoteAlphaDiagnostics.VerifyRewrite(animation, animationName, alphaKeys);
        }

        /// <summary>프레임 배열에서 알파 자리만 1로 덮어쓰고, 바꾼 키 개수를 돌려줍니다.</summary>
        private static int ForceOpaque(ColorTimelineView view)
        {
            var frames = view.Frames;
            if (frames == null) return 0;

            int changed = 0;
            for (int i = SpineColorFrames.AlphaOffset; i < frames.Length; i += view.Entries)
            {
                if (frames[i] >= SpineColorFrames.OpaqueThreshold) continue;
                frames[i] = 1f;
                changed++;
            }
            return changed;
        }
    }
}
