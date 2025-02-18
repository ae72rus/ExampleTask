namespace Zvonarev.FinBeat.Test.DomainObjects;

public record HttpRequestInfo(
    string ReferenceId,
    string HttpId,
    string InitiatorIp,
    string Method,
    string Url,
    string Headers,
    string? Payload,
    int ResponseCode,
    string? Response,
    TimeSpan ResponseTime
);