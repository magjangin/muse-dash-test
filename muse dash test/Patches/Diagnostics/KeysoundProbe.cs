using System;
using HarmonyLib;
using Il2Cpp;
using Il2CppFormulaBase;
using UnityEngine;

namespace muse_dash_test
{
    /// <summary>
    /// 키음(타격음)이 가끔 안 들린다는 제보(2026-11-20, 공식 곡 포함)를 가리기 위한 관찰 전용 계측입니다. 게임 동작은 바꾸지 않습니다.
    ///
    /// <para><b>게임의 키음 경로(ISIL, 2026-11-21)</b>: 일반 노트 타격 → <c>AttacksController.ShowAttackEffect(id, result, isContinue)</c>가
    /// 홀드(<c>isLongPressType</c>)가 아니면 그 노트의 키음 이름으로 <c>AttacksController.PlayOneShot(keyAudio, Hit)</c>을 부릅니다.
    /// <c>PlayOneShot</c>은 키음 볼륨(<c>m_DbOther.m_KeyAudioVolume</c>)이 0 이하면 아무것도 안 하고, 아니면 종류별 소스 하나
    /// (Hit/Touch/Others/Bass, <c>sfx_forte_2/3</c>·<c>sfx_piano_2</c>는 Bass)가 재생 중일 때 <c>Stop()</c>한 뒤
    /// <c>AudioManager.PlayOneShot(source, name, vol)</c> → 캐시에서 클립을 찾고(없으면 <c>"not cache!"</c> 에러 후 그 자리에서 동기 로드)
    /// → <c>AudioSource.PlayOneShot(clip, vol)</c>합니다. 마지막 단계에는 모드의 AP 뮤트 훅(<c>AllPerfectSound</c>)이 걸려 있습니다.</para>
    ///
    /// <para><b>잡는 것</b>: ① 같은 프레임에 같은 종류가 두 번 와서 앞 키음이 한 프레임도 못 울린 것(이름이 다르면 귀에 들리는 누락),
    /// ② 키음 호출 뒤 소스가 재생 중이 아닌 것, ③ 호출이 <c>AudioSource.PlayOneShot</c>까지 못 간 것이나 모드 훅이 막은 것,
    /// ④ 타격 연출(<c>ShowAttackEffect</c>)인데 키음 호출이 없었던 것, ⑤ 키음 직전 프레임 끊김(히치).
    /// 의심 사건 줄은 Verbose, 배틀 요약은 의심 사건이 있을 때만 Info 한 줄입니다. 사용자가 "방금 안 들렸다"고 느낄 때 <b>F8</b>을 누르면 최근 키음 기록을 덤프합니다
    /// (<see cref="Mark"/>).</para>
    /// </summary>
    [HarmonyPatch(typeof(AttacksController), nameof(AttacksController.PlayOneShot), new Type[] { typeof(string), typeof(AttacksController.KeyAudioType) })]
    internal static partial class KeysoundProbe
    {
        private const int TypeCount = 4;
        private const float HitchSeconds = 0.05f;
        // 의심 사건 줄을 배틀당 종류별로 이만큼만 바로 남깁니다(나머지는 요약 숫자로). 훅이 매번 어긋나면 로그가 넘치지 않게.
        private const int MaxLinesPerKind = 40;
        private static int failureLines;

        private static readonly int[] lastFrameByType = new int[TypeCount];
        private static readonly string[] lastKeyByType = new string[TypeCount];
        private static readonly int[] callsByType = new int[TypeCount];
        private static readonly int[] sameFrameByType = new int[TypeCount];
        private static int calls;
        private static int cutWhilePlaying;
        private static int emptyKey;
        private static int sameFrameDifferentKey;
        private static int notPlayingAfter;
        private static int notReachedUnity;
        private static int blockedByMod;
        private static int volumeZero;
        private static int hitches;
        private static int attackEffects;
        private static int attackEffectsWithoutKeysound;
        private static string lastSameFrameKey;
        private static bool loggedError;
        private static bool summaryEmitted;

        // 이번 PlayOneShot 호출 안에서 AudioSource.PlayOneShot까지 갔는지(AllPerfectSound 훅이 알려 줌).
        private static bool insideKeysound;
        private static bool unityHookAlive;
        private static bool unityReached;
        private static bool unityAllowed;
        private static string unityClipName;

        // 이번 ShowAttackEffect 호출 안에서 키음이 몇 번 불렸는지.
        private static int attackEffectDepth;
        private static int keysoundsInAttackEffect;

        public static void Reset()
        {
            EmitSummaryIfPending("배틀 재시작/이탈");
            Array.Clear(lastFrameByType, 0, TypeCount);
            Array.Clear(lastKeyByType, 0, TypeCount);
            Array.Clear(callsByType, 0, TypeCount);
            Array.Clear(sameFrameByType, 0, TypeCount);
            calls = 0;
            cutWhilePlaying = 0;
            emptyKey = 0;
            sameFrameDifferentKey = 0;
            notPlayingAfter = 0;
            notReachedUnity = 0;
            blockedByMod = 0;
            volumeZero = 0;
            hitches = 0;
            failureLines = 0;
            attackEffects = 0;
            attackEffectsWithoutKeysound = 0;
            lastSameFrameKey = null;
            summaryEmitted = false;
            insideKeysound = false;
            attackEffectDepth = 0;
            keysoundsInAttackEffect = 0;
            ClearEvents();
        }

        public static void Prefix(AttacksController __instance, string keyAudio, AttacksController.KeyAudioType audioType)
        {
            try
            {
                calls++;
                if (attackEffectDepth > 0) keysoundsInAttackEffect++;
                if (string.IsNullOrEmpty(keyAudio) || keyAudio == "0") emptyKey++;

                insideKeysound = true;
                unityReached = false;
                unityAllowed = true;
                unityClipName = null;

                int type = keyAudio == "sfx_forte_2" || keyAudio == "sfx_forte_3" || keyAudio == "sfx_piano_2"
                    ? (int)AttacksController.KeyAudioType.Bass
                    : (int)audioType;
                if (type < 0 || type >= TypeCount) return;
                callsByType[type]++;

                EventFlags flags = EventFlags.None;
                string previousKey = null;
                int frame = Time.frameCount;
                if (lastFrameByType[type] == frame)
                {
                    sameFrameByType[type]++;
                    lastSameFrameKey = keyAudio;
                    previousKey = lastKeyByType[type];
                    // 빈 공격음(char_common_empty_atk)은 홀드를 누를 때 sfx_press_top과 같은 프레임에 같이 와서 묻히는데,
                    // 원래 들리지 않아도 되는 소리라 '들리는 누락'으로 세지 않습니다(2026-11-21 실기 10건 전부 이 짝).
                    if (!string.Equals(previousKey, keyAudio, StringComparison.Ordinal)
                        && !IsEmptyAttackSound(previousKey) && !IsEmptyAttackSound(keyAudio))
                    {
                        sameFrameDifferentKey++;
                        flags |= EventFlags.SameFrameDifferentKey;
                    }
                    else
                    {
                        flags |= EventFlags.SameFrameSameKey;
                    }
                }
                lastFrameByType[type] = frame;
                lastKeyByType[type] = keyAudio;

                AudioSource source = SourceFor(__instance, type);
                if (source != null && source.isPlaying)
                {
                    cutWhilePlaying++;
                    flags |= EventFlags.CutWhilePlaying;
                }

                var db = __instance.m_DbOther;
                if (db != null && db.m_KeyAudioVolume <= 0f)
                {
                    volumeZero++;
                    flags |= EventFlags.VolumeZero;
                }

                if (Time.unscaledDeltaTime > HitchSeconds)
                {
                    hitches++;
                    flags |= EventFlags.Hitch;
                }

                if (attackEffectDepth > 0) flags |= EventFlags.FromAttackEffect;

                pendingEvent = BeginEvent(keyAudio, type, frame, flags);
                if ((flags & EventFlags.SameFrameDifferentKey) != 0 && sameFrameDifferentKey <= MaxLinesPerKind)
                {
                    ModLogger.Verbose($"[KeysoundProbe] 같은 프레임 다른 키음: '{previousKey}'가 '{keyAudio}'에 묻혀 안 울림 ({TypeName(type)}, t={GameTime():F3}s)");
                }
            }
            catch (Exception ex)
            {
                LogErrorOnce("Prefix", ex);
            }
        }

        public static void Postfix(AttacksController __instance, string keyAudio, AttacksController.KeyAudioType audioType)
        {
            try
            {
                int type = keyAudio == "sfx_forte_2" || keyAudio == "sfx_forte_3" || keyAudio == "sfx_piano_2"
                    ? (int)AttacksController.KeyAudioType.Bass
                    : (int)audioType;
                if (type < 0 || type >= TypeCount) return;

                EventFlags flags = EventFlags.None;
                AudioSource source = SourceFor(__instance, type);
                bool playing = source != null && source.isPlaying;
                bool volumeWasZero = pendingEvent >= 0 && (events[pendingEvent].Flags & EventFlags.VolumeZero) != 0;

                if (!volumeWasZero)
                {
                    // AudioSource.PlayOneShot 훅이 이번 실행에서 한 번도 안 불렸으면(게임 업데이트로 훅이 빠진 경우 등)
                    // '미도달'을 세지 않습니다. 세면 모든 키음이 실패로 잡히는 오탐이 됩니다.
                    if (!unityReached && unityHookAlive)
                    {
                        notReachedUnity++;
                        flags |= EventFlags.NotReachedUnity;
                    }
                    else if (!unityAllowed)
                    {
                        blockedByMod++;
                        flags |= EventFlags.BlockedByMod;
                    }
                    if (!playing)
                    {
                        notPlayingAfter++;
                        flags |= EventFlags.NotPlayingAfter;
                    }
                }

                CompleteEvent(flags, source, unityClipName);
                if ((flags & (EventFlags.NotReachedUnity | EventFlags.BlockedByMod | EventFlags.NotPlayingAfter)) != 0
                    && ++failureLines <= MaxLinesPerKind)
                {
                    ModLogger.Verbose($"[KeysoundProbe] 키음 재생 실패 의심: '{keyAudio}' ({TypeName(type)}, t={GameTime():F3}s) " +
                                  $"Unity도달={unityReached}, 모드허용={unityAllowed}, 클립={(unityClipName ?? "(none)")}, " +
                                  $"재생중={playing}, {DescribeSource(source)}");
                }
            }
            catch (Exception ex)
            {
                LogErrorOnce("Postfix", ex);
            }
            finally
            {
                insideKeysound = false;
                pendingEvent = -1;
            }
        }

        /// <summary>
        /// <c>AudioSource.PlayOneShot</c> 훅(<c>AllPerfectSound</c>)이 매번 부릅니다. 키음 호출 안일 때만 기록합니다.
        /// </summary>
        internal static void OnUnityPlayOneShot(AudioClip clip, bool allowed)
        {
            unityHookAlive = true;
            if (!insideKeysound) return;
            try
            {
                unityReached = true;
                unityAllowed = allowed;
                unityClipName = clip != null ? clip.name : "(null clip)";
            }
            catch (Exception ex)
            {
                LogErrorOnce("OnUnityPlayOneShot", ex);
            }
        }

        private static AudioSource SourceFor(AttacksController controller, int type)
        {
            return type == 0 ? controller.curHitAs
                : type == 1 ? controller.curTouchAs
                : type == 2 ? controller.curLAs
                : controller.curBassAs;
        }

        /// <summary>샌드백(연타) 노트의 <c>noteData.type</c>. 게임 <c>ShowAttackEffect</c>가 키음을 건너뛰는 값입니다.</summary>
        private const uint MultiHitNoteType = 8;

        private static bool IsEmptyAttackSound(string key)
        {
            return key != null && key.StartsWith("char_common_empty", StringComparison.Ordinal);
        }

        private static string TypeName(int type)
        {
            return type == 0 ? "Hit" : type == 1 ? "Touch" : type == 2 ? "Others" : "Bass";
        }

        private static float GameTime()
        {
            try
            {
                var stage = StageBattleComponent.instance;
                return stage != null ? stage.timeFromMusicStart : -1f;
            }
            catch (Exception)
            {
                return -1f;
            }
        }

        private static string DescribeSource(AudioSource source)
        {
            if (source == null) return "source=(null)";
            return $"source={source.gameObject.name}, enabled={source.enabled}, mute={source.mute}, volume={source.volume:F2}";
        }

        private static void LogErrorOnce(string where, Exception ex)
        {
            if (loggedError) return;
            loggedError = true;
            ModLogger.Msg($"[KeysoundProbe] 계측 예외({where}, 이후 생략): {ex.GetType().Name}: {ex.Message}");
        }

        /// <summary>
        /// 일반 노트 타격 연출. 이 안에서 키음 호출이 0번이면(홀드 제외) "타격했는데 키음 호출 없음"으로 남깁니다.
        /// </summary>
        [HarmonyPatch(typeof(AttacksController), nameof(AttacksController.ShowAttackEffect))]
        internal static class ShowAttackEffectHook
        {
            public static void Prefix()
            {
                attackEffectDepth++;
                if (attackEffectDepth == 1) keysoundsInAttackEffect = 0;
            }

            public static void Postfix(int id, uint resultCode)
            {
                try
                {
                    attackEffectDepth = Math.Max(0, attackEffectDepth - 1);
                    if (attackEffectDepth > 0) return;
                    attackEffects++;
                    if (keysoundsInAttackEffect > 0) return;
                    DescribeNoKeysoundAttack(id, resultCode);
                }
                catch (Exception ex)
                {
                    LogErrorOnce("ShowAttackEffect", ex);
                }
            }
        }

        private static void DescribeNoKeysoundAttack(int id, uint resultCode)
        {
            var stage = StageBattleComponent.instance;
            if (stage == null) return;
            var md = stage.GetMusicDataByIdx(id);
            if (md == null) return;
            if (md.isLongPressType) return; // 홀드는 게임이 여기서 키음을 안 냅니다(다른 경로)
            var note = md.noteData;
            uint noteType = note != null ? note.type : 0u;
            string key = note != null ? note.key_audio : null;
            // 샌드백(연타, type 8)은 게임이 ShowAttackEffect에서 일부러 건너뛰고 MultHitEnemyController.OnControllerAttacked에서
            // 소리를 냅니다(ISIL). 2026-11-21 실기에서 "호출 없음" 588건이 전부 type 8이었고 사용자는 소리를 들었습니다(오탐).
            if (noteType == MultiHitNoteType) return;

            attackEffectsWithoutKeysound++;
            if (attackEffectsWithoutKeysound <= MaxLinesPerKind)
            {
                ModLogger.Verbose($"[KeysoundProbe] 타격 연출인데 키음 호출 없음: id={id}, result={resultCode}, noteType={noteType}, " +
                              $"uid={(note != null ? note.uid : "(null)")}, key_audio='{key ?? "(null)"}', t={GameTime():F3}s");
            }
            RecordAttackWithoutKeysound(id, key);
        }
    }
}
