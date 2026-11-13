using MelonLoader;
using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Video;

namespace muse_dash_test
{
    public static partial class HwaBattleMediaController
    {
        // 비디오 BGA 쿼드용 공유 머티리얼(매 배틀 new Material 생성으로 인한 네이티브 누수 방지).
        private static Material sharedVideoMaterial;

        private static void TryPlayVideo(Camera mainCam, string folderPath)
        {
            try
            {
                if (!Directory.Exists(folderPath))
                {
                    return;
                }

                string[] mp4Files = Directory.GetFiles(folderPath, "*.mp4", SearchOption.AllDirectories);
                if (mp4Files == null || mp4Files.Length == 0)
                {
                    ModLogger.Msg($"[HwaBattleMediaController.Video] {folderPath} 폴더 및 하위 폴더에 mp4 파일이 없습니다.");
                    return;
                }

                Array.Sort(mp4Files, StringComparer.OrdinalIgnoreCase);
                string mp4Path = mp4Files[0];
                FileInfo mp4Info = new FileInfo(mp4Path);
                ModLogger.Msg($"[HwaBattleMediaController.Video] mp4 비디오 주입 대상 발견: path={mp4Path}, fileName={mp4Info.Name}, size={mp4Info.Length}");

                Transform existingQuad = mainCam.transform.Find("VideoBackgroundQuad");
                if (existingQuad != null && existingQuad.gameObject != null)
                {
                    UnityEngine.Object.Destroy(existingQuad.gameObject);
                    ModLogger.Msg("[HwaBattleMediaController.Video] 기존 잔존 VideoBackgroundQuad 오브젝트를 삭제했습니다.");
                }

                GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
                quad.name = "VideoBackgroundQuad";

                var col = quad.GetComponent("MeshCollider");
                if (col != null) UnityEngine.Object.Destroy(col);

                quad.transform.parent = mainCam.transform;
                quad.transform.localPosition = new Vector3(0f, 0f, 25f);

                float height = mainCam.orthographicSize * 2f;
                float width = height * mainCam.aspect;
                quad.transform.localScale = new Vector3(width, height, 1f);

                MeshRenderer renderer = quad.GetComponent<MeshRenderer>();
                Material videoMaterial = GetOrCreateSharedVideoMaterial();
                if (videoMaterial != null)
                {
                    renderer.sharedMaterial = videoMaterial;
                }
                else
                {
                    ModLogger.Warning("[HwaBattleMediaController.Video] 폴백 셰이더를 찾지 못하여 기본 생성된 머티리얼을 재사용합니다.");
                }

                renderer.sortingLayerName = "Background";
                renderer.sortingOrder = -17;

                VideoPlayer videoPlayer = quad.AddComponent<VideoPlayer>();
                videoPlayer.renderMode = VideoRenderMode.MaterialOverride;
                videoPlayer.targetMaterialRenderer = renderer;
                videoPlayer.targetMaterialProperty = "_MainTex";
                videoPlayer.audioOutputMode = VideoAudioOutputMode.Direct;
                videoPlayer.url = mp4Path;
                videoPlayer.isLooping = true;
                videoPlayer.Play();
                ModLogger.Msg("[HwaBattleMediaController.Video] 쿼드 머티리얼 기반 비디오 재생을 성공적으로 시작했습니다.");

                MelonCoroutines.Start(MonitorVideoPlayback(videoPlayer));
            }
            catch (Exception ex)
            {
                ModLogger.Error($"[HwaBattleMediaController.Video] 비디오 재생 중 오류 발생: {ex}");
            }
        }

        private static Material GetOrCreateSharedVideoMaterial()
        {
            if (sharedVideoMaterial != null)
            {
                return sharedVideoMaterial;
            }

            Shader shader = Shader.Find("Unlit/Texture")
                         ?? Shader.Find("Sprites/Default")
                         ?? Shader.Find("UI/Default");
            if (shader == null)
            {
                return null;
            }

            sharedVideoMaterial = new Material(shader)
            {
                name = "HwaSharedVideoMaterial",
                hideFlags = HideFlags.DontUnloadUnusedAsset
            };
            ModLogger.Msg($"[HwaBattleMediaController.Video] '{shader.name}' 셰이더 기반 공유 비디오 머티리얼을 생성했습니다.");
            return sharedVideoMaterial;
        }

        private static IEnumerator MonitorVideoPlayback(VideoPlayer vp)
        {
            if (vp == null) yield break;
            yield return new WaitForSeconds(1.0f);
            if (vp == null) yield break;

            ModLogger.Msg($"[HwaBattleMediaController.Video.Debug] 1초 후 비디오 재생 상태 상세 점검:");
            ModLogger.Msg($"  - isPlaying: {vp.isPlaying}");
            ModLogger.Msg($"  - isPrepared: {vp.isPrepared}");
            ModLogger.Msg($"  - url: {vp.url}");
            ModLogger.Msg($"  - resolution: {vp.width}x{vp.height}");
            ModLogger.Msg($"  - frameCount: {vp.frameCount}");
            ModLogger.Msg($"  - frameRate: {vp.frameRate:0.00}");
            ModLogger.Msg($"  - playbackTime: {vp.time:0.00}s / duration: {vp.length:0.00}s");
        }
    }
}
