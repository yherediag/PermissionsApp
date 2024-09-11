using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PermissionsApp.Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class SetPermissionsType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "PermissionsType",
                columns: new[] { "PermissionTypeId", "Description" },
                values: new object[,]
                {
                    { 1, "Leader" },
                    { 2, "Analyst" },
                    { 3, "Developer" },
                    { 4, "Tester" },
                    { 99, "Admin" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PermissionsType",
                keyColumn: "PermissionTypeId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "PermissionsType",
                keyColumn: "PermissionTypeId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "PermissionsType",
                keyColumn: "PermissionTypeId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "PermissionsType",
                keyColumn: "PermissionTypeId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "PermissionsType",
                keyColumn: "PermissionTypeId",
                keyValue: 99);
        }
    }
}
