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
        /// 현재는 index를 검사하지 않습니다(아래 TODO 참고).
        /// </summary>
        [HarmonyPatch(typeof(DBConfigLocalALBUM), nameof(DBConfigLocalALBUM.GetLocalAlbumInfoByIndex))]
        internal class DBConfigLocalALBUM_GetLocalAlbumInfoByIndex_Patch
        {
            /// <summary>가상 곡이 참조할 수 있는 앨범 인덱스 후보 멤버들.</summary>
            private static readonly string[] AlbumIndexMembers = { "albumJsonIndex", "albumIndex" };

            private static bool _loggedIndexDiagnostics;

            private static bool Prefix(DBConfigLocalALBUM __instance, int index, ref LocalALBUMInfo __result)
            {
                string currentUid = CustomPlaySession.Current.SelectedMusicUid;
                if (!CustomContentIds.IsVirtualSong(currentUid)) return true;

                // TODO(over-capture): 인덱스를 검사하지 않으므로, 가상 곡이 선택돼 있는 동안에는
                // 다른 앨범을 대상으로 한 조회까지 커스텀 이름으로 응답합니다(곡 목록 렌더링 등).
                //
                // 인덱스 게이트를 한 번 시도했다가 되돌렸습니다. 가상 곡의 albumJsonIndex/albumIndex와
                // 실제 조회 인덱스가 일치할 것이라 보고 게이트를 걸었으나, 실측 로그에서 조회가 전부
                // 차단됐습니다(26-8-6_23-8-30.log: 종전에 히트하던 호출 지점 2곳에서 히트 소멸).
                // 즉 게임이 넘기는 index는 곡의 albumJsonIndex/albumIndex가 아닌 다른 경로에서 옵니다.
                // 아래 진단 로그로 실제 index와 후보값의 관계를 확정한 뒤 게이트를 다시 겁니다.
                LogIndexDiagnostics(currentUid, index);

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
            /// 게임이 넘긴 index와, 가상 곡이 들고 있는 앨범 인덱스 후보값들을 1회 대조 출력합니다.
            /// over-capture를 막을 게이트 조건을 확정하기 위한 진단용이며 동작에는 영향을 주지 않습니다.
            /// </summary>
            private static void LogIndexDiagnostics(string uid, int index)
            {
                if (_loggedIndexDiagnostics) return;
                _loggedIndexDiagnostics = true;

                var info = GlobalDataBase.dbMusicTag?.GetMusicInfoFromAll(uid);
                var parts = new List<string>();

                foreach (var member in AlbumIndexMembers)
                {
                    object raw = info != null ? ModReflection.GetValue(info, member, silent: true) : null;
                    parts.Add($"{member}={(raw?.ToString() ?? "(읽기 실패)")}");
                }

                var exInfo = info != null ? ModReflection.GetValue(info, "m_MusicExInfo", silent: true) : null;
                if (exInfo != null)
                {
                    foreach (var member in AlbumIndexMembers)
                    {
                        object raw = ModReflection.GetValue(exInfo, member, silent: true);
                        parts.Add($"ExInfo.{member}={(raw?.ToString() ?? "(읽기 실패)")}");
                    }
                }

                MelonLogger.Msg($"[DBConfigLocalALBUM 진단] 게임이 넘긴 index={index} vs 가상 곡 후보값 [{string.Join(", ", parts)}] " +
                                $"(uid={uid}, 앨범 인덱스 상수 TagUid={CustomTagRegistry.TagUid}). 이 진단은 1회만 표시됩니다.");
            }
        }
    }
}
