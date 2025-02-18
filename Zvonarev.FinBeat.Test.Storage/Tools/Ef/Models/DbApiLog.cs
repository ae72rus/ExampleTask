using Zvonarev.FinBeat.Test.DomainObjects;

namespace Zvonarev.FinBeat.Test.Storage.Tools.Ef.Models;

internal record DbApiLog(
        long Id,
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
    ) : HttpRequestInfo(ReferenceId, HttpId, InitiatorIp, Method, Url, Headers, Payload, ResponseCode, Response, ResponseTime);