using System;
using HarmonyLib;
using Il2CppAssets.Scripts.Database;

namespace muse_dash_test
{
    /// <summary>
    /// 크리스마스 연출을 날짜와 상관없이 켜는 두 경로입니다.
    ///
    /// <para><b>게임의 판정 (ISIL 확인, 2026-11-11)</b></para>
    /// <list type="bullet">
    /// <item><c>GameMusicScene.SceneFestival(name)</c>: <c>name == "scene_05"</c>이고
    ///   [12월 24·25일 또는 곡 uid가 <c>0-47</c>/<c>38-1</c>]이면 <c>"scene_05_christmas"</c>,
    ///   uid가 <c>43-68</c>이면 이름과 상관없이 <c>"scene_05_christmas"</c>를 돌려줍니다.
    ///   날짜는 <c>BattleHelper.isChristmas</c>를 거치지 않고 <c>DateTime.Now</c>를 직접 봅니다.
    ///   <c>LoadScene</c>이 처음 씬과 곡 중간 전환 목록의 씬마다 부릅니다.</item>
    /// <item><c>Boss.BossFestival(name)</c>: <c>name == "0501_boss"</c>이고 [<c>BattleHelper.isChristmas</c>
    ///   또는 uid <c>0-47</c>/<c>38-1</c>]이면 <c>"0501_boss_christmas"</c>. <c>Boss.SetBoss</c>가 부릅니다.</item>
    /// <item><c>BattleHelper.isChristmas</c>: <c>DateTime.Now</c>가 12월 24·25일인지. 위 보스 외에
    ///   <c>GameInit.SpecialLogic</c>, <c>ItemManager.ChristmasItemLogic</c>, <c>MainManager.InitLoading</c>도 씁니다.</item>
    /// </list>
    ///
    /// <para><b>전역 날짜 속이기</b> (<see cref="ModConfig.ForceChristmas"/>): 세 곳 모두 켭니다. 공식 곡도 포함입니다.</para>
    /// <para><b>공식곡 옵션</b> (config.txt '공식곡에서도 크리스마스 적용하기'): 공식 곡의 배경·보스만 바꿉니다(노트 스킨은 아님).
    ///   <c>isChristmas</c>는 건드리지 않습니다.</para>
    /// <para><b>곡별 옵션</b> (info.txt '크리스마스 배경'): 배경과 보스만 바꿉니다. <c>isChristmas</c>는
    ///   아이템·로딩처럼 곡과 무관한 곳도 쓰므로 곡 옵션으로는 건드리지 않습니다.</para>
    /// </summary>
    internal static class ChristmasFestival
    {
        internal const string Scene05 = "scene_05";
        internal const string Scene05Christmas = "scene_05_christmas";
        internal const string Boss05 = "0501_boss";
        internal const string Boss05Christmas = "0501_boss_christmas";

        /// <summary>지금 곡에서 크리스마스 배경·보스를 강제로 쓸지. 판정 근거를 <paramref name="reason"/>에 남깁니다.</summary>
        internal static bool ShouldForceForCurrentSong(out string reason)
            => Evaluate(includeOfficialSongOption: true, out reason);

        /// <summary>
        /// 지금 곡에 크리스마스 노트 스킨을 씌울지. 배경·보스와 같은 판정이지만 config.txt
        /// '공식곡에서도 크리스마스 적용하기'는 보지 않습니다. 그 옵션은 공식 곡의 배경·보스만 바꾸려는 것입니다.
        /// </summary>
        internal static bool ShouldApplyNoteSkinForCurrentSong(out string reason)
            => Evaluate(includeOfficialSongOption: false, out reason);

        private static bool Evaluate(bool includeOfficialSongOption, out string reason)
        {
            reason = null;
            try
            {
                // 1) 커스텀 곡 문맥인 경우: info.txt의 '크리스마스' 설정이 켜진 곡만 적용합니다.
                //    전역 ForceChristmas 설정이 켜져 있더라도 다른 커스텀 곡에 무차별 누출되지 않도록 차단합니다.
                if (CustomPlaySession.Current.ShouldApplyExperimentChart)
                {
                    string uid = CustomPlaySession.Current.LastKnownMusicUid;
                    HwaManifest manifest = HwaResourceManager.GetManifest(uid);
                    if (manifest?.ChristmasScene == true)
                    {
                        reason = $"info.txt 크리스마스 활성화 (uid={uid})";
                        return true;
                    }

                    return false;
                }

                // 2) 공식 곡 문맥일 때만 전역 설정을 참조합니다.
                if (includeOfficialSongOption && InputOverlay.christmasOnOfficialSongs)
                {
                    reason = "config.txt 공식곡에서도 크리스마스 적용하기 (공식 곡)";
                    return true;
                }

                if (ModConfig.ForceChristmas)
                {
                    reason = "ForceChristmas 전역 설정 (공식 곡)";
                    return true;
                }
            }
            catch (Exception ex)
            {
                ModLogger.Warning($"[ChristmasFestival] 곡 옵션 판정 실패, 게임 판정을 그대로 씁니다: {ex.Message}");
            }

            return false;
        }
    }

    [HarmonyPatch(typeof(BattleHelper), nameof(BattleHelper.isChristmas), MethodType.Getter)]
    internal static class BattleHelper_IsChristmas_Patch
    {
        private static void Postfix(ref bool __result)
        {
            if (!__result && ModConfig.ForceChristmas)
            {
                __result = true;
            }
        }
    }

    [HarmonyPatch(typeof(Il2CppGameLogic.GameMusicScene), nameof(Il2CppGameLogic.GameMusicScene.SceneFestival))]
    internal static class GameMusicScene_SceneFestival_Patch
    {
        private static void Postfix(string sceneFestivalName, ref string __result)
        {
            try
            {
                // 크리스마스 판은 5번 씬에만 있습니다. 게임이 이미 바꿨으면(진짜 12/24·25 등) 손대지 않습니다.
                if (sceneFestivalName != ChristmasFestival.Scene05 || __result != ChristmasFestival.Scene05) return;
                if (!ChristmasFestival.ShouldForceForCurrentSong(out string reason)) return;

                __result = ChristmasFestival.Scene05Christmas;
                ModLogger.Msg($"[ChristmasFestival] 씬 {ChristmasFestival.Scene05} -> {__result} ({reason})");
            }
            catch (Exception ex)
            {
                ModLogger.Error($"[ChristmasFestival] SceneFestival Postfix 예외: {ex}");
            }
        }
    }

    [HarmonyPatch(typeof(Il2Cpp.Boss), nameof(Il2Cpp.Boss.BossFestival))]
    internal static class Boss_BossFestival_Patch
    {
        private static void Postfix(string bossFestivalName, ref string __result)
        {
            try
            {
                if ((bossFestivalName != ChristmasFestival.Boss05 && bossFestivalName != ChristmasFestival.Boss05Christmas)
                    || (__result != ChristmasFestival.Boss05 && __result != ChristmasFestival.Boss05Christmas)) return;
                if (!ChristmasFestival.ShouldForceForCurrentSong(out string reason)) return;

                __result = ChristmasFestival.Boss05Christmas;
                ModLogger.Msg($"[ChristmasFestival] 보스 {ChristmasFestival.Boss05} -> {__result} ({reason})");
            }
            catch (Exception ex)
            {
                ModLogger.Error($"[ChristmasFestival] BossFestival Postfix 예외: {ex}");
            }
        }
    }
}
