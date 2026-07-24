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
            if (Cache.TryGetValue(baseName, out var existing) && existing != null)
                return existing;

            try
            {
                var dir = GetSetDirectory(baseName);
                if (!Directory.Exists(dir))
                {
                    MelonLogger.Warning($"[CustomSkinInjector] {baseName} 디렉터리가 없습니다: {dir}");
                    return null;
                }

                string pngPath = Path.Combine(dir, baseName + ".png");
                string atlasPath = Path.Combine(dir, baseName + ".atlas");
                string jsonPath = Path.Combine(dir, baseName + ".json");

                if (!File.Exists(pngPath))
                {
                    var pngs = Directory.GetFiles(dir, "*.png");
                    if (pngs.Length > 0) pngPath = pngs[0];
                }

                if (!File.Exists(atlasPath))
                {
                    var atlases = Directory.GetFiles(dir, "*.atlas");
                    if (atlases.Length == 0) atlases = Directory.GetFiles(dir, "*.atlas.txt");
                    if (atlases.Length == 0) atlases = Directory.GetFiles(dir, "*atlas*.txt");
                    if (atlases.Length > 0) atlasPath = atlases[0];
                }

                if (!File.Exists(jsonPath))
                {
                    var jsons = Directory.GetFiles(dir, "*.json");
                    if (jsons.Length == 0) jsons = Directory.GetFiles(dir, "*.json.txt");
                    if (jsons.Length == 0) jsons = Directory.GetFiles(dir, "*json*.txt");
                    if (jsons.Length > 0) jsonPath = jsons[0];
                }

                bool hasPng = File.Exists(pngPath);
                bool hasAtlas = File.Exists(atlasPath);
                bool hasJson = File.Exists(jsonPath);

                MelonLogger.Msg($"[CustomSkinInjector.Debug] [{baseName}] 파일 탐색 결과: PNG({hasPng})='{pngPath}', ATLAS({hasAtlas})='{atlasPath}', JSON({hasJson})='{jsonPath}'");

                if (!hasPng || !hasAtlas || !hasJson)
                {
                    MelonLogger.Warning($"[CustomSkinInjector] [{baseName}] 필수 스킨 파일이 없어 주입을 스킵합니다 (PNG={hasPng}, ATLAS={hasAtlas}, JSON={hasJson}) 경로: {dir}");
                    return null;
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

                byte[] rawSkeletonBytes = File.ReadAllBytes(jsonPath);
                bool isBinary = rawSkeletonBytes.Length > 0 && rawSkeletonBytes[0] != '{' && rawSkeletonBytes[0] != '[';

                MelonLogger.Msg($"[CustomSkinInjector.Debug] [{baseName}] 스켈레톤 포맷 판별: {(isBinary ? "BINARY (.skel)" : "TEXT JSON (.json)")}, 크기={rawSkeletonBytes.Length} bytes");

                TextAsset skeletonTextAsset;
                if (isBinary)
                {
                    string binaryStr = System.Text.Encoding.GetEncoding("iso-8859-1").GetString(rawSkeletonBytes);
                    skeletonTextAsset = new TextAsset(binaryStr);
                    skeletonTextAsset.name = baseName + ".skel.bytes";
                }
                else
                {
                    string jsonText = File.ReadAllText(jsonPath);
                    skeletonTextAsset = new TextAsset(jsonText);
                    skeletonTextAsset.name = baseName + ".json";
                }
                skeletonTextAsset.hideFlags = HideFlags.DontUnloadUnusedAsset;

                var skeletonDataAsset = SkeletonDataAsset.CreateRuntimeInstance(skeletonTextAsset, atlasAsset, true, 0.01f);
                if (skeletonDataAsset == null || skeletonDataAsset.GetSkeletonData(false) == null)
                {
                    MelonLogger.Warning($"[CustomSkinInjector] [{baseName}] 스켈레톤 데이터 생성 실패 (JSON/Binary 파싱 불일치 또는 아틀라스 키 무효)");
                    return null;
                }

                skeletonDataAsset.hideFlags = HideFlags.DontUnloadUnusedAsset;

                Cache[baseName] = skeletonDataAsset;
                MelonLogger.Msg($"[CustomSkinInjector] 🎉 [{baseName}] 커스텀 SkeletonDataAsset 로드/생성 성공!");
                return skeletonDataAsset;
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[CustomSkinInjector] [{baseName}] 스킨 생성 예외: " + ex);
                return null;
            }
        }
    }
}
