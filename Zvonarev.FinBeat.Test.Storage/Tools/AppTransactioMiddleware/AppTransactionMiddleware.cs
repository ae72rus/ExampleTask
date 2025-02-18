using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Zvonarev.FinBeat.Test.Storage.Tools.Ef;

namespace Zvonarev.FinBeat.Test.Storage.Tools.AppTransactioMiddleware;

internal class AppTransactionMiddleware
{
    private readonly RequestDelegate _next;

    public AppTransactionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext httpContext,
        AppDbContext db,
        ILogger<AppTransactionMiddleware> logger)
    {
        await using var transaction = await db.Database.BeginTransactionAsync();
        try
        {
            await _next.Invoke(httpContext);

            try
            {
                await transaction.CommitAsync();
            }
            catch (Exception e)
            {
                logger.LogCritical(e, "Failed to commit request transaction");
                throw;
            }
        }
        catch (Exception e)
        {
            logger.LogCritical(e, "Request failed, not DB transaction has been commited.");
            throw;
        }
    }
}