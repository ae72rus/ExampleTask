using MediatR;
using Microsoft.Extensions.Logging;
using Zvonarev.FinBeat.Test.Storage.Tools.Ef;
using Zvonarev.FinBeat.Test.Storage.Tools.Ef.Models;

namespace Zvonarev.FinBeat.Test.Storage.UseCases.SaveDataEntries;

internal class SaveDataEntriesCommandHandler : IRequestHandler<SaveDataEntriesCommand>
{
    private readonly AppDbContext _db;
    private readonly ILogger<SaveDataEntriesCommandHandler> _logger;

    public SaveDataEntriesCommandHandler(AppDbContext db,
        ILogger<SaveDataEntriesCommandHandler> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task Handle(SaveDataEntriesCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var orderId = 0;
            var dbEntries = request
                    .Entries
                    .Select(x => new DbDataEntry(0, orderId++, x.Code, x.Value))
                ;

            await _db.Set<DbDataEntry>()
                .AddRangeAsync(dbEntries, cancellationToken);

            await _db.SaveChangesAsync(cancellationToken);
        }
        catch
        {
            _logger.LogError("Failed to save data entries");
            throw;
        }
    }
}