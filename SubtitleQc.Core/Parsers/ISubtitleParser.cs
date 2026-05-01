using System.Collections.Generic;
using SubtitleQc.Core.Models;

namespace SubtitleQc.Core.Parsers;

public interface ISubtitleParser
{
    IReadOnlyList<Cue> Parse(string content);
}
