using Microsoft.EntityFrameworkCore;

namespace Zvonarev.FinBeat.Test.Storage.Tools.Ef;

internal static class DbContextOptionsExtensions
{
    public static TBuilder UseAppSqlServer<TBuilder>(this TBuilder builder, string connectionString)
        where TBuilder : DbContextOptionsBuilder
    {
        builder.UseSqlServer(connectionString,
            x =>
            {
                x.MigrationsHistoryTable(AppDbContext.MigrationsHistoryTableName,
                    AppDbContext.MigrationsHistorySchema);
            });
        return builder;
    }
    public static TBuilder UseLoggingSqlServer<TBuilder>(this TBuilder builder, string connectionString)
        where TBuilder : DbContextOptionsBuilder
    {
        builder.UseSqlServer(connectionString,
            x =>
            {
                x.MigrationsHistoryTable(LoggingDbContext.MigrationsHistoryTableName,
                    LoggingDbContext.MigrationsHistorySchema);
            });
        return builder;
    }
}