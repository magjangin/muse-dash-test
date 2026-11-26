using System;
using System.IO;

namespace muse_dash_test
{
    /// <summary>
    /// 채보가 바뀐 슬롯의 옛 기록을 채보 지문별로 보관하는 규칙입니다(<c>record/archive/</c>).
    ///
    /// <para><b>왜 필요한가</b>: 기록 파일(<c>record/{곡 폴더}_{난이도}.json</c>)은 슬롯마다 하나라서, 곡 폴더의 BMS를
    /// 갈아끼우고 한 판 하면 새 채보의 기록이 옛 기록을 덮습니다. 예전에는 처음 덮을 때의 원본만 <c>.bak</c>으로 남겨,
    /// A → B → C로 바꾸면 B의 최고 기록이 사라졌고, A로 되돌려도 A의 기록이 화면에 돌아오지 않았습니다
    /// (지문이 다르므로 새 채보 취급).</para>
    ///
    /// <para>그래서 덮기 직전 파일의 지문이 새 기록의 지문과 다르면 그 파일을 <c>archive/{파일 이름}.{지문}.json</c>으로
    /// 옮겨 두고, 읽을 때 본 파일이 지금 채보의 것이 아니면 지금 지문의 보관본을 찾습니다. 같은 지문은 한 파일만
    /// 두며 나중 것이 덮습니다(그 지문의 최신 기록이 본 파일에서 내려온 것이므로).</para>
    ///
    /// <para>게임에 의존하지 않으며 로직 테스트(<c>RecordArchiveTests</c>)로 검증됩니다.</para>
    /// </summary>
    public static class RecordArchive
    {
        public const string FolderName = "archive";

        /// <summary>
        /// 본 기록 파일 경로와 지문으로 보관본 경로를 만듭니다. 지문이 비었으면(지문 없는 옛 기록) null입니다.
        /// 예: <c>record/neon_2.json</c> + <c>ab12…</c> → <c>record/archive/neon_2.ab12….json</c>
        /// </summary>
        public static string PathFor(string recordFilePath, string fingerprint)
        {
            if (string.IsNullOrEmpty(recordFilePath) || string.IsNullOrWhiteSpace(fingerprint)) return null;

            string folder = Path.GetDirectoryName(recordFilePath) ?? string.Empty;
            string baseName = Path.GetFileNameWithoutExtension(recordFilePath);
            return Path.Combine(folder, FolderName, baseName + "." + SanitizeFingerprint(fingerprint) + ".json");
        }

        /// <summary>
        /// 덮을 파일을 보관해야 하는지 판정합니다. 두 지문이 모두 있고 서로 다를 때만 참입니다.
        /// 지문 없는 옛 기록(빈 값)은 기존 <c>.bak</c> 백업이 맡습니다.
        /// </summary>
        public static bool ShouldArchive(string existingFingerprint, string newFingerprint)
        {
            if (string.IsNullOrWhiteSpace(existingFingerprint) || string.IsNullOrWhiteSpace(newFingerprint)) return false;
            return !string.Equals(existingFingerprint.Trim(), newFingerprint.Trim(), StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// <paramref name="recordFilePath"/>를 지문 보관본으로 복사합니다. 같은 지문의 보관본이 있으면 덮습니다.
        /// 복사한 경로를 돌려주고, 보관할 필요가 없거나 원본이 없으면 null입니다. 실패는 예외로 올립니다.
        /// </summary>
        public static string ArchiveCopy(string recordFilePath, string existingFingerprint, string newFingerprint)
        {
            if (!ShouldArchive(existingFingerprint, newFingerprint) || !File.Exists(recordFilePath)) return null;

            string target = PathFor(recordFilePath, existingFingerprint);
            if (target == null) return null;

            Directory.CreateDirectory(Path.GetDirectoryName(target));
            File.Copy(recordFilePath, target, overwrite: true);
            return target;
        }

        private static string SanitizeFingerprint(string fingerprint)
        {
            string value = fingerprint.Trim();
            foreach (char c in Path.GetInvalidFileNameChars())
            {
                value = value.Replace(c, '_');
            }
            return value;
        }
    }
}
