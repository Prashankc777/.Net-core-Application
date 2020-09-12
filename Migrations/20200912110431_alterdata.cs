 using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication12.Migrations
{
    public partial class alterdata : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Emplopyess",
                keyColumn: "Id",
                keyValue: 3,
                column: "Name",
                value: "Prashan");

            migrationBuilder.InsertData(
                table: "Emplopyess",
                columns: new[] { "Id", "Department", "Email", "Name" },
                values: new object[] { 4, 1, "cde91@gmail.com", "Prashan" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Emplopyess",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.UpdateData(
                table: "Emplopyess",
                keyColumn: "Id",
                keyValue: 3,
                column: "Name",
                value: "hijk");
        }
    }
}
