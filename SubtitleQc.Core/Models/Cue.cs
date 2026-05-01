using System;
using System.Collections.Generic;

namespace SubtitleQc.Core.Models;

public class Cue
{
    public string Id { get; }
    public TimeSpan Start { get; }
    public TimeSpan End { get; }
    public IReadOnlyList<string> Lines { get; }

    public Cue(string id, TimeSpan start, TimeSpan end, IReadOnlyList<string> lines)
    {
        Id = id;
        Start = start;
        End = end;
        Lines = lines;
    }
}
