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
    /// 팩 이름 추적 2차. 1차에서 라벨(<c>ImgAlbumTittle</c>)이
    /// <c>DBConfigLocalAlbums.GetLocalTitleByIndex</c>의 결과를 쓴다는 것까지는 확인했는데,
    /// 그 메서드가 <b>두 번</b> 불리고 인덱스가 서로 다릅니다(1999 → null, 그 다음 0 → '기본 패키지').
    ///
    /// 가상 곡의 실제 <c>albumIndex</c> 필드가 0이라는 진단이 이미 있어서
    /// (마스크에만 1999이 들어가 있음), 라벨이 쓰는 쪽은 0일 가능성이 큽니다.
    /// 그렇다면 1999에 답해 주는 패치는 라벨에 닿지 않습니다.
    ///
    /// 그래서 조회 결과와 라벨에 실제로 꽂히는 문자열만 남깁니다. 확인되면 지웁니다.
    /// </summary>
    internal static class AlbumTitleTrace
    {
        internal const string Tag = "[AlbumTrace]";

        internal static bool ShouldTrace()
        {
            try
            {
                return CustomContentIds.IsVirtualSong(CustomPlaySession.Current.LastKnownMusicUid);
            }
            catch (Exception)
            {
                return false;
            }
        }

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
    }

    [HarmonyPatch(typeof(DBConfigLocalAlbums), nameof(DBConfigLocalAlbums.GetLocalTitleByIndex), new Type[] { typeof(int) })]
    internal static class DBConfigLocalAlbums_GetLocalTitleByIndex_Trace
    {
        private static void Postfix(int index, string __result)
        {
            try
            {
                if (!AlbumTitleTrace.ShouldTrace()) return;
                MelonLogger.Msg($"{AlbumTitleTrace.Tag} GetLocalTitleByIndex({index}) → '{__result ?? "(null)"}'");
            }
            catch (Exception ex) { MelonLogger.Error($"{AlbumTitleTrace.Tag} 조회 추적 예외: {ex.Message}"); }
        }
    }

    [HarmonyPatch(typeof(LongSongNameController), nameof(LongSongNameController.RefreshText), new Type[] { typeof(string) })]
    internal static class LongSongNameController_RefreshText_Trace
    {
        private static void Prefix(LongSongNameController __instance, string txt)
        {
            try
            {
                if (!AlbumTitleTrace.ShouldTrace()) return;

                string path = AlbumTitleTrace.PathOf(__instance);
                if (path.IndexOf("Album", StringComparison.OrdinalIgnoreCase) < 0) return;

                MelonLogger.Msg($"{AlbumTitleTrace.Tag} 라벨에 꽂힘: '{txt ?? "(null)"}' ← {path}");
            }
            catch (Exception ex) { MelonLogger.Error($"{AlbumTitleTrace.Tag} 라벨 추적 예외: {ex.Message}"); }
        }
    }
}
