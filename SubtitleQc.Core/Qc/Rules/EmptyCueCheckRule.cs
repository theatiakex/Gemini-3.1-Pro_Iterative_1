using System.Collections.Generic;
using System.Linq;
using SubtitleQc.Core.Models;
using SubtitleQc.Core.Qc.Abstractions;

namespace SubtitleQc.Core.Qc.Rules;

public class EmptyCueCheckRule : IQcRule
{
    public IEnumerable<QcResult> Evaluate(IReadOnlyList<Cue> cues)
    {
        return cues.Select(c => new QcResult(
            c.Id, 
            c.Lines.All(string.IsNullOrWhiteSpace) ? QcStatus.Failed : QcStatus.Passed));
    }
}
