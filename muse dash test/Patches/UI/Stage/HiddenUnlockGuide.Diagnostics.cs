using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace muse_dash_test
{
    /// <summary>
    /// 가이드 문구가 실제로 어디에 그려지는지 세션당 한 번 남기는 계측입니다.
    ///
    /// <para><b>왜 있나</b>: 선택창 헤더와 탭 줄은 게임 씬에 직접 들어 있어 에셋 번들로는 계층을 볼 수 없었습니다.
    /// 이 로그로 표시 실패 두 번의 원인(마스크 잘림, 캔버스 순서에 깔림)과 탭 줄의 위치를 확정했습니다
    /// (HiddenUnlockGuide.Placement.cs 주석). 탭 줄 아래 표시가 확인되면 걷어내도 됩니다.</para>
    ///
    /// <para>CHECKLIST에 따라 객체를 훑는 깊은 덤프는 하지 않습니다. 문구의 조상 경로(표시에 영향을 주는
    /// 컴포넌트 여부와 크기), 그리는 캔버스, 부모 기준 위치만 남깁니다.</para>
    /// </summary>
    public static partial class HiddenUnlockGuide
    {
        private static bool placementLogged;

        private static void LogPlacementOnce(Text guide, Transform panelRoot)
        {
            if (placementLogged) return;
            placementLogged = true;

            Transform t = guide.transform;
            Vector3 local = t.localPosition;
            ModLogger.Msg($"[HiddenGuide.Diag] 가이드 경로: {DescribeAncestors(t, panelRoot)}");
            ModLogger.Msg($"[HiddenGuide.Diag] 가이드: 그리는 캔버스={DescribeCanvas(guide.canvas)}, 부모 기준 위치=({local.x:0.#},{local.y:0.#}), {t.GetSiblingIndex() + 1}/{(t.parent != null ? t.parent.childCount : 0)}번째, activeInHierarchy={guide.gameObject.activeInHierarchy}, fontSize={guide.fontSize}, font={(guide.font != null ? guide.font.name : "(null)")}, lossyScale={FormatScale(t.lossyScale)}");
        }

        private static string DescribeCanvas(Canvas canvas)
        {
            return canvas != null ? $"{canvas.name}(order={canvas.sortingOrder}, override={canvas.overrideSorting})" : "(없음)";
        }

        private static string FormatScale(Vector3 scale)
        {
            return $"({scale.x:0.#####},{scale.y:0.#####})";
        }

        /// <summary>
        /// 조상 경로를 "이름(off)[컴포넌트] 폭x높이"로 이어 붙입니다. <paramref name="stopAt"/>에 닿으면 그 이름까지만 적습니다.
        /// </summary>
        private static string DescribeAncestors(Transform start, Transform stopAt = null)
        {
            var sb = new StringBuilder();
            int depth = 0;
            for (Transform t = start; t != null && depth < 12; t = t.parent, depth++)
            {
                if (depth > 0) sb.Append(" <- ");
                sb.Append(t.name);
                if (t == stopAt) break;
                if (!t.gameObject.activeSelf) sb.Append("(off)");
                if (t.GetComponent<Mask>() != null) sb.Append("[Mask]");
                if (t.GetComponent<RectMask2D>() != null) sb.Append("[RectMask2D]");
                if (t.GetComponent<Button>() != null) sb.Append("[Button]");
                if (t.GetComponent<Toggle>() != null) sb.Append("[Toggle]");
                CanvasGroup group = t.GetComponent<CanvasGroup>();
                if (group != null) sb.Append($"[CanvasGroup a={group.alpha:0.##}]");
                Canvas canvas = t.GetComponent<Canvas>();
                if (canvas != null) sb.Append($"[Canvas order={canvas.sortingOrder}]");
                RectTransform rect = t.TryCast<RectTransform>();
                if (rect != null) sb.Append($" {rect.rect.width:0}x{rect.rect.height:0}");
            }
            return sb.ToString();
        }
    }
}
