using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication12.Migrations
{
    public partial class altersedd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Emplopyess",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Email", "Name" },
                values: new object[] { "cde91@gmail.com", "cde" });

            migrationBuilder.InsertData(
                table: "Emplopyess",
                columns: new[] { "Id", "Department", "Email", "Name" },
                values: new object[] { 2, 1, "cde91@gmail.com", "fgh" });

            migrationBuilder.InsertData(
                table: "Emplopyess",
                columns: new[] { "Id", "Department", "Email", "Name" },
                values: new object[] { 3, 1, "cde91@gmail.com", "hijk" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Emplopyess",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Emplopyess",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.UpdateData(
                table: "Emplopyess",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Email", "Name" },
                values: new object[] { "prkprime91@gmail.com", "abc" });
        }
    }
}
