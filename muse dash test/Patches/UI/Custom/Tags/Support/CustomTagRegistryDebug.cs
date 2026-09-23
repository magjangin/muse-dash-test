using MelonLoader;
using Il2CppAssets.Scripts.Database;
using Il2CppAssets.Scripts.Database.DataClass;
using System;
using System.Reflection;

namespace muse_dash_test
{
    internal static class CustomTagRegistryDebug
    {
        internal static void LogNestedMusicExInfo(string label, MusicInfo info)
        {
            try
            {
                var musicExInfo = ModReflection.GetValue(info, "m_MusicExInfo", silent: true) ?? ModReflection.GetValue(info, "MusicExInfo", silent: true);
                if (musicExInfo == null)
                {
                    ModLogger.Msg($"{label}: (null)");
                    return;
                }

                var type = musicExInfo.GetType();
                ModLogger.Msg($"{label}: type={type.FullName}");

                foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
                {
                    if (!prop.CanRead || prop.GetIndexParameters().Length != 0) continue;
                    string name = prop.Name ?? string.Empty;
                    if (name.IndexOf("album", StringComparison.OrdinalIgnoreCase) < 0 && name.IndexOf("cover", StringComparison.OrdinalIgnoreCase) < 0) continue;

                    object value = SafeRead(() => prop.GetValue(musicExInfo));
                    ModLogger.Msg($"{label}: prop {name}={FormatValue(value)}");
                }

                foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
                {
                    string name = field.Name ?? string.Empty;
                    if (name.IndexOf("album", StringComparison.OrdinalIgnoreCase) < 0 && name.IndexOf("cover", StringComparison.OrdinalIgnoreCase) < 0) continue;

                    object value = SafeRead(() => field.GetValue(musicExInfo));
                    ModLogger.Msg($"{label}: field {name}={FormatValue(value)}");
                }
            }
            catch (Exception ex)
            {
                ModLogger.Warning($"{label} 덤프 예외: {ex.Message}");
            }
        }

        internal static object SafeRead(Func<object> read)
        {
            try { return read(); }
            catch (Exception ex) { return "(error: " + ex.Message + ")"; }
        }

        internal static string FormatValue(object value)
        {
            return value == null ? "(null)" : value.ToString();
        }
    }
}
