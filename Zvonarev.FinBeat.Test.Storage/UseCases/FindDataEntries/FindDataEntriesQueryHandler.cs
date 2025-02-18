using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Zvonarev.FinBeat.Test.DomainObjects;
using Zvonarev.FinBeat.Test.Storage.Tools.Ef;
using Zvonarev.FinBeat.Test.Storage.Tools.Ef.Models;

namespace Zvonarev.FinBeat.Test.Storage.UseCases.FindDataEntries;

internal class FindDataEntriesQueryHandler : IRequestHandler<FindDataEntriesQuery, IReadOnlyCollection<OrderedDataEntry>>
{
    private readonly AppDbContext _db;
    private readonly ILogger<FindDataEntriesQueryHandler> _logger;

    public FindDataEntriesQueryHandler(AppDbContext db,
        ILogger<FindDataEntriesQueryHandler> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<IReadOnlyCollection<OrderedDataEntry>> Handle(FindDataEntriesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var predicate = request.Predicate ?? (x => true);
            var result = await _db.Set<DbDataEntry>()
                .Where(predicate)
                .AsNoTracking()
                .ToArrayAsync(cancellationToken);

            return result;
        }
        catch
        {
            _logger.LogError("Failed to get data entries");
            throw;
        }
    }
}