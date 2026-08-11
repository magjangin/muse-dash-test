namespace muse_dash_test
{
    public class HwaManifest
    {
        public string SourcePath;
        public string Uid;
        public string Title;
        public string CustomTitle;
        public string Artist;
        public string CustomArtist;
        public string LevelDesigner;
        public string Album;
        public int? Scene;
        public int? Difficulty1;
        public int? Difficulty2;
        public int? Difficulty3;
        public int? Difficulty4;
        public int? Difficulty5;
        public double? Delay;
        public double? Offset;

        /// <summary>
        /// 이 커스텀 곡에서 고스트 노트(UID `xx=17`)를 판정선까지 보이게 할지.
        /// `hwa info.txt`의 '커스텀 곡 고스트 노트 보이기'입니다. 적지 않으면 null이고,
        /// 그때는 공식곡용 전역 설정(`config.txt`)을 그대로 따릅니다.
        /// </summary>
        public bool? ShowGhostNotes;
    }
}
