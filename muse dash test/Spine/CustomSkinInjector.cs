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
                    MelonLogger.Msg($"[CustomSkinInjector] {baseName} 파일이 없습니다: {dir}");
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

                var jsonText = File.ReadAllText(jsonPath);
                var skeletonTextAsset = new TextAsset(jsonText);
                skeletonTextAsset.hideFlags = HideFlags.DontUnloadUnusedAsset;

                var skeletonDataAsset = SkeletonDataAsset.CreateRuntimeInstance(skeletonTextAsset, atlasAsset, true, 0.01f);
                if (skeletonDataAsset == null || skeletonDataAsset.GetSkeletonData(true) == null)
                {
                    MelonLogger.Msg($"[CustomSkinInjector] {baseName} 스켈레톤 데이터 생성 실패");
                    return null;
                }

                skeletonDataAsset.hideFlags = HideFlags.DontUnloadUnusedAsset;

                Cache[baseName] = skeletonDataAsset;
                MelonLogger.Msg($"[CustomSkinInjector] {baseName} 커스텀 SkeletonDataAsset 생성 완료");
                return skeletonDataAsset;
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[CustomSkinInjector] {baseName} 예외: " + ex);
                return null;
            }
        }
    }
}
