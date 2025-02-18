using MediatR;
using Zvonarev.FinBeat.Test.DomainObjects;

namespace Zvonarev.FinBeat.Test.Storage.UseCases.SaveApiLog;

public record SaveApiLogsCommand(IReadOnlyCollection<HttpRequestInfo> Logs) : IRequest;