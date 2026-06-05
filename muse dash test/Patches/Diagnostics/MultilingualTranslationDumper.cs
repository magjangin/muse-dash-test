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
        /// 다국어 조회 및 주입 가이드 치트시트를 반환합니다.
        /// </summary>
        public static string GetCheatSheetMarkdown()
        {
            var sb = new StringBuilder();
            sb.AppendLine("# 📖 다국어 로컬라이징 조회 및 주입 개발 가이드");
            sb.AppendLine("이 문서는 Muse Dash 모드 개발 시 다국어(한국어, 영어, 일본어, 중국어 등) 탭 이름/곡 텍스트의 조회 및 주입 코드를 구현할 때 참조하는 개발 바이블 힌트 시트입니다.");
            sb.AppendLine();
            sb.AppendLine("## 1. 지원되는 주요 시스템 언어 코드 키");
            sb.AppendLine("- `\"Korean\"` : 🇰🇷 한국어");
            sb.AppendLine("- `\"English\"` : 🇺🇸 영어 (기본 폴백 권장)");
            sb.AppendLine("- `\"Japanese\"` : 🇯🇵 일본어");
            sb.AppendLine("- `\"ChineseSimplified\"` : 🇨🇳 중국어 간체");
            sb.AppendLine("- `\"ChineseTraditional\"` : 🇹🇼 중국어 번체");
            sb.AppendLine();
            sb.AppendLine("## 2. 다국어 딕셔너리 안전 조회 (C# 예제)");
            sb.AppendLine("IL2CPP 환경의 Dictionary는 키가 없을 때 직접 인덱서(`dict[key]`)로 조회하면 예외를 던지며 크래시가 발생할 수 있습니다. 반드시 아래의 안전 조회 패턴을 사용해야 합니다.");
            sb.AppendLine();
            sb.AppendLine("```csharp");
            sb.AppendLine("// 방법 A: TryGetValue 사용 (가장 안전하고 권장됨)");
            sb.AppendLine("var dict = tagInfo.customInfo.tag_name;");
            sb.AppendLine("if (dict != null && dict.TryGetValue(\"Korean\", out string koreanValue))");
            sb.AppendLine("{");
            sb.AppendLine("    MelonLogger.Msg($\"한국어 태그명 조회 성공: {koreanValue}\");");
            sb.AppendLine("}");
            sb.AppendLine("else if (dict != null && dict.TryGetValue(\"English\", out string englishFallback))");
            sb.AppendLine("{");
            sb.AppendLine("    MelonLogger.Msg($\"한국어 번역이 없어 영어로 폴백: {englishFallback}\");");
            sb.AppendLine("}");
            sb.AppendLine();
            sb.AppendLine("// 방법 B: ContainsKey로 존재 여부 확인 후 인덱서 접근");
            sb.AppendLine("if (dict != null && dict.ContainsKey(\"Korean\"))");
            sb.AppendLine("{");
            sb.AppendLine("    string val = dict[\"Korean\"];");
            sb.AppendLine("}");
            sb.AppendLine("```");
            sb.AppendLine();
            sb.AppendLine("## 3. 다국어 딕셔너리 생성 및 주입 (C# 예제)");
            sb.AppendLine("커스텀 태그를 주입할 때, 모든 유저의 로캘 환경에서 탭 이름이 빈 문자열(\"\")로 나오는 참사를 방지하기 위해 5대 주요 로캘 키를 반드시 채워서 사전을 주입해야 합니다.");
            sb.AppendLine();
            sb.AppendLine("```csharp");
            sb.AppendLine("// 1) IL2CPP System Dictionary 인스턴스 생성 (생성자 호출)");
            sb.AppendLine("var il2CppDict = new Il2CppSystem.Collections.Generic.Dictionary<string, string>();");
            sb.AppendLine();
            sb.AppendLine("// 2) 주요 5대 다국어 키와 번역값 주입");
            sb.AppendLine("il2CppDict.Add(\"Korean\", \"실험 모드\");");
            sb.AppendLine("il2CppDict.Add(\"English\", \"Experiment Mod\");");
            sb.AppendLine("il2CppDict.Add(\"Japanese\", \"実験モード\");");
            sb.AppendLine("il2CppDict.Add(\"ChineseSimplified\", \"实验模式\");");
            sb.AppendLine("il2CppDict.Add(\"ChineseTraditional\", \"實\u9a55\u200b\u200b\u200b\u200b모드\"); // 實\u9a55\u200b\u200b\u200b\u200b = 實驗");
            sb.AppendLine("il2CppDict.Remove(\"ChineseTraditional\"); // Clear and re-add to avoid Unicode escape rendering issues if any");
            sb.AppendLine("il2CppDict.Add(\"ChineseTraditional\", \"實驗模式\");");
            sb.AppendLine();
            sb.AppendLine("// 3) 태그 정보 인스턴스에 주입");
            sb.AppendLine("var customInfo = new CustomTagInfo();");
            sb.AppendLine("customInfo.tag_name = il2CppDict;");
            sb.AppendLine("```");
            sb.AppendLine();
            sb.AppendLine("---");
            sb.AppendLine();
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
                string dumpPath = Path.Combine(gameDir, "hwa", "album_tag_localization_reference.md");

                // 인덱스가 0이면 헤더와 가이드를 새로 쓰면서 시작합니다.
                bool isAppend = index != 0 && File.Exists(dumpPath);

                using (var writer = new StreamWriter(dumpPath, isAppend, Encoding.UTF8))
                {
                    if (!isAppend)
                    {
                        writer.Write(TranslationDumperUtils.GetCheatSheetMarkdown());
                        writer.WriteLine("# 📊 현재 로드된 앨범 태그 목록 및 다국어 덤프 데이터");
                        writer.WriteLine($"*덤프 생성 일시: {DateTime.Now}*");
                        writer.WriteLine();
                    }

                    string displayName = tag.name ?? "(null)";
                    string localName = tag.tagName ?? "(null)";

                    writer.WriteLine($"## 🏷️ [태그 인덱스: {index}] {displayName}");
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
