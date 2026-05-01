using System.Collections.Generic;
using System.Linq;
using SubtitleQc.Core.Models;
using SubtitleQc.Core.Qc.Abstractions;

namespace SubtitleQc.Core.Qc.Rules;

public class OverlapCheckRule : IQcRule
{
    public IEnumerable<QcResult> Evaluate(IReadOnlyList<Cue> cues)
    {
        var results = new List<QcResult>();
        var sorted = cues.OrderBy(c => c.Start).ToList();
        
        for (int i = 0; i < sorted.Count; i++)
        {
            bool failed = false;
            for (int j = 0; j < i; j++)
            {
                if (sorted[j].End > sorted[i].Start)
                {
                    failed = true;
                    break;
                }
            }
            results.Add(new QcResult(sorted[i].Id, failed ? QcStatus.Failed : QcStatus.Passed));
        }
        return results;
    }
}
