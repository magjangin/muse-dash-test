using System;
using System.IO;
using System.Text;

namespace muse_dash_test.LogicTests
{
    // TextFileDecoder 검증. 메모장 'ANSI'(CP949)·일본 BMS(Shift-JIS)로 저장한 파일의 한글/일본어가 깨지지 않아야 하고,
    // 올바른 UTF-8 파일은 예전(UTF-8 고정)과 똑같이 읽혀야 합니다.
    public static class TextFileDecoderTests
    {
        private const string KoreanManifest = "제목 = 테스트 곡\n난이도3 bms = hard.bms\n크리스마스 씬 = 활성화\n";

        static TextFileDecoderTests()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        public static void Decode_Utf8WithoutBom_IsUnchanged()
        {
            string text = TextFileDecoder.Decode(new UTF8Encoding(false).GetBytes(KoreanManifest), out string enc);
            Assert.Equal(KoreanManifest, text);
            Assert.Equal("utf-8", enc);
        }

        public static void Decode_Utf8Bom_IsStripped()
        {
            byte[] bytes = new UTF8Encoding(true).GetPreamble();
            byte[] body = Encoding.UTF8.GetBytes(KoreanManifest);
            byte[] all = new byte[bytes.Length + body.Length];
            Buffer.BlockCopy(bytes, 0, all, 0, bytes.Length);
            Buffer.BlockCopy(body, 0, all, bytes.Length, body.Length);

            string text = TextFileDecoder.Decode(all, out string enc);
            Assert.Equal(KoreanManifest, text);
            Assert.Equal("utf-8-bom", enc);
        }

        public static void Decode_Cp949_ReadsKorean()
        {
            byte[] bytes = Encoding.GetEncoding(949).GetBytes(KoreanManifest);
            string text = TextFileDecoder.Decode(bytes, out string enc);
            Assert.Equal(KoreanManifest, text);
            Assert.Equal("cp949", enc);
            Assert.True(TextFileDecoder.IsLegacyEncoding(enc));
        }

        public static void Decode_Utf16LeBom_ReadsText()
        {
            byte[] bytes = Encoding.Unicode.GetPreamble();
            byte[] body = Encoding.Unicode.GetBytes(KoreanManifest);
            byte[] all = new byte[bytes.Length + body.Length];
            Buffer.BlockCopy(bytes, 0, all, 0, bytes.Length);
            Buffer.BlockCopy(body, 0, all, bytes.Length, body.Length);

            Assert.Equal(KoreanManifest, TextFileDecoder.Decode(all, out string enc));
            Assert.Equal("utf-16le", enc);
        }

        public static void Decode_AsciiAndEmpty()
        {
            Assert.Equal("#TITLE abc", TextFileDecoder.Decode(Encoding.ASCII.GetBytes("#TITLE abc"), out string enc));
            Assert.Equal("utf-8", enc);
            Assert.Equal(string.Empty, TextFileDecoder.Decode(Array.Empty<byte>(), out _));
            Assert.Equal(string.Empty, TextFileDecoder.Decode(null, out _));
        }

        public static void BmsParser_ParseFile_ReadsCp949Title()
        {
            string path = Path.Combine(Path.GetTempPath(), "textdecoder-" + Guid.NewGuid().ToString("N") + ".bms");
            try
            {
                string bms = "#TITLE 한글 제목\r\n#ARTIST 화영\r\n#BPM 120\r\n";
                File.WriteAllBytes(path, Encoding.GetEncoding(949).GetBytes(bms));

                var chart = BmsParser.ParseFile(path);
                Assert.Equal("한글 제목", chart.Title);
                Assert.Equal("화영", chart.Artist);
            }
            finally
            {
                if (File.Exists(path)) File.Delete(path);
            }
        }
    }
}
