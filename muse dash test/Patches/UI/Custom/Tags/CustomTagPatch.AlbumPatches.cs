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
                if (musicInfo != null && musicInfo.uid != null && musicInfo.uid.StartsWith("1000-") && CustomTagRegistry.CustomAlbumInfo != null)
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
    }
}