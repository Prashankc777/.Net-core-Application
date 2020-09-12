using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication12.Migrations
{
    public partial class somecommand : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "someProperty",
                table: "Emplopyess",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "someProperty",
                table: "Emplopyess");
        }
    }
}
