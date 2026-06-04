using MelonLoader;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[assembly: MelonInfo(typeof(muse_dash_test.MainMod), "muse-dash-test", "0.1.0", "화영왕")]
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

            try
            {
                AlbumTagToggle_Init_Patch.PatchResourcesManager(this.HarmonyInstance);
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[APMod] ResourcesManager 패치 등록 실패: {ex}");
            }
        }

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            MelonLogger.Msg($"씬이 로드되었습니다: {sceneName} (빌드 인덱스: {buildIndex})");
        }

        public override void OnUpdate()
        {
            HwaSyncManager.HandleBattleSynchronization();
            HandleExperimentStageUpdate();
        }

        /// <summary>
        /// 실험 모드가 활성화되어 있다면, 주기적으로 인게임 스테이지 진입 여부 및 노트 이벤트를 모니터링하여 가상 노트를 생성/조작합니다.
        /// </summary>
        private void HandleExperimentStageUpdate()
        {
            if (!ExperimentPlayContext.ShouldApplyExperimentChart)
            {
                return;
            }

            try
            {
                hywCheckTimer += Time.deltaTime;
                if (hywCheckTimer >= HywCheckInterval)
                {
                    hywCheckTimer = 0f;
                    hywStageManager.CheckForStageAndModify();
                }

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
