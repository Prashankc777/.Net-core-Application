using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication12.Migrations
{
    public partial class seedEmployee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Emplopyess",
                columns: new[] { "Id", "Department", "Email", "Name" },
                values: new object[] { 1, 1, "prkprime91@gmail.com", "abc" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Emplopyess",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
