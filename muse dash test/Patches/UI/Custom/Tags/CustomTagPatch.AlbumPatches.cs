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

            private static bool Prefix(MusicInfo __instance, ref string __result)
            {
                if (__instance == null) return true;
                if (!CustomContentIds.IsVirtualSong(__instance.uid)) return true;

                __result = CustomTagRegistry.AlbumUidString;

                if (!_logged)
                {
                    _logged = true;
                    MelonLogger.Msg($"[CustomTagRegistry] 가상 곡 앨범 UID 응답: uid={__instance.uid} → '{__result}' (이 로그는 1회만 표시됩니다)");
                }
                return false;
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

            private static bool Prefix(MusicInfo __instance, ref int __result)
            {
                if (__instance == null) return true;
                if (!CustomContentIds.IsVirtualSong(__instance.uid)) return true;

                __result = CustomTagRegistry.TagUid;

                if (!_logged)
                {
                    _logged = true;
                    MelonLogger.Msg($"[CustomTagRegistry] 가상 곡 앨범 인덱스 응답: uid={__instance.uid} → {__result} (이 로그는 1회만 표시됩니다)");
                }
                return false;
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

            private static bool Prefix(int index, ref string __result)
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
                    MelonLogger.Msg($"[CustomTagRegistry] 앨범 이름 현지화 응답: index={index} → '{__result}' (이 로그는 1회만 표시됩니다)");
                }
                return false;
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
            /// (26-8-6_23-19-54.log: 게임이 넘긴 index=4 vs musicIndex=4 / ExInfo.musicIndex=4 일치,
            ///  albumJsonIndex=1 · albumIndex=0은 불일치)
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

                // 행 번호만으로는 부족합니다. 가상 곡은 숙주의 얇은 복제본이라 행 번호(musicIndex=4)를
                // 그대로 물려받아서, 앨범마다 같은 행 번호를 가진 곡이 또 있습니다. 그래서 곡 인덱스 화면이
                // 그 곡들의 이름을 물어볼 때도 '아기상어'로 답해 버렸습니다.
                // (실측 26-8-12_8-43-14.log — 10:05:36.565 조회는 직후 PnlStage.RefreshDiffUI[1999-1]로 이어지는
                //  선택 곡 자신의 조회, 10:05:44.258·10:05:46.700·10:06:04.056·10:06:07.430 조회는 앞뒤로
                //  MusicButtonAreaTitle만 있는 목록 렌더링 조회였습니다.)
                // 지금 선택 곡 한 곡을 그리는 중일 때만 답합니다.
                if (!SelectedSongLocalizationScope.IsActive)
                {
                    SelectedSongLocalizationScope.LogRefusedOnce(
                        "DBConfigLocalALBUM.GetLocalAlbumInfoByIndex", index, $"선택 곡 uid={currentUid}의 행 번호와 같지만 목록 렌더링으로 간주");
                    return true;
                }

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
            /// <remarks>
            /// 가상 곡은 원본 곡의 얇은 복제본이므로 musicIndex를 원본에서 그대로 물려받고,
            /// 게임도 같은 값으로 로컬라이제이션 테이블을 조회합니다(위 OwnedIndexMembers 주석의 실측 근거).
            /// musicIndex를 MusicInfo에서도 MusicExInfo에서도 읽지 못하는 경우에만
            /// (게임 업데이트로 멤버명이 바뀐 상황) 종전의 관대한 동작으로 폴백하고 1회 경고를 남깁니다.
            /// </remarks>
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
            /// 게임 업데이트로 index 출처가 바뀌었을 때 곧바로 알아차리기 위한 진단이며 동작에는 영향을 주지 않습니다.
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
