using System;
using System.Text;
using Il2CppInterop.Runtime;
using UnityEngine;

namespace muse_dash_test
{
    /// <summary>
    /// <see cref="KeysoundProbe"/>의 최근 키음 기록(링 버퍼), F8 표시, 배틀 요약입니다.
    /// </summary>
    internal static partial class KeysoundProbe
    {
        [Flags]
        private enum EventFlags
        {
            None = 0,
            SameFrameSameKey = 1 << 0,
            SameFrameDifferentKey = 1 << 1,
            CutWhilePlaying = 1 << 2,
            VolumeZero = 1 << 3,
            Hitch = 1 << 4,
            FromAttackEffect = 1 << 5,
            NotReachedUnity = 1 << 6,
            BlockedByMod = 1 << 7,
            NotPlayingAfter = 1 << 8,
            AttackWithoutKeysound = 1 << 9,
        }

        private struct KeysoundEvent
        {
            public float GameTime;
            public int Frame;
            public float DeltaTime;
            public string Key;
            public int Type;
            public EventFlags Flags;
            public string Clip;
            public string SourceState;
        }

        private const int EventCapacity = 32;
        private const int MarkDumpCount = 16;
        private static readonly KeysoundEvent[] events = new KeysoundEvent[EventCapacity];
        private static int eventCount;
        private static int eventNext;
        private static int pendingEvent = -1;
        private static int marks;

        private static void ClearEvents()
        {
            Array.Clear(events, 0, EventCapacity);
            eventCount = 0;
            eventNext = 0;
            pendingEvent = -1;
            marks = 0;
        }

        private static int BeginEvent(string key, int type, int frame, EventFlags flags)
        {
            int index = eventNext;
            events[index] = new KeysoundEvent
            {
                GameTime = GameTime(),
                Frame = frame,
                DeltaTime = Time.unscaledDeltaTime,
                Key = key,
                Type = type,
                Flags = flags,
            };
            eventNext = (eventNext + 1) % EventCapacity;
            if (eventCount < EventCapacity) eventCount++;
            return index;
        }

        private static void CompleteEvent(EventFlags flags, AudioSource source, string clip)
        {
            if (pendingEvent < 0) return;
            events[pendingEvent].Flags |= flags;
            events[pendingEvent].Clip = clip;
            // 소스 상태 문자열은 실패했을 때만 만듭니다(매 키음마다 문자열을 만들지 않게).
            if ((flags & (EventFlags.NotReachedUnity | EventFlags.BlockedByMod | EventFlags.NotPlayingAfter)) != 0)
            {
                events[pendingEvent].SourceState = DescribeSource(source);
            }
        }

        private static void RecordAttackWithoutKeysound(int id, string key)
        {
            int index = BeginEvent(key, 0, Time.frameCount, EventFlags.AttackWithoutKeysound);
            events[index].Clip = "note#" + id;
        }

        /// <summary>
        /// 사용자가 "방금 키음이 안 들렸다"고 느낄 때 누르는 F8. 최근 키음 기록과 오디오 상태를 덤프합니다.
        /// <c>MainMod.OnUpdate</c>가 매 프레임 부릅니다. 배틀 밖(메뉴·로딩)에서는 관리 플래그만 보고 돌아가
        /// <c>Input.GetKeyDown</c>(IL2CPP 호출)도 하지 않습니다. 배틀 밖 F8은 지난 배틀 기록을 덤프할 뿐이라 쓸모가 없었습니다.
        /// </summary>
        public static void PollMarkKey()
        {
            if (!HywStageManager.IsInStageStatic) return;
            if (!Input.GetKeyDown(KeyCode.F8)) return;
            Mark();
        }

        public static void Mark()
        {
            marks++;
            float now = GameTime();
            var sb = new StringBuilder();
            sb.Append($"[KeysoundProbe] ★ F8 표시 #{marks}: t={now:F3}s, frame={Time.frameCount}, 이번 배틀 키음 {calls}회. 최근 기록(오래된 순):");
            int count = Math.Min(eventCount, MarkDumpCount);
            for (int i = count; i >= 1; i--)
            {
                int index = (eventNext - i + EventCapacity) % EventCapacity;
                var e = events[index];
                sb.Append("\n    ");
                sb.Append($"t={e.GameTime:F3}s ({e.GameTime - now:+0.000;-0.000}s) f={e.Frame} dt={e.DeltaTime * 1000f:F0}ms ");
                sb.Append($"{TypeName(e.Type)} '{e.Key ?? "(null)"}'");
                if (e.Clip != null) sb.Append($" clip={e.Clip}");
                if (e.Flags != EventFlags.None) sb.Append($" [{e.Flags}]");
                if (e.SourceState != null) sb.Append($" {e.SourceState}");
            }
            sb.Append("\n    ");
            sb.Append(DescribeAudioState());
            ModLogger.Msg(sb.ToString());
        }

        private static string DescribeAudioState()
        {
            try
            {
                var config = AudioSettings.GetConfiguration();
                int playing = 0, total = 0;
                var all = UnityEngine.Object.FindObjectsOfType(Il2CppType.Of<AudioSource>());
                if (all != null)
                {
                    foreach (var obj in all)
                    {
                        var source = obj != null ? obj.TryCast<AudioSource>() : null;
                        if (source == null) continue;
                        total++;
                        if (source.isPlaying) playing++;
                    }
                }
                return $"오디오: 재생 중 소스 {playing}/{total}개, realVoices={config.numRealVoices}, virtualVoices={config.numVirtualVoices}, " +
                       $"dspBuffer={config.dspBufferSize}, sampleRate={config.sampleRate}, timeScale={Time.timeScale:F2}, AudioListener.pause={AudioListener.pause}";
            }
            catch (Exception ex)
            {
                return "오디오 상태 조회 예외: " + ex.GetType().Name + ": " + ex.Message;
            }
        }

        /// <summary>배틀 종료 때 한 줄 요약입니다. 호출이 없었으면 null입니다.</summary>
        public static string Summary()
        {
            if (calls == 0) return null;
            summaryEmitted = true;
            int sameFrame = 0;
            for (int i = 0; i < TypeCount; i++) sameFrame += sameFrameByType[i];
            return $"키음 {calls}회 (Hit {callsByType[0]}/Touch {callsByType[1]}/Others {callsByType[2]}/Bass {callsByType[3]}), " +
                   $"재생 중 끊음 {cutWhilePlaying}회, 같은 프레임 같은 종류 {sameFrame}회 (그중 다른 키음 {sameFrameDifferentKey}회" +
                   (lastSameFrameKey != null ? $", 마지막: {lastSameFrameKey}" : "") + "), " +
                   $"Unity 미도달 {notReachedUnity}회, 모드 훅 차단 {blockedByMod}회, 재생 직후 정지 상태 {notPlayingAfter}회, " +
                   $"볼륨0 {volumeZero}회, 히치(>{HitchSeconds * 1000f:F0}ms) {hitches}회, 빈 키음 {emptyKey}회, " +
                   $"타격 연출 {attackEffects}회 중 키음 호출 없음 {attackEffectsWithoutKeysound}회, F8 표시 {marks}회";
        }

        /// <summary>
        /// 귀에 들리는 누락으로 볼 만한 사건 수. 같은 프레임 같은 키음(같은 소리라 안 들림), 빈 공격음, 샌드백, 히치, 볼륨 0은
        /// 넣지 않습니다(2026-11-21 실기에서 이것들이 오탐이었음).
        /// </summary>
        private static int SuspiciousCount()
        {
            return sameFrameDifferentKey + notReachedUnity + blockedByMod + notPlayingAfter + attackEffectsWithoutKeysound;
        }

        /// <summary>
        /// 배틀 요약을 남깁니다. 의심 사건이 없으면 Verbose(평소 로그에 안 보임), 있으면 Info 한 줄입니다.
        /// End Postfix와, End 없이 재시작·이탈한 배틀(<see cref="Reset"/>)에서 부릅니다.
        /// </summary>
        public static void LogSummary(string reason)
        {
            if (summaryEmitted || calls == 0) return;
            string summary = Summary();
            if (summary == null) return;
            if (SuspiciousCount() > 0) ModLogger.Msg($"[KeysoundProbe] ({reason}) 키음 누락 의심 {SuspiciousCount()}건 — {summary}");
            else ModLogger.Verbose($"[KeysoundProbe] ({reason}) {summary}");
        }

        private static void EmitSummaryIfPending(string reason)
        {
            LogSummary(reason);
        }
    }
}
