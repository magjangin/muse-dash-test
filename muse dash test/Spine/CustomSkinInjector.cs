using System;
using System.Collections.Generic;
using System.IO;
using MelonLoader;
using MelonLoader.Utils;
using Il2CppSpine.Unity;
using UnityEngine;

namespace muse_dash_test
{
    // "skin test" 폴더의 {baseName}.png/.atlas/.json 세트로부터
    // 커스텀 SkeletonDataAsset을 만들어서 baseName별로 캐싱한다.
    // (원래 spine skin 모드에서 이식: 진단용 코드는 제외하고 주입 기능만 가져옴)
    public static class CustomSkinInjector
    {
        // 커스텀 스킨 원본 파일을 두는 루트 폴더. 게임 루트의 "skin test".
        // 실제 파일은 세트별 하위 폴더에 둔다: skin test/{baseName}/{baseName}.png/.atlas/.json
        public static readonly string SkinTestDirectory =
            Path.Combine(MelonEnvironment.GameRootDirectory, "skin test");

        // 모드가 자동 생성해줄 세트 목록. InjectHelper.TargetToBaseName의 값과 일치해야 한다.
        public static readonly string[] KnownBaseNames = { "char_3_black", "char_1_sleepy", "char_1_rock", "char_1_rampage", "char_3_violin" };

        // 특정 세트의 원본 파일이 들어가는 하위 폴더 경로.
        public static string GetSetDirectory(string baseName) => Path.Combine(SkinTestDirectory, baseName);

        // skin test 루트 + 알려진 모든 세트 하위 폴더를 생성한다(이미 있으면 건너뜀).
        public static void EnsureSetFolders()
        {
            Directory.CreateDirectory(SkinTestDirectory);
            foreach (var baseName in KnownBaseNames)
            {
                Directory.CreateDirectory(GetSetDirectory(baseName));
            }
        }

        private static readonly Dictionary<string, SkeletonDataAsset> Cache = new Dictionary<string, SkeletonDataAsset>();

        public static SkeletonDataAsset GetOrBuild(string baseName)
        {
            // Unity Object의 == 오버로드 때문에, 네이티브 오브젝트가 파괴되면(씬 전환 중 회수 등)
            // 캐시에 값이 있어도 "죽은" 상태일 수 있다. 그런 경우 다시 만든다.
            if (Cache.TryGetValue(baseName, out var existing) && existing != null)
                return existing;

            try
            {
                var dir = GetSetDirectory(baseName);
                var pngPath = Path.Combine(dir, baseName + ".png");
                var atlasPath = Path.Combine(dir, baseName + ".atlas");
                var jsonPath = Path.Combine(dir, baseName + ".json");

                if (!File.Exists(pngPath) || !File.Exists(atlasPath) || !File.Exists(jsonPath))
                {
                    ModLogger.Msg($"[CustomSkinInjector] {baseName} 파일이 없습니다: {dir}");
                    return null;
                }

                if (CheckObfuscatedJson(jsonPath, baseName, out string warningMsg))
                {
                    string banner = string.Format(
                        "\n================================================================================\n" +
                        "🚨 [CustomSkinInjector] 스킨 파일 난독화 / 바이너리 변형 경고 🚨\n" +
                        "--------------------------------------------------------------------------------\n" +
                        "  대상 스킨 : {0}\n" +
                        "  파일 경로 : {1}\n" +
                        "  감지 사유 : {2}\n" +
                        "--------------------------------------------------------------------------------\n" +
                        "  💡 안내: 해당 .json 파일이 암호화/난독화되어 있거나, .skel 바이너리를 확장자만\n" +
                        "     .json으로 변경한 파일입니다. 순정 Spine 파서 로드가 불가능합니다.\n" +
                        "================================================================================\n",
                        baseName, jsonPath, warningMsg);

                    ModLogger.Error(banner);
                }

                var pngBytes = File.ReadAllBytes(pngPath);
                var texture = new Texture2D(2, 2, TextureFormat.RGBA32, false);
                texture.name = baseName;
                ImageConversion.LoadImage(texture, pngBytes);
                texture.hideFlags = HideFlags.DontUnloadUnusedAsset;

                var atlasText = File.ReadAllText(atlasPath);
                var atlasTextAsset = new TextAsset(atlasText);
                atlasTextAsset.hideFlags = HideFlags.DontUnloadUnusedAsset;

                var shader = Shader.Find("Spine/Skeleton");
                if (shader == null) shader = Shader.Find("Sprites/Default");

                var atlasAsset = SpineAtlasAsset.CreateRuntimeInstance(atlasTextAsset, new Texture2D[] { texture }, shader, true);
                if (atlasAsset != null) atlasAsset.hideFlags = HideFlags.DontUnloadUnusedAsset;

                var jsonText = File.ReadAllText(jsonPath);
                var skeletonTextAsset = new TextAsset(jsonText);
                skeletonTextAsset.hideFlags = HideFlags.DontUnloadUnusedAsset;

                var skeletonDataAsset = SkeletonDataAsset.CreateRuntimeInstance(skeletonTextAsset, atlasAsset, true, 0.01f);
                if (skeletonDataAsset == null || skeletonDataAsset.GetSkeletonData(true) == null)
                {
                    ModLogger.Msg($"[CustomSkinInjector] {baseName} 스켈레톤 데이터 생성 실패");
                    return null;
                }

                skeletonDataAsset.hideFlags = HideFlags.DontUnloadUnusedAsset;

                Cache[baseName] = skeletonDataAsset;
                ModLogger.Msg($"[CustomSkinInjector] {baseName} 커스텀 SkeletonDataAsset 생성 완료");
                return skeletonDataAsset;
            }
            catch (Exception ex)
            {
                ModLogger.Error($"[CustomSkinInjector] {baseName} 예외: " + ex);
                return null;
            }
        }

        /// <summary>
        /// JSON 스킨 파일이 난독화되어 있거나 바이너리/암호화된 형식인지 감지하고, 감지 시 경고 이유를 반환합니다.
        /// </summary>
        public static bool CheckObfuscatedJson(string jsonPath, string baseName, out string reason)
        {
            reason = null;
            if (!File.Exists(jsonPath)) return false;

            try
            {
                byte[] bytes = File.ReadAllBytes(jsonPath);
                if (bytes.Length == 0)
                {
                    reason = "파일 크기가 0바이트입니다.";
                    return true;
                }

                // 1. 바이너리 / 암호화 / 비-JSON 헤더 탐색
                int firstCharIdx = 0;
                while (firstCharIdx < bytes.Length && (bytes[firstCharIdx] == ' ' || bytes[firstCharIdx] == '\t' || bytes[firstCharIdx] == '\r' || bytes[firstCharIdx] == '\n'))
                {
                    firstCharIdx++;
                }

                if (firstCharIdx >= bytes.Length || (bytes[firstCharIdx] != '{' && bytes[firstCharIdx] != '['))
                {
                    reason = $"JSON 시작 문자가 아닙니다 ('{{' 또는 '[' 대신 0x{bytes[firstCharIdx]:X2} 감지 -> 바이너리 .skel 또는 암호화 파일일 가능성 높음)";
                    return true;
                }

                // 2. Spine 키 구성 검사 (bones, slots, skins, skeleton)
                string text = System.Text.Encoding.UTF8.GetString(bytes);

                bool hasBones = text.Contains("\"bones\"") || text.Contains("'bones'");
                bool hasSlots = text.Contains("\"slots\"") || text.Contains("'slots'");
                bool hasSkins = text.Contains("\"skins\"") || text.Contains("'skins'");
                bool hasSkeleton = text.Contains("\"skeleton\"") || text.Contains("'skeleton'");

                if (!hasBones && !hasSlots && !hasSkins && !hasSkeleton)
                {
                    reason = "Spine 핵심 필드(\"bones\", \"slots\", \"skins\", \"skeleton\")가 탐색되지 않았습니다. 변형/난독화된 JSON 구조입니다.";
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                reason = $"파일 탐색 예외: {ex.Message}";
                return true;
            }
        }
    }
}
