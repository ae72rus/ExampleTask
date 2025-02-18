using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Zvonarev.FinBeat.Test.HttpDbLogging.Configuration;
using Zvonarev.FinBeat.Test.HttpDbLogging.Tools;
using Zvonarev.FinBeat.Test.Storage.Configuration;

namespace Zvonarev.FinBeat.Test.HttpDbLogging;

public static class DependencyInjection
{
    public static IServiceCollection AddHttpInfoLogging(this IServiceCollection services, HttpLoggingConfiguration configuration)
    {
        services
            .AddSingleton(configuration)
            .AddSingleton<LogsContainer>()
            .AddHostedService<LogSubmissionService>()
            ;

        return services;
    }

    public static IApplicationBuilder UseApiRequestsLogging(this IApplicationBuilder app)
    {
        app.UseMiddleware<HttpRequestInfoLoggingMiddleware>();
        return app;
    }
}