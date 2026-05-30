using System;

namespace muse_dash_test
{
    /// <summary>
    /// 게임 내부의 앨범 데이터(DBConfigAlbums.AlbumsInfo) 객체를 안전하게 감싸 다루는 추상화 래퍼 모델입니다.
    /// </summary>
    public class AlbumsInfoWrapper : Il2CppWrapperBase
    {
        public AlbumsInfoWrapper(object rawAlbumsInfo) : base(rawAlbumsInfo)
        {
        }

        public string uid
        {
            get => Get<string>("uid");
            set => Set("uid", value);
        }

        public string title
        {
            get => Get<string>("title");
            set => Set("title", value);
        }

        public string tag
        {
            get => Get<string>("tag");
            set => Set("tag", value);
        }

        public string jsonName
        {
            get => Get<string>("jsonName");
            set => Set("jsonName", value);
        }

        public string prefabsName
        {
            get => Get<string>("prefabsName");
            set => Set("prefabsName", value);
        }

        public bool free
        {
            get => Get<bool>("free");
            set => Set("free", value);
        }

        public bool needPurchase
        {
            get => Get<bool>("needPurchase");
            set => Set("needPurchase", value);
        }

        public string price
        {
            get => Get<string>("price");
            set => Set("price", value);
        }
    }
}
