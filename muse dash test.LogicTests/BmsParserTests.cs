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

        // 채널 03(직접 BPM 변경)은 지원하지 않습니다. BPM 변경은 #BPMxx + 채널 08만 씁니다.
        //
        // 예전에는 채널 03을 읽되 셀을 10진수로 먼저 파싱했는데, BMS 스펙상 이 채널은 항상
        // 2자리 16진수라 256개 셀 중 100개(00~99)가 틀린 값이 됐습니다. 예를 들어 "0078"은
        // 120 BPM이어야 하는데 78 BPM으로 읽혔습니다. 쓰는 채보가 없어 채널 자체를 걷어냈습니다.
        public static void ParseText_IgnoresDirectBpmChannel03()
        {
            var chart = BmsParser.ParseText("""
                #BPM 120
                #00103:0078
                """);

            // 기본 BPM 하나만 남고, 채널 03에서는 아무 변경도 만들지 않습니다.
            Assert.Equal(1, chart.BpmChanges.Count);
            Assert.Equal(120f, chart.BpmChanges[0].Bpm);
            Assert.Equal("default", chart.BpmChanges[0].Source);
        }

        // #BPM 0은 60 / 0이 되어 모든 노트 시간이 무한대로 나왔고, 모드가 그 값을 게임 노트 값(Decimal)으로
        // 바꾸는 순간 예외가 나 차트 주입이 통째로 실패했습니다. 0 이하의 헤더 BPM은 무시하고 기본값을 씁니다.
        public static void ParseText_IgnoresNonPositiveHeaderBpm()
        {
            foreach (string header in new[] { "#BPM 0", "#BPM 0.0" })
            {
                var chart = BmsParser.ParseText(header + Environment.NewLine + "#00113:01");

                Assert.Equal(120f, chart.DefaultBpm, header);
                Assert.Equal(1, chart.Notes.Count, header);
                Assert.False(float.IsInfinity(chart.Notes[0].Time) || float.IsNaN(chart.Notes[0].Time), header + ": 노트 시간은 유한해야 합니다.");
                Assert.Equal(2.0f, chart.Notes[0].Time, header); // 120 BPM에서 1마디 = 4박 = 2초
            }
        }

        // 표준 BMS는 "#BPM01 240"처럼 공백으로 값을 적습니다. 모드의 경고문과 BMS_PARSING.md도 이 표기를
        // 안내하는데, 예전 정규식은 ':'나 '='를 반드시 요구해서 공백 표기를 BPM 선언이 아닌 일반 메타데이터로
        // 흘려보냈습니다. 채널 08이 참조할 선언이 없으니 BPM 변경이 경고 한 줄 없이 사라졌습니다.
        public static void ParseText_AcceptsSpaceSeparatedBpmAlias()
        {
            var chart = BmsParser.ParseText("""
                #BPM 120
                #BPM01 240
                #00108:01
                #00113:0001
                """);

            Assert.Equal(240f, chart.BpmDefinitions["01"]);
            Assert.Equal(2, chart.BpmChanges.Count);
            Assert.Equal(1.0f, chart.BpmChanges[1].Tick);
            Assert.Equal(240f, chart.BpmChanges[1].Bpm);

            // 0~1틱은 120 BPM(2초), 1~1.5틱은 240 BPM(0.5초).
            Assert.Equal(2.5f, chart.Notes[0].Time);
        }

        // 0틱(첫 마디 시작)의 BPM 변경은 헤더 기본 BPM을 덮어야 합니다. 예전에는 같은 틱을 Source 문자열로
        // 정렬해서 "BPM01"이 "default"보다 앞에 오고, 기본 BPM이 나중에 적용돼 변경이 통째로 무시됐습니다.
        public static void ParseText_LetsZeroTickBpmChangeOverrideHeaderBpm()
        {
            var chart = BmsParser.ParseText("""
                #BPM 120
                #BPM01:240
                #00008:01
                #00113:01
                """);

            Assert.Equal(2, chart.BpmChanges.Count);
            Assert.Equal("default", chart.BpmChanges[0].Source);
            Assert.Equal("BPM01", chart.BpmChanges[1].Source);

            // 0틱부터 240 BPM이므로 1마디는 4박 = 1초입니다.
            Assert.Equal(1.0f, chart.Notes[0].Time);
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
