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

        // 저장 뒤 되돌릴 값(키 → 읽어 둔 원래 값의 문자열). 되돌릴 때 키의 형식(int/문자열)에 맞춰 다시 만듭니다.
        private static readonly List<KeyValuePair<IVariable, KeyValuePair<string, string>>> pendingRestore =
            new List<KeyValuePair<IVariable, KeyValuePair<string, string>>>();

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
                    current[key] = ReadAsText(key, variable);
                    variables[key] = variable;
                }

                ModLogger.Verbose($"[SaveSelectionGuard] 저장 직전 선택: {DescribeValues(current)}");

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

                    string original = current[pair.Key];
                    if (original == null) continue; // 원래 값을 못 읽었으면 되돌릴 수 없으니 건드리지 않습니다.

                    Write(pair.Key, pair.Value, replacement);
                    pendingRestore.Add(new KeyValuePair<IVariable, KeyValuePair<string, string>>(
                        pair.Value, new KeyValuePair<string, string>(pair.Key, original)));
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
                    Write(pair.Value.Key, pair.Key, pair.Value.Value);
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

        /// <summary>
        /// 값을 문자열로 읽습니다. 태그 인덱스는 <c>GetResult&lt;int&gt;</c>로 읽어야 합니다:
        /// <c>GetResult&lt;Il2CppSystem.Object&gt;</c>로 읽으면 박싱되지 않은 int가 객체로 해석돼 엉뚱한 수(-524941344)가 나왔습니다(2026-11-20 실기).
        /// 읽지 못하면 null입니다.
        /// </summary>
        private static string ReadAsText(string key, IVariable variable)
        {
            try
            {
                if (key == SaveSelectionRules.TagIndexKey)
                {
                    return VariableUtils.GetResult<int>(variable).ToString(System.Globalization.CultureInfo.InvariantCulture);
                }
                var value = VariableUtils.GetResult<Il2CppSystem.Object>(variable);
                return value?.ToString();
            }
            catch (Exception ex)
            {
                ModLogger.Verbose($"[SaveSelectionGuard] {key} 읽기 실패: {ex.GetType().Name}: {ex.Message}");
                return null;
            }
        }

        private static string DescribeValues(Dictionary<string, string> values)
        {
            var parts = new List<string>();
            foreach (var pair in values) parts.Add(pair.Key + "=" + (pair.Value ?? "(null)"));
            return parts.Count > 0 ? string.Join(", ", parts) : "(Account에 선택 키 없음)";
        }

        /// <summary>키의 형식(태그 인덱스 = int, 나머지 = 문자열)에 맞춰 값을 씁니다.</summary>
        private static void Write(string key, IVariable variable, string text)
        {
            Il2CppSystem.Object value = key == SaveSelectionRules.TagIndexKey
                ? BoxInt(int.Parse(text, System.Globalization.CultureInfo.InvariantCulture))
                : (Il2CppSystem.Object)text;
            VariableUtils.SetResult(variable, value);
        }

        private static Il2CppSystem.Object BoxInt(int value)
        {
            var boxed = new Il2CppSystem.Int32 { m_value = value };
            return boxed.BoxIl2CppObject();
        }
    }
}
