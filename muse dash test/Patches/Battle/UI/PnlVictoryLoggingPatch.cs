using MelonLoader;
using HarmonyLib;
using Il2Cpp;
using System;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace muse_dash_test.Patches.Battle.UI
{
    /// <summary>
    /// PnlVictory(결과 정산 화면)의 라이프사이클 및 주요 이벤트 콜백을 실시간으로 감시하고 진단 로그를 남기는 패치 클래스입니다.
    /// </summary>
    [HarmonyPatch]
    public static class PnlVictoryLoggingPatch
    {
        private static IEnumerable<MethodBase> TargetMethods()
        {
            var methods = new List<MethodBase>();
            try
            {
                var type = typeof(PnlVictory);
                foreach (var m in type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly))
                {
                    if (m.Name == "OnVictory" || m.Name == "Awake" || m.Name == "OnEnable" || m.Name == "Init" || m.Name == "Show" || m.Name == "SetResult")
                    {
                        methods.Add(m);
                    }
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[PnlVictoryLoggingPatch] TargetMethods 탐색 중 예외: {ex}");
            }
            return methods;
        }

        public static void Prefix(MethodBase __originalMethod, PnlVictory __instance)
        {
            try
            {
                string methodName = __originalMethod != null ? __originalMethod.Name : "(unknown)";
                MelonLogger.Msg($"🏆 [PnlVictory.{methodName}.Prefix] 호출됨! ActiveSelf={(__instance != null && __instance.gameObject != null ? __instance.gameObject.activeSelf.ToString() : "null")}");
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[PnlVictoryLoggingPatch.Prefix] 예외 발생: {ex}");
            }
        }

        public static void Postfix(MethodBase __originalMethod, PnlVictory __instance)
        {
            try
            {
                string methodName = __originalMethod != null ? __originalMethod.Name : "(unknown)";
                MelonLogger.Msg($"🏆 [PnlVictory.{methodName}.Postfix] 실행 완료!");

                if (__instance != null && (methodName == "OnVictory" || methodName == "OnEnable" || methodName == "SetResult"))
                {
                    LogVictoryDetails(__instance);
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[PnlVictoryLoggingPatch.Postfix] 예외 발생: {ex}");
            }
        }

        private static void LogVictoryDetails(PnlVictory instance)
        {
            try
            {
                string uid = CustomPlaySession.Current.SelectedMusicUid;
                if (string.IsNullOrEmpty(uid))
                {
                    uid = PnlStagePatchHelper.GetCurrentSelectedMusicUid() ?? CustomPlaySession.Current.LastClickedMusicUid ?? "(unknown)";
                }

                MelonLogger.Msg($"📊 [PnlVictory.Diagnostics] 결과 패치 상세 진단 (UID={uid}):");

                var target = VictoryDataCache.ActiveTarget;
                if (target != null)
                {
                    int perfect = ModReflection.GetInt(target, "PerfectResult");
                    int great = target.m_GreatResult;
                    int miss = target.m_MissResult;
                    int score = ModReflection.GetInt(target, "Score");
                    int maxCombo = ModReflection.GetInt(target, "MaxCombo");
                    float acc = target.GetAccuracy();
                    bool fc = target.IsFullCombo();

                    MelonLogger.Msg($"   - [TargetRecord] Score={score}, MaxCombo={maxCombo}, Acc={acc * 100f:F2}%, Perfect={perfect}, Great={great}, Miss={miss}, FC={fc}");
                }
                else
                {
                    MelonLogger.Warning("   - [TargetRecord] VictoryDataCache.ActiveTarget가 null입니다.");
                }

                var curControls = ModReflection.GetValue(instance, "CurControls", silent: true);
                if (curControls != null)
                {
                    MelonLogger.Msg($"   - [CurControls] 발견됨: type={curControls.GetType().Name}");
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[PnlVictoryLoggingPatch.LogVictoryDetails] 예외 발생: {ex}");
            }
        }
    }
}
