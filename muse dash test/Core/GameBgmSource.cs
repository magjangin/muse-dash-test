using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace muse_dash_test
{
    /// <summary>
    /// 게임이 원곡을 트는 BGM AudioSource(<c>Singleton&lt;AudioManager&gt;.instance.bgm</c>)를 돌려줍니다.
    ///
    /// <para><b>BGM 소스를 이름으로 찾지 말고 여기서 얻으십시오.</b> 게임 6.7.0이 "BGM" 옆에 "BGM_OneShot"
    /// (<c>AudioManager.m_BgmOneShotSource</c>)을 추가하자, 이름에 "bgm"이 든 첫 소스를 고르던 배틀 BGM 주입이
    /// 그쪽을 집어 원곡과 커스텀 곡이 겹쳐 들렸습니다(CHECKLIST 참고). 게임이 들고 있는 참조는 오브젝트 이름이
    /// 바뀌어도 따라갑니다. 6.7.0 로그에서 이 참조는 GameObject "BGM"을 가리켰습니다(26-9-26_21-30-2).</para>
    /// </summary>
    internal static class GameBgmSource
    {
        /// <summary>
        /// 게임의 BGM 소스를 얻습니다. 못 얻으면 null을 돌려주고 이유를 <paramref name="failure"/>에 담습니다.
        /// 폴백 경로와 로그는 호출부가 정합니다.
        /// </summary>
        public static AudioSource TryGet(out string failure)
        {
            try
            {
                AudioSource bgm = ReadAudioManagerBgm();
                if (bgm == null || bgm.gameObject == null)
                {
                    failure = "AudioManager.bgm이 비어 있습니다";
                    return null;
                }

                failure = null;
                return bgm;
            }
            catch (Exception ex)
            {
                failure = $"{ex.GetType().Name}: {ex.Message}";
                return null;
            }
        }

        /// <summary>
        /// 이름 스캔 폴백에서 건너뛸 보조 소스인지 확인합니다. 6.7.0의 "BGM_OneShot"은 승리 효과음
        /// (<c>sfx_victory_bgm</c>) 같은 단발음을 트는 소스라 곡을 주입할 대상이 아닙니다.
        /// </summary>
        public static bool IsOneShotSource(AudioSource source)
        {
            string name = source != null && source.gameObject != null ? source.gameObject.name : null;
            return name != null && name.IndexOf("OneShot", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        /// <summary>
        /// 게임 멤버는 이 메서드에서만 만집니다.
        /// 게임 업데이트로 <c>AudioManager.bgm</c>이 사라지면 JIT가 이 메서드를 컴파일하지 못해
        /// <c>MissingMethodException</c>이 <b>호출 지점</b>에서 납니다. 같은 메서드 안의 try/catch로는
        /// 이걸 잡지 못하므로(2026-09-27 실측, .NET 6/10 동일) 호출부 <see cref="TryGet"/>의 try가 잡도록
        /// 분리해 둡니다. 인라인되면 분리한 의미가 없어지므로 막습니다.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static AudioSource ReadAudioManagerBgm()
        {
            var audioManager = Il2CppAssets.Scripts.PeroTools.Commons.Singleton<Il2CppAssets.Scripts.PeroTools.Managers.AudioManager>.instance;
            return audioManager != null ? audioManager.bgm : null;
        }
    }
}
