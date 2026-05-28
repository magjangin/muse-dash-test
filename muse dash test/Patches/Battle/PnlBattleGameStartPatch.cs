using MelonLoader;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Il2CppAssets.Scripts.UI.Panels;
using UnityEngine;
using UnityEngine.Networking;

// PnlBattle.GameStart 후킹: hwa 폴더의 ogg를 찾아 배틀 BGM으로 주입
[HarmonyLib.HarmonyPatch]
public class PnlBattle_GameStart_Patch
{
    private static readonly string HwaFolderPath = Path.Combine("H:\\steam\\steamapps\\common\\Muse Dash", "hwa");

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