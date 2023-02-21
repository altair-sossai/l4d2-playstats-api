namespace L4D2PlayStats.Core.Modules.Statistics.Results;

public class UploadResult
{
    public UploadResult(Statistics statistics)
    {
        FileName = statistics.FileName;
        MustBeDeleted = statistics.Statistic != null;
    }

    private UploadResult(string? fileName, bool mustBeDeleted)
    {
        FileName = fileName;
        MustBeDeleted = mustBeDeleted;
    }

    public string? FileName { get; }
    public bool MustBeDeleted { get; }

    public static UploadResult DeleteFile(string? fileName)
    {
        return new UploadResult(fileName, true);
    }
}