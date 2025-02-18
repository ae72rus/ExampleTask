using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zvonarev.FinBeat.Test.Storage.Tools.Ef.Migrations.LoggingDbContextMigrations
{
    /// <inheritdoc />
    public partial class AddedHeadersLogging : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Headers",
                schema: "log",
                table: "Api",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Headers",
                schema: "log",
                table: "Api");
        }
    }
}
