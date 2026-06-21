using MelonLoader;
using System;
using System.Collections;
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

        public static void TriggerMenuBgmChange(string uid)
        {
            if (string.IsNullOrEmpty(uid) || !CustomContentIds.IsVirtualSong(uid))
            {
                currentLoadingUid = null;
                return;
            }

            currentLoadingUid = uid;
            MelonLogger.Msg($"[MenuBGM] BGM 변경 트리거: uid={uid}");
            MelonCoroutines.Start(LoadAndPlayCustomMenuBgm(uid));
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

            string uri = new Uri(oggPath).AbsoluteUri;
            UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(uri, AudioType.OGGVORBIS);
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
                menuSource.clip = customClip;
                menuSource.loop = true;
                menuSource.Play();
                MelonLogger.Msg($"[MenuBGM] 커스텀 곡 BGM 주입 완료! uid={uid}, clip={customClip.name}");
            }
        }

        private static AudioSource FindMenuAudioSource()
        {
            try
            {
                AudioSource[] sources = UnityEngine.Object.FindObjectsOfType<AudioSource>();
                if (sources == null || sources.Length == 0) return null;

                // 1단계: 재생 중이며 이름이 music, bgm, demo 등 사운드 트랙 계열의 클립이나 오브젝트명을 갖는 AudioSource 탐색
                foreach (AudioSource source in sources)
                {
                    if (source == null || source.gameObject == null || !source.gameObject.activeInHierarchy) continue;
                    if (source.clip == null) continue;

                    string clipName = source.clip.name.ToLower();
                    string goName = source.gameObject.name.ToLower();

                    if (source.isPlaying && (clipName.Contains("demo") || clipName.Contains("music") || clipName.Contains("bgm") || goName.Contains("music") || goName.Contains("bgm")))
                    {
                        return source;
                    }
                }

                // 2단계: 효과음(click, SFX) 등을 제외한 나머지 재생 중인 BGM AudioSource 탐색
                foreach (AudioSource source in sources)
                {
                    if (source == null || source.gameObject == null || !source.gameObject.activeInHierarchy) continue;
                    if (source.clip == null) continue;

                    string clipName = source.clip.name.ToLower();
                    if (source.isPlaying && !clipName.Contains("click") && !clipName.Contains("sfx") && !clipName.Contains("button"))
                    {
                        return source;
                    }
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[MenuBGM] FindMenuAudioSource 예외: {ex}");
            }
            return null;
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
    }
}
