using System;
using System.Collections.Generic;
using System.IO;
using MelonLoader;
using MelonLoader.Utils;
using Il2CppSpine.Unity;
using UnityEngine;

namespace muse_dash_test
{
    // "skin test/character" 폴더의 {baseName}.png/.atlas/.json 세트로부터
    // 커스텀 SkeletonDataAsset을 만들어서 baseName별로 캐싱한다.
    // (원래 spine skin 모드에서 이식: 진단용 코드는 제외하고 주입 기능만 가져옴)
    public static class CustomSkinInjector
    {
        // 커스텀 스킨 원본 파일을 두는 루트 폴더. 게임 루트의 "skin test".
        // 종류별로 나눠 둔다: skin test/character/{baseName}/, skin test/note/{baseName}/ (규칙은 SkinFolderRules)
        public static readonly string SkinTestDirectory =
            Path.Combine(MelonEnvironment.GameRootDirectory, "skin test");

        // 모드가 자동 생성해줄 캐릭터 세트 목록. InjectHelper.TargetToBaseName의 값과 일치해야 한다.
        public static readonly string[] KnownBaseNames = { "char_3_black", "char_1_sleepy", "char_1_rock", "char_1_rampage", "char_3_violin" };

        // 옛 배치(skin test/{baseName}/)에서 읽었다고 알린 세트. 세트마다 한 번만 알린다.
        private static readonly HashSet<string> WarnedLegacy = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// 실제로 읽을 세트 폴더입니다. 새 배치(<c>skin test/{character|note}/{세트}</c>)를 먼저 보고, 없으면 옛 배치(<c>skin test/{세트}</c>)를 봅니다.
        /// 옛 배치를 쓰면 세트마다 한 번 옮기라고 알립니다.
        /// </summary>
        public static string GetSetDirectory(SkinKind kind, string baseName)
        {
            string dir = SkinFolderRules.ResolveSetDirectory(SkinTestDirectory, kind, baseName, File.Exists, out bool usedLegacy);
            if (usedLegacy && WarnedLegacy.Add(kind + "|" + baseName))
            {
                ModLogger.Msg($"[CustomSkinInjector] {baseName}: 옛 위치({dir})에서 읽었습니다. " +
                              $"{SkinFolderRules.PreferredSetDirectory(SkinTestDirectory, kind, baseName)}로 옮기면 정리됩니다.");
            }
            return dir;
        }

        // skin test 루트 + character/·note/ + 알려진 캐릭터 세트 하위 폴더를 생성한다(이미 있으면 건너뜀).
        public static void EnsureSetFolders()
        {
            Directory.CreateDirectory(SkinTestDirectory);
            Directory.CreateDirectory(SkinFolderRules.KindDirectory(SkinTestDirectory, SkinKind.Character));
            Directory.CreateDirectory(SkinFolderRules.KindDirectory(SkinTestDirectory, SkinKind.Note));
            foreach (var baseName in KnownBaseNames)
            {
                // 옛 위치에 이미 세트가 있으면 새 빈 폴더를 만들지 않는다(같은 세트 폴더가 두 군데 보이면 헷갈린다).
                if (Directory.Exists(SkinFolderRules.LegacySetDirectory(SkinTestDirectory, baseName))) continue;
                Directory.CreateDirectory(SkinFolderRules.PreferredSetDirectory(SkinTestDirectory, SkinKind.Character, baseName));
            }
        }

        private static readonly Dictionary<string, SkeletonDataAsset> Cache = new Dictionary<string, SkeletonDataAsset>();

        /// <summary>이 주입기가 만들어 캐릭터에 씌운 에셋인지. 노트 스킨 주입기가 같은 오브젝트를 다시 바꾸지 않게 합니다.</summary>
        public static bool IsCachedAsset(SkeletonDataAsset asset)
        {
            if (asset == null) return false;
            foreach (var cached in Cache.Values)
            {
                if (cached != null && cached.Pointer == asset.Pointer) return true;
            }
            return false;
        }

        // 캐시 키(세트|역할)별로 처음 본 원본 머티리얼. 캐시가 비워진 뒤 다시 만들 때, 오브젝트가 이미 우리 에셋을 들고 있어
        // 원본 머티리얼을 다시 읽을 수 없으므로 여기서 꺼내 씁니다.
        private static readonly Dictionary<string, Material> SourceMaterials = new Dictionary<string, Material>();

        /// <summary>
        /// 캐릭터 스킨 에셋을 만들거나 캐시에서 꺼냅니다. <paramref name="role"/>은 같은 세트를 다른 머티리얼로 그리는 오브젝트를
        /// 나눕니다(본체 <c>main</c> / 동시치기 분신 <c>ghost</c>). <paramref name="materialSource"/>는 원본 스켈레톤의 아틀라스 머티리얼이며,
        /// 셰이더와 속성을 그대로 복사하고 텍스처만 스킨으로 바꿉니다(<see cref="BuildAsset"/>).
        /// </summary>
        public static SkeletonDataAsset GetOrBuild(string baseName, string role, Material materialSource)
        {
            string key = baseName + "|" + role;
            if (materialSource != null) SourceMaterials[key] = materialSource;

            // Unity Object의 == 오버로드 때문에, 네이티브 오브젝트가 파괴되면(씬 전환 중 회수 등)
            // 캐시에 값이 있어도 "죽은" 상태일 수 있다. 그런 경우 다시 만든다.
            if (Cache.TryGetValue(key, out var existing) && existing != null)
                return existing;

            if (materialSource == null && SourceMaterials.TryGetValue(key, out var remembered) && remembered != null)
                materialSource = remembered;

            var dir = GetSetDirectory(SkinKind.Character, baseName);
            var skeletonDataAsset = BuildAsset(
                baseName + ":" + role,
                Path.Combine(dir, baseName + ".png"),
                Path.Combine(dir, baseName + ".atlas"),
                Path.Combine(dir, baseName + ".json"),
                0.01f, // 원본 캐릭터 0.005의 2배. 스킨 파일 좌표가 원본의 절반이라는 관행(NoteSkinRules.HalfImportFactor)과 짝입니다.
                materialSource);
            if (skeletonDataAsset != null) Cache[key] = skeletonDataAsset;
            return skeletonDataAsset;
        }

        /// <summary>
        /// 원본 스켈레톤이 그려지는 아틀라스 머티리얼(첫 아틀라스의 <c>PrimaryMaterial</c>)입니다. 없거나 읽지 못하면 null입니다.
        /// <para>캐릭터 분신(<c>*_ghost</c>)은 본체와 같은 텍스처를 전용 셰이더 머티리얼(<c>char_1_rampage_ghost_Material</c>:
        /// <c>_ColorR/G/B</c>·<c>_Alpha</c>·<c>_Distortion</c>)로 그려 반투명 하늘색으로 보입니다. 이 머티리얼을 버리고
        /// <c>Spine/Skeleton</c>으로 만들면 분신이 본체처럼 불투명하게 나옵니다(2026-11-20 스크린샷).</para>
        /// </summary>
        public static Material PrimaryMaterialOf(SkeletonDataAsset asset)
        {
            try
            {
                var atlases = asset?.atlasAssets;
                if (atlases == null) return null;
                for (int i = 0; i < atlases.Length; i++)
                {
                    var atlas = atlases[i];
                    if (atlas == null) continue;
                    var material = atlas.PrimaryMaterial;
                    if (material != null) return material;
                }
            }
            catch (Exception ex)
            {
                ModLogger.Verbose($"[CustomSkinInjector] 원본 머티리얼 조회 예외: {ex.GetType().Name}: {ex.Message}");
            }
            return null;
        }

        /// <summary>
        /// png·atlas·json 세 파일로 런타임 <see cref="SkeletonDataAsset"/>을 만듭니다. 캐시하지 않으므로 호출자가 보관합니다.
        /// 파일이 없거나 만들지 못하면 null입니다. 캐릭터 스킨(scale 0.01)과 노트 스킨(원본 에셋의 scale)이 같이 씁니다.
        /// <para><paramref name="materialPropertySource"/>가 있으면 그 머티리얼을 복사해 텍스처만 바꿉니다(원본 셰이더·속성 유지).
        /// 없거나 그 방식으로 만들지 못하면 <c>Spine/Skeleton</c> 셰이더로 만듭니다.</para>
        /// </summary>
        public static SkeletonDataAsset BuildAsset(string label, string pngPath, string atlasPath, string jsonPath, float scale, Material materialPropertySource = null)
        {
            try
            {
                if (!File.Exists(pngPath) || !File.Exists(atlasPath) || !File.Exists(jsonPath))
                {
                    ModLogger.Msg($"[CustomSkinInjector] {label} 파일이 없습니다: {Path.GetDirectoryName(jsonPath)}");
                    return null;
                }

                if (CheckObfuscatedJson(jsonPath, label, out string warningMsg))
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
                        label, jsonPath, warningMsg);

                    ModLogger.Error(banner);
                }

                var atlasText = File.ReadAllText(atlasPath);

                var pngBytes = File.ReadAllBytes(pngPath);
                var texture = new Texture2D(2, 2, TextureFormat.RGBA32, false);
                // 아틀라스 페이지는 텍스처 이름으로 찾으므로 페이지 줄(0411_road.png → 0411_road)에 맞춥니다.
                texture.name = NoteSkinRules.AtlasTextureName(atlasText, Path.GetFileNameWithoutExtension(pngPath));
                ImageConversion.LoadImage(texture, pngBytes);
                texture.hideFlags = HideFlags.DontUnloadUnusedAsset;

                var atlasTextAsset = new TextAsset(atlasText);
                atlasTextAsset.hideFlags = HideFlags.DontUnloadUnusedAsset;

                var atlasAsset = CreateAtlasAsset(label, atlasTextAsset, texture, materialPropertySource, out string materialLabel);
                if (atlasAsset != null) atlasAsset.hideFlags = HideFlags.DontUnloadUnusedAsset;

                var jsonText = File.ReadAllText(jsonPath);
                var skeletonTextAsset = new TextAsset(jsonText);
                skeletonTextAsset.hideFlags = HideFlags.DontUnloadUnusedAsset;

                var skeletonDataAsset = SkeletonDataAsset.CreateRuntimeInstance(skeletonTextAsset, atlasAsset, true, scale);
                if (skeletonDataAsset == null || skeletonDataAsset.GetSkeletonData(true) == null)
                {
                    ModLogger.Msg($"[CustomSkinInjector] {label} 스켈레톤 데이터 생성 실패");
                    UnityEngine.Object.Destroy(texture);
                    UnityEngine.Object.Destroy(atlasTextAsset);
                    if (atlasAsset != null) UnityEngine.Object.Destroy(atlasAsset);
                    UnityEngine.Object.Destroy(skeletonTextAsset);
                    if (skeletonDataAsset != null) UnityEngine.Object.Destroy(skeletonDataAsset);
                    return null;
                }

                skeletonDataAsset.hideFlags = HideFlags.DontUnloadUnusedAsset;
                ModLogger.Verbose($"[CustomSkinInjector] {label} 커스텀 SkeletonDataAsset 생성 완료 (scale {scale}, {materialLabel})");
                return skeletonDataAsset;
            }
            catch (Exception ex)
            {
                ModLogger.Error($"[CustomSkinInjector] {label} 예외: " + ex);
                return null;
            }
        }

        /// <summary>
        /// 런타임 아틀라스를 만듭니다. 원본 머티리얼이 있으면 그것을 복사하고(셰이더·속성 유지), 실패하면 셰이더 방식으로 물러납니다.
        /// <paramref name="materialLabel"/>은 로그용으로 무엇으로 만들었는지 적습니다.
        /// </summary>
        private static SpineAtlasAsset CreateAtlasAsset(string label, TextAsset atlasTextAsset, Texture2D texture, Material materialPropertySource, out string materialLabel)
        {
            if (materialPropertySource != null)
            {
                string sourceName = DescribeMaterial(materialPropertySource);
                try
                {
                    var fromSource = SpineAtlasAsset.CreateRuntimeInstance(atlasTextAsset, new Texture2D[] { texture }, materialPropertySource, true);
                    if (fromSource != null)
                    {
                        materialLabel = "material=" + sourceName;
                        return fromSource;
                    }
                    ModLogger.Warning($"[CustomSkinInjector] {label}: 원본 머티리얼({sourceName})로 아틀라스를 만들지 못해 기본 셰이더로 만듭니다.");
                }
                catch (Exception ex)
                {
                    ModLogger.Warning($"[CustomSkinInjector] {label}: 원본 머티리얼({sourceName}) 복사 예외, 기본 셰이더로 만듭니다: {ex.GetType().Name}: {ex.Message}");
                }
            }

            var shader = Shader.Find("Spine/Skeleton");
            if (shader == null) shader = Shader.Find("Sprites/Default");
            materialLabel = "shader=" + (shader != null ? shader.name : "(null)");
            return SpineAtlasAsset.CreateRuntimeInstance(atlasTextAsset, new Texture2D[] { texture }, shader, true);
        }

        /// <summary>로그용 머티리얼 설명: <c>이름(셰이더)</c>.</summary>
        public static string DescribeMaterial(Material material)
        {
            if (material == null) return "(none)";
            try
            {
                var shader = material.shader;
                return material.name + "(" + (shader != null ? shader.name : "no shader") + ")";
            }
            catch (Exception ex)
            {
                return "(material 조회 예외: " + ex.GetType().Name + ")";
            }
        }

        /// <summary>
        /// 캐시된 모든 Spine SkeletonDataAsset 및 관련 네이티브 리소스를 파괴하고 캐시를 초기화합니다.
        /// </summary>
        public static void ClearCache()
        {
            try
            {
                foreach (var kvp in Cache)
                {
                    var asset = kvp.Value;
                    if (asset != null)
                    {
                        UnityEngine.Object.Destroy(asset);
                    }
                }
                Cache.Clear();
                SourceMaterials.Clear();
                ModLogger.Verbose("[CustomSkinInjector] Spine 스킨 캐시 및 네이티브 에셋을 전면 초기화했습니다.");
            }
            catch (Exception ex)
            {
                ModLogger.Warning($"[CustomSkinInjector] 스킨 캐시 초기화 중 예외: {ex.Message}");
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
