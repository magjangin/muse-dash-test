using System.IO;
using System.Text;

namespace muse_dash_test
{
    /// <summary>
    /// 사용자 데이터 파일(기록, 설정, 플래그)을 안전하게 덮어씁니다.
    ///
    /// <para><c>File.WriteAllText</c>는 파일을 여는 순간 내용을 비우므로, 쓰는 도중 게임이 죽으면
    /// 파일이 빈 상태나 잘린 상태로 남습니다. 그래서 같은 폴더의 임시 파일에 먼저 다 쓰고,
    /// 끝난 뒤에 이름을 바꿔 덮어씁니다. 이름 바꾸기는 같은 볼륨 안에서 한 번에 끝나므로,
    /// 읽는 쪽은 옛 내용이나 새 내용 중 하나만 보게 됩니다.</para>
    /// </summary>
    public static class AtomicFile
    {
        public static void WriteAllText(string path, string contents, Encoding encoding)
        {
            string tempPath = path + ".tmp";
            try
            {
                File.WriteAllText(tempPath, contents, encoding);
                File.Move(tempPath, path, overwrite: true);
            }
            finally
            {
                // 성공하면 임시 파일은 이미 이름이 바뀌어 없습니다. 실패했을 때만 남은 찌꺼기를 지웁니다.
                // 찌꺼기 삭제 실패(일시적 파일 잠금 등)가 원래의 예외를 덮어쓰지 않도록 격리합니다.
                try
                {
                    if (File.Exists(tempPath))
                    {
                        File.Delete(tempPath);
                    }
                }
                catch
                {
                }
            }
        }

        private const FileShare SharedReadMode = FileShare.ReadWrite | FileShare.Delete;

        /// <summary>
        /// 다른 프로세스(에디터, 클라우드 동기화, 백업, 원자적 이름 변경 저장기)가 파일을 열고 있어도 공유 위반 없이 안전하게 바이트를 읽습니다.
        /// </summary>
        public static byte[] ReadAllBytesShared(string path)
        {
            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, SharedReadMode))
            {
                long length = fs.Length;
                if (length > int.MaxValue) throw new IOException($"파일 크기가 너무 큽니다: {path} ({length} bytes)");
                byte[] bytes = new byte[(int)length];
                int totalRead = 0;
                while (totalRead < bytes.Length)
                {
                    int read = fs.Read(bytes, totalRead, bytes.Length - totalRead);
                    if (read == 0) break;
                    totalRead += read;
                }
                if (totalRead != bytes.Length)
                {
                    System.Array.Resize(ref bytes, totalRead);
                }
                return bytes;
            }
        }

        /// <summary>
        /// 다른 프로세스가 파일을 열고 있어도 공유 위반 없이 안전하게 텍스트를 읽습니다.
        /// </summary>
        public static string ReadAllTextShared(string path, Encoding encoding = null)
        {
            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, SharedReadMode))
            using (var reader = new StreamReader(fs, encoding ?? Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
