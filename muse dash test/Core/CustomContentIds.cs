using System;

namespace muse_dash_test
{
    /// <summary>커스텀 태그, 앨범, 곡에 사용하는 식별자 규칙입니다.</summary>
    public static class CustomContentIds
    {
        public const int TagIndex = 1999;
        public const string TagUid = "tag-muse-dash-test";
        public const string AlbumUid = "1999-0";
        public const string VirtualAlbumPrefix = "1999-0";
        public const string VirtualSongPrefix = "1999-";
        public const string FallbackSourceMusicUid = "0-0";

        public static string CreateVirtualSongUid(int index)
        {
            if (index < 0) throw new ArgumentOutOfRangeException(nameof(index));
            // 앨범 UID(1999-0)와의 충돌 방지를 위해 곡 인덱스는 1부터 시작하게 합니다.
            return VirtualSongPrefix + (index + 1);
        }

        public static bool IsVirtualSong(string uid) =>
            !string.IsNullOrEmpty(uid) && uid.StartsWith(VirtualSongPrefix, StringComparison.Ordinal) && uid != AlbumUid;

        public static bool IsVirtualAlbum(string uid) =>
            !string.IsNullOrEmpty(uid) && uid == AlbumUid;

        public static bool IsVirtualContent(string uid) =>
            !string.IsNullOrEmpty(uid) && uid.StartsWith(VirtualSongPrefix, StringComparison.Ordinal);
    }
}
