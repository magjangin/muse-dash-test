using MelonLoader;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using Il2CppAssets.Scripts.PeroTools.Commons;

namespace muse_dash_test
{
    /// <summary>
    /// AlbumTagToggle의 수명 주기를 제어하여, 우리의 가상 태그가 표시될 때 
    /// 모드 어셈블리에 내장된 리소스 이미지(tag_icon.png)를 실시간 로드 및 주입하는 패치 클래스입니다.
    /// </summary>
    [HarmonyPatch(typeof(Il2Cpp.AlbumTagToggle), "Init")]
    public class AlbumTagToggle_Init_Patch
    {
        private static Texture2D cachedCustomTexture;
        private static Sprite cachedCustomSprite;
        private static bool hasTriedLoading = false;

        public static Texture2D GetCustomTexture()
        {
            if (hasTriedLoading)
            {
                return cachedCustomTexture;
            }

            hasTriedLoading = true;
            try
            {
                string gameDir = MelonLoader.Utils.MelonEnvironment.GameRootDirectory;
                string pngPath = Path.Combine(gameDir, "hwa tag image", "tag_icon.png");

                // 1. hwa tag image/tag_icon.png 가 없으면 내장 리소스에서 추출
                if (!File.Exists(pngPath))
                {
                    try
                    {
                        var assembly = System.Reflection.Assembly.GetExecutingAssembly();
                        string resourceName = "muse_dash_test.Resources.tag_icon.png";

                        using (var stream = assembly.GetManifestResourceStream(resourceName))
                        {
                            if (stream != null)
                            {
                                byte[] fileData = new byte[stream.Length];
                                stream.Read(fileData, 0, fileData.Length);
                                File.WriteAllBytes(pngPath, fileData);
                                MelonLogger.Msg($"[APMod.TagIcon] 내장 리소스 '{resourceName}'를 '{pngPath}'에 복사 및 추출 완료!");
                            }
                            else
                            {
                                MelonLogger.Error($"[APMod.TagIcon] 추출할 내장 리소스를 찾을 수 없습니다: {resourceName}");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MelonLogger.Error($"[APMod.TagIcon] 내장 리소스 추출 도중 예외 발생: {ex}");
                    }
                }

                // 2. 물리 파일이 존재하면 로드 진행
                if (File.Exists(pngPath))
                {
                    byte[] fileData = File.ReadAllBytes(pngPath);
                    Texture2D texture = new Texture2D(2, 2);
                    
                    if (UnityEngine.ImageConversion.LoadImage(texture, fileData))
                    {
                        texture.name = "CustomTagIconTexture";
                        texture.hideFlags |= HideFlags.DontUnloadUnusedAsset; // 유니티 GC 방지
                        cachedCustomTexture = texture;
                        MelonLogger.Msg($"[APMod.TagIcon] 물리 파일 '{pngPath}' 로드 및 텍스처 디코딩 성공! 해상도: {texture.width}x{texture.height}");
                        return texture;
                    }
                    else
                    {
                        MelonLogger.Error($"[APMod.TagIcon] 물리 파일 '{pngPath}'를 Texture2D로 디코딩하는 데 실패했습니다.");
                    }
                }
                else
                {
                    MelonLogger.Error($"[APMod.TagIcon] 로드할 파일 '{pngPath}'가 존재하지 않습니다.");
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[APMod.TagIcon] 물리 텍스처 로딩 및 생성 중 예외 발생: {ex}");
            }

            return null;
        }

        /// <summary>
        /// 캐싱된 Texture2D로부터 Sprite를 생성하고 영구 캐싱하여 반환합니다.
        /// </summary>
        public static Sprite GetCustomSprite()
        {
            if (cachedCustomSprite != null)
            {
                return cachedCustomSprite;
            }

            Texture2D texture = GetCustomTexture();
            if (texture != null)
            {
                try
                {
                    Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                    sprite.name = "CustomTagIconSprite";
                    sprite.hideFlags |= HideFlags.DontUnloadUnusedAsset; // 유니티 GC(UnloadUnusedAssets)에 의해 해제되는 현상 방지
                    cachedCustomSprite = sprite;
                    MelonLogger.Msg("[APMod.TagIcon] Texture2D로부터 영구 Sprite 생성 완료!");
                    return sprite;
                }
                catch (Exception ex)
                {
                    MelonLogger.Error($"[APMod.TagIcon] Sprite 생성 중 예외 발생: {ex}");
                }
            }
            return null;
        }

        /// <summary>
        /// AlbumTagToggle이 초기화된 후 실행됩니다.
        /// </summary>
        public static void Postfix(Il2Cpp.AlbumTagToggle __instance)
        {
            if (__instance == null) return;

            try
            {
                // 1. 해당 탭이 우리의 가상 탭인지 타입 안전(Type-Safe)하게 검증
                var tagInfo = __instance.tagInfo;
                if (tagInfo == null || tagInfo.tagUid != CustomTagRegistry.TagUidString)
                {
                    return;
                }

                MelonLogger.Msg("[APMod.TagIcon] 가상 태그 AlbumTagToggle 초기화 감지. 내장 텍스처 교체를 시작합니다.");

                // 2. tag_icon.png 텍스처 확보
                Texture2D customTexture = GetCustomTexture();
                if (customTexture == null)
                {
                    return;
                }

                // 3. UI 내부의 m_IconImg(RawImage) 속성에 직접 텍스처 오버라이딩 적용
                var iconImgComp = __instance.m_IconImg;
                if (iconImgComp != null)
                {
                    iconImgComp.texture = customTexture;
                    MelonLogger.Msg("[APMod.TagIcon] AlbumTagToggle m_IconImg(RawImage)의 텍스처를 커스텀 이미지로 오버라이드 완료!");
                }
                else
                {
                    MelonLogger.Warning("[APMod.TagIcon] AlbumTagToggle 인스턴스에서 m_IconImg 컴포넌트를 찾지 못했습니다.");
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[APMod.TagIcon] AlbumTagToggle Postfix 패치 처리 중 오류: {ex}");
            }
        }

    }

    /// <summary>
    /// 비동기적으로 아이콘 텍스처가 로드되어 지정될 때, 커스텀 텍스처로 대체해 주는 패치입니다.
    /// </summary>
    [HarmonyPatch(typeof(Il2Cpp.AlbumTagToggle), "SetIconAsync")]
    public class AlbumTagToggle_SetIconAsync_Patch
    {
        public static bool Prefix(Il2Cpp.AlbumTagToggle __instance, ref Texture2D tex)
        {
            if (__instance == null) return true;
            try
            {
                var tagInfo = __instance.tagInfo;
                if (tagInfo != null && tagInfo.tagUid == CustomTagRegistry.TagUidString)
                {
                    Texture2D customTexture = AlbumTagToggle_Init_Patch.GetCustomTexture();
                    if (customTexture != null)
                    {
                        tex = customTexture;
                        MelonLogger.Msg("[APMod.TagIcon] SetIconAsync 호출 감지 - 가상 태그의 아이콘 텍스처를 커스텀 이미지로 오버라이드합니다.");
                    }
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[APMod.TagIcon] SetIconAsync 패치 처리 중 예외 발생: {ex}");
            }
            return true;
        }
    }

    /// <summary>
    /// 토글 상태 변경(선택/비선택) 시 아이콘 텍스처 및 연출이 덮어쓰이는 문제를 방지하기 위한 패치입니다.
    /// </summary>
    [HarmonyPatch(typeof(Il2Cpp.AlbumTagToggle), "SetStateIcon")]
    public class AlbumTagToggle_SetStateIcon_Patch
    {
        public static void Postfix(Il2Cpp.AlbumTagToggle __instance, bool weekFree, bool newAlbum)
        {
            if (__instance == null) return;
            try
            {
                var tagInfo = __instance.tagInfo;
                if (tagInfo != null && tagInfo.tagUid == CustomTagRegistry.TagUidString)
                {
                    Texture2D customTexture = AlbumTagToggle_Init_Patch.GetCustomTexture();
                    if (customTexture != null && __instance.m_IconImg != null)
                    {
                        __instance.m_IconImg.texture = customTexture;
                    }
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[APMod.TagIcon] SetStateIcon 패치 처리 중 예외 발생: {ex}");
            }
        }
    }

}
// Touched to force MSBuild to package the new tag_icon.png resource
