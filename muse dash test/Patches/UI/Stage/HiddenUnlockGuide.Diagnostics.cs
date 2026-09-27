using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace muse_dash_test
{
    /// <summary>
    /// 가이드 문구가 실제로 어디에 그려지는지 세션당 한 번 남기는 계측입니다.
    ///
    /// <para><b>왜 있나</b>: 첫 구현(아티스트 줄의 자식)이 인게임에서 보이지 않았는데, 선택창 헤더는 게임 씬에
    /// 직접 들어 있어 에셋 번들로는 계층을 확인할 수 없었습니다. 마스크에 잘렸는지, 다른 UI에 가려졌는지,
    /// 화면 밖에 있는지를 추측하지 않고 이 로그로 확정합니다. 원인이 확정되고 표시가 확인되면 걷어내도 됩니다.</para>
    ///
    /// <para>CHECKLIST에 따라 객체를 훑는 깊은 덤프는 하지 않습니다. 아티스트 줄의 조상 최대 10단계의 이름과
    /// 표시에 영향을 주는 컴포넌트(Mask/RectMask2D/CanvasGroup/Canvas) 여부, 두 텍스트의 화면 좌표만 남깁니다.</para>
    /// </summary>
    public static partial class HiddenUnlockGuide
    {
        private static bool placementLogged;

        private static void LogPlacementOnce(Text guide, Text artist)
        {
            if (placementLogged) return;
            placementLogged = true;

            Canvas canvas = artist.canvas != null ? artist.canvas.rootCanvas : null;
            Camera cam = canvas != null && canvas.renderMode != RenderMode.ScreenSpaceOverlay ? canvas.worldCamera : null;
            string canvasInfo = canvas != null ? $"{canvas.name} {canvas.renderMode} order={canvas.sortingOrder}" : "(없음)";

            Transform parent = guide.transform.parent;
            ModLogger.Msg($"[HiddenGuide.Diag] 아티스트 줄 경로: {DescribeAncestors(artist.transform)}");
            ModLogger.Msg($"[HiddenGuide.Diag] 캔버스={canvasInfo}, 화면={Screen.width}x{Screen.height}");
            ModLogger.Msg($"[HiddenGuide.Diag] 아티스트: text='{artist.text}', 화면중심={ScreenPoint(cam, artist.rectTransform.position)}, rect={artist.rectTransform.rect.size}, align={artist.alignment}, preferredH={artist.preferredHeight:0.#}, lossyScale={artist.rectTransform.lossyScale}");
            ModLogger.Msg($"[HiddenGuide.Diag] 가이드: 부모={(parent != null ? parent.name : "(없음)")} ({guide.transform.GetSiblingIndex() + 1}/{(parent != null ? parent.childCount : 0)}번째), 화면위쪽={ScreenPoint(cam, guide.rectTransform.position)}, activeInHierarchy={guide.gameObject.activeInHierarchy}, fontSize={guide.fontSize}, font={(guide.font != null ? guide.font.name : "(null)")}, lossyScale={guide.rectTransform.lossyScale}");
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

        private static string ScreenPoint(Camera cam, Vector3 world)
        {
            Vector2 p = RectTransformUtility.WorldToScreenPoint(cam, world);
            return $"({p.x:0},{p.y:0})";
        }
    }
}
