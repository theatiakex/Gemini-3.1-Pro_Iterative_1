namespace SubtitleQc.Core.Qc;

public class QcResult
{
    public string CueId { get; }
    public QcStatus Status { get; }

    public QcResult(string cueId, QcStatus status)
    {
        CueId = cueId;
        Status = status;
    }
}
