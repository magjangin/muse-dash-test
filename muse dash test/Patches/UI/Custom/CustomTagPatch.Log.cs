using MelonLoader;
using Il2CppAssets.Scripts.Database;
using Il2CppAssets.Scripts.Database.DataClass;
using System;
using System.Reflection;

using static Il2CppAssets.Scripts.Database.DBConfigCustomTags;

namespace muse_dash_test
{
    internal partial class CustomTagPatch
    {
        internal partial class MusicTagPatch
        {
            private static void LogCoverCandidates(AlbumTagInfo tagInfo, DBConfigAlbums.AlbumsInfo albumInfo, CustomTagInfo customInfo)
            {
                try
                {
                    MelonLogger.Msg($"[CoverProbe] AlbumTagInfo: tagUid={tagInfo?.tagUid ?? "(null)"}, name={tagInfo?.name ?? "(null)"}, iconName={tagInfo?.iconName ?? "(null)"}");
                    MelonLogger.Msg($"[CoverProbe] CustomTagInfo: tag_picture={customInfo?.tag_picture ?? "(null)"}");

                    if (albumInfo == null)
                    {
                        MelonLogger.Msg("[CoverProbe] AlbumsInfo: (null)");
                        return;
                    }

                    MelonLogger.Msg($"[CoverProbe] AlbumsInfo direct: uid={albumInfo.uid ?? "(null)"}, title={albumInfo.title ?? "(null)"}, tag={albumInfo.tag ?? "(null)"}, jsonName={albumInfo.jsonName ?? "(null)"}, prefabsName={albumInfo.prefabsName ?? "(null)"}");

                    var type = albumInfo.GetType();
                    foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
                    {
                        if (!prop.CanRead || prop.GetIndexParameters().Length != 0)
                            continue;

                        string name = prop.Name ?? string.Empty;
                        if (!LooksCoverRelated(name))
                            continue;

                        object value = SafeRead(() => prop.GetValue(albumInfo));
                        MelonLogger.Msg($"[CoverProbe] AlbumsInfo prop {name}={FormatValue(value)}");
                    }

                    foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
                    {
                        string name = field.Name ?? string.Empty;
                        if (!LooksCoverRelated(name))
                            continue;

                        object value = SafeRead(() => field.GetValue(albumInfo));
                        MelonLogger.Msg($"[CoverProbe] AlbumsInfo field {name}={FormatValue(value)}");
                    }
                }
                catch (Exception ex)
                {
                    MelonLogger.Warning($"[CoverProbe] 커버 후보 로그 예외: {ex.Message}");
                }
            }

            private static void LogMusicInfoDump(string label, Il2CppAssets.Scripts.Database.MusicInfo info)
            {
                try
                {
                    if (info == null)
                    {
                        MelonLogger.Msg($"{label}: (null)");
                        return;
                    }

                    MelonLogger.Msg($"{label}: uid={info.uid ?? "(null)"}, name={info.name ?? "(null)"}, author={info.author ?? "(null)"}, levelDesigner={info.levelDesigner ?? "(null)"}, cover={info.cover ?? "(null)"}, noteJson={info.noteJson ?? "(null)"}, music={info.music ?? "(null)"}, albumUidName={info.albumUidName ?? "(null)"}, albumJsonName={info.albumJsonName ?? "(null)"}, albumIndex={info.albumIndex}");

                    var type = info.GetType();
                    foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
                    {
                        if (!prop.CanRead || prop.GetIndexParameters().Length != 0)
                            continue;

                        object value = SafeRead(() => prop.GetValue(info));
                        MelonLogger.Msg($"{label}: prop {prop.Name}={FormatValue(value)}");
                    }
                }
                catch (Exception ex)
                {
                    MelonLogger.Error($"{label} 덤프 예외: {ex}");
                }
            }

            private static void LogNestedMusicExInfo(string label, Il2CppAssets.Scripts.Database.MusicInfo info)
            {
                try
                {
                    var musicExInfo = GetMemberValue(info, "m_MusicExInfo") ?? GetMemberValue(info, "MusicExInfo");
                    if (musicExInfo == null)
                    {
                        MelonLogger.Msg($"{label}: (null)");
                        return;
                    }

                    var type = musicExInfo.GetType();
                    MelonLogger.Msg($"{label}: type={type.FullName}");

                    foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
                    {
                        if (!prop.CanRead || prop.GetIndexParameters().Length != 0)
                            continue;

                        string name = prop.Name ?? string.Empty;
                        if (name.IndexOf("album", StringComparison.OrdinalIgnoreCase) < 0 && name.IndexOf("cover", StringComparison.OrdinalIgnoreCase) < 0)
                            continue;

                        object value = SafeRead(() => prop.GetValue(musicExInfo));
                        MelonLogger.Msg($"{label}: prop {name}={FormatValue(value)}");
                    }

                    foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
                    {
                        string name = field.Name ?? string.Empty;
                        if (name.IndexOf("album", StringComparison.OrdinalIgnoreCase) < 0 && name.IndexOf("cover", StringComparison.OrdinalIgnoreCase) < 0)
                            continue;

                        object value = SafeRead(() => field.GetValue(musicExInfo));
                        MelonLogger.Msg($"{label}: field {name}={FormatValue(value)}");
                    }
                }
                catch (Exception ex)
                {
                    MelonLogger.Warning($"{label} 덤프 예외: {ex.Message}");
                }
            }

            private static bool LooksCoverRelated(string name)
            {
                return name.IndexOf("cover", StringComparison.OrdinalIgnoreCase) >= 0 ||
                    name.IndexOf("prefab", StringComparison.OrdinalIgnoreCase) >= 0 ||
                    name.IndexOf("pic", StringComparison.OrdinalIgnoreCase) >= 0 ||
                    name.IndexOf("icon", StringComparison.OrdinalIgnoreCase) >= 0 ||
                    name.IndexOf("json", StringComparison.OrdinalIgnoreCase) >= 0;
            }

            private static object SafeRead(Func<object> read)
            {
                try
                {
                    return read();
                }
                catch (Exception ex)
                {
                    return "(error: " + ex.Message + ")";
                }
            }

            private static string FormatValue(object value)
            {
                return value == null ? "(null)" : value.ToString();
            }
        }
    }
}