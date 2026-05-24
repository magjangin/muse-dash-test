using Il2Cpp;
using Il2CppAssets.Scripts.Database;
using Il2CppAssets.Scripts.Database.DataClass;
using Il2CppAssets.Scripts.PeroTools.Managers;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace muse_dash_test
{
    internal static class CustomTagLinkProbe
    {
        public static void DumpAtLinkTime(MusicTagManager manager, List<string> musicList)
        {
            try
            {
                MelonLogger.Msg($"[커스텀 태그 LinkProbe] InitAlbumTagInfo 안에서 0-0 연결 직후 호출 가능 항목 덤프 시작 | managerNull={manager == null} | musicList=[{string.Join(", ", musicList)}]");

                DumpCallableMethods("[커스텀 태그 LinkProbe] MusicTagManager callable", typeof(MusicTagManager), 160);
                DumpCallableMethods("[커스텀 태그 LinkProbe] dbMusicTag callable", GlobalDataBase.dbMusicTag?.GetType(), 180);
                DumpCallableMethods("[커스텀 태그 LinkProbe] AlbumTagInfo callable", typeof(AlbumTagInfo), 160);
                DumpCallableMethods("[커스텀 태그 LinkProbe] MusicButtonCell callable", typeof(Il2CppAssets.Scripts.UI.Panels.PnlMusicTag.MusicButtonCell), 160);

                MelonLogger.Msg($"[커스텀 태그 LinkProbe] 연결 직후 DB 상태 | dbNull={GlobalDataBase.dbMusicTag == null} | stageShowMusicCount={GlobalDataBase.dbMusicTag?.stageShowMusicCount ?? -1}");
                LogStringList("[커스텀 태그 LinkProbe] 연결 직후 stageShowMusicList", GlobalDataBase.dbMusicTag?.stageShowMusicList, 12);

                var cells = UnityEngine.Object.FindObjectsOfType<Il2CppAssets.Scripts.UI.Panels.PnlMusicTag.MusicButtonCell>();
                MelonLogger.Msg($"[커스텀 태그 LinkProbe] 연결 직후 씬의 MusicButtonCell 인스턴스 수={cells?.Length ?? -1}");

                if (cells == null || cells.Length == 0)
                {
                    MelonLogger.Msg("[커스텀 태그 LinkProbe] 연결 직후 호출할 MusicButtonCell 인스턴스가 없습니다.");
                    return;
                }

                for (int i = 0; i < cells.Length; i++)
                {
                    var cell = cells[i];
                    if (cell == null)
                        continue;

                    var musicInfo = cell.musicInfo;
                    string uid = musicInfo != null ? musicInfo.uid : "(unknown)";
                    string name = musicInfo != null ? musicInfo.name : "(unknown)";
                    string gameObjectName = cell.gameObject != null ? cell.gameObject.name : "(null)";
                    MelonLogger.Msg($"[커스텀 태그 LinkProbe] cell[{i}] GameObject={gameObjectName} | Uid={uid} | Name={name} | ActiveSelf={cell.gameObject?.activeSelf} | ActiveHierarchy={cell.gameObject?.activeInHierarchy}");
                }

                MelonLogger.Msg("[커스텀 태그 LinkProbe] 연결 직후 호출 가능 항목 덤프 완료");
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[커스텀 태그 LinkProbe] 연결 직후 덤프 예외: {ex}");
            }
        }

        private static void DumpCallableMethods(string label, Type type, int max)
        {
            try
            {
                if (type == null)
                {
                    MelonLogger.Msg($"{label}: type=null");
                    return;
                }

                var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly);
                MelonLogger.Msg($"{label}: Type={type.FullName}, Count={methods.Length}, MaxLog={max}");

                int logged = 0;
                for (int i = 0; i < methods.Length && logged < max; i++)
                {
                    var method = methods[i];
                    if (method == null || method.IsSpecialName)
                        continue;

                    MelonLogger.Msg($"{label}[{logged}]: {FormatMethod(method)}");
                    logged++;
                }

                if (methods.Length > logged)
                {
                    MelonLogger.Msg($"{label}: ... omitted={methods.Length - logged}");
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"{label} 덤프 예외: {ex}");
            }
        }

        private static string FormatMethod(MethodInfo method)
        {
            var parameters = method.GetParameters();
            var sb = new System.Text.StringBuilder();

            sb.Append(method.ReturnType.Name);
            sb.Append(' ');
            sb.Append(method.Name);
            sb.Append('(');

            for (int i = 0; i < parameters.Length; i++)
            {
                if (i > 0) sb.Append(", ");
                sb.Append(parameters[i].ParameterType.Name);
                sb.Append(' ');
                sb.Append(parameters[i].Name);
            }

            sb.Append(')');
            return sb.ToString();
        }

        private static void LogStringList(string label, Il2CppSystem.Collections.Generic.List<string> list, int max)
        {
            if (list == null)
            {
                MelonLogger.Msg($"{label}: null");
                return;
            }

            var sb = new System.Text.StringBuilder();
            int count = Math.Min(list.Count, max);
            for (int i = 0; i < count; i++)
            {
                if (i > 0) sb.Append(", ");
                sb.Append(list[i]);
            }
            if (list.Count > count) sb.Append(", ...");
            MelonLogger.Msg($"{label}: Count={list.Count}, Values=[{sb}]");
        }
    }
}
