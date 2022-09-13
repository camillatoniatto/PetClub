using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetClub.Infra.Migrations
{
    public partial class attCashFlow : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IdUsersPartners",
                table: "PurchaseOrder",
                newName: "IdUser");

            migrationBuilder.AddColumn<string>(
                name: "IdPartner",
                table: "PurchaseOrder",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "IdPet",
                table: "PurchaseOrder",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<DateTime>(
                name: "WriteOffDate",
                table: "CashFlow",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "CashFlow",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "isOutflow",
                table: "CashFlow",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdPartner",
                table: "PurchaseOrder");

            migrationBuilder.DropColumn(
                name: "IdPet",
                table: "PurchaseOrder");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "CashFlow");

            migrationBuilder.DropColumn(
                name: "isOutflow",
                table: "CashFlow");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "IdUser",
                table: "PurchaseOrder",
                newName: "IdUsersPartners");

            migrationBuilder.AlterColumn<DateTime>(
                name: "WriteOffDate",
                table: "CashFlow",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }
    }
}
