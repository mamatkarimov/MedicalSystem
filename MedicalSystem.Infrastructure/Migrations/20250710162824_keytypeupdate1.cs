using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicalSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class keytypeupdate1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LabOrderDetails_Users_PerformedByID",
                table: "LabOrderDetails");

            migrationBuilder.AddForeignKey(
                name: "FK_LabOrderDetails_Users_PerformedByID",
                table: "LabOrderDetails",
                column: "PerformedByID",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LabOrderDetails_Users_PerformedByID",
                table: "LabOrderDetails");

            migrationBuilder.AddForeignKey(
                name: "FK_LabOrderDetails_Users_PerformedByID",
                table: "LabOrderDetails",
                column: "PerformedByID",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
