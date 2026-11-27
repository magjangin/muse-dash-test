using System;
using System.Collections.Generic;

namespace muse_dash_test
{
    /// <summary>
    /// 세이브의 '마지막 선택' 값(<c>Account</c>)이 모드 전용 콘텐츠를 가리키는지 판정하고, 바닐라가 읽을 수 있는 값으로 바꾸는 규칙입니다.
    /// 게임에 의존하지 않아 로직 테스트로 검증합니다. 실제 교체는 <c>SaveDataManagerPatch</c>가 저장 직전에 하고 저장 뒤 되돌립니다.
    ///
    /// <para><b>왜</b>: 모드에서 커스텀 태그·곡을 고른 채 저장하면 <c>SelectedAlbumTagIndex=1999</c>,
    /// <c>SelectedAlbumUid="1999-0"</c>, <c>SelectedMusicUidFromInfoList="1999-1"</c>이 남습니다. 모드를 빼면 바닐라가 시작할 때
    /// <c>MusicTagManager.InitDatas → RefreshStageDisplayMusics(1999)</c>에서 NRE가 나 곡 선택 화면이 빈 채로 멈춥니다
    /// (2026-11-20 사용자 제보, <c>Player.log</c> 확인). 모드는 <c>MusicTagManager_RefreshStageDisplayMusics_Patch</c>로 이 인덱스를 건너뛰어 왔습니다.
    /// 예전 정화(<c>CleanCollectionFields</c>)는 키 이름만 봐서 값으로 가리키는 경우를 놓쳤습니다.</para>
    /// </summary>
    public static class SaveSelectionRules
    {
        public const string TagIndexKey = "SelectedAlbumTagIndex";
        public const string AlbumUidKey = "SelectedAlbumUid";
        public const string MusicUidKey = "SelectedMusicUidFromInfoList";

        /// <summary>함께 바꾸는 키입니다. 셋이 서로 맞아야 하므로 하나라도 모드 값이면 셋을 같이 바꿉니다.</summary>
        public static readonly string[] Keys = { TagIndexKey, AlbumUidKey, MusicUidKey };

        /// <summary>바닐라 기본값: 첫 태그, 기본 앨범(<c>music_package_0</c> = Default Music, 곡 <c>0-x</c>), 그 앨범의 첫 곡.</summary>
        public static readonly IReadOnlyDictionary<string, string> VanillaDefaults = new Dictionary<string, string>(StringComparer.Ordinal)
        {
            { TagIndexKey, "0" },
            { AlbumUidKey, "music_package_0" },
            { MusicUidKey, CustomContentIds.FallbackSourceMusicUid },
        };

        /// <summary>이 키의 값(문자열로 읽은 것)이 모드 전용 콘텐츠를 가리키는지.</summary>
        public static bool PointsToModContent(string key, string valueText)
        {
            if (valueText == null) return false;
            if (key == TagIndexKey)
            {
                return valueText.Trim() == CustomContentIds.TagIndex.ToString(System.Globalization.CultureInfo.InvariantCulture);
            }
            if (key == AlbumUidKey || key == MusicUidKey)
            {
                return CustomContentIds.IsVirtualContent(valueText);
            }
            return false;
        }

        /// <summary>읽은 값 묶음(키 → 문자열) 중 하나라도 모드 값인지.</summary>
        public static bool AnyPointsToModContent(IReadOnlyDictionary<string, string> values)
        {
            if (values == null) return false;
            foreach (var pair in values)
            {
                if (PointsToModContent(pair.Key, pair.Value)) return true;
            }
            return false;
        }

        /// <summary>
        /// 저장할 바닐라 값입니다. 이번 실행에서 본 마지막 바닐라 선택(<paramref name="lastVanilla"/>, 셋이 모두 바닐라였던 때)이 있으면
        /// 그것을, 없으면 <see cref="VanillaDefaults"/>를 씁니다. 모드를 뺀 뒤 마지막으로 고르던 공식 곡으로 돌아가게 하려는 것입니다.
        /// </summary>
        public static string ReplacementFor(string key, IReadOnlyDictionary<string, string> lastVanilla)
        {
            if (lastVanilla != null && lastVanilla.TryGetValue(key, out var remembered) && !string.IsNullOrEmpty(remembered)
                && !PointsToModContent(key, remembered))
            {
                return remembered;
            }
            return VanillaDefaults.TryGetValue(key, out var fallback) ? fallback : null;
        }
    }
}
