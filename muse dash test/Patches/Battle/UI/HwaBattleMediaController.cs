using MelonLoader;
using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Video;

namespace muse_dash_test
{
    /// <summary>
    /// 배틀 진입 시 커스텀 BGM(.ogg) 및 커스텀 BGA(.mp4)를 디스크에서 로딩하고 재생/라이프사이클을 제어하는 컨트롤러 클래스입니다.
    /// 미디어 로딩/주입 로직은 이 파일에, 일시정지/재개/정지 등 재생 라이프사이클 제어는 HwaBattleMediaController.Lifecycle.cs에 분리되어 있습니다.
    /// </summary>
    public static partial class HwaBattleMediaController
    {
        private static bool battleMediaInjectionStarted;
        private static AudioSource injectedAudioSource;
        // 우리가 직접 주입한 커스텀 배틀 BGM 클립만 추적합니다(해제 대상 한정).
        private static AudioClip injectedClip;

        public static void ResetState()
        {
            battleMediaInjectionStarted = false;
            injectedAudioSource = null;
            injectedClip = null;
        }

        public static void StartBattleMediaInjection()
        {
            try
            {
                string debugUid = CustomPlaySession.Current.SelectedMusicUid;
                if (string.IsNullOrEmpty(debugUid))
                {
                    debugUid = PnlStagePatchHelper.GetCurrentSelectedMusicUid() ?? CustomPlaySession.Current.LastClickedMusicUid ?? "(unknown)";
                }
                MelonLogger.Msg($"[HwaBattleMediaController.Debug] StartBattleMediaInjection 호출: uid={debugUid}, {CustomPlaySession.Current.DescribeApplyDecision()}, battleMediaInjectionStarted={battleMediaInjectionStarted}");

                if (battleMediaInjectionStarted)
                {
                    MelonLogger.Msg("[HwaBattleMediaController.Debug] 스킵: 이미 이번 배틀에서 주입을 시작함 (battleMediaInjectionStarted=true)");
                    return;
                }

                if (!CustomPlaySession.Current.ShouldApplyExperimentChart)
                {
                    MelonLogger.Msg($"[HwaBattleMediaController.Debug] 스킵: 커스텀 배틀 미디어 비활성 ({CustomPlaySession.Current.DescribeApplyDecision()})");
                    return;
                }

                HwaMenuBgmController.StopMenuMonitoring("battle media injection started");
                battleMediaInjectionStarted = true;
                InjectBattleMedia();
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[HwaBattleMediaController] 주입 시작 실패: {ex}");
            }
        }

        private static void InjectBattleMedia()
        {
            string uid = CustomPlaySession.Current.SelectedMusicUid;
            if (string.IsNullOrEmpty(uid))
            {
                uid = PnlStagePatchHelper.GetCurrentSelectedMusicUid() ?? CustomPlaySession.Current.LastClickedMusicUid ?? "(unknown)";
            }

            string songDir = HwaResourceManager.HwaFolderPath;
            if (HwaResourceManager.TryGetSongDirectory(uid, out string customDir) && !string.IsNullOrEmpty(customDir))
            {
                songDir = customDir;
            }

            Camera mainCam = Camera.main;
            if (mainCam != null)
            {
                MelonLogger.Msg($"[HwaBattleMediaController] 메인 카메라 감지 완료: name={mainCam.name}, position={mainCam.transform.position}");
                if (InputOverlay.enableCinema)
                {
                    TryPlayVideo(mainCam, songDir);
                }
                else
                {
                    MelonLogger.Msg("[HwaBattleMediaController] 시네마(BGA)가 설정에서 비활성화되어 있어 비디오 재생을 건너뜁니다.");
                }
            }
            else
            {
                MelonLogger.Warning("[HwaBattleMediaController] 메인 카메라를 찾지 못했습니다.");
            }

            string oggPath = ResolveHwaOggPath(songDir);
            if (string.IsNullOrWhiteSpace(oggPath))
            {
                MelonLogger.Msg($"[HwaBattleMediaController] ogg 파일을 찾지 못했습니다: folder={songDir}");
                return;
            }

            FileInfo oggInfo = new FileInfo(oggPath);
            MelonLogger.Msg($"[HwaBattleMediaController] ogg 주입 대상 발견: path={oggPath}, fileName={oggInfo.Name}, size={oggInfo.Length}, lastWrite={oggInfo.LastWriteTime:yyyy-MM-dd HH:mm:ss}");

            AudioSource targetSource = FindBattleAudioSource();
            if (targetSource == null)
            {
                targetSource = CreateBattleAudioSource();
            }

            if (targetSource == null)
            {
                MelonLogger.Msg("[HwaBattleMediaController] 배틀 AudioSource를 찾거나 생성하지 못했습니다.");
                return;
            }

            MelonLogger.Msg($"[HwaBattleMediaController] 대상 AudioSource 선택: {DescribeAudioSource(targetSource)}");
            MelonCoroutines.Start(LoadAndApplyClip(targetSource, oggPath));
        }

        private static IEnumerator LoadAndApplyClip(AudioSource targetSource, string oggPath)
        {
            string uri = null;
            bool uriReady = false;
            try
            {
                uri = new Uri(oggPath).AbsoluteUri;
                uriReady = true;
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[HwaBattleMediaController] ogg 경로 변환 실패: {ex}");
            }

            if (!uriReady)
            {
                yield break;
            }

            UnityWebRequest request = new UnityWebRequest(uri, "GET");
            try
            {
                DownloadHandlerAudioClip handler = new DownloadHandlerAudioClip(uri, AudioType.OGGVORBIS);
                handler.streamAudio = true;
                request.downloadHandler = handler;
                yield return request.SendWebRequest();

                if (!string.IsNullOrWhiteSpace(request.error))
                {
                    MelonLogger.Error($"[HwaBattleMediaController] ogg 로드 실패: path={oggPath}, error={request.error}");
                    yield break;
                }

                AudioClip clip = null;
                bool clipReady = false;
                try
                {
                    clip = DownloadHandlerAudioClip.GetContent(request);
                    clipReady = true;
                }
                catch (Exception ex)
                {
                    MelonLogger.Error($"[HwaBattleMediaController] 오디오 클립 변환 실패: {oggPath}, error={ex}");
                }

                if (!clipReady || clip == null)
                {
                    MelonLogger.Error($"[HwaBattleMediaController] 오디오 클립이 비어 있습니다: {oggPath}");
                    yield break;
                }

                clip.name = Path.GetFileName(oggPath);
                MelonLogger.Msg($"[HwaBattleMediaController] 로드한 클립 정보: {DescribeAudioClip(clip)}");

                string beforeState = DescribeAudioSource(targetSource);

                AudioClip previousInjected = injectedClip;

                targetSource.clip = clip;
                targetSource.loop = true;
                targetSource.playOnAwake = false;
                targetSource.Stop();
                targetSource.Play();
                injectedAudioSource = targetSource;
                injectedClip = clip;

                MelonLogger.Msg($"[HwaBattleMediaController] 배틀 BGM 주입 완료: before={beforeState}, after={DescribeAudioSource(targetSource)}, loadedClip={DescribeAudioClip(clip)}");
            }
            finally
            {
                request.Dispose();
            }
        }

        private static string ResolveHwaOggPath(string folderPath)
        {
            try
            {
                if (!Directory.Exists(folderPath))
                {
                    return null;
                }

                string[] txtFiles = Directory.GetFiles(folderPath, "*.txt", SearchOption.AllDirectories);
                string[] oggFiles = Directory.GetFiles(folderPath, "*.ogg", SearchOption.AllDirectories);

                if (oggFiles == null || oggFiles.Length == 0)
                {
                    return null;
                }

                Array.Sort(oggFiles, StringComparer.OrdinalIgnoreCase);

                if (txtFiles != null && txtFiles.Length > 0)
                {
                    Array.Sort(txtFiles, StringComparer.OrdinalIgnoreCase);
                    foreach (string txtFile in txtFiles)
                    {
                        string stem = Path.GetFileNameWithoutExtension(txtFile);
                        foreach (string oggFile in oggFiles)
                        {
                            if (string.Equals(Path.GetFileNameWithoutExtension(oggFile), stem, StringComparison.OrdinalIgnoreCase))
                            {
                                return oggFile;
                            }
                        }
                    }
                }

                foreach (string oggFile in oggFiles)
                {
                    string lower = Path.GetFileNameWithoutExtension(oggFile).ToLowerInvariant();
                    if (lower.Contains("bgm") || lower.Contains("battle") || lower.Contains("music") || lower.Contains("song"))
                    {
                        return oggFile;
                    }
                }

                return oggFiles[0];
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[HwaBattleMediaController] ogg 탐색 실패: {ex}");
                return null;
            }
        }

        private static AudioSource FindBattleAudioSource()
        {
            try
            {
                AudioSource[] sources = UnityEngine.Object.FindObjectsOfType<AudioSource>();
                if (sources == null || sources.Length == 0)
                {
                    return null;
                }

                AudioSource fallback = null;
                foreach (AudioSource source in sources)
                {
                    if (source == null || source.gameObject == null)
                    {
                        continue;
                    }

                    string objectName = source.gameObject.name ?? string.Empty;
                    string clipName = source.clip != null ? source.clip.name : string.Empty;
                    if (LooksLikeBattleAudio(objectName) || LooksLikeBattleAudio(clipName))
                    {
                        return source;
                    }

                    if (fallback == null && source.gameObject.activeInHierarchy)
                    {
                        fallback = source;
                    }
                }

                return fallback;
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[HwaBattleMediaController] AudioSource 탐색 실패: {ex}");
                return null;
            }
        }

        private static AudioSource CreateBattleAudioSource()
        {
            try
            {
                GameObject gameObject = new GameObject("HwaBattleBgmSource");
                AudioSource source = gameObject.AddComponent<AudioSource>();
                source.playOnAwake = false;
                source.loop = true;
                return source;
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[HwaBattleMediaController] AudioSource 생성 실패: {ex}");
                return null;
            }
        }

        private static void TryPlayVideo(Camera mainCam, string folderPath)
        {
            try
            {
                if (!Directory.Exists(folderPath))
                {
                    return;
                }

                string[] mp4Files = Directory.GetFiles(folderPath, "*.mp4", SearchOption.AllDirectories);
                if (mp4Files == null || mp4Files.Length == 0)
                {
                    MelonLogger.Msg($"[HwaBattleMediaController.Video] {folderPath} 폴더 및 하위 폴더에 mp4 파일이 없습니다.");
                    return;
                }

                Array.Sort(mp4Files, StringComparer.OrdinalIgnoreCase);
                string mp4Path = mp4Files[0];
                FileInfo mp4Info = new FileInfo(mp4Path);
                MelonLogger.Msg($"[HwaBattleMediaController.Video] mp4 비디오 주입 대상 발견: path={mp4Path}, fileName={mp4Info.Name}, size={mp4Info.Length}");

                Transform existingQuad = mainCam.transform.Find("VideoBackgroundQuad");
                if (existingQuad != null && existingQuad.gameObject != null)
                {
                    UnityEngine.Object.Destroy(existingQuad.gameObject);
                    MelonLogger.Msg("[HwaBattleMediaController.Video] 기존 잔존 VideoBackgroundQuad 오브젝트를 삭제했습니다.");
                }

                GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
                quad.name = "VideoBackgroundQuad";

                var col = quad.GetComponent("MeshCollider");
                if (col != null) UnityEngine.Object.Destroy(col);

                quad.transform.parent = mainCam.transform;
                quad.transform.localPosition = new Vector3(0f, 0f, 25f);

                float height = mainCam.orthographicSize * 2f;
                float width = height * mainCam.aspect;
                quad.transform.localScale = new Vector3(width, height, 1f);

                MeshRenderer renderer = quad.GetComponent<MeshRenderer>();
                Shader shader = Shader.Find("Unlit/Texture")
                             ?? Shader.Find("Sprites/Default")
                             ?? Shader.Find("UI/Default");

                if (shader != null)
                {
                    renderer.material = new Material(shader);
                    MelonLogger.Msg($"[HwaBattleMediaController.Video] '{shader.name}' 쉐이더를 비디오 판넬에 주입했습니다.");
                }
                else
                {
                    MelonLogger.Warning("[HwaBattleMediaController.Video] 폴백 쉐이더를 찾지 못하여 기본 생성된 머티리얼을 재사용합니다.");
                }

                renderer.sortingLayerName = "Background";
                renderer.sortingOrder = -17;

                VideoPlayer videoPlayer = quad.AddComponent<VideoPlayer>();
                videoPlayer.renderMode = VideoRenderMode.MaterialOverride;
                videoPlayer.targetMaterialRenderer = renderer;
                videoPlayer.targetMaterialProperty = "_MainTex";
                videoPlayer.audioOutputMode = VideoAudioOutputMode.Direct;
                videoPlayer.url = mp4Path;
                videoPlayer.isLooping = true;
                videoPlayer.Play();
                MelonLogger.Msg("[HwaBattleMediaController.Video] 쿼드 머티리얼 기반 비디오 재생을 성공적으로 시작했습니다.");

                MelonCoroutines.Start(MonitorVideoPlayback(videoPlayer));
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[HwaBattleMediaController.Video] 비디오 재생 중 오류 발생: {ex}");
            }
        }

        private static IEnumerator MonitorVideoPlayback(VideoPlayer vp)
        {
            if (vp == null) yield break;
            yield return new WaitForSeconds(1.0f);
            if (vp == null) yield break;

            MelonLogger.Msg($"[HwaBattleMediaController.Video.Debug] 1초 후 비디오 재생 상태 상세 점검:");
            MelonLogger.Msg($"  - isPlaying: {vp.isPlaying}");
            MelonLogger.Msg($"  - isPrepared: {vp.isPrepared}");
            MelonLogger.Msg($"  - url: {vp.url}");
            MelonLogger.Msg($"  - resolution: {vp.width}x{vp.height}");
            MelonLogger.Msg($"  - frameCount: {vp.frameCount}");
            MelonLogger.Msg($"  - frameRate: {vp.frameRate:0.00}");
            MelonLogger.Msg($"  - playbackTime: {vp.time:0.00}s / duration: {vp.length:0.00}s");
        }

        private static bool LooksLikeBattleAudio(string text)
        {
            string value = (text ?? string.Empty).ToLowerInvariant();
            return value.Contains("battle") || value.Contains("bgm") || value.Contains("music") || value.Contains("stage") || value.Contains("song");
        }

        private static string DescribeAudioSource(AudioSource source)
        {
            if (source == null) return "(null)";
            string goName = source.gameObject != null ? source.gameObject.name : "(no go)";
            string clipName = source.clip != null ? source.clip.name : "(null)";
            string clipDetails = source.clip != null ? DescribeAudioClip(source.clip) : "(null)";
            return $"AudioSource(go={goName}, clip={clipName}, playing={source.isPlaying}, mute={source.mute}, volume={source.volume}, loop={source.loop}, enabled={source.enabled}, clipInfo={clipDetails})";
        }

        private static string DescribeAudioClip(AudioClip clip)
        {
            if (clip == null) return "(null)";
            return $"AudioClip(name={clip.name}, length={clip.length:0.000}, samples={clip.samples}, frequency={clip.frequency}, channels={clip.channels}, loadType={clip.loadType}, loadState={clip.loadState}, preload={clip.preloadAudioData})";
        }
    }
}
