using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DangDucThuanFinalYear.Migrations
{
    /// <inheritdoc />
    public partial class updateBooking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Boookings_Rooms_RoomId",
                table: "Boookings");

            migrationBuilder.AlterColumn<int>(
                name: "RoomId",
                table: "Boookings",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "Children",
                table: "Boookings",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "CheckOutDateTime",
                table: "Boookings",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "CheckInDateTime",
                table: "Boookings",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<int>(
                name: "BookingStatus",
                table: "Boookings",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "RoomTypeId",
                table: "Boookings",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<string>(
                name: "SpecialRequest",
                table: "Boookings",
                type: "varchar(250)",
                unicode: false,
                maxLength: 250,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Boookings_RoomTypeId",
                table: "Boookings",
                column: "RoomTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Boookings_RoomTypes_RoomTypeId",
                table: "Boookings",
                column: "RoomTypeId",
                principalTable: "RoomTypes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Boookings_Rooms_RoomId",
                table: "Boookings",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Boookings_RoomTypes_RoomTypeId",
                table: "Boookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Boookings_Rooms_RoomId",
                table: "Boookings");

            migrationBuilder.DropIndex(
                name: "IX_Boookings_RoomTypeId",
                table: "Boookings");

            migrationBuilder.DropColumn(
                name: "BookingStatus",
                table: "Boookings");

            migrationBuilder.DropColumn(
                name: "RoomTypeId",
                table: "Boookings");

            migrationBuilder.DropColumn(
                name: "SpecialRequest",
                table: "Boookings");

            migrationBuilder.AlterColumn<int>(
                name: "RoomId",
                table: "Boookings",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Children",
                table: "Boookings",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CheckOutDateTime",
                table: "Boookings",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CheckInDateTime",
                table: "Boookings",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AddForeignKey(
                name: "FK_Boookings_Rooms_RoomId",
                table: "Boookings",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
