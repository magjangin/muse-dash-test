using MelonLoader;
using System;

[assembly: MelonInfo(typeof(muse_dash_test.MainMod), "muse-dash-test", "0.1.0", "화영왕")]
[assembly: MelonGame("PeroPeroGames", "MuseDash")]

namespace muse_dash_test
{
    public class MainMod : MelonMod
    {
        public override void OnApplicationStart()
        {
            MelonLogger.Msg("모드가 로드되었습니다.");
            try {
                LogUidMethods();
            } catch (System.Exception ex) {
                MelonLogger.Error("LogUidMethods 실패: " + ex.Message);
            }
        }

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            MelonLogger.Msg($"씬이 로드되었습니다: {sceneName} (빌드 인덱스: {buildIndex})");
        }

        public override void OnApplicationQuit()
        {
            MelonLogger.Msg("모드가 종료되었습니다.");
        }

        private void LogUidMethods()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            // Filter: only inspect Assembly-CSharp
            var asmList = new System.Collections.Generic.List<System.Reflection.Assembly>();
            foreach (var a in assemblies)
            {
                try {
                    if (string.Equals(a.GetName().Name, "Assembly-CSharp", System.StringComparison.OrdinalIgnoreCase))
                        asmList.Add(a);
                } catch { }
            }
            if (asmList.Count == 0)
            {
                MelonLogger.Warning("Assembly-CSharp을 찾을 수 없습니다. 로드된 어셈블리 목록에 포함되어 있는지 확인하세요.");
                return;
            }

            int totalTypes = 0, totalMethods = 0;
            foreach (var asm in asmList)
            {
                System.Type[] types = null;
                try
                {
                    types = asm.GetTypes();
                }
                catch (System.Reflection.ReflectionTypeLoadException rtlex)
                {
                    types = rtlex.Types;
                    MelonLogger.Warning($"어셈블리 '{asm.GetName().Name}'에서 일부 타입을 로드할 수 없습니다.");
                }
                if (types == null) continue;

                foreach (var t in types)
                {
                    if (t == null) continue;
                    totalTypes++;
                    try
                    {
                        var methods = t.GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.DeclaredOnly);
                        var matched = new System.Collections.Generic.List<string>();
                        foreach (var m in methods)
                        {
                            if (m == null) continue;
                            // 제외: 컴파일러 생성된 람다/익명 메서드
                            try {
                                if (m.IsDefined(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), false))
                                    continue;
                            } catch { }
                            var declTypeName = m.DeclaringType != null ? m.DeclaringType.Name : string.Empty;
                            if (declTypeName.IndexOf("<", System.StringComparison.Ordinal) >= 0)
                                continue;
                            if (m.Name.IndexOf("<", System.StringComparison.Ordinal) >= 0 || m.Name.IndexOf("b__", System.StringComparison.Ordinal) >= 0)
                                continue;

                            if (m.Name.IndexOf("uid", System.StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                matched.Add(m.ToString());
                                totalMethods++;
                            }
                        }
                        if (matched.Count > 0)
                        {
                            MelonLogger.Msg($"[{asm.GetName().Name}] {t.FullName} - methods containing 'uid' ({matched.Count}):");
                            foreach (var s in matched)
                                MelonLogger.Msg("  " + s);
                        }
                    }
                    catch (System.Exception) { /* ignore type-level reflection errors */ }
                }
            }
            MelonLogger.Msg($"LogUidMethods 완료: 검사된 타입 {totalTypes}, 매칭된 메서드 {totalMethods}");
        }
    }
}
