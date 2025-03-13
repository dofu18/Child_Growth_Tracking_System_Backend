using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InfrastructureLayer.Migrations
{
    /// <inheritdoc />
    public partial class DeleteSpec : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DoctorSpecialization");

            migrationBuilder.DropTable(
                name: "Specializations");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Specializations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    Description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Specializations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Specializations_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DoctorSpecialization",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DoctorLicenseId = table.Column<Guid>(type: "uuid", nullable: false),
                    SpecializtionId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoctorSpecialization", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DoctorSpecialization_DoctorLicense_DoctorLicenseId",
                        column: x => x.DoctorLicenseId,
                        principalTable: "DoctorLicense",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DoctorSpecialization_Specializations_SpecializtionId",
                        column: x => x.SpecializtionId,
                        principalTable: "Specializations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DoctorSpecialization_DoctorLicenseId",
                table: "DoctorSpecialization",
                column: "DoctorLicenseId");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorSpecialization_SpecializtionId",
                table: "DoctorSpecialization",
                column: "SpecializtionId");

            migrationBuilder.CreateIndex(
                name: "IX_Specializations_CreatedBy",
                table: "Specializations",
                column: "CreatedBy");
        }
    }
}
