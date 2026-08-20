using HarmonyLib;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace muse_dash_test
{
    /// <summary>
    /// MelonLogger의 모든 Msg/Warning/Error 메서드를 후킹하여 ModLogger.CurrentLogLevel에 따라
    /// 로그 출력을 가로채고 불필요한 로그 출력을 원천 차단(음소거)하는 Harmony 패치입니다.
    /// UMPC 등 저전력 환경에서 발생하는 콘솔 및 파일 I/O 부하를 전역 차원에서 제거합니다.
    /// </summary>
    [HarmonyPatch]
    public static class MelonLoggerInterceptor
    {
        private static IEnumerable<MethodBase> TargetMethods()
        {
            var targets = new List<MethodBase>();
            try
            {
                var type = typeof(MelonLogger);
                var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Static);
                foreach (var method in methods)
                {
                    if (method.Name == "Msg" || method.Name == "Warning" || method.Name == "Error")
                    {
                        targets.Add(method);
                    }
                }
            }
            catch (Exception)
            {
                // Reflection 실패 시 안전하게 무시
            }
            return targets;
        }

        [HarmonyPrefix]
        public static bool Prefix(MethodBase __originalMethod)
        {
            // ModLogger.LogAlways 실행 중에는 필터를 우회하여 출력 허용
            if (ModLogger.IsBypassing)
            {
                return true;
            }

            if (__originalMethod == null) return true;

            string name = __originalMethod.Name;
            if (name == "Msg")
            {
                return ModLogger.CurrentLogLevel >= ModLogLevel.Info;
            }
            if (name == "Warning")
            {
                return ModLogger.CurrentLogLevel >= ModLogLevel.Warning;
            }
            if (name == "Error")
            {
                return ModLogger.CurrentLogLevel >= ModLogLevel.Error;
            }

            return true;
        }
    }
}
