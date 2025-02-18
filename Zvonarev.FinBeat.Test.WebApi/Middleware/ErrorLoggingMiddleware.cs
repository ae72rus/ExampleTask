namespace Zvonarev.FinBeat.Test.WebApi.Middleware;

internal class ErrorLoggingMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext httpContext, ILogger<ErrorLoggingMiddleware> logger)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception e)
        {
            logger.LogCritical(e, "Request handling failed");
            throw;
        }
    }
}