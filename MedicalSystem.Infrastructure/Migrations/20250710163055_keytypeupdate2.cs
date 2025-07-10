﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicalSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class keytypeupdate2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LabOrderDetails_LabTestTypes_TestTypeID",
                table: "LabOrderDetails");

            migrationBuilder.AddForeignKey(
                name: "FK_LabOrderDetails_LabTestTypes_TestTypeID",
                table: "LabOrderDetails",
                column: "TestTypeID",
                principalTable: "LabTestTypes",
                principalColumn: "TestTypeID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LabOrderDetails_LabTestTypes_TestTypeID",
                table: "LabOrderDetails");

            migrationBuilder.AddForeignKey(
                name: "FK_LabOrderDetails_LabTestTypes_TestTypeID",
                table: "LabOrderDetails",
                column: "TestTypeID",
                principalTable: "LabTestTypes",
                principalColumn: "TestTypeID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
