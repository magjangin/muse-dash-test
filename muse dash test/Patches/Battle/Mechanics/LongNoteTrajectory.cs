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
    public static partial class LongNoteTrajectory
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

        /// <summary>몸통 처리 방식.</summary>
        public static BodyMode Body = BodyMode.Curved;

        /// <summary>진행도-오프셋 곡선 종류.</summary>
        public enum TrajectoryShape
        {
            /// <summary>등장 때 0 → 중간에 최고점 → 판정선에서 다시 0. 라이더가 호를 그리며 내려앉는 느낌.</summary>
            Arc,
            /// <summary>진행 내내 물결. 판정선에서는 0으로 수렴합니다.</summary>
            Wave,
        }

        /// <summary>롱노트 몸통(막대)을 어떻게 처리할지.</summary>
        public enum BodyMode
        {
            /// <summary>원본 막대를 그대로 두고 노트 전체를 곡선 위에 올립니다. 막대는 수평을 유지합니다.</summary>
            Rigid,
            /// <summary>원본 막대를 숨기고 머리~꼬리를 곡선 리본으로 다시 그립니다. 몸통이 실제로 휩니다.</summary>
            Curved,
        }

        /// <summary>검증 로그를 남길 노트 수(로그 폭발 방지).</summary>
        private const int MaxLoggedNotes = 2;

        /// <summary>검증 로그 간격(프레임).</summary>
        private const int LogIntervalFrames = 10;

        /// <summary>래퍼로 인정할 최대 자식 수. 이보다 많으면 여러 노트가 공유하는 컨테이너로 보고 건드리지 않습니다.</summary>
        private const int MaxWrapperChildren = 4;

        /// <summary>
        /// 노트가 비활성인 상태를 견디는 유예 프레임 수.
        /// <para>관측: OnControllerStart 시점에 노트 GameObject는 아직 <c>active=False</c>이고 한두 프레임 뒤 활성화됩니다.
        /// 첫 프레임의 비활성을 "사라짐"으로 처리하면 활성화가 늦은 노트(공중 롱노트에서 관측)를 통째로 놓칩니다.</para>
        /// </summary>
        private const int InactiveGraceFrames = 180;

        /// <summary>판정선을 이만큼 지나면 추적을 끝냅니다(롱노트 꼬리 길이 여유 포함).</summary>
        private const float ExitMargin = 4f;

        private sealed class Tracked
        {
            public Il2Cpp.LongPressController Note;
            public Transform NoteTransform;
            public Transform Target;      // 실제로 오프셋을 쓰는 대상
            public Vector3 TargetBase;    // 오프셋을 더하기 전 원래 좌표
            public string TargetKind;     // 어느 층을 잡았는지(로그용)
            public int Id;
            public bool Verbose;
            public int NextLogFrame;
            public float LastX;
            public float LastOffset;
            public int InactiveFrames;
            public string EndReason;

            // ── Curved 모드 전용 ──
            public RibbonState Ribbon;
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

            Transform target = ResolveOffsetTarget(noteTransform, out string targetKind);
            if (target == null) return;

            // 같은 래퍼를 이미 다른 항목이 잡고 있으면(오브젝트 풀 재사용) 먼저 정리합니다.
            // 두 항목이 한 좌표를 두고 다투면 노트가 떨리거나 오프셋이 누적됩니다.
            for (int i = tracked.Count - 1; i >= 0; i--)
            {
                if (tracked[i].Target != target) continue;

                tracked[i].EndReason = "래퍼 재사용";
                RestoreBase(tracked[i]);
                tracked.RemoveAt(i);
            }

            noteSeq++;
            var entry = new Tracked
            {
                Note = ctrl,
                NoteTransform = noteTransform,
                Target = target,
                TargetBase = target.localPosition,
                TargetKind = targetKind,
                Id = noteSeq,
                Verbose = noteSeq <= MaxLoggedNotes,
                NextLogFrame = Time.frameCount,
            };

            if (Body == BodyMode.Curved)
            {
                // 곡선 리본이 준비되면 몸통 노드는 원위치에 두고 머리/꼬리만 각자 높이로 올립니다.
                entry.Ribbon = AttachRibbon(entry.Id, noteTransform, target, entry.Verbose);
            }
            tracked.Add(entry);

            if (entry.Verbose)
            {
                MelonLogger.Msg(
                    $"[LongNoteTrajectory] #{entry.Id} 추적 시작: target='{target.name}'({targetKind}), " +
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
            if (noteTransform == null || target == null)
            {
                entry.EndReason = "오브젝트 소멸";
                return false;
            }

            bool active;
            float x;
            try
            {
                active = noteTransform.gameObject.activeInHierarchy;
                x = noteTransform.position.x;
            }
            catch
            {
                entry.EndReason = "접근 불가";
                return false;
            }

            if (!active)
            {
                // 등장 직후(아직 활성화 전)이거나 풀에 반납된 뒤입니다. 둘 다 여기로 들어오므로
                // 바로 버리지 않고 유예를 둡니다. 보이지 않는 동안은 오프셋 0으로 눕혀 둡니다.
                entry.InactiveFrames++;
                SetOffset(entry, 0f);

                if (entry.InactiveFrames <= InactiveGraceFrames) return true;

                entry.EndReason = "비활성 유예 초과";
                return false;
            }

            entry.InactiveFrames = 0;

            if (x < HitX - ExitMargin)
            {
                entry.EndReason = "판정선 통과";
                return false;
            }

            float progress = Progress(x);
            float offset = Offset(progress);

            if (entry.Ribbon != null)
            {
                // 몸통이 휘는 모드: 몸통 노드는 원위치 유지, 머리/꼬리/리본이 각자 자기 x의 높이를 따릅니다.
                if (!SetOffset(entry, 0f) || !UpdateRibbon(entry.Ribbon, x))
                {
                    entry.EndReason = "리본 갱신 실패";
                    return false;
                }

                entry.LastOffset = offset;
            }
            else if (!SetOffset(entry, offset))
            {
                entry.EndReason = "좌표 쓰기 실패";
                return false;
            }

            entry.LastX = x;
            entry.LastOffset = offset;

            if (entry.Verbose && frame >= entry.NextLogFrame)
            {
                entry.NextLogFrame = frame + LogIntervalFrames;

                // y 체인을 통째로 찍습니다. 게임이 어느 층의 y를 되돌리는지 한 번의 관찰로 판별하기 위함입니다.
                float noteWorldY = SafeY(() => noteTransform.position.y);
                float targetLocalY = SafeY(() => target.localPosition.y);
                float targetWorldY = SafeY(() => target.position.y);

                MelonLogger.Msg(
                    $"[LongNoteTrajectory] #{entry.Id}({entry.TargetKind}) frame={frame}: x={x:0.###}, " +
                    $"진행도={progress:0.###}, 오프셋={offset:0.###} | 노트world y={noteWorldY:0.###}, " +
                    $"대상local y={targetLocalY:0.###}(기대 {entry.TargetBase.y + offset:0.###}), 대상world y={targetWorldY:0.###}");
            }

            return true;
        }

        /// <summary>대상 좌표에 오프셋을 적용합니다.</summary>
        private static bool SetOffset(Tracked entry, float offset)
        {
            try
            {
                entry.Target.localPosition = new Vector3(
                    entry.TargetBase.x,
                    entry.TargetBase.y + offset,
                    entry.TargetBase.z);
                entry.LastOffset = offset;
                return true;
            }
            catch
            {
                return false;
            }
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
        /// 오프셋을 쓸 대상을 고릅니다.
        /// <para><b>왜 몸통 노드가 1순위인가:</b> 공중 롱노트는 게임이 노트 오브젝트의 <c>transform.position</c>(월드)을
        /// 매 프레임 직접 씁니다. 그래서 노트 자신은 물론 부모 래퍼를 올려도 상쇄되어 화면상 y가 그대로였습니다
        /// (관측: 오프셋 0.999를 줬는데 노트 월드 y는 -0.099 유지). 게임이 쓰는 노드보다 한 단계 아래,
        /// 실제 그림이 달린 <c>*_renderer(Clone)</c> 자식을 움직이면 그 위에서 무엇을 쓰든 영향받지 않습니다.</para>
        /// </summary>
        private static Transform ResolveOffsetTarget(Transform noteTransform, out string kind)
        {
            Transform body = FindBodyNode(noteTransform);
            if (body != null)
            {
                kind = "몸통 노드";
                return body;
            }

            try
            {
                Transform parent = noteTransform.parent;

                // SceneObjectController처럼 씬 전체가 매달린 노드를 움직이면 화면이 통째로 흔들립니다.
                if (parent != null && parent.parent != null && parent.childCount <= MaxWrapperChildren)
                {
                    kind = "부모 래퍼";
                    return parent;
                }
            }
            catch { }

            kind = "노트 자신";
            return noteTransform;
        }

        /// <summary>
        /// 노트 아래에서 그림이 달린 노드를 찾습니다.
        /// 계층 덤프 기준 <c>{zz}02_{road|air}_renderer(Clone)</c> 한 개가 몸통 막대와 머리/꼬리 마커를 모두 담고 있습니다.
        /// </summary>
        private static Transform FindBodyNode(Transform noteTransform)
        {
            try
            {
                for (int i = 0; i < noteTransform.childCount; i++)
                {
                    Transform child = noteTransform.GetChild(i);
                    if (child == null) continue;
                    if (child.name.IndexOf("renderer", StringComparison.OrdinalIgnoreCase) >= 0) return child;
                }

                // 이름 규칙이 바뀌었을 때를 위한 폴백: 자식이 하나뿐이면 그게 그림 노드입니다.
                if (noteTransform.childCount == 1) return noteTransform.GetChild(0);
            }
            catch { }

            return null;
        }

        /// <summary>대상 좌표를 원래대로 되돌립니다(오브젝트 풀 재사용 대비).</summary>
        private static void RestoreBase(Tracked entry)
        {
            if (entry == null) return;

            DetachRibbon(entry.Ribbon);
            entry.Ribbon = null;

            if (entry.Target == null) return;
            try
            {
                entry.Target.localPosition = entry.TargetBase;
            }
            catch { }

            if (entry.Verbose)
            {
                MelonLogger.Msg(
                    $"[LongNoteTrajectory] #{entry.Id} 추적 종료({entry.EndReason ?? "정리"}): " +
                    $"마지막 x={entry.LastX:0.###}, 마지막 오프셋={entry.LastOffset:0.###}, " +
                    $"비활성프레임={entry.InactiveFrames}");
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

        /// <summary>파괴된 Transform 접근으로 로그가 예외를 던지지 않게 감쌉니다.</summary>
        private static float SafeY(Func<float> read)
        {
            try { return read(); }
            catch { return float.NaN; }
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
