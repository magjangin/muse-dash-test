using MelonLoader;
using HarmonyLib;
using Il2Cpp;
using Il2CppAssets.Scripts.Database;
using Il2CppAssets.Scripts.Database.DataClass;
using Il2CppAssets.Scripts.PeroTools.Commons;
using Il2CppAssets.Scripts.PeroTools.Managers;
using System;
using System.Collections.Generic;
using System.Reflection;
using static Il2CppAssets.Scripts.Database.DBConfigCustomTags;

namespace muse_dash_test
{
    internal class CustomTagPatch
    {
        private const int TagUid = 998;
        private const string TagUidString = "tag-muse-dash-test";
        private const string AlbumUidString = "998-0";
        private const string AlbumTitle = "실험 앨범";
        private const string AlbumCoverPrefabName = "album_0";

        internal static DBConfigAlbums.AlbumsInfo CustomAlbumInfo;

        /// <summary>
        /// 런타임에 게임 데이터베이스에 우리의 "실험 모드" 커스텀 태그 카테고리를 동적으로 주입합니다.
        /// </summary>
        [HarmonyPatch(typeof(MusicTagManager), nameof(MusicTagManager.InitAlbumTagInfo))]
        internal class MusicTagPatch
        {
            private static void Postfix(MusicTagManager __instance)
            {
                try
                {
                    // 1. 태그 탭 다국어 명칭 정의
                    var languages = new Dictionary<string, string>
                    {
                        { "Korean", "실험 모드" },
                        { "English", "Experiment Mod" },
                        { "Japanese", "実験モード" },
                        { "ChineseSimplified", "实验模式" },
                        { "ChineseTraditional", "實驗模式" }
                    };

                    // IL2CPP Dictionary 구조로 변환
                    var il2CppLanguages = new Il2CppSystem.Collections.Generic.Dictionary<string, string>(languages.Count);
                    foreach (var kvp in languages)
                    {
                        il2CppLanguages.Add(kvp.Key, kvp.Value);
                    }

                    string defaultName = languages.ContainsKey("English") ? languages["English"] : "Experiment Mod";

                    // 2. AlbumTagInfo 인스턴스 생성 및 기본 정보 기입
                    var info = new AlbumTagInfo
                    {
                        name = defaultName,
                        tagUid = TagUidString,
                        iconName = "IconCustomAlbums" // CustomAlbums에서 자주 쓰이는 기본 아이콘 리소스 이름
                    };

                    // 3. 이 태그 탭 하위에 노출할 곡 UIDs 정의 (가상 복제 곡만 주입하기 위해 빈 리스트로 시작)
                    var musicList = new List<string>();

                    // 0-0 얇은 객체 복사 및 커스텀 태그 주입 실험
                    try
                    {
                        var originalInfo = GlobalDataBase.dbMusicTag?.GetMusicInfoFromAll("0-0");
                        if (originalInfo != null)
                        {
                            MelonLogger.Msg("[CustomTagPatch] === 얇은 복사 및 주입 실험 시작 ===");
                            LogMusicInfoDump("[CustomTagPatch] [원본 곡 상세 덤프] originalInfo", originalInfo);
                            
                            // 1. MemberwiseClone 복사 수행
                            var clonedObj = originalInfo.MemberwiseClone();
                            if (clonedObj != null)
                            {
                                var clonedInfo = clonedObj.TryCast<MusicInfo>();
                                if (clonedInfo != null)
                                {
                                    MelonLogger.Msg("[CustomTagPatch] [성공] originalInfo.MemberwiseClone() 및 MusicInfo 캐스팅 성공!");
                                    
                                    // 2. 복사본 프로퍼티 수정 수행
                                    clonedInfo.uid = "999-0";
                                    clonedInfo.name = "화영왕 0";
                                    clonedInfo.author = "화영왕 0";
                                    clonedInfo.levelDesigner = "화영왕 0";
                                    clonedInfo.cover = "iyaiya_cover"; // 기존 커버 재사용
                                    clonedInfo.noteJson = "iyaiya_map"; // 기존 맵 재사용
                                    clonedInfo.music = "iyaiya_music"; // 기존 음원 재사용
                                    SetMemberValue(clonedInfo, "difficulty1", 2);
                                    SetMemberValue(clonedInfo, "difficulty2", 5);
                                    SetMemberValue(clonedInfo, "difficulty3", 0);
                                    SetMemberValue(clonedInfo, "callBackDifficulty1", 2);
                                    SetMemberValue(clonedInfo, "callBackDifficulty2", 5);
                                    SetMemberValue(clonedInfo, "callBackDifficulty3", 0);
                                    SetMemberValue(clonedInfo, "callBackDifficulty4", 0);
                                    SetMemberValue(clonedInfo, "callBackDifficulty5", 0);
                                    
                                    // 2.5 Mask Value 오버라이드로 앨범 소속 변경 (앨범 2개 생성 문제 및 롤백 방지)
                                    try
                                    {
                                        clonedInfo.AddMaskValue("albumUidName", (Il2CppSystem.String)AlbumUidString);
                                        clonedInfo.AddMaskValue("albumIndex", new Il2CppSystem.Int32 { m_value = TagUid }.BoxIl2CppObject());
                                        clonedInfo.AddMaskValue("albumJsonName", (Il2CppSystem.String)"custom_album_998_0");
                                    }
                                    catch (Exception ex)
                                    {
                                        MelonLogger.Error($"[CustomTagPatch] [실패] AddMaskValue 앨범 마스크 적용 예외: {ex}");
                                    }

                                    MelonLogger.Msg($"[CustomTagPatch] [성공] 복사본 속성 수정 완료: uid='{clonedInfo.uid}', name='{clonedInfo.name}', author='{clonedInfo.author}'");
                                    LogMusicInfoDump("[CustomTagPatch] [복사본 곡 상세 덤프] clonedInfo", clonedInfo);
                                    
                                    // 3. 글로벌 DBMusicTag의 m_AllMusicInfo 맵에 등록 시도
                                    var allMusicDict = GlobalDataBase.dbMusicTag?.m_AllMusicInfo;
                                    if (allMusicDict != null)
                                    {
                                        if (!allMusicDict.ContainsKey("999-0"))
                                        {
                                            allMusicDict.Add("999-0", clonedInfo);
                                            MelonLogger.Msg("[CustomTagPatch] [성공] m_AllMusicInfo 맵에 '999-0' 신규 주입 완료!");
                                        }
                                        else
                                        {
                                            allMusicDict["999-0"] = clonedInfo;
                                            MelonLogger.Msg("[CustomTagPatch] [알림] m_AllMusicInfo에 '999-0'이 이미 존재하여 덮어썼습니다.");
                                        }
                                        
                                        // 4. 주입 검증: GetMusicInfoFromAll("999-0") 성공 여부 확인
                                        var checkInfo = GlobalDataBase.dbMusicTag.GetMusicInfoFromAll("999-0");
                                        if (checkInfo != null && checkInfo.uid == "999-0")
                                        {
                                            MelonLogger.Msg($"[CustomTagPatch] [대성공] GetMusicInfoFromAll('999-0') 검증 성공! 반환된 곡 이름: '{checkInfo.name}'");
                                            LogMusicInfoDump("[CustomTagPatch] [검증 곡 상세 덤프] checkInfo", checkInfo);
                                            
                                            // 5. 커스텀 태그 노출 목록에 "999-0"만 단독으로 포함시킵니다!
                                            musicList.Add("999-0");
                                            MelonLogger.Msg("[CustomTagPatch] [성공] 커스텀 태그 노출 목록에 '999-0' 추가 완료!");
                                        }
                                        else
                                        {
                                            MelonLogger.Error("[CustomTagPatch] [실패] '999-0' 주입 후 조회 검증에 실패했습니다.");
                                        }
                                    }
                                    else
                                    {
                                        MelonLogger.Error("[CustomTagPatch] [실패] GlobalDataBase.dbMusicTag.m_AllMusicInfo가 null입니다.");
                                    }
                                }
                                else
                                {
                                    MelonLogger.Error("[CustomTagPatch] [실패] clonedObj를 MusicInfo로 캐스팅하지 못했습니다.");
                                }
                            }
                            else
                            {
                                MelonLogger.Error("[CustomTagPatch] [실패] originalInfo.MemberwiseClone() 결과가 null입니다.");
                            }
                            
                            MelonLogger.Msg("[CustomTagPatch] =======================================");
                        }
                        else
                        {
                            MelonLogger.Warning("[CustomTagPatch] '0-0'의 MusicInfo를 GlobalDataBase에서 찾지 못했습니다.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MelonLogger.Error($"[CustomTagPatch] 얇은 복사 및 주입 실험 중 예외 발생: {ex}");
                    }

                    // IL2CPP List 구조로 변환합니다.
                    // InitCustomTagInfo 쪽에서 전달된 리스트를 직접 정리할 수 있어, 용도별로 새 리스트를 계속 만들어 씁니다.
                    var customInfoMusicList = ToIl2CppStringList(musicList);

                    // 4. CustomTagInfo 설정
                    var customInfo = new CustomTagInfo
                    {
                        tag_name = il2CppLanguages,
                        tag_picture = "https://cdn.mdmc.moe/static/melon.png", // 대표 이미지 URL
                        music_list = customInfoMusicList
                    };

                    // 5. 커스텀 태그 초기화 및 바인딩
                    info.InitCustomTagInfo(customInfo);

                    // InitCustomTagInfo가 내부 표시 목록을 다시 정리할 수 있으므로 앨범 트리는 초기화 후에 연결합니다.
                    // CustomTagInfo는 music_list만 갖고, 앨범 트리는 AlbumTagInfo 쪽 m_AlbumsInfos/m_DisplayMusicUids가 담당합니다.
                    var tagMusicList = ToIl2CppStringList(musicList);
                    var displayMusicList = ToIl2CppStringList(musicList);
                    info.SetTagUids(ToIl2CppStringList(musicList));

                    DBConfigAlbums.AlbumsInfo albumInfo = null;

                    // 1. 기존의 안전한 앨범을 복제하여 가상 앨범 생성 및 주입
                    try
                    {
                        var albumsConfig = Singleton<ConfigManager>.instance.GetConfigObject<DBConfigAlbums>();
                        if (albumsConfig != null)
                        {
                            var items = albumsConfig.m_Items;
                            if (items != null && items.Count > 0)
                            {
                                // 1.1 첫 번째 공식 앨범(튜토리얼 등)을 복제하여 PeroTools의 내부 필드 안전성 확보
                                var originalAlbum = items[0];
                                var clonedObj = originalAlbum.MemberwiseClone();
                                if (clonedObj != null)
                                {
                                    var clonedAlbum = clonedObj.TryCast<DBConfigAlbums.AlbumsInfo>();
                                    if (clonedAlbum != null)
                                    {
                                        clonedAlbum.uid = AlbumUidString;
                                        clonedAlbum.title = AlbumTitle;
                                        clonedAlbum.tag = TagUidString;
                                        clonedAlbum.jsonName = "custom_album_998_0";
                                        clonedAlbum.prefabsName = AlbumCoverPrefabName;
                                        clonedAlbum.free = true;
                                        clonedAlbum.needPurchase = false;
                                        clonedAlbum.price = "";

                                        albumInfo = clonedAlbum;
                                        CustomAlbumInfo = clonedAlbum;

                                        // 중복 추가 방지 검사 후 추가
                                        bool exists = false;
                                        for (int i = 0; i < items.Count; i++)
                                        {
                                            if (items[i].uid == AlbumUidString)
                                            {
                                                exists = true;
                                                break;
                                            }
                                        }
                                        if (!exists)
                                        {
                                            items.Add(clonedAlbum);
                                            MelonLogger.Msg("[CustomTagPatch] [성공] 얇은 복제 방식으로 DBConfigAlbums.m_Items에 가상 앨범(998-0) 주입 완료!");
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MelonLogger.Error($"[CustomTagPatch] 앨범 복제 주입 중 예외 발생: {ex}");
                    }

                    // 1.2 복제 실패에 대비한 안전 폴백
                    if (albumInfo == null)
                    {
                        albumInfo = new DBConfigAlbums.AlbumsInfo
                        {
                            uid = AlbumUidString,
                            title = AlbumTitle,
                            tag = TagUidString,
                            jsonName = "custom_album_998_0",
                            prefabsName = AlbumCoverPrefabName,
                            free = true,
                            needPurchase = false,
                            price = ""
                        };
                        CustomAlbumInfo = albumInfo;
                        MelonLogger.Warning("[CustomTagPatch] [경고] 복제에 실패하여 new AlbumsInfo 폴백을 생성했습니다.");
                    }

                    var albumInfos = new Il2CppSystem.Collections.Generic.List<DBConfigAlbums.AlbumsInfo>(1);
                    albumInfos.Add(albumInfo);
                    info.m_AlbumsInfos = albumInfos;

                    var displayAlbum = new AlbumDisplayMusic(albumInfo);
                    displayAlbum.AddRangeMusicUid(displayMusicList);

                    var displayAlbums = new Il2CppSystem.Collections.Generic.List<AlbumDisplayMusic>(1);
                    displayAlbums.Add(displayAlbum);
                    info.m_DisplayMusicUids = displayAlbums;
                    info.m_MusicUids = tagMusicList;

                    // 6. 글로벌 데이터베이스의 커스텀 태그 정렬 목록에 등록
                    if (!GlobalDataBase.dbMusicTag.AllAlbumTagsSortContains(TagUid))
                    {
                        GlobalDataBase.dbMusicTag.AddCustomAlbumTagsSort(TagUid);
                    }

                    // 7. 글로벌 데이터베이스에 태그 데이터 최종 등록
                    GlobalDataBase.dbMusicTag.AddAlbumTagData(TagUid, info);
                }
                catch (System.Exception ex)
                {
                    MelonLogger.Error($"커스텀 태그 주입 중 치명적인 예외가 발생했습니다: {ex}");
                }
            }

            private static Il2CppSystem.Collections.Generic.List<string> ToIl2CppStringList(List<string> source)
            {
                var result = new Il2CppSystem.Collections.Generic.List<string>(source.Count);
                foreach (var value in source)
                {
                    result.Add(value);
                }
                return result;
            }

            private static void LogCoverCandidates(AlbumTagInfo tagInfo, DBConfigAlbums.AlbumsInfo albumInfo, CustomTagInfo customInfo)
            {
                try
                {
                    MelonLogger.Msg($"[CoverProbe] AlbumTagInfo: tagUid={tagInfo?.tagUid ?? "(null)"}, name={tagInfo?.name ?? "(null)"}, iconName={tagInfo?.iconName ?? "(null)"}");
                    MelonLogger.Msg($"[CoverProbe] CustomTagInfo: tag_picture={customInfo?.tag_picture ?? "(null)"}");

                    if (albumInfo == null)
                    {
                        MelonLogger.Msg("[CoverProbe] AlbumsInfo: (null)");
                        return;
                    }

                    MelonLogger.Msg($"[CoverProbe] AlbumsInfo direct: uid={albumInfo.uid ?? "(null)"}, title={albumInfo.title ?? "(null)"}, tag={albumInfo.tag ?? "(null)"}, jsonName={albumInfo.jsonName ?? "(null)"}, prefabsName={albumInfo.prefabsName ?? "(null)"}");

                    var type = albumInfo.GetType();
                    foreach (var prop in type.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance))
                    {
                        if (!prop.CanRead || prop.GetIndexParameters().Length != 0)
                            continue;

                        string name = prop.Name ?? string.Empty;
                        if (!LooksCoverRelated(name))
                            continue;

                        object value = SafeRead(() => prop.GetValue(albumInfo));
                        MelonLogger.Msg($"[CoverProbe] AlbumsInfo prop {name}={FormatValue(value)}");
                    }

                    foreach (var field in type.GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance))
                    {
                        string name = field.Name ?? string.Empty;
                        if (!LooksCoverRelated(name))
                            continue;

                        object value = SafeRead(() => field.GetValue(albumInfo));
                        MelonLogger.Msg($"[CoverProbe] AlbumsInfo field {name}={FormatValue(value)}");
                    }
                }
                catch (Exception ex)
                {
                    MelonLogger.Warning($"[CoverProbe] 커버 후보 로그 예외: {ex.Message}");
                }
            }

            private static bool LooksCoverRelated(string name)
            {
                return name.IndexOf("cover", StringComparison.OrdinalIgnoreCase) >= 0 ||
                    name.IndexOf("prefab", StringComparison.OrdinalIgnoreCase) >= 0 ||
                    name.IndexOf("pic", StringComparison.OrdinalIgnoreCase) >= 0 ||
                    name.IndexOf("icon", StringComparison.OrdinalIgnoreCase) >= 0 ||
                    name.IndexOf("json", StringComparison.OrdinalIgnoreCase) >= 0;
            }

            private static object SafeRead(Func<object> read)
            {
                try
                {
                    return read();
                }
                catch (Exception ex)
                {
                    return "(error: " + ex.Message + ")";
                }
            }

            private static string FormatValue(object value)
            {
                return value == null ? "(null)" : value.ToString();
            }

            private static void LogMusicInfoDump(string label, MusicInfo info)
            {
                try
                {
                    if (info == null)
                    {
                        MelonLogger.Msg($"{label}: (null)");
                        return;
                    }

                    MelonLogger.Msg($"{label}: uid={info.uid ?? "(null)"}, name={info.name ?? "(null)"}, author={info.author ?? "(null)"}, levelDesigner={info.levelDesigner ?? "(null)"}, cover={info.cover ?? "(null)"}, noteJson={info.noteJson ?? "(null)"}, music={info.music ?? "(null)"}, albumUidName={info.albumUidName ?? "(null)"}, albumJsonName={info.albumJsonName ?? "(null)"}, albumIndex={info.albumIndex}");

                    var type = info.GetType();
                    foreach (var prop in type.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance))
                    {
                        if (!prop.CanRead || prop.GetIndexParameters().Length != 0)
                            continue;

                        object value;
                        try
                        {
                            value = prop.GetValue(info);
                        }
                        catch (Exception ex)
                        {
                            value = "(error: " + ex.Message + ")";
                        }

                        MelonLogger.Msg($"{label}: prop {prop.Name}={FormatValue(value)}");
                    }

                }
                catch (Exception ex)
                {
                    MelonLogger.Error($"{label} 덤프 예외: {ex}");
                }
            }

            private static void SetMemberValue(object target, string memberName, object value)
            {
                if (target == null)
                {
                    return;
                }

                var type = target.GetType();

                var property = type.GetProperty(memberName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if (property != null && property.CanWrite)
                {
                    property.SetValue(target, ConvertMemberValue(value, property.PropertyType));
                    return;
                }

                var field = type.GetField(memberName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if (field != null)
                {
                    field.SetValue(target, ConvertMemberValue(value, field.FieldType));
                }
            }

            private static object ConvertMemberValue(object value, Type targetType)
            {
                if (value == null)
                {
                    return null;
                }

                var underlyingType = Nullable.GetUnderlyingType(targetType) ?? targetType;
                if (underlyingType.IsInstanceOfType(value))
                {
                    return value;
                }

                if (underlyingType == typeof(string))
                {
                    return value.ToString();
                }

                if (underlyingType.IsEnum)
                {
                    return Enum.ToObject(underlyingType, value);
                }

                return Convert.ChangeType(value, underlyingType);
            }

        }

        /// <summary>
        /// 태그 화면 로딩 시 발생할 수 있는 1000개 앨범 순회 병목 렉을 해결하는 성능 최적화 패치입니다.
        /// </summary>
        [HarmonyPatch(typeof(MusicTagManager), nameof(MusicTagManager.InitDatas))]
        internal static class Fix1000AlbumsPatch
        {
            private static void Postfix()
            {
                try
                {
                    var configObject = Singleton<ConfigManager>.instance.GetConfigObject<DBConfigAlbums>();
                    if (configObject != null)
                    {
                        // 게임이 상정하는 앨범 개수 상한선을 실제 개수(count - 3)로 최적화 리사이징
                        configObject.m_MaxAlbumUid = configObject.count - 3;
                    }
                }
                catch (System.Exception ex)
                {
                    MelonLogger.Warning($"m_MaxAlbumUid 성능 최적화 패치 적용 중 예외 발생 (비치명적): {ex.Message}");
                }
            }
        }



        // 가상 앨범 "998-0"의 구매/소유/무료 상태를 게임의 앨범 데이터베이스에 우회 주입하여
        // 가상 곡 "999-0"이 항상 해금된(무료) 상태로 노출되도록 보장하는 패치입니다.
        [HarmonyPatch(typeof(DBConfigAlbums), nameof(DBConfigAlbums.GetAlbumInfoByMusicInfo))]
        internal class DBConfigAlbums_GetAlbumInfoByMusicInfo_Patch
        {
            private static bool Prefix(DBConfigAlbums __instance, MusicInfo musicInfo, ref DBConfigAlbums.AlbumsInfo __result)
            {
                if (musicInfo != null && musicInfo.uid == "999-0" && CustomAlbumInfo != null)
                {
                    __result = CustomAlbumInfo;
                    return false;
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(DBConfigAlbums), nameof(DBConfigAlbums.GetAlbumsInfoByUid))]
        internal class DBConfigAlbums_GetAlbumsInfoByUid_Patch
        {
            private static bool Prefix(DBConfigAlbums __instance, string uid, ref DBConfigAlbums.AlbumsInfo __result)
            {
                if (uid == "998-0" && CustomAlbumInfo != null)
                {
                    __result = CustomAlbumInfo;
                    return false;
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(DBConfigAlbums), nameof(DBConfigAlbums.GetAlbumIndexByUid))]
        internal class DBConfigAlbums_GetAlbumIndexByUid_Patch
        {
            private static bool Prefix(DBConfigAlbums __instance, string uid, ref int __result)
            {
                if (uid == "998-0")
                {
                    __result = 998;
                    return false;
                }
                return true;
            }
        }
    }
}
