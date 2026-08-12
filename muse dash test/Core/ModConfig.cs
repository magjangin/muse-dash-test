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

            RegisterFeatureMapping();
            MelonLogger.Msg("[ModConfig] 개별 기능 토글 설정 로드 완료.");
        }

        private static void RegisterFeatureMapping()
        {
            FeatureMap.Clear();

            // Custom Chart / BMS
            FeatureMap["Init.OfflineSandbox"] = () => EnableCustomChart;
            FeatureMap["Init.PreloadManifest"] = () => EnableCustomChart;
            FeatureMap["StageCheck"] = () => EnableCustomChart;
            FeatureMap["ExperimentStage"] = () => EnableCustomChart;

            // Skins & Swap
            FeatureMap["Init.SkinsConfig"] = () => EnableRealTimeSwap;
            FeatureMap["Init.SpineSkinFolder"] = () => EnableSpineSkin;
            FeatureMap["Input.RealTimeSwap"] = () => EnableRealTimeSwap;

            // Overlay & Judgment UI
            FeatureMap["Init.InputOverlayConfig"] = () => EnableInputOverlay;
            FeatureMap["Scene.ResetInputOverlay"] = () => EnableInputOverlay;
            FeatureMap["InputOverlay.Config"] = () => EnableInputOverlay;
            FeatureMap["InputOverlay.Draw"] = () => EnableInputOverlay;
            FeatureMap["JudgmentBar.Draw"] = () => EnableJudgmentBar;

            // Discord RPC
            FeatureMap["Init.DiscordRPC"] = () => EnableDiscordRPC;
            FeatureMap["DiscordRPC.Update"] = () => EnableDiscordRPC;

            // HP / HitPoint
            FeatureMap["Scene.ResetHitPoint"] = () => EnableHpTextMod;
            FeatureMap["ExperimentHitPoint"] = () => EnableHpTextMod;

            // Hwa Media
            FeatureMap["HwaSync.Battle"] = () => EnableBattleMedia;
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
    }
}
