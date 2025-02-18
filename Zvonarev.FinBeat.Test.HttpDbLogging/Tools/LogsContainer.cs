using System.Collections.Concurrent;
using Zvonarev.FinBeat.Test.DomainObjects;
using Zvonarev.FinBeat.Test.HttpDbLogging.Configuration;
using Zvonarev.FinBeat.Test.Storage.Configuration;

namespace Zvonarev.FinBeat.Test.HttpDbLogging.Tools;

internal class LogsContainer
{
    private readonly HttpLoggingConfiguration _configuration;
    private readonly ConcurrentQueue<HttpRequestInfo> _logs = new();

    public LogsContainer(HttpLoggingConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void AddLog(HttpRequestInfo log)
    {
        _logs.Enqueue(log);
    }

    public IEnumerable<HttpRequestInfo> GetLogsBatch()
    {
        if(!_logs.Any())
            yield break;

        var counter = 0;
        while (counter <= _configuration.DefaultLogBatchSize && _logs.TryDequeue(out var log))//condition order matters
        {
            counter++;
            yield return log;
        }
    }
}