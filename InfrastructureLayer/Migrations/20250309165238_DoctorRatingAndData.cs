using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace InfrastructureLayer.Migrations
{
    /// <inheritdoc />
    public partial class DoctorRatingAndData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Users",
                type: "text",
                nullable: true,
                defaultValue: "NotVerified",
                oldClrType: typeof(string),
                oldType: "text",
                oldDefaultValue: "NotVerified");

            migrationBuilder.AlterColumn<string>(
                name: "AuthType",
                table: "Users",
                type: "text",
                nullable: true,
                defaultValue: "Email",
                oldClrType: typeof(string),
                oldType: "text",
                oldDefaultValue: "Email");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "UserPackages",
                type: "text",
                nullable: true,
                defaultValue: "OnGoing",
                oldClrType: typeof(string),
                oldType: "text",
                oldDefaultValue: "OnGoing");

            migrationBuilder.AlterColumn<string>(
                name: "PaymentStatus",
                table: "Transactions",
                type: "text",
                nullable: true,
                defaultValue: "Pending",
                oldClrType: typeof(string),
                oldType: "text",
                oldDefaultValue: "Pending");

            migrationBuilder.AddColumn<Guid>(
                name: "DoctorId",
                table: "SharingProfiles",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Roles",
                type: "text",
                nullable: true,
                defaultValue: "Pending",
                oldClrType: typeof(string),
                oldType: "text",
                oldDefaultValue: "Pending");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "RatingFeedbacks",
                type: "text",
                nullable: true,
                defaultValue: "Publish",
                oldClrType: typeof(string),
                oldType: "text",
                oldDefaultValue: "Publish");

            migrationBuilder.AddColumn<Guid>(
                name: "DoctorId",
                table: "RatingFeedbacks",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RatingType",
                table: "RatingFeedbacks",
                type: "text",
                nullable: true,
                defaultValue: "Doctor");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Packages",
                type: "text",
                nullable: true,
                defaultValue: "Pending",
                oldClrType: typeof(string),
                oldType: "text",
                oldDefaultValue: "Pending");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "DoctorLicense",
                type: "text",
                nullable: true,
                defaultValue: "Pending",
                oldClrType: typeof(string),
                oldType: "text",
                oldDefaultValue: "Pending");

            migrationBuilder.AddColumn<string>(
                name: "Degrees",
                table: "DoctorLicense",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Languages",
                table: "DoctorLicense",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RatingAvg",
                table: "DoctorLicense",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Research",
                table: "DoctorLicense",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "ConsultationRequests",
                type: "text",
                nullable: true,
                defaultValue: "Pending",
                oldClrType: typeof(string),
                oldType: "text",
                oldDefaultValue: "Pending");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Children",
                type: "text",
                nullable: true,
                defaultValue: "Active",
                oldClrType: typeof(string),
                oldType: "text",
                oldDefaultValue: "Active");

            migrationBuilder.AlterColumn<string>(
                name: "GroupAge",
                table: "Children",
                type: "text",
                nullable: true,
                defaultValue: "From2to19",
                oldClrType: typeof(string),
                oldType: "text",
                oldDefaultValue: "From2to19");

            migrationBuilder.AlterColumn<string>(
                name: "Gender",
                table: "Children",
                type: "text",
                nullable: true,
                defaultValue: "Male",
                oldClrType: typeof(string),
                oldType: "text",
                oldDefaultValue: "Male");

            migrationBuilder.InsertData(
                table: "BmiCategories",
                columns: new[] { "Id", "BmiBottom", "BmiTop", "CreatedAt", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("01955c45-f781-7835-8d4b-aff20764aca6"), 0m, 5m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Under Weight", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("01955c46-2df8-74fd-bbb1-c8c1d792ee5b"), 5m, 85m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Healthy Weight", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("01955c46-5e27-7c17-97ee-a06ded033c32"), 95m, 85m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Over Weight", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("01955c46-81c0-7e96-9961-3b490a2d1c8f"), 120m, 95m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Obesity", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("01955c46-a1d5-7fe9-a779-da396903a7a4"), 140m, 120m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Servere Obesity", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreatedAt", "Description", "RoleName", "Status", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000001"), null, "This person have fully permission of system", "Admin", "Active", null },
                    { new Guid("00000000-0000-0000-0000-000000000002"), null, "This person have under permission of admin", "Staff", "Active", null },
                    { new Guid("00000000-0000-0000-0000-000000000003"), null, "This person have permission to response user", "Doctor", "Active", null },
                    { new Guid("00000000-0000-0000-0000-000000000004"), null, "This person have limited permission of system", "User", "Active", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_RatingFeedbacks_DoctorId",
                table: "RatingFeedbacks",
                column: "DoctorId");

            migrationBuilder.AddForeignKey(
                name: "FK_RatingFeedbacks_DoctorLicense_DoctorId",
                table: "RatingFeedbacks",
                column: "DoctorId",
                principalTable: "DoctorLicense",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RatingFeedbacks_DoctorLicense_DoctorId",
                table: "RatingFeedbacks");

            migrationBuilder.DropIndex(
                name: "IX_RatingFeedbacks_DoctorId",
                table: "RatingFeedbacks");

            migrationBuilder.DeleteData(
                table: "BmiCategories",
                keyColumn: "Id",
                keyValue: new Guid("01955c45-f781-7835-8d4b-aff20764aca6"));

            migrationBuilder.DeleteData(
                table: "BmiCategories",
                keyColumn: "Id",
                keyValue: new Guid("01955c46-2df8-74fd-bbb1-c8c1d792ee5b"));

            migrationBuilder.DeleteData(
                table: "BmiCategories",
                keyColumn: "Id",
                keyValue: new Guid("01955c46-5e27-7c17-97ee-a06ded033c32"));

            migrationBuilder.DeleteData(
                table: "BmiCategories",
                keyColumn: "Id",
                keyValue: new Guid("01955c46-81c0-7e96-9961-3b490a2d1c8f"));

            migrationBuilder.DeleteData(
                table: "BmiCategories",
                keyColumn: "Id",
                keyValue: new Guid("01955c46-a1d5-7fe9-a779-da396903a7a4"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000004"));

            migrationBuilder.DropColumn(
                name: "DoctorId",
                table: "SharingProfiles");

            migrationBuilder.DropColumn(
                name: "DoctorId",
                table: "RatingFeedbacks");

            migrationBuilder.DropColumn(
                name: "RatingType",
                table: "RatingFeedbacks");

            migrationBuilder.DropColumn(
                name: "Degrees",
                table: "DoctorLicense");

            migrationBuilder.DropColumn(
                name: "Languages",
                table: "DoctorLicense");

            migrationBuilder.DropColumn(
                name: "RatingAvg",
                table: "DoctorLicense");

            migrationBuilder.DropColumn(
                name: "Research",
                table: "DoctorLicense");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "NotVerified",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldDefaultValue: "NotVerified");

            migrationBuilder.AlterColumn<string>(
                name: "AuthType",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "Email",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldDefaultValue: "Email");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "UserPackages",
                type: "text",
                nullable: false,
                defaultValue: "OnGoing",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldDefaultValue: "OnGoing");

            migrationBuilder.AlterColumn<string>(
                name: "PaymentStatus",
                table: "Transactions",
                type: "text",
                nullable: false,
                defaultValue: "Pending",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldDefaultValue: "Pending");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Roles",
                type: "text",
                nullable: false,
                defaultValue: "Pending",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldDefaultValue: "Pending");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "RatingFeedbacks",
                type: "text",
                nullable: false,
                defaultValue: "Publish",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldDefaultValue: "Publish");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Packages",
                type: "text",
                nullable: false,
                defaultValue: "Pending",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldDefaultValue: "Pending");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "DoctorLicense",
                type: "text",
                nullable: false,
                defaultValue: "Pending",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldDefaultValue: "Pending");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "ConsultationRequests",
                type: "text",
                nullable: false,
                defaultValue: "Pending",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldDefaultValue: "Pending");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Children",
                type: "text",
                nullable: false,
                defaultValue: "Active",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldDefaultValue: "Active");

            migrationBuilder.AlterColumn<string>(
                name: "GroupAge",
                table: "Children",
                type: "text",
                nullable: false,
                defaultValue: "From2to19",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldDefaultValue: "From2to19");

            migrationBuilder.AlterColumn<string>(
                name: "Gender",
                table: "Children",
                type: "text",
                nullable: false,
                defaultValue: "Male",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldDefaultValue: "Male");
        }
    }
}
