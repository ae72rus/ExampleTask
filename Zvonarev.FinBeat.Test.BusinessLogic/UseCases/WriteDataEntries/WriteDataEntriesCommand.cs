using MediatR;
using Zvonarev.FinBeat.Test.DomainObjects;
using Zvonarev.FinBeat.Test.Storage.UseCases.ClearDataEntriesTable;
using Zvonarev.FinBeat.Test.Storage.UseCases.SaveDataEntries;

namespace Zvonarev.FinBeat.Test.BusinessLogic.UseCases.WriteDataEntries;

public record WriteDataEntriesCommand(IReadOnlyCollection<DataEntry> Entries) : IRequest;

internal class WriteDataEntriesCommandHandler : IRequestHandler<WriteDataEntriesCommand>
{
    private readonly IMediator _mediator;

    public WriteDataEntriesCommandHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Handle(WriteDataEntriesCommand request, CancellationToken cancellationToken)
    {
        await _mediator.Send(new ClearDataEntriesCommand(), cancellationToken);
        await _mediator.Send(new SaveDataEntriesCommand(
            request
                .Entries
                .OrderBy(x => x.Code)
        ), cancellationToken);
    }
}