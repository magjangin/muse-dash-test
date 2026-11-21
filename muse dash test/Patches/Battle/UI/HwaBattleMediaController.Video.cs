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
                // 영상 속 오디오는 그대로 내보냅니다(사용자 결정 2026-11-21: 영상 소리를 들으며 싱크를 판단·지시함).
                // mp4에 곡 오디오 트랙이 있으면(il vento d'oro: AAC 301.5초) BGM과 같은 곡이 겹쳐 나오므로,
                // 영상을 BGM에 붙이는 보정(HwaSyncManager.CorrectVideoDrift)이 곧 두 소리의 어긋남을 줄이는 일입니다.
                videoPlayer.audioOutputMode = VideoAudioOutputMode.Direct;
                videoPlayer.playOnAwake = false;
                videoPlayer.skipOnDrop = true;
                videoPlayer.waitForFirstFrame = true;
                videoPlayer.url = mp4Path;
                videoPlayer.isLooping = true;
                // 여기(레디·고)서는 디코더만 준비합니다. 재생은 게임이 음악을 시작한 뒤 BGM 위치에서 시작합니다
                // (HwaSyncManager.TryStartVideoAtMusicTime). 예전에는 여기서 Play()해 음악보다 수 초 먼저 흘렀습니다.
                videoStartedAtMusic = false;
                videoPlayer.Prepare();
                ModLogger.Msg("[HwaBattleMediaController.Video] BGA 비디오 준비를 시작했습니다(음악 시작 때 BGM 위치에서 재생, 영상 오디오 켬).");

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
