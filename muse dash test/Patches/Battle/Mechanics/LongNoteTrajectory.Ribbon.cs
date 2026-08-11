using System;
using MelonLoader;
using UnityEngine;

namespace muse_dash_test
{
    /// <summary>
    /// 롱노트 몸통을 곡선으로 다시 그리는 부분입니다.
    /// <para>게임의 몸통은 스프라이트 한 장을 늘린 직선 막대라(계층 덤프 기준
    /// <c>{zz}02_{road|air}_renderer(Clone)</c>의 SpriteRenderer) 휠 수 없습니다. 그래서 원본 막대를 숨기고
    /// 같은 자리에 LineRenderer를 얹어 머리~꼬리 구간을 궤적 곡선대로 샘플링해 그립니다.</para>
    /// <para>머리/꼬리 마커(<c>startParent</c>, <c>startParent(Clone)</c>)도 각자 자기 x 위치의 곡선 높이로 올려
    /// 리본 양 끝에 정확히 얹힙니다.</para>
    /// </summary>
    public static partial class LongNoteTrajectory
    {
        /// <summary>리본을 구성할 점 개수. 많을수록 매끄럽지만 매 프레임 갱신 비용이 늘어납니다.</summary>
        private const int RibbonPoints = 24;

        /// <summary>모드가 만든 리본 오브젝트 이름. 오브젝트 풀 재사용 시 다시 찾아 쓰려고 고정합니다.</summary>
        private const string RibbonObjectName = "hwa_long_ribbon";

        /// <summary>원본 막대 두께를 못 읽었을 때 쓸 리본 두께(월드 단위).</summary>
        private const float FallbackRibbonWidth = 0.35f;

        /// <summary>롱노트 하나에 붙인 곡선 리본과, 되돌리기에 필요한 원본 상태입니다.</summary>
        internal sealed class RibbonState
        {
            public Transform NoteTransform;
            public LineRenderer Line;
            public SpriteRenderer OriginalBody;
            public bool OriginalBodyWasEnabled;

            public Transform HeadMarker;
            public Transform TailMarker;
            public float HeadMarkerBaseY;
            public float TailMarkerBaseY;

            public float BodyLocalY;   // 몸통 노드의 원래 local y (리본을 얹을 높이)
            public float BodyScaleX;   // 몸통 노드의 x 스케일 (마커 local x → 월드 거리 환산용)
            public bool Verbose;
            public int Id;
        }

        /// <summary>
        /// 몸통 노드에서 원본 막대를 숨기고 곡선 리본을 붙입니다.
        /// 준비에 실패하면 null을 돌려주고, 호출부는 기존 Rigid 동작으로 계속 진행합니다.
        /// </summary>
        private static RibbonState AttachRibbon(int id, Transform noteTransform, Transform bodyNode, bool verbose)
        {
            if (Body != BodyMode.Curved || noteTransform == null || bodyNode == null) return null;

            try
            {
                var state = new RibbonState
                {
                    NoteTransform = noteTransform,
                    Id = id,
                    Verbose = verbose,
                    BodyLocalY = bodyNode.localPosition.y,
                    BodyScaleX = bodyNode.localScale.x,
                };

                state.OriginalBody = bodyNode.GetComponent<SpriteRenderer>();
                state.HeadMarker = FindMarker(bodyNode, isTail: false);
                state.TailMarker = FindMarker(bodyNode, isTail: true);

                if (state.TailMarker == null)
                {
                    if (verbose) MelonLogger.Msg($"[LongNoteTrajectory] #{id} 리본 준비 실패: 꼬리 마커를 찾지 못했습니다. Rigid로 진행합니다.");
                    return null;
                }

                // 자식이 하나뿐이면 머리와 꼬리가 같은 오브젝트로 잡힙니다. 두 번 쓰면 마지막 값만 남으므로 머리를 비웁니다.
                if (state.HeadMarker == state.TailMarker) state.HeadMarker = null;

                state.HeadMarkerBaseY = state.HeadMarker != null ? state.HeadMarker.localPosition.y : 0f;
                state.TailMarkerBaseY = state.TailMarker.localPosition.y;

                state.Line = EnsureLineRenderer(noteTransform, state.OriginalBody);
                if (state.Line == null)
                {
                    if (verbose) MelonLogger.Msg($"[LongNoteTrajectory] #{id} 리본 준비 실패: LineRenderer를 만들지 못했습니다. Rigid로 진행합니다.");
                    return null;
                }

                if (state.OriginalBody != null)
                {
                    state.OriginalBodyWasEnabled = state.OriginalBody.enabled;
                    state.OriginalBody.enabled = false;
                }

                state.Line.enabled = true;

                if (verbose)
                {
                    MelonLogger.Msg(
                        $"[LongNoteTrajectory] #{id} 리본 부착: 몸통local y={state.BodyLocalY:0.###}, 몸통scaleX={state.BodyScaleX:0.###}, " +
                        $"꼬리local x={state.TailMarker.localPosition.x:0.###}, " +
                        $"길이(월드)={state.TailMarker.localPosition.x * state.BodyScaleX:0.###}, " +
                        $"원본막대={(state.OriginalBody != null ? "숨김" : "없음")}");
                }

                return state;
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[LongNoteTrajectory] #{id} 리본 부착 예외: {ex}");
                return null;
            }
        }

        /// <summary>리본 점들과 머리/꼬리 마커 높이를 이번 프레임의 노트 x 기준으로 갱신합니다.</summary>
        private static bool UpdateRibbon(RibbonState state, float noteWorldX)
        {
            if (state?.Line == null || state.TailMarker == null) return false;

            try
            {
                // 꼬리는 SetLength로 늘어날 수 있으므로 매 프레임 현재 값을 읽습니다.
                float tailLocalX = state.TailMarker.localPosition.x;
                float tailWorldSpan = tailLocalX * state.BodyScaleX;

                for (int i = 0; i < RibbonPoints; i++)
                {
                    float t = RibbonPoints > 1 ? i / (float)(RibbonPoints - 1) : 0f;
                    float pointWorldX = noteWorldX + tailWorldSpan * t;
                    float y = state.BodyLocalY + Offset(Progress(pointWorldX));

                    // 리본은 노트 루트의 자식이라 local x가 곧 월드 거리(루트 스케일 1)입니다.
                    state.Line.SetPosition(i, new Vector3(tailWorldSpan * t, y, 0f));
                }

                // 마커는 몸통 노드의 자식이므로 x는 local, 높이는 몸통 기준 상대값으로 올립니다.
                if (state.HeadMarker != null)
                {
                    SetMarkerY(state.HeadMarker, state.HeadMarkerBaseY + Offset(Progress(noteWorldX)));
                }

                SetMarkerY(state.TailMarker, state.TailMarkerBaseY + Offset(Progress(noteWorldX + tailWorldSpan)));

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>리본을 걷어내고 원본 막대를 되살립니다(오브젝트 풀 재사용 대비).</summary>
        private static void DetachRibbon(RibbonState state)
        {
            if (state == null) return;

            try
            {
                if (state.Line != null) state.Line.enabled = false;
                if (state.OriginalBody != null) state.OriginalBody.enabled = state.OriginalBodyWasEnabled;
                if (state.HeadMarker != null) SetMarkerY(state.HeadMarker, state.HeadMarkerBaseY);
                if (state.TailMarker != null) SetMarkerY(state.TailMarker, state.TailMarkerBaseY);
            }
            catch { }
        }

        /// <summary>
        /// 노트 루트 아래에 리본용 LineRenderer를 준비합니다.
        /// 노트는 파괴되지 않고 풀로 돌아가므로, 이미 만들어 둔 것이 있으면 그대로 재사용합니다.
        /// </summary>
        private static LineRenderer EnsureLineRenderer(Transform noteTransform, SpriteRenderer bodySprite)
        {
            Transform existing = noteTransform.Find(RibbonObjectName);
            if (existing != null)
            {
                LineRenderer cached = existing.GetComponent<LineRenderer>();
                if (cached != null)
                {
                    ConfigureLineRenderer(cached, bodySprite);
                    return cached;
                }
            }

            var go = new GameObject(RibbonObjectName);
            go.transform.SetParent(noteTransform, false);
            go.transform.localPosition = Vector3.zero;
            go.transform.localRotation = Quaternion.identity;
            go.transform.localScale = Vector3.one;

            LineRenderer line = go.AddComponent<LineRenderer>();
            ConfigureLineRenderer(line, bodySprite);
            return line;
        }

        /// <summary>
        /// 원본 막대 재질을 리본용으로 복제해 캐시합니다.
        /// <para>스프라이트 아틀라스를 그대로 쓰면 LineRenderer가 UV 0~1을 아틀라스 전체에 매핑해서
        /// 엉뚱한 그림이 늘어납니다. 스프라이트의 <c>textureRect</c>를 타일링/오프셋으로 넣어
        /// 해당 조각만 샘플링하게 맞춥니다. 노트마다 Material을 새로 만들면 새기 때문에 원본 재질 기준으로 캐시합니다.
        /// </para>
        /// </summary>
        private static Material GetRibbonMaterial(SpriteRenderer bodySprite)
        {
            if (bodySprite == null) return null;

            Material source = bodySprite.sharedMaterial;
            if (source == null) return null;

            int key = source.GetInstanceID();
            if (ribbonMaterials.TryGetValue(key, out Material cached) && cached != null) return cached;

            Material material;
            try
            {
                material = new Material(source);
                Sprite sprite = bodySprite.sprite;
                Texture2D texture = sprite != null ? sprite.texture : null;

                if (texture != null && texture.width > 0 && texture.height > 0)
                {
                    material.mainTexture = texture;

                    Rect rect = sprite.textureRect;
                    material.SetTextureScale("_MainTex", new Vector2(rect.width / texture.width, rect.height / texture.height));
                    material.SetTextureOffset("_MainTex", new Vector2(rect.x / texture.width, rect.y / texture.height));
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Msg($"[LongNoteTrajectory] 리본 재질 준비 실패({ex.GetType().Name}), 원본 재질을 그대로 씁니다.");
                material = source;
            }

            ribbonMaterials[key] = material;
            return material;
        }

        private static readonly System.Collections.Generic.Dictionary<int, Material> ribbonMaterials
            = new System.Collections.Generic.Dictionary<int, Material>();

        /// <summary>리본이 원본 막대와 같은 재질·두께·정렬 순서로 보이도록 맞춥니다.</summary>
        private static void ConfigureLineRenderer(LineRenderer line, SpriteRenderer bodySprite)
        {
            line.useWorldSpace = false;
            line.positionCount = RibbonPoints;
            line.numCapVertices = 2;
            line.numCornerVertices = 2;
            line.textureMode = LineTextureMode.Stretch;
            line.alignment = LineAlignment.View;
            line.receiveShadows = false;
            line.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

            float width = FallbackRibbonWidth;
            if (bodySprite != null)
            {
                try
                {
                    // 원본 막대의 화면상 두께를 그대로 씁니다.
                    float bodyHeight = bodySprite.bounds.size.y;
                    if (bodyHeight > 0.01f) width = bodyHeight;

                    Material material = GetRibbonMaterial(bodySprite);
                    if (material != null) line.sharedMaterial = material;

                    line.sortingLayerID = bodySprite.sortingLayerID;
                    line.sortingOrder = bodySprite.sortingOrder;
                }
                catch { }
            }

            line.startWidth = width;
            line.endWidth = width;
            line.startColor = Color.white;
            line.endColor = Color.white;
        }

        /// <summary>
        /// 머리/꼬리 마커를 찾습니다. 계층 덤프 기준 몸통 노드 아래에
        /// <c>startParent</c>(머리)와 <c>startParent(Clone)</c>(꼬리, local x &gt; 0)가 있습니다.
        /// 이름 규칙에 기대지 않도록 local x가 가장 큰/작은 자식으로 판별합니다.
        /// </summary>
        private static Transform FindMarker(Transform bodyNode, bool isTail)
        {
            Transform best = null;
            float bestX = 0f;

            try
            {
                for (int i = 0; i < bodyNode.childCount; i++)
                {
                    Transform child = bodyNode.GetChild(i);
                    if (child == null) continue;

                    float childX = child.localPosition.x;
                    if (best == null || (isTail ? childX > bestX : childX < bestX))
                    {
                        best = child;
                        bestX = childX;
                    }
                }
            }
            catch { }

            // 꼬리는 머리보다 확실히 오른쪽에 있어야 의미가 있습니다(길이 0이면 리본을 그릴 수 없음).
            if (isTail && best != null && bestX <= 0.01f) return null;
            return best;
        }

        /// <summary>마커의 y만 바꿉니다(x는 게임이 정한 길이라 건드리지 않습니다).</summary>
        private static void SetMarkerY(Transform marker, float y)
        {
            Vector3 local = marker.localPosition;
            marker.localPosition = new Vector3(local.x, y, local.z);
        }
    }
}
