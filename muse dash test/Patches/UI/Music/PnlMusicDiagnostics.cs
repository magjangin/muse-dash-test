using MelonLoader;
using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace muse_dash_test
{
    /// <summary>
    /// 곡 정보의 추출(Read-only), 로깅, 디버그 덤프 등의 진단 로직을 담당하는 클래스입니다.
    /// 책임별로 다음 partial 파일들로 분리되어 있습니다:
    ///   - PnlMusicDiagnostics.Extraction.cs : 인스턴스 멤버에서 곡 정보 추출
    ///   - PnlMusicDiagnostics.AudioClip.cs  : AudioClip/AudioSource 기반 음악 클립 탐색
    /// </summary>
    public static partial class PnlMusicDiagnostics
    {
        public class MusicInfo
        {
            public string Title;
            public string Clip;
            public string Artist;
            public string LevelDesigner;
            public string ClipReason;
        }

        public static IEnumerator DelayedApplyPrepMusicInfo(object pnlInstance, string source, float delaySeconds)
        {
            yield return new WaitForSeconds(delaySeconds);
            ApplyPrepMusicInfo(pnlInstance, source);
        }

        public static void ApplyPrepMusicInfo(object pnlInstance, string source = "PnlPreparation.Awake")
        {
            try
            {
                string lastKnownUid = CustomPlaySession.Current.LastKnownMusicUid;
                string resolvedUid = ResolveCustomMusicUid(pnlInstance, lastKnownUid);
                if (!string.IsNullOrEmpty(resolvedUid))
                {
                    PnlMusicOverride.ApplySongTitleOverride(source, pnlInstance, resolvedUid);
                }

                // 여기서부터 화면에 쓰는 일은 "패널에서 쓸 만한 제목을 못 읽었으면 PnlStage에도 제목을 덮어쓴다" 하나뿐이고,
                // 나머지(클립·아티스트·레벨 디자이너 추출)는 LogCompact 한 줄을 위한 관찰입니다.
                // 관찰은 패널 멤버 전체 리플렉션과 씬 AudioSource 검색까지 가므로 Verbose가 꺼져 있으면 하지 않고
                // 제목만 읽습니다. 준비 화면은 OnEnable/RefreshUi와 0.25초·1초 지연 적용으로 이 자리를 여러 번 지납니다.
                bool diagnose = ModLogger.IsLevelEnabled(ModLogLevel.Verbose);

                // 지금 곡이 순정 곡으로 확인됐으면 PnlStage에서 다시 해석해도 같은 답(없음)이라 덮어쓸 것이 없습니다.
                bool knownOfficialSong = !string.IsNullOrEmpty(lastKnownUid) && string.IsNullOrEmpty(resolvedUid);
                if (!diagnose && knownOfficialSong) return;

                MusicInfo info = diagnose ? ExtractMusicInfo(pnlInstance, resolvedUid) : null;
                string title = diagnose ? info.Title : ExtractTitle(pnlInstance);
                if (!IsUsefulTitle(title))
                {
                    var stage = FindLivePnlStage();
                    if (stage != null)
                    {
                        string stageResolvedUid = ResolveCustomMusicUid(stage, lastKnownUid);
                        if (!string.IsNullOrEmpty(stageResolvedUid))
                        {
                            PnlMusicOverride.ApplySongTitleOverride(source + "->PnlStage", stage, stageResolvedUid);
                        }
                        if (diagnose)
                        {
                            var stageInfo = ExtractMusicInfo(stage, stageResolvedUid);
                            if (IsUsefulTitle(stageInfo.Title)) info.Title = stageInfo.Title;
                            if (!string.IsNullOrWhiteSpace(stageInfo.Clip)) info.Clip = stageInfo.Clip;
                            if (!string.IsNullOrWhiteSpace(stageInfo.Artist)) info.Artist = stageInfo.Artist;
                            if (!string.IsNullOrWhiteSpace(stageInfo.LevelDesigner)) info.LevelDesigner = stageInfo.LevelDesigner;
                            if (string.IsNullOrWhiteSpace(info.ClipReason) || info.ClipReason == "AudioClip 후보 없음")
                                info.ClipReason = stageInfo.ClipReason;
                        }
                    }
                }
                if (diagnose) LogCompact(source, info);
            }
            catch (Exception ex) { ModLogger.Error($"ApplyPrepMusicInfo 예외: {ex}"); }
        }

        public static string ResolveCustomMusicUid(object pnlInstance)
        {
            return ResolveCustomMusicUid(pnlInstance, CustomPlaySession.Current.LastKnownMusicUid);
        }

        /// <summary>
        /// <paramref name="selected"/>는 호출자가 이미 읽어 둔 <c>LastKnownMusicUid</c>입니다.
        /// 그 프로퍼티는 비어 있을 때 씬을 뒤지므로, 한 번의 적용 안에서 두 번 읽지 않도록 받아 씁니다.
        /// </summary>
        private static string ResolveCustomMusicUid(object pnlInstance, string selected)
        {
            if (!string.IsNullOrEmpty(selected))
            {
                return CustomContentIds.IsVirtualSong(selected) ? selected : null;
            }

            // 마지막 클릭 값은 LastKnownMusicUid가 이미 봤습니다. 여기까지 왔으면 패널 안에서 직접 찾습니다.
            string uid = TryFindCustomMusicUidInObject(pnlInstance, 0, new HashSet<object>());
            return string.IsNullOrEmpty(uid) ? null : uid;
        }

        private static string TryFindCustomMusicUidInObject(object obj, int depth, HashSet<object> visited)
        {
            if (obj == null || depth > 2) return null;
            if (obj is string text) return CustomContentIds.IsVirtualSong(text) ? text : null;
            if (obj is UnityEngine.Object unityObject && !unityObject) return null;
            if (!visited.Add(obj)) return null;

            try
            {
                if (obj is Il2CppAssets.Scripts.Database.MusicInfo musicInfo && CustomContentIds.IsVirtualSong(musicInfo.uid))
                {
                    return musicInfo.uid;
                }

                Type type = obj.GetType();
                foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
                {
                    string uid = TryReadCustomMusicUid(field.FieldType, () => field.GetValue(obj), depth, visited);
                    if (!string.IsNullOrEmpty(uid)) return uid;
                }

                foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
                {
                    if (!property.CanRead || property.GetIndexParameters().Length != 0) continue;
                    string uid = TryReadCustomMusicUid(property.PropertyType, () => property.GetValue(obj), depth, visited);
                    if (!string.IsNullOrEmpty(uid)) return uid;
                }
            }
            catch (Exception) { }

            return null;
        }

        private static string TryReadCustomMusicUid(Type memberType, Func<object> read, int depth, HashSet<object> visited)
        {
            try
            {
                bool promisingType = memberType == typeof(string)
                    || memberType == typeof(Il2CppAssets.Scripts.Database.MusicInfo)
                    || IsMusicLike(memberType);
                if (!promisingType) return null;

                object value = read();
                if (value == null) return null;

                if (value is string text)
                {
                    return CustomContentIds.IsVirtualSong(text) ? text : null;
                }

                if (value is Il2CppAssets.Scripts.Database.MusicInfo musicInfo)
                {
                    return CustomContentIds.IsVirtualSong(musicInfo.uid) ? musicInfo.uid : null;
                }

                return TryFindCustomMusicUidInObject(value, depth + 1, visited);
            }
            catch
            {
                return null;
            }
        }

        private static void LogCompact(string source, MusicInfo info)
        {
            string uid = CustomPlaySession.Current.LastKnownMusicUid;
            if (string.IsNullOrEmpty(uid)) uid = "(unknown)";
            string clip = Clean(info.Clip);
            string reason = string.IsNullOrWhiteSpace(info.Clip) ? $", 클립 사유={Clean(info.ClipReason)}" : "";
            ModLogger.Verbose($"{source}: 곡 이름={Clean(info.Title)}, UID={uid}, 음악 클립={clip}, 아티스트 이름={Clean(info.Artist)}, 레벨 디자이너=레벨 디자이너, 실제 이름={Clean(info.LevelDesigner)}{reason}");
        }

        private static string Clean(string value)
        {
            return string.IsNullOrWhiteSpace(value) ? "(unknown)" : value.Trim();
        }

        private static string FirstNonEmpty(params string[] values)
        {
            foreach (var value in values)
            {
                if (!string.IsNullOrWhiteSpace(value)) return value.Trim();
            }
            return null;
        }

        private static bool IsUsefulTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title)) return false;
            title = title.Trim();
            if (title.StartsWith("Pnl", StringComparison.OrdinalIgnoreCase)) return false;
            if (title.StartsWith("Img", StringComparison.OrdinalIgnoreCase)) return false;
            if (title.StartsWith("Txt", StringComparison.OrdinalIgnoreCase)) return false;
            if (title.Equals("MusicRoot", StringComparison.OrdinalIgnoreCase)) return false;
            if (title.Equals("FancyScrollViewMusic", StringComparison.OrdinalIgnoreCase)) return false;
            return true;
        }

        private static bool IsMusicLike(Type t)
        {
            var name = t.Name.ToLowerInvariant();
            if (name.Contains("music") || name.Contains("song") || name.Contains("track") || name.Contains("bms") || name.Contains("audio") || name.Contains("bgm") || name.Contains("album")) return true;
            return false;
        }
    }
}
