using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace muse_dash_test
{
    /// <summary>
    /// 가이드 문구가 실제로 어디에 그려지는지 세션당 한 번 남기는 계측입니다.
    ///
    /// <para><b>왜 있나</b>: 첫 구현(아티스트 줄의 자식)이 인게임에서 보이지 않았는데, 선택창 헤더는 게임 씬에
    /// 직접 들어 있어 에셋 번들로는 계층을 확인할 수 없었습니다. 이 로그로 두 번의 실패 원인(마스크 잘림,
    /// 캔버스 순서에 깔림)을 확정했습니다(<see cref="ResolveGuideParent"/> 주석). 표시가 확인되면 걷어내도 됩니다.</para>
    ///
    /// <para>CHECKLIST에 따라 객체를 훑는 깊은 덤프는 하지 않습니다. 아티스트 줄의 조상 최대 10단계의 이름과
    /// 표시에 영향을 주는 컴포넌트(Mask/RectMask2D/CanvasGroup/Canvas) 여부, 두 텍스트를 그리는 캔버스와
    /// 상대 위치만 남깁니다.</para>
    /// </summary>
    public static partial class HiddenUnlockGuide
    {
        private static bool placementLogged;

        private static void LogPlacementOnce(Text guide, Text artist)
        {
            if (placementLogged) return;
            placementLogged = true;

            Transform parent = guide.transform.parent;
            // 화면 좌표는 첫 계측에서 실제 위치와 맞지 않았습니다(ScreenSpaceCamera 카메라 설정 탓으로 보임).
            // 그래서 아티스트 줄 기준 상대 위치(아티스트 좌표 단위)로 남깁니다. 기대값은 (0, -24) 근처입니다.
            Vector3 offset = artist.rectTransform.InverseTransformPoint(guide.rectTransform.position);

            ModLogger.Msg($"[HiddenGuide.Diag] 아티스트 줄 경로: {DescribeAncestors(artist.transform)}");
            ModLogger.Msg($"[HiddenGuide.Diag] 아티스트: text='{artist.text}', 그리는 캔버스={DescribeCanvas(artist.canvas)}, rect={artist.rectTransform.rect.size}, align={artist.alignment}, preferredH={artist.preferredHeight:0.#}, lossyScale={FormatScale(artist.rectTransform.lossyScale)}");
            ModLogger.Msg($"[HiddenGuide.Diag] 가이드: 부모={(parent != null ? parent.name : "(없음)")} ({guide.transform.GetSiblingIndex() + 1}/{(parent != null ? parent.childCount : 0)}번째), 그리는 캔버스={DescribeCanvas(guide.canvas)}, 아티스트 기준 위치=({offset.x:0.#},{offset.y:0.#}), activeInHierarchy={guide.gameObject.activeInHierarchy}, fontSize={guide.fontSize}, font={(guide.font != null ? guide.font.name : "(null)")}, lossyScale={FormatScale(guide.rectTransform.lossyScale)}");
        }

        private static string DescribeCanvas(Canvas canvas)
        {
            return canvas != null ? $"{canvas.name}(order={canvas.sortingOrder}, override={canvas.overrideSorting})" : "(없음)";
        }

        private static string FormatScale(Vector3 scale)
        {
            return $"({scale.x:0.#####},{scale.y:0.#####})";
        }

        private static string DescribeAncestors(Transform start)
        {
            var sb = new StringBuilder();
            int depth = 0;
            for (Transform t = start; t != null && depth < 10; t = t.parent, depth++)
            {
                if (depth > 0) sb.Append(" <- ");
                sb.Append(t.name);
                if (!t.gameObject.activeSelf) sb.Append("(off)");
                if (t.GetComponent<Mask>() != null) sb.Append("[Mask]");
                if (t.GetComponent<RectMask2D>() != null) sb.Append("[RectMask2D]");
                CanvasGroup group = t.GetComponent<CanvasGroup>();
                if (group != null) sb.Append($"[CanvasGroup a={group.alpha:0.##}]");
                Canvas canvas = t.GetComponent<Canvas>();
                if (canvas != null) sb.Append($"[Canvas order={canvas.sortingOrder}]");
            }
            return sb.ToString();
        }
    }
}
