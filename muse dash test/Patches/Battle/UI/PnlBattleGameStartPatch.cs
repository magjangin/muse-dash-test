using MelonLoader;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Il2CppAssets.Scripts.UI.Panels;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Video;

// PnlBattle.GameStart 후킹: hwa 폴더의 ogg를 찾아 배틀 BGM으로 주입
[HarmonyLib.HarmonyPatch]
public class PnlBattle_GameStart_Patch
{
    private static readonly string HwaFolderPath = Path.Combine(MelonLoader.Utils.MelonEnvironment.GameRootDirectory, "hwa");

    private static MethodBase TargetMethod()
    {
        Type battleType = FindBattleType();
        if (battleType == null)
        {
            MelonLogger.Warning("[PnlBattle.GameStart.Bgm] PnlBattle 타입을 찾지 못했습니다.");
            return null;
        }

        return battleType.GetMethod("GameStart", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
    }

    public static void Postfix(object __instance)
    {
        try
        {
            if (!ExperimentPlayContext.ShouldApplyExperimentChart)
            {
                return;
            }

            MelonCoroutines.Start(InjectBattleBgmAfterDelay(__instance));
        }
        catch (Exception ex)
        {
            MelonLogger.Error($"[PnlBattle.GameStart.Bgm] 코루틴 시작 실패: {ex}");
        }
    }

    private static IEnumerator InjectBattleBgmAfterDelay(object __instance)
    {
        yield return null;

        Camera mainCam = Camera.main;
        if (mainCam != null)
        {
            MelonLogger.Msg($"[PnlBattle.GameStart.Bgm] 메인 카메라 감지 완료: name={mainCam.name}, position={mainCam.transform.position}");
            TryPlayVideo(mainCam);
        }
        else
        {
            MelonLogger.Warning("[PnlBattle.GameStart.Bgm] 메인 카메라를 찾지 못했습니다.");
        }

        string oggPath = ResolveHwaOggPath();
        if (string.IsNullOrWhiteSpace(oggPath))
        {
            MelonLogger.Msg($"[PnlBattle.GameStart.Bgm] ogg 파일을 찾지 못했습니다: folder={HwaFolderPath}");
            yield break;
        }

        FileInfo oggInfo = new FileInfo(oggPath);
        MelonLogger.Msg($"[PnlBattle.GameStart.Bgm] ogg 주입 대상 발견: path={oggPath}, fileName={oggInfo.Name}, size={oggInfo.Length}, lastWrite={oggInfo.LastWriteTime:yyyy-MM-dd HH:mm:ss}");

        AudioSource targetSource = FindBattleAudioSource();
        if (targetSource == null)
        {
            targetSource = CreateBattleAudioSource();
        }

        if (targetSource == null)
        {
            MelonLogger.Msg("[PnlBattle.GameStart.Bgm] 배틀 AudioSource를 찾거나 생성하지 못했습니다.");
            yield break;
        }

        MelonLogger.Msg($"[PnlBattle.GameStart.Bgm] 대상 AudioSource 선택: {DescribeAudioSource(targetSource)}");

        yield return LoadAndApplyClip(targetSource, oggPath);
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
            MelonLogger.Error($"[PnlBattle.GameStart.Bgm] ogg 경로 변환 실패: {ex}");
        }

        if (!uriReady)
        {
            yield break;
        }

        UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(uri, AudioType.OGGVORBIS);
        yield return request.SendWebRequest();

        if (!string.IsNullOrWhiteSpace(request.error))
        {
            MelonLogger.Error($"[PnlBattle.GameStart.Bgm] ogg 로드 실패: path={oggPath}, error={request.error}");
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
            MelonLogger.Error($"[PnlBattle.GameStart.Bgm] 오디오 클립 변환 실패: {oggPath}, error={ex}");
        }

        if (!clipReady || clip == null)
        {
            MelonLogger.Error($"[PnlBattle.GameStart.Bgm] 오디오 클립이 비어 있습니다: {oggPath}");
            yield break;
        }

        MelonLogger.Msg($"[PnlBattle.GameStart.Bgm] 로드한 클립 정보: {DescribeAudioClip(clip)}");

        string beforeState = DescribeAudioSource(targetSource);

        targetSource.clip = clip;
        targetSource.loop = true;
        targetSource.playOnAwake = false;
        targetSource.Stop();
        targetSource.Play();

        MelonLogger.Msg($"[PnlBattle.GameStart.Bgm] 배틀 BGM 주입 완료: before={beforeState}, after={DescribeAudioSource(targetSource)}, loadedClip={DescribeAudioClip(clip)}");
    }

    private static string ResolveHwaOggPath()
    {
        try
        {
            if (!Directory.Exists(HwaFolderPath))
            {
                return null;
            }

            string[] txtFiles = Directory.GetFiles(HwaFolderPath, "*.txt", SearchOption.TopDirectoryOnly);
            string[] oggFiles = Directory.GetFiles(HwaFolderPath, "*.ogg", SearchOption.TopDirectoryOnly);

            if (oggFiles == null || oggFiles.Length == 0)
            {
                oggFiles = Directory.GetFiles(HwaFolderPath, "*.ogg", SearchOption.AllDirectories);
            }

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
            MelonLogger.Error($"[PnlBattle.GameStart.Bgm] ogg 탐색 실패: {ex}");
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
            MelonLogger.Error($"[PnlBattle.GameStart.Bgm] AudioSource 탐색 실패: {ex}");
            return null;
        }
    }

    private static Type FindBattleType()
    {
        try
        {
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                Type[] types = null;
                try
                {
                    types = assembly.GetTypes();
                }
                catch (ReflectionTypeLoadException ex)
                {
                    types = ex.Types;
                }
                catch
                {
                    continue;
                }

                if (types == null)
                {
                    continue;
                }

                foreach (Type type in types)
                {
                    if (type == null)
                    {
                        continue;
                    }

                    if (string.Equals(type.Name, "PnlBattle", StringComparison.OrdinalIgnoreCase) ||
                        string.Equals(type.FullName, "Il2Cpp.PnlBattle", StringComparison.OrdinalIgnoreCase) ||
                        (type.FullName != null && type.FullName.EndsWith(".PnlBattle", StringComparison.OrdinalIgnoreCase)))
                    {
                        return type;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            MelonLogger.Error($"[PnlBattle.GameStart.Bgm] PnlBattle 타입 탐색 실패: {ex}");
        }

        return null;
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
            MelonLogger.Error($"[PnlBattle.GameStart.Bgm] AudioSource 생성 실패: {ex}");
            return null;
        }
    }

    private static void TryPlayVideo(Camera mainCam)
    {
        try
        {
            if (!Directory.Exists(HwaFolderPath))
            {
                return;
            }

            string[] mp4Files = Directory.GetFiles(HwaFolderPath, "*.mp4", SearchOption.TopDirectoryOnly);
            if (mp4Files == null || mp4Files.Length == 0)
            {
                MelonLogger.Msg("[PnlBattle.GameStart.Video] hwa 폴더에 mp4 파일이 없습니다.");
                return;
            }

            string mp4Path = mp4Files[0];
            FileInfo mp4Info = new FileInfo(mp4Path);
            MelonLogger.Msg($"[PnlBattle.GameStart.Video] mp4 비디오 주입 대상 발견: path={mp4Path}, fileName={mp4Info.Name}, size={mp4Info.Length}");

            // 기존에 혹시 생성되었던 비디오 쿼드가 있다면 깔끔히 정리
            Transform existingQuad = mainCam.transform.Find("VideoBackgroundQuad");
            if (existingQuad != null && existingQuad.gameObject != null)
            {
                UnityEngine.Object.Destroy(existingQuad.gameObject);
                MelonLogger.Msg("[PnlBattle.GameStart.Video] 기존 잔존 VideoBackgroundQuad 오브젝트를 삭제했습니다.");
            }

            // 1. 유니티 3D 쿼드(Quad) 오브젝트 동적 생성
            GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
            quad.name = "VideoBackgroundQuad";

            // 2. 불필요한 콜라이더 컴포넌트 제거 (물리 버그 및 오버헤드 방지)
            var col = quad.GetComponent("MeshCollider");
            if (col != null) UnityEngine.Object.Destroy(col);

            // 3. 카메라 아래로 귀속(Parent)시키고 Z 축 방향 뒤쪽으로 충분히 밀기
            // Camera_2D 위치가 Z = -10 이고 노트/플레이어 평면이 Z = 0 부근이므로,
            // localPosition.z = 25f로 두면 월드 Z = 15에 위치하여 노트 및 보스(Z = 0)보다 완벽히 뒤에 자리잡습니다.
            quad.transform.parent = mainCam.transform;
            quad.transform.localPosition = new Vector3(0f, 0f, 25f);

            // 4. 메인 오르토그래픽 카메라 화면 크기에 맞추어 쿼드 스케일링
            float height = mainCam.orthographicSize * 2f;
            float width = height * mainCam.aspect;
            quad.transform.localScale = new Vector3(width, height, 1f);

            // 5. Unlit/Texture 최경량 쉐이더 머티리얼 적용 (IL2CPP 환경 대응용 다중 폴백 적용)
            MeshRenderer renderer = quad.GetComponent<MeshRenderer>();
            Shader shader = Shader.Find("Unlit/Texture") 
                         ?? Shader.Find("Sprites/Default") 
                         ?? Shader.Find("UI/Default");

            if (shader != null)
            {
                renderer.material = new Material(shader);
                MelonLogger.Msg($"[PnlBattle.GameStart.Video] '{shader.name}' 쉐이더를 비디오 판넬에 주입했습니다.");
            }
            else
            {
                MelonLogger.Warning("[PnlBattle.GameStart.Video] 폴백 쉐이더를 찾지 못하여 기본 생성된 머티리얼을 재사용합니다.");
            }

            // 2D 오르토그래픽 스프라이트 정렬 버그 방지: 소팅 레이어를 Background로, 순서를 피드백에 따라 -17로 미세 조정합니다.
            renderer.sortingLayerName = "Background";
            renderer.sortingOrder = -17;

            // 6. 비디오 플레이어 부착 및 머티리얼 오버라이드 렌더링 설정
            VideoPlayer videoPlayer = quad.AddComponent<VideoPlayer>();
            videoPlayer.renderMode = VideoRenderMode.MaterialOverride;
            videoPlayer.targetMaterialRenderer = renderer;
            videoPlayer.targetMaterialProperty = "_MainTex";
            videoPlayer.audioOutputMode = VideoAudioOutputMode.Direct;
            videoPlayer.url = mp4Path;
            videoPlayer.isLooping = true;
            videoPlayer.Play();
            MelonLogger.Msg("[PnlBattle.GameStart.Video] 쿼드 머티리얼 기반 비디오 재생을 성공적으로 시작했습니다.");

            MelonCoroutines.Start(MonitorVideoPlayback(videoPlayer));
        }
        catch (Exception ex)
        {
            MelonLogger.Error($"[PnlBattle.GameStart.Video] 비디오 재생 중 오류 발생: {ex}");
        }
    }

    private static IEnumerator MonitorVideoPlayback(VideoPlayer vp)
    {
        if (vp == null) yield break;

        yield return new WaitForSeconds(1.0f);

        if (vp == null) yield break;

        MelonLogger.Msg($"[PnlBattle.GameStart.Video.Debug] 1초 후 비디오 재생 상태 상세 점검:");
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
        if (source == null)
        {
            return "(null)";
        }

        string goName = source.gameObject != null ? source.gameObject.name : "(no go)";
        string clipName = source.clip != null ? source.clip.name : "(null)";
        string clipDetails = source.clip != null ? DescribeAudioClip(source.clip) : "(null)";
        return $"AudioSource(go={goName}, clip={clipName}, playing={source.isPlaying}, mute={source.mute}, volume={source.volume}, loop={source.loop}, enabled={source.enabled}, clipInfo={clipDetails})";
    }

    private static string DescribeAudioClip(AudioClip clip)
    {
        if (clip == null)
        {
            return "(null)";
        }

        return $"AudioClip(name={clip.name}, length={clip.length:0.000}, samples={clip.samples}, frequency={clip.frequency}, channels={clip.channels}, loadType={clip.loadType}, loadState={clip.loadState}, preload={clip.preloadAudioData})";
    }
}