using HarmonyLib;
using Il2CppAssets.Scripts.Database;
using Il2CppPeroPeroGames.GlobalDefines;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace muse_dash_test
{
    /// <summary>
    /// 콜라보(IP) DLC의 종료일이 코드/데이터상에 존재하는지 확인하기 위한 진단 패치.
    ///
    /// DBConfigDlcUIExtension이 게임 시작 시 내장 JSON을 Deserialize해서
    /// List&lt;DlcUIExtensionInfo&gt;를 채우는데, 그 안의 DlcUIExtensionInfo.dlcEndTime
    /// (DateTime)이 콜라보 종료 시각이다(jsonIndex로 각 콜라보/DLC 항목과 매칭).
    ///
    /// DlcUIExtensionInfo 자체에는 jsonIndex라는 숫자만 있고 이름이 없는데,
    /// Il2CppPeroPeroGames.GlobalDefines.AlbumJsonIndexDefine이라는 struct가
    /// "cytus", "djmax", "ark_nights" 같은 이름의 static int 필드로 그 숫자값들을
    /// 정의해두고 있다. 리플렉션으로 그 static 프로퍼티들을 전부 읽어서
    /// jsonIndex -> 이름 역매핑을 만든 뒤, 로그에 이름까지 같이 찍는다.
    ///
    /// Deserialize 완료 직후(Postfix) 전체 목록을 한 번 로그로 덤프한다.
    /// </summary>
    [HarmonyPatch]
    public static class CollabEndTimeDumpPatch
    {
        private static Dictionary<int, string> BuildJsonIndexNameMap()
        {
            var map = new Dictionary<int, string>();
            try
            {
                var props = typeof(AlbumJsonIndexDefine).GetProperties(BindingFlags.Public | BindingFlags.Static);
                foreach (var prop in props)
                {
                    if (prop.PropertyType != typeof(int))
                    {
                        continue;
                    }

                    try
                    {
                        int value = (int)prop.GetValue(null);
                        if (!map.ContainsKey(value))
                        {
                            map[value] = prop.Name;
                        }
                    }
                    catch (Exception ex)
                    {
                        ModLogger.Error($"[CollabEndTime] AlbumJsonIndexDefine.{prop.Name} 읽기 실패: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                ModLogger.Error($"[CollabEndTime] AlbumJsonIndexDefine 리플렉션 실패: {ex.Message}");
            }

            return map;
        }

        [HarmonyPatch(typeof(DBConfigDlcUIExtension), nameof(DBConfigDlcUIExtension.Deserialize))]
        [HarmonyPostfix]
        public static void PostfixDeserialize(DBConfigDlcUIExtension __instance)
        {
            try
            {
                var list = __instance.list;
                if (list == null)
                {
                    ModLogger.Msg("[CollabEndTime] DBConfigDlcUIExtension.list가 null입니다.");
                    return;
                }

                var nameMap = BuildJsonIndexNameMap();

                ModLogger.Msg($"[CollabEndTime] DlcUIExtensionInfo {list.Count}건 로드됨:");
                for (int i = 0; i < list.Count; i++)
                {
                    var info = list[i];
                    if (info == null)
                    {
                        continue;
                    }

                    string name = nameMap.TryGetValue(info.jsonIndex, out string n) ? n : "(이름 매칭 안 됨)";
                    ModLogger.Msg($"[CollabEndTime]   jsonIndex={info.jsonIndex} ({name}), dlcEndTime={info.dlcEndTime:yyyy-MM-dd HH:mm:ss}");
                }

                // AlbumJsonIndexDefine에 정의된 "전체" 이름 목록 기준으로 역조회.
                // djmax처럼 위 목록(DlcUIExtensionInfo)엔 없던 콜라보도 여기서 다 훑어서
                // 실제로 UI 확장 정보(=종료일 데이터)가 있는지 없는지 전부 보여준다.
                ModLogger.Msg($"[CollabEndTime] AlbumJsonIndexDefine 전체 {nameMap.Count}건 역조회:");
                foreach (var pair in nameMap.OrderBy(p => p.Value))
                {
                    int jsonIndex = pair.Key;
                    string collabName = pair.Value;
                    var info = __instance.GetDlcUIExtensionByJsonIndex(jsonIndex);
                    if (info == null)
                    {
                        ModLogger.Msg($"[CollabEndTime]   jsonIndex={jsonIndex} ({collabName}) -> DlcUIExtensionInfo 없음(UI 확장 정보 미등록)");
                    }
                    else
                    {
                        ModLogger.Msg($"[CollabEndTime]   jsonIndex={jsonIndex} ({collabName}) -> dlcEndTime={info.dlcEndTime:yyyy-MM-dd HH:mm:ss}");
                    }
                }
            }
            catch (Exception ex)
            {
                ModLogger.Error($"[CollabEndTime] PostfixDeserialize 예외: {ex.Message}");
            }
        }
    }
}
