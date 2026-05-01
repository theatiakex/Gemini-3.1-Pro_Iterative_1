using System;
using System.Collections.Generic;
using System.Globalization;
using SubtitleQc.Core.Models;

namespace SubtitleQc.Core.Parsers;

public class WebVttParser : ISubtitleParser
{
    public IReadOnlyList<Cue> Parse(string content)
    {
        var cues = new List<Cue>();
        if (string.IsNullOrWhiteSpace(content)) return cues;

        var blocks = content.Split(new[] { "\r\n\r\n", "\n\n" }, StringSplitOptions.RemoveEmptyEntries);
        int idCounter = 1;

        foreach (var block in blocks)
        {
            if (block.Trim().StartsWith("WEBVTT")) continue;
            ParseBlock(block, cues, ref idCounter);
        }

        return cues;
    }

    private void ParseBlock(string block, List<Cue> cues, ref int idCounter)
    {
        var lines = block.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
        if (lines.Length < 2) return;

        int timeLineIndex = FindTimeLineIndex(lines);
        if (timeLineIndex == -1) return;

        string id = timeLineIndex > 0 ? lines[0].Trim() : idCounter.ToString();
        string[] timeParts = lines[timeLineIndex].Split(new[] { " --> " }, StringSplitOptions.None);
        if (timeParts.Length != 2) return;

        string startStr = timeParts[0].Trim().Split(' ')[0];
        string endStr = timeParts[1].Trim().Split(' ')[0];

        if (TryParseTime(startStr, out TimeSpan start) && TryParseTime(endStr, out TimeSpan end))
        {
            var textLines = new List<string>();
            for (int i = timeLineIndex + 1; i < lines.Length; i++) textLines.Add(lines[i]);
            cues.Add(new Cue(id, start, end, textLines));
            idCounter++;
        }
    }

    private int FindTimeLineIndex(string[] lines)
    {
        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i].Contains(" --> ")) return i;
        }
        return -1;
    }

    private bool TryParseTime(string timeStr, out TimeSpan time)
    {
        if (TimeSpan.TryParseExact(timeStr, @"hh\:mm\:ss\.fff", CultureInfo.InvariantCulture, out time)) return true;
        if (TimeSpan.TryParseExact(timeStr, @"mm\:ss\.fff", CultureInfo.InvariantCulture, out time)) return true;
        return false;
    }
}
