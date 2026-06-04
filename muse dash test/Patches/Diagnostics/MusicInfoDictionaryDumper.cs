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
    /// 게임 내 앨범 태그 정보(DBMusicTag)를 가로채어 다국어 현지화 정보와 함께 마크다운 파일로 덤프하는 클래스입니다.
    /// 다른 곳에서도 다국어 딕셔너리 덤프 형식을 재사용할 수 있도록 설계되었습니다.
    /// </summary>
    public static class AlbumTagDumperUtils
    {
        /// <summary>
        /// 게임 내 언어 코드 문자열을 대응하는 국기 이모지와 한글/영문 설명이 가미된 친숙한 이름으로 변환합니다.
        /// </summary>
        /// <param name="langKey">게임 내 언어 코드 키 (예: "Korean", "ko_KR")</param>
        /// <returns>국기 이모지가 포함된 읽기 쉬운 언어 명칭</returns>
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
                case "zh_CN":
                case "cn":
                    return "🇨🇳 Chinese Simplified (중국어 간체)";
                case "ChineseTraditional":
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
        /// IL2CPP 제네릭 딕셔너리 형태의 다국어 번역 데이터를 읽기 쉬운 마크다운 테이블 문자열로 변환합니다.
        /// 다른 다국어 딕셔너리 출력이 필요한 모듈에서 호출하여 재사용할 수 있습니다.
        /// </summary>
        /// <param name="dict">IL2CPP 다국어 번역 딕셔너리</param>
        /// <returns>Markdown 표 형식으로 서식화된 문자열</returns>
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
                string dumpPath = Path.Combine(gameDir, "hwa", "album_tag_dump.md");

                // 인덱스가 0이면 헤더를 새로 쓰면서 시작합니다.
                bool isAppend = index != 0 && File.Exists(dumpPath);

                using (var writer = new StreamWriter(dumpPath, isAppend, Encoding.UTF8))
                {
                    if (!isAppend)
                    {
                        writer.WriteLine("# Muse Dash Album Tags & Multilingual Localization Dump");
                        writer.WriteLine($"*Generated on: {DateTime.Now}*");
                        writer.WriteLine();
                    }

                    string displayName = tag.name ?? "(null)";
                    string localName = tag.tagName ?? "(null)";

                    writer.WriteLine($"## 🏷️ [Tag Index: {index}] {displayName}");
                    writer.WriteLine();
                    writer.WriteLine("| Property | Value |");
                    writer.WriteLine("| :--- | :--- |");
                    writer.WriteLine($"| **Tag UID** | `{tag.tagUid ?? "(null)"}` |");
                    writer.WriteLine($"| **Default Name** | `{displayName}` |");
                    writer.WriteLine($"| **Korean Name (Local)** | `{localName}` |");
                    writer.WriteLine($"| **Icon Name** | `{tag.iconName ?? "(null)"}` |");
                    writer.WriteLine($"| **Is Custom Tag** | `{tag.isCustomTag}` |");
                    writer.WriteLine();

                    var customInfo = tag.customInfo;
                    if (customInfo != null)
                    {
                        writer.WriteLine("### 🌐 Multilingual Translations");
                        writer.WriteLine();
                        
                        // 공용 유틸리티 메소드를 사용하여 다국어 딕셔너리를 마크다운 표 구조로 서식화합니다.
                        string formattedDict = AlbumTagDumperUtils.FormatMultilingualDictionary(customInfo.tag_name);
                        writer.Write(formattedDict);
                        writer.WriteLine();

                        writer.WriteLine("### 🛠️ Custom Tag Details");
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
