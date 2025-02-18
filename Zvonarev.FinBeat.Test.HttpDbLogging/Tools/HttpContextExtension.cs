using System.Text;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Zvonarev.FinBeat.Test.DomainObjects;
using Zvonarev.FinBeat.Test.HttpDbLogging.UseCases.AddHttpInfoLog;

namespace Zvonarev.FinBeat.Test.HttpDbLogging.Tools;

internal static class HttpContextExtension
{
    //todo make config setting
    private const int _maxSize = 4096 * 4;//16KB

    public static async  Task SaveRequestInfo(this HttpContext httpContext, IMediator mediator, ILogger logger,
        string requestId, TimeSpan requestProcessingTime, bool isErrorOccurred)
    {
        try
        {
            var info = await GetRequestInfo(httpContext, requestId, requestProcessingTime, isErrorOccurred);
            await mediator.Send(new AddHttpInfoCommand(info));
        }
        catch (Exception e)
        {
            logger.LogCritical(e, "Failed to save http request info");
        }
    }

    private static async Task<HttpRequestInfo> GetRequestInfo(this HttpContext context, string requestId, TimeSpan requestProcessingTime, bool isErrorOccurred)
    {
        var headersLineBuilder = new StringBuilder();
        foreach (var header in context.Request.Headers)
        foreach (var value in header.Value)
            headersLineBuilder.Append($"{header.Key}: {value}{Environment.NewLine}");

        var info = new HttpRequestInfo(
            requestId,
            context.TraceIdentifier,
            context.Connection.RemoteIpAddress?.ToString() ?? "<unknown>",
            context.Request.Method,
            $"{context.Request.Path}{context.Request.QueryString.ToUriComponent()}",
            headersLineBuilder.ToString(),
            await GetRequestBody(context),
            isErrorOccurred
                ? 500 
                : context.Response.StatusCode,
            await GetResponsePayload(context),
            requestProcessingTime
        );

        return info;
    }

    private static async Task<string?> GetRequestBody(this HttpContext context)
    {
        var requestStream = (MemoryStream)context.Request.Body;
        if (context.Request.ContentLength == 0)
            return null;

        return await ReadFromStream(requestStream);
    }

    private static async Task<string?> GetResponsePayload(this HttpContext context)
    {
        var responseStream = (MemoryStream)context.Response.Body;

        if (responseStream.Length == 0)
            return null;

        return await ReadFromStream(responseStream);
    }
    private static async Task<string?> ReadFromStream(this MemoryStream stream)
    {
        await stream.FlushAsync();
        stream.Seek(0, SeekOrigin.Begin);

        var bytes = stream.ToArray();

        var sb = new StringBuilder(Encoding.UTF8.GetString(bytes, 0, bytes.Length > _maxSize ? _maxSize : bytes.Length));

        if (stream.Length > _maxSize)
            sb.Append("...<content is too long>");

        return sb.ToString();
    }
}