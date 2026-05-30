using MelonLoader;
using Il2CppFormulaBase;
using Il2CppGameLogic;
using System.Reflection;
using UnityEngine;
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

