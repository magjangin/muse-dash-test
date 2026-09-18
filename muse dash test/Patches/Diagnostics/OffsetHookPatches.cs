using MelonLoader;
using HarmonyLib;
using Il2CppFormulaBase;
using Il2CppAssets.Scripts.Database;
using Il2CppAssets.Scripts.UI.Panels;
using System;

namespace muse_dash_test
{
    /// <summary>
    /// 커스텀 곡의 오프셋(Offset)과 딜레이(Delay)를 게임의 계산 결과 위에 주입합니다.
    /// 주입이 실제로 일어난 사실은 곡·메서드당 한 번만 로그로 남깁니다.
    ///
    /// <para>전역 오프셋(<c>DataHelper.offset</c>) 값만 보던 비교용 훅은 여기 없습니다.
    /// 아무것도 주입하지 않으면서 getter마다 훅을 하나 물려 두는 자리였기 때문에,
    /// 씬 로드 때 한 번 읽어 남기는 <see cref="GlobalOffsetDiagnostics"/>로 옮겼습니다.</para>
    /// </summary>
    [HarmonyPatch]
    public static class OffsetHookPatches
    {
        // 오프셋/딜레이 getter는 배틀 중 매우 빈번하게 호출됩니다. 동일 컨텍스트(곡/메서드)당
        // 로그를 1회만 출력해 매 호출 MelonLogger I/O로 인한 로그 폭발 및 프레임 드랍을 방지합니다.
        private static readonly System.Collections.Generic.HashSet<string> loggedOnce = new System.Collections.Generic.HashSet<string>();

        // 해당 key가 처음 출력되는 경우에만 true를 반환합니다. (HashSet.Add는 신규 추가 시 true)
        private static bool LogOnce(string key) => loggedOnce.Add(key);

        // LogOnce의 키를 호출마다 문자열 보간으로 만들면 그 자체가 호출마다 힙 할당입니다.
        // 아래 훅들은 배틀 중 계속 도는 자리여서 그 할당이 그대로 GC 압력이 됩니다.
        // 그래서 훅마다 '마지막으로 판정한 uid'를 따로 들고, 곡이 그대로면 키를 만들지도 않습니다.
        // 곡이 바뀔 때만 loggedOnce를 보므로 "곡·훅당 한 번"이라는 결과는 그대로입니다.
        private static string lastFixedOffsetUid;
        private static string lastFixedMusicOffsetUid;
        private static string lastDelayUid;

        private static bool ShouldLogFor(ref string lastUid, string uid, string keyPrefix)
        {
            if (string.Equals(lastUid, uid, StringComparison.Ordinal))
            {
                return false;
            }

            lastUid = uid;
            return LogOnce(keyPrefix + uid);
        }

        // 폴백 조회 결과를 재사용하는 시간(ms).
        //
        // 아래 훅들, 특히 delay getter는 배틀 중 한 프레임에도 여러 번 돕니다. 그런데 폴백인
        // PnlStagePatchHelper.GetCurrentSelectedMusicUid()는 FindObjectOfType<PnlStage>() 한 번에
        // PnlStage의 필드·프로퍼티를 전부 리플렉션으로 훑는 조회라, 한 번이 비쌉니다.
        // 예전에는 그 결과를 어디에도 남기지 않아 호출마다 처음부터 다시 했고, '못 찾은 경우'도
        // 캐시하지 않았기 때문에 uid를 모르는 상태가 가장 비쌌습니다.
        //
        // 프레임 수(UnityEngine.Time.frameCount)가 아니라 Stopwatch로 재는 이유: Time은 메인 스레드
        // 전용이라 훅이 다른 스레드에서 불리면 호출마다 예외가 나고, 아래 바깥 catch가 그때마다
        // 에러 로그를 찍어 플레이 중 로그가 폭발합니다. Stopwatch는 그런 제약이 없습니다.
        private const long FallbackUidCacheMs = 100;

        private static readonly System.Diagnostics.Stopwatch fallbackClock = System.Diagnostics.Stopwatch.StartNew();
        private static string cachedFallbackUid;
        private static long cachedFallbackAtMs = long.MinValue;

        /// <summary>
        /// 폴백 UID 캐시를 버립니다. 씬이 바뀌면 곡도 바뀔 수 있으므로
        /// <c>MainMod.OnSceneWasLoaded</c>에서 부릅니다. 시간 만료를 기다리지 않고 즉시 버립니다.
        /// </summary>
        public static void ResetUidCache()
        {
            cachedFallbackUid = null;
            cachedFallbackAtMs = long.MinValue;
        }

        // 현재 로드된 가상 곡의 UID를 안전하게 조회하는 헬퍼 메서드
        private static string GetCurrentSongUid()
        {
            // 세션이 곡을 알고 있으면 비싼 조회는 아예 없습니다. 배틀 중에는
            // DBStageInfo.SetRuntimeMusicData 패치가 RememberMusicSelection으로 이 값을 채웁니다.
            string uid = CustomPlaySession.Current.SelectedMusicUid;
            if (!string.IsNullOrEmpty(uid))
            {
                return uid;
            }

            long now = fallbackClock.ElapsedMilliseconds;
            if (cachedFallbackAtMs == long.MinValue || now - cachedFallbackAtMs >= FallbackUidCacheMs)
            {
                cachedFallbackUid = PnlStagePatchHelper.GetCurrentSelectedMusicUid() ?? CustomPlaySession.Current.LastClickedMusicUid;
                cachedFallbackAtMs = now;
            }

            return cachedFallbackUid;
        }

        // 1. StageBattleComponent.FixedOffset 후킹 및 커스텀 오프셋(소수점 최대 7자리 보존) 주입
        [HarmonyPatch(typeof(StageBattleComponent), nameof(StageBattleComponent.FixedOffset))]
        [HarmonyPostfix]
        public static void PostfixFixedOffset(StageBattleComponent __instance)
        {
            try
            {
                string uid = GetCurrentSongUid();
                if (!string.IsNullOrEmpty(uid) && CustomContentIds.IsVirtualSong(uid))
                {
                    HwaManifest manifest = HwaResourceManager.GetManifest(uid);
                    if (manifest != null && manifest.Offset.HasValue)
                    {
                        float customOffset = (float)manifest.Offset.Value;
                        float originalOffset = __instance.offset;
                        
                        __instance.offset = customOffset;
                        
                        if (ShouldLogFor(ref lastFixedOffsetUid, uid, "FixedOffset:"))
                            ModLogger.Msg($"[OffsetInject] FixedOffset 오버라이드 완료. 곡={uid}, 기존={originalOffset}초 -> 주입={customOffset}초");
                    }
                }
            }
            catch (Exception ex)
            {
                ModLogger.Error($"[OffsetInject] FixedOffset Postfix 예외: {ex.Message}");
            }
        }

        // 2. StageBattleComponent.FixedMusicOffset 후킹 및 커스텀 오프셋 주입
        [HarmonyPatch(typeof(StageBattleComponent), nameof(StageBattleComponent.FixedMusicOffset))]
        [HarmonyPostfix]
        public static void PostfixFixedMusicOffset(StageBattleComponent __instance, UnityEngine.AudioSource bgm)
        {
            try
            {
                string uid = GetCurrentSongUid();
                if (!string.IsNullOrEmpty(uid) && CustomContentIds.IsVirtualSong(uid))
                {
                    HwaManifest manifest = HwaResourceManager.GetManifest(uid);
                    if (manifest != null && manifest.Offset.HasValue)
                    {
                        float customOffset = (float)manifest.Offset.Value;
                        float originalOffset = __instance.offset;
                        
                        __instance.offset = customOffset;
                        
                        if (ShouldLogFor(ref lastFixedMusicOffsetUid, uid, "FixedMusicOffset:"))
                        {
                            string bgmName = bgm != null ? bgm.name : "(null)";
                            ModLogger.Msg($"[OffsetInject] FixedMusicOffset 오버라이드 완료. BGM={bgmName}, 곡={uid}, 기존={originalOffset}초 -> 주입={customOffset}초");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ModLogger.Error($"[OffsetInject] FixedMusicOffset Postfix 예외: {ex.Message}");
            }
        }

        // 3. DBStageInfo.delay Getter 후킹 및 커스텀 지연 시간(소수점 최대 15자리 이상 보존 가능) 주입
        [HarmonyPatch(typeof(DBStageInfo), nameof(DBStageInfo.delay), MethodType.Getter)]
        [HarmonyPostfix]
        public static void PostfixGetDelay(DBStageInfo __instance, ref Il2CppSystem.Decimal __result)
        {
            try
            {
                string uid = GetCurrentSongUid();
                if (!string.IsNullOrEmpty(uid) && CustomContentIds.IsVirtualSong(uid))
                {
                    HwaManifest manifest = HwaResourceManager.GetManifest(uid);
                    if (manifest != null && manifest.Delay.HasValue)
                    {
                        double customDelay = manifest.Delay.Value;
                        bool shouldLogDelay = ShouldLogFor(ref lastDelayUid, uid, "delay:");
                        string originalStr = shouldLogDelay && __result != null ? __result.ToString() : "null";
                        
                        __result = (Il2CppSystem.Decimal)customDelay;
                        
                        if (shouldLogDelay)
                            ModLogger.Msg($"[OffsetInject] DBStageInfo.delay Getter 오버라이드 완료. 곡={uid}, 기존={originalStr} -> 주입={customDelay}");
                    }
                }
            }
            catch (Exception ex)
            {
                ModLogger.Error($"[OffsetInject] DBStageInfo.delay Getter Postfix 예외: {ex.Message}");
            }
        }
    }
}
