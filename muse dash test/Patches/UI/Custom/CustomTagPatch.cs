using MelonLoader;
using HarmonyLib;
using Il2Cpp;
using Il2CppAssets.Scripts.Database;
using Il2CppAssets.Scripts.Database.DataClass;
using Il2CppAssets.Scripts.PeroTools.Commons;
using Il2CppAssets.Scripts.PeroTools.Managers;
using System;
using System.Collections.Generic;
using static Il2CppAssets.Scripts.Database.DBConfigCustomTags;

namespace muse_dash_test
{
    internal partial class CustomTagPatch
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
        internal partial class MusicTagPatch
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
                        iconName = "IconCustomAlbums" // CustomAlbums에서 자주  쓰이는 기본 아이콘 리소스 이름
                    };

                    // 3. 이 태그 탭 하위에 노출할 곡 UIDs 정의 (가상 복제 곡만 주입하기 위해 빈 리스트로 시작)
                    var musicList = new List<string>();

                    // manifest에서 지정한 원본 곡을 찾고 커스텀 태그 주입 실험
                    try
                    {
                        muse_dash_test.MainMod.TryGetCachedHwaSearchTerms(out string sourceUid, out string sourceTitle, out string sourceArtist, out string sourceDescription);
                        string lookupQuery = string.IsNullOrWhiteSpace(sourceUid) ? null : sourceUid;
                        if (string.IsNullOrWhiteSpace(lookupQuery))
                        {
                            lookupQuery = sourceTitle;
                        }
                        if (string.IsNullOrWhiteSpace(lookupQuery))
                        {
                            lookupQuery = sourceArtist;
                        }

                        var originalInfo = string.IsNullOrWhiteSpace(lookupQuery) ? null : GlobalDataBase.dbMusicTag?.GetMusicInfoFromAll(lookupQuery);
                        if (originalInfo == null && !string.IsNullOrWhiteSpace(lookupQuery))
                        {
                            PnlStagePatchHelper.TryFindMusicInfoByQuery(lookupQuery, out originalInfo, out _);
                        }

                        if (originalInfo != null)
                        {
                            MelonLogger.Msg($"[CustomTagPatch] === 얇은 복사 및  주입 실험 시작 === lookup={lookupQuery ?? "(null)"}, sourceUid={originalInfo.uid}");
                            LogMusicInfoDump("[CustomTagPatch] [원본 곡 상세 덤 프] originalInfo", originalInfo);
                            
                            // "999-0" 화영왕 0 주입
                            InjectVirtualSong(originalInfo, "999-0", "화영왕 0", "화영왕 0", "화영왕 0", "iyaiya_cover", "iyaiya_map", "iyaiya_music", 2, 5, musicList);

                            // "999-1" 화영왕 1 주입
                            InjectVirtualSong(originalInfo, "999-1", "화영왕 1", "화영왕 1", "화영왕 1", "iyaiya_cover", "iyaiya_map", "iyaiya_music", 3, 6, musicList);

                            // "999-2" 화영왕 2 주입
                            InjectVirtualSong(originalInfo, "999-2", "화영왕 2", "화영왕 2", "화영왕 2", "iyaiya_cover", "iyaiya_map", "iyaiya_music", 4, 7, musicList);

                            MelonLogger.Msg("[CustomTagPatch] =======================================");
                        }
                        else
                        {
                            MelonLogger.Warning("[CustomTagPatch] 검색된 원본 MusicInfo를 찾지 못했습니다.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MelonLogger.Error($"[CustomTagPatch] 얇은 복사 및 주입  실험 중 예외 발생: {ex}");
                    }

                    // IL2CPP List 구조로 변환합니다.
                    // InitCustomTagInfo 쪽에서 전달된 리스트를 직접 정리할 수  있어, 용도별로 새 리스트를 계속 만들어 씁니다.
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

                    // InitCustomTagInfo가 내부 표시 목록을 다시 정리할 수 있으 므로 앨범 트리는 초기화 후에 연결합니다.
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
                                            MelonLogger.Msg("[CustomTagPatch] [ 성공] 얇은 복제 방식으로 DBConfigAlbums.m_Items에 가상 앨범(998-0) 주입 완료!");
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MelonLogger.Error($"[CustomTagPatch] 앨범 복제 주입 중  예외 발생: {ex}");
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

            private static void InjectVirtualSong(MusicInfo originalInfo, string uid, string name, string author, string levelDesigner, string cover, string noteJson, string music, int diff1, int diff2, List<string> musicList)
            {
                try
                {
                    // 1. 원본 객체를 얇은 복사로 복제
                    var clonedObj = originalInfo.MemberwiseClone();
                    if (clonedObj == null)
                    {
                        MelonLogger.Error($"[CustomTagPatch] [실패] {uid} originalInfo.MemberwiseClone() 결과가 null입니다.");
                        return;
                    }

                    var clonedInfo = clonedObj.TryCast<MusicInfo>();
                    if (clonedInfo == null)
                    {
                        MelonLogger.Error($"[CustomTagPatch] [실패] {uid} clonedObj를 MusicInfo로 캐스팅하지 못했습니다.");
                        return;
                    }

                    // 2. 복사본 프로퍼티 수정 수행
                    clonedInfo.uid = uid;
                    clonedInfo.name = name;
                    clonedInfo.author = author;
                    clonedInfo.levelDesigner = levelDesigner;
                    SetMemberValue(clonedInfo, "difficulty1", diff1);
                    SetMemberValue(clonedInfo, "difficulty2", diff2);
                    SetMemberValue(clonedInfo, "difficulty3", 0);
                    SetMemberValue(clonedInfo, "callBackDifficulty1", diff1);
                    SetMemberValue(clonedInfo, "callBackDifficulty2", diff2);
                    SetMemberValue(clonedInfo, "callBackDifficulty3", 0);
                    SetMemberValue(clonedInfo, "callBackDifficulty4", 0);
                    SetMemberValue(clonedInfo, "callBackDifficulty5", 0);

                    // 2.5 Mask Value 오버라이드로 앨범 소속 변경 (앨범 2개 생성 문제 및 롤백 방지)
                    try
                    {
                        clonedInfo.AddMaskValue("albumUidName", (Il2CppSystem.String)AlbumUidString);
                        clonedInfo.AddMaskValue("albumIndex", new Il2CppSystem.Int32 { m_value = TagUid }.BoxIl2CppObject());
                        clonedInfo.AddMaskValue("albumJsonName", (Il2CppSystem.String)"custom_album_998_0");
                        SetAlbumMetadata(clonedInfo, AlbumUidString, TagUid, TagUid + 1, "custom_album_998_0");
                    }
                    catch (Exception ex)
                    {
                        MelonLogger.Error($"[CustomTagPatch] [실패] {uid} AddMaskValue 앨범 마스크 적용 예외: {ex}");
                    }

                    LogNestedMusicExInfo("[CustomTagPatch] [복제 후 MusicExInfo 덤프]", clonedInfo);

                    // 3. 글로벌 DBMusicTag의 m_AllMusicInfo 맵에 등록 시도
                    var allMusicDict = GlobalDataBase.dbMusicTag?.m_AllMusicInfo;
                    if (allMusicDict != null)
                    {
                        if (!allMusicDict.ContainsKey(uid))
                        {
                            allMusicDict.Add(uid, clonedInfo);
                            MelonLogger.Msg($"[CustomTagPatch] [성공] m_AllMusicInfo 맵에 '{uid}' 신규 주입 완료!");
                        }
                        else
                        {
                            allMusicDict[uid] = clonedInfo;
                            MelonLogger.Msg($"[CustomTagPatch] [알림] m_AllMusicInfo에 '{uid}'이 이미 존재하여 덮어썼습니다.");
                        }

                        // 4. 주입 검증: GetMusicInfoFromAll(uid) 성공 여부 확인
                        var checkInfo = GlobalDataBase.dbMusicTag.GetMusicInfoFromAll(uid);
                        if (checkInfo != null && checkInfo.uid == uid)
                        {
                            MelonLogger.Msg($"[CustomTagPatch] [대성공] GetMusicInfoFromAll('{uid}') 검증 성공! 반환된 곡 이름: '{checkInfo.name}'");
                            
                            // 5. 커스텀 태그 노출 목록에 추가
                            musicList.Add(uid);
                            MelonLogger.Msg($"[CustomTagPatch] [성공] 커스텀 태그 노출 목록에 '{uid}' 추가 완료!");
                        }
                        else
                        {
                            MelonLogger.Error($"[CustomTagPatch] [실패] '{uid}' 주입 후 조회 검증에 실패했습니다.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MelonLogger.Error($"[CustomTagPatch] {uid} 주입 중 예외 발생: {ex}");
                }
            }
        }
    }
}
