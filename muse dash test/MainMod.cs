using MelonLoader;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[assembly: MelonInfo(typeof(muse_dash_test.MainMod), "muse-dash-custom-chart", "0.10.6", "화영왕")]
[assembly: MelonColor(255, 147, 112, 219)] // 모드 이름 색상: 보라색(MediumPurple #9370DB)
[assembly: MelonGame("PeroPeroGames", "MuseDash")]

namespace muse_dash_test
{
    /// <summary>
    /// 모드의 주요 초기화 및 프레임 틱 업데이트(라이프사이클)를 담당하는 MelonMod 구현 클래스입니다.
    /// </summary>
    public class MainMod : MelonMod
    {
        private static readonly HywStageManager hywStageManager = new HywStageManager();
        private static float hywCheckTimer = 0f;
        private const float HywCheckInterval = 0.1f;

        public override void OnInitializeMelon()
        {
            DeviceDetector.Detect();
            ModConfig.Load();

            if (DeviceDetector.IsUmpc)
            {
                ModLogger.LogAlways($"[DeviceDetector] UMPC 기기가 감지되었습니다 ({DeviceDetector.DetectedModel} / {DeviceDetector.GpuName}).");
                ModLogger.LogAlways($"[DeviceDetector] 렉 및 프레임 드랍을 방지하기 위해 로그 레벨을 '{ModLogger.CurrentLogLevel}'(으)로 최적화(억제)했습니다.");
            }

            ModLogger.Msg("모드가 로드되었습니다.");
            try { UnityEngine.Input.multiTouchEnabled = true; } catch (Exception) { }
            ModLogger.Msg("HywHpTextMod - 체력바 텍스트 모드가 성공적으로 연동 활성화되었습니다!");

            // 게임 업데이트로 깨진 패치 대상이 있는지 시작 시 점검하여 요약 로그로 표시합니다.
            PatchHealthCheck.Run();

            // 초기화 단계의 각 기능을 독립적으로 격리합니다. 한 기능의 실패가 뒤따르는
            // 다른 기능의 초기화를 막지 않도록 FeatureGuard로 분리합니다.
            // (1회성 초기화이므로 서킷 브레이커 자동 비활성화는 사용하지 않습니다: maxConsecutiveFailures=0)

            // 오프라인 커스텀 샌드박스 및 디스커버리 덤프 실행
            FeatureGuard.Run("Init.OfflineSandbox", OfflineCustomSandbox.Initialize, maxConsecutiveFailures: 0);

            // 게임이 켜질 때 즉시 설정 폴더/파일을 감지 및 생성/로드합니다.
            // config.txt는 키 오버레이 전용이 아니라 오토플레이·강제퍼펙트·판정바까지 담은 공용
            // 설정 파일이므로, 기능 토글에 묶지 않습니다(ModConfig.RegisterFeatureMapping 주석 참고).
            FeatureGuard.Run("Init.ConfigFile", InputOverlay.LoadConfigIfNeeded, maxConsecutiveFailures: 0);

            // hwa 작업 폴더 생성 및 이전 실행의 진단 덤프 정리
            FeatureGuard.Run("Init.HwaFolder", () =>
            {
                string hwaPath = HwaResourceManager.HwaFolderPath;
                Directory.CreateDirectory(hwaPath);
                ModLogger.Msg($"hwa 폴더를 확인/생성했습니다: {hwaPath}");
                CleanupStaleDumpFiles(hwaPath);
            }, maxConsecutiveFailures: 0);

            // hwa tag image 폴더 생성 및 내장 태그 아이콘 추출
            FeatureGuard.Run("Init.HwaTagImage", () =>
            {
                string hwaTagImageFolderPath = Path.Combine(MelonLoader.Utils.MelonEnvironment.GameRootDirectory, "hwa tag image");
                Directory.CreateDirectory(hwaTagImageFolderPath);
                ModLogger.Msg($"hwa tag image 폴더를 확인/생성했습니다: {hwaTagImageFolderPath}");
                EmbeddedResource.EnsureTagIcon(hwaTagImageFolderPath);
            }, maxConsecutiveFailures: 0);

            // skin test 폴더 + 세트별 하위 폴더 생성 (커스텀 Spine 스킨 주입용 원본 파일 위치)
            // 예: skin test/char_3_black/char_3_black.png/.atlas/.json 세트를 두면 black_girl_battle에 주입됨
            FeatureGuard.Run("Init.SpineSkinFolder", () =>
            {
                CustomSkinInjector.EnsureSetFolders();
                ModLogger.Msg($"skin test 폴더를 확인/생성했습니다: {CustomSkinInjector.SkinTestDirectory}");
                foreach (var baseName in CustomSkinInjector.KnownBaseNames)
                {
                    ModLogger.Msg($"  - 세트 폴더: {CustomSkinInjector.GetSetDirectory(baseName)}");
                }
            }, maxConsecutiveFailures: 0);

            // hwa 매니페스트 사전 로드
            FeatureGuard.Run("Init.PreloadManifest", HwaResourceManager.PreloadHwaManifest, maxConsecutiveFailures: 0);

            // Discord Rich Presence 초기화
            FeatureGuard.Run("Init.DiscordRPC", DiscordPresenceManager.Initialize, maxConsecutiveFailures: 0);
        }

        // 이전 실행에서 생성된 진단 덤프 파일 목록. 매 실행 시작 시 삭제하여 새로 기록되게 합니다.
        private static readonly string[] StaleDumpFileNames =
        {
            "album_tag_dump.txt",
            "album_tag_dump.md",
            "music_info_dump.txt",
            "tag_manager_dump.txt",
        };

        /// <summary>
        /// 이전 실행에서 남은 진단 덤프 파일들을 삭제합니다.
        /// </summary>
        private static void CleanupStaleDumpFiles(string hwaPath)
        {
            foreach (var name in StaleDumpFileNames)
            {
                string path = Path.Combine(hwaPath, name);
                if (File.Exists(path)) File.Delete(path);
            }
        }

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            ModLogger.Msg($"씬이 로드되었습니다: {sceneName} (빌드 인덱스: {buildIndex})");

            // 씬 전환은 게임 상태가 크게 바뀌는 지점이므로, 특정 씬에서만 일시적으로 실패해
            // 자동 비활성화된 기능들에게 1회 재시도 기회를 부여합니다.
            FeatureGuard.RearmAll();

            FeatureGuard.Run("Scene.ResetInputOverlay", InputOverlay.ResetCache);
            FeatureGuard.Run("Scene.ResetHitPoint", ExperimentHitPointInstaller.Reset);
            // 연타 도중 곡을 빠져나가면 구간 플래그가 켜진 채 남아 다음 곡까지 영향을 줍니다.
            FeatureGuard.Run("Scene.ResetSpineContractWindow", SpineActionContract.ResetWindow);
            FeatureGuard.Run("Scene.ResetVictoryCache", Patches.VictoryDataCache.ResetSession);
        }

        public override void OnUpdate()
        {
            // 매 프레임 호출되므로 각 기능을 FeatureGuard로 격리합니다.
            // 람다 클로저 대신 정적 메서드를 전달하여 매 프레임 GC 가비지 생성을 차단합니다.
            // (정적 메서드 그룹의 델리게이트 캐시는 C# 11부터입니다. csproj의 LangVersion을 낮추면 다시 매번 할당됩니다.)
            FeatureGuard.Run("ConfigFile.Reload", InputOverlay.LoadConfigIfNeeded);
            FeatureGuard.Run("HwaSync.Battle", HwaSyncManager.HandleBattleSynchronization);
            FeatureGuard.Run("StageCheck", UpdateStageCheck);
            FeatureGuard.Run("ExperimentStage", HandleExperimentStageUpdate);
            FeatureGuard.Run("ExperimentHitPoint", UpdateExperimentHitPoint);
            FeatureGuard.Run("DiscordRPC.Update", DiscordPresenceManager.Update);
        }

        private static void UpdateStageCheck()
        {
            hywCheckTimer += Time.deltaTime;
            if (hywCheckTimer >= HywCheckInterval)
            {
                hywCheckTimer = 0f;
                hywStageManager.CheckForStageAndModify();
            }
        }

        private static void UpdateExperimentHitPoint()
        {
            ExperimentHitPointInstaller.Update(hywStageManager != null && hywStageManager.IsInStage);
        }

        public override void OnGUI()
        {
            // 인게임 오버레이 및 판정바 그리기(GUI). 각 그리기를 FeatureGuard로 격리하여
            // 예외 발생 시 로그 폭발/프레임 드랍을 방지합니다.
            if (hywStageManager == null || !hywStageManager.IsInStage) return;

            // OnGUI는 프레임당 1회가 아니라 Layout 1회 + Repaint 1회 + 입력 이벤트 1건당 1회씩
            // 호출됩니다. 리듬 게임은 키 입력이 초당 수십 건이라, 게이트가 없으면 오버레이와
            // 판정바 전체가 그 횟수만큼 다시 그려집니다. 실제 픽셀이 찍히는 Repaint에서만 그립니다.
            // (Event.current가 null이면 GUI 문맥을 알 수 없는 경우이므로, 오버레이가 통째로
            //  사라지지 않도록 막지 않고 그립니다.)
            var guiEvent = Event.current;
            if (guiEvent != null && guiEvent.type != EventType.Repaint) return;

            FeatureGuard.Run("InputOverlay.Draw", InputOverlay.DrawInputOverlay);
            FeatureGuard.Run("JudgmentBar.Draw", JudgmentBar.DrawJudgmentBar);
        }

        /// <summary>
        /// 커스텀 차트 적용 중이고 인게임 스테이지에 들어가 있으면, 체력바 텍스트 워터마크
        /// ("made in 화영왕")가 게임에 의해 덮어써졌는지 주기적으로 확인해 다시 적용합니다.
        /// (가상 노트 생성/주입은 여기가 아니라 DBStageInfoExperimentChart에서 차트 주입 시점에 수행됩니다.)
        /// </summary>
        private static void HandleExperimentStageUpdate()
        {
            if (!CustomPlaySession.Current.ShouldApplyExperimentChart)
            {
                return;
            }

            try
            {
                if (hywStageManager.IsInStage)
                {
                    hywStageManager.CheckForNoteEvents();
                }
            }
            catch (Exception ex)
            {
                ModLogger.Error($"[HywHpTextMod] Update 오류: {ex}");
            }
        }

        public override void OnApplicationQuit()
        {
            DiscordPresenceManager.Shutdown();
            ModLogger.Msg("모드가 종료되었습니다.");
        }

    }
}
