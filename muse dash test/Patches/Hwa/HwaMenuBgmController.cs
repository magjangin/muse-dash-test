using MelonLoader;
using System;
using System.Collections;
using HarmonyLib;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace muse_dash_test
{
    /// <summary>
    /// 곡 선택 화면(PnlStage) 및 곡 준비 화면(PnlPreparation)에서 가상 곡(1999-*) 선택 시 
    /// 씬 안의 BGM 재생용 AudioSource를 감지하여 로컬 ogg 파일로 BGM을 실시간 핫스왑하는 컨트롤러입니다.
    /// </summary>
    public static class HwaMenuBgmController
    {
        private static string currentLoadingUid = null;
        private static int monitorGeneration;

        public static void TriggerMenuBgmChange(string uid)
        {
            if (string.IsNullOrEmpty(uid) || !CustomContentIds.IsVirtualSong(uid))
            {
                currentLoadingUid = null;
                monitorGeneration++;
                return;
            }

            currentLoadingUid = uid;
            monitorGeneration++;
            MelonLogger.Msg($"[MenuBGM] BGM 변경 트리거: uid={uid}");
            MelonCoroutines.Start(LoadAndPlayCustomMenuBgm(uid));
        }

        public static void StopMenuMonitoring(string reason)
        {
            currentLoadingUid = null;
            monitorGeneration++;
            MelonLogger.Msg($"[MenuBGM.Monitor] 메뉴 BGM 모니터링 중지 요청: {reason}");
        }

        private static IEnumerator LoadAndPlayCustomMenuBgm(string uid)
        {
            string songDir;
            if (!HwaResourceManager.TryGetSongDirectory(uid, out songDir) || string.IsNullOrEmpty(songDir))
            {
                yield break;
            }

            string oggPath = ResolveHwaOggPath(songDir);
            if (string.IsNullOrWhiteSpace(oggPath) || !File.Exists(oggPath))
            {
                MelonLogger.Warning($"[MenuBGM] ogg 파일을 찾지 못했습니다: folder={songDir}");
                yield break;
            }

            // 게임 엔진이 기본 오디오 재생을 초기화하고 원래 오디오가 로드 및 실행을 시작할 때까지 0.2초 대기합니다.
            yield return new WaitForSeconds(0.2f);

            // 대기 시간 도중 다른 곡으로 선택이 넘어간 경우 로딩을 중단합니다.
            if (currentLoadingUid != uid || PnlStagePatchHelper.GetCurrentSelectedMusicUid() != uid)
            {
                yield break;
            }

            AudioSource menuSource = FindMenuAudioSource();
            if (menuSource == null)
            {
                MelonLogger.Warning("[MenuBGM] 활성화된 BGM AudioSource를 찾지 못했습니다.");
                yield break;
            }

            MelonLogger.Msg($"[MenuBGM] 대상 AudioSource 선택됨: GO={menuSource.gameObject.name}, Clip={menuSource.clip?.name ?? "(null)"}, Vol={menuSource.volume}, Mute={menuSource.mute}, SpatialBlend={menuSource.spatialBlend}, MixerGroup={menuSource.outputAudioMixerGroup?.name ?? "(null)"}");

            string uri = new Uri(oggPath).AbsoluteUri;
            UnityWebRequest request = new UnityWebRequest(uri, "GET");
            try
            {
                DownloadHandlerAudioClip handler = new DownloadHandlerAudioClip(uri, AudioType.OGGVORBIS);
                handler.streamAudio = true;
                request.downloadHandler = handler;
                yield return request.SendWebRequest();

                if (!string.IsNullOrWhiteSpace(request.error))
                {
                    MelonLogger.Error($"[MenuBGM] OGG 로드 실패: {oggPath}, error={request.error}");
                    yield break;
                }

                // 로드가 완료되었을 시점에도 유효성 검사 (사용자가 곡을 다시 변경했는지 여부)
                if (currentLoadingUid != uid || PnlStagePatchHelper.GetCurrentSelectedMusicUid() != uid)
                {
                    yield break;
                }

                AudioClip customClip = null;
                try
                {
                    customClip = DownloadHandlerAudioClip.GetContent(request);
                }
                catch (Exception ex)
                {
                    MelonLogger.Error($"[MenuBGM] AudioClip 변환 실패: {ex.Message}");
                }

                if (customClip != null)
                {
                    customClip.name = Path.GetFileName(oggPath);
                    float prevVolume = menuSource.volume;
                    bool prevMute = menuSource.mute;

                    menuSource.Stop();
                    menuSource.clip = customClip;
                    menuSource.loop = true;
                    menuSource.Play();
                    MelonLogger.Msg($"[MenuBGM] 커스텀 곡 BGM 주입 완료! uid={uid}, clip={customClip.name}, length={customClip.length}s, loadState={customClip.loadState}");
                    MelonLogger.Msg($"[MenuBGM] 주입 후 AudioSource 상태: isPlaying={menuSource.isPlaying}, volume={menuSource.volume} (이전: {prevVolume}), mute={menuSource.mute} (이전: {prevMute}), spatialBlend={menuSource.spatialBlend}");

                    // 후속 볼륨 페이드아웃이나 변경 현상 감시를 위해 실시간 모니터러 작동
                    MelonCoroutines.Start(MonitorAudioSource(menuSource, uid, customClip.name, monitorGeneration));
                }
            }
            finally
            {
                request.Dispose();
            }
        }

        private static AudioSource FindMenuAudioSource()
        {
            try
            {
                // 0단계: 핀포인트로 "BGM" 오브젝트 검색 시도
                GameObject bgmGo = GameObject.Find("BGM");
                if (bgmGo != null)
                {
                    AudioSource source = bgmGo.GetComponent<AudioSource>();
                    if (source != null && source.gameObject.activeInHierarchy)
                    {
                        MelonLogger.Msg($"[MenuBGM] 핀포인트 매칭 성공: GO={bgmGo.name}");
                        return source;
                    }
                }

                // 핀포인트 검색 실패 시에만 예외적으로 씬 내 전체 스캔 진행 (폴백)
                MelonLogger.Msg("[MenuBGM] 핀포인트 검색 실패, 씬 내 모든 AudioSource 스캔 폴백 진행");
                AudioSource[] sources = UnityEngine.Object.FindObjectsOfType<AudioSource>();
                if (sources == null || sources.Length == 0)
                {
                    MelonLogger.Warning("[MenuBGM] 씬 내에 어떠한 AudioSource도 존재하지 않습니다.");
                    return null;
                }

                foreach (AudioSource source in sources)
                {
                    if (source == null) continue;
                    string goName = source.gameObject != null ? source.gameObject.name : "(null)";
                    string clipName = source.clip != null ? source.clip.name : "(null)";
                    MelonLogger.Msg($"[MenuBGM] 후보 - GO: {goName}, Clip: {clipName}, Playing: {source.isPlaying}, Vol: {source.volume}, Mute: {source.mute}, SpatialBlend: {source.spatialBlend}, Enabled: {source.enabled}, Active: {source.gameObject?.activeInHierarchy}");
                }

                AudioSource selectedSource = FindActiveSource(s => s.gameObject.name.Equals("BGM", StringComparison.OrdinalIgnoreCase));
                if (selectedSource != null)
                {
                    MelonLogger.Msg($"[MenuBGM] 0단계(이름 매칭) 성공: GO={selectedSource.gameObject.name}");
                    return selectedSource;
                }

                selectedSource = FindActiveSource(IsLikelySoundtrackSource);
                if (selectedSource != null)
                {
                    MelonLogger.Msg($"[MenuBGM] 1단계 매칭 성공: GO={selectedSource.gameObject.name.ToLower()}, Clip={selectedSource.clip.name}");
                    return selectedSource;
                }

                selectedSource = FindActiveSource(IsPlayingNonEffectSource);
                if (selectedSource != null)
                {
                    MelonLogger.Msg($"[MenuBGM] 2단계 매칭 성공: GO={selectedSource.gameObject.name.ToLower()}, Clip={selectedSource.clip.name}");
                    return selectedSource;
                }

                AudioSource FindActiveSource(Func<AudioSource, bool> predicate)
                {
                    foreach (AudioSource candidate in sources)
                    {
                        if (candidate == null || candidate.gameObject == null || !candidate.gameObject.activeInHierarchy) continue;
                        if (predicate(candidate)) return candidate;
                    }

                    return null;
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[MenuBGM] FindMenuAudioSource 예외: {ex}");
            }
            return null;
        }

        private static bool IsLikelySoundtrackSource(AudioSource source)
        {
            if (source.clip == null || !source.isPlaying) return false;

            string clipName = source.clip.name.ToLower();
            string goName = source.gameObject.name.ToLower();
            return clipName.Contains("demo") || clipName.Contains("music") || clipName.Contains("bgm") || goName.Contains("music") || goName.Contains("bgm");
        }

        private static bool IsPlayingNonEffectSource(AudioSource source)
        {
            if (source.clip == null || !source.isPlaying) return false;

            string clipName = source.clip.name.ToLower();
            return !clipName.Contains("click") && !clipName.Contains("sfx") && !clipName.Contains("button");
        }

        private static string ResolveHwaOggPath(string folderPath)
        {
            try
            {
                if (!Directory.Exists(folderPath)) return null;
                string[] oggFiles = Directory.GetFiles(folderPath, "*.ogg", SearchOption.AllDirectories);
                if (oggFiles == null || oggFiles.Length == 0) return null;
                Array.Sort(oggFiles, StringComparer.OrdinalIgnoreCase);

                foreach (string oggFile in oggFiles)
                {
                    string lower = Path.GetFileNameWithoutExtension(oggFile).ToLowerInvariant();
                    if (lower.Contains("bgm") || lower.Contains("battle") || lower.Contains("music") || lower.Contains("song") || lower.Contains("demo"))
                    {
                        return oggFile;
                    }
                }
                return oggFiles[0];
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[MenuBGM] ogg 탐색 실패: {ex}");
                return null;
            }
        }

        private static IEnumerator MonitorAudioSource(AudioSource source, string uid, string clipName, int generation)
        {
            MelonLogger.Msg($"[MenuBGM.Monitor] 모니터링 시작: GO={source.gameObject.name}, targetClip={clipName}");
            int i = 0;
            while (true)
            {
                yield return new WaitForSeconds(5.0f);
                if (generation != monitorGeneration)
                {
                    MelonLogger.Msg("[MenuBGM.Monitor] 모니터링 세대가 변경되어 종료합니다.");
                    yield break;
                }

                if (source == null)
                {
                    MelonLogger.Warning("[MenuBGM.Monitor] AudioSource가 파괴되었습니다.");
                    yield break;
                }

                string currentClip = source.clip != null ? source.clip.name : "(null)";
                if (currentClip != clipName || PnlStagePatchHelper.GetCurrentSelectedMusicUid() != uid)
                {
                    MelonLogger.Msg($"[MenuBGM.Monitor] 대상 클립 또는 선택 곡이 변경되어 모니터링을 종료합니다. (현재 클립: {currentClip})");
                    yield break;
                }

                MelonLogger.Msg($"[MenuBGM.Monitor] T+{i*5.0f:F1}s - Playing: {source.isPlaying}, Vol: {source.volume:F4}, Mute: {source.mute}, Clip: {currentClip}, Time: {source.time:F2}");
                i++;
            }
        }
    }

    [HarmonyPatch(typeof(AudioSource), "clip", MethodType.Setter)]
    public class AudioSource_SetClip_Patch
    {
        public static bool Prefix(AudioSource __instance, ref AudioClip value)
        {
            try
            {
                if (__instance != null && __instance.gameObject != null && __instance.gameObject.name == "BGM")
                {
                    string selectedUid = PnlStagePatchHelper.GetCurrentSelectedMusicUid();
                    if (CustomContentIds.IsVirtualSong(selectedUid))
                    {
                        bool isCustomClip = value != null && (value.name.EndsWith(".ogg") || value.name.Equals("music.ogg"));
                        if (!isCustomClip)
                        {
                            string clipName = value != null ? value.name : "(null)";
                            MelonLogger.Msg($"[MenuBGM.Patch] 가상 곡 활성화 중 허용되지 않은 클립 대입 차단! (요청 클립: {clipName})");
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[MenuBGM.Patch] set_clip 패치 에러: {ex}");
            }
            return true;
        }
    }
}
