using MediatR;
using Microsoft.Extensions.Logging;
using Zvonarev.FinBeat.Test.Storage.Tools.Ef;
using Zvonarev.FinBeat.Test.Storage.Tools.Ef.Models;

namespace Zvonarev.FinBeat.Test.Storage.UseCases.SaveApiLog;

internal class SaveApiLogsCommandHandler : IRequestHandler<SaveApiLogsCommand>
{
    private readonly ILogger<SaveApiLogsCommandHandler> _logger;
    private readonly LoggingDbContext _db;

    public SaveApiLogsCommandHandler(ILogger<SaveApiLogsCommandHandler> logger,
        LoggingDbContext db)
    {
        _logger = logger;
        _db = db;
    }

    public async Task Handle(SaveApiLogsCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var apiLogs = request.Logs
                .Select(x => new DbApiLog(
                    0,
                    x.ReferenceId,
                    x.HttpId,
                    x.InitiatorIp,
                    x.Method,
                    x.Url,
                    x.Headers,
                    x.Payload,
                    x.ResponseCode,
                    x.Response,
                    x.ResponseTime
                ))
                .ToArray();

            if (!apiLogs.Any())
                return;

            await _db.Set<DbApiLog>()
                .AddRangeAsync(apiLogs, cancellationToken);

            await _db.SaveChangesAsync(cancellationToken);
        }
        catch
        {
            _logger.LogCritical("Unable to save Api Requests logs. Items: {items}", request.Logs);
            throw;
        }
    }
}