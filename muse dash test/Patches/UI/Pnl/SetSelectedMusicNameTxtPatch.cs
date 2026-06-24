using HarmonyLib;
using MelonLoader;
using System;
using UnityEngine.UI;

namespace muse_dash_test
{
    internal static class SetSelectedMusicNameTxtPatchHelper
    {
        public static void Apply(Il2Cpp.SetSelectedMusicNameTxt instance, string source)
        {
            try
            {
                if (instance == null) return;

                // 현재 화면의 곡 uid를 신뢰도 순으로 해석합니다.
                //   1) 라이브 선택(PnlStage 실시간)
                //   2) 세션 선택(곡 내비게이션마다 RefreshDiffUI에서 갱신)
                //   3) 마지막 클릭(셀 클릭 때만 갱신 → 스크롤 중엔 직전 곡으로 고착되는 stale 값)
                // 기존 로직은 1·2가 순정곡이면 3(stale)으로 되돌아가, 커스텀곡을 본 직후
                // 순정곡 준비화면에 직전 커스텀 제목이 잔상으로 남는 버그가 있었습니다.
                string uid = PnlStagePatchHelper.GetCurrentSelectedMusicUid();
                if (string.IsNullOrEmpty(uid)) uid = CustomPlaySession.Current.SelectedMusicUid;
                if (string.IsNullOrEmpty(uid)) uid = CustomPlaySession.Current.LastClickedMusicUid;

                if (!CustomContentIds.IsVirtualSong(uid)) return;
                if (!MainMod.TryGetHwaPrimarySong(
                        uid,
                        out string title,
                        out string artist,
                        out _, out _, out _, out _, out _, out _, out _))
                {
                    return;
                }

                string value = null;
                if (instance.isMusicName && !string.IsNullOrWhiteSpace(title))
                {
                    value = title;
                }
                else if (instance.isMusicAuthor && !string.IsNullOrWhiteSpace(artist))
                {
                    value = artist;
                }

                if (string.IsNullOrWhiteSpace(value)) return;

                int writes = 0;
                if (instance.txt != null)
                {
                    instance.txt.text = value;
                    writes++;
                }

                // 긴 제목 컨트롤러는 simple/backup 텍스트를 별도로 보유합니다.
                // 해당 컨트롤러가 활성화되는 화면에서도 같은 값으로 맞춥니다.
                if (instance.m_LongCtrl != null && instance.m_LongCtrl.gameObject != null)
                {
                    var texts = instance.m_LongCtrl.gameObject.GetComponentsInChildren<Text>(true);
                    if (texts != null)
                    {
                        foreach (var text in texts)
                        {
                            if (text == null || text.text == value) continue;
                            text.text = value;
                            writes++;
                        }
                    }
                }

                MelonLogger.Msg($"[SetSelectedMusicNameTxt.{source}] uid={uid}, value={value}, writes={writes}");
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[SetSelectedMusicNameTxt.{source}] 커스텀 곡 정보 적용 예외: {ex}");
            }
        }
    }

    [HarmonyPatch(typeof(Il2Cpp.SetSelectedMusicNameTxt), GameBindings.SetSelectedMusicNameTxt.Awake)]
    internal static class SetSelectedMusicNameTxt_Awake_Patch
    {
        private static void Postfix(Il2Cpp.SetSelectedMusicNameTxt __instance)
        {
            SetSelectedMusicNameTxtPatchHelper.Apply(__instance, "Awake");
        }
    }

    [HarmonyPatch(typeof(Il2Cpp.SetSelectedMusicNameTxt), GameBindings.SetSelectedMusicNameTxt.OnEnable)]
    internal static class SetSelectedMusicNameTxt_OnEnable_Patch
    {
        private static void Postfix(Il2Cpp.SetSelectedMusicNameTxt __instance)
        {
            SetSelectedMusicNameTxtPatchHelper.Apply(__instance, "OnEnable");
        }
    }
}
