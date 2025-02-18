using Microsoft.EntityFrameworkCore;
using Zvonarev.FinBeat.Test.Storage.Tools.Ef.EntityConfigurations;

namespace Zvonarev.FinBeat.Test.Storage.Tools.Ef;

internal class LoggingDbContext : DbContext
{
    public const string MigrationsHistoryTableName = "__Migrations";
    public const string MigrationsHistorySchema = "logs";
    public LoggingDbContext(DbContextOptions<LoggingDbContext> dbContextOptions)
        : base(dbContextOptions)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new DbApiLogConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}