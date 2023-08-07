using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestB1_Task2_.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FileInfos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileInfos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FileRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OpeningBalanceAsset = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OpeningBalanceLiability = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DebitTurnover = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreditTurnover = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ClosingBalanceAsset = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ClosingBalanceLiability = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FileInfoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FileRecords_FileInfos_FileInfoId",
                        column: x => x.FileInfoId,
                        principalTable: "FileInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FileRecords_FileInfoId",
                table: "FileRecords",
                column: "FileInfoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FileRecords");

            migrationBuilder.DropTable(
                name: "FileInfos");
        }
    }
}
