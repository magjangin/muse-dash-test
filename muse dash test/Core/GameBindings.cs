namespace muse_dash_test
{
    /// <summary>
    /// 게임 버전에 종속된 "raw 문자열 식별자"의 단일 소스(Single Source of Truth)입니다.
    ///
    /// 게임이 업데이트되어 패치가 깨지면 가장 먼저 이 파일을 확인/수정하세요.
    /// 흩어져 있던 메서드명 문자열을 한곳에 모아, 업데이트 시 수정 지점을 하나로 만듭니다.
    /// 시작 시 <see cref="PatchHealthCheck"/>가 이 식별자들의 실제 해석 여부를 점검하고,
    /// 런타임 실패는 <see cref="FeatureGuard"/>가 기능 단위로 격리합니다.
    ///
    /// 주의 — 여기 모은 것은 "컴파일러가 검증하지 못하는" raw 문자열뿐입니다.
    /// typeof(...)나 nameof(...) 기반 패치 대상(예: nameof(MusicTagManager.InitDatas))은
    /// 빌드가 즉시 잡아주므로 더 안전하며, 이 파일에 중복으로 둘 필요가 없습니다.
    /// 향후 가능한 항목은 raw 문자열 대신 nameof(...)로 전환하는 것이 가장 강한 내성입니다.
    /// </summary>
    public static class GameBindings
    {
        /// <summary>Il2CppAssets.Scripts.GameCore.HostComponent.TaskStageTarget — 점수/정확도/풀콤보 판정.</summary>
        public static class TaskStageTarget
        {
            public const string AddScore = "AddScore";
            public const string GetAccuracy = "GetAccuracy";
            public const string GetTrueAccuracy = "GetTrueAccuracy";
            public const string GetTrueAccuracyNew = "GetTrueAccuracyNew";
            public const string IsFullCombo = "IsFullCombo";
        }

        /// <summary>Il2CppAssets.Scripts.UI.GameMain.PnlVictory2dManager — 결과(승리) 화면.</summary>
        public static class PnlVictory2dManager
        {
            public const string OnShowVictory = "OnShowVictory";
        }

        /// <summary>Il2Cpp.PnlBattle — 인게임 배틀 패널.</summary>
        public static class PnlBattle
        {
            /// <summary>리플렉션 타입 탐색용 단순 타입명(<see cref="PnlBattle_GameStart_Patch"/>에서 사용).</summary>
            public const string TypeName = "PnlBattle";
            public const string MusicProgressInit = "MusicProgressInit";
            public const string GameStart = "GameStart";
        }

        /// <summary>Il2Cpp.SetSelectedMusicNameTxt — 선택 곡명 텍스트. (Awake/OnEnable은 Unity 메시지로 비교적 안정적)</summary>
        public static class SetSelectedMusicNameTxt
        {
            public const string Awake = "Awake";
            public const string OnEnable = "OnEnable";
        }

        /// <summary>Il2Cpp.ChangeHealthValue — 체력 변화 이벤트 핸들러.</summary>
        public static class ChangeHealthValue
        {
            public const string OnGameStart = "OnGameStart";
            public const string OnHpRateChange = "OnHpRateChange";
            public const string OnHpDeduct = "OnHpDeduct";
            public const string OnHpAdd = "OnHpAdd";
        }

        /// <summary>Il2Cpp.AlbumTagToggle — 커스텀 앨범 태그 토글 UI.</summary>
        public static class AlbumTagToggle
        {
            public const string Init = "Init";
            public const string SetIconAsync = "SetIconAsync";
            public const string SetStateIcon = "SetStateIcon";
        }

        /// <summary>MusicTagManager — 태그/곡 목록 관리.</summary>
        public static class MusicTagManager
        {
            public const string RefreshStageDisplayMusics = "RefreshStageDisplayMusics";
        }

        /// <summary>Il2Cpp 컬렉션 마스크 래퍼(<see cref="Il2CppWrapperBase"/>에서 리플렉션 호출).</summary>
        public static class Il2CppCollection
        {
            public const string AddMaskValue = "AddMaskValue";
        }
    }
}
