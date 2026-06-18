using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace muse_dash_test
{
    // BMS 텍스트 → BmsChart 변환 본체.
    // 저수준 토큰 파싱(정규식/셀 처리)은 BmsParser.Lexer.cs, 데이터 모델은 BmsModels.cs 참고.
    public static partial class BmsParser
    {
        private static readonly Dictionary<int, BmsLane> ChannelToLaneMap = new Dictionary<int, BmsLane>
        {
            { 0x13, BmsLane.Note },
            { 0x14, BmsLane.Note },
            { 0x15, BmsLane.BossInOut },
            { 0x18, BmsLane.BossAction },
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

                // Check TryParseMeasureLine first as 95%+ lines in BMS are measure lines
                if (TryParseMeasureLine(line, out int measure, out int channel, out string data))
                {
                    measureEvents.Add(new RawMeasureEvent
                    {
                        Measure = measure,
                        Channel = channel,
                        Data = NormalizeHexData(data),
                    });
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
            }

            chart.DefaultBpm = defaultBpm;
            chart.BpmDefinitions = bpmDefinitions;
            chart.Metadata = metadata;
            int noteCellWidth = DetectWavCellWidth(metadata);

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
                if (!ChannelToLaneMap.TryGetValue(evt.Channel, out BmsLane lane))
                {
                    continue;
                }

                AddNoteEvents(evt, lane, notes, noteCellWidth);
            }

            bpmChanges.Sort((left, right) =>
            {
                int tickCompare = left.Tick.CompareTo(right.Tick);
                return tickCompare != 0
                    ? tickCompare
                    : string.Compare(left.Source, right.Source, StringComparison.OrdinalIgnoreCase);
            });

            // O(N + M) single sweep to calculate time for all notes
            notes.Sort((left, right) =>
            {
                int tickCompare = left.Tick.CompareTo(right.Tick);
                return tickCompare != 0 ? tickCompare : left.Channel.CompareTo(right.Channel);
            });
            float time = 0f;
            float currentTick = 0f;
            float currentBpm = (bpmChanges.Count > 0 && bpmChanges[0].Bpm > 0f) ? bpmChanges[0].Bpm : defaultBpm;
            int bpmIndex = 0;

            foreach (var note in notes)
            {
                while (bpmIndex < bpmChanges.Count)
                {
                    var change = bpmChanges[bpmIndex];
                    if (change.Tick > note.Tick)
                    {
                        break;
                    }

                    if (change.Tick > currentTick)
                    {
                        time += (change.Tick - currentTick) * 4f * (60f / currentBpm);
                        currentTick = change.Tick;
                    }

                    currentBpm = change.Bpm > 0f ? change.Bpm : currentBpm;
                    bpmIndex++;
                }

                float noteTime = time;
                if (note.Tick > currentTick)
                {
                    noteTime += (note.Tick - currentTick) * 4f * (60f / currentBpm);
                }
                note.Time = noteTime;
            }

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

            foreach (var change in bpmChanges)
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
            int cellWidth = 2;
            int cellCount = GetCellCount(evt.Data, cellWidth);
            if (cellCount == 0)
            {
                return;
            }

            for (int i = 0; i < cellCount; i++)
            {
                int offset = i * cellWidth;
                if (IsZeroCell(evt.Data, offset, cellWidth))
                {
                    continue;
                }

                string cell = evt.Data.Substring(offset, cellWidth);
                if (!bpmDefinitions.TryGetValue(cell, out float bpm))
                {
                    continue;
                }

                bpmChanges.Add(new BpmChange
                {
                    Tick = evt.Measure + (i / (float)cellCount),
                    Bpm = bpm,
                    Source = $"BPM{cell}"
                });
            }
        }

        private static void AddDirectBpmChanges(RawMeasureEvent evt, List<BpmChange> bpmChanges)
        {
            int cellWidth = 2;
            int cellCount = GetCellCount(evt.Data, cellWidth);
            if (cellCount == 0)
            {
                return;
            }

            for (int i = 0; i < cellCount; i++)
            {
                int offset = i * cellWidth;
                if (IsZeroCell(evt.Data, offset, cellWidth))
                {
                    continue;
                }

                string cell = evt.Data.Substring(offset, cellWidth);
                if (!TryParseBpmValue(cell, out float bpm))
                {
                    continue;
                }

                bpmChanges.Add(new BpmChange
                {
                    Tick = evt.Measure + (i / (float)cellCount),
                    Bpm = bpm,
                    Source = "03"
                });
            }
        }

        private static void AddNoteEvents(RawMeasureEvent evt, BmsLane lane, List<BmsNote> notes, int cellWidth)
        {
            int width = Math.Max(2, cellWidth);
            int cellCount = GetCellCount(evt.Data, width);
            if (cellCount == 0)
            {
                return;
            }

            for (int i = 0; i < cellCount; i++)
            {
                int offset = i * width;
                if (IsZeroCell(evt.Data, offset, width))
                {
                    continue;
                }

                string cell = evt.Data.Substring(offset, width);
                notes.Add(new BmsNote
                {
                    Measure = evt.Measure,
                    Channel = evt.Channel,
                    CellIndex = i,
                    Lane = lane,
                    RawValue = cell,
                    Tick = evt.Measure + (i / (float)cellCount)
                });
            }
        }

        private sealed class RawMeasureEvent
        {
            public int Measure;
            public int Channel;
            public string Data;
        }
    }
}
