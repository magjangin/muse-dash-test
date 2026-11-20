using System;
using System.Collections.Generic;
using Il2CppAssets.Scripts.PeroTools.Nice.Datas;
using Il2CppAssets.Scripts.PeroTools.Nice.Interface;

namespace muse_dash_test
{
    /// <summary>
    /// 저장 직전 <c>Account</c>의 '마지막 선택' 값이 모드 전용 태그·앨범·곡을 가리키면 바닐라 값으로 바꿔 저장하고,
    /// 저장이 끝나면 원래 값으로 되돌립니다(모드 화면은 그대로). 규칙과 이유는 <see cref="SaveSelectionRules"/>에 있습니다.
    ///
    /// <para>되돌리기는 <c>DataManager.Save</c> Postfix에서 합니다. 원본 Save가 예외를 던지면 Postfix가 건너뛰어져
    /// 메모리의 선택이 바닐라 값으로 남는데, 그러면 곡 선택 화면이 바닐라 기본 태그로 돌아갈 뿐 깨지지는 않습니다.</para>
    /// </summary>
    internal static class SaveSelectionGuard
    {
        // 이번 실행에서 본 마지막 바닐라 선택(셋 다 바닐라였던 때). 모드를 뺀 뒤 그 곡으로 돌아가게 합니다.
        private static readonly Dictionary<string, string> lastVanilla = new Dictionary<string, string>();

        // 저장 뒤 되돌릴 값. 원래 객체(박싱된 int, Il2Cpp 문자열)를 그대로 다시 넣어 형식이 바뀌지 않게 합니다.
        private static readonly List<KeyValuePair<IVariable, Il2CppSystem.Object>> pendingRestore =
            new List<KeyValuePair<IVariable, Il2CppSystem.Object>>();

        // 같은 선택으로 저장이 반복될 때 Info 로그를 한 번만 남기려고 둡니다.
        private static string lastLoggedSelection;

        public static void SanitizeForSave(DataManager dataManager)
        {
            pendingRestore.Clear();
            try
            {
                if (dataManager == null) return;
                var account = (DataObject)dataManager["Account"];
                if (account == null) return;

                var current = new Dictionary<string, string>();
                var variables = new Dictionary<string, IVariable>();
                foreach (string key in SaveSelectionRules.Keys)
                {
                    IVariable variable = account[key];
                    if (variable == null) continue;
                    var value = VariableUtils.GetResult<Il2CppSystem.Object>(variable);
                    current[key] = value?.ToString();
                    variables[key] = variable;
                }

                if (!SaveSelectionRules.AnyPointsToModContent(current))
                {
                    if (current.Count == SaveSelectionRules.Keys.Length)
                    {
                        foreach (var pair in current) lastVanilla[pair.Key] = pair.Value;
                    }
                    return;
                }

                var replaced = new List<string>();
                foreach (var pair in variables)
                {
                    string replacement = SaveSelectionRules.ReplacementFor(pair.Key, lastVanilla);
                    if (replacement == null) continue;

                    Il2CppSystem.Object original = VariableUtils.GetResult<Il2CppSystem.Object>(pair.Value);
                    Il2CppSystem.Object newValue = pair.Key == SaveSelectionRules.TagIndexKey
                        ? BoxInt(int.Parse(replacement, System.Globalization.CultureInfo.InvariantCulture))
                        : (Il2CppSystem.Object)replacement;

                    VariableUtils.SetResult(pair.Value, newValue);
                    pendingRestore.Add(new KeyValuePair<IVariable, Il2CppSystem.Object>(pair.Value, original));
                    replaced.Add($"{pair.Key}={current[pair.Key]}→{replacement}");
                }

                string summary = string.Join(", ", replaced);
                if (summary != lastLoggedSelection)
                {
                    lastLoggedSelection = summary;
                    ModLogger.Msg($"[SaveSelectionGuard] 모드 전용 선택을 바닐라 값으로 저장합니다(저장 뒤 되돌림): {summary}");
                }
            }
            catch (Exception ex)
            {
                ModLogger.Warning($"[SaveSelectionGuard] 저장 전 선택 정리 실패: {ex.GetType().Name}: {ex.Message}");
                RestoreAfterSave();
            }
        }

        public static void RestoreAfterSave()
        {
            if (pendingRestore.Count == 0) return;
            try
            {
                foreach (var pair in pendingRestore)
                {
                    VariableUtils.SetResult(pair.Key, pair.Value);
                }
            }
            catch (Exception ex)
            {
                ModLogger.Warning($"[SaveSelectionGuard] 저장 뒤 선택 되돌리기 실패: {ex.GetType().Name}: {ex.Message}");
            }
            finally
            {
                pendingRestore.Clear();
            }
        }

        private static Il2CppSystem.Object BoxInt(int value)
        {
            var boxed = new Il2CppSystem.Int32 { m_value = value };
            return boxed.BoxIl2CppObject();
        }
    }
}
