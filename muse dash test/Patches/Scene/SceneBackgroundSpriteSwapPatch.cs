using MelonLoader;
using System;
using System.Collections.Generic;
using System.IO;
using muse_dash_test;
using UnityEngine;

/// <summary>
/// [테스트] 공식 곡에서 scene_04 배경 스프라이트를 이름이 같은 PNG로 바꿉니다.
///
/// 방식: `GameMusicScene.InitSceneObject(name, isActive)` Postfix에서 방금 만든 `scene_04` 루트의 `SpriteRenderer`를 훑어,
/// 원본 스프라이트 이름과 같은 `{이름}.png`가 `skin test/custom_background/`에 있으면 그 그림으로 교체합니다.
/// 원본의 PPU·pivot을 따라 월드 크기와 기준점이 같도록 맞춥니다(PPU = 원본 PPU × 새 폭 / 원본 폭).
/// 같은 PNG는 한 번만 읽어 캐시합니다. 원본 프리팹·스프라이트 파일은 건드리지 않습니다.
///
/// 적용 조건: 공식 곡만(`ShouldApplyExperimentChart`가 false일 때). 커스텀 곡에는 적용하지 않습니다.
/// 대상: 씬 이름이 정확히 "scene_04"인 경우만.
/// </summary>
[HarmonyLib.HarmonyPatch(typeof(Il2CppGameLogic.GameMusicScene), nameof(Il2CppGameLogic.GameMusicScene.InitSceneObject))]
public class SceneBackgroundSpriteSwapPatch
{
    private const string TargetSceneName = "scene_04";
    private const string CustomFolderName = "custom_background";

    // 스프라이트 이름 → 읽은 텍스처. 파일이 없으면 null을 넣어 다시 찾지 않습니다.
    private static readonly Dictionary<string, Texture2D> textureCache = new Dictionary<string, Texture2D>(StringComparer.OrdinalIgnoreCase);
    private static bool folderMissingLogged;

    public static void Postfix(string name)
    {
        try
        {
            if (name != TargetSceneName) return;
            // 공식 곡에서만 적용합니다. 커스텀 곡(실험 채보)에서는 건드리지 않습니다.
            if (CustomPlaySession.Current.ShouldApplyExperimentChart) return;

            string folder = Path.Combine(CustomSkinInjector.SkinTestDirectory, CustomFolderName);
            if (!Directory.Exists(folder))
            {
                if (!folderMissingLogged)
                {
                    folderMissingLogged = true;
                    ModLogger.Warning($"[SceneBackgroundSwap] 폴더가 없어 건너뜁니다: {folder}");
                }
                return;
            }

            // GameMusicScene은 MonoBehaviour가 아니므로, 방금 만든 씬 루트(SceneSubControl이 붙은 오브젝트)를 이름으로 찾습니다.
            int roots = 0;
            int swapped = 0;
            int total = 0;
            var swappedByName = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            foreach (var sceneSub in Resources.FindObjectsOfTypeAll<Il2CppAssets.Scripts.UI.GameMain.SceneSubControl>())
            {
                if (sceneSub == null || sceneSub.gameObject.name != name) continue;
                roots++;

                foreach (var renderer in sceneSub.GetComponentsInChildren<SpriteRenderer>(true))
                {
                    total++;
                    if (renderer == null || renderer.sprite == null) continue;

                    // 바꾼 뒤의 스프라이트는 이름이 비어 있어 여기서 다시 걸리지 않습니다(중복 교체 방지).
                    string spriteName = renderer.sprite.name;
                    if (string.IsNullOrEmpty(spriteName)) continue;

                    Texture2D texture = GetCustomTexture(folder, spriteName);
                    if (texture == null) continue;

                    renderer.sprite = MakeSpriteLike(renderer.sprite, texture);
                    swapped++;
                    swappedByName[spriteName] = swappedByName.TryGetValue(spriteName, out int count) ? count + 1 : 1;
                }
            }

            var names = new List<string>();
            foreach (var pair in swappedByName)
            {
                names.Add($"{pair.Key}x{pair.Value}");
            }

            ModLogger.Msg($"[SceneBackgroundSwap] {name}: 씬 루트 {roots}개, PNG로 교체 {swapped}개 (SpriteRenderer 전체 {total}개): {string.Join(", ", names)}");
        }
        catch (Exception ex)
        {
            ModLogger.Error($"SceneBackgroundSpriteSwapPatch 예외: {ex}");
        }
    }

    // 스프라이트 이름과 같은 PNG를 읽어 캐시합니다. 파일이 없거나 읽기에 실패하면 null입니다.
    private static Texture2D GetCustomTexture(string folder, string spriteName)
    {
        if (textureCache.TryGetValue(spriteName, out Texture2D cached)) return cached;

        Texture2D texture = null;
        string path = Path.Combine(folder, spriteName + ".png");
        if (File.Exists(path))
        {
            try
            {
                texture = new Texture2D(2, 2, TextureFormat.RGBA32, false);
                texture.name = spriteName;
                if (!ImageConversion.LoadImage(texture, File.ReadAllBytes(path)))
                {
                    ModLogger.Warning($"[SceneBackgroundSwap] PNG 디코딩 실패: {path}");
                    texture = null;
                }
            }
            catch (Exception ex)
            {
                ModLogger.Warning($"[SceneBackgroundSwap] PNG 읽기 실패: {path} ({ex.Message})");
                texture = null;
            }
        }

        textureCache[spriteName] = texture;
        return texture;
    }

    // 원본과 월드 크기·기준점이 같도록 새 스프라이트를 만듭니다.
    private static Sprite MakeSpriteLike(Sprite original, Texture2D texture)
    {
        Rect originalRect = original.rect;
        Vector2 pivot = new Vector2(original.pivot.x / originalRect.width, original.pivot.y / originalRect.height);
        float pixelsPerUnit = original.pixelsPerUnit * texture.width / originalRect.width;
        return Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), pivot, pixelsPerUnit);
    }
}
