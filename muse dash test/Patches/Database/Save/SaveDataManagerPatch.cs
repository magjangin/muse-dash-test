using HarmonyLib;
using MelonLoader;
using Il2CppAssets.Scripts.PeroTools.Nice.Datas;
using Il2CppAssets.Scripts.PeroTools.Nice.Interface;
using Il2CppSystem.Collections.Generic;

namespace muse_dash_test
{
    /// <summary>
    /// 세이브 파일 오염 방지를 위해 가상 앨범/곡 기록(1000-x, 999-x)이 디스크에 물리적으로 저장되기 직전
    /// DataManager.Save() 시점에 인메모리 DB에서 모든 가상 데이터(필드, 최고점수 리스트, 최근플레이 리스트 등)를
    /// 정밀하게 제거하여 원본 세이브 파일의 순수성을 보장하는 하모니 패치 클래스입니다.
    /// </summary>
    [HarmonyPatch(typeof(DataManager), nameof(DataManager.Save))]
    public static class SaveDataManagerPatch
    {
        [HarmonyPrefix]
        public static void Prefix(DataManager __instance)
        {
            try
            {
                if (__instance == null) return;

                MelonLogger.Msg("[SaveDataManagerPatch] DataManager.Save() 호출 감지 - 오염 방지를 위한 정밀 클렌징을 개시합니다.");

                var datas = __instance.datas;
                if (datas == null)
                {
                    MelonLogger.Warning("[SaveDataManagerPatch] DataManager.datas가 null입니다.");
                    return;
                }

                int totalRemovedFields = 0;

                // ==========================================
                // 1. 제네릭 컬렉션 필드 클렌징 (Account, Task, IAP, StageAchievement 등)
                // ==========================================
                foreach (var k in datas.Keys)
                {
                    var subData = datas[k];
                    if (subData != null)
                    {
                        var fields = subData.fields;
                        if (fields != null)
                        {
                            var keysToRemove = new System.Collections.Generic.List<string>();
                            foreach (var key in fields.Keys)
                            {
                                if (key != null && (key.StartsWith("1000-") || key.StartsWith("999-")))
                                {
                                    keysToRemove.Add(key);
                                }
                            }

                            if (keysToRemove.Count > 0)
                            {
                                MelonLogger.Msg($"[SaveDataManagerPatch] -> '{k}' 컬렉션에서 가상 필드 {keysToRemove.Count}개 제거 중: {string.Join(", ", keysToRemove)}");
                                foreach (var key in keysToRemove)
                                {
                                    fields.Remove(key);
                                }
                                totalRemovedFields += keysToRemove.Count;
                            }
                        }
                    }
                }

                // ==========================================
                // 2. Achievement 컬렉션 내부 리스트 및 다차원 데이터 클렌징 (최고점수, 최근 플레이, 패스 기록 등)
                // ==========================================
                if (datas.ContainsKey("Achievement"))
                {
                    var achievementData = datas["Achievement"];
                    if (achievementData != null)
                    {
                        var fields = achievementData.fields;
                        if (fields != null)
                        {
                            // 2-1. highest 최고 기록 리스트 클렌징
                            if (fields.ContainsKey("highest"))
                            {
                                var highestVar = fields["highest"];
                                if (highestVar != null)
                                {
                                    var highestList = VariableUtils.GetResult<Il2CppSystem.Collections.Generic.List<IData>>(highestVar);
                                    if (highestList != null)
                                    {
                                        int highestRemoved = CleanIDataList(highestList);
                                        if (highestRemoved > 0)
                                        {
                                            MelonLogger.Msg($"[SaveDataManagerPatch] -> Achievement.highest 리스트에서 가상 곡 플레이 결과 {highestRemoved}개 클렌징 완료.");
                                        }
                                    }
                                }
                            }

                            // 2-2. recentPassLevelData 최근 플레이 리스트 클렌징
                            if (fields.ContainsKey("recentPassLevelData"))
                            {
                                var recentVar = fields["recentPassLevelData"];
                                if (recentVar != null)
                                {
                                    var recentList = VariableUtils.GetResult<Il2CppSystem.Collections.Generic.List<IData>>(recentVar);
                                    if (recentList != null)
                                    {
                                        int recentRemoved = CleanIDataList(recentList);
                                        if (recentRemoved > 0)
                                        {
                                            MelonLogger.Msg($"[SaveDataManagerPatch] -> Achievement.recentPassLevelData 리스트에서 가상 곡 기록 {recentRemoved}개 클렌징 완료.");
                                        }
                                    }
                                }
                            }

                            // 2-3. easy_pass, hard_pass, master_pass 난이도별 클리어 곡 리스트 클렌징
                            string[] passKeys = { "easy_pass", "hard_pass", "master_pass" };
                            foreach (var pk in passKeys)
                            {
                                if (fields.ContainsKey(pk))
                                {
                                    var passVar = fields[pk];
                                    if (passVar != null)
                                    {
                                        var passList = VariableUtils.GetResult<Il2CppSystem.Collections.Generic.List<string>>(passVar);
                                        if (passList != null)
                                        {
                                            int passRemoved = CleanStringList(passList);
                                            if (passRemoved > 0)
                                            {
                                                MelonLogger.Msg($"[SaveDataManagerPatch] -> Achievement.{pk} 리스트에서 가상 클리어 {passRemoved}개 클렌징 완료.");
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                MelonLogger.Msg("[SaveDataManagerPatch] 세이브 데이터 정밀 정화 완료. 깨끗한 상태로 안전하게 저장을 계속 진행합니다.");
            }
            catch (System.Exception ex)
            {
                MelonLogger.Error($"[SaveDataManagerPatch] Save Prefix 정화 중 예외 발생: {ex}");
            }
        }

        /// <summary>
        /// IData 리스트 내부를 순회하며 가상 곡 관련 결과를 제거합니다.
        /// </summary>
        private static int CleanIDataList(Il2CppSystem.Collections.Generic.List<IData> list)
        {
            if (list == null) return 0;
            int removedCount = 0;

            for (int i = list.Count - 1; i >= 0; i--)
            {
                var item = list[i];
                if (item == null) continue;

                // 1. SavedSongResult 형변환 시도
                var songResult = item.TryCast<Il2Cpp.SavedSongResult>();
                if (songResult != null)
                {
                    string uid = songResult.uid;
                    if (uid != null && (uid.StartsWith("1000-") || uid.StartsWith("999-")))
                    {
                        list.RemoveAt(i);
                        removedCount++;
                        continue;
                    }
                }

                // 2. 제네릭 IData fields 기반 Fallback 대조
                var fields = item.fields;
                if (fields != null && fields.ContainsKey("uid"))
                {
                    var uidVar = fields["uid"];
                    if (uidVar != null)
                    {
                        var valObj = VariableUtils.GetResult<Il2CppSystem.Object>(uidVar);
                        if (valObj != null)
                        {
                            string uid = valObj.ToString();
                            if (uid != null && (uid.StartsWith("1000-") || uid.StartsWith("999-")))
                            {
                                list.RemoveAt(i);
                                removedCount++;
                            }
                        }
                    }
                }
            }
            return removedCount;
        }

        /// <summary>
        /// string 리스트 내부를 순회하며 가상 곡 UIDs를 제거합니다.
        /// </summary>
        private static int CleanStringList(Il2CppSystem.Collections.Generic.List<string> list)
        {
            if (list == null) return 0;
            int removedCount = 0;

            for (int i = list.Count - 1; i >= 0; i--)
            {
                string val = list[i];
                if (val != null && (val.StartsWith("1000-") || val.StartsWith("999-")))
                {
                    list.RemoveAt(i);
                    removedCount++;
                }
            }
            return removedCount;
        }
    }
}
