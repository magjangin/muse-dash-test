using MelonLoader;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace muse_dash_test
{
    /// <summary>대사 한 줄이 말풍선에 대해 수행하는 동작입니다.</summary>
    public enum HwaDialogAction
    {
        /// <summary>말풍선을 열고 텍스트를 표시합니다.</summary>
        Show,
        /// <summary>이미 열린 말풍선의 텍스트만 교체합니다.</summary>
        Update,
        /// <summary>말풍선을 닫습니다.</summary>
        Hide
    }

    /// <summary>dialog.txt에서 읽어낸 대사 한 줄입니다.</summary>
    public sealed class HwaDialogLine
    {
        /// <summary>곡 시작 기준 초. BMS 노트 시간과 같은 기준이며 소수 2자리로 반올림됩니다.</summary>
        public double Time;
        /// <summary>0=ROLE, 1=BOSS, 2=UI. 말풍선 스프라이트를 고릅니다.</summary>
        public int DialogType;
        /// <summary>0=UP, 1=DOWN. 말풍선 위치입니다.</summary>
        public int DialogIndex;
        public HwaDialogAction Action;
        public string Text;
        public int FontSize;
        /// <summary>상자 가로 크기. -1이면 타입별 기본값을 씁니다.</summary>
        public int BoxWidth = -1;
        /// <summary>상자 세로 크기. -1이면 타입별 기본값을 씁니다.</summary>
        public int BoxHeight = -1;
        /// <summary>경고 메시지에 쓸 원본 파일의 줄 번호입니다.</summary>
        public int SourceLineNumber;
    }

    /// <summary>
    /// 커스텀 곡 폴더의 <c>dialog.txt</c>를 읽어 대사 줄 목록으로 변환합니다.
    /// <para>형식(슬래시 구분, 뒤쪽 칸은 생략 가능):</para>
    /// <code>
    /// // 시간 / 타입 / 방향 / 상태 / 폰트 / 상자크기 / 텍스트
    /// 5초    / ui   / 아래 / 활성화   / 30 / 400x150 / 첫 줄입니다
    /// 7.25초 /      /      /          /    /         / 두 번째 줄
    /// 9초    /      /      / 비활성화
    /// </code>
    /// <list type="bullet">
    /// <item>타입/방향/폰트/상자크기를 비우면 <b>직전 줄의 값을 물려받습니다</b>. 그래서 <c>5초 / 안녕하세요</c>처럼 두 칸만 적어도 됩니다.</item>
    /// <item>상태를 비우면, 해당 말풍선이 닫혀 있으면 여는 것으로, 열려 있으면 텍스트 교체로 처리합니다.</item>
    /// <item><b>텍스트에 슬래시를 쓰려면 앞의 6칸을 모두 채워야 합니다</b>(비워도 되니 슬래시만 찍으면 됩니다).
    ///       7번째 칸부터는 슬래시를 포함해 통째로 텍스트로 봅니다.</item>
    /// <item>텍스트 안의 <c>\n</c>은 줄바꿈이 됩니다.</item>
    /// <item><c>//</c> 또는 <c>#</c>로 시작하는 줄과 빈 줄은 무시합니다.</item>
    /// </list>
    /// <para>쓸 수 있는 낱말 (한글/영어 아무거나):</para>
    /// <list type="bullet">
    /// <item>타입: <c>ui</c>·<c>방백</c> / <c>boss</c>·<c>보스</c> / <c>role</c>·<c>캐릭터</c></item>
    /// <item>방향: <c>위</c>·<c>up</c> / <c>아래</c>·<c>down</c></item>
    /// <item>상태: <c>활성화</c>·<c>show</c> / <c>비활성화</c>·<c>hide</c> / <c>유지</c>·<c>update</c>(또는 빈칸)</item>
    /// <item>상자크기: <c>400x150</c> 또는 가로만 <c>400</c></item>
    /// </list>
    /// </summary>
    public static class HwaDialogFile
    {
        private const string Tag = "[HwaDialog]";

        /// <summary>커스텀 곡 폴더에서 찾는 파일 이름입니다.</summary>
        public const string FileName = "dialog.txt";

        /// <summary>대사 시간 키는 소수 2자리입니다. (게임의 시간별 딕셔너리 키와 같은 정밀도)</summary>
        private const int TimeDecimals = 2;

        private const int DefaultFontSize = 30;

        /// <summary>칸 개수. 시간 / 타입 / 방향 / 상태 / 폰트 / 상자크기 / 텍스트.</summary>
        private const int ColumnCount = 7;

        private const int ColText = 6;

        /// <summary>
        /// 지정한 곡 폴더의 dialog.txt를 읽습니다. 파일이 없으면 false를 반환합니다(오류 아님).
        /// </summary>
        public static bool TryLoad(string songDir, out List<HwaDialogLine> lines, out string description)
        {
            lines = null;
            description = string.Empty;

            if (string.IsNullOrWhiteSpace(songDir))
            {
                description = "곡 폴더 경로가 비어 있습니다.";
                return false;
            }

            string path = Path.Combine(songDir, FileName);
            if (!File.Exists(path))
            {
                description = $"{FileName} 없음";
                return false;
            }

            try
            {
                var parsed = Parse(File.ReadAllLines(path));
                if (parsed.Count == 0)
                {
                    description = $"{path}: 유효한 대사 줄이 없습니다.";
                    return false;
                }

                lines = parsed;
                description = $"{path}: {parsed.Count}줄";
                return true;
            }
            catch (Exception ex)
            {
                description = $"{path} 읽기 실패: {ex.Message}";
                MelonLogger.Error($"{Tag} {description}");
                return false;
            }
        }

        /// <summary>파일 내용을 대사 줄 목록으로 변환합니다. (파일 입출력과 분리해 테스트 가능)</summary>
        public static List<HwaDialogLine> Parse(IEnumerable<string> rawLines)
        {
            var result = new List<HwaDialogLine>();
            if (rawLines == null) return result;

            // 비어 있는 칸이 물려받을 직전 값입니다.
            int currentType = DialogTypeCodes.Ui;
            int currentDir = DialogDirCodes.Down;
            int currentFontSize = DefaultFontSize;
            int currentBoxWidth = -1;
            int currentBoxHeight = -1;

            // 말풍선 슬롯(타입+방향)별로 열려 있는지와 마지막 텍스트를 추적합니다.
            var openSlots = new HashSet<int>();
            var lastTextBySlot = new Dictionary<int, string>();

            int lineNumber = 0;
            foreach (string rawLine in rawLines)
            {
                lineNumber++;

                string line = rawLine != null ? rawLine.Trim() : string.Empty;
                if (line.Length == 0) continue;
                if (line.StartsWith("//", StringComparison.Ordinal) || line.StartsWith("#", StringComparison.Ordinal)) continue;

                // 7번째 칸부터는 슬래시를 포함해 전부 텍스트이므로, 앞의 6칸까지만 자릅니다.
                string[] columns = line.Split(new char[] { '/' }, ColumnCount);
                int count = columns.Length;

                if (!TryParseTime(columns[0], out double time))
                {
                    MelonLogger.Warning($"{Tag} {lineNumber}번째 줄: 시간을 읽을 수 없어 건너뜁니다: '{columns[0].Trim()}'");
                    continue;
                }

                // 칸이 7개보다 적으면 마지막 칸을 텍스트로 봅니다. (뒤쪽 칸 생략 허용)
                string text = count >= ColumnCount ? columns[ColText] : (count >= 2 ? columns[count - 1] : string.Empty);
                text = text.Trim();

                if (!TryParseType(Column(columns, 1), ref currentType, lineNumber)) continue;
                if (!TryParseDir(Column(columns, 2), ref currentDir, lineNumber)) continue;
                if (!TryParseAction(Column(columns, 3), openSlots, currentType, currentDir, out HwaDialogAction action, lineNumber)) continue;
                if (!TryParseFontSize(Column(columns, 4), ref currentFontSize, lineNumber)) continue;
                if (!TryParseBoxSize(Column(columns, 5), ref currentBoxWidth, ref currentBoxHeight, lineNumber)) continue;

                int slot = SlotKey(currentType, currentDir);

                if (action == HwaDialogAction.Hide)
                {
                    // 닫는 줄은 텍스트를 비워도 되며, 그 말풍선의 마지막 텍스트를 그대로 씁니다.
                    if (text.Length == 0) lastTextBySlot.TryGetValue(slot, out text);
                    openSlots.Remove(slot);
                }
                else
                {
                    if (text.Length == 0)
                    {
                        MelonLogger.Warning($"{Tag} {lineNumber}번째 줄: 텍스트가 비어 있어 건너뜁니다. (닫으려면 상태 칸에 '비활성화'를 적으세요)");
                        continue;
                    }
                    openSlots.Add(slot);
                }

                text = (text ?? string.Empty).Replace("\\n", "\n");
                lastTextBySlot[slot] = text;

                result.Add(new HwaDialogLine
                {
                    Time = time,
                    DialogType = currentType,
                    DialogIndex = currentDir,
                    Action = action,
                    Text = text,
                    FontSize = currentFontSize,
                    BoxWidth = currentBoxWidth,
                    BoxHeight = currentBoxHeight,
                    SourceLineNumber = lineNumber
                });
            }

            // 같은 시간에 여러 줄이 오면(예: 한쪽 말풍선을 닫고 다른 쪽을 여는 경우) 순서가 연출을 좌우하므로,
            // List.Sort가 불안정 정렬인 점을 감안해 파일에 적힌 순서를 동점 기준으로 씁니다.
            result.Sort((a, b) =>
            {
                int byTime = a.Time.CompareTo(b.Time);
                return byTime != 0 ? byTime : a.SourceLineNumber.CompareTo(b.SourceLineNumber);
            });
            return result;
        }

        /// <summary>
        /// 중간 칸 값을 꺼냅니다. 마지막 칸은 텍스트이므로 중간 칸으로 취급하지 않습니다.
        /// </summary>
        private static string Column(string[] columns, int index)
        {
            int lastMiddle = columns.Length >= ColumnCount ? ColumnCount - 2 : columns.Length - 2;
            if (index < 1 || index > lastMiddle) return string.Empty;
            return columns[index];
        }

        private static int SlotKey(int dialogType, int dialogIndex)
        {
            return (dialogType * 10) + dialogIndex;
        }

        /// <summary>"5초", "7.25", "12.0s" 처럼 뒤에 단위가 붙어도 읽습니다.</summary>
        private static bool TryParseTime(string text, out double time)
        {
            time = 0.0;

            string value = (text ?? string.Empty).Trim();
            int end = 0;
            while (end < value.Length && (char.IsDigit(value[end]) || value[end] == '.' || value[end] == '-' || value[end] == '+'))
            {
                end++;
            }
            value = value.Substring(0, end);

            if (!double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out double parsed))
            {
                return false;
            }

            // 게임의 시간별 딕셔너리 키가 소수 2자리라, 같은 정밀도로 맞춰야 틱이 정확히 일치합니다.
            time = Math.Round(parsed, TimeDecimals, MidpointRounding.AwayFromZero);
            return true;
        }

        private static bool TryParseType(string text, ref int current, int lineNumber)
        {
            string value = (text ?? string.Empty).Trim();
            if (value.Length == 0) return true; // 직전 값 유지

            switch (value.ToLowerInvariant())
            {
                case "ui":
                case "방백":
                    current = DialogTypeCodes.Ui;
                    return true;
                case "boss":
                case "보스":
                    current = DialogTypeCodes.Boss;
                    return true;
                case "role":
                case "캐릭터":
                case "주인공":
                    current = DialogTypeCodes.Role;
                    return true;
            }

            MelonLogger.Warning($"{Tag} {lineNumber}번째 줄: 알 수 없는 타입 '{value}' (ui·보스·캐릭터), 줄을 건너뜁니다. " +
                                "텍스트에 슬래시를 쓰셨다면 앞의 6칸을 모두 채워주세요.");
            return false;
        }

        private static bool TryParseDir(string text, ref int current, int lineNumber)
        {
            string value = (text ?? string.Empty).Trim();
            if (value.Length == 0) return true;

            switch (value.ToLowerInvariant())
            {
                case "up":
                case "위":
                case "위쪽":
                    current = DialogDirCodes.Up;
                    return true;
                case "down":
                case "아래":
                case "아래쪽":
                    current = DialogDirCodes.Down;
                    return true;
            }

            MelonLogger.Warning($"{Tag} {lineNumber}번째 줄: 알 수 없는 방향 '{value}' (위·아래), 줄을 건너뜁니다.");
            return false;
        }

        private static bool TryParseAction(string text, HashSet<int> openSlots, int dialogType, int dialogIndex,
            out HwaDialogAction action, int lineNumber)
        {
            string value = (text ?? string.Empty).Trim();

            if (value.Length == 0)
            {
                // 닫혀 있으면 열고, 열려 있으면 텍스트만 교체합니다.
                action = openSlots.Contains(SlotKey(dialogType, dialogIndex)) ? HwaDialogAction.Update : HwaDialogAction.Show;
                return true;
            }

            switch (value.ToLowerInvariant())
            {
                case "show":
                case "활성화":
                case "열기":
                case "표시":
                    action = HwaDialogAction.Show;
                    return true;
                case "hide":
                case "비활성화":
                case "숨김":
                case "닫기":
                    action = HwaDialogAction.Hide;
                    return true;
                case "update":
                case "none":
                case "유지":
                case "갱신":
                    action = HwaDialogAction.Update;
                    return true;
            }

            action = HwaDialogAction.Update;
            MelonLogger.Warning($"{Tag} {lineNumber}번째 줄: 알 수 없는 상태 '{value}' (활성화·비활성화·유지), 줄을 건너뜁니다.");
            return false;
        }

        private static bool TryParseFontSize(string text, ref int current, int lineNumber)
        {
            string value = (text ?? string.Empty).Trim();
            if (value.Length == 0) return true;

            if (int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out int parsed) && parsed > 0)
            {
                current = parsed;
                return true;
            }

            MelonLogger.Warning($"{Tag} {lineNumber}번째 줄: 폰트 크기 '{value}'를 읽을 수 없어 줄을 건너뜁니다.");
            return false;
        }

        /// <summary>"400x150" 또는 가로만 "400"을 읽습니다. 비우면 직전 값을 유지합니다.</summary>
        private static bool TryParseBoxSize(string text, ref int currentWidth, ref int currentHeight, int lineNumber)
        {
            string value = (text ?? string.Empty).Trim();
            if (value.Length == 0) return true;

            string[] parts = value.Split(new char[] { 'x', 'X', '*' }, 2);

            if (!int.TryParse(parts[0].Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out int width) || width <= 0)
            {
                MelonLogger.Warning($"{Tag} {lineNumber}번째 줄: 상자 크기 '{value}'를 읽을 수 없어 줄을 건너뜁니다. (예: 400x150 또는 400)");
                return false;
            }

            int height = currentHeight > 0 ? currentHeight : DialogBoxSizes.DefaultHeight;
            if (parts.Length == 2)
            {
                if (!int.TryParse(parts[1].Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out height) || height <= 0)
                {
                    MelonLogger.Warning($"{Tag} {lineNumber}번째 줄: 상자 세로 크기 '{parts[1].Trim()}'를 읽을 수 없어 줄을 건너뜁니다.");
                    return false;
                }
            }

            currentWidth = width;
            currentHeight = height;
            return true;
        }
    }

    /// <summary>GameDialogArgs.dialogType 값입니다.</summary>
    public static class DialogTypeCodes
    {
        public const int Role = 0;
        public const int Boss = 1;
        public const int Ui = 2;
    }

    /// <summary>GameDialogArgs.dialogIndex(말풍선 방향) 값입니다.</summary>
    public static class DialogDirCodes
    {
        public const int Up = 0;
        public const int Down = 1;
    }

    /// <summary>
    /// 말풍선 상자 크기 기본값입니다. 공식 곡(how_to_make_otoge_kyoku)에서 실측한 값이며,
    /// 게임은 이 값을 150x100 ~ 1200x200 범위로 클램프합니다.
    /// </summary>
    public static class DialogBoxSizes
    {
        public const int DefaultWidth = 300;
        public const int DefaultHeight = 150;

        /// <summary>UI 아래쪽 말풍선만 원본이 가로 400을 씁니다.</summary>
        public const int UiDownWidth = 400;
    }
}
