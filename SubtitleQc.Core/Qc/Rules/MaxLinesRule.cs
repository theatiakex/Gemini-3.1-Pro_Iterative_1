using System.Collections.Generic;
using System.Linq;
using SubtitleQc.Core.Models;
using SubtitleQc.Core.Qc.Abstractions;

namespace SubtitleQc.Core.Qc.Rules;

public class MaxLinesRule : IQcRule
{
    private readonly int _threshold;

    public MaxLinesRule(int threshold)
    {
        _threshold = threshold;
    }

    public IEnumerable<QcResult> Evaluate(IReadOnlyList<Cue> cues)
    {
        return cues.Select(c => new QcResult(
            c.Id, 
            c.Lines.Count > _threshold ? QcStatus.Failed : QcStatus.Passed));
    }
}
