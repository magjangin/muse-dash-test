using MelonLoader;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Il2CppFormulaBase;
using Il2CppGameLogic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Video;

// Il2CppFormulaBase.StageBattleComponent.LoadMusicData 하모니 패치
[HarmonyLib.HarmonyPatch(typeof(StageBattleComponent), "LoadMusicData")]
public class StageBattleComponent_LoadMusicData_Patch
{
    public static void Postfix(StageBattleComponent __instance) { }

    public static void DumpStageBattleComponentProperties(StageBattleComponent __instance)
    {
        MelonLogger.Msg($"StageBattleComponent Properties for instance: {__instance}");
        foreach (var prop in __instance.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            try
            {
                var value = prop.GetValue(__instance);
                MelonLogger.Msg($"  {prop.Name}: {value}");

                if ((prop.Name == "m_MusicTickData" || prop.Name == "m_SortedMusicTickData" || prop.Name == "m_TimeNodeOrders") && value != null)
                {
                    MelonLogger.Msg($"    {prop.Name} contains:");
                    if (value is Il2CppSystem.Collections.Generic.List<MusicData> musicDataList)
                    {
                        int count = musicDataList.Count;
                        for (int i = 0; i < count; i++)
                        {
                            var musicData = musicDataList[i];
                            MelonLogger.Msg($"      MusicData {i}: {musicData}");
                            foreach (var musicProp in musicData.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
                            {
                                try
                                {
                                    var musicValue = musicProp.GetValue(musicData);
                                    MelonLogger.Msg($"        {musicProp.Name}: {musicValue}");

                                    // configData와 noteData가 Il2Cpp 객체이므로 내부 프로퍼티를 덤프
                                    if (musicProp.Name == "configData" && musicValue != null)
                                    {
                                        MelonLogger.Msg($"          {musicProp.Name} properties:");
                                        foreach (var cfgProp in musicValue.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
                                        {
                                            try
                                            {
                                                var cfgValue = cfgProp.GetValue(musicValue);
                                                MelonLogger.Msg($"            {cfgProp.Name}: {cfgValue}");
                                            }
                                            catch (System.Exception ex)
                                            {
                                                MelonLogger.Msg($"            {cfgProp.Name}: (예외 발생: {ex.Message})");
                                            }
                                        }
                                    }
                                    else if (musicProp.Name == "noteData" && musicValue != null)
                                    {
                                        MelonLogger.Msg($"          {musicProp.Name} properties:");
                                        foreach (var noteProp in musicValue.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
                                        {
                                            try
                                            {
                                                var noteVal = noteProp.GetValue(musicValue);
                                                MelonLogger.Msg($"            {noteProp.Name}: {noteVal}");
                                            }
                                            catch (System.Exception ex)
                                            {
                                                MelonLogger.Msg($"            {noteProp.Name}: (예외 발생: {ex.Message})");
                                            }
                                        }
                                    }
                                }
                                catch (System.Exception ex)
                                {
                                    MelonLogger.Msg($"        {musicProp.Name}: (예외 발생: {ex.Message})");
                                }
                            }
                        }
                    }
                    else if (value is Il2CppSystem.Collections.Generic.List<TimeNodeOrder> timeNodeOrderList)
                    {
                        int count = timeNodeOrderList.Count;
                        for (int i = 0; i < count; i++)
                        {
                            var timeNodeOrder = timeNodeOrderList[i];
                            MelonLogger.Msg($"      TimeNodeOrder {i}: {timeNodeOrder}");
                            foreach (var orderProp in timeNodeOrder.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
                            {
                                try
                                {
                                    var orderValue = orderProp.GetValue(timeNodeOrder);
                                    MelonLogger.Msg($"        {orderProp.Name}: {orderValue}");
                                }
                                catch (System.Exception ex)
                                {
                                    MelonLogger.Msg($"        {orderProp.Name}: (예외 발생: {ex.Message})");
                                }
                            }
                        }
                    }
                    else
                    {
                        MelonLogger.Msg($"    {prop.Name} is not a List<MusicData> or List<TimeNodeOrder>, actual type: {value.GetType()}");
                    }
                }
                // sceneInfo 덤프 로직은 제거됨
            }
            catch (System.Exception ex)
            {
                MelonLogger.Msg($"  {prop.Name}: (예외 발생: {ex.Message})");
            }
        }
    }

}

[HarmonyLib.HarmonyPatch(typeof(StageBattleComponent), "InitData")]
public class StageBattleComponent_InitData_Patch
{
    public static void Postfix(StageBattleComponent __instance)
    {
        string uid = PnlStagePatchHelper.LastSelectedMusicUid;
        if (string.IsNullOrEmpty(uid))
        {
            uid = PnlStagePatchHelper.GetCurrentSelectedMusicUid() ?? muse_dash_test.MusicButtonCell_OnButtonClicked_Patch.LastClickedMusicUid ?? "(unknown)";
        }
        MelonLogger.Msg($"StageBattleComponent.InitData 호출됨: {__instance}, 곡 UID={uid}");
    }
}

[HarmonyLib.HarmonyPatch(typeof(StageBattleComponent), "Load")]
public class StageBattleComponent_Load_Patch
{
    public static void Postfix(StageBattleComponent __instance)
    {
        try
        {
            MelonLogger.Msg($"[StageBattleComponent.Load] 호출됨: {__instance}");
            StageBattleComponent_LoadMedia_Patch.StartBattleMediaInjection();
        }
        catch (System.Exception ex)
        {
            MelonLogger.Error($"[StageBattleComponent.Load] 예외 발생: {ex}");
        }
    }
}

public static class StageBattleComponent_LoadMedia_Patch
{
    private static readonly string HwaFolderPath = Path.Combine(MelonLoader.Utils.MelonEnvironment.GameRootDirectory, "hwa");
    private static bool battleMediaInjectionStarted;

    public static void StartBattleMediaInjection()
    {
        try
        {
            if (battleMediaInjectionStarted)
            {
                return;
            }

            if (!ExperimentPlayContext.ShouldApplyExperimentChart)
            {
                return;
            }

            battleMediaInjectionStarted = true;
            InjectBattleMedia();
        }
        catch (Exception ex)
        {
            MelonLogger.Error($"[StageBattleComponent.Load.Bgm] 주입 시작 실패: {ex}");
        }
    }

    private static void InjectBattleMedia()
    {
        Camera mainCam = Camera.main;
        if (mainCam != null)
        {
            MelonLogger.Msg($"[StageBattleComponent.Load.Bgm] 메인 카메라 감지 완료: name={mainCam.name}, position={mainCam.transform.position}");
            TryPlayVideo(mainCam);
        }
        else
        {
            MelonLogger.Warning("[StageBattleComponent.Load.Bgm] 메인 카메라를 찾지 못했습니다.");
        }

        string oggPath = ResolveHwaOggPath();
        if (string.IsNullOrWhiteSpace(oggPath))
        {
            MelonLogger.Msg($"[StageBattleComponent.Load.Bgm] ogg 파일을 찾지 못했습니다: folder={HwaFolderPath}");
            return;
        }

        FileInfo oggInfo = new FileInfo(oggPath);
        MelonLogger.Msg($"[StageBattleComponent.Load.Bgm] ogg 주입 대상 발견: path={oggPath}, fileName={oggInfo.Name}, size={oggInfo.Length}, lastWrite={oggInfo.LastWriteTime:yyyy-MM-dd HH:mm:ss}");

        AudioSource targetSource = FindBattleAudioSource();
        if (targetSource == null)
        {
            targetSource = CreateBattleAudioSource();
        }

        if (targetSource == null)
        {
            MelonLogger.Msg("[StageBattleComponent.Load.Bgm] 배틀 AudioSource를 찾거나 생성하지 못했습니다.");
            return;
        }

        MelonLogger.Msg($"[StageBattleComponent.Load.Bgm] 대상 AudioSource 선택: {DescribeAudioSource(targetSource)}");
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
            MelonLogger.Error($"[StageBattleComponent.Load.Bgm] ogg 경로 변환 실패: {ex}");
        }

        if (!uriReady)
        {
            yield break;
        }

        UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(uri, AudioType.OGGVORBIS);
        yield return request.SendWebRequest();

        if (!string.IsNullOrWhiteSpace(request.error))
        {
            MelonLogger.Error($"[StageBattleComponent.Load.Bgm] ogg 로드 실패: path={oggPath}, error={request.error}");
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
            MelonLogger.Error($"[StageBattleComponent.Load.Bgm] 오디오 클립 변환 실패: {oggPath}, error={ex}");
        }

        if (!clipReady || clip == null)
        {
            MelonLogger.Error($"[StageBattleComponent.Load.Bgm] 오디오 클립이 비어 있습니다: {oggPath}");
            yield break;
        }

        MelonLogger.Msg($"[StageBattleComponent.Load.Bgm] 로드한 클립 정보: {DescribeAudioClip(clip)}");

        string beforeState = DescribeAudioSource(targetSource);

        targetSource.clip = clip;
        targetSource.loop = true;
        targetSource.playOnAwake = false;
        targetSource.Stop();
        targetSource.Play();

        MelonLogger.Msg($"[StageBattleComponent.Load.Bgm] 배틀 BGM 주입 완료: before={beforeState}, after={DescribeAudioSource(targetSource)}, loadedClip={DescribeAudioClip(clip)}");
    }

    private static string ResolveHwaOggPath()
    {
        try
        {
            if (!Directory.Exists(HwaFolderPath))
            {
                return null;
            }

            string[] txtFiles = Directory.GetFiles(HwaFolderPath, "*.txt", SearchOption.AllDirectories);
            string[] oggFiles = Directory.GetFiles(HwaFolderPath, "*.ogg", SearchOption.AllDirectories);

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
            MelonLogger.Error($"[StageBattleComponent.Load.Bgm] ogg 탐색 실패: {ex}");
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
            MelonLogger.Error($"[StageBattleComponent.Load.Bgm] AudioSource 탐색 실패: {ex}");
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
            MelonLogger.Error($"[StageBattleComponent.Load.Bgm] AudioSource 생성 실패: {ex}");
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

            string[] mp4Files = Directory.GetFiles(HwaFolderPath, "*.mp4", SearchOption.AllDirectories);
            if (mp4Files == null || mp4Files.Length == 0)
            {
                MelonLogger.Msg("[StageBattleComponent.Load.Video] hwa 폴더 및 하위 폴더에 mp4 파일이 없습니다.");
                return;
            }

            Array.Sort(mp4Files, StringComparer.OrdinalIgnoreCase);
            string mp4Path = mp4Files[0];
            FileInfo mp4Info = new FileInfo(mp4Path);
            MelonLogger.Msg($"[StageBattleComponent.Load.Video] mp4 비디오 주입 대상 발견: path={mp4Path}, fileName={mp4Info.Name}, size={mp4Info.Length}");

            Transform existingQuad = mainCam.transform.Find("VideoBackgroundQuad");
            if (existingQuad != null && existingQuad.gameObject != null)
            {
                UnityEngine.Object.Destroy(existingQuad.gameObject);
                MelonLogger.Msg("[StageBattleComponent.Load.Video] 기존 잔존 VideoBackgroundQuad 오브젝트를 삭제했습니다.");
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
                MelonLogger.Msg($"[StageBattleComponent.Load.Video] '{shader.name}' 쉐이더를 비디오 판넬에 주입했습니다.");
            }
            else
            {
                MelonLogger.Warning("[StageBattleComponent.Load.Video] 폴백 쉐이더를 찾지 못하여 기본 생성된 머티리얼을 재사용합니다.");
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
            MelonLogger.Msg("[StageBattleComponent.Load.Video] 쿼드 머티리얼 기반 비디오 재생을 성공적으로 시작했습니다.");

            MelonCoroutines.Start(MonitorVideoPlayback(videoPlayer));
        }
        catch (Exception ex)
        {
            MelonLogger.Error($"[StageBattleComponent.Load.Video] 비디오 재생 중 오류 발생: {ex}");
        }
    }

    private static IEnumerator MonitorVideoPlayback(VideoPlayer vp)
    {
        if (vp == null) yield break;

        yield return new WaitForSeconds(1.0f);

        if (vp == null) yield break;

        MelonLogger.Msg($"[StageBattleComponent.Load.Video.Debug] 1초 후 비디오 재생 상태 상세 점검:");
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

[HarmonyLib.HarmonyPatch(typeof(StageBattleComponent), "Pause")]
public class StageBattleComponent_Pause_Patch
{
    public static void Postfix(StageBattleComponent __instance, bool pauseCorountine)
    {
        try
        {
            MelonLogger.Msg($"[StageBattleComponent.Pause] 게임 일시정지 호출됨: pauseCorountine={pauseCorountine}");

            // 1. 커스텀 BGA 비디오 일시정지
            Camera mainCam = Camera.main;
            if (mainCam != null)
            {
                VideoPlayer vp = mainCam.GetComponentInChildren<VideoPlayer>();
                if (vp != null && vp.isPlaying)
                {
                    vp.Pause();
                    MelonLogger.Msg("[StageBattleComponent.Pause] 배경 비디오 재생을 일시정지했습니다.");
                }
            }

            // 2. 커스텀 BGM 오디오 일시정지
            GameObject bgmGo = GameObject.Find("HwaBattleBgmSource");
            if (bgmGo != null)
            {
                AudioSource bgm = bgmGo.GetComponent<AudioSource>();
                if (bgm != null && bgm.isPlaying)
                {
                    bgm.Pause();
                    MelonLogger.Msg("[StageBattleComponent.Pause] 커스텀 BGM 오디오 재생을 일시정지했습니다.");
                }
            }
        }
        catch (System.Exception ex)
        {
            MelonLogger.Error($"[StageBattleComponent.Pause] 예외 발생: {ex}");
        }
    }
}

[HarmonyLib.HarmonyPatch(typeof(StageBattleComponent), "Resume")]
public class StageBattleComponent_Resume_Patch
{
    public static void Postfix(StageBattleComponent __instance, bool isExit)
    {
        try
        {
            MelonLogger.Msg($"[StageBattleComponent.Resume] 게임 재개 호출됨: isExit={isExit}");

            if (isExit)
            {
                return;
            }

            // 1. 커스텀 BGA 비디오 재개
            Camera mainCam = Camera.main;
            if (mainCam != null)
            {
                VideoPlayer vp = mainCam.GetComponentInChildren<VideoPlayer>();
                if (vp != null)
                {
                    bool wasPlaying = vp.isPlaying;
                    bool wasPrepared = vp.isPrepared;
                    vp.Play();
                    MelonLogger.Msg($"[StageBattleComponent.Resume] 배경 비디오 재생을 재개했습니다. (이전 상태: isPlaying={wasPlaying}, isPrepared={wasPrepared})");
                }
            }

            // 2. 커스텀 BGM 오디오 재개
            GameObject bgmGo = GameObject.Find("HwaBattleBgmSource");
            if (bgmGo != null)
            {
                AudioSource bgm = bgmGo.GetComponent<AudioSource>();
                if (bgm != null && !bgm.isPlaying)
                {
                    bgm.UnPause();
                    MelonLogger.Msg("[StageBattleComponent.Resume] 커스텀 BGM 오디오 재생을 재개했습니다.");
                }
            }
        }
        catch (System.Exception ex)
        {
            MelonLogger.Error($"[StageBattleComponent.Resume] 예외 발생: {ex}");
        }
    }
}

[HarmonyLib.HarmonyPatch(typeof(StageBattleComponent), "End")]
public class StageBattleComponent_End_Patch
{
    public static void Postfix(StageBattleComponent __instance)
    {
        try
        {
            MelonLogger.Msg("[StageBattleComponent.End] 스테이지 종료 호출됨 - 비디오 및 BGM을 정지합니다.");

            // 1. 커스텀 BGA 비디오 완전히 정지
            Camera mainCam = Camera.main;
            if (mainCam != null)
            {
                VideoPlayer vp = mainCam.GetComponentInChildren<VideoPlayer>();
                if (vp != null)
                {
                    vp.Stop();
                    MelonLogger.Msg("[StageBattleComponent.End] 배경 비디오 재생을 완전히 멈췄습니다.");
                }
            }

            // 2. 커스텀 BGM 오디오 완전히 정지
            GameObject bgmGo = GameObject.Find("HwaBattleBgmSource");
            if (bgmGo != null)
            {
                AudioSource bgm = bgmGo.GetComponent<AudioSource>();
                if (bgm != null)
                {
                    bgm.Stop();
                    MelonLogger.Msg("[StageBattleComponent.End] 커스텀 BGM 오디오 재생을 완전히 멈췄습니다.");
                }
            }
        }
        catch (System.Exception ex)
        {
            MelonLogger.Error($"[StageBattleComponent.End] 예외 발생: {ex}");
        }
    }
}

