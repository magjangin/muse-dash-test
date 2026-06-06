using MelonLoader;
using Il2Cpp;
using Il2CppAssets.Scripts.Database;
using Il2CppAssets.Scripts.Database.DataClass;
using Il2CppAssets.Scripts.PeroTools.Commons;
using Il2CppAssets.Scripts.PeroTools.Managers;
using System;
using System.Collections.Generic;
using UnityEngine;

using static Il2CppAssets.Scripts.Database.DBConfigCustomTags;

namespace muse_dash_test
{
    internal static class CustomTagRegistrySupport
    {
        internal static Il2CppSystem.Collections.Generic.Dictionary<string, string> CreateTagLanguages(out string defaultName)
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

        internal static List<string> BuildAndInjectVirtualSongs()
        {
            var musicList = new List<string>();

            try
            {
                var vUids = MainMod.GetVirtualUids();
                MelonLogger.Msg($"[CustomTagRegistry] 가상 곡 생성 시작: count={vUids.Count}");

                foreach (var uid in vUids)
                {
                    MainMod.TryGetCachedHwaSearchTerms(uid, out string sourceUid, out string sourceTitle, out string sourceArtist, out string sourceAlbum, out string _);

                    string lookupQuery = string.IsNullOrWhiteSpace(sourceUid) ? null : sourceUid;
                    if (string.IsNullOrWhiteSpace(lookupQuery)) lookupQuery = sourceTitle;
                    if (string.IsNullOrWhiteSpace(lookupQuery)) lookupQuery = sourceArtist;

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
                            if (!string.IsNullOrWhiteSpace(manifestTitle)) primaryName = manifestTitle;
                            if (!string.IsNullOrWhiteSpace(manifestArtist)) primaryAuthor = manifestArtist;
                            if (!string.IsNullOrWhiteSpace(manifestLevelDesigner)) primaryLevelDesigner = manifestLevelDesigner;
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

        internal static DBConfigAlbums.AlbumsInfo CreateAndInjectAlbumInfo()
        {
            DBConfigAlbums.AlbumsInfo albumInfo = null;

            try
            {
                var albumsConfig = Il2CppAssets.Scripts.PeroTools.Commons.Singleton<ConfigManager>.instance.GetConfigObject<DBConfigAlbums>();
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
                                albumWrapper.uid = CustomTagRegistry.AlbumUidString;
                                albumWrapper.title = CustomTagRegistry.AlbumTitle;
                                albumWrapper.tag = CustomTagRegistry.TagUidString;
                                albumWrapper.jsonName = "custom_album_1998_0";
                                albumWrapper.prefabsName = CustomTagRegistry.AlbumCoverPrefabName;

                                CustomTagRegistrySupport.CleanPurchaseProperties(clonedAlbum);
                                var albumExInfo = ModReflection.GetValue(clonedAlbum, "m_AlbumExInfo");
                                if (albumExInfo != null)
                                {
                                    CustomTagRegistrySupport.CleanPurchaseProperties(albumExInfo);
                                }

                                albumInfo = clonedAlbum;
                                CustomTagRegistry.CustomAlbumInfo = clonedAlbum;

                                bool exists = false;
                                for (int i = 0; i < items.Count; i++)
                                {
                                    if (items[i].uid == CustomTagRegistry.AlbumUidString)
                                    {
                                        exists = true;
                                        break;
                                    }
                                }

                                if (!exists)
                                {
                                    items.Add(clonedAlbum);
                                    MelonLogger.Msg("[CustomTagRegistry] [성공] 얇은 복제 방식으로 DBConfigAlbums.m_Items에 가상 앨범(1998-0) 주입 완료!");
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

            if (albumInfo == null)
            {
                var fallbackAlbum = new DBConfigAlbums.AlbumsInfo();
                var fallbackWrapper = new AlbumsInfoWrapper(fallbackAlbum);
                fallbackWrapper.uid = CustomTagRegistry.AlbumUidString;
                fallbackWrapper.title = CustomTagRegistry.AlbumTitle;
                fallbackWrapper.tag = CustomTagRegistry.TagUidString;
                fallbackWrapper.jsonName = "custom_album_1998_0";
                fallbackWrapper.prefabsName = CustomTagRegistry.AlbumCoverPrefabName;

                CustomTagRegistrySupport.CleanPurchaseProperties(fallbackAlbum);

                albumInfo = fallbackAlbum;
                CustomTagRegistry.CustomAlbumInfo = fallbackAlbum;
                MelonLogger.Warning("[CustomTagRegistry] [경고] 복제에 실패하여 new AlbumsInfo 폴백을 생성했습니다.");
            }

            return albumInfo;
        }

        internal static Il2CppSystem.Collections.Generic.List<string> ToIl2CppStringList(List<string> source)
        {
            var result = new Il2CppSystem.Collections.Generic.List<string>(source.Count);
            foreach (var value in source)
            {
                result.Add(value);
            }
            return result;
        }

        internal static void InjectVirtualSong(MusicInfo originalInfo, string uid, string name, string author, string levelDesigner, string cover, string noteJson, string music, int diff1, int diff2, List<string> musicList)
        {
            InjectVirtualSong(originalInfo, uid, name, author, levelDesigner, cover, noteJson, music, diff1, diff2, 0, 0, 0, musicList);
        }

        internal static void InjectVirtualSong(MusicInfo originalInfo, string uid, string name, string author, string levelDesigner, string cover, string noteJson, string music, int diff1, int diff2, int diff3, int diff4, int diff5, List<string> musicList)
        {
            try
            {
                if (!TryCloneMusicInfo(originalInfo, uid, out MusicInfo clonedInfo)) return;

                ApplyVirtualSongMetadata(clonedInfo, uid, name, author, levelDesigner, diff1, diff2, diff3, diff4, diff5);
                CleanPurchaseProperties(clonedInfo);
                var musicExInfo = ModReflection.GetValue(clonedInfo, "m_MusicExInfo");
                if (musicExInfo != null) CleanPurchaseProperties(musicExInfo);

                if (!ApplyVirtualSongAlbumMetadata(clonedInfo, uid)) return;

                CustomTagRegistryDebug.LogNestedMusicExInfo("[CustomTagRegistry] [복제 후 MusicExInfo 덤프]", clonedInfo);
                RegisterVirtualSong(clonedInfo, uid, musicList);
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[CustomTagRegistry] {uid} 주입 중 예외 발생: {ex}");
            }
        }

        internal static bool TryCloneMusicInfo(MusicInfo originalInfo, string uid, out MusicInfo clonedInfo)
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

        internal static void ApplyVirtualSongMetadata(MusicInfo clonedInfo, string uid, string name, string author, string levelDesigner, int diff1, int diff2, int diff3, int diff4, int diff5)
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

        internal static bool ApplyVirtualSongAlbumMetadata(MusicInfo clonedInfo, string uid)
        {
            try
            {
                var wrapper = new MusicInfoWrapper(clonedInfo);
                wrapper.AddMaskValue("albumUidName", (Il2CppSystem.String)CustomTagRegistry.AlbumUidString);
                wrapper.AddMaskValue("albumIndex", new Il2CppSystem.Int32 { m_value = CustomTagRegistry.TagUid }.BoxIl2CppObject());
                wrapper.AddMaskValue("albumJsonName", (Il2CppSystem.String)"custom_album_1998_0");
                SetAlbumMetadata(clonedInfo, CustomTagRegistry.AlbumUidString, CustomTagRegistry.TagUid, CustomTagRegistry.TagUid + 1, "custom_album_1998_0");
                return true;
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[CustomTagRegistry] [실패] {uid} AddMaskValue 앨범 마스크 적용 예외: {ex}");
                return false;
            }
        }

        internal static void RegisterVirtualSong(MusicInfo clonedInfo, string uid, List<string> musicList)
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

        internal static void SetAlbumMetadata(MusicInfo info, string albumUidString, int albumIndex, int albumJsonIndex, string albumJsonName)
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

        internal static void CleanPurchaseProperties(object obj)
        {
            if (obj == null) return;
            ModReflection.SetValue(obj, "needPurchase", false, silent: true);
            ModReflection.SetValue(obj, "free", true, silent: true);
            ModReflection.SetValue(obj, "pay_ids", null, silent: true);
            ModReflection.SetValue(obj, "dlc", "", silent: true);
        }
    }
}
