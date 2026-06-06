using MelonLoader;
using Il2Cpp;
using Il2CppAssets.Scripts.Database;
using Il2CppAssets.Scripts.Database.DataClass;
using Il2CppAssets.Scripts.PeroTools.Commons;
using Il2CppAssets.Scripts.PeroTools.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using UnityEngine;

using static Il2CppAssets.Scripts.Database.DBConfigCustomTags;

namespace muse_dash_test
{
    /// <summary>
    /// 게임 내부의 커스텀 태그(실험 모드 카테고리) 및 가상 곡, 가상 앨범 주입을 전담하는 도메인 매니저 레지스트리 서비스 클래스입니다.
    /// </summary>
    public static class CustomTagRegistry
    {
        public const int TagUid = 9998;
        public const string TagUidString = "tag-muse-dash-test";
        public const string AlbumUidString = "9998-0";
        public const string AlbumTitle = "실험 앨범";
        public const string AlbumCoverPrefabName = "album_0";

        public static DBConfigAlbums.AlbumsInfo CustomAlbumInfo { get; private set; }

        /// <summary>
        /// MusicTagManager가 앨범 태그 정보를 초기화할 때 진입하여 커스텀 태그 및 가상 앨범/가상 곡 정보를 일괄 등록합니다.
        /// </summary>
        public static void RegisterAll(MusicTagManager musicTagManager)
        {
            if (musicTagManager == null) return;

            try
            {
                // 1. 태그 탭 다국어 명칭 정의 및 인스턴스 생성
                var il2CppLanguages = CreateTagLanguages(out string defaultName);
                var info = new AlbumTagInfo
                {
                    name = defaultName,
                    tagUid = TagUidString,
                    iconName = "IconCustomAlbums"
                };

                // 2. 가상 곡 생성 및 주입
                var musicList = BuildAndInjectVirtualSongs();
                var customInfoMusicList = ToIl2CppStringList(musicList);

                // 3. CustomTagInfo 설정
                var customInfo = new CustomTagInfo
                {
                    tag_name = il2CppLanguages,
                    tag_picture = System.IO.Path.Combine(MelonLoader.Utils.MelonEnvironment.GameRootDirectory, "hwa tag image", "tag_icon.png"),
                    music_list = customInfoMusicList
                };

                info.InitCustomTagInfo(customInfo);

                var tagMusicList = ToIl2CppStringList(musicList);
                var displayMusicList = ToIl2CppStringList(musicList);
                info.SetTagUids(ToIl2CppStringList(musicList));

                // 4. 가상 앨범 생성 및 주입
                var albumInfo = CreateAndInjectAlbumInfo();

                // 5. 앨범 정보 맵에 바인딩
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
            catch (Exception ex)
            {
                MelonLogger.Error($"[CustomTagRegistry] 커스텀 태그 주입 중 치명적인 예외가 발생했습니다: {ex}");
            }
        }

        /// <summary>
        /// 커스텀 태그의 다국어 번역 언어 사전을 생성합니다.
        /// </summary>
        private static Il2CppSystem.Collections.Generic.Dictionary<string, string> CreateTagLanguages(out string defaultName)
        {
            var languages = new Dictionary<string, string>
            {
                { "Korean", "실험 모드" },
                { "English", "Experiment Mod" },
                { "Japanese", "実験モード" },
                { "ChineseSimplified", "实验模式" },
                { "ChineseTraditional", "實驗模式" }
            };

            var il2CppLanguages = new Il2CppSystem.Collections.Generic.Dictionary<string, string>(languages.Count);
            foreach (var kvp in languages)
            {
                il2CppLanguages.Add(kvp.Key, kvp.Value);
            }

            defaultName = languages.ContainsKey("English") ? languages["English"] : "Experiment Mod";
            return il2CppLanguages;
        }

        /// <summary>
        /// 외부 매니페스트 설정 정보를 바탕으로 가상 곡들을 클로닝하여 데이터베이스에 등록하고 등록된 Uids 리스트를 반환합니다.
        /// </summary>
        private static List<string> BuildAndInjectVirtualSongs()
        {
            var musicList = new List<string>();

            try
            {
                var vUids = MainMod.GetVirtualUids();
                MelonLogger.Msg($"[CustomTagRegistry] 가상 곡 생성 시작: count={vUids.Count}");

                foreach (var uid in vUids)
                {
                    MainMod.TryGetCachedHwaSearchTerms(uid, out string sourceUid, out string sourceTitle, out string sourceArtist, out string sourceAlbum, out string sourceDescription);
                    
                    string lookupQuery = string.IsNullOrWhiteSpace(sourceUid) ? null : sourceUid;
                    if (string.IsNullOrWhiteSpace(lookupQuery))
                    {
                        lookupQuery = sourceTitle;
                    }
                    if (string.IsNullOrWhiteSpace(lookupQuery))
                    {
                        lookupQuery = sourceArtist;
                    }

                    MusicInfo originalInfo = null;
                    if (!string.IsNullOrWhiteSpace(lookupQuery))
                    {
                        originalInfo = GlobalDataBase.dbMusicTag?.GetMusicInfoFromAll(lookupQuery);
                        if (originalInfo == null)
                        {
                            PnlStagePatchHelper.TryFindMusicInfoByQuery(lookupQuery, sourceAlbum, out originalInfo, out _);
                        }
                    }
                    
                    if (originalInfo == null && !string.IsNullOrWhiteSpace(sourceAlbum))
                    {
                        PnlStagePatchHelper.TryFindMusicInfoByQuery("", sourceAlbum, out originalInfo, out _);
                    }

                    if (originalInfo == null)
                    {
                        originalInfo = GlobalDataBase.dbMusicTag?.GetMusicInfoFromAll("0-0");
                        MelonLogger.Warning($"[CustomTagRegistry] [{uid}] 원본 곡을 찾지 못하여 기본 곡(0-0)으로 폴백합니다. query={lookupQuery ?? "(null)"}, album={sourceAlbum ?? "(null)"}");
                    }

                    if (originalInfo != null)
                    {
                        string primaryName = $"화영왕 {uid.Substring(4)}";
                        string primaryAuthor = $"화영왕 {uid.Substring(4)}";
                        string primaryLevelDesigner = $"화영왕 {uid.Substring(4)}";
                        int primaryDiff1 = 2;
                        int primaryDiff2 = 5;
                        int primaryDiff3 = 0;
                        int primaryDiff4 = 0;
                        int primaryDiff5 = 0;

                        if (MainMod.TryGetCachedHwaPrimaryVirtualSong(uid, out string manifestTitle, out string manifestArtist, out string manifestLevelDesigner, out int manifestDiff1, out int manifestDiff2, out int manifestDiff3, out int manifestDiff4, out int manifestDiff5, out _))
                        {
                            if (!string.IsNullOrWhiteSpace(manifestTitle))
                            {
                                primaryName = manifestTitle;
                            }
                            if (!string.IsNullOrWhiteSpace(manifestArtist))
                            {
                                primaryAuthor = manifestArtist;
                            }
                            if (!string.IsNullOrWhiteSpace(manifestLevelDesigner))
                            {
                                primaryLevelDesigner = manifestLevelDesigner;
                            }
                            primaryDiff1 = manifestDiff1;
                            primaryDiff2 = manifestDiff2;
                            primaryDiff3 = manifestDiff3;
                            primaryDiff4 = manifestDiff4;
                            primaryDiff5 = manifestDiff5;
                        }

                        MelonLogger.Msg($"[CustomTagRegistry] === [{uid}] 주입 시도 === sourceUid={originalInfo.uid}, title={primaryName}, artist={primaryAuthor}, diff={primaryDiff1}/{primaryDiff2}");
                        InjectVirtualSong(originalInfo, uid, primaryName, primaryAuthor, primaryLevelDesigner, "iyaiya_cover", "iyaiya_map", "iyaiya_music", primaryDiff1, primaryDiff2, primaryDiff3, primaryDiff4, primaryDiff5, musicList);
                    }
                }
                MelonLogger.Msg($"[CustomTagRegistry] 가상 곡 생성 완료: count={musicList.Count}");
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[CustomTagRegistry] 가상 곡 생성/주입 중 예외 발생: {ex}");
            }

            return musicList;
        }

        /// <summary>
        /// 기존 앨범을 복제하거나 폴백 객체를 생성하여 DBConfigAlbums의 m_Items에 안전하게 주입하고 앨범 정보를 반환합니다.
        /// </summary>
        private static DBConfigAlbums.AlbumsInfo CreateAndInjectAlbumInfo()
        {
            DBConfigAlbums.AlbumsInfo albumInfo = null;

            try
            {
                var albumsConfig = Singleton<ConfigManager>.instance.GetConfigObject<DBConfigAlbums>();
                if (albumsConfig != null)
                {
                    var items = albumsConfig.m_Items;
                    if (items != null && items.Count > 0)
                    {
                        var originalAlbum = items[0];
                        var clonedObj = originalAlbum.MemberwiseClone();
                        if (clonedObj != null)
                        {
                            var clonedAlbum = clonedObj.TryCast<DBConfigAlbums.AlbumsInfo>();
                            if (clonedAlbum != null)
                            {
                                var albumWrapper = new AlbumsInfoWrapper(clonedAlbum);
                                
                                albumWrapper.uid = AlbumUidString;
                                albumWrapper.title = AlbumTitle;
                                albumWrapper.tag = TagUidString;
                                albumWrapper.jsonName = "custom_album_9998_0";
                                albumWrapper.prefabsName = AlbumCoverPrefabName;
                                
                                // 앨범 본체 및 상세 정보의 DLC/구매 플래그들 동적 세정
                                CleanPurchaseProperties(clonedAlbum);
                                var albumExInfo = ModReflection.GetValue(clonedAlbum, "m_AlbumExInfo");
                                if (albumExInfo != null)
                                {
                                    CleanPurchaseProperties(albumExInfo);
                                }

                                albumInfo = clonedAlbum;
                                CustomAlbumInfo = clonedAlbum;

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
                                    MelonLogger.Msg("[CustomTagRegistry] [성공] 얇은 복제 방식으로 DBConfigAlbums.m_Items에 가상 앨범(9998-0) 주입 완료!");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[CustomTagRegistry] 앨범 복제 주입 중 예외 발생: {ex}");
            }

            // 복제 실패 시 폴백
            if (albumInfo == null)
            {
                var fallbackAlbum = new DBConfigAlbums.AlbumsInfo();
                var fallbackWrapper = new AlbumsInfoWrapper(fallbackAlbum);
                fallbackWrapper.uid = AlbumUidString;
                fallbackWrapper.title = AlbumTitle;
                fallbackWrapper.tag = TagUidString;
                fallbackWrapper.jsonName = "custom_album_9998_0";
                fallbackWrapper.prefabsName = AlbumCoverPrefabName;
                
                CleanPurchaseProperties(fallbackAlbum);

                albumInfo = fallbackAlbum;
                CustomAlbumInfo = fallbackAlbum;
                MelonLogger.Warning("[CustomTagRegistry] [경고] 복제에 실패하여 new AlbumsInfo 폴백을 생성했습니다.");
            }

            return albumInfo;
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
            InjectVirtualSong(originalInfo, uid, name, author, levelDesigner, cover, noteJson, music, diff1, diff2, 0, 0, 0, musicList);
        }

        private static void InjectVirtualSong(MusicInfo originalInfo, string uid, string name, string author, string levelDesigner, string cover, string noteJson, string music, int diff1, int diff2, int diff3, int diff4, int diff5, List<string> musicList)
        {
            try
            {
                if (!TryCloneMusicInfo(originalInfo, uid, out MusicInfo clonedInfo))
                {
                    return;
                }

                ApplyVirtualSongMetadata(clonedInfo, uid, name, author, levelDesigner, diff1, diff2, diff3, diff4, diff5);

                // 가상 곡 본체 및 상세 정보의 DLC/구매 플래그들 동적 세정
                CleanPurchaseProperties(clonedInfo);
                var musicExInfo = ModReflection.GetValue(clonedInfo, "m_MusicExInfo");
                if (musicExInfo != null)
                {
                    CleanPurchaseProperties(musicExInfo);
                }

                if (!ApplyVirtualSongAlbumMetadata(clonedInfo, uid))
                {
                    return;
                }

                LogNestedMusicExInfo("[CustomTagRegistry] [복제 후 MusicExInfo 덤프]", clonedInfo);
                RegisterVirtualSong(clonedInfo, uid, musicList);
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[CustomTagRegistry] {uid} 주입 중 예외 발생: {ex}");
            }
        }

        private static bool TryCloneMusicInfo(MusicInfo originalInfo, string uid, out MusicInfo clonedInfo)
        {
            clonedInfo = null;

            var clonedObj = originalInfo?.MemberwiseClone();
            if (clonedObj == null)
            {
                MelonLogger.Error($"[CustomTagRegistry] [실패] {uid} originalInfo.MemberwiseClone() 결과가 null입니다.");
                return false;
            }

            clonedInfo = clonedObj.TryCast<MusicInfo>();
            if (clonedInfo == null)
            {
                MelonLogger.Error($"[CustomTagRegistry] [실패] {uid} clonedObj를 MusicInfo로 캐스팅하지 못했습니다.");
                return false;
            }

            return true;
        }

        private static void ApplyVirtualSongMetadata(MusicInfo clonedInfo, string uid, string name, string author, string levelDesigner, int diff1, int diff2, int diff3, int diff4, int diff5)
        {
            if (clonedInfo == null) return;

            var wrapper = new MusicInfoWrapper(clonedInfo);
            wrapper.uid = uid;
            wrapper.name = name;
            wrapper.author = author;
            wrapper.levelDesigner = levelDesigner;
            wrapper.difficulty1 = diff1;
            wrapper.difficulty2 = diff2;
            wrapper.difficulty3 = diff3;
            wrapper.difficulty4 = diff4;
            wrapper.difficulty5 = diff5;

            ModReflection.SetValue(clonedInfo, "callBackDifficulty1", diff1, silent: true);
            ModReflection.SetValue(clonedInfo, "callBackDifficulty2", diff2, silent: true);
            ModReflection.SetValue(clonedInfo, "callBackDifficulty3", diff3, silent: true);
            ModReflection.SetValue(clonedInfo, "callBackDifficulty4", diff4, silent: true);
            ModReflection.SetValue(clonedInfo, "callBackDifficulty5", diff5, silent: true);
        }

        private static bool ApplyVirtualSongAlbumMetadata(MusicInfo clonedInfo, string uid)
        {
            try
            {
                var wrapper = new MusicInfoWrapper(clonedInfo);
                wrapper.AddMaskValue("albumUidName", (Il2CppSystem.String)AlbumUidString);
                wrapper.AddMaskValue("albumIndex", new Il2CppSystem.Int32 { m_value = TagUid }.BoxIl2CppObject());
                wrapper.AddMaskValue("albumJsonName", (Il2CppSystem.String)"custom_album_9998_0");
                SetAlbumMetadata(clonedInfo, AlbumUidString, TagUid, TagUid + 1, "custom_album_9998_0");
                return true;
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[CustomTagRegistry] [실패] {uid} AddMaskValue 앨범 마스크 적용 예외: {ex}");
                return false;
            }
        }

        private static void RegisterVirtualSong(MusicInfo clonedInfo, string uid, List<string> musicList)
        {
            var allMusicDict = GlobalDataBase.dbMusicTag?.m_AllMusicInfo;
            if (allMusicDict == null) return;

            if (!allMusicDict.ContainsKey(uid))
            {
                allMusicDict.Add(uid, clonedInfo);
                MelonLogger.Msg($"[CustomTagRegistry] [성공] m_AllMusicInfo 맵에 '{uid}' 신규 주입 완료!");
            }
            else
            {
                allMusicDict[uid] = clonedInfo;
                MelonLogger.Msg($"[CustomTagRegistry] [알림] m_AllMusicInfo에 '{uid}'이 이미 존재하여 덮어썼습니다.");
            }

            var checkInfo = GlobalDataBase.dbMusicTag.GetMusicInfoFromAll(uid);
            if (checkInfo != null && checkInfo.uid == uid)
            {
                MelonLogger.Msg($"[CustomTagRegistry] [대성공] GetMusicInfoFromAll('{uid}') 검증 성공! 반환된 곡 이름: '{checkInfo.name}'");
                musicList.Add(uid);
                MelonLogger.Msg($"[CustomTagRegistry] [성공] 커스텀 태그 노출 목록에 '{uid}' 추가 완료!");
            }
            else
            {
                MelonLogger.Error($"[CustomTagRegistry] [실패] '{uid}' 주입 후 조회 검증에 실패했습니다.");
            }
        }

        private static void SetAlbumMetadata(MusicInfo info, string albumUidString, int albumIndex, int albumJsonIndex, string albumJsonName)
        {
            if (info == null) return;

            ModReflection.SetValue(info, "albumUidName", albumUidString, silent: true);
            ModReflection.SetValue(info, "albumIndex", albumIndex, silent: true);
            ModReflection.SetValue(info, "albumJsonIndex", albumJsonIndex, silent: true);
            ModReflection.SetValue(info, "albumJsonName", albumJsonName, silent: true);

            var musicExInfo = ModReflection.GetValue(info, "m_MusicExInfo", silent: true) ?? ModReflection.GetValue(info, "MusicExInfo", silent: true);
            if (musicExInfo != null)
            {
                ModReflection.SetValue(musicExInfo, "albumUidName", albumUidString, silent: true);
                ModReflection.SetValue(musicExInfo, "albumIndex", albumIndex, silent: true);
                ModReflection.SetValue(musicExInfo, "albumJsonName", albumJsonName, silent: true);
            }
        }

        // ==================== 디버깅 및 로깅 헬퍼 기능 통합 ====================

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
                foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
                {
                    if (!prop.CanRead || prop.GetIndexParameters().Length != 0) continue;
                    object value = SafeRead(() => prop.GetValue(info));
                    MelonLogger.Msg($"{label}: prop {prop.Name}={FormatValue(value)}");
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"{label} 덤프 예외: {ex}");
            }
        }

        private static void LogNestedMusicExInfo(string label, MusicInfo info)
        {
            try
            {
                var musicExInfo = ModReflection.GetValue(info, "m_MusicExInfo", silent: true) ?? ModReflection.GetValue(info, "MusicExInfo", silent: true);
                if (musicExInfo == null)
                {
                    MelonLogger.Msg($"{label}: (null)");
                    return;
                }

                var type = musicExInfo.GetType();
                MelonLogger.Msg($"{label}: type={type.FullName}");

                foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
                {
                    if (!prop.CanRead || prop.GetIndexParameters().Length != 0) continue;
                    string name = prop.Name ?? string.Empty;
                    if (name.IndexOf("album", StringComparison.OrdinalIgnoreCase) < 0 && name.IndexOf("cover", StringComparison.OrdinalIgnoreCase) < 0) continue;

                    object value = SafeRead(() => prop.GetValue(musicExInfo));
                    MelonLogger.Msg($"{label}: prop {name}={FormatValue(value)}");
                }

                foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
                {
                    string name = field.Name ?? string.Empty;
                    if (name.IndexOf("album", StringComparison.OrdinalIgnoreCase) < 0 && name.IndexOf("cover", StringComparison.OrdinalIgnoreCase) < 0) continue;

                    object value = SafeRead(() => field.GetValue(musicExInfo));
                    MelonLogger.Msg($"{label}: field {name}={FormatValue(value)}");
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Warning($"{label} 덤프 예외: {ex.Message}");
            }
        }

        private static object SafeRead(Func<object> read)
        {
            try { return read(); }
            catch (Exception ex) { return "(error: " + ex.Message + ")"; }
        }

        private static string FormatValue(object value)
        {
            return value == null ? "(null)" : value.ToString();
        }

        private static void CleanPurchaseProperties(object obj)
        {
            if (obj == null) return;
            ModReflection.SetValue(obj, "needPurchase", false, silent: true);
            ModReflection.SetValue(obj, "free", true, silent: true);
            ModReflection.SetValue(obj, "pay_ids", null, silent: true);
            ModReflection.SetValue(obj, "dlc", "", silent: true);
        }
    }
}
