using MediatR;
using Zvonarev.FinBeat.Test.DomainObjects;

namespace Zvonarev.FinBeat.Test.HttpDbLogging.UseCases.AddHttpInfoLog;

public record AddHttpInfoCommand(HttpRequestInfo Info) : IRequest;