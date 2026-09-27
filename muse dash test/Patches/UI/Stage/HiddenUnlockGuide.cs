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
    /// 곡 선택창(PnlStage)에서 고른 곡에 히든이 있으면, 위쪽 탭 줄("기본 패키지 Q / 음악 팩 E") 바로 아래에
    /// 해금 조건을 한 줄로 띄웁니다. 예: "히든 해금: Master 버튼 여러 번 연타".
    /// 문구 규칙은 <see cref="HiddenUnlockText"/>, 놓는 자리는 HiddenUnlockGuide.Placement.cs에 있습니다.
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

        // 글자 크기는 아티스트 이름의 0.8배, 색은 연한 금색입니다. 띄우는 간격은 Placement 쪽에 있습니다.
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
            // 아티스트 이름은 글꼴·크기의 기준이자, 탭 줄을 못 찾았을 때의 대체 자리입니다.
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
            // 탭 버튼 구성이나 아티스트 줄 길이가 바뀔 수 있어 매번 다시 맞춥니다.
            Place(guide, pnlStage, artist);
            LogPlacementOnce(guide, pnlStage.transform);
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
