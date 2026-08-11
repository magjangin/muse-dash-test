using System;
using System.Collections.Generic;
using System.Text;
using MelonLoader;
using UnityEngine;

namespace muse_dash_test
{
    /// <summary>
    /// "라이더처럼 궤적을 그리는 롱노트"를 만들기 전에, 런타임 롱노트 체인의 실제 구조를 확인하는 관찰 전용 모듈입니다.
    /// <para>디컴파일 소스(Il2CppInterop 프록시)는 시그니처만 있고 메서드 바디가 없어서 아래 세 가지는 코드로 확인이 불가능합니다.
    /// 실제 플레이 로그로만 확정할 수 있어 프로브를 먼저 둡니다.</para>
    /// <list type="number">
    /// <item>start/middle/end 조각이 각각 별개 GameObject인가 → 조각별 좌표 제어가 가능한지</item>
    /// <item>노트 이동을 게임이 transform으로 굴리는가, Spine 본이 굴리고 transform은 고정인가</item>
    /// <item>모드가 LateUpdate에서 쓴 좌표가 다음 프레임까지 살아남는가 (WriteTest)</item>
    /// </list>
    /// <para>판정 경로에는 전혀 손대지 않습니다. 좌표를 읽기만 하고, <see cref="EnableWriteTest"/>가 켜져 있을 때만
    /// 체인 머리 조각의 y를 <see cref="WriteTestOffsetY"/>만큼 한 번 올려 보고 다음 샘플에서 유지 여부를 확인합니다.</para>
    /// </summary>
    public static partial class LongNoteTrajectoryProbe
    {
        /// <summary>
        /// 프로브 전체 on/off.
        /// <para>관찰이 끝나 기본값을 false로 내렸습니다. 롱노트 계층이 바뀌었거나 궤적이 안 먹을 때
        /// true로 올리면 조각 구조·좌표·계층 덤프를 다시 확인할 수 있습니다.</para>
        /// </summary>
        public static bool Enabled = false;

        /// <summary>
        /// 체인 머리 조각의 y를 한 번 올려 보고, 그 값이 다음 프레임까지 유지되는지 확인합니다.
        /// <para>1차 관찰 결론(2026-08-11): 지상 롱노트는 유지, 공중 롱노트는 매 프레임 덮어써짐.
        /// 그래서 <see cref="LongNoteTrajectory"/>는 노트가 아니라 부모 래퍼를 움직입니다.
        /// 이제는 궤적 모듈과 같은 좌표를 두고 다투게 되므로 기본값을 false로 둡니다.</para>
        /// </summary>
        public static bool EnableWriteTest = false;

        /// <summary>첫 롱노트의 계층/렌더러 구성을 1회 덤프합니다(곡선 몸통으로 교체할 대상 탐색용).</summary>
        public static bool EnableHierarchyDump = true;

        /// <summary>WriteTest에서 더할 y 오프셋. 화면에서 눈으로도 구분되도록 충분히 크게 잡습니다.</summary>
        private const float WriteTestOffsetY = 0.6f;

        /// <summary>OnControllerStart 호출 로그는 이 개수까지만 남깁니다(로그 폭발 방지).</summary>
        private const int MaxLoggedStarts = 16;

        /// <summary>좌표 샘플링은 이 개수의 체인까지만 추적합니다.</summary>
        private const int MaxTrackedChains = 2;

        /// <summary>체인 하나당 남길 좌표 샘플 수.</summary>
        private const int SamplesPerChain = 14;

        /// <summary>좌표 샘플 간격(프레임).</summary>
        private const int SampleIntervalFrames = 4;

        /// <summary>자식 조각은 이 개수까지만 좌표를 찍습니다(롱노트 middle은 0.1틱마다 생겨 매우 많습니다).</summary>
        private const int MaxLoggedChildren = 4;

        private sealed class Chain
        {
            public Il2Cpp.LongPressController Head;
            public string Tag;
            public int SamplesTaken;
            public int NextSampleFrame;
            public bool WriteApplied;
            public float ExpectedY;
            public Vector3 FirstWorld;
            public bool HasFirstWorld;
        }

        private static readonly List<Chain> chains = new List<Chain>();
        private static int loggedStarts;
        private static int chainSeq;
        private static bool hierarchyDumped;
        private static int dumpedNodes;

        /// <summary>씬 전환 시 상태를 비웁니다(곡을 다시 시작하면 처음부터 다시 관찰).</summary>
        public static void Reset()
        {
            chains.Clear();
            loggedStarts = 0;
            chainSeq = 0;
            hierarchyDumped = false;
            dumpedNodes = 0;
        }

        /// <summary>
        /// LongPressController.OnControllerStart Postfix에서 호출됩니다.
        /// 롱노트 조각이 등장할 때마다 어떤 조각(start/middle/end)인지 로그로 남기고,
        /// 체인 머리(start)면 좌표 샘플링 대상으로 등록합니다.
        /// </summary>
        public static void OnLongPressStart(Il2Cpp.LongPressController ctrl)
        {
            if (!Enabled || ctrl == null) return;

            Il2CppGameLogic.MusicData md = GetMusicData(ctrl);

            if (loggedStarts < MaxLoggedStarts)
            {
                loggedStarts++;
                MelonLogger.Msg(
                    $"[LongNoteProbe] OnControllerStart #{loggedStarts}: go='{SafeName(ctrl)}', idx={SafeIdx(ctrl)}, " +
                    $"childCount={SafeChildCount(ctrl)}, {Describe(md)}");
            }

            if (!IsChainHead(md) || chains.Count >= MaxTrackedChains) return;

            chainSeq++;
            var chain = new Chain
            {
                Head = ctrl,
                Tag = $"chain#{chainSeq}",
                NextSampleFrame = Time.frameCount,
            };
            chains.Add(chain);

            MelonLogger.Msg($"[LongNoteProbe] {chain.Tag} 좌표 샘플링 시작: {Describe(md)}, path={PathOf(SafeTransform(ctrl))}");

            DumpHierarchyOnce(ctrl);
        }

        /// <summary>
        /// MelonMod.OnLateUpdate에서 호출됩니다. 애니메이션/트윈이 끝난 뒤 좌표를 읽어야
        /// "게임이 이번 프레임에 최종적으로 확정한 좌표"를 보게 됩니다.
        /// </summary>
        public static void LateUpdate()
        {
            if (!Enabled || chains.Count == 0) return;

            int frame = Time.frameCount;
            for (int i = chains.Count - 1; i >= 0; i--)
            {
                Chain chain = chains[i];
                if (frame < chain.NextSampleFrame) continue;

                chain.NextSampleFrame = frame + SampleIntervalFrames;
                if (!SampleChain(chain, frame) || chain.SamplesTaken >= SamplesPerChain)
                {
                    chains.RemoveAt(i);
                }
            }
        }

        /// <summary>체인 하나를 한 번 샘플링합니다. 대상이 사라졌으면 false를 돌려 추적을 끝냅니다.</summary>
        private static bool SampleChain(Chain chain, int frame)
        {
            Il2Cpp.LongPressController head = chain.Head;
            Transform headTransform = SafeTransform(head);
            if (headTransform == null)
            {
                MelonLogger.Msg($"[LongNoteProbe] {chain.Tag} 추적 종료: 대상이 파괴되었거나 접근 불가 (샘플 {chain.SamplesTaken}회).");
                return false;
            }

            chain.SamplesTaken++;

            Vector3 world = headTransform.position;
            Vector3 local = headTransform.localPosition;

            // (2) 이동 주체 판별: 첫 샘플 대비 월드 좌표가 움직였는지.
            string moved;
            if (!chain.HasFirstWorld)
            {
                chain.FirstWorld = world;
                chain.HasFirstWorld = true;
                moved = "기준";
            }
            else
            {
                Vector3 d = world - chain.FirstWorld;
                moved = $"Δ({d.x:0.###},{d.y:0.###})";
            }

            // (3) 쓰기 유지 판별: 지난 샘플에서 올려 둔 y가 이번 프레임까지 살아남았는지.
            string writeResult = "";
            if (chain.WriteApplied)
            {
                bool kept = Mathf.Abs(local.y - chain.ExpectedY) < 0.001f;
                writeResult = kept
                    ? $", WriteTest=유지됨(y={local.y:0.###})"
                    : $", WriteTest=덮어써짐(기대 {chain.ExpectedY:0.###} → 실제 {local.y:0.###})";
            }

            MelonLogger.Msg(
                $"[LongNoteProbe] {chain.Tag} 샘플 {chain.SamplesTaken}/{SamplesPerChain} frame={frame}: " +
                $"world=({world.x:0.###},{world.y:0.###}), local=({local.x:0.###},{local.y:0.###}), " +
                $"scale={headTransform.localScale.x:0.###}, 이동={moved}{writeResult}, parent='{SafeParentName(headTransform)}'");

            LogChildren(chain, head);
            LogSlider(chain, head);

            // 첫 샘플에서 한 번만 y를 올려 둡니다. 다음 샘플에서 유지 여부를 봅니다.
            if (EnableWriteTest && !chain.WriteApplied)
            {
                chain.ExpectedY = local.y + WriteTestOffsetY;
                headTransform.localPosition = new Vector3(local.x, chain.ExpectedY, local.z);
                chain.WriteApplied = true;
                MelonLogger.Msg($"[LongNoteProbe] {chain.Tag} WriteTest 적용: y {local.y:0.###} → {chain.ExpectedY:0.###}");
            }

            return true;
        }

        /// <summary>middle/end 조각이 별개 GameObject인지, 각자 좌표를 갖는지 확인합니다.</summary>
        private static void LogChildren(Chain chain, Il2Cpp.LongPressController head)
        {
            try
            {
                var children = head.m_LongChild;
                if (children == null)
                {
                    MelonLogger.Msg($"[LongNoteProbe] {chain.Tag}   자식: m_LongChild=null");
                    return;
                }

                int count = children.Count;
                var sb = new StringBuilder();
                sb.Append($"[LongNoteProbe] {chain.Tag}   자식 {count}개");

                int logged = 0;
                for (int i = 0; i < count && logged < MaxLoggedChildren; i++)
                {
                    Il2Cpp.LongPressController child = children[i];
                    Transform t = SafeTransform(child);
                    if (t == null)
                    {
                        sb.Append($" | [{i}] (transform 없음)");
                        logged++;
                        continue;
                    }

                    Vector3 w = t.position;
                    sb.Append($" | [{i}] go='{SafeName(child)}' world=({w.x:0.###},{w.y:0.###}) {DescribeShort(GetMusicData(child))}");
                    logged++;
                }

                MelonLogger.Msg(sb.ToString());
            }
            catch (Exception ex)
            {
                MelonLogger.Msg($"[LongNoteProbe] {chain.Tag}   자식 조회 예외: {ex.GetType().Name} {ex.Message}");
            }
        }

        /// <summary>몸통 슬라이더 스프라이트의 좌표/스케일/스프라이트명을 확인합니다(곡선 리본으로 교체할 대상).</summary>
        private static void LogSlider(Chain chain, Il2Cpp.LongPressController head)
        {
            try
            {
                if (!head.TryGetVisibleComponent(out var visible) || visible == null)
                {
                    MelonLogger.Msg($"[LongNoteProbe] {chain.Tag}   슬라이더: VisibleComponent 없음");
                    return;
                }

                SpriteRenderer slider = visible.m_SliderRenderer;
                if (slider == null)
                {
                    MelonLogger.Msg($"[LongNoteProbe] {chain.Tag}   슬라이더: m_SliderRenderer=null");
                    return;
                }

                Transform t = slider.transform;
                string spriteName = "(null)";
                try { spriteName = slider.sprite != null ? slider.sprite.name : "(null)"; } catch { }

                MelonLogger.Msg(
                    $"[LongNoteProbe] {chain.Tag}   슬라이더: sprite='{spriteName}', enabled={slider.enabled}, " +
                    $"drawMode={slider.drawMode}, world=({t.position.x:0.###},{t.position.y:0.###}), " +
                    $"localScale=({t.localScale.x:0.###},{t.localScale.y:0.###}), boundsSize=({slider.bounds.size.x:0.###},{slider.bounds.size.y:0.###}), " +
                    $"path={PathOf(t)}");
            }
            catch (Exception ex)
            {
                MelonLogger.Msg($"[LongNoteProbe] {chain.Tag}   슬라이더 조회 예외: {ex.GetType().Name} {ex.Message}");
            }
        }

        // ── 헬퍼 ────────────────────────────────────────────────────────────────

        /// <summary>length가 있고 중간/끝 플래그가 없는 조각이 체인 머리(롱노트 start)입니다.</summary>
        private static bool IsChainHead(Il2CppGameLogic.MusicData md)
        {
            if (md == null) return false;
            try
            {
                if (md.isLongPressing || md.isLongPressEnd) return false;
                return md.configData != null && ToDouble(md.configData.length) > 0.0;
            }
            catch
            {
                return false;
            }
        }

        private static Il2CppGameLogic.MusicData GetMusicData(Il2Cpp.LongPressController ctrl)
        {
            if (ctrl == null) return null;
            try
            {
                var md = ctrl.MusicData;
                if (md != null) return md;
            }
            catch { }

            try { return ctrl.PMusicData; }
            catch { return null; }
        }

        private static string Describe(Il2CppGameLogic.MusicData md)
        {
            if (md == null) return "md=(null)";
            try
            {
                string uid = "(null)";
                string prefab = "(null)";
                int type = -1;
                int pathway = -1;

                var nd = md.noteData;
                if (nd != null)
                {
                    uid = nd.uid ?? "(null)";
                    prefab = nd.prefab_name ?? "(null)";
                    type = (int)nd.type;
                    pathway = nd.pathway;
                }

                double length = md.configData != null ? ToDouble(md.configData.length) : 0.0;
                return $"objId={md.objId}, tick={ToDouble(md.tick):0.###}, len={length:0.###}, " +
                       $"pTick={ToDouble(md.longPressPTick):0.###}, pressing={md.isLongPressing}, end={md.isLongPressEnd}, " +
                       $"uid={uid}, type={type}, pathway={pathway}, prefab={prefab}";
            }
            catch (Exception ex)
            {
                return $"md=(읽기 예외 {ex.GetType().Name})";
            }
        }

        private static string DescribeShort(Il2CppGameLogic.MusicData md)
        {
            if (md == null) return "md=(null)";
            try
            {
                return $"objId={md.objId} tick={ToDouble(md.tick):0.###} pressing={md.isLongPressing} end={md.isLongPressEnd}";
            }
            catch
            {
                return "md=(읽기 예외)";
            }
        }

        private static double ToDouble(Il2CppSystem.Decimal value)
        {
            // 실험차트 쪽과 같은 변환기를 씁니다(로케일 소수점 구분자 차이까지 처리).
            return global::DBStageInfo_SetRuntimeMusicData_Patch.ParseMusicDecimal(value);
        }

        private static Transform SafeTransform(Il2Cpp.LongPressController ctrl)
        {
            if (ctrl == null) return null;
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

        private static string SafeName(Il2Cpp.LongPressController ctrl)
        {
            try { return ctrl != null ? ctrl.gameObject.name : "(null)"; }
            catch { return "(접근 불가)"; }
        }

        private static string SafeIdx(Il2Cpp.LongPressController ctrl)
        {
            try { return ctrl.GetIdx().ToString(); }
            catch { return "?"; }
        }

        private static string SafeChildCount(Il2Cpp.LongPressController ctrl)
        {
            try
            {
                var children = ctrl.m_LongChild;
                return children != null ? children.Count.ToString() : "null";
            }
            catch { return "?"; }
        }

        private static string SafeParentName(Transform t)
        {
            try
            {
                Transform parent = t.parent;
                return parent != null ? parent.name : "(root)";
            }
            catch { return "(접근 불가)"; }
        }

        private static string PathOf(Transform t)
        {
            if (t == null) return "(null)";
            try
            {
                string path = t.name;
                Transform parent = t.parent;
                while (parent != null)
                {
                    path = parent.name + "/" + path;
                    parent = parent.parent;
                }
                return path;
            }
            catch
            {
                return "(경로 계산 실패)";
            }
        }
    }

    /// <summary>
    /// 롱노트 조각이 등장하는 시점을 잡습니다. 관찰 전용이라 원본 동작은 그대로 두고 Postfix만 답니다.
    /// </summary>
    [HarmonyLib.HarmonyPatch(typeof(Il2Cpp.LongPressController), "OnControllerStart")]
    public class LongPressController_OnControllerStart_Probe
    {
        public static void Postfix(Il2Cpp.LongPressController __instance)
        {
            try
            {
                LongNoteTrajectoryProbe.OnLongPressStart(__instance);
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[LongNoteProbe] OnControllerStart Postfix 예외: {ex}");
            }
        }
    }
}
