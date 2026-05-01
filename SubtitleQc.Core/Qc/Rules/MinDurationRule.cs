using System;
using System.Collections.Generic;
using System.Linq;
using SubtitleQc.Core.Models;
using SubtitleQc.Core.Qc.Abstractions;

namespace SubtitleQc.Core.Qc.Rules;

public class MinDurationRule : IQcRule
{
    private readonly TimeSpan _threshold;

    public MinDurationRule(TimeSpan threshold)
    {
        _threshold = threshold;
    }

    public IEnumerable<QcResult> Evaluate(IReadOnlyList<Cue> cues)
    {
        return cues.Select(c => new QcResult(
            c.Id, 
            (c.End - c.Start) < _threshold ? QcStatus.Failed : QcStatus.Passed));
    }
}
