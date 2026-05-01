using System;
using System.Collections.Generic;
using System.Linq;
using SubtitleQc.Core.Models;
using SubtitleQc.Core.Qc.Abstractions;

namespace SubtitleQc.Core.Qc.Rules;

public class MinFramesFromShotChangeRule : IQcRule
{
    private readonly IShotChangeProvider _shotChangeProvider;
    private readonly int _thresholdFrames;

    public MinFramesFromShotChangeRule(IShotChangeProvider shotChangeProvider, int thresholdFrames)
    {
        _shotChangeProvider = shotChangeProvider;
        _thresholdFrames = thresholdFrames;
    }

    public IEnumerable<QcResult> Evaluate(IReadOnlyList<Cue> cues)
    {
        var cutFrames = _shotChangeProvider.GetShotChangeFrames();
        
        return cues.Select(c => 
        {
            if (!c.StartFrame.HasValue) return new QcResult(c.Id, QcStatus.Passed);

            bool tooClose = cutFrames.Any(cut => 
                Math.Abs(c.StartFrame.Value - cut) < _thresholdFrames);

            return new QcResult(c.Id, tooClose ? QcStatus.Failed : QcStatus.Passed);
        });
    }
}
