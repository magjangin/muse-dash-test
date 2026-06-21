using MelonLoader;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[assembly: MelonInfo(typeof(muse_dash_test.MainMod), "muse-dash-custom-chart", "0.4.2", "화영왕")]
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

            try
            {
                // 오프라인 커스텀 샌드박스 및 디스커버리 덤프 실행
                OfflineCustomSandbox.Initialize();
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[OfflineSandbox] 초기화 중 예외 발생: {ex}");
            }

            try
            {
                // 게임이 켜질 때 즉시 설정 폴더/파일을 감지 및 생성/로드합니다.
                InputOverlay.LoadConfigIfNeeded();

                // OBS 자동 녹화 연동 설정 로드 (설정 파일 없으면 기본값 생성)
                ObsController.LoadConfig();

                string hwaPath = HwaResourceManager.HwaFolderPath;
                Directory.CreateDirectory(hwaPath);
                MelonLogger.Msg($"hwa 폴더를 확인/생성했습니다: {hwaPath}");
                
                string albumDumpPath = Path.Combine(hwaPath, "album_tag_dump.txt");
                if (File.Exists(albumDumpPath)) File.Delete(albumDumpPath);
                
                string albumDumpMdPath = Path.Combine(hwaPath, "album_tag_dump.md");
                if (File.Exists(albumDumpMdPath)) File.Delete(albumDumpMdPath);
                
                string musicDumpPath = Path.Combine(hwaPath, "music_info_dump.txt");
                if (File.Exists(musicDumpPath)) File.Delete(musicDumpPath);

                // hwa tag image 폴더 생성
                string hwaTagImageFolderPath = Path.Combine(MelonLoader.Utils.MelonEnvironment.GameRootDirectory, "hwa tag image");
                Directory.CreateDirectory(hwaTagImageFolderPath);
                MelonLogger.Msg($"hwa tag image 폴더를 확인/생성했습니다: {hwaTagImageFolderPath}");

                // tag_icon.png 추출
                string pngPath = Path.Combine(hwaTagImageFolderPath, "tag_icon.png");
                if (!File.Exists(pngPath))
                {
                    try
                    {
                        var assembly = System.Reflection.Assembly.GetExecutingAssembly();
                        string resourceName = "muse_dash_test.Resources.tag_icon.png";
                        using (var stream = assembly.GetManifestResourceStream(resourceName))
                        {
                            if (stream != null)
                            {
                                byte[] fileData = new byte[stream.Length];
                                stream.Read(fileData, 0, fileData.Length);
                                File.WriteAllBytes(pngPath, fileData);
                                MelonLogger.Msg($"[APMod.TagIcon] OnInitializeMelon: 내장 리소스 '{resourceName}'를 '{pngPath}'에 복사 및 추출 완료!");
                            }
                            else
                            {
                                MelonLogger.Error($"[APMod.TagIcon] OnInitializeMelon: 추출할 내장 리소스를 찾을 수 없습니다: {resourceName}");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MelonLogger.Error($"[APMod.TagIcon] OnInitializeMelon: 내장 리소스 추출 중 예외 발생: {ex}");
                    }
                }

                PreloadHwaManifest();
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"hwa 폴더 생성 및 초기화 중 예외: {ex}");
            }

        }

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            MelonLogger.Msg($"씬이 로드되었습니다: {sceneName} (빌드 인덱스: {buildIndex})");
            InputOverlay.ResetCache();
            ExperimentHitPointInstaller.Reset();
        }

        public override void OnUpdate()
        {
            // 실시간 설정 파일 변경 감지 (오토플레이 등 인게임 진입 전 설정 로드 보장)
            InputOverlay.LoadConfigIfNeeded();

            HwaSyncManager.HandleBattleSynchronization();
            
            // 1. 순정/실험 맵에 구애받지 않고 스테이지 상태를 지속적으로 모니터링합니다.
            try
            {
                hywCheckTimer += Time.deltaTime;
                if (hywCheckTimer >= HywCheckInterval)
                {
                    hywCheckTimer = 0f;
                    hywStageManager.CheckForStageAndModify();
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[MainMod.StageCheck] 스테이지 상태 감지 오류: {ex}");
            }

            // 2. 실험 모드 관련 업데이트 처리
            HandleExperimentStageUpdate();
            ExperimentHitPointInstaller.Update(hywStageManager != null && hywStageManager.IsInStage);

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
            // 인게임 오버레이 및 판정바 그리기(GUI)
            if (hywStageManager != null && hywStageManager.IsInStage)
            {
                InputOverlay.DrawInputOverlay();
                JudgmentBar.DrawJudgmentBar();
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
