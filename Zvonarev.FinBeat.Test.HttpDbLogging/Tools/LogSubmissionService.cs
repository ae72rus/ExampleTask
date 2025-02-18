using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Zvonarev.FinBeat.Test.HttpDbLogging.Configuration;
using Zvonarev.FinBeat.Test.Storage.UseCases.SaveApiLog;

namespace Zvonarev.FinBeat.Test.HttpDbLogging.Tools;

internal class LogSubmissionService : IHostedService
{
    private Task _submissionJob;
    private bool _stopRequested;
    private readonly LogsContainer _logsContainer;
    private readonly HttpLoggingConfiguration _loggingConfiguration;
    private readonly ILogger<LogSubmissionService> _logger;
    private readonly IServiceProvider _sp;

    public LogSubmissionService(
        LogsContainer logsContainer,
        HttpLoggingConfiguration loggingConfiguration,
        ILogger<LogSubmissionService> logger,
        IServiceProvider sp)
    {
        _logsContainer = logsContainer;
        _loggingConfiguration = loggingConfiguration;
        _logger = logger;
        _sp = sp;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _submissionJob = RunLogsSubmission();

        return Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _stopRequested = true;
        await _submissionJob;
    }

    private Task RunLogsSubmission()
    {
        return Task.Factory.StartNew(async () =>
        {
            while (true)
            {
                await Task.Delay(_loggingConfiguration.DefaultLogSubmissionInterval);

                var logsBatch = _logsContainer
                    .GetLogsBatch()
                    .ToArray();

                if (!logsBatch.Any() && _stopRequested)
                    break;

                if(!logsBatch.Any())
                    continue;

                try
                {
                    await using var scope = _sp.CreateAsyncScope();//creates scope containing db context
                    await scope.ServiceProvider.GetRequiredService<IMediator>().Send(new SaveApiLogsCommand(logsBatch));
                }
                catch (Exception e)
                {
                    _logger.LogCritical(e, "Failed to save api requests logs");
                }
            }
        }, TaskCreationOptions.LongRunning);
    }
}