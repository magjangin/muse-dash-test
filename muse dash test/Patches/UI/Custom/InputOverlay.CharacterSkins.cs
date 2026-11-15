using System;
using System.Collections.Generic;
using System.Text;

namespace muse_dash_test
{
    // config.txt [8] 커스텀 캐릭터 스킨: 'skin test/character/{세트}' 스킨을 세트마다 켜고 끕니다.
    // 어느 캐릭터에 어느 세트를 씌우는지는 InjectHelper.TargetToBaseName이 정하고, 여기는 켜짐/꺼짐만 둡니다.
    // 기능 전체 스위치는 MelonPreferences의 EnableSpineSkin이며, 둘 다 켜져 있어야 씌웁니다.
    public static partial class InputOverlay
    {
        /// <summary>
        /// 파일에 적히는 항목 이름의 앞부분. 뒤에 세트 이름이 붙습니다(예: <c>커스텀 스킨 char_1_rampage=true</c>).
        /// 비교는 <see cref="CharacterSkinKeyPrefix"/>(공백 제거)로 하므로 두 상수를 함께 고치세요.
        /// </summary>
        private const string CharacterSkinKeyPrefixText = "커스텀 스킨 ";
        private const string CharacterSkinKeyPrefix = "커스텀스킨";

        /// <summary>세트 이름 → 켜짐. 파일에 항목이 없는 세트는 켜진 것으로 봅니다(예전 동작 그대로).</summary>
        private static readonly Dictionary<string, bool> characterSkinEnabled =
            new Dictionary<string, bool>(StringComparer.OrdinalIgnoreCase);

        /// <summary>이 캐릭터 스킨 세트를 씌워도 되는지. <see cref="InjectHelper.TryInject"/>가 봅니다.</summary>
        public static bool IsCharacterSkinEnabled(string baseName)
        {
            if (string.IsNullOrEmpty(baseName)) return true;
            return !characterSkinEnabled.TryGetValue(baseName, out bool enabled) || enabled;
        }

        /// <summary>비교용 키(공백 제거·소문자). <see cref="IsCanonicalConfigText"/>의 누락 검사에 씁니다.</summary>
        private static IEnumerable<string> CharacterSkinCanonicalKeys()
        {
            foreach (string baseName in CustomSkinInjector.KnownBaseNames)
            {
                yield return (CharacterSkinKeyPrefix + baseName).ToLowerInvariant();
            }
        }

        /// <summary>
        /// <paramref name="strippedKey"/>(공백 제거·소문자)가 캐릭터 스킨 항목이면 값을 읽고 true를 돌려줍니다.
        /// 알려진 세트가 아닌 이름은 무시합니다(연결표에 없는 세트라 켜도 씌울 캐릭터가 없음).
        /// </summary>
        private static bool TryParseCharacterSkinEntry(string strippedKey, string val, string rawKey)
        {
            if (!strippedKey.StartsWith(CharacterSkinKeyPrefix, StringComparison.Ordinal)) return false;

            string baseName = strippedKey.Substring(CharacterSkinKeyPrefix.Length);
            foreach (string known in CustomSkinInjector.KnownBaseNames)
            {
                if (!string.Equals(known, baseName, StringComparison.OrdinalIgnoreCase)) continue;

                characterSkinEnabled[known] = ParseBool(val, rawKey, IsCharacterSkinEnabled(known));
                return true;
            }

            ModLogger.Warning($"[InputOverlay] '{rawKey}': '{baseName}'은 모드가 아는 캐릭터 스킨이 아닙니다. " +
                              $"({string.Join(", ", CustomSkinInjector.KnownBaseNames)} 중 하나)");
            return true;
        }

        /// <summary>파싱 요약 로그에 붙일 꺼진 세트 목록. 다 켜져 있으면 "없음".</summary>
        private static string DisabledCharacterSkinsSummary()
        {
            var off = new List<string>();
            foreach (string baseName in CustomSkinInjector.KnownBaseNames)
            {
                if (!IsCharacterSkinEnabled(baseName)) off.Add(baseName);
            }
            return off.Count == 0 ? "없음" : string.Join("/", off);
        }

        /// <summary>[8] 섹션을 씁니다. <see cref="WriteConfigFile"/>만 부릅니다.</summary>
        private static void AppendCharacterSkinSection(StringBuilder sb, string onOff)
        {
            sb.AppendLine(Rule);
            sb.AppendLine("#  [8] 커스텀 캐릭터 스킨");
            sb.AppendLine(Rule);
            sb.AppendLine("#  'skin test/character/{세트}' 폴더의 스킨을 세트마다 켜고 끕니다.");
            sb.AppendLine("#  false면 그 캐릭터는 게임 원래 모습으로 나옵니다. 다음 판부터 반영됩니다.");
            sb.AppendLine("#  (어느 캐릭터에 어느 세트가 씌워지는지는 모드에 정해져 있습니다)");
            sb.AppendLine($"#  값 형식 : {onOff}");
            sb.AppendLine(ThinRule);
            foreach (string baseName in CustomSkinInjector.KnownBaseNames)
            {
                sb.AppendLine($"{CharacterSkinKeyPrefixText}{baseName}={IsCharacterSkinEnabled(baseName).ToString().ToLower()}");
            }
        }
    }
}
