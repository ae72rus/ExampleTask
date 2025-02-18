using System.Diagnostics;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Zvonarev.FinBeat.Test.HttpDbLogging.Tools;

internal class HttpRequestInfoLoggingMiddleware
{
    private readonly RequestDelegate _next;

    public HttpRequestInfoLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext httpContext, IMediator mediator, ILogger<HttpRequestInfoLoggingMiddleware> logger)
    {
        var requestId = Guid.NewGuid().ToString();
        var sw = new Stopwatch();
        var originalResponseStream = httpContext.Response.Body;
        var originalRequestStream = httpContext.Request.Body;
        await using var mockResponseStream = new MemoryStream();
        await using var mockRequestStream = new MemoryStream();

        var requestLength = httpContext.Request.ContentLength ?? 0L;
        if (requestLength > 0)
            await originalRequestStream.CopyToAsync(mockRequestStream, (int)requestLength);

        mockRequestStream.Seek(0, SeekOrigin.Begin);

        httpContext.Response.Body = mockResponseStream;
        httpContext.Request.Body = mockRequestStream;
        var isErrorOccurred = false;
        try
        {
            using var loggerScope = logger.BeginScope(new
            {
                RequestId = requestId,
                HttpRequestId = httpContext.TraceIdentifier
            });
            sw.Start();
            await _next(httpContext);
        }
        catch
        {
            isErrorOccurred = true;
            throw;
        }
        finally
        {
            sw.Stop();
            await httpContext.SaveRequestInfo(mediator, logger, requestId, sw.Elapsed, isErrorOccurred);
            mockResponseStream.Seek(0, SeekOrigin.Begin);
            await mockResponseStream.CopyToAsync(originalResponseStream);
        }
    }
}