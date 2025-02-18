using MediatR;
using Zvonarev.FinBeat.Test.HttpDbLogging.Tools;

namespace Zvonarev.FinBeat.Test.HttpDbLogging.UseCases.AddHttpInfoLog;

internal class AddHttpInfoCommandHandler : IRequestHandler<AddHttpInfoCommand>
{
    private readonly LogsContainer _logsContainer;

    public AddHttpInfoCommandHandler(LogsContainer logsContainer)
    {
        _logsContainer = logsContainer;
    }

    public Task Handle(AddHttpInfoCommand request, CancellationToken cancellationToken)
    {
        _logsContainer.AddLog(request.Info);
        return Task.CompletedTask;
    }
}