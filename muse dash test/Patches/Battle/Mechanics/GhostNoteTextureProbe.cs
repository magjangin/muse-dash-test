using System;
using System.Collections.Generic;
using System.IO;
using Il2Cpp;
using MelonLoader;
using UnityEngine;

namespace muse_dash_test
{
    /// <summary>
    /// "고스트 노트는 무슨 색인가"에 답하는 진단입니다.
    ///
    /// 컬러 타임라인의 rgb는 전 구간 (1,1,1)이었는데, Spine의 슬롯 색은 텍스처에 <b>곱하는</b> 값입니다.
    /// 1은 "색을 안 건드림"이라는 뜻이라, 눈에 보이는 색은 전부 아틀라스 텍스처 픽셀에 있습니다.
    /// 그래서 슬롯 → 어태치먼트 → 아틀라스 영역 → 페이지 텍스처까지 따라가서 실제 픽셀을 셉니다.
    ///
    /// 아틀라스 텍스처는 보통 CPU에서 읽을 수 없게(isReadable=false) 올라오므로
    /// RenderTexture로 한 번 복사해 읽습니다. 원본은 건드리지 않고, 복사본은 다 쓰면 버립니다.
    /// </summary>
    internal static class GhostNoteTextureProbe
    {
        private const string Tag = "[GhostNote.Texture]";

        /// <summary>대표색 히스토그램의 채널당 칸 수. 32칸이면 채널당 8단계씩 뭉쳐 봅니다.</summary>
        private const int ColorBuckets = 32;

        /// <summary>대표색으로 보고할 개수.</summary>
        private const int TopColors = 4;

        /// <summary>이 이상 큰 영역은 픽셀을 건너뛰며 셉니다(색 비율은 거의 안 변합니다).</summary>
        private const int SampleBudget = 65536;

        /// <summary>PNG로 통째 저장할 페이지 크기 상한.</summary>
        private const int MaxPagePixels = 4096 * 4096;

        /// <summary>불투명하다고 볼 알파 기준. 반투명 외곽선이 평균색을 흐리지 않게 합니다.</summary>
        private const float SolidAlpha = 0.5f;

        internal static void Dump(SpineActionController controller)
        {
            var readableByPage = new Dictionary<IntPtr, Texture2D>();

            try
            {
                var skeleton = controller.skeletonAnimation != null ? controller.skeletonAnimation.skeleton : null;
                var slots = skeleton != null ? skeleton.slots : null;
                var items = slots != null ? slots.Items : null;
                if (items == null)
                {
                    MelonLogger.Msg($"{Tag} 슬롯 목록을 얻지 못했습니다.");
                    return;
                }

                MelonLogger.Msg($"{Tag} ════ 고스트 노트 실제 색 조사 ════ (색 공간 {QualitySettings.activeColorSpace})");
                MelonLogger.Msg($"{Tag} 슬롯 색·어태치먼트 틴트는 텍스처에 곱해지는 값입니다. 1.000이면 텍스처 원본 색이 그대로 보입니다.");

                int count = Math.Min(slots.Count, items.Length);
                for (int i = 0; i < count; i++)
                {
                    DescribeSlot(items[i], i, readableByPage);
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Warning($"{Tag} 텍스처 조사 실패: {ex}");
            }
            finally
            {
                foreach (var pair in readableByPage)
                {
                    if (pair.Value == null) continue;
                    try { UnityEngine.Object.Destroy(pair.Value); } catch (Exception) { }
                }
            }
        }

        private static void DescribeSlot(Il2CppSpine.Slot slot, int index, Dictionary<IntPtr, Texture2D> readableByPage)
        {
            if (slot == null) return;

            string slotName = "(이름 없음)";
            try { slotName = slot.data != null ? slot.data.name : slotName; } catch (Exception) { }

            var attachment = GetAttachment(slot);
            if (attachment == null)
            {
                MelonLogger.Msg($"{Tag} 슬롯 #{index} \"{slotName}\" — 어태치먼트가 없습니다(이 슬롯은 안 그려집니다).");
                return;
            }

            string attachmentName = Safe(() => attachment.Name);

            // 노트 부위는 RegionAttachment(사각 스프라이트)와 MeshAttachment(변형 가능한 메시)가 섞여 있습니다.
            // 둘 다 RendererObject로 같은 AtlasRegion을 가리키므로, 틴트만 각자에서 읽고 이후는 똑같이 처리합니다.
            Il2CppSystem.Object rendererObject = null;
            string kind = null;
            string tint = null;

            var region = attachment.TryCast<Il2CppSpine.RegionAttachment>();
            if (region != null)
            {
                kind = "Region";
                tint = $"({region.r:F3},{region.g:F3},{region.b:F3},{region.a:F3})";
                rendererObject = Safe(() => region.RendererObject, null);
            }
            else
            {
                var mesh = attachment.TryCast<Il2CppSpine.MeshAttachment>();
                if (mesh != null)
                {
                    kind = "Mesh";
                    tint = $"({mesh.r:F3},{mesh.g:F3},{mesh.b:F3},{mesh.a:F3})";
                    rendererObject = Safe(() => mesh.RendererObject, null);
                }
            }

            if (kind == null)
            {
                MelonLogger.Msg($"{Tag} 슬롯 #{index} \"{slotName}\" — 어태치먼트 \"{attachmentName}\"는 Region도 Mesh도 아닙니다(그림이 없는 종류).");
                return;
            }

            MelonLogger.Msg($"{Tag} 슬롯 #{index} \"{slotName}\" ← {kind} 어태치먼트 \"{attachmentName}\" 틴트 rgba={tint}");

            var atlasRegion = rendererObject != null ? rendererObject.TryCast<Il2CppSpine.AtlasRegion>() : null;
            if (atlasRegion == null)
            {
                MelonLogger.Msg($"{Tag}    아틀라스 영역을 얻지 못했습니다.");
                return;
            }

            var page = atlasRegion.page;
            var texture = GetPageTexture(page);
            if (texture == null)
            {
                MelonLogger.Msg($"{Tag}    아틀라스 \"{Safe(() => atlasRegion.name)}\" — 페이지 텍스처를 얻지 못했습니다.");
                return;
            }

            // 아틀라스에 회전 배치된 영역은 가로·세로가 뒤바뀐 채 들어 있습니다.
            int packedWidth = atlasRegion.rotate ? atlasRegion.height : atlasRegion.width;
            int packedHeight = atlasRegion.rotate ? atlasRegion.width : atlasRegion.height;

            MelonLogger.Msg($"{Tag}    아틀라스 \"{Safe(() => atlasRegion.name)}\" @ 페이지 \"{Safe(() => page.name)}\" " +
                            $"({texture.width}×{texture.height}, {Safe(() => texture.name)}) " +
                            $"영역 x={atlasRegion.x} y={atlasRegion.y} {packedWidth}×{packedHeight} rotate={atlasRegion.rotate}");

            var readable = GetReadable(texture, readableByPage);
            if (readable == null)
            {
                MelonLogger.Msg($"{Tag}    텍스처를 읽을 수 있게 복사하지 못했습니다.");
                return;
            }

            SampleAndReport(readable, atlasRegion, packedWidth, packedHeight, slotName);
        }

        /// <summary>영역 픽셀을 세어 평균색·대표색을 찍고, 그 조각을 PNG로 남깁니다.</summary>
        private static void SampleAndReport(Texture2D readable, Il2CppSpine.AtlasRegion atlasRegion,
                                            int packedWidth, int packedHeight, string slotName)
        {
            // Spine의 y는 페이지 위쪽 기준, Unity 텍스처는 아래쪽 기준이라 뒤집어 줍니다.
            int x = atlasRegion.x;
            int y = readable.height - atlasRegion.y - packedHeight;

            if (x < 0 || y < 0 || packedWidth <= 0 || packedHeight <= 0 ||
                x + packedWidth > readable.width || y + packedHeight > readable.height)
            {
                MelonLogger.Msg($"{Tag}    영역이 텍스처 밖입니다 (x={x} y={y} {packedWidth}×{packedHeight} / 텍스처 {readable.width}×{readable.height})");
                return;
            }

            var pixels = readable.GetPixels(x, y, packedWidth, packedHeight);
            if (pixels == null || pixels.Length == 0)
            {
                MelonLogger.Msg($"{Tag}    영역 픽셀을 읽지 못했습니다.");
                return;
            }

            int stride = Math.Max(1, pixels.Length / SampleBudget);
            var buckets = new Dictionary<int, ColorBucket>();
            float sumR = 0f, sumG = 0f, sumB = 0f, sumWeight = 0f;
            int solid = 0;
            int sampled = 0;
            float maxAlpha = 0f;

            for (int i = 0; i < pixels.Length; i += stride)
            {
                var pixel = pixels[i];
                sampled++;
                if (pixel.a > maxAlpha) maxAlpha = pixel.a;
                if (pixel.a < SolidAlpha) continue;

                solid++;
                sumR += pixel.r * pixel.a;
                sumG += pixel.g * pixel.a;
                sumB += pixel.b * pixel.a;
                sumWeight += pixel.a;

                // 칸으로 뭉쳐서 세되, 보고는 그 칸에 들어온 실제 픽셀의 평균으로 합니다(칸 중앙값은 최대 4/255 어긋납니다).
                int key = Bucket(pixel.r) * ColorBuckets * ColorBuckets + Bucket(pixel.g) * ColorBuckets + Bucket(pixel.b);
                if (!buckets.TryGetValue(key, out var bucket))
                {
                    bucket = new ColorBucket();
                    buckets[key] = bucket;
                }
                bucket.Weight += pixel.a;
                bucket.SumR += pixel.r * pixel.a;
                bucket.SumG += pixel.g * pixel.a;
                bucket.SumB += pixel.b * pixel.a;
            }

            if (solid == 0 || sumWeight <= 0f)
            {
                MelonLogger.Msg($"{Tag}    불투명 픽셀이 없습니다 (샘플 {sampled}개, 최대 알파 {maxAlpha:F3})");
                return;
            }

            float avgR = sumR / sumWeight, avgG = sumG / sumWeight, avgB = sumB / sumWeight;
            MelonLogger.Msg($"{Tag}    평균색 {Hex(avgR, avgG, avgB)} rgb=({avgR:F3},{avgG:F3},{avgB:F3}) " +
                            $"— 불투명 픽셀 {solid}/{sampled}개({solid * 100f / sampled:F1}%)" +
                            (stride > 1 ? $", {stride}픽셀마다 표본" : string.Empty));
            MelonLogger.Msg($"{Tag}    대표색 {TopColorsText(buckets, sumWeight)}");

            WriteRegionPng(pixels, packedWidth, packedHeight, slotName);
        }

        private static string TopColorsText(Dictionary<int, ColorBucket> buckets, float total)
        {
            var top = new List<ColorBucket>(buckets.Values);
            top.Sort((a, b) => b.Weight.CompareTo(a.Weight));

            var parts = new List<string>();
            for (int i = 0; i < top.Count && i < TopColors; i++)
            {
                var bucket = top[i];
                float r = bucket.SumR / bucket.Weight;
                float g = bucket.SumG / bucket.Weight;
                float b = bucket.SumB / bucket.Weight;
                parts.Add($"{Hex(r, g, b)} {bucket.Weight * 100f / total:F1}%");
            }
            return string.Join(", ", parts);
        }

        /// <summary>비슷한 색끼리 뭉쳐 세는 칸 하나. 합을 들고 있다가 마지막에 평균을 냅니다.</summary>
        private sealed class ColorBucket
        {
            internal float Weight;
            internal float SumR;
            internal float SumG;
            internal float SumB;
        }

        private static void WriteRegionPng(Il2CppInterop.Runtime.InteropTypes.Arrays.Il2CppStructArray<Color> pixels,
                                           int width, int height, string slotName)
        {
            Texture2D crop = null;
            try
            {
                crop = new Texture2D(width, height, TextureFormat.RGBA32, false);
                crop.SetPixels(pixels);
                crop.Apply();

                var bytes = ImageConversion.EncodeToPNG(crop);
                if (bytes == null || bytes.Length == 0) return;

                string path = Path.Combine(SpineActionContract.ContractDirectory, $"ghost_tex_{SafeName(slotName)}.png");
                Directory.CreateDirectory(SpineActionContract.ContractDirectory);
                File.WriteAllBytes(path, bytes);
                MelonLogger.Msg($"{Tag}    조각 저장: {path}");
            }
            catch (Exception ex)
            {
                MelonLogger.Msg($"{Tag}    PNG 저장 실패: {ex.Message}");
            }
            finally
            {
                if (crop != null) { try { UnityEngine.Object.Destroy(crop); } catch (Exception) { } }
            }
        }

        // ── 텍스처 접근 ──────────────────────────────────────────────────────────

        private static Il2CppSpine.Attachment GetAttachment(Il2CppSpine.Slot slot)
        {
            return Safe(() =>
            {
                var attachment = slot.attachment;
                if (attachment != null) return attachment;

                // 셋업 포즈 어태치먼트가 아직 안 걸린 슬롯을 위한 보조 경로입니다.
                var skeleton = slot.Skeleton;
                var data = slot.data;
                if (skeleton == null || data == null || string.IsNullOrEmpty(data.attachmentName)) return null;
                return skeleton.GetAttachment(data.index, data.attachmentName);
            }, null);
        }

        private static Texture GetPageTexture(Il2CppSpine.AtlasPage page)
        {
            return Safe(() =>
            {
                if (page == null || page.rendererObject == null) return null;
                var material = page.rendererObject.TryCast<Material>();
                return material != null ? material.mainTexture : null;
            }, null);
        }

        /// <summary>페이지 텍스처당 한 번만 읽기 가능한 복사본을 만듭니다.</summary>
        private static Texture2D GetReadable(Texture source, Dictionary<IntPtr, Texture2D> cache)
        {
            IntPtr key = source.Pointer;
            if (cache.TryGetValue(key, out var cached)) return cached;

            Texture2D readable = null;
            RenderTexture temporary = null;
            var previous = RenderTexture.active;

            try
            {
                temporary = RenderTexture.GetTemporary(source.width, source.height, 0, RenderTextureFormat.ARGB32);
                Graphics.Blit(source, temporary);
                RenderTexture.active = temporary;

                readable = new Texture2D(source.width, source.height, TextureFormat.RGBA32, false);
                readable.ReadPixels(new Rect(0f, 0f, source.width, source.height), 0, 0);
                readable.Apply();

                WritePagePng(readable, source);
            }
            catch (Exception ex)
            {
                MelonLogger.Warning($"{Tag} 텍스처 복사 실패: {ex.Message}");
                if (readable != null) { try { UnityEngine.Object.Destroy(readable); } catch (Exception) { } }
                readable = null;
            }
            finally
            {
                RenderTexture.active = previous;
                if (temporary != null) RenderTexture.ReleaseTemporary(temporary);
            }

            cache[key] = readable;
            return readable;
        }

        /// <summary>아틀라스 페이지 전체도 한 장 남깁니다. 조각 크롭이 어긋났는지 눈으로 대조할 수 있게.</summary>
        private static void WritePagePng(Texture2D readable, Texture source)
        {
            try
            {
                if (readable.width * readable.height > MaxPagePixels) return;

                var bytes = ImageConversion.EncodeToPNG(readable);
                if (bytes == null || bytes.Length == 0) return;

                string name = SafeName(Safe(() => source.name));
                string path = Path.Combine(SpineActionContract.ContractDirectory, $"ghost_tex_page_{name}.png");
                Directory.CreateDirectory(SpineActionContract.ContractDirectory);
                File.WriteAllBytes(path, bytes);
                MelonLogger.Msg($"{Tag} 아틀라스 페이지 저장: {path} ({readable.width}×{readable.height})");
            }
            catch (Exception ex)
            {
                MelonLogger.Msg($"{Tag} 페이지 PNG 저장 실패: {ex.Message}");
            }
        }

        // ── 잡동사니 ─────────────────────────────────────────────────────────────

        private static int Bucket(float value)
        {
            int bucket = (int)(Math.Max(0f, Math.Min(0.999f, value)) * ColorBuckets);
            return bucket < 0 ? 0 : (bucket >= ColorBuckets ? ColorBuckets - 1 : bucket);
        }

        private static string Hex(float r, float g, float b) => $"#{Channel(r):X2}{Channel(g):X2}{Channel(b):X2}";

        private static int Channel(float value) => (int)Math.Round(Math.Max(0f, Math.Min(1f, value)) * 255f);

        private static string SafeName(string name)
        {
            if (string.IsNullOrEmpty(name)) return "unknown";

            var invalid = Path.GetInvalidFileNameChars();
            var sb = new System.Text.StringBuilder(name.Length);
            foreach (var c in name)
            {
                sb.Append(Array.IndexOf(invalid, c) >= 0 ? '_' : c);
            }
            return sb.ToString();
        }

        private static string Safe(Func<string> read)
        {
            try { return read() ?? "(null)"; }
            catch (Exception ex) { return $"(예외:{ex.GetType().Name})"; }
        }

        private static T Safe<T>(Func<T> read, T fallback) where T : class
        {
            try { return read(); }
            catch (Exception) { return fallback; }
        }
    }
}
