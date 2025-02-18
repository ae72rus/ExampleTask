using Microsoft.EntityFrameworkCore;
using Zvonarev.FinBeat.Test.Storage.Tools.Ef.EntityConfigurations;

namespace Zvonarev.FinBeat.Test.Storage.Tools.Ef;

internal class AppDbContext : DbContext
{
    public const string MigrationsHistoryTableName = "__Migrations";
    public const string MigrationsHistorySchema = "app";
    public AppDbContext(DbContextOptions<AppDbContext> dbContextOptions)
        : base(dbContextOptions)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new DbDataEntryConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}