using System.Linq.Expressions;
using MediatR;
using Zvonarev.FinBeat.Test.BusinessLogic.UseCases.ReadDataEntries.Query;
using Zvonarev.FinBeat.Test.DomainObjects;

namespace Zvonarev.FinBeat.Test.BusinessLogic.UseCases.ReadDataEntries;

public record EntriesQuery : IRequest<IReadOnlyCollection<OrderedDataEntry>>
{
    public IReadOnlyCollection<EntriesBaseFilter> Filters { get; init; }

    public EntriesQuery(IReadOnlyCollection<EntriesBaseFilter> Filters)
    {
        this.Filters = Filters;
    }
}