// 실험 차트가 다루는 게임 내부 데이터의 "암묵적 스키마"를 한곳에 모은 정의 파일입니다.
//  - NoteTypes: NoteConfigData.type(uint)에 쓰이는 매직 정수의 의미.
//  - UidCode  : 6자리 UID 문자열의 자릿수별 인코딩(씬/모션/레인) 해석.
// 두 헬퍼 모두 기존 호출부와 100% 동일한 값을 돌려주도록 설계되어 동작 변화가 없습니다.
public partial class DBStageInfo_SetRuntimeMusicData_Patch
{
    /// <summary>
    /// NoteConfigData.type 필드에 들어가는 노트 종류 코드입니다.
    /// IL2CPP의 type 필드가 uint라 캐스팅 마찰을 피하려고 enum 대신 const int로 둡니다.
    /// </summary>
    internal static class NoteTypes
    {
        /// <summary>보스 동작 지시용 placeholder. 키음/충돌 없음.</summary>
        public const int Boss = 0;
        /// <summary>일반 탭 노트. 지상+공중 더블 판정 대상.</summary>
        public const int Normal = 1;
        /// <summary>기어(운석) 노트. 정확도 집계상 Gears.</summary>
        public const int Gear = 2;
        /// <summary>롱노트(홀드). start/middle/end 체인으로 전개됨.</summary>
        public const int Long = 3;
        /// <summary>하트(HP) 노트. 정확도 집계상 Hearts.</summary>
        public const int Heart = 6;
        /// <summary>블루(음표) 노트. 정확도 집계상 BlueNotes.</summary>
        public const int Blue = 7;
        /// <summary>샌드백(mul) 노트.</summary>
        public const int Sandbag = 8;
        /// <summary>씬 전환 토글 노트.</summary>
        public const int SceneToggle = 9;
    }

    /// <summary>
    /// 6자리 UID 문자열의 자릿수별 인코딩을 해석합니다.
    /// <para>- [0..2) Scene: 씬 번호 prefix (예: "04" → scene_04, 0401_boss)</para>
    /// <para>- [2..4) Xx   : 모션/발사체 계열 코드 (15=down, 16=up, 06~08=발사체 계열)</para>
    /// <para>- [4..6) Yy   : 레인(pathway)/모션/dt 코드 (04·10·16=공중 등)</para>
    /// 각 헬퍼는 해당 구간을 안전하게 잘라낼 수 있을 때만 값을 반환하고, 길이가 부족하면 null을 돌려줍니다.
    /// (기존 호출부의 length 가드와 동일한 경계를 사용합니다.)
    /// </summary>
    internal static class UidCode
    {
        /// <summary>UID[0..2) 씬 번호 2자리. 길이 &lt; 2면 null.</summary>
        public static string Scene(string uid)
            => (uid != null && uid.Length >= 2) ? uid.Substring(0, 2) : null;

        /// <summary>UID[2..4) "xx" 코드. 길이 &lt; 4면 null.</summary>
        public static string Xx(string uid)
            => (uid != null && uid.Length >= 4) ? uid.Substring(2, 2) : null;

        /// <summary>UID[4..6) "yy" 코드. 길이 &lt; 6면 null.</summary>
        public static string Yy(string uid)
            => (uid != null && uid.Length >= 6) ? uid.Substring(4, 2) : null;
    }
}
