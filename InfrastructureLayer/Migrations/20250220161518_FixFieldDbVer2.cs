using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InfrastructureLayer.Migrations
{
    /// <inheritdoc />
    public partial class FixFieldDbVer2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GrowthRecords_Users_ChildrentId",
                table: "GrowthRecords");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "UserPackages");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "SharingProfiles");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "RatingFeedbacks");

            migrationBuilder.DropColumn(
                name: "FeedbackDate",
                table: "RatingFeedbacks");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "PackageFeatures");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "DoctorSpecialization");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "DoctorLicense");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "ConsultationResponses");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "ConsultationRequests");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Children");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "BmiCategories");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Alerts");

            migrationBuilder.RenameColumn(
                name: "Commnet",
                table: "RatingFeedbacks",
                newName: "Feedback");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Specializations",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "Specializations",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Roles",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "RatingFeedbacks",
                type: "text",
                nullable: false,
                defaultValue: "Publish");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Packages",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "Packages",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Packages",
                type: "text",
                nullable: false,
                defaultValue: "Pending");

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "GrowthRecords",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "GrowthRecords",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Features",
                type: "character varying(300)",
                maxLength: 300,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(300)",
                oldMaxLength: 300);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "Features",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Specialize",
                table: "DoctorLicense",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "ProfileImg",
                table: "DoctorLicense",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "Metadata",
                table: "DoctorLicense",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "Certificate",
                table: "DoctorLicense",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Biography",
                table: "DoctorLicense",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "ConsultationResponses",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Attachments",
                table: "ConsultationResponses",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "ConsultationRequests",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "Attachments",
                table: "ConsultationRequests",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500);

            migrationBuilder.AddColumn<Guid>(
                name: "DoctorReceiveId",
                table: "ConsultationRequests",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "ConsultationRequests",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "Children",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Children",
                type: "text",
                nullable: false,
                defaultValue: "Active");

            migrationBuilder.AlterColumn<string>(
                name: "Message",
                table: "Alerts",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500);

            migrationBuilder.CreateIndex(
                name: "IX_ConsultationRequests_DoctorReceiveId",
                table: "ConsultationRequests",
                column: "DoctorReceiveId");

            migrationBuilder.AddForeignKey(
                name: "FK_ConsultationRequests_Users_DoctorReceiveId",
                table: "ConsultationRequests",
                column: "DoctorReceiveId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GrowthRecords_Children_ChildrentId",
                table: "GrowthRecords",
                column: "ChildrentId",
                principalTable: "Children",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConsultationRequests_Users_DoctorReceiveId",
                table: "ConsultationRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_GrowthRecords_Children_ChildrentId",
                table: "GrowthRecords");

            migrationBuilder.DropIndex(
                name: "IX_ConsultationRequests_DoctorReceiveId",
                table: "ConsultationRequests");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "RatingFeedbacks");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Packages");

            migrationBuilder.DropColumn(
                name: "DoctorReceiveId",
                table: "ConsultationRequests");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "ConsultationRequests");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Children");

            migrationBuilder.RenameColumn(
                name: "Feedback",
                table: "RatingFeedbacks",
                newName: "Commnet");

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "Users",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "UserPackages",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "Transactions",
                type: "uuid",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Specializations",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "Specializations",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "SharingProfiles",
                type: "uuid",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Roles",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "Roles",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "RatingFeedbacks",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FeedbackDate",
                table: "RatingFeedbacks",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Packages",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "Packages",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "PackageFeatures",
                type: "uuid",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "GrowthRecords",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "GrowthRecords",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Features",
                type: "character varying(300)",
                maxLength: 300,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(300)",
                oldMaxLength: 300,
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "Features",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "DoctorSpecialization",
                type: "uuid",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Specialize",
                table: "DoctorLicense",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProfileImg",
                table: "DoctorLicense",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Metadata",
                table: "DoctorLicense",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Certificate",
                table: "DoctorLicense",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Biography",
                table: "DoctorLicense",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "DoctorLicense",
                type: "uuid",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "ConsultationResponses",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Attachments",
                table: "ConsultationResponses",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "ConsultationResponses",
                type: "uuid",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "ConsultationRequests",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Attachments",
                table: "ConsultationRequests",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "ConsultationRequests",
                type: "uuid",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "Children",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "Children",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "BmiCategories",
                type: "uuid",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Message",
                table: "Alerts",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "Alerts",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_GrowthRecords_Users_ChildrentId",
                table: "GrowthRecords",
                column: "ChildrentId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
