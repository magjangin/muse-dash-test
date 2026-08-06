using HarmonyLib;
using Il2CppAssets.Scripts.Database;
using Il2CppAssets.Scripts.Database.DataClass;
using MelonLoader;
using System.Collections.Generic;

namespace muse_dash_test
{
    internal partial class CustomTagPatch
    {
        [HarmonyPatch(typeof(DBConfigAlbums), nameof(DBConfigAlbums.GetAlbumInfoByUid))]
        internal class DBConfigAlbums_GetAlbumInfoByUid_Patch
        {
            private static bool Prefix(DBConfigAlbums __instance, string uid, ref AlbumInfo __result)
            {
                if (uid == CustomTagRegistry.AlbumUidString)
                {
                    __result = CustomTagRegistry.VirtualAlbumInfo;
                    return false;
                }

                return true;
            }
        }

        [HarmonyPatch(typeof(DBConfigAlbums), nameof(DBConfigAlbums.GetAlbumInfoByIndex))]
        internal class DBConfigAlbums_GetAlbumInfoByIndex_Patch
        {
            private static bool Prefix(DBConfigAlbums __instance, int index, ref AlbumInfo __result)
            {
                if (index == CustomTagRegistry.TagUid)
                {
                    __result = CustomTagRegistry.VirtualAlbumInfo;
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

        [HarmonyPatch(typeof(DBConfigAlbums), nameof(DBConfigAlbums.GetAlbumJsonIndexByUid))]
        internal class DBConfigAlbums_GetAlbumJsonIndexByUid_Patch
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
        /// 단, 해당 가상 곡의 행 번호(musicIndex)를 대상으로 한 조회만 가로챕니다.
        /// </summary>
        [HarmonyPatch(typeof(DBConfigLocalALBUM), nameof(DBConfigLocalALBUM.GetLocalAlbumInfoByIndex))]
        internal class DBConfigLocalALBUM_GetLocalAlbumInfoByIndex_Patch
        {
            /// <summary>
            /// 게임이 넘기는 index의 출처가 될 수 있는 후보 멤버들.
            /// 로컬라이제이션 DB는 언어별 테이블(m_LocalDic) 안의 행 번호로 조회되므로,
            /// 앨범 인덱스뿐 아니라 곡 행 번호(musicIndex)도 후보에 포함합니다.
            /// </summary>
            private static readonly string[] AlbumIndexMembers = { "musicIndex", "albumJsonIndex", "albumIndex" };

            /// <summary>
            /// 실측으로 확정된 index의 출처. 로컬라이제이션 테이블의 곡 행 번호입니다.
            /// </summary>
            private static readonly string[] OwnedIndexMembers = { "musicIndex" };

            private static bool _loggedIndexDiagnostics;
            private static bool _loggedIndexResolveFailure;

            private static bool Prefix(DBConfigLocalALBUM __instance, int index, ref LocalALBUMInfo __result)
            {
                string currentUid = CustomPlaySession.Current.SelectedMusicUid;
                if (!CustomContentIds.IsVirtualSong(currentUid)) return true;

                LogIndexDiagnostics(currentUid, index);

                // 인덱스를 검사하지 않으면 가상 곡이 선택돼 있는 동안 다른 곡을 대상으로 한 조회까지
                // 커스텀 이름으로 응답해 버립니다(곡 목록 렌더링 등). 소유 행 번호만 가로챕니다.
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
            /// 조회된 index가 해당 가상 곡의 행 번호인지 판정합니다.
            /// </summary>
            private static bool IsIndexOwnedByVirtualSong(string uid, int index)
            {
                var info = GlobalDataBase.dbMusicTag?.GetMusicInfoFromAll(uid);
                var exInfo = info != null ? ModReflection.GetValue(info, "m_MusicExInfo", silent: true) : null;

                bool resolvedAny = false;
                foreach (var source in new[] { info, exInfo })
                {
                    if (source == null) continue;
                    foreach (var member in OwnedIndexMembers)
                    {
                        if (ModReflection.GetValue(source, member, silent: true) is int candidate)
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
                        MelonLogger.Warning($"[DBConfigLocalALBUM Patch] '{uid}'의 musicIndex를 읽지 못해 인덱스 검사 없이 응답합니다. " +
                                            "(이 경고는 1회만 표시됩니다. 게임 업데이트로 멤버명이 바뀌었을 수 있습니다.)");
                    }
                    return true;
                }

                return false;
            }

            /// <summary>
            /// 게임이 넘긴 index와 가상 곡이 들고 있는 인덱스 후보값들을 1회 대조 출력합니다.
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

                MelonLogger.Msg($"[DBConfigLocalALBUM Patch] Index 진단: queryIndex={index}, {string.Join(", ", parts)}");
            }
        }
    }
}
