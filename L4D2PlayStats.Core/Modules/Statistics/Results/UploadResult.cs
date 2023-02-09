namespace L4D2PlayStats.Core.Modules.Statistics.Results;

public class UploadResult
{
    public UploadResult(Statistics statistics)
    {
        FileName = statistics.FileName;
        MustBeDeleted = false;
    }

    public string? FileName { get; }
    public bool MustBeDeleted { get; }
}