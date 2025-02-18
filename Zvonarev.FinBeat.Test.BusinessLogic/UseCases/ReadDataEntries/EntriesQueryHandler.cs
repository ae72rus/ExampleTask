using System.Linq.Expressions;
using MediatR;
using Microsoft.Extensions.Logging;
using Zvonarev.FinBeat.Test.BusinessLogic.UseCases.ReadDataEntries.Query.Internals;
using Zvonarev.FinBeat.Test.DomainObjects;
using Zvonarev.FinBeat.Test.Storage.UseCases.FindDataEntries;

namespace Zvonarev.FinBeat.Test.BusinessLogic.UseCases.ReadDataEntries;

internal class EntriesQueryHandler : IRequestHandler<EntriesQuery, IReadOnlyCollection<OrderedDataEntry>>
{
    private static readonly Expression<Func<OrderedDataEntry, bool>> NegativePredicate = x => false;
    private readonly IMediator _mediator;
    private readonly ILogger<EntriesQueryHandler> _logger;

    public EntriesQueryHandler(IMediator mediator,
        ILogger<EntriesQueryHandler> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task<IReadOnlyCollection<OrderedDataEntry>> Handle(EntriesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var predicate = GetFilterPredicate(request);
            if (predicate == NegativePredicate)
                return [];

            var storageQuery = new FindDataEntriesQuery(predicate);
            var entries = await _mediator.Send(storageQuery, cancellationToken);
            return entries;
        }
        catch
        {
            _logger.LogError("Unable to get data entries. Filters: {filters}", request.Filters);
            throw;
        }
    }

    private Expression<Func<OrderedDataEntry, bool>> GetFilterPredicate(EntriesQuery request)
    {
        var filters = request.Filters;
        if (!filters.Any())
            return x => true;//would allow all entries

        if (filters.All(
                x => !x.AllowedValues.Any()
                     && !x.ForbiddenValues.Any())
           )
            return NegativePredicate;//would allow no entries

        return filters.ToPredicate();
    }
}