using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace InfrastructureLayer.Migrations
{
    /// <inheritdoc />
    public partial class ModifyCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "BmiCategories",
                keyColumn: "Id",
                keyValue: new Guid("01955c45-f781-7835-8d4b-aff20764aca6"));

            migrationBuilder.AddColumn<int>(
                name: "FromAge",
                table: "BmiCategories",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ToAge",
                table: "BmiCategories",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "BmiCategories",
                keyColumn: "Id",
                keyValue: new Guid("01955c46-2df8-74fd-bbb1-c8c1d792ee5b"),
                columns: new[] { "BmiBottom", "BmiTop", "FromAge", "ToAge" },
                values: new object[] { 14m, 17m, 0, 2 });

            migrationBuilder.UpdateData(
                table: "BmiCategories",
                keyColumn: "Id",
                keyValue: new Guid("01955c46-5e27-7c17-97ee-a06ded033c32"),
                columns: new[] { "FromAge", "ToAge" },
                values: new object[] { 0, 2 });

            migrationBuilder.UpdateData(
                table: "BmiCategories",
                keyColumn: "Id",
                keyValue: new Guid("01955c46-81c0-7e96-9961-3b490a2d1c8f"),
                columns: new[] { "FromAge", "ToAge" },
                values: new object[] { 0, 2 });

            migrationBuilder.UpdateData(
                table: "BmiCategories",
                keyColumn: "Id",
                keyValue: new Guid("01955c46-a1d5-7fe9-a779-da396903a7a4"),
                columns: new[] { "FromAge", "ToAge" },
                values: new object[] { 0, 2 });

            migrationBuilder.InsertData(
                table: "BmiCategories",
                columns: new[] { "Id", "BmiBottom", "BmiTop", "CreatedAt", "FromAge", "Name", "ToAge", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("10cf12cd-9e5f-456e-a073-1b359028123e"), 95m, 85m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 16, "Over Weight", 100, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("1bc993ae-e0a8-4eec-bf2a-c0576d62bc25"), 14m, 17m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, "Healthy Weight", 10, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("1e567c0b-62f1-4cf7-a528-f82f623ff9ee"), 0m, 14m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 0, "Under Weight", 2, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("2928f86d-44c9-46ac-8c26-13a77d1bc9a2"), 95m, 85m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, "Over Weight", 10, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("2b4a6c93-7be8-41c4-b173-242b37ae3f15"), 95m, 85m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 11, "Over Weight", 15, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("2ff769f8-c61b-4e0d-9e62-c1685e1a79c2"), 0m, 14m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 11, "Under Weight", 15, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("337a2951-d570-4fa7-8a3a-264cbfe1ea0a"), 95m, 85m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Over Weight", 5, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("3a9b3c93-e64e-4f0e-9345-2c5cccb20e82"), 0m, 14m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, "Under Weight", 10, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("4aa18090-73ec-4c41-bcfc-3f8b2f144e7e"), 140m, 120m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, "Servere Obesity", 10, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("54812287-f31f-4c8c-b90f-5e1d52782810"), 14m, 17m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 16, "Healthy Weight", 100, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("5c3fa0f5-f60b-4c4f-b2d0-0d0a4c03db43"), 14m, 17m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 11, "Healthy Weight", 15, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("61f4d3d2-6f6a-408a-9d4f-1ad84a82e150"), 0m, 13m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 16, "Under Weight", 100, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("889fd61d-679f-4191-878e-f52bb3d5ec65"), 120m, 95m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 16, "Obesity", 100, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("88e091a4-02ef-404e-9522-0cb7f933f03d"), 140m, 120m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 16, "Servere Obesity", 100, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("92c0c29c-978d-43d5-b58a-3f7fd5ea1d60"), 140m, 120m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Servere Obesity", 5, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("a25596e3-6c7f-4e59-bac3-19f155b8a7dc"), 120m, 95m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, "Obesity", 10, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("b4c1f9b4-544e-45d5-936f-83e90e9ac0a6"), 120m, 95m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Obesity", 5, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("c8fbecc3-857a-4427-a626-d38c56a147cc"), 120m, 95m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 11, "Obesity", 15, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("dbbe82b4-ef1e-48b5-a6d5-d479905228d8"), 140m, 120m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 11, "Servere Obesity", 15, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("efc73434-1e42-43ae-8d14-14878e5b13b1"), 14m, 17m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Healthy Weight", 5, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("f6ed1a64-0a0e-4217-bcb4-c1179c519a79"), 0m, 13m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Under Weight", 5, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "BmiCategories",
                keyColumn: "Id",
                keyValue: new Guid("10cf12cd-9e5f-456e-a073-1b359028123e"));

            migrationBuilder.DeleteData(
                table: "BmiCategories",
                keyColumn: "Id",
                keyValue: new Guid("1bc993ae-e0a8-4eec-bf2a-c0576d62bc25"));

            migrationBuilder.DeleteData(
                table: "BmiCategories",
                keyColumn: "Id",
                keyValue: new Guid("1e567c0b-62f1-4cf7-a528-f82f623ff9ee"));

            migrationBuilder.DeleteData(
                table: "BmiCategories",
                keyColumn: "Id",
                keyValue: new Guid("2928f86d-44c9-46ac-8c26-13a77d1bc9a2"));

            migrationBuilder.DeleteData(
                table: "BmiCategories",
                keyColumn: "Id",
                keyValue: new Guid("2b4a6c93-7be8-41c4-b173-242b37ae3f15"));

            migrationBuilder.DeleteData(
                table: "BmiCategories",
                keyColumn: "Id",
                keyValue: new Guid("2ff769f8-c61b-4e0d-9e62-c1685e1a79c2"));

            migrationBuilder.DeleteData(
                table: "BmiCategories",
                keyColumn: "Id",
                keyValue: new Guid("337a2951-d570-4fa7-8a3a-264cbfe1ea0a"));

            migrationBuilder.DeleteData(
                table: "BmiCategories",
                keyColumn: "Id",
                keyValue: new Guid("3a9b3c93-e64e-4f0e-9345-2c5cccb20e82"));

            migrationBuilder.DeleteData(
                table: "BmiCategories",
                keyColumn: "Id",
                keyValue: new Guid("4aa18090-73ec-4c41-bcfc-3f8b2f144e7e"));

            migrationBuilder.DeleteData(
                table: "BmiCategories",
                keyColumn: "Id",
                keyValue: new Guid("54812287-f31f-4c8c-b90f-5e1d52782810"));

            migrationBuilder.DeleteData(
                table: "BmiCategories",
                keyColumn: "Id",
                keyValue: new Guid("5c3fa0f5-f60b-4c4f-b2d0-0d0a4c03db43"));

            migrationBuilder.DeleteData(
                table: "BmiCategories",
                keyColumn: "Id",
                keyValue: new Guid("61f4d3d2-6f6a-408a-9d4f-1ad84a82e150"));

            migrationBuilder.DeleteData(
                table: "BmiCategories",
                keyColumn: "Id",
                keyValue: new Guid("889fd61d-679f-4191-878e-f52bb3d5ec65"));

            migrationBuilder.DeleteData(
                table: "BmiCategories",
                keyColumn: "Id",
                keyValue: new Guid("88e091a4-02ef-404e-9522-0cb7f933f03d"));

            migrationBuilder.DeleteData(
                table: "BmiCategories",
                keyColumn: "Id",
                keyValue: new Guid("92c0c29c-978d-43d5-b58a-3f7fd5ea1d60"));

            migrationBuilder.DeleteData(
                table: "BmiCategories",
                keyColumn: "Id",
                keyValue: new Guid("a25596e3-6c7f-4e59-bac3-19f155b8a7dc"));

            migrationBuilder.DeleteData(
                table: "BmiCategories",
                keyColumn: "Id",
                keyValue: new Guid("b4c1f9b4-544e-45d5-936f-83e90e9ac0a6"));

            migrationBuilder.DeleteData(
                table: "BmiCategories",
                keyColumn: "Id",
                keyValue: new Guid("c8fbecc3-857a-4427-a626-d38c56a147cc"));

            migrationBuilder.DeleteData(
                table: "BmiCategories",
                keyColumn: "Id",
                keyValue: new Guid("dbbe82b4-ef1e-48b5-a6d5-d479905228d8"));

            migrationBuilder.DeleteData(
                table: "BmiCategories",
                keyColumn: "Id",
                keyValue: new Guid("efc73434-1e42-43ae-8d14-14878e5b13b1"));

            migrationBuilder.DeleteData(
                table: "BmiCategories",
                keyColumn: "Id",
                keyValue: new Guid("f6ed1a64-0a0e-4217-bcb4-c1179c519a79"));

            migrationBuilder.DropColumn(
                name: "FromAge",
                table: "BmiCategories");

            migrationBuilder.DropColumn(
                name: "ToAge",
                table: "BmiCategories");

            migrationBuilder.UpdateData(
                table: "BmiCategories",
                keyColumn: "Id",
                keyValue: new Guid("01955c46-2df8-74fd-bbb1-c8c1d792ee5b"),
                columns: new[] { "BmiBottom", "BmiTop" },
                values: new object[] { 5m, 85m });

            migrationBuilder.InsertData(
                table: "BmiCategories",
                columns: new[] { "Id", "BmiBottom", "BmiTop", "CreatedAt", "Name", "UpdatedAt" },
                values: new object[] { new Guid("01955c45-f781-7835-8d4b-aff20764aca6"), 0m, 5m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Under Weight", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) });
        }
    }
}
