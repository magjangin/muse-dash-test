using HarmonyLib;
using MelonLoader;
using System;
using UnityEngine;

namespace muse_dash_test
{
    [HarmonyPatch(typeof(AudioSource), nameof(AudioSource.clip), MethodType.Setter)]
    public class AudioSource_SetClip_Patch
    {
        private static Il2CppAssets.Scripts.UI.Panels.PnlStage cachedPnlStage;
        private static Il2Cpp.PnlPreparation cachedPnlPreparation;
        private static int lastCheckFrame = -1;
        private static bool lastCheckResult = false;
        private static int lastScanFrame = -1;

        /// <summary>패널 씬 스캔(<c>FindObjectOfType</c>) 사이에 둘 최소 프레임 수.</summary>
        private const int ScanIntervalFrames = 30;

        // 게임 BGM 소스의 네이티브 포인터. 프레임당 한 번만 다시 얻습니다(IsGameBgmSource 참고).
        private static IntPtr gameBgmPointer = IntPtr.Zero;
        private static int gameBgmPointerFrame = -1;

        /// <summary>
        /// 이 소스가 게임이 원곡을 트는 BGM 소스인지 확인합니다.
        ///
        /// <para>게임이 들고 있는 참조(<see cref="GameBgmSource"/>)와 포인터를 비교합니다. 이 세터는 모든
        /// <c>AudioSource</c>가 공유하므로, 예전처럼 호출마다 <c>gameObject.name == "BGM"</c>을 보면 Prefix·Postfix에서
        /// 한 번씩 이름 문자열이 마샬링됩니다. 이름으로 고르는 방식은 게임이 비슷한 이름의 오브젝트를 추가하면
        /// 엉뚱한 쪽이 걸리는 문제도 있습니다(CHECKLIST "이름 부분 일치로 고르지 않았는가").
        /// 클립을 주입하는 쪽(<c>FindMenuAudioSource</c>)이 같은 참조를 먼저 쓰므로 기준도 일치합니다.</para>
        ///
        /// <para>참조를 못 얻을 때만(초기화 전, 게임 업데이트로 멤버가 사라진 경우) 종전의 이름 비교로 돌아갑니다.</para>
        /// </summary>
        private static bool IsGameBgmSource(AudioSource source)
        {
            if (source == null) return false;

            int frame = Time.frameCount;
            if (frame != gameBgmPointerFrame)
            {
                gameBgmPointerFrame = frame;
                AudioSource gameBgm = GameBgmSource.TryGet(out _);
                gameBgmPointer = gameBgm != null ? gameBgm.Pointer : IntPtr.Zero;
            }

            if (gameBgmPointer != IntPtr.Zero)
            {
                return source.Pointer == gameBgmPointer;
            }

            return source.gameObject != null && source.gameObject.name == "BGM";
        }

        public static bool Prefix(AudioSource __instance, ref AudioClip value)
        {
            try
            {
                if (IsGameBgmSource(__instance))
                {
                    string selectedUid = CustomPlaySession.Current.LastKnownMusicUid;
                    string clipNameForLog = value != null ? value.name : "(null)";

                    // MD는 단일 씬(UISystem_PC) 구조라 씬 이름으로는 "곡 선택/준비 화면을 벗어났는지"를 구분할 수 없다.
                    // 대신 곡 선택(PnlStage)/준비(PnlPreparation) 패널이 실제로 활성 상태인지를 직접 확인한다.
                    bool inStageSelectionContext = IsStageSelectionContextActive();
                    ModLogger.Verbose($"[MenuBGM.Patch.Debug] set_clip 호출: selectedUid={selectedUid ?? "(null)"}, requestedClip={clipNameForLog}, inStageSelectionContext={inStageSelectionContext}");

                    if (!inStageSelectionContext)
                    {
                        // 곡 선택/준비 화면을 벗어나면 활성 상태인 커스텀 메뉴 BGM 재생만 정지하고 모니터링을 종료하여 게임이 자기 BGM을 회복하도록 합니다.
                        // 단, 가상 곡 로딩이 방금 시작된 도중(CurrentLoadingUid 활성)에는 패널 활성화 전환 지연일 수 있으므로 취소하지 않습니다.
                        if (HwaMenuBgmController.IsCustomMenuBgmActive && string.IsNullOrEmpty(HwaMenuBgmController.CurrentLoadingUid))
                        {
                            ModLogger.Msg($"[MenuBGM.Patch] 곡 선택 화면을 벗어남을 감지, 커스텀 BGM 정지 및 모니터링 종료: selectedUid={CustomPlaySession.Current.SelectedMusicUid}");
                            HwaMenuBgmController.StopCustomMenuBgm("곡 선택/준비 화면 이탈 감지");
                            HwaMenuBgmController.StopMenuMonitoring("곡 선택/준비 화면 이탈 감지");
                        }
                    }

                    // 클립 대입 차단은 곡 선택/준비 화면 안이거나 가상 곡 로딩 중일 때 적용합니다.
                    bool isVirtualLoading = !string.IsNullOrEmpty(HwaMenuBgmController.CurrentLoadingUid) && CustomContentIds.IsVirtualSong(HwaMenuBgmController.CurrentLoadingUid);
                    if ((inStageSelectionContext || isVirtualLoading) && CustomContentIds.IsVirtualSong(selectedUid))
                    {
                        bool isCustomClip = IsCustomOggClip(value);
                        if (!isCustomClip)
                        {
                            // [중요] 비커스텀 클립 대입 시 __instance.Stop()을 절대 호출하면 안 됩니다!
                            // __instance는 현재 커스텀 BGM이 돌고 있는 AudioSource이므로, Stop()을 부르면
                            // 재생 중이던 커스텀 BGM이 꺼져버립니다. 클립 대입만 차단(return false)하고
                            // 현재 커스텀 클립이 계속 재생되도록 보장합니다.
                            ModLogger.Msg($"[MenuBGM.Patch] 가상 곡 활성화 중 허용되지 않은 클립 대입 차단! (selectedUid={selectedUid}, 요청 클립: {clipNameForLog})");
                            if (__instance != null && __instance.clip != null && !__instance.isPlaying)
                            {
                                __instance.Play();
                            }
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ModLogger.Error($"[MenuBGM.Patch] set_clip 패치 에러: {ex}");
            }
            return true;
        }

        public static void Postfix(AudioSource __instance)
        {
            try
            {
                if (IsGameBgmSource(__instance))
                {
                    HwaBattleMediaController.DestroyUnreferencedRetiredClips("BGM.set_clip Postfix");
                }
            }
            catch (Exception ex)
            {
                ModLogger.Verbose($"[MenuBGM.Patch] set_clip Postfix 무시된 예외: {ex.Message}");
            }
        }

        private static bool IsCustomOggClip(AudioClip clip)
        {
            if (clip == null || string.IsNullOrEmpty(clip.name))
            {
                return false;
            }

            return clip.name.EndsWith(".ogg", StringComparison.OrdinalIgnoreCase);
        }

        public static void ResetCache()
        {
            cachedPnlStage = null;
            cachedPnlPreparation = null;
            lastCheckFrame = -1;
            lastCheckResult = false;
            lastScanFrame = -1;
            gameBgmPointer = IntPtr.Zero;
            gameBgmPointerFrame = -1;
        }

        private static bool IsAlive<T>(T comp) where T : UnityEngine.Component
        {
            if (comp == null) return false;
            try
            {
                if (comp.Pointer == IntPtr.Zero) return false;
                var go = comp.gameObject;
                return go != null && go.Pointer != IntPtr.Zero;
            }
            catch
            {
                return false;
            }
        }

        private static bool IsAliveAndActive<T>(T comp) where T : UnityEngine.Component
        {
            if (!IsAlive(comp)) return false;
            try
            {
                return comp.gameObject.activeInHierarchy;
            }
            catch
            {
                return false;
            }
        }

        private static T FindSceneComponent<T>() where T : UnityEngine.Component
        {
            try
            {
                T comp = UnityEngine.Object.FindObjectOfType<T>();
                if (comp != null) return comp;

                // 비활성 상태인 오브젝트도 검색 (PnlPreparation 등이 비활성화 상태로 씬에 상주할 때)
                var all = Resources.FindObjectsOfTypeAll<T>();
                if (all != null)
                {
                    for (int i = 0; i < all.Length; i++)
                    {
                        var candidate = all[i];
                        if (candidate != null && candidate.gameObject != null && candidate.gameObject.scene.isLoaded)
                        {
                            return candidate;
                        }
                    }
                }
            }
            catch
            {
            }
            return null;
        }

        /// <summary>
        /// 곡 선택(PnlStage) 또는 곡 준비(PnlPreparation) 패널이 현재 실제로 활성 상태인지 확인합니다.
        /// MD가 단일 씬 안에서 패널만 켜고 끄는 구조이므로, 이게 "곡 선택 컨텍스트에 있는지"의 유일하게 신뢰 가능한 신호입니다.
        /// </summary>
        internal static bool IsStageSelectionContextActive()
        {
            try
            {
                int currentFrame = Time.frameCount;
                if (currentFrame == lastCheckFrame)
                {
                    return lastCheckResult;
                }

                // 씬 언로드로 파괴된 이전 C# 래퍼 참조는 null로 정리
                if (cachedPnlStage != null && !IsAlive(cachedPnlStage))
                {
                    cachedPnlStage = null;
                }
                if (cachedPnlPreparation != null && !IsAlive(cachedPnlPreparation))
                {
                    cachedPnlPreparation = null;
                }

                // 기존 캐시가 살아있으면 activeInHierarchy 확인
                bool stageActive = IsAliveAndActive(cachedPnlStage);
                bool prepActive = IsAliveAndActive(cachedPnlPreparation);

                if (stageActive || prepActive)
                {
                    lastCheckFrame = currentFrame;
                    lastCheckResult = true;
                    return true;
                }

                // 둘 다 비활성이거나 파괴되었을 때 스캔
                // 단, 직전 프레임까지 활성이었다가 방금 꺼진 경우(PnlStage ↔ PnlPreparation 전환 순간)에는
                // 30프레임 스로틀링을 즉시 우회하여 대상 패널 활성화를 즉시 감지합니다.
                bool justDeactivated = lastCheckResult;
                bool scanAllowed = justDeactivated || lastScanFrame < 0 || currentFrame - lastScanFrame >= ScanIntervalFrames;
                if (scanAllowed)
                {
                    lastScanFrame = currentFrame;

                    if (cachedPnlStage == null)
                    {
                        cachedPnlStage = FindSceneComponent<Il2CppAssets.Scripts.UI.Panels.PnlStage>();
                    }

                    if (cachedPnlPreparation == null)
                    {
                        cachedPnlPreparation = FindSceneComponent<Il2Cpp.PnlPreparation>();
                    }

                    if (IsAliveAndActive(cachedPnlStage) || IsAliveAndActive(cachedPnlPreparation))
                    {
                        lastCheckFrame = currentFrame;
                        lastCheckResult = true;
                        return true;
                    }
                }

                lastCheckFrame = currentFrame;
                lastCheckResult = false;
                return false;
            }
            catch (Exception ex)
            {
                ModLogger.Error($"[MenuBGM.Patch] IsStageSelectionContextActive 예외: {ex}");
            }
            return false;
        }
    }
}
