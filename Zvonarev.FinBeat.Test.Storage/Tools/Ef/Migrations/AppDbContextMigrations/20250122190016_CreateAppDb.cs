#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace Zvonarev.FinBeat.Test.Storage.Tools.Ef.Migrations.AppDbContextMigrations
{
    /// <inheritdoc />
    public partial class CreateAppDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "DataEntries",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataEntries", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DataEntries_Code",
                schema: "dbo",
                table: "DataEntries",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_DataEntries_OrderId",
                schema: "dbo",
                table: "DataEntries",
                column: "OrderId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DataEntries",
                schema: "dbo");
        }
    }
}
