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
    internal class CustomTagPatch
    {
        private const int TagUid = 998;
        private const string TagUidString = "tag-muse-dash-test";
        private const string AlbumUidString = "998-0";
        private const string AlbumTitle = "실험 앨범";
        private const string AlbumCoverPrefabName = "album_0";

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

                    // 3. 이 태그 탭 하위에 노출할 곡 UIDs 정의 (튜토리얼 곡 딱 하나만 기본 노출로 원복)
                    var musicList = new List<string> { "0-0" };

                    // 0-0의 메타데이터 출력 추가
                    try
                    {
                        var musicInfo = GlobalDataBase.dbMusicTag?.GetMusicInfoFromAll("0-0");
                        if (musicInfo != null)
                        {
                            MelonLogger.Msg("[CustomTagPatch] === Metadata of '0-0' ===");
                            MelonLogger.Msg($"[CustomTagPatch] uid: {musicInfo.uid}");
                            MelonLogger.Msg($"[CustomTagPatch] name: {musicInfo.name}");
                            MelonLogger.Msg($"[CustomTagPatch] author: {musicInfo.author}");
                            MelonLogger.Msg($"[CustomTagPatch] bpm: {musicInfo.bpm}");
                            MelonLogger.Msg($"[CustomTagPatch] music: {musicInfo.music}");
                            MelonLogger.Msg($"[CustomTagPatch] demo: {musicInfo.demo}");
                            MelonLogger.Msg($"[CustomTagPatch] cover: {musicInfo.cover}");
                            MelonLogger.Msg($"[CustomTagPatch] hexieCover: {musicInfo.hexieCover}");
                            MelonLogger.Msg($"[CustomTagPatch] noteJson: {musicInfo.noteJson}");
                            MelonLogger.Msg($"[CustomTagPatch] scene: {musicInfo.scene}");
                            MelonLogger.Msg($"[CustomTagPatch] levelDesigner: {musicInfo.levelDesigner}");
                            MelonLogger.Msg($"[CustomTagPatch] levelDesigner1: {musicInfo.levelDesigner1}");
                            MelonLogger.Msg($"[CustomTagPatch] levelDesigner2: {musicInfo.levelDesigner2}");
                            MelonLogger.Msg($"[CustomTagPatch] levelDesigner3: {musicInfo.levelDesigner3}");
                            MelonLogger.Msg($"[CustomTagPatch] levelDesigner4: {musicInfo.levelDesigner4}");
                            MelonLogger.Msg($"[CustomTagPatch] levelDesigner5: {musicInfo.levelDesigner5}");
                            MelonLogger.Msg($"[CustomTagPatch] difficulty1: {musicInfo.difficulty1}");
                            MelonLogger.Msg($"[CustomTagPatch] difficulty2: {musicInfo.difficulty2}");
                            MelonLogger.Msg($"[CustomTagPatch] difficulty3: {musicInfo.difficulty3}");
                            MelonLogger.Msg($"[CustomTagPatch] difficulty4: {musicInfo.difficulty4}");
                            MelonLogger.Msg($"[CustomTagPatch] difficulty5: {musicInfo.difficulty5}");
                            MelonLogger.Msg("[CustomTagPatch] ==============================");
                        }
                        else
                        {
                            MelonLogger.Warning("[CustomTagPatch] '0-0'의 MusicInfo를 GlobalDataBase에서 찾지 못했습니다.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MelonLogger.Error($"[CustomTagPatch] '0-0' 메타데이터 출력 예외: {ex}");
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

                    var albumInfo = new DBConfigAlbums.AlbumsInfo
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
    }
}
