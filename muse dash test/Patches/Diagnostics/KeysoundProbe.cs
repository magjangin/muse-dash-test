using System;
using HarmonyLib;
using Il2Cpp;
using UnityEngine;

namespace muse_dash_test
{
    /// <summary>
    /// 키음(타격음)이 가끔 안 들린다는 제보(2026-11-20, 공식 곡 포함)를 가리기 위한 관찰 전용 계측입니다. 게임 동작은 바꾸지 않습니다.
    ///
    /// <para><b>게임 동작(ISIL)</b>: <c>AttacksController.PlayOneShot(keyAudio, type)</c>은 종류별 소스 하나
    /// (Hit=<c>curHitAs</c>, Touch=<c>curTouchAs</c>, Others=<c>curLAs</c>, Bass=<c>curBassAs</c>;
    /// <c>sfx_forte_2</c>·<c>sfx_forte_3</c>·<c>sfx_piano_2</c>는 Bass로 강제)를 쓰고, 그 소스가 재생 중이면
    /// <c>Stop()</c>한 뒤 새 키음을 <c>PlayOneShot</c>합니다. 그래서 같은 종류가 같은 프레임에 두 번 오면 앞의 것은
    /// 한 프레임도 안 울리고 끊깁니다. 키음 볼륨(<c>KeyAudioVolume</c>)이 0 이하면 아예 재생하지 않습니다.</para>
    ///
    /// <para>배틀이 끝날 때 한 줄로 요약합니다: 전체 호출, 재생 중이라 끊은 수, 같은 프레임 같은 종류(앞의 것이 안 들림),
    /// 키음 이름이 비었거나 "0"인 수. 같은 프레임 수가 체감 누락과 맞으면 게임 자체 동작입니다.</para>
    /// </summary>
    [HarmonyPatch(typeof(AttacksController), nameof(AttacksController.PlayOneShot), new Type[] { typeof(string), typeof(AttacksController.KeyAudioType) })]
    internal static class KeysoundProbe
    {
        private const int TypeCount = 4;
        private static readonly int[] lastFrameByType = new int[TypeCount];
        private static readonly int[] callsByType = new int[TypeCount];
        private static readonly int[] sameFrameByType = new int[TypeCount];
        private static int calls;
        private static int cutWhilePlaying;
        private static int emptyKey;
        private static string lastSameFrameKey;
        private static bool loggedError;

        public static void Reset()
        {
            Array.Clear(lastFrameByType, 0, TypeCount);
            Array.Clear(callsByType, 0, TypeCount);
            Array.Clear(sameFrameByType, 0, TypeCount);
            calls = 0;
            cutWhilePlaying = 0;
            emptyKey = 0;
            lastSameFrameKey = null;
        }

        public static void Prefix(AttacksController __instance, string keyAudio, AttacksController.KeyAudioType audioType)
        {
            try
            {
                calls++;
                if (string.IsNullOrEmpty(keyAudio) || keyAudio == "0") emptyKey++;

                int type = keyAudio == "sfx_forte_2" || keyAudio == "sfx_forte_3" || keyAudio == "sfx_piano_2"
                    ? (int)AttacksController.KeyAudioType.Bass
                    : (int)audioType;
                if (type < 0 || type >= TypeCount) return;
                callsByType[type]++;

                int frame = Time.frameCount;
                if (lastFrameByType[type] == frame)
                {
                    sameFrameByType[type]++;
                    lastSameFrameKey = keyAudio;
                }
                lastFrameByType[type] = frame;

                AudioSource source = type == 0 ? __instance.curHitAs
                    : type == 1 ? __instance.curTouchAs
                    : type == 2 ? __instance.curLAs
                    : __instance.curBassAs;
                if (source != null && source.isPlaying) cutWhilePlaying++;
            }
            catch (Exception ex)
            {
                if (!loggedError)
                {
                    loggedError = true;
                    ModLogger.Verbose($"[KeysoundProbe] 계측 예외(이후 생략): {ex.GetType().Name}: {ex.Message}");
                }
            }
        }

        /// <summary>배틀 종료 때 한 줄 요약입니다. 호출이 없었으면 null입니다.</summary>
        public static string Summary()
        {
            if (calls == 0) return null;
            int sameFrame = 0;
            for (int i = 0; i < TypeCount; i++) sameFrame += sameFrameByType[i];
            return $"키음 {calls}회 (Hit {callsByType[0]}/Touch {callsByType[1]}/Others {callsByType[2]}/Bass {callsByType[3]}), " +
                   $"재생 중 끊음 {cutWhilePlaying}회, 같은 프레임 같은 종류(앞 키음 무음) {sameFrame}회" +
                   (lastSameFrameKey != null ? $" (마지막: {lastSameFrameKey})" : "") +
                   $", 빈 키음 {emptyKey}회";
        }
    }
}
