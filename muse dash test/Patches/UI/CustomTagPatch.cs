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

        /// <summary>
        /// 런타임에 게임 데이터베이스에 우리의 "실험 모드" 커스텀 태그 카테고리를 동적으로 주입합니다.
        /// </summary>
        [HarmonyPatch(typeof(MusicTagManager), nameof(MusicTagManager.InitAlbumTagInfo))]
        internal class MusicTagPatch
        {
            private static void Postfix()
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

                    // 3. 이 태그 탭 하위에 노출할 곡 UIDs 정의 (예: 튜토리얼 "0-0"을 테스트용으로 추가)
                    var musicList = new List<string> { "0-0" };

                    // IL2CPP List 구조로 변환
                    var il2CppMusicList = new Il2CppSystem.Collections.Generic.List<string>(musicList.Count);
                    foreach (var uid in musicList)
                    {
                        il2CppMusicList.Add(uid);
                    }

                    // 4. CustomTagInfo 설정
                    var customInfo = new CustomTagInfo
                    {
                        tag_name = il2CppLanguages,
                        tag_picture = "https://cdn.mdmc.moe/static/melon.png", // 대표 이미지 URL
                        music_list = il2CppMusicList
                    };

                    // 5. 커스텀 태그 초기화 및 바인딩
                    info.InitCustomTagInfo(customInfo);

                    // 6. 글로벌 데이터베이스의 앨범 정렬 순서 목록(m_AlbumTagsSort)에 태그 추가
                    var tagsSort = GlobalDataBase.dbMusicTag.m_AlbumTagsSort;
                    if (tagsSort != null)
                    {
                        // 렉 현상 등을 최소화하고 하단 특정 위치(끝에서 4번째 부근)에 안전하게 추가
                        int insertIndex = System.Math.Max(0, tagsSort.Count - 4);
                        tagsSort.Insert(insertIndex, TagUid);
                        MelonLogger.Msg($"m_AlbumTagsSort에 커스텀 태그 UID({TagUid}) 삽입 완료 (인덱스: {insertIndex})");
                    }

                    // 7. 글로벌 데이터베이스에 태그 데이터 최종 등록
                    GlobalDataBase.dbMusicTag.AddAlbumTagData(TagUid, info);
                    MelonLogger.Msg("글로벌 데이터베이스에 커스텀 태그 데이터 등록 완료!");
                }
                catch (System.Exception ex)
                {
                    MelonLogger.Error($"커스텀 태그 주입 중 치명적인 예외가 발생했습니다: {ex}");
                }
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
