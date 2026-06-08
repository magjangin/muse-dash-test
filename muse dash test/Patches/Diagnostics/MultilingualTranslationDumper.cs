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
    /// 게임 내 앨범 태그 정보(DBMusicTag)를 가로채어 다국어 현지화 정보와 함께 참조용 마크다운 파일로 덤프할 때 필요한 다국어 유틸리티 기능을 제공하는 클래스입니다.
    /// </summary>
    public static class TranslationDumperUtils
    {
        /// <summary>
        /// 언어 코드 또는 식별 명칭에 상응하는 국가 국기와 언어 표시명을 반환합니다.
        /// </summary>
        /// <param name="langKey">언어 코드 식별자 (예: "Korean", "ko_KR", "ChineseS" 등)</param>
        /// <returns>국기와 언어 한글 설명이 포함된 표시 문자열</returns>
        public static string GetLanguageDisplayName(string langKey)
        {
            if (string.IsNullOrEmpty(langKey))
            {
                return "Unknown";
            }
            
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

        /// <summary>
        /// 다국어 딕셔너리 맵 데이터를 마크다운 표 문자열 형태로 직렬화하여 반환합니다.
        /// </summary>
        /// <param name="dict">IL2CPP 언어-번역 값 딕셔너리</param>
        /// <returns>마크다운 테이블 구조의 문자열</returns>
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

        /// <summary>
        /// 매칭 가능한 주요 다국어 식별 코드 세트 정의
        /// </summary>
        private static readonly string[][] LanguageAliasGroups = new string[][]
        {
            new string[] { "Korean", "ko_KR", "ko" },
            new string[] { "English", "en_US", "en" },
            new string[] { "Japanese", "ja_JP", "jp" },
            new string[] { "ChineseSimplified", "ChineseS", "zh_CN", "cn" },
            new string[] { "ChineseTraditional", "ChineseT", "zh_TW", "tw" }
        };

        /// <summary>
        /// 주어진 언어 식별 코드와 연관된 모든 별칭(Alias) 키 목록을 반환합니다.
        /// </summary>
        /// <param name="language">조회하려는 기본 언어 키</param>
        /// <returns>해당 언어의 별칭 목록 (예: "ChineseSimplified" -> "ChineseSimplified", "ChineseS", "zh_CN", "cn")</returns>
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

        /// <summary>
        /// 앨범 태그 정보에 대한 다국어 현지화 번역명을 다양한 경로(커스텀 태그 딕셔너리 및 게임 내 ConfigManager 테이블)를 통해 복합 탐색하여 반환합니다.
        /// </summary>
        /// <param name="tag">번역을 찾을 앨범 태그 객체</param>
        /// <param name="language">요청할 대상 언어 식별 키 (예: "Korean", "Japanese" 등)</param>
        /// <returns>탐색된 현지화 번역명 문자열. 찾지 못할 시 null 반환</returns>
        public static string GetLocalizedTagName(AlbumTagInfo tag, string language)
        {
            if (tag == null)
            {
                return null;
            }

            var aliases = GetLanguageAliases(language);

            // 1. 주입된 커스텀 태그 딕셔너리에서 별칭으로 다국어 이름 우선 조회
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

            // 2. 게임 내부 내장 ConfigManager 테이블에서 매칭되는 현지화 설정 문자열 조회
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

    /// <summary>
    /// 게임 내 DBMusicTag.AddAlbumTagData 실행 시점에 개입하여, 로드되는 모든 태그의 정보를 다국어 메타데이터와 함께 가이드 마크다운 파일로 내보내는 Harmony 패치 클래스입니다.
    /// </summary>
    [HarmonyPatch(typeof(DBMusicTag), "AddAlbumTagData")]
    public class DBMusicTag_AddAlbumTagData_Patch
    {
        private static readonly bool EnableLocalizationReferenceDump = false;

        /// <summary>
        /// DBMusicTag.AddAlbumTagData 호출 완료 후 각 태그의 정보를 마크다운 파일에 추가 기록합니다.
        /// </summary>
        /// <param name="__instance">DBMusicTag 대상 인스턴스</param>
        /// <param name="index">태그 고유 인덱스</param>
        /// <param name="tag">등록 완료된 앨범 태그 정보 인스턴스</param>
        public static void Postfix(DBMusicTag __instance, int index, AlbumTagInfo tag)
        {
            // album_tag_localization_reference.md 생성은 현재 비활성화합니다.
            // 필요할 때 EnableLocalizationReferenceDump를 true로 바꾸면 아래 덤프 로직을 다시 사용할 수 있습니다.
            if (!EnableLocalizationReferenceDump)
            {
                return;
            }

            try
            {
                if (tag == null)
                {
                    return;
                }

                string gameDir = MelonLoader.Utils.MelonEnvironment.GameRootDirectory;
                string dumpPath = Path.Combine(gameDir, "hwa", "album_tag_localization_reference.md");

                // 첫 번째 태그(인덱스 0)가 등록될 때 파일을 초기화하여 새로 작성합니다.
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

                    // 각 대상 언어별 현지화 명칭을 복합 조회
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

                    // 주입된 사용자 정의 메타데이터가 존재할 경우 번역 딕셔너리와 설정 정보를 추가 출력
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
