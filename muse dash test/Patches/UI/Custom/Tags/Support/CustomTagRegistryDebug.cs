using MelonLoader;
using Il2CppAssets.Scripts.Database;
using Il2CppAssets.Scripts.Database.DataClass;
using System;
using System.Reflection;

namespace muse_dash_test
{
    internal static class CustomTagRegistryDebug
    {
        /// <summary>
        /// 복제한 <c>MusicExInfo</c>의 앨범·커버 필드를 한 줄씩 열거합니다. 곡마다 5줄이 나와
        /// 시작 로그를 덮었으므로 Verbose 전용이고, 꺼져 있으면 리플렉션 순회 자체를 건너뜁니다.
        /// </summary>
        internal static void LogNestedMusicExInfo(string label, MusicInfo info)
        {
            if (!ModLogger.IsLevelEnabled(ModLogLevel.Verbose)) return;

            try
            {
                var musicExInfo = ModReflection.GetValue(info, "m_MusicExInfo", silent: true) ?? ModReflection.GetValue(info, "MusicExInfo", silent: true);
                if (musicExInfo == null)
                {
                    ModLogger.Verbose($"{label}: (null)");
                    return;
                }

                var type = musicExInfo.GetType();
                ModLogger.Verbose($"{label}: type={type.FullName}");

                foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
                {
                    if (!prop.CanRead || prop.GetIndexParameters().Length != 0) continue;
                    string name = prop.Name ?? string.Empty;
                    if (name.IndexOf("album", StringComparison.OrdinalIgnoreCase) < 0 && name.IndexOf("cover", StringComparison.OrdinalIgnoreCase) < 0) continue;

                    object value = SafeRead(() => prop.GetValue(musicExInfo));
                    ModLogger.Verbose($"{label}: prop {name}={FormatValue(value)}");
                }

                foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
                {
                    string name = field.Name ?? string.Empty;
                    if (name.IndexOf("album", StringComparison.OrdinalIgnoreCase) < 0 && name.IndexOf("cover", StringComparison.OrdinalIgnoreCase) < 0) continue;

                    object value = SafeRead(() => field.GetValue(musicExInfo));
                    ModLogger.Verbose($"{label}: field {name}={FormatValue(value)}");
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
