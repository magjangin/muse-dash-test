using MelonLoader;
using HarmonyLib;
using Il2Cpp;
using Il2CppAssets.Scripts.Database;
using Il2CppAssets.Scripts.Database.DataClass;
using Il2CppAssets.Scripts.PeroTools.Commons;
using Il2CppAssets.Scripts.PeroTools.Managers;
using System.Collections.Generic;
using static Il2CppAssets.Scripts.Database.DBConfigCustomTags;

namespace muse_dash_test
{
    internal class CustomTagPatch
    {
        private const int TagUid = 998;
        private const string TagUidString = "tag-muse-dash-test";
        private const string AlbumUidString = "998-0";
        private const string AlbumTitle = "실험 앨범";

        /// <summary>
        /// 런타임에 게임 데이터베이스에 우리의 "실험 모드" 커스텀 태그 카테고리를 동적으로 주입합니다.
        /// </summary>
        [HarmonyPatch(typeof(MusicTagManager), nameof(MusicTagManager.InitAlbumTagInfo))]
        internal class MusicTagPatch
        {
            private static void Postfix(MusicTagManager __instance)
            {
                MelonLogger.Msg("MusicTagManager.InitAlbumTagInfo Postfix: 커스텀 태그 주입을 시작합니다.");

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

                    // 3. 이 태그 탭 하위에 노출할 곡 UIDs 정의 (튜토리얼 곡 딱 하나만 기본 노출로 원복)
                    var musicList = new List<string> { "0-0" };

                    // IL2CPP List 구조로 변환합니다.
                    // InitCustomTagInfo 쪽에서 전달된 리스트를 직접 정리할 수 있어, 용도별로 새 리스트를 계속 만들어 씁니다.
                    var customInfoMusicList = ToIl2CppStringList(musicList);

                    // 3.5. 원래 있던 기본 탑재 곡들의 목록을 상세히 로그로 출력
                    var allMusicInfo = GlobalDataBase.dbMusicTag?.m_AllMusicInfo;
                    if (allMusicInfo != null)
                    {
                        MelonLogger.Msg($"[기본 탑재 곡 목록 조회 - 총 {allMusicInfo.Count}개]");
                        bool checkedOne = false;
                        foreach (var key in allMusicInfo.Keys)
                        {
                            if (key != null && !key.StartsWith("999-"))
                            {
                                var songInfo = GlobalDataBase.dbMusicTag.GetMusicInfoFromAll(key);
                                string songName = songInfo?.name ?? "(이름 없음)";
                                string authorName = songInfo?.author ?? "(아티스트 없음)";
                                MelonLogger.Msg($"  - [기본 곡] UID: {key} | 제목: {songName} | 아티스트: {authorName}");

                                // 단 한 곡에 대해서만 데이터베이스상 제목, 아티스트 및 난이도 속성의 직접 수정(쓰기) 가능 여부를 검증
                                if (!checkedOne && songInfo != null)
                                {
                                    checkedOne = true;
                                    var type = songInfo.GetType();
                                    
                                    var nameProp = type.GetProperty("name", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                                    var authorProp = type.GetProperty("author", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                                    
                                    bool canWriteName = nameProp != null && nameProp.CanWrite;
                                    bool canWriteAuthor = authorProp != null && authorProp.CanWrite;

                                    var nameField = type.GetField("name", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                                    var authorField = type.GetField("author", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                                    if (nameField != null) canWriteName = !nameField.IsInitOnly;
                                    if (authorField != null) canWriteAuthor = !authorField.IsInitOnly;

                                    MelonLogger.Msg($"[글로벌 DB 속성 수정 가능 여부 검사 - 대상 곡 UID: {key}]");
                                    MelonLogger.Msg($"  - 제목(name) 속성 존재: Prop={nameProp != null}, Field={nameField != null} | 직접 수정(쓰기) 가능: {canWriteName}");
                                    MelonLogger.Msg($"  - 아티스트(author) 속성 존재: Prop={authorProp != null}, Field={authorField != null} | 직접 수정(쓰기) 가능: {canWriteAuthor}");

                                    // 난이도(Difficulty / Level / Grade) 관련 필드 및 프로퍼티 동적 스캔
                                    MelonLogger.Msg("  - [난이도 관련 속성 동적 검색 스캔 시작]");
                                    var properties = type.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                                    foreach (var prop in properties)
                                    {
                                        string propNameLower = prop.Name.ToLowerInvariant();
                                        if (propNameLower.Contains("diff") || propNameLower.Contains("level") || propNameLower.Contains("grade"))
                                        {
                                            MelonLogger.Msg($"    - [난이도 Property] 이름: {prop.Name} | 타입: {prop.PropertyType.Name} | 수정(쓰기) 가능: {prop.CanWrite}");
                                        }
                                    }

                                    var fields = type.GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                                    foreach (var field in fields)
                                    {
                                        string fieldNameLower = field.Name.ToLowerInvariant();
                                        if (fieldNameLower.Contains("diff") || fieldNameLower.Contains("level") || fieldNameLower.Contains("grade"))
                                        {
                                            MelonLogger.Msg($"    - [난이도 Field] 이름: {field.Name} | 타입: {field.FieldType.Name} | 수정(쓰기) 가능: {!field.IsInitOnly}");
                                        }
                                    }
                                    MelonLogger.Msg("  - [난이도 관련 속성 동적 검색 스캔 완료]");
                                }
                            }
                        }
                        MelonLogger.Msg("[기본 탑재 곡 목록 조회 완료]");
                    }

                    // 4. CustomTagInfo 설정
                    var customInfo = new CustomTagInfo
                    {
                        tag_name = il2CppLanguages,
                        tag_picture = "https://cdn.mdmc.moe/static/melon.png", // 대표 이미지 URL
                        music_list = customInfoMusicList
                    };

                    // 5. 커스텀 태그 초기화 및 바인딩
                    info.InitCustomTagInfo(customInfo);
                    MelonLogger.Msg($"[커스텀 태그 진단] InitCustomTagInfo 이후 customInfo.music_list.Count={customInfo.music_list?.Count ?? -1}");

                    // InitCustomTagInfo가 내부 표시 목록을 다시 정리할 수 있으므로 앨범 트리는 초기화 후에 연결합니다.
                    // CustomTagInfo는 music_list만 갖고, 앨범 트리는 AlbumTagInfo 쪽 m_AlbumsInfos/m_DisplayMusicUids가 담당합니다.
                    var tagMusicList = ToIl2CppStringList(musicList);
                    var displayMusicList = ToIl2CppStringList(musicList);

                    var albumInfo = new DBConfigAlbums.AlbumsInfo
                    {
                        uid = AlbumUidString,
                        title = AlbumTitle,
                        tag = TagUidString,
                        jsonName = "custom_album_998_0",
                        prefabsName = "",
                        free = true,
                        needPurchase = false,
                        price = ""
                    };

                    var albumInfos = new Il2CppSystem.Collections.Generic.List<DBConfigAlbums.AlbumsInfo>(1);
                    albumInfos.Add(albumInfo);
                    info.m_AlbumsInfos = albumInfos;

                    var displayAlbum = new AlbumDisplayMusic(albumInfo);
                    displayAlbum.AddRangeMusicUid(displayMusicList);

                    var displayAlbums = new Il2CppSystem.Collections.Generic.List<AlbumDisplayMusic>(1);
                    displayAlbums.Add(displayAlbum);
                    info.m_DisplayMusicUids = displayAlbums;
                    info.m_MusicUids = tagMusicList;
                    info.SetTagUids(ToIl2CppStringList(musicList));

                    // 6. 글로벌 데이터베이스의 커스텀 태그 정렬 목록에 등록
                    if (!GlobalDataBase.dbMusicTag.AllAlbumTagsSortContains(TagUid))
                    {
                        GlobalDataBase.dbMusicTag.AddCustomAlbumTagsSort(TagUid);
                        MelonLogger.Msg($"AddCustomAlbumTagsSort로 커스텀 태그 UID({TagUid}) 등록 완료");
                    }
                    else
                    {
                        MelonLogger.Msg($"커스텀 태그 UID({TagUid})는 이미 태그 정렬 목록에 있습니다.");
                    }

                    // 7. 글로벌 데이터베이스에 태그 데이터 최종 등록
                    GlobalDataBase.dbMusicTag.AddAlbumTagData(TagUid, info);
                    MelonLogger.Msg($"글로벌 데이터베이스에 커스텀 태그/앨범 데이터 등록 완료! AlbumUid={AlbumUidString}, AlbumTitle={AlbumTitle}, MusicCount={musicList.Count}");

                    LogCustomTagRegistrationState(__instance, info, ToIl2CppStringList(musicList));
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

            private static void LogCustomTagRegistrationState(MusicTagManager manager, AlbumTagInfo info, Il2CppSystem.Collections.Generic.List<string> musicUids)
            {
                try
                {
                    var db = GlobalDataBase.dbMusicTag;
                    if (db == null)
                    {
                        MelonLogger.Warning("[커스텀 태그 진단] dbMusicTag가 null입니다.");
                        return;
                    }

                    MelonLogger.Msg($"[커스텀 태그 진단] AllAlbumTagsSortContains({TagUid})={db.AllAlbumTagsSortContains(TagUid)}, customTagsCount={db.customTagsCount}");
                    MelonLogger.Msg($"[커스텀 태그 진단] m_AlbumTagsSort.Count={db.m_AlbumTagsSort?.Count ?? -1}, m_CustomAlbumTagsSort.Count={db.m_CustomAlbumTagsSort?.Count ?? -1}, m_AllAlbumTagData.Count={db.m_AllAlbumTagData?.Count ?? -1}");
                    LogIntList("[커스텀 태그 진단] m_AlbumTagsSort", db.m_AlbumTagsSort, TagUid);
                    LogIntList("[커스텀 태그 진단] m_CustomAlbumTagsSort", db.m_CustomAlbumTagsSort, TagUid);

                    var allSort = new Il2CppSystem.Collections.Generic.List<int>();
                    db.GetAllTagsIndexSort(allSort);
                    LogIntList("[커스텀 태그 진단] GetAllTagsIndexSort", allSort, TagUid);

                    var registered = db.GetAlbumTagInfo(TagUid);
                    if (registered == null)
                    {
                        MelonLogger.Warning($"[커스텀 태그 진단] GetAlbumTagInfo({TagUid}) 결과가 null입니다.");
                    }
                    else
                    {
                        LogAlbumTagInfo("[커스텀 태그 진단] registered", registered);
                    }

                    LogAlbumTagInfo("[커스텀 태그 진단] local", info);

                    if (manager != null)
                    {
                        bool refreshByTag = manager.RefreshStageDisplayMusics(TagUid);
                        bool refreshByList = manager.RefreshStageDisplayMusics(musicUids, TagUid, true);
                        MelonLogger.Msg($"[커스텀 태그 진단] RefreshStageDisplayMusics(tag)={refreshByTag}, RefreshStageDisplayMusics(list,tag,true)={refreshByList}, stageShowMusicCount={db.stageShowMusicCount}");
                        LogStringList("[커스텀 태그 진단] stageShowMusicList", db.stageShowMusicList, 12);
                    }
                    else
                    {
                        MelonLogger.Warning("[커스텀 태그 진단] MusicTagManager __instance가 null이라 RefreshStageDisplayMusics 검증을 건너뜁니다.");
                    }
                }
                catch (System.Exception ex)
                {
                    MelonLogger.Error($"[커스텀 태그 진단] 예외: {ex}");
                }
            }

            private static void LogAlbumTagInfo(string label, AlbumTagInfo tag)
            {
                try
                {
                    var displayAlbums = new Il2CppSystem.Collections.Generic.List<AlbumDisplayMusic>();
                    tag.GetDisplayAlbums(displayAlbums, true);

                    var musicUids = new Il2CppSystem.Collections.Generic.List<string>();
                    tag.GetMusicUids(musicUids, true);

                    MelonLogger.Msg($"{label}: tagIndex={tag.tagIndex}, tagUid={tag.tagUid}, tagName={tag.tagName}, isCustomTag={tag.isCustomTag}, musicUids={musicUids.Count}, albumsInfos={tag.albumsInfos?.Count ?? -1}, displayAlbums={displayAlbums.Count}, m_DisplayMusicUids={tag.m_DisplayMusicUids?.Count ?? -1}, m_MusicUids={tag.m_MusicUids?.Count ?? -1}");
                    LogStringList($"{label}.GetMusicUids", musicUids, 12);

                    for (int i = 0; i < displayAlbums.Count; i++)
                    {
                        var album = displayAlbums[i];
                        if (album == null) continue;
                        MelonLogger.Msg($"{label}.DisplayAlbum[{i}]: title={album.displayTitle}, count={album.count}, albumInfo.title={album.albumInfo?.title}, albumInfo.uid={album.albumInfo?.uid}, albumInfo.tag={album.albumInfo?.tag}");
                    }
                }
                catch (System.Exception ex)
                {
                    MelonLogger.Error($"{label} 로그 예외: {ex}");
                }
            }

            private static void LogIntList(string label, Il2CppSystem.Collections.Generic.List<int> list, int target)
            {
                if (list == null)
                {
                    MelonLogger.Msg($"{label}: null");
                    return;
                }

                int found = -1;
                var sb = new System.Text.StringBuilder();
                int max = System.Math.Min(list.Count, 40);
                for (int i = 0; i < max; i++)
                {
                    int value = list[i];
                    if (value == target) found = i;
                    if (i > 0) sb.Append(", ");
                    sb.Append(value);
                }
                if (list.Count > max) sb.Append(", ...");
                MelonLogger.Msg($"{label}: Count={list.Count}, targetIndex={found}, Values=[{sb}]");
            }

            private static void LogStringList(string label, Il2CppSystem.Collections.Generic.List<string> list, int max)
            {
                if (list == null)
                {
                    MelonLogger.Msg($"{label}: null");
                    return;
                }

                var sb = new System.Text.StringBuilder();
                int count = System.Math.Min(list.Count, max);
                for (int i = 0; i < count; i++)
                {
                    if (i > 0) sb.Append(", ");
                    sb.Append(list[i]);
                }
                if (list.Count > count) sb.Append(", ...");
                MelonLogger.Msg($"{label}: Count={list.Count}, Values=[{sb}]");
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
                        MelonLogger.Msg($"MusicTagManager.InitDatas Postfix: m_MaxAlbumUid 성능 최적화 패치 완료 (최대 Uid: {configObject.count - 3})");
                    }
                }
                catch (System.Exception ex)
                {
                    MelonLogger.Warning($"m_MaxAlbumUid 성능 최적화 패치 적용 중 예외 발생 (비치명적): {ex.Message}");
                }
            }
        }
    }
}
