using System.Collections.Generic;
using System.Linq;
using SubtitleQc.Core.Models;
using SubtitleQc.Core.Qc.Abstractions;

namespace SubtitleQc.Core.Qc.Rules;

public class MaxCpsRule : IQcRule
{
    private readonly int _threshold;

    public MaxCpsRule(int threshold)
    {
        _threshold = threshold;
    }

    public IEnumerable<QcResult> Evaluate(IReadOnlyList<Cue> cues)
    {
        return cues.Select(c => 
        {
            double duration = (c.End - c.Start).TotalSeconds;
            if (duration <= 0) return new QcResult(c.Id, QcStatus.Failed);
            int chars = c.Lines.Sum(l => l.Length);
            return new QcResult(c.Id, (chars / duration) > _threshold ? QcStatus.Failed : QcStatus.Passed);
        });
    }
}
