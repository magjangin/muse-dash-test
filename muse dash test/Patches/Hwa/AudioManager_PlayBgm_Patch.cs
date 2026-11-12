using HarmonyLib;
using MelonLoader;
using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using Il2CppAssets.Scripts.PeroTools.Managers;

namespace muse_dash_test
{
    /// <summary>
    /// 가상 곡(커스텀 곡) 선택 상태에서 곡 선택 화면(PnlStage) 및 준비 화면(PnlPreparation)에 머무는 동안,
    /// 게임 내부 로직(캐릭터 전용 BGM, 공식 데모 BGM 등)이 커스텀 BGM을 임의로 중단하거나
    /// 다른 클립으로 교체하지 못하도록 AudioManager의 BGM 제어 메서드를 방어합니다.
    /// </summary>
    [HarmonyPatch]
    public static class AudioManager_PlayBgm_Patch
    {
        private static IEnumerable<MethodBase> TargetMethods()
        {
            var methods = new List<MethodBase>();
            try
            {
                var type = typeof(AudioManager);
                foreach (var m in type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
                {
                    if (m.Name == "PlayBGM" || m.Name == "StopBGM")
                    {
                        methods.Add(m);
                    }
                }
            }
            catch (Exception ex)
            {
                ModLogger.Error($"[MenuBGM.Patch] TargetMethods 탐색 중 예외: {ex}");
            }
            return methods;
        }

        [HarmonyPrefix]
        public static bool Prefix(MethodBase __originalMethod, object[] __args)
        {
            try
            {
                string selectedUid = CustomPlaySession.Current.LastKnownMusicUid;
                if (string.IsNullOrEmpty(selectedUid) || !CustomContentIds.IsVirtualSong(selectedUid))
                {
                    return true;
                }

                bool inStageSelectionContext = AudioSource_SetClip_Patch.IsStageSelectionContextActive();
                bool isVirtualLoading = !string.IsNullOrEmpty(HwaMenuBgmController.CurrentLoadingUid)
                    && CustomContentIds.IsVirtualSong(HwaMenuBgmController.CurrentLoadingUid);

                if (inStageSelectionContext || isVirtualLoading)
                {
                    string methodName = __originalMethod != null ? __originalMethod.Name : "(unknown)";
                    string argInfo = (__args != null && __args.Length > 0 && __args[0] != null) 
                        ? __args[0].ToString() 
                        : "none";

                    ModLogger.Verbose($"[MenuBGM.Patch] 가상 곡 활성화 중 AudioManager.{methodName} 차단! (selectedUid={selectedUid}, arg={argInfo})");

                    EnsureCustomBgmPlaying();
                    return false;
                }
            }
            catch (Exception ex)
            {
                ModLogger.Error($"[MenuBGM.Patch] AudioManager.{__originalMethod?.Name} Prefix 예외: {ex}");
            }
            return true;
        }

        private static void EnsureCustomBgmPlaying()
        {
            try
            {
                AudioSource src = GameBgmSource.TryGet(out _);
                if (src != null && src.clip != null)
                {
                    if (src.volume < 0.05f)
                    {
                        src.volume = 1.0f;
                    }
                    if (!src.isPlaying)
                    {
                        src.Play();
                    }
                }
            }
            catch
            {
            }
        }
    }
}
