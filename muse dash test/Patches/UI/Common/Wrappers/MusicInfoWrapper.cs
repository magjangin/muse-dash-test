using System;

namespace muse_dash_test
{
    /// <summary>
    /// 게임 내부의 원본 MusicInfo 객체와의 강결합을 방지하고 필드 업데이트 취약성을 격리하는 추상화 어댑터 래퍼입니다.
    /// </summary>
    public class MusicInfoWrapper : Il2CppWrapperBase
    {
        public MusicInfoWrapper(object rawMusicInfo) : base(rawMusicInfo)
        {
        }

        public string uid
        {
            get => Get<string>("uid");
            set => Set("uid", value);
        }

        public string name
        {
            get => Get<string>("name");
            set => Set("name", value);
        }

        public string author
        {
            get => Get<string>("author");
            set => Set("author", value);
        }

        public string levelDesigner
        {
            get => Get<string>("levelDesigner");
            set => Set("levelDesigner", value);
        }

        public string cover
        {
            get => Get<string>("cover");
            set => Set("cover", value);
        }

        public int difficulty1
        {
            get => Get<int>("difficulty1");
            set => Set("difficulty1", value);
        }

        public int difficulty2
        {
            get => Get<int>("difficulty2");
            set => Set("difficulty2", value);
        }

        public int difficulty3
        {
            get => Get<int>("difficulty3");
            set => Set("difficulty3", value);
        }

        public int difficulty4
        {
            get => Get<int>("difficulty4");
            set => Set("difficulty4", value);
        }

        public int difficulty5
        {
            get => Get<int>("difficulty5");
            set => Set("difficulty5", value);
        }
    }
}
