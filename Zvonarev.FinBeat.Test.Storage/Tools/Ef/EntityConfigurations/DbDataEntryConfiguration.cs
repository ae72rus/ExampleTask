using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zvonarev.FinBeat.Test.Storage.Tools.Ef.Models;

namespace Zvonarev.FinBeat.Test.Storage.Tools.Ef.EntityConfigurations;

internal class DbDataEntryConfiguration : IEntityTypeConfiguration<DbDataEntry>
{
    public const string TableName = "DataEntries";
    public const string Schema = "dbo";
    public void Configure(EntityTypeBuilder<DbDataEntry> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasIndex(x => x.OrderId)
            .IsUnique();
        builder.HasIndex(x => x.Code);

        builder.ToTable(TableName, Schema);
    }
}