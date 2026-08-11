using System;
using System.Collections.Generic;
using MelonLoader;
using UnityEngine;

namespace muse_dash_test
{
    /// <summary>
    /// 롱노트가 라이더처럼 궤적을 그리며 날아오게 만듭니다.
    /// <para>판정에는 전혀 손대지 않습니다. 게임의 판정은 tick + 레인(공중/지상) 불린만 보므로
    /// (<c>GameTouchPlay.LongNoteCheck(bool isAir, int headIdx)</c>), 좌표를 어떻게 흔들어도 판정은 동일합니다.
    /// 이 모듈은 순수하게 보이는 위치만 바꿉니다.</para>
    /// <para><b>왜 부모 래퍼를 움직이는가:</b> 프로브 관측 결과 지상 롱노트는 노트 자신의 localPosition.y를 써도
    /// 유지됐지만, 공중 롱노트는 게임이 매 프레임 y를 원래 값으로 덮어썼습니다. 노트 계층이
    /// <c>SceneObjectController / {prefab}{idx} / {prefab}(Clone){idx}</c> 구조라, 게임이 건드리는 자식 대신
    /// 가운데 래퍼를 움직이면 두 레인 모두에서 오프셋이 살아남습니다.</para>
    /// </summary>
    public static class LongNoteTrajectory
    {
        /// <summary>기능 on/off.</summary>
        public static bool Enabled = true;

        /// <summary>궤적의 최대 높이(월드 단위). 프로브 기준 0.6도 눈에 뚜렷하게 보였습니다.</summary>
        public static float Amplitude = 1.0f;

        /// <summary>궤적 모양.</summary>
        public static TrajectoryShape Shape = TrajectoryShape.Arc;

        /// <summary><see cref="TrajectoryShape.Wave"/>일 때 등장~판정 구간에서 반복할 파형 수.</summary>
        public static float WaveCount = 1.5f;

        /// <summary>노트가 등장하는 x. 프로브 관측값(10).</summary>
        public static float SpawnX = 10f;

        /// <summary>판정선 x. 여기서 오프셋이 0으로 돌아와 레인 높이에 정확히 안착합니다.</summary>
        public static float HitX = 0f;

        /// <summary>진행도-오프셋 곡선 종류.</summary>
        public enum TrajectoryShape
        {
            /// <summary>등장 때 0 → 중간에 최고점 → 판정선에서 다시 0. 라이더가 호를 그리며 내려앉는 느낌.</summary>
            Arc,
            /// <summary>진행 내내 물결. 판정선에서는 0으로 수렴합니다.</summary>
            Wave,
        }

        /// <summary>검증 로그를 남길 노트 수(로그 폭발 방지).</summary>
        private const int MaxLoggedNotes = 2;

        /// <summary>검증 로그 간격(프레임).</summary>
        private const int LogIntervalFrames = 10;

        /// <summary>래퍼로 인정할 최대 자식 수. 이보다 많으면 여러 노트가 공유하는 컨테이너로 보고 건드리지 않습니다.</summary>
        private const int MaxWrapperChildren = 4;

        private sealed class Tracked
        {
            public Il2Cpp.LongPressController Note;
            public Transform NoteTransform;
            public Transform Target;      // 실제로 오프셋을 쓰는 대상(부모 래퍼, 없으면 노트 자신)
            public Vector3 TargetBase;    // 오프셋을 더하기 전 원래 좌표
            public bool TargetIsWrapper;
            public int Id;
            public bool Verbose;
            public int NextLogFrame;
            public float LastX;
            public float LastOffset;
        }

        private static readonly List<Tracked> tracked = new List<Tracked>();
        private static int noteSeq;

        /// <summary>씬 전환 시 추적 상태를 비웁니다.</summary>
        public static void Reset()
        {
            // 남아 있는 대상이 있으면 원래 좌표로 되돌려 둡니다(풀 재사용 시 오프셋이 새 노트에 묻어가는 것 방지).
            for (int i = 0; i < tracked.Count; i++)
            {
                RestoreBase(tracked[i]);
            }

            tracked.Clear();
            noteSeq = 0;
        }

        /// <summary>LongPressController.OnControllerStart Postfix에서 호출됩니다. 체인 머리만 추적합니다.</summary>
        public static void OnLongPressStart(Il2Cpp.LongPressController ctrl)
        {
            if (!Enabled || ctrl == null) return;

            Il2CppGameLogic.MusicData md = GetMusicData(ctrl);
            if (!IsChainHead(md)) return;

            Transform noteTransform = SafeTransform(ctrl);
            if (noteTransform == null) return;

            Transform target = ResolveOffsetTarget(noteTransform, out bool isWrapper);
            if (target == null) return;

            noteSeq++;
            var entry = new Tracked
            {
                Note = ctrl,
                NoteTransform = noteTransform,
                Target = target,
                TargetBase = target.localPosition,
                TargetIsWrapper = isWrapper,
                Id = noteSeq,
                Verbose = noteSeq <= MaxLoggedNotes,
                NextLogFrame = Time.frameCount,
            };
            tracked.Add(entry);

            if (entry.Verbose)
            {
                MelonLogger.Msg(
                    $"[LongNoteTrajectory] #{entry.Id} 추적 시작: target='{target.name}'({(isWrapper ? "부모 래퍼" : "노트 자신")}), " +
                    $"base=({entry.TargetBase.x:0.###},{entry.TargetBase.y:0.###}), shape={Shape}, amp={Amplitude:0.##}, " +
                    $"pathway={SafePathway(md)}, uid={SafeUid(md)}");
            }
        }

        /// <summary>
        /// MelonMod.OnLateUpdate에서 호출됩니다. 게임이 이번 프레임의 x를 확정한 뒤에 y를 얹어야
        /// 진행도와 궤적이 어긋나지 않습니다.
        /// </summary>
        public static void LateUpdate()
        {
            if (!Enabled || tracked.Count == 0) return;

            int frame = Time.frameCount;
            for (int i = tracked.Count - 1; i >= 0; i--)
            {
                Tracked entry = tracked[i];
                if (!Apply(entry, frame))
                {
                    RestoreBase(entry);
                    tracked.RemoveAt(i);
                }
            }
        }

        /// <summary>한 노트의 궤적 오프셋을 갱신합니다. 노트가 사라졌으면 false를 돌립니다.</summary>
        private static bool Apply(Tracked entry, int frame)
        {
            Transform noteTransform = entry.NoteTransform;
            Transform target = entry.Target;
            if (noteTransform == null || target == null) return false;

            float x;
            try
            {
                if (!noteTransform.gameObject.activeInHierarchy) return false;
                x = noteTransform.position.x;
            }
            catch
            {
                return false;
            }

            float progress = Progress(x);
            float offset = Offset(progress);

            try
            {
                target.localPosition = new Vector3(
                    entry.TargetBase.x,
                    entry.TargetBase.y + offset,
                    entry.TargetBase.z);
            }
            catch
            {
                return false;
            }

            entry.LastX = x;
            entry.LastOffset = offset;

            if (entry.Verbose && frame >= entry.NextLogFrame)
            {
                entry.NextLogFrame = frame + LogIntervalFrames;
                float actualY;
                try { actualY = noteTransform.position.y; } catch { actualY = float.NaN; }

                MelonLogger.Msg(
                    $"[LongNoteTrajectory] #{entry.Id} frame={frame}: x={x:0.###}, 진행도={progress:0.###}, " +
                    $"오프셋={offset:0.###}, 노트실제y={actualY:0.###}");
            }

            return true;
        }

        /// <summary>등장 x → 판정 x 구간의 진행도(0~1).</summary>
        private static float Progress(float x)
        {
            float span = SpawnX - HitX;
            if (Mathf.Abs(span) < 0.0001f) return 0f;
            return Mathf.Clamp01((SpawnX - x) / span);
        }

        /// <summary>진행도 → y 오프셋. 판정선(진행도 1)에서는 항상 0으로 돌아옵니다.</summary>
        private static float Offset(float progress)
        {
            switch (Shape)
            {
                case TrajectoryShape.Wave:
                    // 판정선 근처에서 0으로 수렴하도록 (1 - progress)로 감쇠시킵니다.
                    return Amplitude * Mathf.Sin(progress * Mathf.PI * 2f * WaveCount) * (1f - progress);

                case TrajectoryShape.Arc:
                default:
                    return Amplitude * Mathf.Sin(progress * Mathf.PI);
            }
        }

        /// <summary>
        /// 오프셋을 쓸 대상을 고릅니다. 부모가 이 노트만 담는 래퍼로 보이면 부모를,
        /// 아니면(여러 노트를 담는 공용 컨테이너 등) 노트 자신을 씁니다.
        /// </summary>
        private static Transform ResolveOffsetTarget(Transform noteTransform, out bool isWrapper)
        {
            isWrapper = false;
            try
            {
                Transform parent = noteTransform.parent;
                if (parent == null) return noteTransform;

                // SceneObjectController처럼 씬 전체가 매달린 노드를 움직이면 화면이 통째로 흔들립니다.
                if (parent.childCount > MaxWrapperChildren) return noteTransform;
                if (parent.parent == null) return noteTransform;

                isWrapper = true;
                return parent;
            }
            catch
            {
                isWrapper = false;
                return noteTransform;
            }
        }

        /// <summary>대상 좌표를 원래대로 되돌립니다(오브젝트 풀 재사용 대비).</summary>
        private static void RestoreBase(Tracked entry)
        {
            if (entry?.Target == null) return;
            try
            {
                entry.Target.localPosition = entry.TargetBase;
            }
            catch { }

            if (entry.Verbose)
            {
                MelonLogger.Msg(
                    $"[LongNoteTrajectory] #{entry.Id} 추적 종료: 마지막 x={entry.LastX:0.###}, " +
                    $"마지막 오프셋={entry.LastOffset:0.###} (HitX 보정 참고값)");
            }
        }

        // ── 헬퍼 ────────────────────────────────────────────────────────────────

        private static bool IsChainHead(Il2CppGameLogic.MusicData md)
        {
            if (md == null) return false;
            try
            {
                if (md.isLongPressing || md.isLongPressEnd) return false;
                return md.configData != null
                    && global::DBStageInfo_SetRuntimeMusicData_Patch.ParseMusicDecimal(md.configData.length) > 0.0;
            }
            catch
            {
                return false;
            }
        }

        private static Il2CppGameLogic.MusicData GetMusicData(Il2Cpp.LongPressController ctrl)
        {
            try
            {
                var md = ctrl.MusicData;
                if (md != null) return md;
            }
            catch { }

            try { return ctrl.PMusicData; }
            catch { return null; }
        }

        private static Transform SafeTransform(Il2Cpp.LongPressController ctrl)
        {
            try
            {
                Transform t = ctrl.transform;
                return t != null ? t : null;
            }
            catch
            {
                return null;
            }
        }

        private static string SafeUid(Il2CppGameLogic.MusicData md)
        {
            try { return md?.noteData?.uid ?? "(null)"; }
            catch { return "(접근 불가)"; }
        }

        private static string SafePathway(Il2CppGameLogic.MusicData md)
        {
            try { return md?.noteData != null ? md.noteData.pathway.ToString() : "?"; }
            catch { return "?"; }
        }
    }

    /// <summary>롱노트 체인 머리가 등장하는 시점을 잡아 궤적 추적에 등록합니다.</summary>
    [HarmonyLib.HarmonyPatch(typeof(Il2Cpp.LongPressController), "OnControllerStart")]
    public class LongPressController_OnControllerStart_Trajectory
    {
        public static void Postfix(Il2Cpp.LongPressController __instance)
        {
            try
            {
                LongNoteTrajectory.OnLongPressStart(__instance);
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[LongNoteTrajectory] OnControllerStart Postfix 예외: {ex}");
            }
        }
    }
}
