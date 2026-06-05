using MelonLoader;
using HarmonyLib;
using Il2CppAssets.Scripts.Database;
using Il2CppAssets.Scripts.Database.DataClass;
using System;
using System.IO;
using System.Text;

namespace muse_dash_test
{
    /// <summary>
    /// 게임 내 앨범 태그 정보(DBMusicTag)를 가로채어 다국어 현지화 정보와 함께 참조 가이드용 마크다운 파일로 덤프하는 클래스입니다.
    /// 개발자가 다국어 딕셔너리 조회 및 주입 코드를 작성할 때 힌트로 삼을 수 있는 상세 치트시트를 문서에 기재합니다.
    /// </summary>
    public static class TranslationDumperUtils
    {
        public static string GetLanguageDisplayName(string langKey)
        {
            if (string.IsNullOrEmpty(langKey)) return "Unknown";
            
            switch (langKey.Trim())
            {
                case "Korean":
                case "ko_KR":
                case "ko":
                    return "🇰🇷 Korean (한국어)";
                case "English":
                case "en_US":
                case "en":
                    return "🇺🇸 English (영어)";
                case "Japanese":
                case "ja_JP":
                case "jp":
                    return "🇯🇵 Japanese (일본어)";
                case "ChineseSimplified":
                case "ChineseS":
                case "zh_CN":
                case "cn":
                    return "🇨🇳 Chinese Simplified (중국어 간체)";
                case "ChineseTraditional":
                case "ChineseT":
                case "zh_TW":
                case "tw":
                    return "🇹🇼 Chinese Traditional (중국어 번체)";
                case "Spanish":
                case "es_ES":
                case "es":
                    return "🇪🇸 Spanish (스페인어)";
                case "Portuguese":
                case "pt_BR":
                case "pt":
                    return "🇵🇹 Portuguese (포르투갈어)";
                case "Russian":
                case "ru_RU":
                case "ru":
                    return "🇷🇺 Russian (러시아어)";
                case "French":
                case "fr_FR":
                case "fr":
                    return "🇫🇷 French (프랑스어)";
                case "German":
                case "de_DE":
                case "de":
                    return "🇩🇪 German (독일어)";
                case "Italian":
                case "it_IT":
                case "it":
                    return "🇮🇹 Italian (이탈리아어)";
                default:
                    return $"🌐 {langKey}";
            }
        }

        public static string FormatMultilingualDictionary(Il2CppSystem.Collections.Generic.Dictionary<string, string> dict)
        {
            if (dict == null || dict.Count == 0)
            {
                return "*No translations available or translation dictionary is null.*\n";
            }

            var sb = new StringBuilder();
            sb.AppendLine("| Language | Translation |");
            sb.AppendLine("| :--- | :--- |");

            var keys = dict.Keys;
            var enumerator = keys.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var key = enumerator.Current;
                var val = dict[key];
                string langLabel = GetLanguageDisplayName(key);
                sb.AppendLine($"| {langLabel} | `{val}` |");
            }

            return sb.ToString();
        }

        private static readonly string[][] LanguageAliasGroups = new string[][]
        {
            new string[] { "Korean", "ko_KR", "ko" },
            new string[] { "English", "en_US", "en" },
            new string[] { "Japanese", "ja_JP", "jp" },
            new string[] { "ChineseSimplified", "ChineseS", "zh_CN", "cn" },
            new string[] { "ChineseTraditional", "ChineseT", "zh_TW", "tw" }
        };

        public static System.Collections.Generic.List<string> GetLanguageAliases(string language)
        {
            var list = new System.Collections.Generic.List<string> { language };
            foreach (var group in LanguageAliasGroups)
            {
                bool matches = false;
                foreach (var alias in group)
                {
                    if (string.Equals(alias, language, StringComparison.OrdinalIgnoreCase))
                    {
                        matches = true;
                        break;
                    }
                }
                if (matches)
                {
                    foreach (var alias in group)
                    {
                        if (!list.Contains(alias))
                        {
                            list.Add(alias);
                        }
                    }
                    break;
                }
            }
            return list;
        }

        public static string GetLocalizedTagName(AlbumTagInfo tag, string language)
        {
            if (tag == null) return null;

            var aliases = GetLanguageAliases(language);

            try
            {
                if (tag.customInfo != null && tag.customInfo.tag_name != null)
                {
                    foreach (var alias in aliases)
                    {
                        if (tag.customInfo.tag_name.TryGetValue(alias, out string val))
                        {
                            return val;
                        }
                    }
                }
            }
            catch {}

            try
            {
                var configManager = Il2CppAssets.Scripts.PeroTools.Commons.Singleton<Il2CppAssets.Scripts.PeroTools.Managers.ConfigManager>.instance;
                if (configManager != null && !string.IsNullOrEmpty(tag.tagUid))
                {
                    string[] configs = { "languages", "language", "custom_tags", "music_tag" };
                    string[] cmpKeys = { "key", "uid", "id", "tagUid" };

                    foreach (var configName in configs)
                    {
                        foreach (var cmpKey in cmpKeys)
                        {
                            foreach (var alias in aliases)
                            {
                                try
                                {
                                    string val = configManager.GetConfigStringValue(configName, cmpKey, alias, (Il2CppSystem.String)tag.tagUid);
                                    if (!string.IsNullOrEmpty(val))
                                    {
                                        return val;
                                    }
                                }
                                catch {}
                            }
                        }
                    }
                }
            }
            catch {}

            return null;
        }
    }

    [HarmonyPatch(typeof(DBMusicTag), "AddAlbumTagData")]
    public class DBMusicTag_AddAlbumTagData_Patch
    {
        public static void Postfix(DBMusicTag __instance, int index, AlbumTagInfo tag)
        {
            try
            {
                if (tag == null) return;

                string gameDir = MelonLoader.Utils.MelonEnvironment.GameRootDirectory;
                string dumpPath = Path.Combine(gameDir, "hwa", "album_tag_localization_reference.md");

                // 인덱스가 0이면 헤더와 가이드를 새로 쓰면서 시작합니다.
                bool isAppend = index != 0 && File.Exists(dumpPath);

                using (var writer = new StreamWriter(dumpPath, isAppend, Encoding.UTF8))
                {
                    if (!isAppend)
                    {
                        writer.WriteLine("# 📊 현재 로드된 앨범 태그 목록 및 다국어 덤프 데이터");
                        writer.WriteLine($"*덤프 생성 일시: {DateTime.Now}*");
                        writer.WriteLine();
                    }

                    string displayName = tag.name ?? "(null)";
                    string localName = tag.tagName ?? "(null)";

                    string engName = TranslationDumperUtils.GetLocalizedTagName(tag, "English") ?? "(null)";
                    string korName = TranslationDumperUtils.GetLocalizedTagName(tag, "Korean") ?? localName;
                    string jpnName = TranslationDumperUtils.GetLocalizedTagName(tag, "Japanese") ?? "(null)";
                    string chsName = TranslationDumperUtils.GetLocalizedTagName(tag, "ChineseSimplified") ?? "(null)";
                    string chtName = TranslationDumperUtils.GetLocalizedTagName(tag, "ChineseTraditional") ?? "(null)";

                    writer.WriteLine($"## 🏷️ [태그 인덱스: {index}] {displayName}");
                    writer.WriteLine();
                    writer.WriteLine("| Property | Value |");
                    writer.WriteLine("| :--- | :--- |");
                    writer.WriteLine($"| **Tag UID** | `{tag.tagUid ?? "(null)"}` |");
                    writer.WriteLine($"| **Default Name** | `{displayName}` |");
                    writer.WriteLine($"| **English Name (Local)** | `{engName}` |");
                    writer.WriteLine($"| **Korean Name (Local)** | `{korName}` |");
                    writer.WriteLine($"| **Japanese Name (Local)** | `{jpnName}` |");
                    writer.WriteLine($"| **Chinese Simplified Name (Local)** | `{chsName}` |");
                    writer.WriteLine($"| **Chinese Traditional Name (Local)** | `{chtName}` |");
                    writer.WriteLine($"| **Icon Name** | `{tag.iconName ?? "(null)"}` |");
                    writer.WriteLine($"| **Is Custom Tag** | `{tag.isCustomTag}` |");
                    writer.WriteLine();

                    var customInfo = tag.customInfo;
                    if (customInfo != null)
                    {
                        writer.WriteLine("### 🌐 주입된 언어별 번역 테이블");
                        writer.WriteLine();
                        
                        string formattedDict = TranslationDumperUtils.FormatMultilingualDictionary(customInfo.tag_name);
                        writer.Write(formattedDict);
                        writer.WriteLine();

                        writer.WriteLine("### 🛠️ 커스텀 태그 부가 설정");
                        writer.WriteLine($"- **Picture Path / URL**: `{customInfo.tag_picture ?? "(null)"}`");
                        writer.WriteLine();
                    }
                    
                    writer.WriteLine("---");
                    writer.WriteLine();
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[APMod.Dumper] AddAlbumTagData Postfix 처리 중 예외 발생: {ex}");
            }
        }
    }
}
