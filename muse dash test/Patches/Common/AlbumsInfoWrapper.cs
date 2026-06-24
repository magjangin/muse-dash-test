using System;

namespace muse_dash_test
{
    /// <summary>
    /// 게임 내부의 앨범 데이터(DBConfigAlbums.AlbumsInfo) 객체를 리플렉션 기반으로 안전하게 감싸 다루는 추상화 래퍼 모델입니다.
    /// </summary>
    public class AlbumsInfoWrapper : Il2CppWrapperBase
    {
        /// <summary>
        /// 원본 AlbumsInfo 인스턴스를 받아 래퍼를 생성합니다.
        /// </summary>
        /// <param name="rawAlbumsInfo">Il2CppAssets.Scripts.Database.DBConfigAlbums+AlbumsInfo 인스턴스</param>
        public AlbumsInfoWrapper(object rawAlbumsInfo) : base(rawAlbumsInfo)
        {
        }

        /// <summary>
        /// 앨범의 고유 식별자 UID (예: "music_package_0", "999-0")
        /// </summary>
        public string uid
        {
            get => Get<string>("uid");
            set => Set("uid", value);
        }

        /// <summary>
        /// 앨범 팩의 타이틀 명칭 (다국어 키 또는 기본 출력 이름)
        /// </summary>
        public string title
        {
            get => Get<string>("title");
            set => Set("title", value);
        }

        /// <summary>
        /// 앨범 팩의 태그 명칭 (태그별 그룹화 식별 값)
        /// </summary>
        public string tag
        {
            get => Get<string>("tag");
            set => Set("tag", value);
        }

        /// <summary>
        /// 앨범 곡 목록이 담겨 있는 Json 메타데이터 자산 명칭 (예: "ALBUM1")
        /// </summary>
        public string jsonName
        {
            get => Get<string>("jsonName");
            set => Set("jsonName", value);
        }

        /// <summary>
        /// 앨범 커버 아트를 표시할 때 사용할 프리팹 리소스의 에셋 명칭 (예: "AlbumDisco1")
        /// </summary>
        public string prefabsName
        {
            get => Get<string>("prefabsName");
            set => Set("prefabsName", value);
        }

        /// <summary>
        /// 무료 배포 앨범 팩인지 여부
        /// </summary>
        public bool free
        {
            get => Get<bool>("free");
            set => Set("free", value);
        }

        /// <summary>
        /// 앨범을 플레이하기 위해 구매(DLC 권한 소유)가 필요한 앨범 팩인지 여부
        /// </summary>
        public bool needPurchase
        {
            get => Get<bool>("needPurchase");
            set => Set("needPurchase", value);
        }

        /// <summary>
        /// 상점에서 표시할 앨범의 원본 가격 문자열 (예: "¥9.99")
        /// </summary>
        public string price
        {
            get => Get<string>("price");
            set => Set("price", value);
        }
    }
}
