using System.Collections.Generic;
using System.Linq;
using SubtitleQc.Core.Models;
using SubtitleQc.Core.Qc.Abstractions;

namespace SubtitleQc.Core.Qc.Rules;

public class MaxCplRule : IQcRule
{
    private readonly int _threshold;

    public MaxCplRule(int threshold)
    {
        _threshold = threshold;
    }

    public IEnumerable<QcResult> Evaluate(IReadOnlyList<Cue> cues)
    {
        return cues.Select(c => new QcResult(
            c.Id, 
            c.Lines.Any(l => l.Length > _threshold) ? QcStatus.Failed : QcStatus.Passed));
    }
}
