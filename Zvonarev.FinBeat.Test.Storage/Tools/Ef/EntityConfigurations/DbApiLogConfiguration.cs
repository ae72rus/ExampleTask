using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zvonarev.FinBeat.Test.Storage.Tools.Ef.Models;

namespace Zvonarev.FinBeat.Test.Storage.Tools.Ef.EntityConfigurations;

internal class DbApiLogConfiguration : IEntityTypeConfiguration<DbApiLog>
{
    public const string TableName = "Api";
    public const string Schema = "log";
    public void Configure(EntityTypeBuilder<DbApiLog> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasIndex(x => x.ReferenceId)
            .IsUnique();
        builder.HasIndex(x => x.HttpId)
            .IsUnique();
        builder.HasIndex(x => x.InitiatorIp);
        builder.HasIndex(x => new { x.Method, x.Url });

        builder.ToTable(TableName, Schema);
    }
}