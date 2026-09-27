using Il2CppAssets.Scripts.UI.Panels;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace muse_dash_test
{
    /// <summary>
    /// 가이드 문구를 어디에 둘지 정합니다. 1순위는 곡 선택창 위쪽 탭 줄("기본 패키지 Q / 음악 팩 E") 바로 아래,
    /// 탭 줄을 못 찾으면 아티스트 이름 바로 아래입니다.
    ///
    /// <para><b>실측한 계층</b>(2026-09-27, 6.7.0, HiddenUnlockGuide.Diagnostics 로그). 둘 다 게임 씬에 직접 들어 있어
    /// 에셋 번들로는 볼 수 없었습니다.</para>
    /// <code>
    /// 탭 줄   : BtnOwn[Button] 580x80 &lt;- Tag&amp;Difficulty&amp;Dlc &lt;- StageUi &lt;- PnlStage[Canvas order=2]  (BtnDlc도 같은 부모)
    /// 아티스트: TxtArtist &lt;- ImgArtistMask[Mask] &lt;- Info[Canvas order=4] &lt;- StageUi &lt;- PnlStage
    /// </code>
    /// </summary>
    public static partial class HiddenUnlockGuide
    {
        // 위치는 인게임에서 보며 맞춘 값입니다(각 부모 좌표 단위).
        private const float GapBelowTabBar = 8f;
        private const float GapBelowArtist = 4f;

        // 탭 줄을 못 찾아 대체 자리로 간 경우, 곡마다 경고하지 않도록 한 번만 남깁니다.
        private static bool tabBarFallbackLogged;

        /// <summary>문구 오브젝트를 만듭니다. 부모와 위치는 <see cref="Place"/>가 곡마다 정합니다.</summary>
        private static Text GetOrCreateGuideText(Transform initialParent, Text artist)
        {
            if (guideText != null) return guideText;

            var go = new GameObject(GuideObjectName);
            go.transform.SetParent(initialParent, false);

            Text guide = go.AddComponent<Text>();
            guide.font = artist.font; // 게임 폰트라 한글이 그대로 나옵니다.
            guide.fontSize = Mathf.Max(12, Mathf.RoundToInt(artist.fontSize * FontScale));
            guide.alignment = TextAnchor.UpperCenter;
            guide.color = GuideColor;
            guide.horizontalOverflow = HorizontalWrapMode.Overflow;
            guide.verticalOverflow = VerticalWrapMode.Overflow;
            // 문구가 버튼 위에 겹쳐도 클릭을 가로채면 안 됩니다. 히든 해금이 바로 난이도 버튼 연타입니다.
            guide.raycastTarget = false;

            var outline = go.AddComponent<Outline>();
            outline.effectColor = new Color(0f, 0f, 0f, 0.8f);
            outline.effectDistance = new Vector2(1.5f, -1.5f);

            RectTransform rect = go.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0.5f, 1f); // 위쪽 가운데가 기준점입니다.

            guideText = guide;
            return guide;
        }

        private static void Place(Text guide, PnlStage pnlStage, Text artist)
        {
            if (TryPlaceBelowTabBar(guide, pnlStage, artist)) return;
            PlaceBelowArtist(guide, artist, pnlStage.transform);
        }

        /// <summary>
        /// 탭 줄 = 게임이 들고 있는 "기본 패키지" 탭 버튼(<c>PnlStage.m_BtnOwn</c>)의 부모입니다.
        /// 그 부모 아래 켜져 있는 버튼들(기본 패키지·음악 팩)의 전체 범위 바로 아래 가운데에 둡니다.
        ///
        /// <para>이름이나 표시 글자로 찾지 않습니다(CHECKLIST "이름 부분 일치"). 탭 줄의 자식이라 탭 줄이 움직이거나
        /// 숨으면 문구도 따라갑니다. 실측상 이 경로에는 마스크가 없고, 탭과 같은 캔버스라 가려지지 않습니다.</para>
        /// </summary>
        private static bool TryPlaceBelowTabBar(Text guide, PnlStage pnlStage, Text artist)
        {
            Button ownTab;
            try
            {
                ownTab = ReadOwnTabButton(pnlStage);
            }
            catch (Exception ex)
            {
                LogTabBarFallbackOnce($"{ex.GetType().Name}: {ex.Message}");
                return false;
            }

            Transform bar = ownTab != null ? ownTab.transform.parent : null;
            if (bar == null)
            {
                LogTabBarFallbackOnce("m_BtnOwn이 비어 있습니다");
                return false;
            }

            // 탭 줄 범위(탭 줄 좌표): 켜져 있는 버튼 자식들의 사각형을 합칩니다. 문구 자신은 버튼이 아니라 빠집니다.
            bool found = false;
            float xMin = 0f, xMax = 0f, yMin = 0f;
            for (int i = 0; i < bar.childCount; i++)
            {
                Transform child = bar.GetChild(i);
                if (!child.gameObject.activeSelf || child.GetComponent<Button>() == null) continue;
                RectTransform rect = child.TryCast<RectTransform>();
                if (rect == null) continue;

                Rect r = rect.rect;
                Vector3 p = rect.localPosition;
                Vector3 s = rect.localScale;
                float left = p.x + r.xMin * s.x;
                float right = p.x + r.xMax * s.x;
                float bottom = p.y + r.yMin * s.y;

                xMin = found ? Mathf.Min(xMin, left) : left;
                xMax = found ? Mathf.Max(xMax, right) : right;
                yMin = found ? Mathf.Min(yMin, bottom) : bottom;
                found = true;
            }

            if (!found)
            {
                LogTabBarFallbackOnce("탭 줄에 켜진 버튼이 없습니다");
                return false;
            }

            RectTransform guideRect = guide.rectTransform;
            if (guideRect.parent != bar) guideRect.SetParent(bar, false);
            MatchWorldScale(guideRect, artist.rectTransform.lossyScale);
            guideRect.sizeDelta = new Vector2(Mathf.Max(xMax - xMin, 600f), guide.fontSize * 1.3f);
            guideRect.localPosition = new Vector3((xMin + xMax) / 2f, yMin - GapBelowTabBar, 0f);
            guideRect.SetAsLastSibling();
            return true;
        }

        /// <summary>
        /// 게임 멤버는 여기서만 만집니다. 업데이트로 <c>m_BtnOwn</c>이 사라지면 JIT 실패가 호출부의 try에서 잡히고
        /// 아티스트 이름 아래로 대체됩니다(<see cref="GameBgmSource"/>와 같은 이유).
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static Button ReadOwnTabButton(PnlStage pnlStage)
        {
            return pnlStage.m_BtnOwn;
        }

        private static void LogTabBarFallbackOnce(string reason)
        {
            if (tabBarFallbackLogged) return;
            tabBarFallbackLogged = true;
            ModLogger.Warning($"[HiddenGuide] 탭 줄을 찾지 못해 아티스트 이름 아래에 표시합니다: {reason}");
        }

        /// <summary>
        /// 대체 자리: 아티스트 이름 글자 바로 아래. 부모는 아티스트를 그리는 캔버스(<c>Info</c>)입니다.
        ///
        /// <para>이 자리를 찾기까지 두 번 실패했습니다(같은 날 실측). 아티스트 줄의 자식으로 두면 그 줄이
        /// <c>ImgArtistMask</c> 마스크 안이라 줄 아래 문구가 잘렸고, <c>PnlStage</c> 아래로 옮기면 헤더 <c>Info</c>가
        /// 더 나중에 그려지는 캔버스(순서 4 &gt; 2)라 그 밑에 깔렸습니다. 같은 캔버스의 마지막 자식이면 둘 다 피합니다.</para>
        /// </summary>
        private static void PlaceBelowArtist(Text guide, Text artist, Transform panelRoot)
        {
            RectTransform artistRect = artist.rectTransform;
            RectTransform guideRect = guide.rectTransform;
            Transform parent = artist.canvas != null ? artist.canvas.transform : panelRoot;
            if (guideRect.parent != parent) guideRect.SetParent(parent, false);

            Rect r = artistRect.rect;
            // 아티스트 줄의 사각형이 글자보다 클 수 있어서, 정렬 방식으로 실제 글자 아래쪽을 계산합니다.
            float textHeight = Mathf.Min(artist.preferredHeight, r.height > 0f ? r.height : artist.preferredHeight);
            int vertical = (int)artist.alignment / 3; // TextAnchor: 0~2 Upper, 3~5 Middle, 6~8 Lower
            float glyphBottom = vertical == 0 ? r.yMax - textHeight
                : vertical == 1 ? r.center.y - textHeight / 2f
                : r.yMin;

            MatchWorldScale(guideRect, artistRect.lossyScale);
            guideRect.sizeDelta = new Vector2(Mathf.Max(r.width, 600f), guide.fontSize * 1.3f);
            guideRect.position = artistRect.TransformPoint(new Vector3(r.center.x, glyphBottom - GapBelowArtist, 0f));
            guideRect.SetAsLastSibling();
        }

        /// <summary>문구의 월드 배율을 <paramref name="targetLossyScale"/>(아티스트 이름)에 맞춰, 어느 부모 아래서든 같은 크기로 보이게 합니다.</summary>
        private static void MatchWorldScale(RectTransform guideRect, Vector3 targetLossyScale)
        {
            Vector3 parentScale = guideRect.parent != null ? guideRect.parent.lossyScale : Vector3.one;
            guideRect.localScale = new Vector3(
                parentScale.x != 0f ? targetLossyScale.x / parentScale.x : 1f,
                parentScale.y != 0f ? targetLossyScale.y / parentScale.y : 1f,
                1f);
        }
    }
}
