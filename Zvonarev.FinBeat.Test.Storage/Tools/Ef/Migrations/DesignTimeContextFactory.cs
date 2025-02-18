using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Zvonarev.FinBeat.Test.Storage.Tools.Ef.Migrations;

internal class DesignTimeContextFactory : IDesignTimeDbContextFactory<AppDbContext>,
    IDesignTimeDbContextFactory<LoggingDbContext>
{
    AppDbContext IDesignTimeDbContextFactory<AppDbContext>.CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseAppSqlServer("")
            .Options;

        return new AppDbContext(options);
    }

    LoggingDbContext IDesignTimeDbContextFactory<LoggingDbContext>.CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<LoggingDbContext>()
            .UseLoggingSqlServer("")
            .Options;

        return new LoggingDbContext(options);
    }
}