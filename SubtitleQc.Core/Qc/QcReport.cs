using System.Collections.Generic;

namespace SubtitleQc.Core.Qc;

public class QcReport
{
    public IReadOnlyList<QcResult> Results { get; }

    public QcReport(IReadOnlyList<QcResult> results)
    {
        Results = results;
    }
}
