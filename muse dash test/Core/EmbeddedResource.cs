using System;
using System.IO;
using System.Reflection;

namespace muse_dash_test
{
    /// <summary>
    /// 모드 어셈블리에 박아 둔 리소스를 게임 폴더로 꺼내는 공용 경로입니다.
    ///
    /// <para>예전에는 같은 추출 코드가 <c>MainMod.EnsureTagIconExtracted</c>와
    /// <c>AlbumTagTogglePatch</c> 두 곳에 복사돼 있었고, <b>양쪽 다 같은 결함</b>을 갖고 있었습니다.
    /// <c>Stream.Read(buf, 0, buf.Length)</c>는 요청한 만큼 다 읽어준다는 보장이 없는데
    /// 반환값을 버렸기 때문에, 짧게 읽히면 뒷부분이 0으로 남은 <b>잘린 PNG</b>가 저장됩니다.
    /// 예외도 안 나고 로그도 정상으로 찍히므로 알아채기 어렵습니다.</para>
    ///
    /// <para>여기서는 <see cref="Stream.CopyTo(Stream)"/>가 끝까지 읽는 루프를 대신하게 합니다.
    /// (.NET 7의 <c>Stream.ReadExactly</c>는 이 프로젝트가 net6 대상이라 쓸 수 없습니다.)</para>
    /// </summary>
    internal static class EmbeddedResource
    {
        /// <summary>커스텀 태그 아이콘의 내장 리소스 이름입니다.</summary>
        internal const string TagIconResourceName = "muse_dash_test.Resources.tag_icon.png";

        /// <summary>커스텀 태그 아이콘 파일 이름입니다.</summary>
        internal const string TagIconFileName = "tag_icon.png";

        /// <summary>
        /// 대상 파일이 없으면 내장 리소스를 꺼내 씁니다. 이미 있으면 그대로 둡니다
        /// (사용자가 아이콘을 갈아 끼웠을 수 있으므로 덮어쓰지 않습니다).
        /// </summary>
        /// <returns>호출 뒤 대상 파일이 존재하면 true.</returns>
        internal static bool EnsureExtracted(string resourceName, string targetPath)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(targetPath)) return false;
                if (File.Exists(targetPath)) return true;

                // 두 호출부 중 한쪽(AlbumTagTogglePatch)은 폴더를 직접 만들지 않고 MainMod의
                // 초기화 블록에 기대고 있었습니다. 그 블록은 FeatureGuard로 감싸여 있어 조용히
                // 실패할 수 있으므로, 여기서 스스로 챙깁니다.
                string targetDir = Path.GetDirectoryName(targetPath);
                if (!string.IsNullOrEmpty(targetDir)) Directory.CreateDirectory(targetDir);

                var assembly = Assembly.GetExecutingAssembly();
                using (var stream = assembly.GetManifestResourceStream(resourceName))
                {
                    if (stream == null)
                    {
                        ModLogger.Error($"[EmbeddedResource] 추출할 내장 리소스를 찾을 수 없습니다: {resourceName}");
                        return false;
                    }

                    using (var file = File.Create(targetPath))
                    {
                        stream.CopyTo(file);
                    }
                }

                ModLogger.Msg($"[EmbeddedResource] 내장 리소스 '{resourceName}'를 '{targetPath}'에 추출했습니다.");
                return true;
            }
            catch (Exception ex)
            {
                ModLogger.Error($"[EmbeddedResource] 내장 리소스 추출 중 예외 발생: resource={resourceName}, target={targetPath}, {ex}");
                return false;
            }
        }

        /// <summary>태그 아이콘을 지정한 폴더에 추출합니다(이미 있으면 그대로 둡니다).</summary>
        internal static bool EnsureTagIcon(string targetFolder)
        {
            if (string.IsNullOrWhiteSpace(targetFolder)) return false;
            return EnsureExtracted(TagIconResourceName, Path.Combine(targetFolder, TagIconFileName));
        }
    }
}
