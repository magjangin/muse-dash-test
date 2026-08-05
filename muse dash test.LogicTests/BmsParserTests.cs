using System.Text;

namespace muse_dash_test.LogicTests
{
    // BmsParser: BMS 텍스트 → BmsChart 변환(헤더/메타데이터, 채널→레인 매핑, 틱/시간 계산) 검증.
    public static class BmsParserTests
    {
        public static void ParseText_ReadsMetadataAndHeaderBpm()
        {
            var chart = BmsParser.ParseText("""
                #TITLE Test Song
                #ARTIST Test Artist
                #BPM 150
                """);

            Assert.Equal("Test Song", chart.Title);
            Assert.Equal("Test Artist", chart.Artist);
            Assert.Equal(150f, chart.DefaultBpm);
            Assert.Equal("Test Song", chart.Metadata["title"]); // 메타데이터 조회는 대소문자 무시
        }

        public static void ParseText_MapsKnownChannelsToLanesAndSkipsOthers()
        {
            var chart = BmsParser.ParseText("""
                #BPM 120
                #00113:01
                #00115:01
                #00118:01
                #00111:01
                """);

            // 0x11은 ChannelToLaneMap에 없으므로 노트로 잡히지 않습니다.
            Assert.Equal(3, chart.Notes.Count);
            Assert.Equal(BmsLane.Note, chart.Notes[0].Lane);
            Assert.Equal(BmsLane.BossInOut, chart.Notes[1].Lane);
            Assert.Equal(BmsLane.BossAction, chart.Notes[2].Lane);
        }

        public static void ParseText_ComputesTickAndTimeFromCellPosition()
        {
            var chart = BmsParser.ParseText("""
                #BPM 120
                #00113:01000101
                """);

            Assert.Equal(3, chart.Notes.Count);

            Assert.Equal(1.00f, chart.Notes[0].Tick);
            Assert.Equal(1.50f, chart.Notes[1].Tick);
            Assert.Equal(1.75f, chart.Notes[2].Tick);

            // 120 BPM에서 1마디 = 4박 = 2초
            Assert.Equal(2.0f, chart.Notes[0].Time);
            Assert.Equal(3.0f, chart.Notes[1].Time);
            Assert.Equal(3.5f, chart.Notes[2].Time);

            // CellIndex는 원본 셀 위치(0, 2, 3)를 그대로 보존합니다.
            Assert.Equal(0, chart.Notes[0].CellIndex);
            Assert.Equal(2, chart.Notes[1].CellIndex);
            Assert.Equal(3, chart.Notes[2].CellIndex);
        }

        public static void ParseText_UsesThreeCharacterCellsWhenWavKeysAreThreeCharacters()
        {
            var chart = BmsParser.ParseText("""
                #BPM 120
                #WAV001 011001_ground.wav
                #WAV00A 011001_air.wav
                #00113:001000
                """);

            // WAV 키가 3자리이므로 노트 데이터도 3자리 단위로 끊어 읽습니다.
            Assert.Equal(1, chart.Notes.Count);
            Assert.Equal("001", chart.Notes[0].RawValue);
            Assert.Equal(1.0f, chart.Notes[0].Tick);
        }

        public static void ParseText_AppliesBpmAliasChangesFromChannel08()
        {
            var chart = BmsParser.ParseText("""
                #BPM 120
                #BPM01:240
                #00108:0001
                #00113:0001
                """);

            Assert.Equal(240f, chart.BpmDefinitions["01"]);
            Assert.Equal(2, chart.BpmChanges.Count);
            Assert.Equal(1.5f, chart.BpmChanges[1].Tick);
            Assert.Equal(240f, chart.BpmChanges[1].Bpm);

            // 1.5틱까지는 120 BPM 구간이므로 1.5 * 4 * (60/120) = 3초
            Assert.Equal(3.0f, chart.Notes[0].Time);
        }

        public static void ParseText_ReadsHexBpmFromChannel03()
        {
            var chart = BmsParser.ParseText("""
                #BPM 120
                #00103:009C
                """);

            // "9C"는 10진 파싱에 실패하므로 16진수 0x9C = 156으로 읽힙니다.
            Assert.Equal(2, chart.BpmChanges.Count);
            Assert.Equal(1.5f, chart.BpmChanges[1].Tick);
            Assert.Equal(156f, chart.BpmChanges[1].Bpm);
        }

        public static void ParseText_Channel03PrefersDecimalOverHexForDigitOnlyCells()
        {
            var chart = BmsParser.ParseText("""
                #BPM 120
                #00103:0078
                """);

            // 현재 구현은 10진 파싱을 먼저 시도합니다.
            // BMS 표준(채널 03 = 16진수)대로면 0x78 = 120이어야 하므로 의도적 차이인지 확인이 필요합니다.
            Assert.Equal(78f, chart.BpmChanges[1].Bpm);
        }

        public static void ParseText_StripsSlashAndSemicolonComments()
        {
            var chart = BmsParser.ParseText("""
                #BPM 120 // 기본 BPM
                #00113:01 ; 첫 노트
                // 줄 전체가 주석
                """);

            Assert.Equal(120f, chart.DefaultBpm);
            Assert.Equal(1, chart.Notes.Count);
            Assert.Equal("01", chart.Notes[0].RawValue);
        }

        public static void ParseText_SortsNotesByTickThenChannel()
        {
            var chart = BmsParser.ParseText("""
                #BPM 120
                #00218:01
                #00113:01
                #00115:01
                """);

            Assert.Equal(3, chart.Notes.Count);
            Assert.Equal(0x13, chart.Notes[0].Channel);
            Assert.Equal(0x15, chart.Notes[1].Channel);
            Assert.Equal(0x18, chart.Notes[2].Channel);
            Assert.Equal(2.0f, chart.Notes[2].Tick);
        }

        public static void ParseText_ReturnsEmptyChartForBlankInput()
        {
            var chart = BmsParser.ParseText("   ");

            Assert.Equal(0, chart.Notes.Count);
            Assert.Equal(0, chart.BpmChanges.Count);
        }

        public static void ParseFile_ThrowsForMissingPath()
        {
            Assert.Throws<ArgumentException>(() => BmsParser.ParseFile(null));
            Assert.Throws<ArgumentException>(() => BmsParser.ParseFile("   "));
        }

        public static void ParseFile_ReadsChartFromDiskAndKeepsSourcePath()
        {
            string path = Path.Combine(Path.GetTempPath(), $"md-logic-tests-{Guid.NewGuid():N}.bms");
            File.WriteAllText(path, "#TITLE Disk Chart\n#BPM 120\n#00113:01\n", Encoding.UTF8);

            try
            {
                var chart = BmsParser.ParseFile(path);

                Assert.Equal("Disk Chart", chart.Title);
                Assert.Equal(1, chart.Notes.Count);
                Assert.Equal(path, chart.SourcePath);
            }
            finally
            {
                File.Delete(path);
            }
        }

        public static void CalculateTime_FallsBackTo120BpmWithoutChanges()
        {
            Assert.Equal(4f, BmsParser.CalculateTime(2f, null));
            Assert.Equal(4f, BmsParser.CalculateTime(2f, new List<BpmChange>()));
        }

        public static void CalculateTime_AccumulatesAcrossBpmChanges()
        {
            var changes = new List<BpmChange>
            {
                new BpmChange { Tick = 0f, Bpm = 120f, Source = "default" },
                new BpmChange { Tick = 1f, Bpm = 240f, Source = "03" },
            };

            // 0~1틱: 120 BPM → 2초, 1~2틱: 240 BPM → 1초
            Assert.Equal(3f, BmsParser.CalculateTime(2f, changes));
        }
    }
}
