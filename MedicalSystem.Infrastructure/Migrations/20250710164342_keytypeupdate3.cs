using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicalSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class keytypeupdate3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LabOrderDetails_LabOrders_OrderID",
                table: "LabOrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_LabOrderDetails_LabTestTypes_TestTypeID",
                table: "LabOrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_LabOrderDetails_Users_PerformedByID",
                table: "LabOrderDetails");

            migrationBuilder.AddForeignKey(
                name: "FK_LabOrderDetails_LabOrders_OrderID",
                table: "LabOrderDetails",
                column: "OrderID",
                principalTable: "LabOrders",
                principalColumn: "OrderID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LabOrderDetails_LabTestTypes_TestTypeID",
                table: "LabOrderDetails",
                column: "TestTypeID",
                principalTable: "LabTestTypes",
                principalColumn: "TestTypeID");

            migrationBuilder.AddForeignKey(
                name: "FK_LabOrderDetails_Users_PerformedByID",
                table: "LabOrderDetails",
                column: "PerformedByID",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LabOrderDetails_LabOrders_OrderID",
                table: "LabOrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_LabOrderDetails_LabTestTypes_TestTypeID",
                table: "LabOrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_LabOrderDetails_Users_PerformedByID",
                table: "LabOrderDetails");

            migrationBuilder.AddForeignKey(
                name: "FK_LabOrderDetails_LabOrders_OrderID",
                table: "LabOrderDetails",
                column: "OrderID",
                principalTable: "LabOrders",
                principalColumn: "OrderID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LabOrderDetails_LabTestTypes_TestTypeID",
                table: "LabOrderDetails",
                column: "TestTypeID",
                principalTable: "LabTestTypes",
                principalColumn: "TestTypeID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LabOrderDetails_Users_PerformedByID",
                table: "LabOrderDetails",
                column: "PerformedByID",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
