namespace muse_dash_test.LogicTests
{
    // BmsNoteMatcher: 채널별로 홀드(NoteType 3)/샌드백(NoteType 8) 시작-끝 노트를 짝짓는 로직 검증.
    public static class BmsNoteMatcherTests
    {
        public static void MatchSpecialNotes_PairsHoldStartAndEndOnSameChannel()
        {
            var chart = BmsParser.ParseText("""
                #BPM 120
                #WAV01 010201_홀드 지상.wav
                #00113:01000100
                """);

            var pairs = BmsNoteMatcher.MatchSpecialNotes(chart.Notes, chart);

            Assert.Equal(1, pairs.Count);
            Assert.Equal(BmsSpecialNoteType.Hold, pairs[0].Type);
            Assert.Equal(1.0f, pairs[0].StartNote.Tick);
            Assert.Equal(1.5f, pairs[0].EndNote.Tick);
            Assert.Equal(0.5f, pairs[0].LengthInTicks);
            Assert.Equal(1.0f, pairs[0].Duration); // 120 BPM에서 0.5틱 = 1초
            Assert.Equal(BmsLane.Note, pairs[0].Lane);
        }

        public static void MatchSpecialNotes_PairsSandbagStartAndEnd()
        {
            var chart = BmsParser.ParseText("""
                #BPM 120
                #WAV01 010401_샌드백.wav
                #00113:01000100
                """);

            var pairs = BmsNoteMatcher.MatchSpecialNotes(chart.Notes, chart);

            Assert.Equal(1, pairs.Count);
            Assert.Equal(BmsSpecialNoteType.Sandbag, pairs[0].Type);
        }

        public static void MatchSpecialNotes_DoesNotPairAcrossDifferentChannels()
        {
            var chart = BmsParser.ParseText("""
                #BPM 120
                #WAV01 010201_홀드.wav
                #00113:01
                #00114:01
                """);

            // 두 노트 모두 홀드지만 채널이 달라 각각 짝을 찾지 못합니다.
            Assert.Equal(0, BmsNoteMatcher.MatchSpecialNotes(chart.Notes, chart).Count);
        }

        public static void MatchSpecialNotes_LeavesTrailingUnpairedNoteUnmatched()
        {
            var chart = BmsParser.ParseText("""
                #BPM 120
                #WAV01 010201_홀드.wav
                #00113:010101
                """);

            var pairs = BmsNoteMatcher.MatchSpecialNotes(chart.Notes, chart);

            // 홀드 3개 → 앞의 두 개만 짝이 되고 마지막 하나는 남습니다.
            Assert.Equal(3, chart.Notes.Count);
            Assert.Equal(1, pairs.Count);
        }

        public static void MatchSpecialNotes_IgnoresNormalNotes()
        {
            var chart = BmsParser.ParseText("""
                #BPM 120
                #WAV01 011001_일반 노트.wav
                #00113:01000100
                """);

            Assert.Equal(0, BmsNoteMatcher.MatchSpecialNotes(chart.Notes, chart).Count);
        }

        public static void MatchSpecialNotes_ReturnsEmptyListForMissingInput()
        {
            var chart = BmsParser.ParseText("#BPM 120\n#00113:01");

            Assert.Equal(0, BmsNoteMatcher.MatchSpecialNotes(null, chart).Count);
            Assert.Equal(0, BmsNoteMatcher.MatchSpecialNotes(chart.Notes, null).Count);
            Assert.Equal(0, BmsNoteMatcher.MatchSpecialNotes(Array.Empty<BmsNote>(), chart).Count);
        }
    }
}
