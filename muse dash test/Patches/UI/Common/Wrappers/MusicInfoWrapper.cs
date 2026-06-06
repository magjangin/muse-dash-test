using System;

namespace muse_dash_test
{
    /// <summary>
    /// 게임 내부의 원본 MusicInfo 객체와의 강결합을 방지하고, 리플렉션을 통해 필드 업데이트 취약성을 차단하는 추상화 어댑터 래퍼입니다.
    /// </summary>
    public class MusicInfoWrapper : Il2CppWrapperBase
    {
        /// <summary>
        /// 원본 MusicInfo 인스턴스를 받아 래퍼를 생성합니다.
        /// </summary>
        /// <param name="rawMusicInfo">Il2CppAssets.Scripts.Database.MusicInfo 인스턴스</param>
        public MusicInfoWrapper(object rawMusicInfo) : base(rawMusicInfo)
        {
        }

        /// <summary>
        /// 곡의 고유 식별자 UID (예: "48-8", "9999-0")
        /// </summary>
        public string uid
        {
            get => Get<string>("uid");
            set => Set("uid", value);
        }

        /// <summary>
        /// 곡의 공식 타이틀 제목 명칭
        /// </summary>
        public string name
        {
            get => Get<string>("name");
            set => Set("name", value);
        }

        /// <summary>
        /// 곡의 작곡가 / 아티스트 이름
        /// </summary>
        public string author
        {
            get => Get<string>("author");
            set => Set("author", value);
        }

        /// <summary>
        /// 채보 제작자 / 레벨 디자이너 명칭
        /// </summary>
        public string levelDesigner
        {
            get => Get<string>("levelDesigner");
            set => Set("levelDesigner", value);
        }

        /// <summary>
        /// 곡 선택 화면 및 배경 패널에 표시할 앨범 커버 이미지 자산 이름 (예: "memory_of_beach_cover")
        /// </summary>
        public string cover
        {
            get => Get<string>("cover");
            set => Set("cover", value);
        }

        /// <summary>
        /// 난이도 1단계(보통 Easy)의 레벨 수치 값
        /// </summary>
        public int difficulty1
        {
            get => Get<int>("difficulty1");
            set => Set("difficulty1", value);
        }

        /// <summary>
        /// 난이도 2단계(보통 Hard)의 레벨 수치 값
        /// </summary>
        public int difficulty2
        {
            get => Get<int>("difficulty2");
            set => Set("difficulty2", value);
        }

        /// <summary>
        /// 난이도 3단계(보통 Master)의 레벨 수치 값
        /// </summary>
        public int difficulty3
        {
            get => Get<int>("difficulty3");
            set => Set("difficulty3", value);
        }

        /// <summary>
        /// 난이도 4단계(보통 Another / Special)의 레벨 수치 값
        /// </summary>
        public int difficulty4
        {
            get => Get<int>("difficulty4");
            set => Set("difficulty4", value);
        }

        /// <summary>
        /// 난이도 5단계(히든 등의 특수 채보)의 레벨 수치 값
        /// </summary>
        public int difficulty5
        {
            get => Get<int>("difficulty5");
            set => Set("difficulty5", value);
        }
    }
}
