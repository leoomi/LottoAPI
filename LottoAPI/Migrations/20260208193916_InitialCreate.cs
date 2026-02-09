using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LottoAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Lottos",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    LastResultId = table.Column<long>(type: "INTEGER", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lottos", x => x.Id);
                }
            );

            migrationBuilder.CreateTable(
                name: "Results",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    LottoId = table.Column<string>(type: "TEXT", nullable: false),
                    Numbers = table.Column<string>(type: "TEXT", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Results", x => new { x.Id, x.LottoId });
                    table.ForeignKey(
                        name: "FK_Results_Lottos_LottoId",
                        column: x => x.LottoId,
                        principalTable: "Lottos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.InsertData(
                table: "Lottos",
                columns: new[] { "Id", "LastResultId", "Name" },
                values: new object[,]
                {
                    { "lotofacil", null, "Loto Fácil" },
                    { "megasena", null, "Mega Sena" },
                    { "quina", null, "Quina" },
                }
            );

            migrationBuilder.CreateIndex(
                name: "IX_Results_LottoId",
                table: "Results",
                column: "LottoId"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "Results");

            migrationBuilder.DropTable(name: "Lottos");
        }
    }
}
