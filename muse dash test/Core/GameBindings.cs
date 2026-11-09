namespace muse_dash_test
{
    /// <summary>
    /// 컴파일러가 검증하지 못하는 게임 쪽 "raw 문자열 식별자"를 모아 두는 곳입니다.
    ///
    /// <para><b>패치 대상은 여기에 없습니다.</b> Harmony 패치 대상은 전부 <c>typeof(...)</c> + <c>nameof(...)</c>로
    /// 옮겨서, 게임 업데이트로 이름이 바뀌면 빌드가 먼저 깨집니다. 예전에 이 파일에 있던 패치용 상수 20개
    /// (<c>TaskStageTarget.AddScore</c> 등)는 그 뒤로 쓰는 곳이 없었고, <see cref="PatchHealthCheck"/>도 이 파일이 아니라
    /// 패치 클래스의 어트리뷰트를 직접 읽습니다. 쓰이지 않는 상수는 "업데이트 때 여기부터 고치라"는 잘못된 길잡이가
    /// 되므로 2026-11-10에 지웠습니다.</para>
    ///
    /// <para>남은 것은 <c>nameof</c>를 쓸 수 없는 리플렉션 호출용 이름뿐입니다. 게임이 업데이트되어 패치가 깨졌을 때는
    /// 시작 로그의 <c>[PatchHealthCheck]</c>·<c>[PatchInstaller]</c> 줄을 먼저 보십시오. 런타임 실패는
    /// <see cref="FeatureGuard"/>가 기능 단위로 격리합니다.</para>
    /// </summary>
    public static class GameBindings
    {
        /// <summary>Il2Cpp 컬렉션 마스크 래퍼(<see cref="Il2CppWrapperBase"/>에서 리플렉션 호출).</summary>
        public static class Il2CppCollection
        {
            public const string AddMaskValue = "AddMaskValue";
        }
    }
}
