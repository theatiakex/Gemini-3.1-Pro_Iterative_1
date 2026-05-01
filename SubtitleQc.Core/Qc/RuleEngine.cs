using System.Collections.Generic;
using System.Linq;
using SubtitleQc.Core.Models;
using SubtitleQc.Core.Qc.Abstractions;

namespace SubtitleQc.Core.Qc;

public class RuleEngine
{
    private readonly IReadOnlyList<IQcRule> _rules;

    public RuleEngine(IEnumerable<IQcRule> rules)
    {
        _rules = rules.ToList();
    }

    public QcReport Evaluate(IReadOnlyList<Cue> cues)
    {
        var allResults = _rules.SelectMany(r => r.Evaluate(cues)).ToList();
        
        var finalResults = cues.Select(c => 
        {
            var failed = allResults.Any(r => r.CueId == c.Id && r.Status == QcStatus.Failed);
            return new QcResult(c.Id, failed ? QcStatus.Failed : QcStatus.Passed);
        }).ToList();

        return new QcReport(finalResults);
    }
}
