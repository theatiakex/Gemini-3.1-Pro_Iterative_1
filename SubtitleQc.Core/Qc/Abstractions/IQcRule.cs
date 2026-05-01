using System.Collections.Generic;
using SubtitleQc.Core.Models;

namespace SubtitleQc.Core.Qc.Abstractions;

public interface IQcRule
{
    IEnumerable<QcResult> Evaluate(IReadOnlyList<Cue> cues);
}
