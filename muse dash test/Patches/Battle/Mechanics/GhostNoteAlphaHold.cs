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

        /// <summary>고스트 노트임을 가리키는 NoteConfigData.type 값.</summary>
        private const uint GhostType = 4;

        private static float lastScanTime;
        private static int heldCount;
        private static int killedTweenCount;
        private static bool announced;
        private static DateTime lastSummaryTime = DateTime.MinValue;
        private static float lastHeartbeatTime;
        private static int sampleBudget = 24;

        internal static void HoldAlpha()
        {
            if (!InputOverlay.showGhostNotes) return;
            if (Time.unscaledTime - lastScanTime < ScanIntervalSeconds) return;
            lastScanTime = Time.unscaledTime;

            // 비활성(풀 대기)까지 포함해 훑습니다. 활성만 보는 FindObjectsOfType으로는
            // 풀링된 노트를 놓칩니다(set_NoteMData 프로브에서 확인된 함정).
            var found = Resources.FindObjectsOfTypeAll(Il2CppType.Of<NoteObjectController>());
            int total = found == null ? 0 : found.Length;
            int ghostCount = 0;

            for (int i = 0; i < total; i++)
            {
                try
                {
                    var controller = found[i]?.TryCast<NoteObjectController>();
                    if (controller == null || !IsGhostNote(controller)) continue;

                    ghostCount++;
                    SampleGhostState(controller);
                    KillFadeTween(controller);
                    ForceOpaque(controller.m_SkeletonAnimation);
                }
                catch (Exception ex)
                {
                    MelonLogger.Error($"[GhostNote.AlphaHold] 노트 처리 중 예외 발생: {ex}");
                }
            }

            LogHeartbeatIfDue(total, ghostCount);
            LogSummaryIfDue();
        }

        /// <summary>
        /// UID(xx=17) 또는 type(4) 어느 쪽으로든 고스트로 판정합니다.
        /// 실측에서 프리팹이 일반 노트(`_road_nor_1`)로 굴러가는데도 사라졌으므로,
        /// 페이드가 프리팹이 아니라 type에서 나올 가능성을 함께 덮습니다.
        /// </summary>
        private static bool IsGhostNote(NoteObjectController controller)
        {
            var md = controller.m_MusicData;
            if (md == null) return false;

            var note = md.noteData;
            if (note == null) return false;

            if (note.type == GhostType) return true;

            string uid = note.uid;
            return uid != null && uid.Length == 6 && uid.Substring(2, 2) == GhostXx;
        }

        /// <summary>
        /// "사라진다"의 정체를 가리기 위한 상태 표본입니다. 알파가 깎이는 것인지,
        /// 오브젝트가 비활성화되는 것인지, 렌더러가 꺼지는 것인지가 여기서 갈립니다.
        /// </summary>
        private static void SampleGhostState(NoteObjectController controller)
        {
            if (sampleBudget <= 0) return;
            sampleBudget--;

            string uid = "(null)";
            uint type = 0;
            try
            {
                var note = controller.m_MusicData?.noteData;
                if (note != null) { uid = note.uid ?? "(null)"; type = note.type; }
            }
            catch (Exception) { }

            string alpha = "(스켈레톤 없음)";
            try
            {
                var sk = controller.m_SkeletonAnimation;
                if (sk != null && sk.skeleton != null) alpha = sk.skeleton.A.ToString("0.###");
            }
            catch (Exception) { }

            int exCount = -1;
            bool hasTweener = false;
            try
            {
                var ex = controller.m_ExNoteControllers;
                exCount = ex == null ? -1 : ex.Count;
                for (int i = 0; i < (ex == null ? 0 : ex.Count); i++)
                {
                    var visible = ex[i]?.TryCast<NoteVisibleController>();
                    if (visible != null && visible.m_NoteTweener != null) hasTweener = true;
                }
            }
            catch (Exception) { }

            bool activeSelf = false, activeInHierarchy = false;
            try
            {
                var go = controller.gameObject;
                if (go != null) { activeSelf = go.activeSelf; activeInHierarchy = go.activeInHierarchy; }
            }
            catch (Exception) { }

            MelonLogger.Msg($"[GhostNote.AlphaHold.Sample] uid={uid}, type={type}, alpha={alpha}, exControllers={exCount}, " +
                            $"fadeTweener={hasTweener}, activeSelf={activeSelf}, activeInHierarchy={activeInHierarchy}, showTick={controller.m_ShowTick}");
        }

        private static void LogHeartbeatIfDue(int total, int ghostCount)
        {
            if (Time.unscaledTime - lastHeartbeatTime < 2f) return;
            lastHeartbeatTime = Time.unscaledTime;

            MelonLogger.Msg($"[GhostNote.AlphaHold] 스캔: 노트 컨트롤러 {total}개, 그중 고스트 {ghostCount}개 " +
                            $"(누적 알파 복구 {heldCount}, 트윈 제거 {killedTweenCount})");
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
