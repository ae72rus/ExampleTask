using System.Linq.Expressions;
using MediatR;
using Zvonarev.FinBeat.Test.DomainObjects;

namespace Zvonarev.FinBeat.Test.Storage.UseCases.FindDataEntries;

public record FindDataEntriesQuery(Expression<Func<OrderedDataEntry, bool>>? Predicate = null) : IRequest<IReadOnlyCollection<OrderedDataEntry>>;