using MelonLoader;
using Il2CppFormulaBase;
using Il2CppGameLogic;
using System.Reflection;

// Il2CppFormulaBase.StageBattleComponent.LoadMusicData 하모니 패치
[HarmonyLib.HarmonyPatch(typeof(StageBattleComponent), "LoadMusicData")]
public class StageBattleComponent_LoadMusicData_Patch
{
    public static void Postfix(StageBattleComponent __instance)
    {
        MelonLogger.Msg($"StageBattleComponent.LoadMusicData 호출됨: {__instance}");
        DumpStageBattleComponentProperties(__instance);
    }

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
        MelonLogger.Msg($"StageBattleComponent.InitData 호출됨: {__instance}");
    }
}
