using Il2CppAssets.Scripts.GameCore.Managers;
using Il2CppAssets.Scripts.PeroTools.Commons;
using UnityEngine;
using UnityEngine.UI;

namespace muse_dash_test
{
    /// <summary>
    /// 좌상단 스킬 아이콘(불량소녀 린 GREAT→PERFECT, 라봇-233 MISS 살리기)에 남은 횟수 숫자를 붙입니다.
    ///
    /// <para><b>왜 직접 붙이나</b>: 게임 UI에는 이 횟수를 그리는 숫자 칸이 없습니다. 아이콘은
    /// <c>PnlBattle/…/Score/SkillIconGrid</c> 아래에서 이벤트(<c>Battle/OnGreat2Perfect</c>·<c>OnMiss2Great</c>)로
    /// 켜질 뿐이고, 블러드 상속자 쪽 코드의 <c>m_ImgCarrotRobotSkill_Img5</c>처럼 "5"는 그림에 박혀 있습니다
    /// (2026-10-25 UI 진단: 숫자만 든 Text는 이 그리드에 하나도 없었습니다). 횟수를 999로 바꾸면 보여줄 곳이 없어서,
    /// 아이콘마다 Text 자식을 하나 만들어 <c>BattleProperty</c>의 남은 횟수를 씁니다.</para>
    ///
    /// <para>그리드는 게임이 참조를 들고 있지 않아 전체 경로로 찾습니다(이름 부분 일치 아님). 아이콘 종류는 이름으로
    /// 가립니다. 이름 규칙이 맞지 않으면 숫자를 붙이지 않고, 그리드 구성을 판마다 한 번 로그로 남깁니다.</para>
    /// </summary>
    internal static class SkillCounterBadge
    {
        private const string GridPath = "UI_2D/Standard/PnlBattle/PnlBattleUI/PnlBattleOthers/Score/SkillIconGrid";
        private const string BadgeName = "HwaSkillCount";
        private const float FindRetrySeconds = 1f;
        private const int MaxDumpLines = 30;

        private static Transform grid;
        private static float nextFindTime;
        private static bool dumped;
        private static int shownGreat = int.MinValue;
        private static int shownMiss = int.MinValue;

        /// <summary>MainMod.OnUpdate에서 매 프레임 부릅니다. 그리드를 못 찾으면 1초마다만 다시 찾습니다.</summary>
        internal static void Update()
        {
            if (!Patches.UI.Role.CustomRoleSlotExperiment.IsClonedRoleActive()) return;

            if (grid == null)
            {
                if (Time.unscaledTime < nextFindTime) return;
                nextFindTime = Time.unscaledTime + FindRetrySeconds;
                var go = GameObject.Find(GridPath);
                if (go == null) return;
                grid = go.transform;
                dumped = false;
                shownGreat = shownMiss = int.MinValue;
            }

            var property = Singleton<BattleProperty>.instance;
            if (property == null) return;
            int great = property.greatToPerfect;
            int miss = property.missToGreat;
            if (great == shownGreat && miss == shownMiss) return;

            bool allKnown = true;
            for (int i = 0; i < grid.childCount; i++)
            {
                Transform icon = grid.GetChild(i);
                int? count = ClassifyIcon(icon.name) switch
                {
                    IconKind.GreatToPerfect => great,
                    IconKind.MissRescue => miss,
                    _ => (int?)null,
                };
                if (count == null)
                {
                    allKnown = false;
                    continue;
                }
                GetOrCreateBadge(icon).text = count.Value.ToString();
            }

            shownGreat = great;
            shownMiss = miss;
            if (!allKnown && !dumped) DumpGrid();
        }

        private enum IconKind { Unknown, GreatToPerfect, MissRescue }

        private static IconKind ClassifyIcon(string name)
        {
            string n = name.ToLowerInvariant();
            if (n.Contains("robot") || n.Contains("carrot") || n.Contains("rabbot") || n.Contains("miss")) return IconKind.MissRescue;
            if (n.Contains("rampage") || n.Contains("great") || n.Contains("perfect")) return IconKind.GreatToPerfect;
            return IconKind.Unknown;
        }

        private static Text GetOrCreateBadge(Transform icon)
        {
            Transform existing = icon.Find(BadgeName);
            if (existing != null) return existing.GetComponent<Text>();

            var go = new GameObject(BadgeName);
            go.transform.SetParent(icon, false);

            var iconRect = icon.GetComponent<RectTransform>();
            float iconHeight = iconRect != null && iconRect.rect.height > 1f ? iconRect.rect.height : 80f;

            Text text = go.AddComponent<Text>();
            text.font = FindFont();
            text.fontSize = Mathf.Max(14, Mathf.RoundToInt(iconHeight * 0.38f));
            text.alignment = TextAnchor.LowerRight;
            text.color = Color.white;
            text.horizontalOverflow = HorizontalWrapMode.Overflow;
            text.verticalOverflow = VerticalWrapMode.Overflow;
            text.raycastTarget = false;

            var outline = go.AddComponent<Outline>();
            outline.effectColor = new Color(0f, 0f, 0f, 0.9f);
            outline.effectDistance = new Vector2(2f, -2f);

            // 아이콘 오른쪽 아래 모서리에 붙입니다.
            RectTransform rect = go.GetComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
            return text;
        }

        /// <summary>점수판 근처 게임 Text의 폰트를 빌려 씁니다. 없으면 Unity 내장 Arial.</summary>
        private static Font FindFont()
        {
            Transform score = grid != null ? grid.parent : null;
            if (score != null)
            {
                foreach (var t in score.GetComponentsInChildren<Text>(true))
                    if (t.font != null && t.gameObject.name != BadgeName) return t.font;
            }
            return Resources.GetBuiltinResource<Font>("Arial.ttf");
        }

        /// <summary>이름 규칙에 안 맞는 아이콘이 있을 때, 그리드 구성(깊이 2)을 판마다 한 번 남깁니다.</summary>
        private static void DumpGrid()
        {
            dumped = true;
            int lines = 0;
            for (int i = 0; i < grid.childCount && lines < MaxDumpLines; i++)
            {
                Transform icon = grid.GetChild(i);
                ModLogger.Msg($"[SkillCounter.Badge] 아이콘 '{icon.name}' active={icon.gameObject.activeSelf} 분류={ClassifyIcon(icon.name)} sprite={SpriteName(icon)}");
                lines++;
                for (int j = 0; j < icon.childCount && lines < MaxDumpLines; j++)
                {
                    Transform child = icon.GetChild(j);
                    ModLogger.Msg($"[SkillCounter.Badge]   └ '{child.name}' active={child.gameObject.activeSelf} sprite={SpriteName(child)}");
                    lines++;
                }
            }
            ModLogger.Msg($"[SkillCounter.Badge] 이름으로 종류를 가리지 못한 아이콘이 있어 구성을 남겼습니다. ({lines}줄)");
        }

        private static string SpriteName(Transform t)
        {
            var image = t.GetComponent<Image>();
            return image != null && image.sprite != null ? image.sprite.name : "-";
        }
    }
}
