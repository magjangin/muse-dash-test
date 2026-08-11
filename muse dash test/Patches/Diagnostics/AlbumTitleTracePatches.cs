using MelonLoader;
using HarmonyLib;
using Il2Cpp;
using Il2CppAssets.Scripts.Database;
using System;
using System.Text;
using UnityEngine;

namespace muse_dash_test
{
    /// <summary>
    /// 곡 선택 화면의 앨범 이름 라벨이 <b>어디서 값을 받아오는지</b> 추적하는 임시 진단입니다.
    ///
    /// 알아낸 사실: 커스텀 곡(1999-N)이 선택된 상태에서 라벨이 39ms 사이에 두 번 채워지고,
    /// 값이 서로 다릅니다('肥宅快乐包 Vol.2' → '기본 패키지'). 즉 채우는 경로가 하나가 아닙니다.
    ///
    /// 그래서 <b>받는 쪽</b>(LongSongNameController)과 <b>주는 쪽</b>(DBConfigAlbums 조회)을 동시에 찍고,
    /// 로그 순서로 "그 값을 돌려준 조회"를 짚어냅니다. il2cpp 호출자는 스택 트레이스에 안 남기 때문에
    /// 시간 순서가 유일한 단서입니다.
    ///
    /// 원인이 확정되면 이 파일은 지웁니다.
    /// </summary>
    internal static class AlbumTitleTrace
    {
        /// <summary>개발자 전용 스위치. 필요할 때만 켜고 다시 빌드합니다.</summary>
        internal static readonly bool TraceEnabled = true;

        internal const string Tag = "[AlbumTrace]";

        /// <summary>커스텀 곡이 선택돼 있을 때만 찍습니다. 이 경로들은 곡 목록 전체가 함께 쓰는 곳이라 그냥 두면 로그가 넘칩니다.</summary>
        internal static bool ShouldTrace()
        {
            if (!TraceEnabled) return false;

            try
            {
                return CustomContentIds.IsVirtualSong(CustomPlaySession.Current.LastKnownMusicUid);
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>어느 라벨인지 구분하려면 이름만으로는 부족해서 계층 경로째로 남깁니다.</summary>
        internal static string PathOf(Component component)
        {
            try
            {
                if (component == null) return "(null)";

                var transform = component.transform;
                if (transform == null) return "(transform 없음)";

                var sb = new StringBuilder(transform.name);
                var parent = transform.parent;
                while (parent != null)
                {
                    sb.Insert(0, parent.name + "/");
                    parent = parent.parent;
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                return $"(예외:{ex.GetType().Name})";
            }
        }

        internal static string Describe(DBConfigAlbums.AlbumsInfo album)
        {
            try
            {
                if (album == null) return "(null)";

                var wrapper = new AlbumsInfoWrapper(album);
                return $"uid={wrapper.uid ?? "(null)"} title='{wrapper.title ?? "(null)"}' jsonName={wrapper.jsonName ?? "(null)"}";
            }
            catch (Exception ex)
            {
                return $"(예외:{ex.GetType().Name})";
            }
        }
    }

    // ── 받는 쪽: 라벨에 글자가 실제로 꽂히는 지점 ────────────────────────────────

    [HarmonyPatch(typeof(LongSongNameController), nameof(LongSongNameController.RefreshText), new Type[] { typeof(string) })]
    internal static class LongSongNameController_RefreshText_Trace
    {
        private static void Prefix(LongSongNameController __instance, string txt)
        {
            try
            {
                if (!AlbumTitleTrace.ShouldTrace()) return;
                MelonLogger.Msg($"{AlbumTitleTrace.Tag} RefreshText('{txt ?? "(null)"}') ← {AlbumTitleTrace.PathOf(__instance)}");
            }
            catch (Exception ex) { MelonLogger.Error($"{AlbumTitleTrace.Tag} RefreshText 추적 예외: {ex.Message}"); }
        }
    }

    [HarmonyPatch(typeof(LongSongNameController), nameof(LongSongNameController.Refresh), new Type[] { typeof(string), typeof(bool), typeof(float) })]
    internal static class LongSongNameController_Refresh_Trace
    {
        private static void Prefix(LongSongNameController __instance, string text, bool isSpecialFont, float delay)
        {
            try
            {
                if (!AlbumTitleTrace.ShouldTrace()) return;
                MelonLogger.Msg($"{AlbumTitleTrace.Tag} Refresh('{text ?? "(null)"}', special={isSpecialFont}) ← {AlbumTitleTrace.PathOf(__instance)}");
            }
            catch (Exception ex) { MelonLogger.Error($"{AlbumTitleTrace.Tag} Refresh 추적 예외: {ex.Message}"); }
        }
    }

    [HarmonyPatch(typeof(LongSongNameController), nameof(LongSongNameController.RefreshForBestFit), new Type[] { typeof(string), typeof(float) })]
    internal static class LongSongNameController_RefreshForBestFit_Trace
    {
        private static void Prefix(LongSongNameController __instance, string txt, float delay)
        {
            try
            {
                if (!AlbumTitleTrace.ShouldTrace()) return;
                MelonLogger.Msg($"{AlbumTitleTrace.Tag} RefreshForBestFit('{txt ?? "(null)"}') ← {AlbumTitleTrace.PathOf(__instance)}");
            }
            catch (Exception ex) { MelonLogger.Error($"{AlbumTitleTrace.Tag} RefreshForBestFit 추적 예외: {ex.Message}"); }
        }
    }

    // ── 주는 쪽: 앨범 정보 조회 여섯 갈래 ───────────────────────────────────────

    [HarmonyPatch(typeof(DBConfigAlbums), nameof(DBConfigAlbums.GetAlbumInfoByMusicInfo))]
    internal static class DBConfigAlbums_GetAlbumInfoByMusicInfo_Trace
    {
        private static void Postfix(MusicInfo musicInfo, DBConfigAlbums.AlbumsInfo __result)
        {
            try
            {
                if (!AlbumTitleTrace.ShouldTrace()) return;
                string uid = musicInfo != null ? (musicInfo.uid ?? "(null)") : "(musicInfo null)";
                MelonLogger.Msg($"{AlbumTitleTrace.Tag} GetAlbumInfoByMusicInfo({uid}) → {AlbumTitleTrace.Describe(__result)}");
            }
            catch (Exception ex) { MelonLogger.Error($"{AlbumTitleTrace.Tag} GetAlbumInfoByMusicInfo 추적 예외: {ex.Message}"); }
        }
    }

    [HarmonyPatch(typeof(DBConfigAlbums), nameof(DBConfigAlbums.GetAlbumsInfoByUid))]
    internal static class DBConfigAlbums_GetAlbumsInfoByUid_Trace
    {
        private static void Postfix(string uid, DBConfigAlbums.AlbumsInfo __result)
        {
            try
            {
                if (!AlbumTitleTrace.ShouldTrace()) return;
                MelonLogger.Msg($"{AlbumTitleTrace.Tag} GetAlbumsInfoByUid('{uid ?? "(null)"}') → {AlbumTitleTrace.Describe(__result)}");
            }
            catch (Exception ex) { MelonLogger.Error($"{AlbumTitleTrace.Tag} GetAlbumsInfoByUid 추적 예외: {ex.Message}"); }
        }
    }

    [HarmonyPatch(typeof(DBConfigAlbums), nameof(DBConfigAlbums.GetAlbumInfoByIndex))]
    internal static class DBConfigAlbums_GetAlbumInfoByIndex_Trace
    {
        private static void Postfix(int index, DBConfigAlbums.AlbumsInfo __result)
        {
            try
            {
                if (!AlbumTitleTrace.ShouldTrace()) return;
                MelonLogger.Msg($"{AlbumTitleTrace.Tag} GetAlbumInfoByIndex({index}) → {AlbumTitleTrace.Describe(__result)}");
            }
            catch (Exception ex) { MelonLogger.Error($"{AlbumTitleTrace.Tag} GetAlbumInfoByIndex 추적 예외: {ex.Message}"); }
        }
    }

    [HarmonyPatch(typeof(DBConfigAlbums), nameof(DBConfigAlbums.GetAlbumInfoByAlbumJsonIndex))]
    internal static class DBConfigAlbums_GetAlbumInfoByAlbumJsonIndex_Trace
    {
        private static void Postfix(int albumIndex, DBConfigAlbums.AlbumsInfo __result)
        {
            try
            {
                if (!AlbumTitleTrace.ShouldTrace()) return;
                MelonLogger.Msg($"{AlbumTitleTrace.Tag} GetAlbumInfoByAlbumJsonIndex({albumIndex}) → {AlbumTitleTrace.Describe(__result)}");
            }
            catch (Exception ex) { MelonLogger.Error($"{AlbumTitleTrace.Tag} GetAlbumInfoByAlbumJsonIndex 추적 예외: {ex.Message}"); }
        }
    }

    [HarmonyPatch(typeof(DBConfigAlbums), nameof(DBConfigAlbums.GetAlbumIndexByUid))]
    internal static class DBConfigAlbums_GetAlbumIndexByUid_Trace
    {
        private static void Postfix(string uid, int __result)
        {
            try
            {
                if (!AlbumTitleTrace.ShouldTrace()) return;
                MelonLogger.Msg($"{AlbumTitleTrace.Tag} GetAlbumIndexByUid('{uid ?? "(null)"}') → {__result}");
            }
            catch (Exception ex) { MelonLogger.Error($"{AlbumTitleTrace.Tag} GetAlbumIndexByUid 추적 예외: {ex.Message}"); }
        }
    }

    [HarmonyPatch(typeof(DBConfigAlbums), nameof(DBConfigAlbums.GetAlbumJsonIndexByUid))]
    internal static class DBConfigAlbums_GetAlbumJsonIndexByUid_Trace
    {
        private static void Postfix(string uid, int __result)
        {
            try
            {
                if (!AlbumTitleTrace.ShouldTrace()) return;
                MelonLogger.Msg($"{AlbumTitleTrace.Tag} GetAlbumJsonIndexByUid('{uid ?? "(null)"}') → {__result}");
            }
            catch (Exception ex) { MelonLogger.Error($"{AlbumTitleTrace.Tag} GetAlbumJsonIndexByUid 추적 예외: {ex.Message}"); }
        }
    }
}
