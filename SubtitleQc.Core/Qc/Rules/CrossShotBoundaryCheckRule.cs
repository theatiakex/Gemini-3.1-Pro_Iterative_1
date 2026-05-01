using System;
using System.Collections.Generic;
using System.Linq;
using SubtitleQc.Core.Models;
using SubtitleQc.Core.Qc.Abstractions;

namespace SubtitleQc.Core.Qc.Rules;

public class CrossShotBoundaryCheckRule : IQcRule
{
    private readonly IShotChangeProvider _shotChangeProvider;

    public CrossShotBoundaryCheckRule(IShotChangeProvider shotChangeProvider)
    {
        _shotChangeProvider = shotChangeProvider;
    }

    public IEnumerable<QcResult> Evaluate(IReadOnlyList<Cue> cues)
    {
        var cuts = _shotChangeProvider.GetShotChangeTimestamps();
        
        return cues.Select(c => 
        {
            bool spansCut = cuts.Any(cut => cut > c.Start && cut < c.End);
            return new QcResult(c.Id, spansCut ? QcStatus.Failed : QcStatus.Passed);
        });
    }
}
