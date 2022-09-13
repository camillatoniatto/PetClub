using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetClub.Infra.Migrations
{
    public partial class attScheduler : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IdPartner",
                table: "Scheduler",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdPartner",
                table: "Scheduler");
        }
    }
}
