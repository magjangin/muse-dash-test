using MelonLoader;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[assembly: MelonInfo(typeof(muse_dash_test.MainMod), "muse-dash-custom-chart", "0.5.6", "화영왕")]
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
            MelonLogger.Msg("모드가 로드되었습니다.");
            MelonLogger.Msg("HywHpTextMod - 체력바 텍스트 모드가 성공적으로 연동 활성화되었습니다!");

            // 게임 업데이트로 깨진 패치 대상이 있는지 시작 시 점검하여 요약 로그로 표시합니다.
            PatchHealthCheck.Run();

            // 초기화 단계의 각 기능을 독립적으로 격리합니다. 한 기능의 실패가 뒤따르는
            // 다른 기능의 초기화를 막지 않도록 FeatureGuard로 분리합니다.
            // (1회성 초기화이므로 서킷 브레이커 자동 비활성화는 사용하지 않습니다: maxConsecutiveFailures=0)

            // 오프라인 커스텀 샌드박스 및 디스커버리 덤프 실행
            FeatureGuard.Run("Init.OfflineSandbox", OfflineCustomSandbox.Initialize, maxConsecutiveFailures: 0);

            // 게임이 켜질 때 즉시 설정 폴더/파일을 감지 및 생성/로드합니다.
            FeatureGuard.Run("Init.InputOverlayConfig", InputOverlay.LoadConfigIfNeeded, maxConsecutiveFailures: 0);

            // hwa 작업 폴더 생성 및 이전 실행의 진단 덤프 정리
            FeatureGuard.Run("Init.HwaFolder", () =>
            {
                string hwaPath = HwaResourceManager.HwaFolderPath;
                Directory.CreateDirectory(hwaPath);
                MelonLogger.Msg($"hwa 폴더를 확인/생성했습니다: {hwaPath}");
                CleanupStaleDumpFiles(hwaPath);
            }, maxConsecutiveFailures: 0);

            // hwa tag image 폴더 생성 및 내장 태그 아이콘 추출
            FeatureGuard.Run("Init.HwaTagImage", () =>
            {
                string hwaTagImageFolderPath = Path.Combine(MelonLoader.Utils.MelonEnvironment.GameRootDirectory, "hwa tag image");
                Directory.CreateDirectory(hwaTagImageFolderPath);
                MelonLogger.Msg($"hwa tag image 폴더를 확인/생성했습니다: {hwaTagImageFolderPath}");
                EnsureTagIconExtracted(hwaTagImageFolderPath);
            }, maxConsecutiveFailures: 0);

            // hwa 매니페스트 사전 로드
            FeatureGuard.Run("Init.PreloadManifest", PreloadHwaManifest, maxConsecutiveFailures: 0);

            // FavGirl 즐겨찾기 설정 및 핫키 정보 초기화
            FavSave.Load();
            MelonLogger.Msg("=== FavGirl 실시간 교체 기능 활성화 ===");
            MelonLogger.Msg("P키: 실시간 교체 모드 켜기/끄기");
            MelonLogger.Msg("O키: 실시간 교체 실행 (모드 활성화 후)");
            MelonLogger.Msg("======================================");
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

        /// <summary>
        /// 내장 리소스 tag_icon.png를 대상 폴더에 추출합니다(이미 존재하면 건너뜀).
        /// </summary>
        private static void EnsureTagIconExtracted(string targetFolder)
        {
            string pngPath = Path.Combine(targetFolder, "tag_icon.png");
            if (File.Exists(pngPath)) return;

            const string resourceName = "muse_dash_test.Resources.tag_icon.png";
            try
            {
                var assembly = System.Reflection.Assembly.GetExecutingAssembly();
                using (var stream = assembly.GetManifestResourceStream(resourceName))
                {
                    if (stream == null)
                    {
                        MelonLogger.Error($"[APMod.TagIcon] 추출할 내장 리소스를 찾을 수 없습니다: {resourceName}");
                        return;
                    }

                    byte[] fileData = new byte[stream.Length];
                    stream.Read(fileData, 0, fileData.Length);
                    File.WriteAllBytes(pngPath, fileData);
                    MelonLogger.Msg($"[APMod.TagIcon] 내장 리소스 '{resourceName}'를 '{pngPath}'에 추출 완료!");
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[APMod.TagIcon] 내장 리소스 추출 중 예외 발생: {ex}");
            }
        }

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            MelonLogger.Msg($"씬이 로드되었습니다: {sceneName} (빌드 인덱스: {buildIndex})");

            // 씬 전환은 게임 상태가 크게 바뀌는 지점이므로, 특정 씬에서만 일시적으로 실패해
            // 자동 비활성화된 기능들에게 1회 재시도 기회를 부여합니다.
            FeatureGuard.RearmAll();

            FeatureGuard.Run("Scene.ResetInputOverlay", InputOverlay.ResetCache);
            FeatureGuard.Run("Scene.ResetHitPoint", ExperimentHitPointInstaller.Reset);
        }

        public override void OnUpdate()
        {
            // 매 프레임 호출되므로 각 기능을 FeatureGuard로 격리합니다.
            // 한 기능의 예외가 다른 기능을 막거나 로그를 폭발시키지 않으며,
            // 연속 실패가 누적되면 해당 기능만 자동 비활성화됩니다.

            // FavGirl 실시간 교체 입력 감지
            FeatureGuard.Run("Input.RealTimeSwap", () =>
            {
                RealTimeSwapper.CheckForOKeyPress();
                if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.P))
                {
                    RealTimeSwapManager.ToggleRealTimeMode();
                }
            });

            // 실시간 설정 파일 변경 감지 (오토플레이 등 인게임 진입 전 설정 로드 보장)
            FeatureGuard.Run("InputOverlay.Config", InputOverlay.LoadConfigIfNeeded);

            FeatureGuard.Run("HwaSync.Battle", HwaSyncManager.HandleBattleSynchronization);

            // 1. 순정/실험 맵에 구애받지 않고 스테이지 상태를 지속적으로 모니터링합니다.
            FeatureGuard.Run("StageCheck", () =>
            {
                hywCheckTimer += Time.deltaTime;
                if (hywCheckTimer >= HywCheckInterval)
                {
                    hywCheckTimer = 0f;
                    hywStageManager.CheckForStageAndModify();
                }
            });

            // 2. 실험 모드 관련 업데이트 처리
            FeatureGuard.Run("ExperimentStage", HandleExperimentStageUpdate);
            FeatureGuard.Run("ExperimentHitPoint", () =>
                ExperimentHitPointInstaller.Update(hywStageManager != null && hywStageManager.IsInStage));

            // 3. 디버그용 공격 키 입력 감지 테스트는 릴리즈 버전이므로 주석 처리합니다.
            /*
            if (hywStageManager != null && hywStageManager.IsInStage)
            {
                InputOverlay.UpdateKeyTest();
            }
            */
        }

        public override void OnGUI()
        {
            // 인게임 오버레이 및 판정바 그리기(GUI). OnGUI는 프레임당 여러 번 호출되므로
            // 각 그리기를 FeatureGuard로 격리하여 예외 발생 시 로그 폭발/프레임 드랍을 방지합니다.
            if (hywStageManager != null && hywStageManager.IsInStage)
            {
                FeatureGuard.Run("InputOverlay.Draw", InputOverlay.DrawInputOverlay);
                FeatureGuard.Run("JudgmentBar.Draw", JudgmentBar.DrawJudgmentBar);
            }
        }

        /// <summary>
        /// 실험 모드가 활성화되어 있다면, 주기적으로 인게임 스테이지 진입 여부 및 노트 이벤트를 모니터링하여 가상 노트를 생성/조작합니다.
        /// </summary>
        private void HandleExperimentStageUpdate()
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
                MelonLogger.Error($"[HywHpTextMod] Update 오류: {ex}");
            }
        }

        public override void OnApplicationQuit()
        {
            MelonLogger.Msg("모드가 종료되었습니다.");
        }

        // ==========================================
        // HwaResourceManager 기능 포워딩 프록시 메서드
        // ==========================================

        public static void PreloadHwaManifest()
        {
            HwaResourceManager.PreloadHwaManifest();
        }

        public static bool TryGetCachedHwaManifest(string uid, out string description)
        {
            return HwaResourceManager.TryGetCachedHwaManifest(uid, out description);
        }

        public static bool TryGetCachedHwaSearchTerms(string uid, out string sourceUid, out string sourceTitle, out string sourceArtist, out string sourceAlbum, out string description)
        {
            return HwaResourceManager.TryGetCachedHwaSearchTerms(uid, out sourceUid, out sourceTitle, out sourceArtist, out sourceAlbum, out description);
        }

        public static bool TryGetCachedHwaPrimaryVirtualSong(string uid, out string title, out string artist, out string levelDesigner, out int diff1, out int diff2, out int diff3, out int diff4, out int diff5, out string description)
        {
            return HwaResourceManager.TryGetCachedHwaPrimaryVirtualSong(uid, out title, out artist, out levelDesigner, out diff1, out diff2, out diff3, out diff4, out diff5, out description);
        }

        public static bool TryGetCachedHwaScene(string uid, out int scene)
        {
            return HwaResourceManager.TryGetCachedHwaScene(uid, out scene);
        }

        public static bool TryGetCachedHwaBmsChart(string uid, out BmsChart chart, out string description)
        {
            return HwaResourceManager.TryGetCachedHwaBmsChart(uid, out chart, out description);
        }

        public static bool TryGetSongDirectory(string uid, out string songDir)
        {
            return HwaResourceManager.TryGetSongDirectory(uid, out songDir);
        }

        public static List<string> GetVirtualUids()
        {
            return HwaResourceManager.GetVirtualUids();
        }
    }
}
