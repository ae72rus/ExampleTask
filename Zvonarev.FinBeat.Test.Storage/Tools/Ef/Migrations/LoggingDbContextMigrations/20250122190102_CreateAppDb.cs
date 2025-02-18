#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace Zvonarev.FinBeat.Test.Storage.Tools.Ef.Migrations.LoggingDbContextMigrations
{
    /// <inheritdoc />
    public partial class CreateAppDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "log");

            migrationBuilder.CreateTable(
                name: "Api",
                schema: "log",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReferenceId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    HttpId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    InitiatorIp = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Method = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Payload = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseCode = table.Column<int>(type: "int", nullable: false),
                    Response = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseTime = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Api", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Api_HttpId",
                schema: "log",
                table: "Api",
                column: "HttpId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Api_InitiatorIp",
                schema: "log",
                table: "Api",
                column: "InitiatorIp");

            migrationBuilder.CreateIndex(
                name: "IX_Api_Method_Url",
                schema: "log",
                table: "Api",
                columns: new[] { "Method", "Url" });

            migrationBuilder.CreateIndex(
                name: "IX_Api_ReferenceId",
                schema: "log",
                table: "Api",
                column: "ReferenceId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Api",
                schema: "log");
        }
    }
}
