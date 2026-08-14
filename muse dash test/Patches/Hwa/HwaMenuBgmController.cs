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
        // 우리가 직접 주입한 커스텀 메뉴 BGM 클립만 추적합니다(게임 원본 클립은 절대 해제하지 않기 위해).
        private static AudioClip injectedMenuClip;

        public static void TriggerMenuBgmChange(string uid)
        {
            if (string.IsNullOrEmpty(uid) || !CustomContentIds.IsVirtualSong(uid))
            {
                currentLoadingUid = null;
                monitorGeneration++;
                return;
            }

            currentLoadingUid = uid;
            // 이번 요청의 세대 번호를 코루틴에 넘겨 줍니다. uid만으로는 A→B→A처럼 같은 곡으로
            // 되돌아온 경우 낡은 코루틴이 자기가 낡았다는 걸 알 수 없습니다(아래 코루틴 주석 참고).
            int generation = ++monitorGeneration;
            MelonLogger.Msg($"[MenuBGM] BGM 변경 트리거: uid={uid}, generation={generation}");
            MelonCoroutines.Start(LoadAndPlayCustomMenuBgm(uid, generation));
        }

        public static void StopMenuMonitoring(string reason)
        {
            currentLoadingUid = null;
            monitorGeneration++;
            MelonLogger.Msg($"[MenuBGM.Monitor] 메뉴 BGM 모니터링 중지 요청: {reason}");
        }

        /// <summary>
        /// 곡 선택/준비 화면을 벗어난 화면(메인 메뉴·캐릭터/엘핀 선택 등)으로 이동했을 때 호출합니다.
        /// 모니터링을 끄고, 우리가 주입한 커스텀 클립이 아직 "BGM" 소스에서 재생 중이면 멈춰서
        /// 게임이 자기 BGM을 되찾을 수 있게 합니다(커스텀 곡 미리듣기가 엉뚱한 화면까지 따라오는 문제 방지).
        /// </summary>
        public static void StopCustomMenuBgm(string reason)
        {
            currentLoadingUid = null;
            monitorGeneration++;
            try
            {
                GameObject bgmGo = GameObject.Find("BGM");
                if (bgmGo == null) return;

                AudioSource src = bgmGo.GetComponent<AudioSource>();
                if (src != null && injectedMenuClip != null && src.clip == injectedMenuClip)
                {
                    src.Stop();
                    MelonLogger.Msg($"[MenuBGM] 커스텀 메뉴 BGM 정지 ({reason}): 우리가 주입한 클립 재생 중단");
                }

                // 곡 선택/준비 화면 이탈 시 디스코드 프로필도 In Menu 상태로 즉시 복원
                var discordManager = Il2CppPeroTools2.Commons.Singleton<Il2Cpp.DiscordManager>.instance;
                if (discordManager != null)
                {
                    discordManager.SetUpdateActivity(false, "In Menu");
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[MenuBGM] StopCustomMenuBgm 예외: {ex}");
            }
        }

        /// <summary>
        /// 커스텀 곡의 ogg를 읽어 메뉴 BGM AudioSource에 주입합니다.
        ///
        /// <para><paramref name="generation"/>은 이 코루틴이 시작될 때 발급받은 요청 번호입니다.
        /// 중간에 다른 선택이 들어오면 <see cref="monitorGeneration"/>이 올라가므로, 두 값을 비교해
        /// 자기가 낡았는지 판단합니다. 예전에는 <c>currentLoadingUid != uid</c>로만 판단했는데,
        /// A→B→A로 빠르게 이동하면 <c>currentLoadingUid</c>가 다시 A로 돌아와 있어서 첫 번째 A
        /// 코루틴이 그대로 통과했습니다. 그 결과 A 코루틴 두 개가 같은 ogg를 로드하고 둘 다
        /// Play()를 불러 재생이 겹치고 클립 하나가 즉시 누수됐습니다.</para>
        /// </summary>
        private static IEnumerator LoadAndPlayCustomMenuBgm(string uid, int generation)
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
            if (generation != monitorGeneration || PnlStagePatchHelper.GetCurrentSelectedMusicUid() != uid)
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
                if (generation != monitorGeneration || PnlStagePatchHelper.GetCurrentSelectedMusicUid() != uid)
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

                    AudioClip previousInjected = injectedMenuClip;

                    menuSource.Stop();
                    menuSource.clip = customClip;
                    menuSource.loop = true;
                    menuSource.Play();
                    injectedMenuClip = customClip;

                    // 직전에 우리가 주입했던 클립을 해제합니다. 위에서 menuSource.clip을 새 클립으로
                    // 갈아끼웠으므로 이 시점의 previousInjected는 어떤 AudioSource도 참조하지 않습니다.
                    //
                    // injectedMenuClip에는 "우리가 만든 클립"만 담기므로(게임 원본 클립은 절대 들어오지
                    // 않습니다) 여기서 해제해도 게임 BGM을 건드리지 않습니다. 이 해제가 없으면 곡 선택
                    // 화면에서 커스텀 곡 사이를 오갈 때마다 디코딩된 OGG가 하나씩 쌓입니다.
                    if (previousInjected != null && previousInjected != customClip)
                    {
                        UnityEngine.Object.Destroy(previousInjected);
                        MelonLogger.Msg($"[MenuBGM] 이전 커스텀 클립 해제: {previousInjected.name}");
                    }

                    MelonLogger.Msg($"[MenuBGM] 커스텀 곡 BGM 주입 완료! uid={uid}, clip={customClip.name}, length={customClip.length}s, loadState={customClip.loadState}");
                    MelonLogger.Msg($"[MenuBGM] 주입 후 AudioSource 상태: isPlaying={menuSource.isPlaying}, volume={menuSource.volume} (이전: {prevVolume}), mute={menuSource.mute} (이전: {prevMute}), spatialBlend={menuSource.spatialBlend}");

                    // 후속 볼륨 페이드아웃이나 변경 현상 감시를 위해 실시간 모니터러 작동
                    // (바로 위 가드에서 generation == monitorGeneration을 확인했으므로 같은 값이며,
                    //  이 코루틴이 책임지는 세대를 그대로 넘기는 편이 의도가 분명합니다.)
                    MelonCoroutines.Start(MonitorAudioSource(menuSource, uid, customClip.name, generation));
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
                    string clipNameForLog = value != null ? value.name : "(null)";

                    // MD는 단일 씬(UISystem_PC) 구조라 씬 이름으로는 "곡 선택/준비 화면을 벗어났는지"를 구분할 수 없다.
                    // 대신 곡 선택(PnlStage)/준비(PnlPreparation) 패널이 실제로 활성 상태인지를 직접 확인한다.
                    bool inStageSelectionContext = IsStageSelectionContextActive();
                    MelonLogger.Msg($"[MenuBGM.Patch.Debug] set_clip 호출: selectedUid={selectedUid ?? "(null)"}, requestedClip={clipNameForLog}, inStageSelectionContext={inStageSelectionContext}");

                    if (!inStageSelectionContext)
                    {
                        // 곡 선택/준비 화면을 벗어나면 커스텀 BGM 재생을 정지하고 모니터링을 종료하여 게임이 자기 BGM을 회복하도록 합니다.
                        if (!string.IsNullOrEmpty(CustomPlaySession.Current.SelectedMusicUid))
                        {
                            MelonLogger.Msg($"[MenuBGM.Patch] 곡 선택 화면을 벗어남을 감지, 커스텀 BGM 정지 및 모니터링 종료: selectedUid={CustomPlaySession.Current.SelectedMusicUid}");
                            HwaMenuBgmController.StopCustomMenuBgm("곡 선택/준비 화면 이탈 감지");
                            HwaMenuBgmController.StopMenuMonitoring("곡 선택/준비 화면 이탈 감지");
                        }
                    }

                    // 클립 대입 차단은 곡 선택/준비 화면 안에서만 적용합니다. 메인 메뉴·캐릭터/엘핀
                    // 선택 등으로 빠져나온 뒤에도 차단하면, 게임이 자기 BGM을 되찾지 못해 커스텀 곡
                    // 미리듣기가 계속 재생되는 버그가 생깁니다.
                    if (inStageSelectionContext && CustomContentIds.IsVirtualSong(selectedUid))
                    {
                        bool isCustomClip = IsCustomOggClip(value);
                        if (!isCustomClip)
                        {
                            __instance.Stop();
                            MelonLogger.Msg($"[MenuBGM.Patch] 가상 곡 활성화 중 허용되지 않은 클립 대입 차단! (selectedUid={selectedUid}, 요청 클립: {clipNameForLog})");
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

        private static bool IsCustomOggClip(AudioClip clip)
        {
            if (clip == null || string.IsNullOrEmpty(clip.name))
            {
                return false;
            }

            return clip.name.EndsWith(".ogg", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 곡 선택(PnlStage) 또는 곡 준비(PnlPreparation) 패널이 현재 실제로 활성 상태인지 확인합니다.
        /// MD가 단일 씬 안에서 패널만 켜고 끄는 구조이므로, 이게 "곡 선택 컨텍스트에 있는지"의 유일하게 신뢰 가능한 신호입니다.
        /// </summary>
        private static bool IsStageSelectionContextActive()
        {
            try
            {
                var pnlStage = UnityEngine.Object.FindObjectOfType<Il2CppAssets.Scripts.UI.Panels.PnlStage>();
                if (pnlStage != null && pnlStage.gameObject != null && pnlStage.gameObject.activeInHierarchy)
                {
                    return true;
                }

                var pnlPreparation = UnityEngine.Object.FindObjectOfType<Il2Cpp.PnlPreparation>();
                if (pnlPreparation != null && pnlPreparation.gameObject != null && pnlPreparation.gameObject.activeInHierarchy)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[MenuBGM.Patch] IsStageSelectionContextActive 예외: {ex}");
            }
            return false;
        }
    }
}
