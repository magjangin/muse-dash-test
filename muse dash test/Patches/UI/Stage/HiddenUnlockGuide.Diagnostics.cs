using Il2CppAssets.Scripts.UI.Panels;
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

        private static bool tagBarLogged;

        // 진단 전용입니다. 한국어 클라이언트에서 탭 줄("기본 패키지 Q / 음악 팩 E")의 글자를 찾아 그 계층을 남깁니다.
        // 배치에는 쓰지 않습니다(글자는 언어마다 다르고, CHECKLIST는 표시 내용·이름 부분 일치로 오브젝트를 고르지
        // 말라고 합니다). 이 로그로 탭 줄 오브젝트의 경로를 확정한 뒤, 배치는 그 경로로 합니다.
        private static readonly string[] TagBarProbeTexts = { "기본 패키지", "음악 팩" };
        private const int MaxTagBarCandidates = 12;

        /// <summary>
        /// 곡 선택창 위쪽 탭 줄의 계층을 세션당 한 번 남깁니다. 탭 줄은 헤더처럼 게임 씬에 직접 들어 있어
        /// 에셋 번들에서 찾을 수 없었습니다(번들의 BtnTagL/BtnQ는 PnlMusicTag·PnlLevelConfig 소속이었음).
        /// </summary>
        private static void LogTagBarCandidatesOnce(PnlStage pnlStage)
        {
            if (tagBarLogged) return;
            tagBarLogged = true;

            // 진단이 실패해도 문구 표시는 막지 않습니다.
            try
            {
                var texts = pnlStage.GetComponentsInChildren<Text>(true);
                int found = 0;
                foreach (Text t in texts)
                {
                    if (t == null || !ContainsAnyProbe(t.text)) continue;
                    ModLogger.Msg($"[HiddenGuide.Diag] 탭 후보 '{t.text}' 캔버스={DescribeCanvas(t.canvas)}: {DescribeAncestors(t.transform, pnlStage.transform)}");
                    if (++found >= MaxTagBarCandidates) break;
                }

                GameObject albumTitle = pnlStage.m_AlbumTitleObj;
                ModLogger.Msg($"[HiddenGuide.Diag] 탭 후보 {found}개(Text {texts.Length}개 중). 참고 m_AlbumTitleObj: {(albumTitle != null ? DescribeAncestors(albumTitle.transform, pnlStage.transform) : "(null)")}");
            }
            catch (System.Exception ex)
            {
                ModLogger.Warning($"[HiddenGuide.Diag] 탭 줄 조사 실패: {ex.GetType().Name}: {ex.Message}");
            }
        }

        private static bool ContainsAnyProbe(string text)
        {
            if (string.IsNullOrEmpty(text)) return false;
            foreach (string probe in TagBarProbeTexts)
            {
                if (text.Contains(probe)) return true;
            }
            return false;
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
