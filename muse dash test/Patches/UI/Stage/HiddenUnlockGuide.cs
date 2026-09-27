using HarmonyLib;
using Il2CppAssets.Scripts.Database;
using Il2CppAssets.Scripts.PeroTools.Commons;
using Il2CppAssets.Scripts.PeroTools.Managers;
using Il2CppAssets.Scripts.UI.Panels;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace muse_dash_test
{
    /// <summary>
    /// 곡 선택창(PnlStage)에서 고른 곡에 히든이 있으면, 아티스트 이름 줄 바로 아래에 해금 조건을 한 줄로 띄웁니다.
    /// 예: "히든 해금: Master 버튼 여러 번 연타". 문구 규칙은 <see cref="HiddenUnlockText"/>에 있습니다.
    ///
    /// <para><b>데이터 출처</b>: 게임 설정 <c>DBConfigHideMusic</c>(에셋 <c>hideMusic</c>)의 곡별
    /// <c>invokeType</c> / <c>diffIndex</c>만 씁니다. 게임은 세션 중 이 설정을 바꾸지 않으므로 처음 한 번
    /// 읽어 uid → 문구 표로 만들어 둡니다.</para>
    ///
    /// <para><b>게임 업데이트 대비</b>: 게임 멤버 접근은 <see cref="ReadHideMusicRules"/> 한 곳에 몰아
    /// 별도 메서드로 두었습니다. 업데이트로 그 멤버가 사라지면 그 메서드만 JIT에 실패하고, 호출부의 try가 잡아
    /// 가이드만 꺼집니다(<see cref="GameBgmSource"/>와 같은 이유). 선택창 자체는 영향을 받지 않습니다.</para>
    /// </summary>
    public static partial class HiddenUnlockGuide
    {
        private const string GuideObjectName = "HwaHiddenUnlockGuide";

        // 위치·크기는 인게임에서 보며 맞출 자리입니다. 아티스트 이름 글자 아래쪽에서 이만큼 띄웁니다(아티스트 줄 좌표 단위).
        private const float GapBelowArtist = 4f;
        private const float FontScale = 0.8f;
        private static readonly Color GuideColor = new Color(1f, 0.86f, 0.35f, 1f);

        // uid → 표시 문구. null이면 아직 못 읽은 상태입니다.
        private static Dictionary<string, string> guideByUid;
        // 읽기가 예외로 실패하면(게임 구조 변경) 곡을 고를 때마다 다시 던지지 않도록 세션 동안 접습니다.
        private static bool tableUnavailable;

        // 만든 문구 오브젝트. 선택창 패널이 파괴되면 Unity 쪽 == null이 참이 되어 다시 만듭니다.
        private static Text guideText;

        /// <summary>선택 곡이 바뀔 때마다 부릅니다. 히든이 없거나 기능이 꺼져 있으면 문구를 숨깁니다.</summary>
        public static void Refresh(PnlStage pnlStage, MusicInfo musicInfo)
        {
            Text artist = pnlStage != null ? pnlStage.artistNameTitle : null;
            if (artist == null) return;

            string text = ModConfig.EnableHiddenGuide && musicInfo != null ? FindGuide(musicInfo.uid) : null;

            if (text == null)
            {
                // 보여줄 게 없으면 새로 만들지 않고, 이미 만든 게 있을 때만 숨깁니다.
                if (guideText != null) guideText.gameObject.SetActive(false);
                return;
            }

            Text guide = GetOrCreateGuideText(pnlStage.transform, artist);
            guide.text = text;
            guide.gameObject.SetActive(true);
            // 곡마다 아티스트 줄 길이와 위치가 달라질 수 있어 매번 다시 맞춥니다.
            PlaceBelowArtist(guide, artist);
            LogPlacementOnce(guide, artist);
        }

        private static string FindGuide(string uid)
        {
            // 모드가 만든 가상 곡(1999-*)은 게임 히든 데이터에 없습니다.
            if (string.IsNullOrEmpty(uid) || CustomContentIds.IsVirtualSong(uid)) return null;

            Dictionary<string, string> table = GetTable();
            return table != null && table.TryGetValue(uid, out string text) ? text : null;
        }

        private static Dictionary<string, string> GetTable()
        {
            if (guideByUid != null || tableUnavailable) return guideByUid;

            var rules = new List<(string Uid, int InvokeType, int DiffIndex)>();
            try
            {
                ReadHideMusicRules(rules);
            }
            catch (Exception ex)
            {
                tableUnavailable = true;
                ModLogger.Warning($"[HiddenGuide] 게임의 히든 해금 데이터를 읽지 못해 가이드를 끕니다(게임 업데이트로 구조가 바뀌었을 수 있음): {ex.GetType().Name}: {ex.Message}");
                return null;
            }

            // 설정이 아직 안 올라온 경우입니다. 빈 표로 굳히지 않고 다음 선택 때 다시 읽습니다.
            if (rules.Count == 0) return null;

            var table = new Dictionary<string, string>(rules.Count, StringComparer.Ordinal);
            foreach (var rule in rules)
            {
                string text = HiddenUnlockText.Describe(rule.InvokeType, rule.DiffIndex);
                if (text != null) table[rule.Uid] = text;
            }

            guideByUid = table;
            ModLogger.Msg($"[HiddenGuide] 히든 해금 데이터 {rules.Count}곡을 읽었습니다(표시 대상 {table.Count}곡).");
            return table;
        }

        /// <summary>
        /// 게임 멤버는 이 메서드에서만 만집니다. 업데이트로 사라지면 JIT 실패가 호출 지점(<see cref="GetTable"/>의 try)에서
        /// 잡히도록 분리해 두었고, 인라인되면 그 의미가 없어지므로 막습니다.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void ReadHideMusicRules(List<(string Uid, int InvokeType, int DiffIndex)> into)
        {
            // DBConfigAlbums를 읽는 곳(CustomTagRegistrySupport)과 같은 경로입니다.
            var config = Singleton<ConfigManager>.instance.GetConfigObject<DBConfigHideMusic>();
            var items = config != null ? config.m_Items : null;
            if (items == null) return;

            for (int i = 0; i < items.Count; i++)
            {
                HideMusicInfo info = items[i];
                if (info == null || string.IsNullOrEmpty(info.musicUid)) continue;
                into.Add((info.musicUid, info.invokeType, info.diffIndex));
            }
        }

        /// <summary>
        /// 문구 오브젝트를 선택창 패널 최상위(<paramref name="panelRoot"/>)의 마지막 자식으로 만듭니다.
        ///
        /// <para><b>아티스트 줄의 자식으로 두지 않는 이유</b>: 처음에는 그렇게 만들었는데 인게임에서 보이지 않았습니다
        /// (2026-09-27, 히든 곡 0-11 선택, 데이터 로드·예외 없음 확인). 이 게임은 긴 곡명을 <c>Mask</c> +
        /// <c>LongSongNameController</c> 안에서 스크롤시키는데(곡 목록 셀 프리팹에서 확인), 헤더도 그렇다면
        /// 줄 아래에 붙인 자식은 마스크 밖이라 통째로 잘립니다. 뒤에 그려지는 다른 UI에 가려졌을 수도 있습니다.
        /// 패널 최상위의 마지막 자식은 두 경우 모두 피합니다. 선택창이 닫히면 여전히 같이 사라집니다.</para>
        /// </summary>
        private static Text GetOrCreateGuideText(Transform panelRoot, Text artist)
        {
            if (guideText != null) return guideText;

            var go = new GameObject(GuideObjectName);
            go.transform.SetParent(panelRoot, false);

            Text guide = go.AddComponent<Text>();
            guide.font = artist.font; // 게임 폰트라 한글이 그대로 나옵니다.
            guide.fontSize = Mathf.Max(12, Mathf.RoundToInt(artist.fontSize * FontScale));
            // 선택창 헤더가 가운데 정렬이라(스크린샷 기준) 가운데 위쪽에 맞춥니다. 위치는 PlaceBelowArtist가 정합니다.
            guide.alignment = TextAnchor.UpperCenter;
            guide.color = GuideColor;
            guide.horizontalOverflow = HorizontalWrapMode.Overflow;
            guide.verticalOverflow = VerticalWrapMode.Overflow;
            // 문구가 난이도 버튼 위에 겹쳐도 클릭을 가로채면 안 됩니다. 히든 해금이 바로 그 버튼 연타입니다.
            guide.raycastTarget = false;

            var outline = go.AddComponent<Outline>();
            outline.effectColor = new Color(0f, 0f, 0f, 0.8f);
            outline.effectDistance = new Vector2(1.5f, -1.5f);

            RectTransform rect = go.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0.5f, 1f);

            guideText = guide;
            return guide;
        }

        /// <summary>
        /// 문구의 위쪽 가운데를 아티스트 이름 글자의 아래쪽 가운데에 맞춥니다.
        /// 부모가 달라도 월드 좌표로 맞추므로 계층과 상관없이 같은 자리에 옵니다. 크기도 아티스트 줄의 실제 배율을 따릅니다.
        /// </summary>
        private static void PlaceBelowArtist(Text guide, Text artist)
        {
            RectTransform artistRect = artist.rectTransform;
            RectTransform guideRect = guide.rectTransform;
            Rect r = artistRect.rect;

            // 아티스트 줄의 사각형이 글자보다 클 수 있어서, 정렬 방식으로 실제 글자 아래쪽을 계산합니다.
            float textHeight = Mathf.Min(artist.preferredHeight, r.height > 0f ? r.height : artist.preferredHeight);
            int vertical = (int)artist.alignment / 3; // TextAnchor: 0~2 Upper, 3~5 Middle, 6~8 Lower
            float glyphBottom = vertical == 0 ? r.yMax - textHeight
                : vertical == 1 ? r.center.y - textHeight / 2f
                : r.yMin;

            // 크기: 아티스트 줄과 같은 월드 배율이 되도록 부모 배율로 나눕니다.
            Vector3 artistScale = artistRect.lossyScale;
            Vector3 parentScale = guideRect.parent != null ? guideRect.parent.lossyScale : Vector3.one;
            guideRect.localScale = new Vector3(
                parentScale.x != 0f ? artistScale.x / parentScale.x : 1f,
                parentScale.y != 0f ? artistScale.y / parentScale.y : 1f,
                1f);
            guideRect.sizeDelta = new Vector2(Mathf.Max(r.width, 600f), guide.fontSize * 1.3f);

            guideRect.position = artistRect.TransformPoint(new Vector3(r.center.x, glyphBottom - GapBelowArtist, 0f));
            // 같은 패널 안에서 가장 나중에 그려지게 합니다(다른 UI에 가려지지 않게).
            guideRect.SetAsLastSibling();
        }
    }

    // 곡을 고를 때마다 불리는 자리입니다(PnlStagePatch의 RefreshDiffUI 패치와 같은 대상, 역할만 분리).
    [HarmonyPatch(typeof(PnlStage), "RefreshDiffUI", new Type[] { typeof(MusicInfo) })]
    public class PnlStage_RefreshDiffUI_HiddenGuidePatch
    {
        public static void Postfix(PnlStage __instance, MusicInfo musicInfo)
        {
            // FeatureGuard가 연속 실패 시 로그를 한 번만 남기고 자동으로 꺼 줍니다.
            // EnableHiddenGuide는 FeatureMap 게이트로 걸지 않고 Refresh 안에서 봅니다. 게이트로 건너뛰면
            // 이미 띄워 둔 문구를 숨기는 코드까지 같이 건너뛰기 때문입니다(CHECKLIST "게이트를 쓰는 쪽에만").
            FeatureGuard.Run("UI.HiddenGuide", () => HiddenUnlockGuide.Refresh(__instance, musicInfo));
        }
    }
}
