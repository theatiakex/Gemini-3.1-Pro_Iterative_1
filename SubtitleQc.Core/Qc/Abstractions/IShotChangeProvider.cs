using System;
using System.Collections.Generic;

namespace SubtitleQc.Core.Qc.Abstractions;

public interface IShotChangeProvider
{
    IReadOnlyList<TimeSpan> GetShotChangeTimestamps();
    IReadOnlyList<int> GetShotChangeFrames();
}
