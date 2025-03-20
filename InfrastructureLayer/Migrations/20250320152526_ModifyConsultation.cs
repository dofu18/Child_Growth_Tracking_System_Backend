using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InfrastructureLayer.Migrations
{
    /// <inheritdoc />
    public partial class ModifyConsultation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConsultationRequests_Children_ChildrentId",
                table: "ConsultationRequests");

            migrationBuilder.DropIndex(
                name: "IX_ConsultationRequests_ChildrentId",
                table: "ConsultationRequests");

            migrationBuilder.DropColumn(
                name: "ChildrentId",
                table: "ConsultationRequests");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ChildrentId",
                table: "ConsultationRequests",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ConsultationRequests_ChildrentId",
                table: "ConsultationRequests",
                column: "ChildrentId");

            migrationBuilder.AddForeignKey(
                name: "FK_ConsultationRequests_Children_ChildrentId",
                table: "ConsultationRequests",
                column: "ChildrentId",
                principalTable: "Children",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
