using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MedicalSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UserTableOptimized : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("07b88d68-d966-408d-bea5-f16ff0ae80e0"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("1a1d1760-8f1d-41c5-aad0-210e8e8a8038"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("3bdd09d5-a1cd-42f7-8b2e-346c3680b9e2"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("4e264931-d794-4eee-9886-9c6ff9992a37"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("73672637-2b13-4157-bab4-9a469ccfb877"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("d487a27c-f2fd-4cb2-a2ec-6b05b961e8b7"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("df06bd00-00a9-483c-8fa1-838f03349213"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("f1d9274d-df5a-4ab1-9642-e91958febf9a"));

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "Users");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "Users",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("07b88d68-d966-408d-bea5-f16ff0ae80e0"), "User" },
                    { new Guid("1a1d1760-8f1d-41c5-aad0-210e8e8a8038"), "Reception" },
                    { new Guid("3bdd09d5-a1cd-42f7-8b2e-346c3680b9e2"), "ChefDoctor" },
                    { new Guid("4e264931-d794-4eee-9886-9c6ff9992a37"), "Doctor" },
                    { new Guid("73672637-2b13-4157-bab4-9a469ccfb877"), "Laboratory" },
                    { new Guid("d487a27c-f2fd-4cb2-a2ec-6b05b961e8b7"), "Cashier" },
                    { new Guid("df06bd00-00a9-483c-8fa1-838f03349213"), "Admin" },
                    { new Guid("f1d9274d-df5a-4ab1-9642-e91958febf9a"), "Nurse" }
                });
        }
    }
}
