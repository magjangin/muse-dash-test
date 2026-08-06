using MelonLoader;
using HarmonyLib;
using Il2Cpp;
using Il2CppAssets.Scripts.Database;
using Il2CppAssets.Scripts.Database.DataClass;
using Il2CppAssets.Scripts.PeroTools.Commons;
using Il2CppAssets.Scripts.PeroTools.Managers;
using System;

namespace muse_dash_test
{
    internal partial class CustomTagPatch
    {
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
                        configObject.m_MaxAlbumUid = configObject.count - 3;
                    }
                }
                catch (Exception ex)
                {
                    MelonLogger.Warning($"m_MaxAlbumUid 성능 최적화 패치 적용 중 예외 발생 (비치명적): {ex.Message}");
                }
            }
        }

        [HarmonyPatch(typeof(DBConfigAlbums), nameof(DBConfigAlbums.GetAlbumInfoByMusicInfo))]
        internal class DBConfigAlbums_GetAlbumInfoByMusicInfo_Patch
        {
            private static bool Prefix(DBConfigAlbums __instance, MusicInfo musicInfo, ref DBConfigAlbums.AlbumsInfo __result)
            {
                if (musicInfo != null && CustomContentIds.IsVirtualSong(musicInfo.uid) && CustomTagRegistry.CustomAlbumInfo != null)
                {
                    __result = CustomTagRegistry.CustomAlbumInfo;
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
                if (uid == CustomTagRegistry.AlbumUidString && CustomTagRegistry.CustomAlbumInfo != null)
                {
                    __result = CustomTagRegistry.CustomAlbumInfo;
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
                if (uid == CustomTagRegistry.AlbumUidString)
                {
                    __result = CustomTagRegistry.TagUid;
                    return false;
                }

                return true;
            }
        }

        /// <summary>
        /// MusicInfo.GetLocal 호출 시 가상 곡의 LocalALBUMInfo(name, author)를 즉석 반환하여 로컬라이제이션 오버라이드를 차단합니다.
        /// </summary>
        [HarmonyPatch(typeof(MusicInfo), nameof(MusicInfo.GetLocal))]
        internal class MusicInfo_GetLocal_Patch
        {
            private static bool Prefix(MusicInfo __instance, int language, ref LocalALBUMInfo __result)
            {
                if (__instance != null && CustomContentIds.IsVirtualSong(__instance.uid))
                {
                    if (MainMod.TryGetHwaPrimarySong(__instance.uid, out string title, out string artist, out _, out _, out _, out _, out _, out _, out _))
                    {
                        var localInfo = new LocalALBUMInfo();
                        localInfo.name = title;
                        localInfo.author = artist;
                        __result = localInfo;
                        MelonLogger.Msg($"[MusicInfo.GetLocal Patch] 가상 곡 로컬 라이브러리 가로채기 성공: uid={__instance.uid}, title={title}, artist={artist}");
                        return false;
                    }
                }
                return true;
            }
        }

        /// <summary>
        /// DBConfigLocalALBUM.GetLocalAlbumInfoByIndex 쿼리 시 가상 곡의 LocalALBUMInfo를 즉석 응답합니다.
        /// </summary>
        [HarmonyPatch(typeof(DBConfigLocalALBUM), nameof(DBConfigLocalALBUM.GetLocalAlbumInfoByIndex))]
        internal class DBConfigLocalALBUM_GetLocalAlbumInfoByIndex_Patch
        {
            private static bool Prefix(DBConfigLocalALBUM __instance, int index, ref LocalALBUMInfo __result)
            {
                string currentUid = CustomPlaySession.Current.SelectedMusicUid;
                if (CustomContentIds.IsVirtualSong(currentUid))
                {
                    if (MainMod.TryGetHwaPrimarySong(currentUid, out string title, out string artist, out _, out _, out _, out _, out _, out _, out _))
                    {
                        var localInfo = new LocalALBUMInfo();
                        localInfo.name = title;
                        localInfo.author = artist;
                        __result = localInfo;
                        MelonLogger.Msg($"[DBConfigLocalALBUM Patch] Index={index} 쿼리를 가상 곡 로컬 정보로 응답: uid={currentUid}, title={title}, artist={artist}");
                        return false;
                    }
                }
                return true;
            }
        }
    }
}
