using System;
using UnityEngine;

namespace muse_dash_test
{
    /// <summary>
    /// UMPC(핸드헬드 PC: ROG Ally, Steam Deck, Legion Go 등) 및 하드웨어 환경을 감지하는 유틸리티 클래스입니다.
    /// </summary>
    public static class DeviceDetector
    {
        private static bool _initialized = false;
        private static bool _isUmpc = false;
        private static string _detectedModel = "Unknown PC";
        private static string _gpuName = "Unknown GPU";
        private static bool _hasBattery = false;

        /// <summary>
        /// 현재 실행 중인 기기가 UMPC(핸드헬드 PC)인지 여부입니다.
        /// </summary>
        public static bool IsUmpc
        {
            get
            {
                if (!_initialized) Detect();
                return _isUmpc;
            }
        }

        /// <summary>
        /// 감지된 기기 모델명 또는 분류명입니다.
        /// </summary>
        public static string DetectedModel
        {
            get
            {
                if (!_initialized) Detect();
                return _detectedModel;
            }
        }

        /// <summary>
        /// 감지된 그래픽 장치명입니다.
        /// </summary>
        public static string GpuName
        {
            get
            {
                if (!_initialized) Detect();
                return _gpuName;
            }
        }

        /// <summary>
        /// 배터리 탑재 여부입니다.
        /// </summary>
        public static bool HasBattery
        {
            get
            {
                if (!_initialized) Detect();
                return _hasBattery;
            }
        }

        /// <summary>
        /// 하드웨어 정보를 스캔하여 UMPC 여부를 판별합니다.
        /// </summary>
        public static void Detect()
        {
            if (_initialized) return;
            _initialized = true;

            try
            {
                string model = SystemInfo.deviceModel ?? string.Empty;
                string modelLower = model.ToLowerInvariant();

                string gpu = SystemInfo.graphicsDeviceName ?? string.Empty;
                string gpuLower = gpu.ToLowerInvariant();

                _gpuName = string.IsNullOrEmpty(gpu) ? "Unknown GPU" : gpu;
                _hasBattery = SystemInfo.batteryStatus != BatteryStatus.Unknown || SystemInfo.batteryLevel > 0f;

                // 1. 대표적인 UMPC 기기 모델명 식별
                if (modelLower.Contains("steamdeck") || modelLower.Contains("jupiter") || modelLower.Contains("galileo"))
                {
                    _isUmpc = true;
                    _detectedModel = "Valve Steam Deck";
                    return;
                }

                if (modelLower.Contains("rog ally") || modelLower.Contains("rc71l") || modelLower.Contains("rc72l") || modelLower.Contains("ally"))
                {
                    _isUmpc = true;
                    _detectedModel = "ASUS ROG Ally";
                    return;
                }

                if (modelLower.Contains("legion go") || modelLower.Contains("83e1"))
                {
                    _isUmpc = true;
                    _detectedModel = "Lenovo Legion Go";
                    return;
                }

                if (modelLower.Contains("ayaneo") || modelLower.Contains("aya neo"))
                {
                    _isUmpc = true;
                    _detectedModel = "AYANEO Handheld";
                    return;
                }

                if (modelLower.Contains("gpd") || modelLower.Contains("win mini") || modelLower.Contains("win 4") || modelLower.Contains("win max"))
                {
                    _isUmpc = true;
                    _detectedModel = "GPD Handheld";
                    return;
                }

                if (modelLower.Contains("onexplayer") || modelLower.Contains("1xplayer") || modelLower.Contains("onexfly") || modelLower.Contains("aokzoe"))
                {
                    _isUmpc = true;
                    _detectedModel = "ONEXPLAYER / AOKZOE";
                    return;
                }

                if (modelLower.Contains("claw") || modelLower.Contains("a1m"))
                {
                    _isUmpc = true;
                    _detectedModel = "MSI Claw";
                    return;
                }

                // 2. GPU 기반 식별 (Steam Deck APU 및 핸드헬드 전용 APU)
                if (gpuLower.Contains("custom gpu 0405") || gpuLower.Contains("vangogh") || gpuLower.Contains("sephiroth"))
                {
                    _isUmpc = true;
                    _detectedModel = "Steam Deck (Custom APU)";
                    return;
                }

                // 3. APU 및 배터리 조합 휴리스틱 감지
                bool isHandheldApu = gpuLower.Contains("z1 extreme") ||
                                     gpuLower.Contains("ryzen z1") ||
                                     gpuLower.Contains("radeon 780m") ||
                                     gpuLower.Contains("radeon 880m") ||
                                     gpuLower.Contains("radeon 890m") ||
                                     gpuLower.Contains("radeon 680m") ||
                                     gpuLower.Contains("radeon 660m");

                if (isHandheldApu && _hasBattery)
                {
                    _isUmpc = true;
                    _detectedModel = $"Handheld UMPC ({(!string.IsNullOrEmpty(model) ? model : gpu)})";
                    return;
                }

                // 일반 PC 또는 랩톱
                _isUmpc = false;
                _detectedModel = !string.IsNullOrEmpty(model) ? model : "Standard PC";
            }
            catch (Exception)
            {
                _isUmpc = false;
                _detectedModel = "Standard PC";
            }
        }
    }
}
