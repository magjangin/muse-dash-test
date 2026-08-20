using MelonLoader;
using System.Collections.Generic;
using UnityEngine;

namespace muse_dash_test
{
    /// <summary>
    /// "지금 선택된 곡 하나를 그리는 중"인 구간을 표시하는 스코프입니다.
    ///
    /// <para><b>왜 필요한가</b> — 현지화 DB는 곡을 UID가 아니라 <b>행 번호(index)</b>로 물어봅니다
    /// (<c>DBConfigLocalALBUM.GetLocalAlbumInfoByIndex</c>, <c>DBConfigLocalAlbums.GetLocalTitleByIndex</c>).
    /// 가상 곡은 숙주의 얇은 복제본이라 행 번호도 숙주 것을 그대로 물려받기 때문에
    /// (실측: <c>musicIndex=4</c>), "그 번호로 들어온 조회에 커스텀 이름을 답한다"로만 막으면
    /// <b>다른 곡을 대상으로 한 조회까지</b> 가로채 버립니다. il2cpp는 호출자가 스택에 남지 않아
    /// 조회만 봐서는 "누구를 위한 질문인지" 알 수 없으므로, 답해도 되는 구간을 호출자 쪽에서 열어 줍니다.</para>
    ///
    /// <para><b>실측 근거(26-8-12_8-43-14.log)</b> — 같은 <c>index=4</c> 조회가 두 종류로 들어옵니다.
    /// <list type="bullet">
    /// <item>10:05:36.565 — 직후 <c>PnlStage.RefreshDiffUI -> [1999-1]</c>. 선택 곡 자신을 위한 조회(가로채야 함).</item>
    /// <item>10:05:44.258 / 10:05:46.700 / 10:06:04.056 / 10:06:07.430 — 앞뒤로 <c>MusicButtonAreaTitle</c>만 있고
    /// <c>ChangeMusic</c>·<c>RefreshDiffUI</c>가 없음. 곡 목록이 <b>다른 곡</b>의 이름을 묻는 조회(가로채면 안 됨).
    /// 이것이 곡 인덱스 화면에서 엉뚱한 곡들이 '아기상어'로 보이던 원인입니다.</item>
    /// </list></para>
    ///
    /// <para>스코프는 <see cref="Enter"/>/<see cref="Exit"/>가 짝이 맞지 않아도(원본이 예외로 빠져나가는 경우)
    /// 프레임이 바뀌면 스스로 닫힙니다. 열린 채로 고착되면 버그가 그대로 돌아오기 때문입니다.</para>
    /// </summary>
    internal static class SelectedSongLocalizationScope
    {
        private static int _depth;
        private static int _frame;
        private static readonly HashSet<string> _loggedRefusals = new HashSet<string>();

        /// <summary>지금 선택 곡 한 곡을 그리는 중이라 인덱스 기반 조회에 답해도 되는지 여부입니다.</summary>
        public static bool IsActive => _depth > 0 && Time.frameCount == _frame;

        public static void Enter()
        {
            // 프레임이 넘어갔다면 짝이 맞지 않은 채 남은 깊이이므로 버립니다(스코프 고착 방지).
            if (_depth > 0 && Time.frameCount != _frame) _depth = 0;

            _frame = Time.frameCount;
            _depth++;
        }

        public static void Exit()
        {
            if (_depth > 0) _depth--;
        }

        /// <summary>
        /// 스코프 밖이라 가로채지 않고 원본에 넘긴 조회를 호출자별 1회만 기록합니다.
        /// 이 로그가 보이는데 화면에 커스텀 이름이 안 나오는 곳이 생겼다면,
        /// 그 화면을 그리는 메서드를 스코프 진입점에 추가해야 한다는 뜻입니다.
        /// </summary>
        public static void LogRefusedOnce(string caller, int index, string detail)
        {
            if (!_loggedRefusals.Add(caller)) return;

            ModLogger.Msg($"[{caller}] 스코프 밖에서 들어온 index={index} 조회는 가로채지 않고 원본에 넘겼습니다 " +
                            $"({detail}). 이 로그는 호출 지점당 1회만 표시됩니다.");
        }
    }
}
