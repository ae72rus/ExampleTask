namespace Zvonarev.FinBeat.Test.HttpDbLogging.Configuration;

public record HttpLoggingConfiguration
{
    public static HttpLoggingConfiguration Default { get; } = new()
    {
        DefaultLogBatchSize = 50,
        DefaultLogSubmissionInterval = TimeSpan.FromSeconds(1)
    };

    public int DefaultLogBatchSize { get; init; }
    public TimeSpan DefaultLogSubmissionInterval { get; init; }
}