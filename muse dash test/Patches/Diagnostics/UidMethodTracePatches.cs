using HarmonyLib;
using Il2Cpp;
using Il2CppAssets.Scripts.Database;
using Il2CppAssets.Scripts.Database.DataClass;
using MelonLoader;
using System;
using System.Reflection;
using System.Text;
using Il2CppStringList = Il2CppSystem.Collections.Generic.List<string>;

namespace muse_dash_test
{
    internal static class UidRewriteExperiment
    {
        public const string SourceUid = CustomContentIds.FallbackSourceMusicUid;
        public const string TargetUid = CustomContentIds.VirtualSongPrefix + "0";
        public const string TargetDisplayName = "화영왕 0";

        public static string MusicInfo(MusicInfo info)
        {
            if (info == null)
            {
                return "(null)";
            }

            try
            {
                return $"uid={info.uid ?? "(null)"}, name={info.name ?? "(null)"}, cover={info.cover ?? "(null)"}, hexieCover={info.hexieCover ?? "(null)"}, coverName={info.coverName ?? "(null)"}";
            }
            catch (Exception ex)
            {
                return $"(music info read error: {ex.Message})";
            }
        }

        public static string StringList(Il2CppStringList list)
        {
            if (list == null)
            {
                return "(null)";
            }

            try
            {
                int take = Math.Min(list.Count, 6);
                var values = new string[take];
                for (int i = 0; i < take; i++)
                {
                    values[i] = list[i] ?? "(null)";
                }

                string suffix = list.Count > take ? ", ..." : string.Empty;
                return $"count={list.Count}, values=[{string.Join(", ", values)}{suffix}]";
            }
            catch (Exception ex)
            {
                return $"(list read error: {ex.Message})";
            }
        }

        public static string PropertyDump(MusicInfo info)
        {
            if (info == null)
            {
                return "(null)";
            }

            try
            {
                var sb = new StringBuilder();
                var props = info.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                foreach (var prop in props)
                {
                    if (!prop.CanRead || prop.GetIndexParameters().Length != 0)
                    {
                        continue;
                    }

                    Type propType = prop.PropertyType;
                    if (propType != typeof(string) && propType != typeof(int) && propType != typeof(bool) && propType != typeof(float) && propType != typeof(double))
                    {
                        continue;
                    }

                    object value;
                    try
                    {
                        value = prop.GetValue(info);
                    }
                    catch (Exception ex)
                    {
                        value = "(error: " + ex.Message + ")";
                    }

                    if (sb.Length > 0)
                    {
                        sb.Append(", ");
                    }
                    sb.Append(prop.Name);
                    sb.Append('=');
                    sb.Append(value ?? "(null)");
                }

                return sb.Length == 0 ? "(no scalar properties)" : sb.ToString();
            }
            catch (Exception ex)
            {
                return "(property dump error: " + ex.Message + ")";
            }
        }

        public static string PropertyDump(object instance)
        {
            if (instance == null)
            {
                return "(null)";
            }

            try
            {
                var sb = new StringBuilder();
                var props = instance.GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                foreach (var prop in props)
                {
                    if (!prop.CanRead || prop.GetIndexParameters().Length != 0)
                    {
                        continue;
                    }

                    object value;
                    try
                    {
                        value = prop.GetValue(instance);
                    }
                    catch (Exception ex)
                    {
                        value = "(error: " + ex.Message + ")";
                    }

                    if (sb.Length > 0)
                    {
                        sb.Append(", ");
                    }

                    sb.Append(prop.Name);
                    sb.Append('=');
                    sb.Append(value ?? "(null)");
                }

                return sb.Length == 0 ? "(no properties)" : sb.ToString();
            }
            catch (Exception ex)
            {
                return "(property dump error: " + ex.Message + ")";
            }
        }

        public static string TextPropertyDump(object instance)
        {
            if (instance == null)
            {
                return "(null)";
            }

            try
            {
                var sb = new StringBuilder();
                var type = instance.GetType();

                foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
                {
                    if (!prop.CanRead || prop.GetIndexParameters().Length != 0)
                    {
                        continue;
                    }

                    if (prop.PropertyType != typeof(string) && !prop.Name.Equals("text", StringComparison.OrdinalIgnoreCase) && !prop.Name.Equals("m_Text", StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    try
                    {
                        var value = prop.GetValue(instance) as string;
                        if (!string.IsNullOrEmpty(value))
                        {
                            if (sb.Length > 0)
                            {
                                sb.Append(", ");
                            }

                            sb.Append(prop.Name);
                            sb.Append('=');
                            sb.Append(value);
                        }
                    }
                    catch (Exception ex)
                    {
                        if (sb.Length > 0)
                        {
                            sb.Append(", ");
                        }

                        sb.Append(prop.Name);
                        sb.Append("=(error: ");
                        sb.Append(ex.Message);
                        sb.Append(")");
                    }
                }

                foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
                {
                    if (field.FieldType != typeof(string) && !field.Name.Equals("text", StringComparison.OrdinalIgnoreCase) && !field.Name.Equals("m_Text", StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    try
                    {
                        var value = field.GetValue(instance) as string;
                        if (!string.IsNullOrEmpty(value))
                        {
                            if (sb.Length > 0)
                            {
                                sb.Append(", ");
                            }

                            sb.Append(field.Name);
                            sb.Append('=');
                            sb.Append(value);
                        }
                    }
                    catch (Exception ex)
                    {
                        if (sb.Length > 0)
                        {
                            sb.Append(", ");
                        }

                        sb.Append(field.Name);
                        sb.Append("=(error: ");
                        sb.Append(ex.Message);
                        sb.Append(")");
                    }
                }

                return sb.Length == 0 ? "(no text properties)" : sb.ToString();
            }
            catch (Exception ex)
            {
                return "(text property dump error: " + ex.Message + ")";
            }
        }

        public static string DescribeSetSelectedMusicNameTxt(Il2Cpp.SetSelectedMusicNameTxt instance)
        {
            if (instance == null)
            {
                return "(null)";
            }

            try
            {
                object txtValue = null;
                object longCtrlValue = null;
                object isMusicNameValue = null;
                object isMusicAuthorValue = null;

                try { txtValue = instance.GetType().GetProperty("txt", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(instance); } catch { }
                try { longCtrlValue = instance.GetType().GetProperty("m_LongCtrl", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(instance); } catch { }
                try { isMusicNameValue = instance.GetType().GetProperty("isMusicName", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(instance); } catch { }
                try { isMusicAuthorValue = instance.GetType().GetProperty("isMusicAuthor", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(instance); } catch { }

                string txtText = "(null)";
                if (txtValue != null)
                {
                    var txtType = txtValue.GetType();
                    var textProp = txtType.GetProperty("text", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                    if (textProp != null)
                    {
                        txtText = textProp.GetValue(txtValue) as string ?? "(null)";
                    }
                }

                string longCtrlName = "(null)";
                if (longCtrlValue is UnityEngine.Object unityObject)
                {
                    longCtrlName = unityObject.name ?? "(null)";
                }
                else if (longCtrlValue != null)
                {
                    longCtrlName = longCtrlValue.ToString();
                }

                return $"txt={txtText}, isMusicName={isMusicNameValue ?? "(null)"}, isMusicAuthor={isMusicAuthorValue ?? "(null)"}, m_LongCtrl={longCtrlName}";
            }
            catch (Exception ex)
            {
                return "(describe error: " + ex.Message + ")";
            }
        }

        public static string GetTargetDisplayName(string uid)
        {
            if (string.IsNullOrEmpty(uid))
            {
                return null;
            }

            if (uid == TargetUid)
            {
                return TargetDisplayName;
            }

            if (uid == CustomContentIds.VirtualSongPrefix + "1")
            {
                return "화영왕 1";
            }

            if (uid == CustomContentIds.VirtualSongPrefix + "2")
            {
                return "화영왕 2";
            }

            return null;
        }

    }

}
