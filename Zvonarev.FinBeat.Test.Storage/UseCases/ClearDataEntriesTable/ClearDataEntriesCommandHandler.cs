using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Zvonarev.FinBeat.Test.Storage.Tools.Ef;
using Zvonarev.FinBeat.Test.Storage.Tools.Ef.EntityConfigurations;

namespace Zvonarev.FinBeat.Test.Storage.UseCases.ClearDataEntriesTable;

internal class ClearDataEntriesCommandHandler : IRequestHandler<ClearDataEntriesCommand>
{
    private readonly AppDbContext _db;
    private readonly ILogger<ClearDataEntriesCommandHandler> _logger;

    public ClearDataEntriesCommandHandler(AppDbContext db,
        ILogger<ClearDataEntriesCommandHandler> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task Handle(ClearDataEntriesCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _db.Database.ExecuteSqlRawAsync(
                $"TRUNCATE TABLE [{DbDataEntryConfiguration.Schema}].[{DbDataEntryConfiguration.TableName}]",
                cancellationToken);
        }
        catch
        {
            _logger.LogError("Failed to truncate data entries table");
            throw;
        }
    }
}