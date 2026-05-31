using MelonLoader;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;

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
        private static bool hasTriedLoading = false;

        /// <summary>
        /// 모드 어셈블리에 내장된 리소스(Embedded Resource)에서 tag_icon.png 파일을 읽어 유니티 Texture2D로 디코딩하고 정적 캐싱합니다.
        /// </summary>
        private static Texture2D GetCustomTexture()
        {
            if (hasTriedLoading)
            {
                return cachedCustomTexture;
            }

            hasTriedLoading = true;
            try
            {
                var assembly = System.Reflection.Assembly.GetExecutingAssembly();
                string resourceName = "muse_dash_test.Resources.tag_icon.png";

                using (var stream = assembly.GetManifestResourceStream(resourceName))
                {
                    if (stream == null)
                    {
                        MelonLogger.Error($"[APMod.TagIcon] 내장 리소스를 찾을 수 없습니다: {resourceName}");
                        return null;
                    }

                    byte[] fileData = new byte[stream.Length];
                    stream.Read(fileData, 0, fileData.Length);

                    Texture2D texture = new Texture2D(2, 2);
                    
                    // UnityEngine.ImageConversion을 통해 원시 바이너리 바이트 데이터를 Unity Texture2D로 디코딩합니다.
                    if (UnityEngine.ImageConversion.LoadImage(texture, fileData))
                    {
                        texture.name = "CustomTagIconTexture";
                        cachedCustomTexture = texture;
                        MelonLogger.Msg($"[APMod.TagIcon] 내장 리소스 '{resourceName}' 로드 및 텍스처 디코딩 완료! 해상도: {texture.width}x{texture.height}");
                        return texture;
                    }
                }
                
                MelonLogger.Error("[APMod.TagIcon] tag_icon.png 바이너리를 Texture2D로 변환하는 데 실패했습니다.");
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[APMod.TagIcon] 내장 텍스처 로딩 및 생성 중 치명적 예외: {ex}");
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
}
// Touched to force MSBuild to package the new tag_icon.png resource
