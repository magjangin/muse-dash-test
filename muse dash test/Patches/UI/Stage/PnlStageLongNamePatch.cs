using HarmonyLib;
using System;
using Il2Cpp;
using Il2CppAssets.Scripts.UI.Panels;

namespace muse_dash_test
{
    /// <summary>
    /// 곡 선택 화면 하단의 곡명/작곡가 줄(PnlStage.musicLongNameController / artistLongNameController)에
    /// 가상 곡의 커스텀 제목을 넣습니다.
    ///
    /// <para><b>왜 필요한가</b> — RefreshDiffUI Postfix가 musicNameTitle.text만 바꿔 왔는데, 긴 이름은
    /// LongSongNameController가 simple/backup 두 텍스트로 흘려 보내며 그립니다. 숙주(0-4)의 제목이
    /// 길어 흐름 애니메이션이 켜진 상태에서는 화면에 숙주 제목(…feat.yousa 恋爱语音导航)이 그대로 흘렀습니다.
    /// 게임이 이 컨트롤러에 넘기는 문자열 자체를 바꿔야 두 텍스트와 폭 계산이 같이 맞습니다.</para>
    ///
    /// <para>PnlStage의 두 컨트롤러만 대상입니다. 곡 목록 셀(MusicButtonCell)도 같은 컨트롤러를 쓰는데,
    /// 그쪽은 다른 곡의 이름을 그리는 중일 수 있어 건드리면 안 됩니다.</para>
    /// </summary>
    internal static class PnlStageLongNameOverride
    {
        /// <summary>
        /// 이 컨트롤러가 선택 곡의 곡명/작곡가 줄이고 선택 곡이 가상 곡이면 커스텀 값을 돌려줍니다.
        /// </summary>
        internal static bool TryGetOverride(LongSongNameController controller, out string value)
        {
            value = null;
            if (controller == null) return false;

            string uid = CustomPlaySession.Current.LastKnownMusicUid;
            if (!CustomContentIds.IsVirtualSong(uid)) return false;

            var stage = controller.GetComponentInParent<PnlStage>();
            if (stage == null) return false;

            bool isTitle = IsSame(stage.musicLongNameController, controller);
            bool isArtist = !isTitle && IsSame(stage.artistLongNameController, controller);
            if (!isTitle && !isArtist) return false;

            if (!HwaResourceManager.TryGetHwaPrimarySong(
                    uid, out string title, out string artist,
                    out _, out _, out _, out _, out _, out _, out _))
            {
                return false;
            }

            value = isTitle ? title : artist;
            return !string.IsNullOrWhiteSpace(value);
        }

        internal static void Apply(string source, LongSongNameController controller, ref string text)
        {
            try
            {
                if (!TryGetOverride(controller, out string value) || text == value) return;

                ModLogger.Verbose($"[{source}] PnlStage 곡명 줄 텍스트 교체: '{text}' -> '{value}'");
                text = value;
            }
            catch (Exception ex)
            {
                ModLogger.Error($"[{source}] 커스텀 곡명 적용 예외: {ex}");
            }
        }

        private static bool IsSame(LongSongNameController a, LongSongNameController b)
        {
            return a != null && b != null && a.Pointer == b.Pointer;
        }
    }

    [HarmonyPatch(typeof(LongSongNameController), nameof(LongSongNameController.Refresh),
        new Type[] { typeof(string), typeof(bool), typeof(float) })]
    internal static class LongSongNameController_Refresh_Patch
    {
        private static void Prefix(LongSongNameController __instance, ref string text)
            => PnlStageLongNameOverride.Apply("LongSongNameController.Refresh", __instance, ref text);
    }

    [HarmonyPatch(typeof(LongSongNameController), nameof(LongSongNameController.RefreshText),
        new Type[] { typeof(string) })]
    internal static class LongSongNameController_RefreshText_Patch
    {
        private static void Prefix(LongSongNameController __instance, ref string txt)
            => PnlStageLongNameOverride.Apply("LongSongNameController.RefreshText", __instance, ref txt);
    }

    [HarmonyPatch(typeof(LongSongNameController), nameof(LongSongNameController.RefreshForBestFit),
        new Type[] { typeof(string), typeof(float) })]
    internal static class LongSongNameController_RefreshForBestFit_Patch
    {
        private static void Prefix(LongSongNameController __instance, ref string txt)
            => PnlStageLongNameOverride.Apply("LongSongNameController.RefreshForBestFit", __instance, ref txt);
    }
}
