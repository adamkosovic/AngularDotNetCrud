using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestPraktik.Migrations
{
    /// <inheritdoc />
    public partial class FixQuotesTableStructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quotes_AspNetUsers_UserId",
                table: "Quotes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Quotes",
                table: "Quotes");

            migrationBuilder.RenameTable(
                name: "Quotes",
                newName: "quotes");

            migrationBuilder.RenameIndex(
                name: "IX_Quotes_UserId",
                table: "quotes",
                newName: "IX_quotes_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_quotes",
                table: "quotes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_quotes_AspNetUsers_UserId",
                table: "quotes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_quotes_AspNetUsers_UserId",
                table: "quotes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_quotes",
                table: "quotes");

            migrationBuilder.RenameTable(
                name: "quotes",
                newName: "Quotes");

            migrationBuilder.RenameIndex(
                name: "IX_quotes_UserId",
                table: "Quotes",
                newName: "IX_Quotes_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Quotes",
                table: "Quotes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Quotes_AspNetUsers_UserId",
                table: "Quotes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
