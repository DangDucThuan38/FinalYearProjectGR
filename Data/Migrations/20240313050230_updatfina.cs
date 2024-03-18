using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DangDucThuanFinalYear.Migrations
{
    /// <inheritdoc />
    public partial class updatfina : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Finances",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Finances");
        }
    }
}
