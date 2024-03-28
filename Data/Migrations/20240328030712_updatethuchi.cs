using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DangDucThuanFinalYear.Migrations
{
    /// <inheritdoc />
    public partial class updatethuchi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TypeFinance",
                table: "Finances");

            migrationBuilder.AddColumn<string>(
                name: "NameFinance",
                table: "Finances",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NameFinance",
                table: "Finances");

            migrationBuilder.AddColumn<int>(
                name: "TypeFinance",
                table: "Finances",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
