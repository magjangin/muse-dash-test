using System;
using MelonLoader;
using UnityEngine;
using Il2CppInterop.Runtime;
using NoteObjectController = Il2Cpp.BaseEnemyObjectController;
using NoteVisibleController = Il2CppAssets.Scripts.GameCore.GameObjectLogics.GameObjectControl.NormalNoteVisibleController;

namespace muse_dash_test
{
    // === 고스트 노트 알파 고정 (설정 파일 연동) ===
    //
    // 고스트 노트(UID xx=17, type 4)는 판정선에 가까워질수록 알파가 깎여 사라집니다.
    // 목적은 고스트 고유 외형을 유지한 채 그 노트가 계속 보이게 만드는 것이라,
    // 프리팹을 일반 노트로 갈아끼우는 우회 대신 런타임에 알파를 되돌립니다.
    //
    // 후킹하지 않습니다. 페이드를 만드는 NormalNoteVisibleController의 OnAppear/Init/OnUpdate가
    // 전부 virtual이고 이 프로젝트에서 virtual/private 훅은 로그도 없는 네이티브 크래시를 냈습니다.
    // 한 줄짜리 세터는 IL2CPP가 인라인해서 훅이 아예 돌지 않습니다(set_NoteMData로 확인).
    // 그래서 살아 있는 노트 오브젝트를 주기적으로 훑는 방식을 씁니다.
    //
    // 처리 순서가 중요합니다.
    //   1. 페이드 트윈(m_NoteTweener)을 먼저 죽인다  → 이후 알파를 다시 깎을 주체가 없어짐
    //   2. 스켈레톤 알파를 1로 되돌린다              → 이미 깎인 만큼을 복구
    // 트윈을 죽이지 않고 알파만 밀면 매 프레임 서로 덮어써 깜빡입니다.

    internal static class GhostNoteAlphaHold
    {
        /// <summary>고스트 노트를 가리키는 UID 중간 두 자리(zzxxyy의 xx).</summary>
        private const string GhostXx = "17";

        /// <summary>노트는 1.5초 내외를 날아가므로 20Hz면 등장 직후에 바로 잡힙니다.</summary>
        private const float ScanIntervalSeconds = 0.05f;

        private static float lastScanTime;
        private static int heldCount;
        private static int killedTweenCount;
        private static bool announced;
        private static DateTime lastSummaryTime = DateTime.MinValue;

        internal static void HoldAlpha()
        {
            if (!InputOverlay.showGhostNotes) return;
            if (Time.unscaledTime - lastScanTime < ScanIntervalSeconds) return;
            lastScanTime = Time.unscaledTime;

            // 화면에 있는(활성) 노트만 대상입니다. 풀에서 대기 중인 비활성 노트는 아직 알파가 의미 없습니다.
            var found = UnityEngine.Object.FindObjectsOfType(Il2CppType.Of<NoteObjectController>());
            if (found == null) return;

            for (int i = 0; i < found.Length; i++)
            {
                try
                {
                    var controller = found[i]?.TryCast<NoteObjectController>();
                    if (controller == null || !IsGhostNote(controller)) continue;

                    KillFadeTween(controller);
                    ForceOpaque(controller.m_SkeletonAnimation);
                }
                catch (Exception ex)
                {
                    MelonLogger.Error($"[GhostNote.AlphaHold] 노트 처리 중 예외 발생: {ex}");
                }
            }

            LogSummaryIfDue();
        }

        private static bool IsGhostNote(NoteObjectController controller)
        {
            var md = controller.m_MusicData;
            if (md == null) return false;

            var note = md.noteData;
            string uid = note == null ? null : note.uid;
            return uid != null && uid.Length == 6 && uid.Substring(2, 2) == GhostXx;
        }

        /// <summary>
        /// 노트에 붙은 Ex 컨트롤러 중 페이드 담당(NormalNoteVisibleController)의 트윈을 죽입니다.
        /// 알파를 깎는 주체가 사라지므로 이후 복구값이 유지됩니다.
        /// </summary>
        private static void KillFadeTween(NoteObjectController controller)
        {
            var exControllers = controller.m_ExNoteControllers;
            if (exControllers == null) return;

            for (int i = 0; i < exControllers.Count; i++)
            {
                var visible = exControllers[i]?.TryCast<NoteVisibleController>();
                if (visible == null) continue;

                var tweener = visible.m_NoteTweener;
                if (tweener == null) continue;

                Il2CppDG.Tweening.TweenExtensions.Kill(tweener, false);
                visible.m_NoteTweener = null;
                killedTweenCount++;
            }
        }

        /// <summary>스켈레톤 알파를 불투명으로 되돌립니다.</summary>
        private static void ForceOpaque(Il2CppSpine.Unity.SkeletonAnimation skeletonAnimation)
        {
            if (skeletonAnimation == null) return;

            var skeleton = skeletonAnimation.skeleton;
            if (skeleton == null) return;
            if (skeleton.A >= 0.999f) return;

            skeleton.A = 1f;
            heldCount++;

            if (!announced)
            {
                announced = true;
                MelonLogger.Msg("[GhostNote.AlphaHold] 고스트 노트 알파 복구 첫 적용 (판정선 근처 페이드 무효화)");
            }
        }

        private static void LogSummaryIfDue()
        {
            if (heldCount == 0 && killedTweenCount == 0) return;
            if ((DateTime.UtcNow - lastSummaryTime).TotalSeconds < 10.0) return;

            lastSummaryTime = DateTime.UtcNow;
            MelonLogger.Msg($"[GhostNote.AlphaHold] 누적 알파 복구 {heldCount}회, 페이드 트윈 제거 {killedTweenCount}회");
        }
    }
}
