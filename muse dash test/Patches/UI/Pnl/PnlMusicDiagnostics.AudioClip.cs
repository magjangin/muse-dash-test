using System;
using System.Reflection;
using UnityEngine;

namespace muse_dash_test
{
    /// <summary>
    /// AudioClip/AudioSource 멤버에서 현재 재생 중인 음악 클립 이름을 탐색하는 로직.
    /// </summary>
    public static partial class PnlMusicDiagnostics
    {
        private static string FindAudioClipName(object obj, out string reason)
        {
            reason = "AudioClip 후보 없음";
            if (obj == null)
            {
                reason = "대상 인스턴스 없음";
                return null;
            }
            var t = obj.GetType();
            int audioClipMembers = 0;
            int excludedSfxMembers = 0;

            foreach (var f in t.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                try
                {
                    if (IsAudioClipType(f.FieldType)) audioClipMembers++;
                    if (IsExcludedAudioMember(f.Name))
                    {
                        if (IsAudioClipType(f.FieldType)) excludedSfxMembers++;
                        continue;
                    }
                    if (!LooksLikeMusicClipMember(f.Name, f.FieldType)) continue;
                    var name = ValueToUsefulText(f.GetValue(obj));
                    if (!string.IsNullOrWhiteSpace(name)) return name;
                    reason = $"후보 {f.Name} 값 비어있음";
                }
                catch (Exception ex) { reason = $"후보 {f.Name} 읽기 실패: {ex.GetType().Name}"; }
            }

            foreach (var p in t.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                try
                {
                    if (p.GetIndexParameters().Length > 0) continue;
                    if (IsAudioClipType(p.PropertyType)) audioClipMembers++;
                    if (IsExcludedAudioMember(p.Name))
                    {
                        if (IsAudioClipType(p.PropertyType)) excludedSfxMembers++;
                        continue;
                    }
                    if (!LooksLikeMusicClipMember(p.Name, p.PropertyType)) continue;
                    var name = ValueToUsefulText(p.GetValue(obj));
                    if (!string.IsNullOrWhiteSpace(name)) return name;
                    reason = $"후보 {p.Name} 값 비어있음";
                }
                catch (Exception ex) { reason = $"후보 {p.Name} 읽기 실패: {ex.GetType().Name}"; }
            }

            if (audioClipMembers > 0 && audioClipMembers == excludedSfxMembers)
                reason = "효과음 AudioClip만 있음";
            else if (audioClipMembers == 0)
                reason = "AudioClip 멤버 없음";

            return null;
        }

        private static string FindSceneMusicAudioClipName(out string reason)
        {
            reason = "씬 AudioSource 없음";
            try
            {
                GameObject bgmGo = GameObject.Find("BGM");
                if (bgmGo != null)
                {
                    AudioSource source = bgmGo.GetComponent<AudioSource>();
                    if (source != null && source.clip != null)
                    {
                        reason = null;
                        return source.clip.name;
                    }
                }

                var sources = UnityEngine.Object.FindObjectsOfType<AudioSource>();
                if (sources == null || sources.Length == 0) return null;

                int clipCount = 0;
                int excludedCount = 0;
                string firstMusicClip = null;
                string firstPausedMusicClip = null;

                foreach (var source in sources)
                {
                    try
                    {
                        var clip = source != null ? source.clip : null;
                        if (clip == null) continue;

                        clipCount++;
                        string clipName = clip.name;
                        if (IsExcludedAudioMember(clipName))
                        {
                            excludedCount++;
                            continue;
                        }

                        if (source.isPlaying) return clipName;
                        if (firstMusicClip == null && source.gameObject != null && LooksLikeMusicObjectName(source.gameObject.name))
                            firstMusicClip = clipName;
                        if (firstPausedMusicClip == null)
                            firstPausedMusicClip = clipName;
                    }
                    catch (Exception) { }
                }

                if (!string.IsNullOrWhiteSpace(firstMusicClip)) return firstMusicClip;
                if (!string.IsNullOrWhiteSpace(firstPausedMusicClip)) return firstPausedMusicClip;

                if (clipCount > 0 && clipCount == excludedCount)
                    reason = "씬 AudioSource에 효과음 클립만 있음";
                else if (clipCount == 0)
                    reason = "씬 AudioSource에 clip 없음";
                return null;
            }
            catch (Exception ex)
            {
                reason = $"씬 AudioSource 검색 실패: {ex.GetType().Name}";
                return null;
            }
        }

        private static bool LooksLikeMusicClipMember(string memberName, Type memberType)
        {
            string name = (memberName ?? "").ToLowerInvariant();
            if (IsExcludedAudioMember(name)) return false;

            string type = memberType != null ? memberType.Name.ToLowerInvariant() : "";
            if (type.Contains("audioclip")) return true;
            return name.Contains("musicclip") || name.Contains("demomusic") || name.Contains("bgm") || name.Contains("audio");
        }

        private static bool IsAudioClipType(Type memberType)
        {
            return memberType != null && memberType.Name.ToLowerInvariant().Contains("audioclip");
        }

        private static bool IsExcludedAudioMember(string memberName)
        {
            string name = (memberName ?? "").ToLowerInvariant();
            return name.Contains("click") || name.Contains("sfx") || name.Contains("button");
        }

        private static bool LooksLikeMusicObjectName(string objectName)
        {
            string name = (objectName ?? "").ToLowerInvariant();
            return name.Contains("music") || name.Contains("bgm") || name.Contains("song") || name.Contains("demo");
        }
    }
}
