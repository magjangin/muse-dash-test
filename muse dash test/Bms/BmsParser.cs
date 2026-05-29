using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace muse_dash_test
{
    public static class BmsParser
    {
        private static readonly Regex HeaderBpmRegex = new Regex(@"^#BPM\s+([0-9]+(?:\.[0-9]+)?)\s*$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex BpmAliasRegex = new Regex(@"^#BPM([0-9A-Fa-f]{2})\s*[:=]\s*([0-9]+(?:\.[0-9]+)?)\s*$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex MeasureLineRegex = new Regex(@"^#(?<measure>\d{3})(?<channel>[0-9A-Fa-f]{2})\s*:\s*(?<data>[0-9A-Fa-f\s]+)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex MetadataRegex = new Regex(@"^#(?<key>[A-Za-z0-9_]+)\s+(?<value>.+)$", RegexOptions.Compiled);

        private static readonly Dictionary<int, int> ChannelToLaneMap = new Dictionary<int, int>
        {
            { 0x11, 0 },
            { 0x12, 1 },
            { 0x14, 2 },
            { 0x15, 3 },
            { 0x16, 4 },
            { 0x18, 5 },
        };

        public static BmsChart ParseFile(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentException("filePath is null or empty.", nameof(filePath));
            }

            return ParseText(File.ReadAllText(filePath), filePath);
        }

        public static BmsChart ParseText(string text, string sourcePath = null)
        {
            var chart = new BmsChart(sourcePath);
            if (string.IsNullOrWhiteSpace(text))
            {
                return chart;
            }

            var measureEvents = new List<RawMeasureEvent>();
            var metadata = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            var bpmDefinitions = new Dictionary<string, float>(StringComparer.OrdinalIgnoreCase);
            float defaultBpm = 120f;

            foreach (var rawLine in text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None))
            {
                var line = StripComments(rawLine).Trim();
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                if (TryParseHeaderBpm(line, out float headerBpm))
                {
                    defaultBpm = headerBpm;
                    continue;
                }

                if (TryParseBpmAlias(line, out string bpmKey, out float bpmValue))
                {
                    bpmDefinitions[bpmKey] = bpmValue;
                    continue;
                }

                if (TryParseMetadata(line, out string metaKey, out string metaValue))
                {
                    metadata[metaKey] = metaValue;
                    if (string.Equals(metaKey, "TITLE", StringComparison.OrdinalIgnoreCase))
                    {
                        chart.Title = metaValue;
                    }
                    else if (string.Equals(metaKey, "ARTIST", StringComparison.OrdinalIgnoreCase))
                    {
                        chart.Artist = metaValue;
                    }
                    continue;
                }

                if (TryParseMeasureLine(line, out int measure, out int channel, out string data))
                {
                    measureEvents.Add(new RawMeasureEvent
                    {
                        Measure = measure,
                        Channel = channel,
                        Data = NormalizeHexData(data),
                    });
                }
            }

            chart.DefaultBpm = defaultBpm;
            chart.BpmDefinitions = bpmDefinitions;
            chart.Metadata = metadata;

            var bpmChanges = new List<BpmChange>
            {
                new BpmChange
                {
                    Tick = 0f,
                    Bpm = defaultBpm,
                    Source = "default"
                }
            };

            foreach (var evt in measureEvents)
            {
                if (evt.Channel == 0x08)
                {
                    AddBpmReferenceChanges(evt, bpmDefinitions, bpmChanges);
                }
                else if (evt.Channel == 0x03)
                {
                    AddDirectBpmChanges(evt, bpmChanges);
                }
            }

            var notes = new List<BmsNote>();
            foreach (var evt in measureEvents)
            {
                if (!ChannelToLaneMap.TryGetValue(evt.Channel, out int lane))
                {
                    continue;
                }

                AddNoteEvents(evt, lane, notes);
            }

            bpmChanges = bpmChanges
                .OrderBy(change => change.Tick)
                .ThenBy(change => change.Source, StringComparer.OrdinalIgnoreCase)
                .ToList();

            foreach (var note in notes)
            {
                note.Time = CalculateTime(note.Tick, bpmChanges);
            }

            notes = notes
                .OrderBy(note => note.Time)
                .ThenBy(note => note.Tick)
                .ThenBy(note => note.Lane)
                .ToList();

            chart.BpmChanges = bpmChanges;
            chart.Notes = notes;
            return chart;
        }

        public static float CalculateTime(float tick, IReadOnlyList<BpmChange> bpmChanges)
        {
            if (bpmChanges == null || bpmChanges.Count == 0)
            {
                return tick * 4f * (60f / 120f);
            }

            float time = 0f;
            float currentTick = 0f;
            float currentBpm = bpmChanges[0].Bpm <= 0f ? 120f : bpmChanges[0].Bpm;

            foreach (var change in bpmChanges.OrderBy(change => change.Tick))
            {
                if (change.Tick <= currentTick)
                {
                    currentBpm = change.Bpm > 0f ? change.Bpm : currentBpm;
                    continue;
                }

                if (change.Tick > tick)
                {
                    break;
                }

                time += (change.Tick - currentTick) * 4f * (60f / currentBpm);
                currentTick = change.Tick;
                currentBpm = change.Bpm > 0f ? change.Bpm : currentBpm;
            }

            if (tick > currentTick)
            {
                time += (tick - currentTick) * 4f * (60f / currentBpm);
            }

            return time;
        }

        private static void AddBpmReferenceChanges(RawMeasureEvent evt, Dictionary<string, float> bpmDefinitions, List<BpmChange> bpmChanges)
        {
            var cells = SplitCells(evt.Data);
            if (cells.Count == 0)
            {
                return;
            }

            for (int i = 0; i < cells.Count; i++)
            {
                var cell = cells[i];
                if (string.IsNullOrWhiteSpace(cell) || cell == "00")
                {
                    continue;
                }

                if (!bpmDefinitions.TryGetValue(cell, out float bpm))
                {
                    continue;
                }

                bpmChanges.Add(new BpmChange
                {
                    Tick = evt.Measure + (i / (float)cells.Count),
                    Bpm = bpm,
                    Source = $"BPM{cell}"
                });
            }
        }

        private static void AddDirectBpmChanges(RawMeasureEvent evt, List<BpmChange> bpmChanges)
        {
            var cells = SplitCells(evt.Data);
            if (cells.Count == 0)
            {
                return;
            }

            for (int i = 0; i < cells.Count; i++)
            {
                var cell = cells[i];
                if (string.IsNullOrWhiteSpace(cell) || cell == "00")
                {
                    continue;
                }

                if (!TryParseBpmValue(cell, out float bpm))
                {
                    continue;
                }

                bpmChanges.Add(new BpmChange
                {
                    Tick = evt.Measure + (i / (float)cells.Count),
                    Bpm = bpm,
                    Source = "03"
                });
            }
        }

        private static void AddNoteEvents(RawMeasureEvent evt, int lane, List<BmsNote> notes)
        {
            var cells = SplitCells(evt.Data);
            if (cells.Count == 0)
            {
                return;
            }

            for (int i = 0; i < cells.Count; i++)
            {
                var cell = cells[i];
                if (string.IsNullOrWhiteSpace(cell) || cell == "00")
                {
                    continue;
                }

                notes.Add(new BmsNote
                {
                    Measure = evt.Measure,
                    Channel = evt.Channel,
                    CellIndex = i,
                    Lane = lane,
                    RawValue = cell,
                    Tick = evt.Measure + (i / (float)cells.Count)
                });
            }
        }

        private static bool TryParseHeaderBpm(string line, out float bpm)
        {
            bpm = 0f;
            var match = HeaderBpmRegex.Match(line);
            if (!match.Success)
            {
                return false;
            }

            return float.TryParse(match.Groups[1].Value, NumberStyles.Float, CultureInfo.InvariantCulture, out bpm);
        }

        private static bool TryParseBpmAlias(string line, out string key, out float bpm)
        {
            key = null;
            bpm = 0f;

            var match = BpmAliasRegex.Match(line);
            if (!match.Success)
            {
                return false;
            }

            key = match.Groups[1].Value.ToUpperInvariant();
            return float.TryParse(match.Groups[2].Value, NumberStyles.Float, CultureInfo.InvariantCulture, out bpm);
        }

        private static bool TryParseMetadata(string line, out string key, out string value)
        {
            key = null;
            value = null;

            if (!line.StartsWith("#", StringComparison.Ordinal))
            {
                return false;
            }

            if (MeasureLineRegex.IsMatch(line) || HeaderBpmRegex.IsMatch(line) || BpmAliasRegex.IsMatch(line))
            {
                return false;
            }

            var match = MetadataRegex.Match(line);
            if (!match.Success)
            {
                return false;
            }

            key = match.Groups["key"].Value.Trim();
            value = match.Groups["value"].Value.Trim();
            return !string.IsNullOrWhiteSpace(key) && !string.IsNullOrWhiteSpace(value);
        }

        private static bool TryParseMeasureLine(string line, out int measure, out int channel, out string data)
        {
            measure = 0;
            channel = 0;
            data = null;

            var match = MeasureLineRegex.Match(line);
            if (!match.Success)
            {
                return false;
            }

            if (!int.TryParse(match.Groups["measure"].Value, NumberStyles.Integer, CultureInfo.InvariantCulture, out measure))
            {
                return false;
            }

            if (!int.TryParse(match.Groups["channel"].Value, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out channel))
            {
                return false;
            }

            data = match.Groups["data"].Value;
            return !string.IsNullOrWhiteSpace(data);
        }

        private static bool TryParseBpmValue(string text, out float bpm)
        {
            bpm = 0f;

            if (float.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out bpm))
            {
                return true;
            }

            if (int.TryParse(text, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out int bpmAsHex))
            {
                bpm = bpmAsHex;
                return true;
            }

            return false;
        }

        private static List<string> SplitCells(string data)
        {
            var cells = new List<string>();
            if (string.IsNullOrWhiteSpace(data))
            {
                return cells;
            }

            var cleaned = NormalizeHexData(data);
            if (cleaned.Length < 2)
            {
                return cells;
            }

            for (int i = 0; i + 1 < cleaned.Length; i += 2)
            {
                cells.Add(cleaned.Substring(i, 2));
            }

            return cells;
        }

        private static string NormalizeHexData(string data)
        {
            if (string.IsNullOrWhiteSpace(data))
            {
                return string.Empty;
            }

            var chars = data.Where(char.IsLetterOrDigit).ToArray();
            return new string(chars).ToUpperInvariant();
        }

        private static string StripComments(string line)
        {
            if (string.IsNullOrEmpty(line))
            {
                return string.Empty;
            }

            int commentIndex = line.IndexOf("//", StringComparison.Ordinal);
            if (commentIndex >= 0)
            {
                return line.Substring(0, commentIndex);
            }

            commentIndex = line.IndexOf(';');
            if (commentIndex >= 0)
            {
                return line.Substring(0, commentIndex);
            }

            return line;
        }

        private sealed class RawMeasureEvent
        {
            public int Measure;
            public int Channel;
            public string Data;
        }
    }

    public sealed class BmsChart
    {
        internal BmsChart(string sourcePath)
        {
            SourcePath = sourcePath;
            Notes = new List<BmsNote>();
            BpmChanges = new List<BpmChange>();
            BpmDefinitions = new Dictionary<string, float>(StringComparer.OrdinalIgnoreCase);
            Metadata = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }

        public string SourcePath { get; }
        public string Title { get; internal set; }
        public string Artist { get; internal set; }
        public float DefaultBpm { get; internal set; }
        public IReadOnlyDictionary<string, float> BpmDefinitions { get; internal set; }
        public Dictionary<string, string> Metadata { get; internal set; }
        public IReadOnlyList<BpmChange> BpmChanges { get; internal set; }
        public IReadOnlyList<BmsNote> Notes { get; internal set; }
    }

    public sealed class BmsNote
    {
        public int Measure { get; internal set; }
        public int Channel { get; internal set; }
        public int CellIndex { get; internal set; }
        public int Lane { get; internal set; }
        public string RawValue { get; internal set; }
        public float Tick { get; internal set; }
        public float Time { get; internal set; }
    }

    public sealed class BpmChange
    {
        public float Tick { get; internal set; }
        public float Bpm { get; internal set; }
        public string Source { get; internal set; }
    }
}
