using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using MelonLoader;

namespace muse_dash_test
{
    // config.txt의 '표준 형식' 정의와 파일 쓰기 담당.
    // 이 파일 하나가 설정 파일의 레이아웃(구분선/설명/항목 순서)과 표준 여부 판정을 모두 쥡니다.
    // 값을 읽고 해석하는 쪽은 InputOverlay.Config.cs 입니다.
    public static partial class InputOverlay
    {
        /// <summary>고스트 노트 항목이 파일에 적히는 문구. 띄어쓰기는 사람이 읽기 좋으라고 넣은 것입니다.</summary>
        private const string GhostNotesKeyText = "공식곡에서도 고스트 노트 보이기";

        /// <summary>
        /// 위 문구에서 공백을 지운 비교용 키. 파일에는 띄어 적고 코드에서는 이 이름 하나로 봅니다.
        /// <b>이 둘이 어긋나면</b> 존재 검사가 영원히 실패해 파일을 매번 다시 쓰게 되므로,
        /// 문구를 바꿀 때는 반드시 두 상수를 같이 고치세요. (실제로 그 사고가 있었습니다)
        /// </summary>
        private const string GhostNotesKey = "공식곡에서도고스트노트보이기";

        /// <summary>굵은 구분선. 섹션의 시작과 끝을 나눕니다.</summary>
        private const string Rule = "# ============================================================================";

        /// <summary>얇은 구분선. 섹션 설명과 실제 설정값 사이를 나눕니다.</summary>
        private const string ThinRule = "# ----------------------------------------------------------------------------";

        /// <summary>
        /// config.txt 레이아웃 버전. 주석/구분선 배치를 바꿀 때마다 올립니다.
        /// 파일에 적힌 값이 이 값보다 낮으면 <see cref="EnsureCanonicalConfigFile"/>이
        /// 설정값은 그대로 둔 채 파일 전체를 새 레이아웃으로 다시 씁니다.
        /// (레이아웃만 바뀌면 키 목록은 그대로라 '누락/중복' 검사만으로는 갱신되지 않기 때문입니다.)
        /// </summary>
        private const int ConfigFormatVersion = 3;

        /// <summary>레이아웃 버전을 기록하는 주석 줄의 앞부분.</summary>
        private const string ConfigFormatMarkerPrefix = "config-format:";

        /// <summary>
        /// config.txt에 들어가는 설정 키 전체 목록(공백 제거·소문자 형태).
        /// <see cref="WriteConfigFile"/>이 실제로 쓰는 키와 반드시 일치해야 합니다.
        /// 새 설정을 추가할 때는 이 배열과 WriteConfigFile 두 곳만 고치면 됩니다.
        /// </summary>
        private static readonly string[] CanonicalKeys =
        {
            "키가로크기", "키세로크기", "키간격", "하단여백", "오버레이표시", "글자크기",
            "공중색상", "공중투명도", "지상색상", "지상투명도", "대기색상", "대기투명도",
            "판정바표시", "판정바가로크기", "판정바세로크기", "판정바하단여백",
            "판정바글자크기", "판정바틱유지시간", "판정바반응형",
            "오토플레이", "피버충전금지", "시네마", "강제퍼펙트",
            GhostNotesKey,
            "모바일터치조작", "모바일좌우모드", "모바일반전모드", "모바일오토피버",
        };

        /// <summary>
        /// 기존 config.txt가 표준 형식이 아니면(항목 누락 / 같은 키 중복 / 옛 키 이름) 전체를 다시 씁니다.
        ///
        /// <para>예전에는 부족한 항목만 파일 끝에 append 하는 방식이었는데, 존재 검사가 한 번 어긋나자
        /// 폴링마다 같은 줄이 덧붙어 파일이 무한히 커졌습니다. 그래서 부분 수정(append)을 아예 없애고,
        /// "파일을 만들 때 쓰는 그 함수"로 항상 전체를 다시 쓰는 방식으로 통일했습니다.
        /// 쓰는 경로가 <see cref="WriteConfigFile"/> 하나뿐이라 형식이 갈라질 여지가 없습니다.</para>
        ///
        /// <para>다시 쓰기 전에 <see cref="ParseConfigFile"/>로 현재 값을 필드에 먼저 실어 둡니다.
        /// WriteConfigFile은 필드 값을 그대로 출력하므로 사용자가 적어 둔 값이 보존되고,
        /// 옛 키 이름(<see cref="GhostNotesLegacyKey"/>)도 파싱 단계에서 흡수되어 새 이름으로 옮겨집니다.</para>
        /// </summary>
        private static void EnsureCanonicalConfigFile()
        {
            string text = ReadConfigTextRobust();
            if (string.IsNullOrEmpty(text))
            {
                WriteConfigFile("config.txt가 비어 있어 기본값으로 다시 생성했습니다");
                return;
            }

            ParseConfigFile();

            if (IsCanonicalConfigText(text, out string reason)) return;

            WriteConfigFile($"config.txt를 표준 형식으로 다시 썼습니다 (사유: {reason}, 설정값은 그대로 유지)");
        }

        /// <summary>
        /// 파일이 표준 형식인지 판정합니다. 표준이 아니면 <paramref name="reason"/>에 사유를 채웁니다.
        /// 모르는 키가 섞여 있는 것만으로는 다시 쓰지 않습니다(사용자가 직접 적어 둔 줄일 수 있으므로).
        /// </summary>
        private static bool IsCanonicalConfigText(string text, out string reason)
        {
            var seen = new HashSet<string>(StringComparer.Ordinal);
            int formatVersion = 0;

            foreach (string line in text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None))
            {
                string trimmed = line.Trim();
                if (trimmed.Length == 0) continue;

                if (trimmed.StartsWith("#", StringComparison.Ordinal))
                {
                    // 레이아웃 버전 표시는 주석 줄에 숨겨 둡니다(파싱에는 영향 없음).
                    int markerIdx = trimmed.IndexOf(ConfigFormatMarkerPrefix, StringComparison.Ordinal);
                    if (markerIdx >= 0)
                    {
                        string rest = trimmed.Substring(markerIdx + ConfigFormatMarkerPrefix.Length).Trim();
                        int digits = 0;
                        while (digits < rest.Length && char.IsDigit(rest[digits])) digits++;
                        if (digits > 0) int.TryParse(rest.Substring(0, digits), out formatVersion);
                    }
                    continue;
                }

                int idx = trimmed.IndexOf('=');
                if (idx <= 0) continue;

                // 파일에는 '공식곡에서도 고스트 노트 보이기'처럼 띄어 적으므로 공백을 지우고 비교합니다.
                string key = StripSpaces(trimmed.Substring(0, idx)).ToLowerInvariant();
                if (key.Length == 0) continue;

                if (!seen.Add(key))
                {
                    reason = $"'{key}' 항목이 중복됨";
                    return false;
                }
            }

            foreach (string key in CanonicalKeys)
            {
                if (!seen.Contains(key))
                {
                    reason = $"'{key}' 항목이 없음";
                    return false;
                }
            }

            // 버전이 더 높으면(= 옛 모드로 되돌린 경우) 건드리지 않습니다. 낮을 때만 새 레이아웃으로 올립니다.
            if (formatVersion < ConfigFormatVersion)
            {
                reason = $"설명/구분선 레이아웃이 옛 버전(v{formatVersion} → v{ConfigFormatVersion})";
                return false;
            }

            reason = null;
            return true;
        }

        /// <summary>
        /// 현재 설정값 전체를 config.txt에 씁니다. 생성·정규화 모두 이 함수 하나만 씁니다.
        /// (부분 추가/수정 경로를 두지 않는 것이 핵심입니다. 아래 <see cref="CanonicalKeys"/>와 짝을 맞추세요.)
        /// </summary>
        private static void WriteConfigFile(string reasonLog)
        {
            try
            {
                string onOff = "true / false  (on/off, 켜짐/끔, 1/0 도 됩니다)";

                StringBuilder sb = new StringBuilder();
                sb.AppendLine(Rule);
                sb.AppendLine("#  뮤즈 대시 커스텀 차트 모드 - 키 뷰어 / 판정바 설정");
                sb.AppendLine(Rule);
                sb.AppendLine("#  이 파일을 메모장 등으로 고쳐 저장하면 약 1초 안에 인게임에 반영됩니다.");
                sb.AppendLine("#  '#'으로 시작하는 줄은 설명이며 읽을 때 무시됩니다.");
                sb.AppendLine("#  값을 지우거나 항목을 통째로 없애도, 다음 실행에서 기본값으로 다시 채워집니다.");
                sb.AppendLine($"#  {ConfigFormatMarkerPrefix} {ConfigFormatVersion}   <- 이 줄은 지우지 마세요 (설정 파일 형식 버전)");
                sb.AppendLine(Rule);
                sb.AppendLine();
                sb.AppendLine();

                sb.AppendLine(Rule);
                sb.AppendLine("#  [1] 키 입력 오버레이 - 화면 아래쪽에 눌린 키를 보여줍니다");
                sb.AppendLine(Rule);
                sb.AppendLine("#  키가로크기 / 키세로크기 : 키 한 칸의 크기(픽셀)");
                sb.AppendLine("#  키간격                  : 키 칸 사이 여백(픽셀)");
                sb.AppendLine("#  하단여백                : 화면 아래에서 얼마나 띄울지(픽셀)");
                sb.AppendLine($"#  오버레이표시            : {onOff}");
                sb.AppendLine("#  글자크기                : 키 이름 글자 크기");
                sb.AppendLine(ThinRule);
                sb.AppendLine($"키가로크기={keyWidth}");
                sb.AppendLine($"키세로크기={keyHeight}");
                sb.AppendLine($"키간격={spacing}");
                sb.AppendLine($"하단여백={offsetFromBottom}");
                sb.AppendLine($"오버레이표시={showOverlay.ToString().ToLower()}");
                sb.AppendLine($"글자크기={fontSize}");
                sb.AppendLine();
                sb.AppendLine();

                sb.AppendLine(Rule);
                sb.AppendLine("#  [2] 오버레이 색상");
                sb.AppendLine(Rule);
                sb.AppendLine("#  쓸 수 있는 색상 : 파랑, 빨강, 노랑, 초록, 보라, 핑크, 하양, 검정, 회색, 어두운회색");
                sb.AppendLine("#  투명도 범위     : 0(완전 투명) ~ 100(불투명)");
                sb.AppendLine("#  공중 = 위쪽 줄, 지상 = 아래쪽 줄, 대기 = 안 눌린 상태");
                sb.AppendLine(ThinRule);
                sb.AppendLine($"공중색상={airColorName}");
                sb.AppendLine($"공중투명도={airAlpha}");
                sb.AppendLine($"지상색상={groundColorName}");
                sb.AppendLine($"지상투명도={groundAlpha}");
                sb.AppendLine($"대기색상={inactiveColorName}");
                sb.AppendLine($"대기투명도={inactiveAlpha}");
                sb.AppendLine();
                sb.AppendLine();

                sb.AppendLine(Rule);
                sb.AppendLine("#  [3] 판정바 (Hit Error Bar) - 판정이 얼마나 빠르고 느렸는지 보여줍니다");
                sb.AppendLine(Rule);
                sb.AppendLine($"#  판정바표시     : {onOff}");
                sb.AppendLine("#  판정바가로크기 : 바 전체 너비(픽셀). 좌우 끝이 각각 -150ms / +150ms 입니다");
                sb.AppendLine("#  판정바하단여백 : 화면 아래에서 띄울 거리. 키 오버레이가 켜져 있으면 그 위에 자동으로 붙습니다");
                sb.AppendLine("#  판정바틱유지시간 : 지나간 판정 눈금이 남아 있는 시간(초)");
                sb.AppendLine("#  판정바반응형   : true면 ms 숫자 대신 '퍼펙트!', '조금 빨라요!' 같은 말로 표시합니다");
                sb.AppendLine(ThinRule);
                sb.AppendLine($"판정바표시={showBar.ToString().ToLower()}");
                sb.AppendLine($"판정바가로크기={barWidth}");
                sb.AppendLine($"판정바세로크기={barHeight}");
                sb.AppendLine($"판정바하단여백={barOffsetFromBottom}");
                sb.AppendLine($"판정바글자크기={barFontSize}");
                sb.AppendLine($"판정바틱유지시간={tickDuration}");
                sb.AppendLine($"판정바반응형={barResponsive.ToString().ToLower()}");
                sb.AppendLine();
                sb.AppendLine();

                sb.AppendLine(Rule);
                sb.AppendLine("#  [4] 플레이 보조");
                sb.AppendLine(Rule);
                sb.AppendLine("#  오토플레이   : 차트를 자동으로 연주합니다.");
                sb.AppendLine("#                 (여기에 true를 적어 둬도, 모드를 새로 켠 첫 판은 항상 꺼진 채로 시작합니다)");
                sb.AppendLine("#  피버충전금지 : 피버 게이지가 차지 않게 막습니다.");
                sb.AppendLine("#  강제퍼펙트   : 오토플레이가 아닙니다. 입력은 직접 하되 '기록되는 판정'만 Perfect로 올립니다.");
                sb.AppendLine($"#  값 형식      : {onOff}");
                sb.AppendLine(ThinRule);
                sb.AppendLine($"오토플레이={forceAutoPlay.ToString().ToLower()}");
                sb.AppendLine($"피버충전금지={blockFever.ToString().ToLower()}");
                sb.AppendLine($"강제퍼펙트={forcePerfect.ToString().ToLower()}");
                sb.AppendLine();
                sb.AppendLine();

                sb.AppendLine(Rule);
                sb.AppendLine("#  [5] 연출");
                sb.AppendLine(Rule);
                sb.AppendLine("#  시네마 : 커스텀 곡의 BGA 동영상을 재생합니다.");
                sb.AppendLine("#");
                sb.AppendLine($"#  {GhostNotesKeyText}");
                sb.AppendLine("#         : 고스트 노트가 판정선 근처에서 사라지지 않도록 투명도를 유지합니다.");
                sb.AppendLine("#           생김새는 고스트 그대로이고, 보이기만 합니다.");
                sb.AppendLine("#           이 값은 '공식 곡'에만 적용됩니다. 커스텀 곡은 각 곡 폴더의 'hwa info.txt'에 적은");
                sb.AppendLine("#           '커스텀 곡 고스트 노트 보이기' 값을 따릅니다.");
                sb.AppendLine($"#  값 형식 : {onOff}");
                sb.AppendLine(ThinRule);
                sb.AppendLine($"시네마={enableCinema.ToString().ToLower()}");
                sb.AppendLine($"{GhostNotesKeyText}={showGhostNotes.ToString().ToLower()}");
                sb.AppendLine();
                sb.AppendLine();

                sb.AppendLine(Rule);
                sb.AppendLine("#  [6] 모바일 터치 조작 (Mobile Touch & Mouse Bridge)");
                sb.AppendLine(Rule);
                sb.AppendLine("#  모바일터치조작 : 마우스 클릭 및 터치스크린 입력을 모바일 터치(공중/지상)로 인식합니다.");
                sb.AppendLine("#  모바일좌우모드 : true면 화면 좌우 분할(왼쪽:공중, 오른쪽:지상), false면 상하 분할(위:공중, 아래:지상)");
                sb.AppendLine("#  모바일반전모드 : true면 터치/클릭 영역이 반대로(좌우/상하 반전) 바뀝니다.");
                sb.AppendLine("#  모바일오토피버 : 모바일 자동 피버 활성화 여부입니다.");
                sb.AppendLine($"#  값 형식        : {onOff}");
                sb.AppendLine(ThinRule);
                sb.AppendLine($"모바일터치조작={enableMobileTouch.ToString().ToLower()}");
                sb.AppendLine($"모바일좌우모드={mobileLeftRight.ToString().ToLower()}");
                sb.AppendLine($"모바일반전모드={mobileReverse.ToString().ToLower()}");
                sb.AppendLine($"모바일오토피버={mobileAutoFever.ToString().ToLower()}");

                File.WriteAllText(configPath, sb.ToString(), new UTF8Encoding(true));
                MelonLogger.Msg($"[InputOverlay] {reasonLog}: {configPath}");
            }
            catch (Exception ex)
            {
                hasFailedToWrite = true;
                MelonLogger.Error($"[InputOverlay] 설정 파일 저장 중 실패 (쓰기 시도가 중단됩니다): {ex.Message}");
            }
        }
    }
}
