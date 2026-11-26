using System;
using System.IO;
using System.Text;

namespace muse_dash_test
{
    /// <summary>
    /// 사용자가 만든 텍스트 파일(BMS 채보, 곡 폴더의 <c>info.txt</c>)을 인코딩을 가려 읽습니다.
    ///
    /// <para><b>왜 필요한가</b>: 두 파일은 예전에 UTF-8로만 읽었습니다. 윈도우 메모장에서 'ANSI'로 저장하거나
    /// 한국어 BMS 에디터가 CP949로 저장하면, 한글이 전부 대체 문자(U+FFFD)로 바뀌어
    /// <c>info.txt</c>의 한글 키(<c>난이도N bms</c>, <c>크리스마스 …</c>, <c>dt를 권장 표로 고치기</c>)가 조용히 무시되고
    /// BMS의 <c>#TITLE</c>·<c>#WAV</c> 파일 이름이 깨졌습니다. <c>config.txt</c>만 따로 CP949 폴백을 갖고 있었습니다
    /// (<c>InputOverlay.ReadConfigTextRobust</c>).</para>
    ///
    /// <para>판정 순서: BOM(UTF-8/UTF-16) → 엄격한 UTF-8(잘못된 바이트가 하나라도 있으면 실패) → CP949 → Shift-JIS(일본 BMS)
    /// → 마지막으로 관대한 UTF-8. 올바른 UTF-8 파일(ASCII 포함)은 예전과 똑같이 읽힙니다.</para>
    ///
    /// <para>게임에 의존하지 않으며 로직 테스트(<c>TextFileDecoderTests</c>)로 검증됩니다.</para>
    /// </summary>
    public static class TextFileDecoder
    {
        private static readonly UTF8Encoding StrictUtf8 = new UTF8Encoding(false, throwOnInvalidBytes: true);

        static TextFileDecoder()
        {
            try
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            }
            catch (Exception)
            {
                // 등록 실패 시 아래 GetStrictCodePage가 null을 돌려주고 UTF-8로 읽습니다.
            }
        }

        /// <summary>파일을 읽어 문자열로 돌려줍니다. 다른 프로그램이 쓰는 중이어도 읽을 수 있게 공유 모드로 엽니다.</summary>
        public static string ReadAllText(string path, out string encodingName)
        {
            byte[] bytes;
            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                long length = fs.Length;
                if (length > int.MaxValue) throw new IOException($"파일 크기가 너무 큽니다: {path} ({length} bytes)");
                bytes = new byte[(int)length];
                int totalRead = 0;
                while (totalRead < bytes.Length)
                {
                    int read = fs.Read(bytes, totalRead, bytes.Length - totalRead);
                    if (read == 0) break;
                    totalRead += read;
                }
                if (totalRead != bytes.Length)
                {
                    Array.Resize(ref bytes, totalRead);
                }
            }
            return Decode(bytes, out encodingName);
        }

        /// <summary>바이트를 위 순서대로 해석합니다. <paramref name="encodingName"/>은 로그용 이름입니다.</summary>
        public static string Decode(byte[] bytes, out string encodingName)
        {
            if (bytes == null || bytes.Length == 0)
            {
                encodingName = "empty";
                return string.Empty;
            }

            if (bytes.Length >= 3 && bytes[0] == 0xEF && bytes[1] == 0xBB && bytes[2] == 0xBF)
            {
                encodingName = "utf-8-bom";
                return Encoding.UTF8.GetString(bytes, 3, bytes.Length - 3);
            }
            if (bytes.Length >= 2 && bytes[0] == 0xFF && bytes[1] == 0xFE)
            {
                encodingName = "utf-16le";
                return Encoding.Unicode.GetString(bytes, 2, bytes.Length - 2);
            }
            if (bytes.Length >= 2 && bytes[0] == 0xFE && bytes[1] == 0xFF)
            {
                encodingName = "utf-16be";
                return Encoding.BigEndianUnicode.GetString(bytes, 2, bytes.Length - 2);
            }

            if (TryDecode(StrictUtf8, bytes, out string text))
            {
                encodingName = "utf-8";
                return text;
            }

            foreach (int codePage in new[] { 949, 932 })
            {
                Encoding strict = GetStrictCodePage(codePage);
                if (strict != null && TryDecode(strict, bytes, out text))
                {
                    encodingName = codePage == 949 ? "cp949" : "shift_jis";
                    return text;
                }
            }

            encodingName = "utf-8-lossy";
            return Encoding.UTF8.GetString(bytes);
        }

        /// <summary>UTF-8 말고 다른 인코딩으로 읽었는지(로그에 한 줄 남길 만한지) 판정합니다.</summary>
        public static bool IsLegacyEncoding(string encodingName)
        {
            return encodingName == "cp949" || encodingName == "shift_jis" || encodingName == "utf-8-lossy";
        }

        private static Encoding GetStrictCodePage(int codePage)
        {
            try
            {
                return Encoding.GetEncoding(codePage, EncoderFallback.ExceptionFallback, DecoderFallback.ExceptionFallback);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static bool TryDecode(Encoding encoding, byte[] bytes, out string text)
        {
            try
            {
                text = encoding.GetString(bytes);
                return true;
            }
            catch (DecoderFallbackException)
            {
                text = null;
                return false;
            }
        }
    }
}
