using MelonLoader;
using System;
using System.Collections.Generic;

namespace muse_dash_test
{
    /// <summary>
    /// 모드의 각 기능을 개별적으로 On/Off 제어할 수 있는 통합 설정 클래스입니다.
    /// MelonPreferences와 연동하여 UserData/MelonPreferences.cfg 파일에 자동 저장/로드됩니다.
    /// </summary>
    public static class ModConfig
    {
        public static MelonPreferences_Category Category { get; private set; }

        public static MelonPreferences_Entry<bool> CustomChartEntry { get; private set; }
        public static MelonPreferences_Entry<bool> RealTimeSwapEntry { get; private set; }
        public static MelonPreferences_Entry<bool> InputOverlayEntry { get; private set; }
        public static MelonPreferences_Entry<bool> JudgmentBarEntry { get; private set; }
        public static MelonPreferences_Entry<bool> DiscordRpcEntry { get; private set; }
        public static MelonPreferences_Entry<bool> HpTextModEntry { get; private set; }
        public static MelonPreferences_Entry<bool> ApModEntry { get; private set; }
        public static MelonPreferences_Entry<bool> AllPerfectSoundEntry { get; private set; }
        public static MelonPreferences_Entry<bool> AutoPlayEntry { get; private set; }
        public static MelonPreferences_Entry<bool> ForcePerfectEntry { get; private set; }
        public static MelonPreferences_Entry<bool> BattleMediaEntry { get; private set; }
        public static MelonPreferences_Entry<bool> SpineSkinEntry { get; private set; }
        public static MelonPreferences_Entry<bool> MobileTouchEntry { get; private set; }
        public static MelonPreferences_Entry<bool> VerboseLogEntry { get; private set; }

        public static bool EnableCustomChart => CustomChartEntry?.Value ?? true;
        public static bool EnableRealTimeSwap => RealTimeSwapEntry?.Value ?? true;
        public static bool EnableInputOverlay => InputOverlayEntry?.Value ?? true;
        public static bool EnableJudgmentBar => JudgmentBarEntry?.Value ?? true;
        public static bool EnableDiscordRPC => DiscordRpcEntry?.Value ?? true;
        public static bool EnableHpTextMod => HpTextModEntry?.Value ?? true;
        public static bool EnableAPMod => ApModEntry?.Value ?? true;
        public static bool EnableAllPerfectSound => AllPerfectSoundEntry?.Value ?? true;
        public static bool EnableAutoPlay => AutoPlayEntry?.Value ?? true;
        public static bool EnableForcePerfect => ForcePerfectEntry?.Value ?? true;
        public static bool EnableBattleMedia => BattleMediaEntry?.Value ?? true;
        public static bool EnableSpineSkin => SpineSkinEntry?.Value ?? true;
        public static bool EnableMobileTouch => MobileTouchEntry?.Value ?? true;
        public static bool EnableVerboseLog => VerboseLogEntry?.Value ?? false;

        private static readonly Dictionary<string, Func<bool>> FeatureMap = new Dictionary<string, Func<bool>>(StringComparer.OrdinalIgnoreCase);

        public static void Load()
        {
            if (Category != null) return;

            Category = MelonPreferences.CreateCategory("muse-dash-custom-chart-features", "Muse Dash Mod Feature Toggles");

            CustomChartEntry = Category.CreateEntry("EnableCustomChart", true, description: "커스텀 차트 및 BMS 주입/로드 기능 활성화");
            RealTimeSwapEntry = Category.CreateEntry("EnableRealTimeSwap", true, description: "FavGirl 실시간 소녀/스킨 교체 모드 활성화 (P/O 핫키)");
            InputOverlayEntry = Category.CreateEntry("EnableInputOverlay", true, description: "인게임 키 입력 오버레이 HUD 표시 활성화");
            JudgmentBarEntry = Category.CreateEntry("EnableJudgmentBar", true, description: "인게임 판정바 UI 그래픽 표시 활성화");
            DiscordRpcEntry = Category.CreateEntry("EnableDiscordRPC", true, description: "Discord Rich Presence 연동 상태 표시 활성화");
            HpTextModEntry = Category.CreateEntry("EnableHpTextMod", true, description: "체력바 텍스트 워터마크 및 HitPoint 표시 연동 활성화");
            ApModEntry = Category.CreateEntry("EnableAPMod", true, description: "올 퍼펙트 및 판정/정확도 계산 오버라이드 활성화");
            AllPerfectSoundEntry = Category.CreateEntry("EnableAllPerfectSound", true, description: "올 퍼펙트 달성 효과음 재생 활성화");
            AutoPlayEntry = Category.CreateEntry("EnableAutoPlay", true, description: "오토 플레이 기능 패치 활성화");
            ForcePerfectEntry = Category.CreateEntry("EnableForcePerfect", true, description: "강제 올 퍼펙트 (All-Perfect Parameter Mod) 기능 활성화");
            BattleMediaEntry = Category.CreateEntry("EnableBattleMedia", true, description: "배틀 커스텀 BGA 비디오/미디어 재생기 활성화");
            SpineSkinEntry = Category.CreateEntry("EnableSpineSkin", true, description: "Spine 커스텀 스킨 텍스처/아틀라스 주입 활성화");
            MobileTouchEntry = Category.CreateEntry("EnableMobileTouch", true, description: "모바일 터치 조작 모드 및 마우스-터치 브릿지 기능 활성화");
            VerboseLogEntry = Category.CreateEntry("EnableVerboseLog", false, description: "진단 로그 상세 출력 활성화 (개발/디버깅용, 평소에는 꺼두세요)");

            RegisterFeatureMapping();

            // 신규 추가된 항목이나 주석 설명을 MelonPreferences.cfg에 즉시 반영 저장
            Save(false);
            MelonLogger.Msg("[ModConfig] 개별 기능 토글 설정 로드 및 동기화 완료.");
        }

        /// <summary>
        /// 현재 설정된 모든 항목과 주석 설명을 UserData/MelonPreferences.cfg 파일에 저장합니다.
        /// </summary>
        public static void Save(bool printLog = true)
        {
            try
            {
                Category?.SaveToFile(false);
                if (printLog)
                {
                    MelonLogger.Msg("[ModConfig] MelonPreferences.cfg 설정 파일이 성공적으로 저장/업데이트되었습니다.");
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[ModConfig] MelonPreferences.cfg 저장 중 에러 발생: {ex.Message}");
            }
        }

        /// <summary>
        /// 디스크의 UserData/MelonPreferences.cfg 파일로부터 최신 설정값을 다시 읽어옵니다.
        /// </summary>
        public static void Reload(bool printLog = true)
        {
            try
            {
                Category?.LoadFromFile(false);
                if (printLog)
                {
                    MelonLogger.Msg("[ModConfig] MelonPreferences.cfg 설정 파일을 다시 로드했습니다.");
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[ModConfig] MelonPreferences.cfg 재로드 중 에러 발생: {ex.Message}");
            }
        }

        /// <summary>
        /// 특정 기능의 토글 값을 프로그래밍 방식으로 변경하고 설정 파일에 즉시 저장합니다.
        /// </summary>
        public static bool TrySetEntry(string entryName, bool value)
        {
            try
            {
                if (Category == null) return false;

                var entry = Category.GetEntry<bool>(entryName);
                if (entry != null)
                {
                    entry.Value = value;
                    Save(false);
                    MelonLogger.Msg($"[ModConfig] '{entryName}' 설정값이 '{value}'(으)로 갱신되었습니다.");
                    return true;
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[ModConfig] '{entryName}' 설정값 변경 실패: {ex.Message}");
            }
            return false;
        }

        private static void RegisterFeatureMapping()
        {
            FeatureMap.Clear();

            // Custom Chart / BMS
            FeatureMap["Init.PreloadManifest"] = () => EnableCustomChart;
            FeatureMap["ExperimentStage"] = () => EnableCustomChart;
            FeatureMap["Scene.ResetHitPoint"] = () => EnableCustomChart;
            FeatureMap["ExperimentHitPoint"] = () => EnableCustomChart;

            // Mobile Touch
            FeatureMap["Battle.MobileTouch"] = () => EnableMobileTouch;
            FeatureMap["UI.MobileTouchSetting"] = () => EnableMobileTouch;

            // Skins & Swap
            FeatureMap["Init.SkinsConfig"] = () => EnableRealTimeSwap;
            FeatureMap["Init.SpineSkinFolder"] = () => EnableSpineSkin;
            FeatureMap["Input.RealTimeSwap"] = () => EnableRealTimeSwap;

            // Overlay & Judgment UI
            FeatureMap["Scene.ResetInputOverlay"] = () => EnableInputOverlay;
            FeatureMap["InputOverlay.Draw"] = () => EnableInputOverlay;
            FeatureMap["JudgmentBar.Draw"] = () => EnableJudgmentBar;

            // Discord RPC
            FeatureMap["Init.DiscordRPC"] = () => EnableDiscordRPC;
            FeatureMap["DiscordRPC.Update"] = () => EnableDiscordRPC;

            // Hwa Media
            FeatureMap["HwaSync.Battle"] = () => EnableBattleMedia;

            // ────────────────────────────────────────────────────────────────────────
            //  의도적으로 등록하지 않는 키
            //
            //  미등록 키는 IsEnabled가 true를 반환하므로, 등록하지 않는 것이 곧 "항상 실행"입니다.
            //  아래 셋은 전부 "A 기능의 토글이 B 기능을 조용히 죽이던" 같은 사고로 한 번씩 당한
            //  자리입니다. FeatureGuard는 게이트에 걸리면 로그 없이 return하므로, 잘못 묶으면
            //  증상만 남고 원인을 추적할 방법이 없습니다. 다시 묶기 전에 반드시 읽어 주세요.
            //
            //  • "Init.ConfigFile" / "ConfigFile.Reload"  (예전: EnableInputOverlay)
            //      config.txt는 키 오버레이 전용이 아니라 오토플레이·강제퍼펙트·피버충전금지·
            //      시네마·고스트노트·판정바까지 담은 모드 공용 설정 파일입니다. 오버레이 하나를
            //      끄면 파일이 읽히지도 생성되지도 않아 나머지 설정이 전부 기본값에 고정됐습니다.
            //
            //  • "StageCheck"  (예전: EnableCustomChart)
            //      HywStageManager.CheckForStageAndModify는 체력바 워터마크만 담당하는 것처럼
            //      보이지만, 배틀 진입/이탈을 추적해 IsInStage / IsInStageStatic을 유지하는
            //      유일한 곳입니다. 그 값을 쓰는 쪽은 커스텀 차트와 무관합니다.
            //        - MainMod.OnGUI : !IsInStage면 통째로 return → 키 오버레이 + 판정바 전체
            //        - ExperimentHitPointInstaller.Update(IsInStage)
            //        - InputOverlay.LoadConfigIfNeeded : 배틀 중 디스크 I/O 회피
            //      워터마크 적용 자체는 안쪽 HywHpText.ShouldApply가 이미 거르므로, 게이트를
            //      빼도 체력바 텍스트가 원치 않게 덮어써지지 않습니다.
            //
            //  • "Init.OfflineSandbox"  (예전: EnableCustomChart)
            //      오프라인 샌드박스는 DLC 허용 / DLCVerify 바이패스 / 콜라보 만료 우회를 다루는
            //      기능이라 커스텀 차트와 관련이 없고, 자체 제어 수단
            //      (save custom key/OFFLINE_SANDBOX.txt)을 이미 가집니다. 게다가 Initialize()가
            //      그 플래그 파일을 만드는 유일한 경로라, 묶여 있으면 파일 생성조차 되지 않아
            //      사용자가 설정할 방법 자체가 사라집니다.
            // ────────────────────────────────────────────────────────────────────────
        }

        /// <summary>
        /// 지정한 기능 식별자 또는 기능 키가 활성화되어 있는지 여부를 반환합니다.
        /// </summary>
        public static bool IsEnabled(string featureName)
        {
            if (string.IsNullOrEmpty(featureName)) return true;

            if (FeatureMap.TryGetValue(featureName, out var checkFunc))
            {
                return checkFunc();
            }

            return true;
        }

        /// <summary>
        /// 기능 식별자에 매핑된 동적 체크 대리자를 조회합니다. (FeatureGuard 1회성 캐싱용)
        /// </summary>
        public static bool TryGetFeatureChecker(string featureName, out Func<bool> checkFunc)
        {
            if (!string.IsNullOrEmpty(featureName) && FeatureMap.TryGetValue(featureName, out checkFunc))
            {
                return true;
            }
            checkFunc = null;
            return false;
        }

        /// <summary>
        /// <see cref="EnableVerboseLog"/>가 켜져 있을 때만 MelonLogger.Msg를 출력하는 헬퍼입니다.
        /// 진단성 로그를 조건부로 출력하여 릴리스 플레이 중 로그 노이즈를 방지합니다.
        /// </summary>
        public static void VerboseLog(string msg)
        {
            if (EnableVerboseLog) MelonLogger.Msg(msg);
        }
    }
}
