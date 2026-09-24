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
        /// 곡의 고유 식별자 UID (예: "48-8", "1000-0")
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
        /// 곡의 음원/에셋 식별자 명칭 (예: "danxiang_ditie_music")
        /// </summary>
        public string music
        {
            get => Get<string>("music");
            set => Set("music", value);
        }

        /// <summary>
        /// UI 렌더링에 사용되는 곡 제목 프로퍼티 (언어팩 렌더링 키 참조)
        /// </summary>
        public string musicName
        {
            get => Get<string>("musicName");
            set => Set("musicName", value);
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
        /// 난이도 1단계(보통 Easy)의 레벨 표기.
        /// <para><b>게임 필드가 <c>string</c>이므로 래퍼도 문자열로 둡니다.</b> 공식 곡 중에는 숫자가 아닌
        /// 값을 가진 곡이 있어서(50-1 "ペロペロ in the Universe"), 예전처럼 <c>int</c>로 읽으면
        /// <c>Convert.ChangeType</c>이 던져 그 곡을 고를 때 [ERROR] 로그가 남았습니다(세션당 1회).
        /// 숫자가 필요하면 <c>int.TryParse</c>로 직접 변환하십시오(PnlReportCardPatch 참고).</para>
        /// </summary>
        public string difficulty1
        {
            get => Get<string>("difficulty1");
            set => Set("difficulty1", value);
        }

        /// <summary>
        /// 난이도 2단계(보통 Hard)의 레벨 표기(문자열, <see cref="difficulty1"/> 참고)
        /// </summary>
        public string difficulty2
        {
            get => Get<string>("difficulty2");
            set => Set("difficulty2", value);
        }

        /// <summary>
        /// 난이도 3단계(보통 Master)의 레벨 표기(문자열, <see cref="difficulty1"/> 참고)
        /// </summary>
        public string difficulty3
        {
            get => Get<string>("difficulty3");
            set => Set("difficulty3", value);
        }

        /// <summary>
        /// 난이도 4단계(보통 Another / Special)의 레벨 표기(문자열, <see cref="difficulty1"/> 참고)
        /// </summary>
        public string difficulty4
        {
            get => Get<string>("difficulty4");
            set => Set("difficulty4", value);
        }

        /// <summary>
        /// 난이도 5단계(히든 등의 특수 채보)의 레벨 표기(문자열, <see cref="difficulty1"/> 참고)
        /// </summary>
        public string difficulty5
        {
            get => Get<string>("difficulty5");
            set => Set("difficulty5", value);
        }
    }
}
