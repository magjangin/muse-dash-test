using MelonLoader;
using HarmonyLib;
using Il2Cpp;
using Il2CppAssets.Scripts.Database;
using Il2CppAssets.Scripts.Database.DataClass;
using Il2CppAssets.Scripts.PeroTools.Commons;
using Il2CppAssets.Scripts.PeroTools.Managers;
using System;
using System.Collections.Generic;

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
                    ModLogger.Warning($"m_MaxAlbumUid 성능 최적화 패치 적용 중 예외 발생 (비치명적): {ex.Message}");
                }
            }
        }

        [HarmonyPatch(typeof(DBConfigAlbums), nameof(DBConfigAlbums.GetAlbumInfoByMusicInfo))]
        internal class DBConfigAlbums_GetAlbumInfoByMusicInfo_Patch
        {
            private static bool _loggedPrefixFailure;

            private static bool Prefix(DBConfigAlbums __instance, MusicInfo musicInfo, ref DBConfigAlbums.AlbumsInfo __result)
            {
                try
                {
                    if (musicInfo != null && CustomContentIds.IsVirtualSong(musicInfo.uid) && CustomTagRegistry.CustomAlbumInfo != null)
                    {
                        __result = CustomTagRegistry.CustomAlbumInfo;
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    if (!_loggedPrefixFailure)
                    {
                        _loggedPrefixFailure = true;
                        ModLogger.Error($"[CustomTagRegistry.GetAlbumInfoByMusicInfo] 예외로 원본 조회로 넘깁니다: {ex}");
                    }
                }

                return true;
            }
        }

        [HarmonyPatch(typeof(DBConfigAlbums), nameof(DBConfigAlbums.GetAlbumsInfoByUid))]
        internal class DBConfigAlbums_GetAlbumsInfoByUid_Patch
        {
            private static bool _loggedPrefixFailure;

            private static bool Prefix(DBConfigAlbums __instance, string uid, ref DBConfigAlbums.AlbumsInfo __result)
            {
                try
                {
                    if (uid == CustomTagRegistry.AlbumUidString && CustomTagRegistry.CustomAlbumInfo != null)
                    {
                        __result = CustomTagRegistry.CustomAlbumInfo;
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    if (!_loggedPrefixFailure)
                    {
                        _loggedPrefixFailure = true;
                        ModLogger.Error($"[CustomTagRegistry.GetAlbumsInfoByUid] 예외로 원본 조회로 넘깁니다: {ex}");
                    }
                }

                return true;
            }
        }

        /// <summary>
        /// 가상 곡의 소속 앨범 UID를 커스텀 앨범('1999-0')으로 답합니다.
        ///
        /// <para><c>albumIndex</c>와 마찬가지로 getter 전용 프로퍼티라 값을 써 넣을 수 없어,
        /// 지금까지 숙주의 'music_package_0'을 그대로 돌려주고 있었습니다.
        /// 팩 이름 라벨이 <c>albumIndex</c>를 1999로 고쳐 준 뒤에도 여전히 0을 물어본 이유가 이것으로 보입니다 —
        /// 인덱스를 이 UID에서 되찾아 오면(<c>GetAlbumIndexByUid("music_package_0")</c>) 0이 나옵니다.</para>
        /// </summary>
        [HarmonyPatch(typeof(MusicInfo), nameof(MusicInfo.albumUidName), MethodType.Getter)]
        internal class MusicInfo_albumUidName_Patch
        {
            private static bool _logged;
            private static bool _loggedPrefixFailure;

            private static bool Prefix(MusicInfo __instance, ref string __result)
            {
                try
                {
                    if (__instance == null) return true;
                    if (!CustomContentIds.IsVirtualSong(__instance.uid)) return true;

                    __result = CustomTagRegistry.AlbumUidString;

                    if (!_logged)
                    {
                        _logged = true;
                        ModLogger.Verbose($"[CustomTagRegistry] 가상 곡 앨범 UID 응답: uid={__instance.uid} → '{__result}' (이 로그는 1회만 표시됩니다)");
                    }
                    return false;
                }
                catch (Exception ex)
                {
                    if (!_loggedPrefixFailure)
                    {
                        _loggedPrefixFailure = true;
                        ModLogger.Error($"[CustomTagRegistry.albumUidName] 예외로 원본 조회로 넘깁니다: {ex}");
                    }
                    return true;
                }
            }
        }

        /// <summary>
        /// 가상 곡의 앨범 인덱스를 1999(<see cref="CustomTagRegistry.TagUid"/>)로 답합니다.
        ///
        /// <para><b>왜 필드에 쓰지 않고 getter를 가로채나</b> — <c>MusicInfo.albumIndex</c>는 인터롭에
        /// getter만 있는 계산 프로퍼티입니다(백킹 필드 없음). 그래서
        /// <c>CustomTagRegistrySupport.SetAlbumMetadata</c>의 `SetValue(info, "albumIndex", 1999)`는
        /// <c>silent: true</c>에 묻혀 조용히 실패해 왔고, 값은 숙주에서 물려받은 0으로 남아 있었습니다.
        /// (<c>albumJsonIndex</c>가 2000이 아니라 1인 것도 같은 이유입니다.)</para>
        ///
        /// <para>실측(26-8-12 로그): 곡 정보 패널을 채우는 루틴이 곡 제목·아티스트를 현지화한 직후
        /// <c>GetLocalTitleByIndex(albumIndex)</c>로 팩 이름을 가져오는데, 그 인덱스가 0이라
        /// '기본 패키지'가 찍혔습니다. 같은 순간 1999로 물어본 다른 호출자는 '실험 앨범'을 제대로 받았습니다.</para>
        /// </summary>
        [HarmonyPatch(typeof(MusicInfo), nameof(MusicInfo.albumIndex), MethodType.Getter)]
        internal class MusicInfo_albumIndex_Patch
        {
            private static bool _logged;
            private static bool _loggedPrefixFailure;

            private static bool Prefix(MusicInfo __instance, ref int __result)
            {
                try
                {
                    if (__instance == null) return true;
                    if (!CustomContentIds.IsVirtualSong(__instance.uid)) return true;

                    __result = CustomTagRegistry.TagUid;

                    if (!_logged)
                    {
                        _logged = true;
                        ModLogger.Verbose($"[CustomTagRegistry] 가상 곡 앨범 인덱스 응답: uid={__instance.uid} → {__result} (이 로그는 1회만 표시됩니다)");
                    }
                    return false;
                }
                catch (Exception ex)
                {
                    if (!_loggedPrefixFailure)
                    {
                        _loggedPrefixFailure = true;
                        ModLogger.Error($"[CustomTagRegistry.albumIndex] 예외로 원본 조회로 넘깁니다: {ex}");
                    }
                    return true;
                }
            }
        }

        /// <summary>
        /// 곡 선택 화면의 팩 이름(<c>ImgAlbumTittle</c>)은 <c>AlbumsInfo.title</c>이 아니라
        /// <b>인덱스로 현지화 테이블</b>을 조회해서 만듭니다. 커스텀 앨범은 그 테이블에 행이 없어
        /// <c>GetLocalTitleByIndex(1999)</c>가 null을 돌려주고, 호출자가 인덱스 0으로 폴백해
        /// '기본 패키지'가 찍혔습니다. 그래서 title을 아무리 맞춰 놔도 화면에는 안 나왔습니다.
        ///
        /// 실측 순서(26-8-12 로그):
        ///   GetAlbumIndexByUid('1999-0') → 1999
        ///   GetLocalTitleByIndex(1999)   → (null)
        ///   GetLocalTitleByIndex(0)      → '기본 패키지'   ← 폴백
        ///   RefreshText('기본 패키지')    → ImgAlbumTittle
        ///
        /// 우리 인덱스에만 답을 채워 주면 폴백 자체가 일어나지 않습니다. 인덱스 0은 건드리지 않으므로
        /// 진짜 기본 패키지에 속한 곡들의 팩 이름은 그대로입니다.
        /// </summary>
        [HarmonyPatch(typeof(DBConfigLocalAlbums), nameof(DBConfigLocalAlbums.GetLocalTitleByIndex), new Type[] { typeof(int) })]
        internal class DBConfigLocalAlbums_GetLocalTitleByIndex_Patch
        {
            private static bool _logged;
            private static bool _loggedPrefixFailure;

            private static bool Prefix(int index, ref string __result)
            {
                try
                {
                    // 게임이 넘기는 index는 두 가지입니다. 앨범의 전역 인덱스(1999)로 물어보는 호출자도 있고,
                    // '지금 선택된 태그 안에서 몇 번째 앨범이냐'로 물어보는 호출자도 있습니다.
                    // 후자가 팩 이름 라벨을 채우는 쪽인데, 커스텀 태그에는 앨범이 하나뿐이라 늘 0으로 옵니다.
                    // 그 0은 현지화 테이블에서는 첫 앨범('기본 패키지')을 가리켜서 엉뚱한 이름이 찍혔습니다.
                    //
                    // 다만 후자는 인덱스만으로 우리 앨범인지 가릴 수 없어서, 커스텀 태그를 보고 있는 동안
                    // 들어오는 조회를 전부 '실험 앨범'으로 답해 왔습니다. 곡 인덱스 화면은 팩마다 이 조회를
                    // 돌리기 때문에 화면의 팩 이름이 전부 '실험 앨범'이 됐습니다
                    // (실측 26-8-12_8-43-14.log 10:05:44~10:06:09: MusicButtonAreaTitle이 전부 '실험 앨범').
                    // 그래서 관대한 쪽은 선택 곡 한 곡을 그리는 구간에서만 허용합니다.
                    bool ours = index == CustomTagRegistry.TagUid ||
                                (IsCustomTagSelected() && SelectedSongLocalizationScope.IsActive);
                    if (!ours)
                    {
                        if (IsCustomTagSelected())
                        {
                            SelectedSongLocalizationScope.LogRefusedOnce(
                                "DBConfigLocalAlbums.GetLocalTitleByIndex", index, "커스텀 태그 선택 중이지만 곡 목록 렌더링으로 간주");
                        }
                        return true;
                    }

                    __result = CustomTagRegistry.AlbumTitle;

                    if (!_logged)
                    {
                        _logged = true;
                        ModLogger.Verbose($"[CustomTagRegistry] 앨범 이름 현지화 응답: index={index} → '{__result}' (이 로그는 1회만 표시됩니다)");
                    }
                    return false;
                }
                catch (Exception ex)
                {
                    if (!_loggedPrefixFailure)
                    {
                        _loggedPrefixFailure = true;
                        ModLogger.Error($"[CustomTagRegistry.GetLocalTitleByIndex] 예외로 원본 조회로 넘깁니다: {ex}");
                    }
                    return true;
                }
            }

            /// <summary>커스텀 태그를 보고 있는 동안의 조회는 전부 우리 앨범입니다. 그 태그에는 앨범이 하나뿐입니다.</summary>
            private static bool IsCustomTagSelected()
            {
                try
                {
                    var db = GlobalDataBase.dbMusicTag;
                    return db != null && db.m_CurSelectedTagIndex == CustomTagRegistry.TagUid;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        [HarmonyPatch(typeof(DBConfigAlbums), nameof(DBConfigAlbums.GetAlbumIndexByUid))]
        internal class DBConfigAlbums_GetAlbumIndexByUid_Patch
        {
            private static bool _loggedPrefixFailure;

            private static bool Prefix(DBConfigAlbums __instance, string uid, ref int __result)
            {
                try
                {
                    if (uid == CustomTagRegistry.AlbumUidString)
                    {
                        __result = CustomTagRegistry.TagUid;
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    if (!_loggedPrefixFailure)
                    {
                        _loggedPrefixFailure = true;
                        ModLogger.Error($"[CustomTagRegistry.GetAlbumIndexByUid] 예외로 원본 조회로 넘깁니다: {ex}");
                    }
                }

                return true;
            }
        }
    }
}
