using System;
using System.Collections.Generic;
using System.Globalization;
using SubtitleQc.Core.Models;

namespace SubtitleQc.Core.Parsers;

public class SrtParser : ISubtitleParser
{
    public IReadOnlyList<Cue> Parse(string content)
    {
        var cues = new List<Cue>();
        if (string.IsNullOrWhiteSpace(content)) return cues;

        var blocks = content.Split(new[] { "\r\n\r\n", "\n\n" }, StringSplitOptions.RemoveEmptyEntries);
        foreach (var block in blocks)
        {
            ParseBlock(block, cues);
        }

        return cues;
    }

    private void ParseBlock(string block, List<Cue> cues)
    {
        var lines = block.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
        if (lines.Length < 3) return;

        string id = lines[0].Trim();
        string[] timeParts = lines[1].Split(new[] { " --> " }, StringSplitOptions.None);
        if (timeParts.Length != 2) return;

        if (TimeSpan.TryParseExact(timeParts[0].Trim(), @"hh\:mm\:ss\,fff", CultureInfo.InvariantCulture, out TimeSpan start) &&
            TimeSpan.TryParseExact(timeParts[1].Trim(), @"hh\:mm\:ss\,fff", CultureInfo.InvariantCulture, out TimeSpan end))
        {
            var textLines = new List<string>();
            for (int i = 2; i < lines.Length; i++) textLines.Add(lines[i]);
            cues.Add(new Cue(id, start, end, textLines));
        }
    }
}
