using MediatR;
using Zvonarev.FinBeat.Test.DomainObjects;

namespace Zvonarev.FinBeat.Test.Storage.UseCases.SaveDataEntries;

public record SaveDataEntriesCommand(IEnumerable<DataEntry> Entries) : IRequest;