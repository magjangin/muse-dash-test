using System;

namespace muse_dash_test
{
    /// <summary>커스텀 태그, 앨범, 곡에 사용하는 식별자 규칙입니다.</summary>
    public static class CustomContentIds
    {
        public const int TagIndex = 1998;
        public const string TagUid = "tag-muse-dash-test";
        public const string AlbumUid = "1998-0";
        public const string VirtualAlbumPrefix = "1998-";
        public const string VirtualSongPrefix = "1999-";
        public const string FallbackSourceMusicUid = "0-0";

        public static string CreateVirtualSongUid(int index)
        {
            if (index < 0) throw new ArgumentOutOfRangeException(nameof(index));
            return VirtualSongPrefix + index;
        }

        public static bool IsVirtualSong(string uid) =>
            !string.IsNullOrEmpty(uid) && uid.StartsWith(VirtualSongPrefix, StringComparison.Ordinal);

        public static bool IsVirtualAlbum(string uid) =>
            !string.IsNullOrEmpty(uid) && uid.StartsWith(VirtualAlbumPrefix, StringComparison.Ordinal);

        public static bool IsVirtualContent(string uid) => IsVirtualSong(uid) || IsVirtualAlbum(uid);
    }
}
