using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Zvonarev.FinBeat.Test.Storage.Configuration;
using Zvonarev.FinBeat.Test.Storage.Tools.AppTransactioMiddleware;
using Zvonarev.FinBeat.Test.Storage.Tools.Ef;

namespace Zvonarev.FinBeat.Test.Storage;

public static class DependencyInjection
{
    public static IServiceCollection AddStorage(this IServiceCollection services, StorageConfiguration configuration)
    {
        services.AddSingleton(configuration);

        #region Data storage
        services.AddDbContextPool<AppDbContext>(optionsBuilder =>
            optionsBuilder.UseAppSqlServer(configuration.ConnectionString)
        );
        #endregion

        #region Log storage
        services.AddDbContextPool<LoggingDbContext>(optionsBuilder =>
            optionsBuilder.UseLoggingSqlServer(configuration.ConnectionString)
        );
        #endregion

        return services;
    }

    public static IApplicationBuilder UsePerRequestTransactions(this IApplicationBuilder app)
    {
        app.UseMiddleware<AppTransactionMiddleware>();
        return app;
    }
}