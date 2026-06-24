using MelonLoader;
using HarmonyLib;
using Il2CppFormulaBase;
using Il2CppAssets.Scripts.Database;
using Il2CppAssets.Scripts.UI.Panels;
using System;

namespace muse_dash_test
{
    /// <summary>
    /// 게임 내 오프셋(Offset) 및 딜레이(Delay) 연산 흐름을 추적하기 위해
    /// 핵심 메서드들을 실시간으로 후킹하여 MelonLogger 로그로 출력합니다.
    /// </summary>
    [HarmonyPatch]
    public static class OffsetHookPatches
    {
        // 오프셋/딜레이 getter는 배틀 중 매우 빈번하게 호출됩니다. 동일 컨텍스트(곡/메서드)당
        // 로그를 1회만 출력해 매 호출 MelonLogger I/O로 인한 로그 폭발 및 프레임 드랍을 방지합니다.
        private static readonly System.Collections.Generic.HashSet<string> loggedOnce = new System.Collections.Generic.HashSet<string>();

        // 해당 key가 처음 출력되는 경우에만 true를 반환합니다. (HashSet.Add는 신규 추가 시 true)
        private static bool LogOnce(string key) => loggedOnce.Add(key);

        // 현재 로드된 가상 곡의 UID를 안전하게 조회하는 헬퍼 메서드
        private static string GetCurrentSongUid()
        {
            string uid = CustomPlaySession.Current.SelectedMusicUid;
            if (string.IsNullOrEmpty(uid))
            {
                uid = PnlStagePatchHelper.GetCurrentSelectedMusicUid() ?? CustomPlaySession.Current.LastClickedMusicUid;
            }
            return uid;
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
                        
                        if (LogOnce($"FixedOffset:{uid}"))
                            MelonLogger.Msg($"[OffsetInject] FixedOffset 오버라이드 완료. 곡={uid}, 기존={originalOffset}초 -> 주입={customOffset}초");
                    }
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[OffsetInject] FixedOffset Postfix 예외: {ex.Message}");
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
                        
                        if (LogOnce($"FixedMusicOffset:{uid}"))
                        {
                            string bgmName = bgm != null ? bgm.name : "(null)";
                            MelonLogger.Msg($"[OffsetInject] FixedMusicOffset 오버라이드 완료. BGM={bgmName}, 곡={uid}, 기존={originalOffset}초 -> 주입={customOffset}초");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[OffsetInject] FixedMusicOffset Postfix 예외: {ex.Message}");
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
                        bool shouldLogDelay = LogOnce($"delay:{uid}");
                        string originalStr = shouldLogDelay && __result != null ? __result.ToString() : "null";
                        
                        __result = (Il2CppSystem.Decimal)customDelay;
                        
                        if (shouldLogDelay)
                            MelonLogger.Msg($"[OffsetInject] DBStageInfo.delay Getter 오버라이드 완료. 곡={uid}, 기존={originalStr} -> 주입={customDelay}");
                    }
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[OffsetInject] DBStageInfo.delay Getter Postfix 예외: {ex.Message}");
            }
        }

        // 4. DataHelper.offset Getter 후킹 (디버그 비교용 로그)
        [HarmonyPatch(typeof(DataHelper), nameof(DataHelper.offset), MethodType.Getter)]
        [HarmonyPostfix]
        public static void PostfixGetGlobalOffset(ref int __result)
        {
            try
            {
                if (LogOnce("DataHelper.offset"))
                    MelonLogger.Msg($"[OffsetInject] DataHelper.offset Getter 호출됨: {__result}ms");
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[OffsetInject] DataHelper.offset Getter Postfix 예외: {ex.Message}");
            }
        }
    }
}
