using HarmonyLib;
using Il2Cpp;
using Il2CppAssets.Scripts.Database;
using Il2CppAssets.Scripts.Database.DataClass;
using System;
using System.Collections.Generic;

namespace muse_dash_test
{
    internal partial class CustomTagPatch
    {
        /// <summary>
        /// MusicInfo.GetLocal 호출 시 가상 곡의 LocalALBUMInfo(name, author)를 즉석 반환하여 로컬라이제이션 오버라이드를 차단합니다.
        /// </summary>
        [HarmonyPatch(typeof(MusicInfo), nameof(MusicInfo.GetLocal))]
        internal class MusicInfo_GetLocal_Patch
        {
            private static bool _loggedPrefixFailure;

            // 제목 조회는 곡 목록을 그릴 때마다 불립니다. 여기서 던지면 게임 UI 갱신이 끊기므로
            // 예외는 첫 1회만 기록하고 원본 조회로 넘깁니다(원본 제목이 보일 뿐 화면은 유지).
            private static bool Prefix(MusicInfo __instance, int language, ref LocalALBUMInfo __result)
            {
                try
                {
                    return PrefixCore(__instance, ref __result);
                }
                catch (Exception ex)
                {
                    if (!_loggedPrefixFailure)
                    {
                        _loggedPrefixFailure = true;
                        ModLogger.Error($"[MusicInfo.GetLocal Patch] 예외로 원본 조회로 넘깁니다(이후 같은 예외는 생략): {ex}");
                    }
                    return true;
                }
            }

            private static bool PrefixCore(MusicInfo __instance, ref LocalALBUMInfo __result)
            {
                if (__instance != null && CustomContentIds.IsVirtualSong(__instance.uid))
                {
                    if (HwaResourceManager.TryGetHwaPrimarySong(__instance.uid, out string title, out string artist, out _, out _, out _, out _, out _, out _))
                    {
                        var localInfo = new LocalALBUMInfo();
                        localInfo.name = title;
                        localInfo.author = artist;
                        __result = localInfo;
                        ModLogger.Verbose($"[MusicInfo.GetLocal Patch] 가상 곡 로컬 라이브러리 가로채기 성공: uid={__instance.uid}, title={title}, artist={artist}");
                        return false;
                    }
                }
                return true;
            }
        }

        /// <summary>
        /// DBConfigLocalALBUM.GetLocalAlbumInfoByIndex 쿼리 시 가상 곡의 LocalALBUMInfo를 즉석 응답합니다.
        /// 단, 해당 가상 곡의 행 번호(BaseDataObject.listIndex)를 대상으로 한 조회만 가로챕니다.
        /// </summary>
        [HarmonyPatch(typeof(DBConfigLocalALBUM), nameof(DBConfigLocalALBUM.GetLocalAlbumInfoByIndex))]
        internal class DBConfigLocalALBUM_GetLocalAlbumInfoByIndex_Patch
        {
            /// <summary>
            /// 게임이 넘기는 index의 출처가 될 수 있는 후보 멤버들.
            /// 로컬라이제이션 DB는 언어별 테이블(m_LocalDic) 안의 행 번호로 조회되므로,
            /// 앨범 인덱스뿐 아니라 곡 행 번호(musicIndex)도 후보에 포함합니다.
            /// </summary>
            private static readonly string[] AlbumIndexMembers = { "listIndex", "musicIndex", "albumJsonIndex", "albumIndex" };

            private static bool _loggedIndexDiagnostics;
            private static bool _loggedIndexResolveFailure;

            private static bool _loggedPrefixFailure;

            // MusicInfo.GetLocal 패치와 같은 이유로 예외는 첫 1회만 기록하고 원본 조회로 넘깁니다.
            private static bool Prefix(DBConfigLocalALBUM __instance, int index, ref LocalALBUMInfo __result)
            {
                try
                {
                    return PrefixCore(index, ref __result);
                }
                catch (Exception ex)
                {
                    if (!_loggedPrefixFailure)
                    {
                        _loggedPrefixFailure = true;
                        ModLogger.Error($"[DBConfigLocalALBUM Patch] 예외로 원본 조회로 넘깁니다(이후 같은 예외는 생략): {ex}");
                    }
                    return true;
                }
            }

            private static bool PrefixCore(int index, ref LocalALBUMInfo __result)
            {
                // 곡을 바꾸는 순간엔 세션의 선택 곡이 아직 이전 곡입니다. 지금 그리는 곡을 먼저 봅니다.
                string currentUid = SelectedSongLocalizationScope.DrawingUid ?? CustomPlaySession.Current.SelectedMusicUid;
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

                if (HwaResourceManager.TryGetHwaPrimarySong(currentUid, out string title, out string artist, out _, out _, out _, out _, out _, out _))
                {
                    var localInfo = new LocalALBUMInfo();
                    localInfo.name = title;
                    localInfo.author = artist;
                    __result = localInfo;
                    ModLogger.Verbose($"[DBConfigLocalALBUM Patch] Index={index} 쿼리를 가상 곡 로컬 정보로 응답: uid={currentUid}, title={title}, artist={artist}");
                    return false;
                }

                return true;
            }

            /// <summary>
            /// 조회된 index가 해당 가상 곡의 행 번호(<see cref="MusicInfoWrapper.listIndex"/>)인지 판정합니다.
            /// </summary>
            /// <remarks>
            /// 게임의 제목 조회는 두 겹입니다(ISIL 덤프, 게임 6.7.0 — <c>MusicInfo.GetLocal</c>,
            /// <c>PnlStage.ChangeMusic</c>, <c>SetSelectedMusicNameTxt</c> 모두 같은 모양):
            /// <list type="number">
            /// <item><c>MusicInfo.m_MusicExInfo.m_AlbumIndex</c>(+0xE8, 프로퍼티 이름은 <c>albumJsonIndex</c>)로 앨범 현지화 표를 고르고</item>
            /// <item>그 언어 표에 <c>BaseDataObject.listIndex</c>(+0x10)를 넘겨 <c>GetLocalAlbumInfoByIndex</c>를 부릅니다.</item>
            /// </list>
            /// 예전에는 musicIndex(ExInfo.m_MusicIndex)를 행 번호로 봤습니다. 가상 곡이 하나뿐이던 때는
            /// 두 값이 우연히 같았지만(26-8-6 실측: 4 = 4), 곡이 늘자 갈라졌습니다(26-10-23 실측: 게임이 넘긴
            /// index=23, musicIndex=10). 그래서 가로채기가 빗나가 원본 곡 23번 행의 제목(恋爱语音导航 feat.yousa)이
            /// 곡 선택 화면 하단에 그대로 나왔습니다.
            /// listIndex를 읽지 못하는 경우에만(게임 업데이트로 멤버가 바뀐 상황) 종전의 관대한 동작으로
            /// 폴백하고 1회 경고를 남깁니다.
            /// </remarks>
            private static bool IsIndexOwnedByVirtualSong(string uid, int index)
            {
                var info = GlobalDataBase.dbMusicTag?.GetMusicInfoFromAll(uid);
                int? listIndex = info != null ? new MusicInfoWrapper(info).listIndex : null;

                if (listIndex == null)
                {
                    if (!_loggedIndexResolveFailure)
                    {
                        _loggedIndexResolveFailure = true;
                        ModLogger.Warning($"[DBConfigLocalALBUM Patch] '{uid}'의 listIndex를 읽지 못해 인덱스 검사 없이 응답합니다. " +
                                            "(이 경고는 1회만 표시됩니다. 게임 업데이트로 멤버명이 바뀌었을 수 있습니다.)");
                    }
                    return true;
                }

                return listIndex.Value == index;
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

                ModLogger.Verbose($"[DBConfigLocalALBUM 진단] 게임이 넘긴 index={index} vs 가상 곡 후보값 [{string.Join(", ", parts)}] " +
                                $"(uid={uid}, 앨범 인덱스 상수 TagUid={CustomTagRegistry.TagUid}). 이 진단은 1회만 표시됩니다.");
            }
        }
    }
}
