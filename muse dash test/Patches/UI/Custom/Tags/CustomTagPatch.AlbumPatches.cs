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
        /// 단, 가상 곡 자신이 참조하는 앨범 인덱스에 대한 조회만 가로챕니다.
        /// </summary>
        [HarmonyPatch(typeof(DBConfigLocalALBUM), nameof(DBConfigLocalALBUM.GetLocalAlbumInfoByIndex))]
        internal class DBConfigLocalALBUM_GetLocalAlbumInfoByIndex_Patch
        {
            /// <summary>가상 곡이 참조할 수 있는 앨범 인덱스 후보 멤버들. 앞쪽이 우선 순위가 높습니다.</summary>
            private static readonly string[] AlbumIndexMembers = { "albumJsonIndex", "albumIndex" };

            private static bool _loggedIndexResolveFailure;

            private static bool Prefix(DBConfigLocalALBUM __instance, int index, ref LocalALBUMInfo __result)
            {
                string currentUid = CustomPlaySession.Current.SelectedMusicUid;
                if (!CustomContentIds.IsVirtualSong(currentUid)) return true;

                // 인덱스를 검사하지 않으면 가상 곡이 선택돼 있는 동안 다른 앨범의 조회까지 전부
                // 커스텀 이름으로 응답해 버립니다(곡 목록 렌더링 등). 소유 인덱스만 가로챕니다.
                if (!IsIndexOwnedByVirtualSong(currentUid, index)) return true;

                if (MainMod.TryGetHwaPrimarySong(currentUid, out string title, out string artist, out _, out _, out _, out _, out _, out _, out _))
                {
                    var localInfo = new LocalALBUMInfo();
                    localInfo.name = title;
                    localInfo.author = artist;
                    __result = localInfo;
                    MelonLogger.Msg($"[DBConfigLocalALBUM Patch] Index={index} 쿼리를 가상 곡 로컬 정보로 응답: uid={currentUid}, title={title}, artist={artist}");
                    return false;
                }

                return true;
            }

            /// <summary>
            /// 조회된 index가 해당 가상 곡이 실제로 참조하는 앨범 인덱스인지 판정합니다.
            /// </summary>
            /// <remarks>
            /// 가상 곡은 원본 곡의 얇은 복제본이고, MusicInfo의 albumIndex/albumJsonIndex는 인터롭 프록시에서
            /// getter 전용이라 주입이 불가능합니다. 따라서 이 값들은 복제 원본의 인덱스를 그대로 물려받습니다.
            /// 게임도 같은 값으로 조회하므로 이 대조가 성립합니다.
            /// 어떤 후보도 읽어내지 못한 경우에만(게임 업데이트로 멤버명이 바뀐 상황) 종전의 관대한 동작으로
            /// 폴백하되, 그 사실을 1회 경고로 남깁니다.
            /// </remarks>
            private static bool IsIndexOwnedByVirtualSong(string uid, int index)
            {
                var info = GlobalDataBase.dbMusicTag?.GetMusicInfoFromAll(uid);

                bool resolvedAny = false;
                if (info != null)
                {
                    foreach (var member in AlbumIndexMembers)
                    {
                        if (ModReflection.GetValue(info, member, silent: true) is int candidate)
                        {
                            resolvedAny = true;
                            if (candidate == index) return true;
                        }
                    }
                }

                if (!resolvedAny)
                {
                    if (!_loggedIndexResolveFailure)
                    {
                        _loggedIndexResolveFailure = true;
                        MelonLogger.Warning($"[DBConfigLocalALBUM Patch] '{uid}'의 앨범 인덱스를 읽지 못해 인덱스 검사 없이 응답합니다. " +
                                            $"(이 경고는 1회만 표시됩니다. 게임 업데이트로 albumJsonIndex/albumIndex 멤버명이 바뀌었을 수 있습니다.)");
                    }
                    return true;
                }

                return false;
            }
        }
    }
}
