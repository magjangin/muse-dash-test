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
        /// <summary>원본 직선 막대를 숨길지. 끄면 원본과 리본이 같이 보여 위치 비교에 쓸 수 있습니다.</summary>
        public static bool HideOriginalBody = true;

        /// <summary>
        /// 리본에 원본 막대의 스프라이트 텍스처를 입힐지. false면 <see cref="RibbonColor"/> 단색으로 그립니다.
        /// <para>텍스처를 못 잘라내면 자동으로 단색으로 떨어집니다.</para>
        /// </summary>
        public static bool UseSpriteTexture = true;

        /// <summary>단색 모드에서 쓸 리본 색. 게임 원본과 헷갈리지 않게 눈에 띄는 색을 기본으로 둡니다.</summary>
        public static Color RibbonColor = new Color(0.2f, 1f, 0.7f, 0.9f);

        /// <summary>리본을 구성할 점 개수. 많을수록 매끄럽지만 매 프레임 갱신 비용이 늘어납니다.</summary>
        private const int RibbonPoints = 24;

        /// <summary>리본 상태 진단 로그 간격(프레임).</summary>
        private const int RibbonLogIntervalFrames = 10;

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
            public int NextLogFrame;

            // 게임이 우리가 끈/켠 상태를 되돌린 횟수. 깜빡임 원인 판별용입니다.
            public int LineReEnabledCount;
            public int BodyReHiddenCount;
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
                    if (HideOriginalBody) state.OriginalBody.enabled = false;
                }

                state.Line.enabled = true;
                state.NextLogFrame = Time.frameCount;

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
                // 깜빡임 진단: 우리가 정해 둔 상태를 게임이 되돌렸는지 먼저 읽어 둡니다.
                bool lineWasEnabled = state.Line.enabled;
                bool bodyWasVisible = state.OriginalBody != null && state.OriginalBody.enabled;

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

                // 게임 쪽 로직(SetVisible, 투명도 코루틴 등)이 렌더러 상태를 되돌려도 매 프레임 다시 강제합니다.
                if (!lineWasEnabled)
                {
                    state.Line.enabled = true;
                    state.LineReEnabledCount++;
                }

                if (bodyWasVisible && HideOriginalBody)
                {
                    state.OriginalBody.enabled = false;
                    state.BodyReHiddenCount++;
                }

                LogRibbonState(state, lineWasEnabled, bodyWasVisible, tailWorldSpan);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 깜빡임 원인 판별용 진단 로그입니다.
        /// <para><c>보임</c>(Renderer.isVisible)이 false로 떨어지면 카메라 컬링, <c>되살아남</c> 카운트가 오르면
        /// 게임 로직이 렌더러 상태를 되돌리는 것입니다. 둘은 대처가 다릅니다.</para>
        /// </summary>
        private static void LogRibbonState(RibbonState state, bool lineWasEnabled, bool bodyWasVisible, float span)
        {
            if (!state.Verbose) return;

            int frame = Time.frameCount;
            if (frame < state.NextLogFrame) return;
            state.NextLogFrame = frame + RibbonLogIntervalFrames;

            try
            {
                // 리본 양 끝과 별(마커) 위치를 같은 월드 좌표계로 찍습니다.
                // 두 쌍이 일치하면 지오메트리는 정확한 것이고, 화면에서 어긋나 보이는 원인은 텍스처(UV)입니다.
                int last = state.Line.positionCount - 1;
                Vector3 ribbonStart = state.Line.transform.TransformPoint(state.Line.GetPosition(0));
                Vector3 ribbonEnd = state.Line.transform.TransformPoint(state.Line.GetPosition(last));

                string headText = state.HeadMarker != null
                    ? $"({state.HeadMarker.position.x:0.##},{state.HeadMarker.position.y:0.##})"
                    : "(없음)";
                Vector3 tail = state.TailMarker.position;

                MelonLogger.Msg(
                    $"[LongNoteTrajectory] #{state.Id} 리본 frame={frame}: 보임={state.Line.isVisible}, " +
                    $"점수={state.Line.positionCount}, 길이={span:0.###}, 두께={state.Line.startWidth:0.###} | " +
                    $"리본끝 ({ribbonStart.x:0.##},{ribbonStart.y:0.##})→({ribbonEnd.x:0.##},{ribbonEnd.y:0.##}) vs " +
                    $"별 머리{headText}→꼬리({tail.x:0.##},{tail.y:0.##}) | " +
                    $"원본막대 보임={bodyWasVisible}, 리본되살림={state.LineReEnabledCount}회, 원본재숨김={state.BodyReHiddenCount}회");
            }
            catch { }
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

            // 지상/공중처럼 재질은 같고 스프라이트만 다른 경우가 있어 둘을 합쳐 캐시 키로 씁니다.
            long key = ((long)source.GetInstanceID() << 32) ^ (uint)(bodySprite.sprite != null ? bodySprite.sprite.GetInstanceID() : 0);
            if (ribbonMaterials.TryGetValue(key, out Material cached) && cached != null) return cached;

            Material material;
            try
            {
                material = new Material(source);

                // 텍스처를 비우면 셰이더가 흰색으로 채우고 정점 색(RibbonColor)이 곱해집니다.
                // 잘라내기에 실패했을 때도 이 상태로 남아 단색 리본이 됩니다.
                material.mainTexture = null;

                if (UseSpriteTexture)
                {
                    Texture2D cropped = GetCroppedSpriteTexture(bodySprite.sprite);
                    if (cropped != null)
                    {
                        material.mainTexture = cropped;
                        material.SetTextureScale("_MainTex", Vector2.one);
                        material.SetTextureOffset("_MainTex", Vector2.zero);
                    }
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

        /// <summary>
        /// 스프라이트가 차지하는 아틀라스 영역만 잘라낸 텍스처를 만듭니다.
        /// <para><b>왜 필요한가:</b> 아틀라스 텍스처를 그대로 물리고 타일링/오프셋(<c>_MainTex_ST</c>)으로 조각을
        /// 지정하려 했지만, 스프라이트 셰이더는 UV를 그대로 통과시켜 ST를 무시합니다. 그래서 리본이 아틀라스
        /// 전체를 늘려 그렸고 엉뚱한 조각이 보였습니다. 조각을 미리 잘라 두면 UV 0~1이 곧 막대가 되어
        /// 셰이더 구현에 기대지 않아도 됩니다.</para>
        /// <para>GPU 블릿 → ReadPixels 경로라 압축 포맷/블록 정렬 제약을 받지 않습니다.
        /// 스프라이트당 한 번만 수행하고 캐시합니다.</para>
        /// </summary>
        private static Texture2D GetCroppedSpriteTexture(Sprite sprite)
        {
            if (sprite == null) return null;

            Texture2D source = sprite.texture;
            if (source == null || source.width <= 0 || source.height <= 0) return null;

            int key = sprite.GetInstanceID();
            if (croppedTextures.TryGetValue(key, out Texture2D cached)) return cached;

            Texture2D result = null;
            RenderTexture previous = RenderTexture.active;
            RenderTexture temporary = null;

            try
            {
                Rect rect = sprite.textureRect;
                int width = Mathf.RoundToInt(rect.width);
                int height = Mathf.RoundToInt(rect.height);

                if (width > 0 && height > 0)
                {
                    var scale = new Vector2(rect.width / source.width, rect.height / source.height);
                    var offset = new Vector2(rect.x / source.width, rect.y / source.height);

                    temporary = RenderTexture.GetTemporary(width, height, 0, RenderTextureFormat.ARGB32);
                    Graphics.Blit(source, temporary, scale, offset);

                    RenderTexture.active = temporary;
                    result = new Texture2D(width, height, TextureFormat.RGBA32, false)
                    {
                        wrapMode = TextureWrapMode.Clamp,
                        filterMode = source.filterMode,
                    };
                    result.ReadPixels(new Rect(0f, 0f, width, height), 0, 0);
                    result.Apply();

                    MelonLogger.Msg(
                        $"[LongNoteTrajectory] 리본 텍스처 잘라내기 완료: sprite='{sprite.name}', " +
                        $"{width}x{height} (원본 {source.width}x{source.height}, rect={rect})");
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Msg($"[LongNoteTrajectory] 리본 텍스처 잘라내기 실패({ex.GetType().Name}: {ex.Message}), 단색 리본으로 그립니다.");
                result = null;
            }
            finally
            {
                RenderTexture.active = previous;
                if (temporary != null) RenderTexture.ReleaseTemporary(temporary);
            }

            croppedTextures[key] = result;
            return result;
        }

        private static readonly System.Collections.Generic.Dictionary<int, Texture2D> croppedTextures
            = new System.Collections.Generic.Dictionary<int, Texture2D>();

        private static readonly System.Collections.Generic.Dictionary<long, Material> ribbonMaterials
            = new System.Collections.Generic.Dictionary<long, Material>();

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

            Color color = UseSpriteTexture ? Color.white : RibbonColor;
            line.startColor = color;
            line.endColor = color;
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
