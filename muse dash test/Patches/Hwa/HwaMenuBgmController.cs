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
    public static partial class HwaMenuBgmController
    {
        private static string currentLoadingUid = null;
        private static string activeMenuBgmUid = null;
        private static int monitorGeneration;
        // 우리가 직접 주입한 커스텀 메뉴 BGM 클립만 추적합니다(게임 원본 클립은 절대 해제하지 않기 위해).
        private static AudioClip injectedMenuClip;
        private static bool isCustomMenuBgmActive;
        private static object activeMenuBgmCoroutine;

        internal static bool IsCustomMenuBgmActive => isCustomMenuBgmActive || !string.IsNullOrEmpty(currentLoadingUid);
        internal static string CurrentLoadingUid => currentLoadingUid;
        internal static AudioClip InjectedMenuClip => injectedMenuClip;

        public static void TriggerMenuBgmChange(string uid)
        {
            if (string.IsNullOrEmpty(uid) || !CustomContentIds.IsVirtualSong(uid))
            {
                currentLoadingUid = null;
                activeMenuBgmUid = null;
                isCustomMenuBgmActive = false;
                monitorGeneration++;
                if (activeMenuBgmCoroutine != null)
                {
                    MelonCoroutines.Stop(activeMenuBgmCoroutine);
                    activeMenuBgmCoroutine = null;
                }
                return;
            }

            if ((activeMenuBgmUid == uid || currentLoadingUid == uid) && injectedMenuClip != null)
            {
                AudioSource currentSource = FindMenuAudioSource();
                if (currentSource != null && currentSource.clip == injectedMenuClip && currentSource.isPlaying)
                {
                    ModLogger.Verbose($"[MenuBGM] uid={uid}는 이미 재생 중이므로 재로드를 건너뜁니다.");
                    return;
                }
            }

            currentLoadingUid = uid;
            activeMenuBgmUid = uid;
            isCustomMenuBgmActive = true;

            if (activeMenuBgmCoroutine != null)
            {
                MelonCoroutines.Stop(activeMenuBgmCoroutine);
                activeMenuBgmCoroutine = null;
            }

            // 이번 요청의 세대 번호를 코루틴에 넘겨 줍니다. uid만으로는 A→B→A처럼 같은 곡으로
            // 되돌아온 경우 낡은 코루틴이 자기가 낡았다는 걸 알 수 없습니다(아래 코루틴 주석 참고).
            int generation = ++monitorGeneration;
            ModLogger.Verbose($"[MenuBGM] BGM 변경 트리거: uid={uid}, generation={generation}");
            activeMenuBgmCoroutine = MelonCoroutines.Start(LoadAndPlayCustomMenuBgm(uid, generation));
        }

        public static void StopMenuMonitoring(string reason)
        {
            currentLoadingUid = null;
            activeMenuBgmUid = null;
            isCustomMenuBgmActive = false;
            monitorGeneration++;
            if (activeMenuBgmCoroutine != null)
            {
                MelonCoroutines.Stop(activeMenuBgmCoroutine);
                activeMenuBgmCoroutine = null;
            }
            ModLogger.Msg($"[MenuBGM.Monitor] 메뉴 BGM 모니터링 중지 요청: {reason}");
        }

        /// <summary>씬이 새로 로드될 때(예: 배틀 종료 후 UISystem_PC 재진입) 파괴된 이전 패널 참조 캐시를 비웁니다.</summary>
        public static void ResetSceneCache()
        {
            AudioSource_SetClip_Patch.ResetCache();
        }

        /// <summary>
        /// 곡 선택/준비 화면을 벗어난 화면(메인 메뉴·캐릭터/엘핀 선택 등)으로 이동했을 때 호출합니다.
        /// 모니터링을 끄고, 우리가 주입한 커스텀 클립이 아직 "BGM" 소스에서 재생 중이면 멈춰서
        /// 게임이 자기 BGM을 되찾을 수 있게 합니다(커스텀 곡 미리듣기가 엉뚱한 화면까지 따라오는 문제 방지).
        /// </summary>
        public static void StopCustomMenuBgm(string reason)
        {
            currentLoadingUid = null;
            activeMenuBgmUid = null;
            isCustomMenuBgmActive = false;
            monitorGeneration++;
            if (activeMenuBgmCoroutine != null)
            {
                MelonCoroutines.Stop(activeMenuBgmCoroutine);
                activeMenuBgmCoroutine = null;
            }
            try
            {
                AudioSource src = GameBgmSource.TryGet(out _);
                if (src == null)
                {
                    GameObject bgmGo = GameObject.Find("BGM");
                    if (bgmGo != null)
                    {
                        src = bgmGo.GetComponent<AudioSource>();
                    }
                }

                if (src != null && injectedMenuClip != null && src.clip == injectedMenuClip)
                {
                    src.Stop();
                    src.clip = null;
                    ModLogger.Msg($"[MenuBGM] 커스텀 메뉴 BGM 정지 ({reason}): 우리가 주입한 클립 재생 중단");
                }

                if (injectedMenuClip != null)
                {
                    RetireMenuClip(injectedMenuClip);
                    injectedMenuClip = null;
                    DestroyUnreferencedRetiredClips($"StopCustomMenuBgm: {reason}");
                }

                // 배틀 진입이 아닌 메뉴 이동일 때만 디스코드 프로필을 In Menu 상태로 복원합니다.
                // DiscordManager 직접 참조 대신 NoInlining 격리된 DiscordPresenceManager를 거칩니다.
                if (!HywStageManager.IsInStageStatic)
                {
                    DiscordPresenceManager.SetIdleState();
                }
            }
            catch (Exception ex)
            {
                ModLogger.Error($"[MenuBGM] StopCustomMenuBgm 예외: {ex}");
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
            try
            {
                string songDir;
                if (!HwaResourceManager.TryGetSongDirectory(uid, out songDir) || string.IsNullOrEmpty(songDir))
                {
                    yield break;
                }

                // 배틀 BGM과 같은 규칙으로 고릅니다(SongAudioFiles). 미리듣기와 본 플레이가 다른 파일을 틀지 않게 합니다.
                string oggPath = SongAudioFiles.ResolveBgmOggPath(songDir);
                if (string.IsNullOrWhiteSpace(oggPath) || !File.Exists(oggPath))
                {
                    ModLogger.Warning($"[MenuBGM] ogg 파일을 찾지 못했습니다: folder={songDir}");
                    yield break;
                }

                // 게임 엔진이 기본 오디오 재생을 초기화하고 원래 오디오가 로드 및 실행을 시작할 때까지 0.4초 대기합니다(빠른 휠 스크롤 디바운스).
                yield return new WaitForSeconds(0.4f);

                // 대기 시간 도중 다른 곡으로 선택이 넘어간 경우 로딩을 중단합니다.
                if (generation != monitorGeneration || CustomPlaySession.Current.LastKnownMusicUid != uid)
                {
                    yield break;
                }

                AudioSource menuSource = FindMenuAudioSource();
                if (menuSource == null)
                {
                    ModLogger.Warning("[MenuBGM] 활성화된 BGM AudioSource를 찾지 못했습니다.");
                    yield break;
                }

                ModLogger.Verbose($"[MenuBGM] 대상 AudioSource 선택됨: GO={menuSource.gameObject.name}, Clip={menuSource.clip?.name ?? "(null)"}, Vol={menuSource.volume}, Mute={menuSource.mute}, SpatialBlend={menuSource.spatialBlend}, MixerGroup={menuSource.outputAudioMixerGroup?.name ?? "(null)"}");

                string uri = new Uri(oggPath).AbsoluteUri;
                UnityWebRequest request = new UnityWebRequest(uri, "GET");
                try
                {
                    DownloadHandlerAudioClip handler = new DownloadHandlerAudioClip(uri, AudioType.OGGVORBIS);
                    // 배틀 BGM과 같이 스트리밍으로 읽습니다. 풀 디코딩(streamAudio = false)은 GetContent가 곡 전체를
                    // PCM으로 풀며 메인 스레드를 316~450ms 막아, 휠을 돌리다 멈칫할 때마다 렉이 났습니다.
                    // 스트리밍 클립을 바로 파괴하면 크래시가 나므로 해제는 RetireGraceSeconds 뒤로 미룹니다.
                    handler.streamAudio = true;
                    request.downloadHandler = handler;
                    // 휠을 빠르게 돌릴 때의 렉이 이 로드/디코드 때문인지 재기 위한 계측입니다.
                    // load는 요청~완료까지의 벽시계 시간(여러 프레임), getContent는 메인 스레드에서 막히는 시간입니다.
                    var loadTimer = System.Diagnostics.Stopwatch.StartNew();
                    yield return request.SendWebRequest();
                    loadTimer.Stop();

                    if (!string.IsNullOrWhiteSpace(request.error))
                    {
                        ModLogger.Error($"[MenuBGM] OGG 로드 실패: {oggPath}, error={request.error}");
                        yield break;
                    }

                    // 로드가 완료되었을 시점에도 유효성 검사 (사용자가 곡을 다시 변경했거나 AudioSource가 파괴되었는지 여부)
                    if (generation != monitorGeneration || CustomPlaySession.Current.LastKnownMusicUid != uid || menuSource == null || menuSource.gameObject == null)
                    {
                        yield break;
                    }

                    AudioClip customClip = null;
                    var getContentTimer = System.Diagnostics.Stopwatch.StartNew();
                    try
                    {
                        customClip = DownloadHandlerAudioClip.GetContent(request);
                    }
                    catch (Exception ex)
                    {
                        ModLogger.Error($"[MenuBGM] AudioClip 변환 실패: {ex.Message}");
                    }

                    getContentTimer.Stop();

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
                        isCustomMenuBgmActive = true;

                        // 직전에 우리가 주입했던 클립은 즉시 파괴하지 않고 은퇴 큐에 보관한 뒤 안전하게 지연 해제합니다.
                        // FMOD가 이전 클립 재생/버퍼 정리를 끝내기 전에 즉시 Destroy하면 0xc0000005 크래시가 발생할 수 있습니다.
                        if (previousInjected != null && previousInjected != customClip)
                        {
                            RetireMenuClip(previousInjected);
                        }
                        DestroyUnreferencedRetiredClips("custom menu BGM applied");

                        // 이전 배틀에서 은퇴한 배틀 BGM 클립이 있다면 menuSource.clip 교체 직후 안전하게 정리합니다.
                        HwaBattleMediaController.DestroyUnreferencedRetiredClips("custom menu BGM applied");

                        ModLogger.Msg($"[MenuBGM] 커스텀 곡 BGM 주입 완료! uid={uid}, clip={customClip.name}, length={customClip.length}s, loadState={customClip.loadState}, loadType={customClip.loadType}, load={loadTimer.ElapsedMilliseconds}ms, getContent={getContentTimer.ElapsedMilliseconds}ms");
                        ModLogger.Verbose($"[MenuBGM] 주입 후 AudioSource 상태: isPlaying={menuSource.isPlaying}, volume={menuSource.volume} (이전: {prevVolume}), mute={menuSource.mute} (이전: {prevMute}), spatialBlend={menuSource.spatialBlend}");

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
            finally
            {
                if (generation == monitorGeneration)
                {
                    currentLoadingUid = null;
                }
            }
        }

        private static IEnumerator MonitorAudioSource(AudioSource source, string uid, string clipName, int generation)
        {
            ModLogger.Verbose($"[MenuBGM.Monitor] 모니터링 시작: GO={source.gameObject.name}, targetClip={clipName}");
            int i = 0;
            while (true)
            {
                yield return new WaitForSeconds(5.0f);
                if (generation != monitorGeneration)
                {
                    ModLogger.Verbose("[MenuBGM.Monitor] 모니터링 세대가 변경되어 종료합니다.");
                    yield break;
                }

                if (source == null)
                {
                    ModLogger.Warning("[MenuBGM.Monitor] AudioSource가 파괴되었습니다.");
                    yield break;
                }

                string currentClip = source.clip != null ? source.clip.name : "(null)";
                if (currentClip != clipName || CustomPlaySession.Current.LastKnownMusicUid != uid)
                {
                    ModLogger.Verbose($"[MenuBGM.Monitor] 대상 클립 또는 선택 곡이 변경되어 모니터링을 종료합니다. (현재 클립: {currentClip})");
                    yield break;
                }

                ModLogger.Verbose($"[MenuBGM.Monitor] T+{i*5.0f:F1}s - Playing: {source.isPlaying}, Vol: {source.volume:F4}, Mute: {source.mute}, Clip: {currentClip}, Time: {source.time:F2}");
                i++;
            }
        }
    }
}
