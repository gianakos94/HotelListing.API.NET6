using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HotelListingAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddedDefaultRoles2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "74309ec1-cf0c-43d3-b7c7-d41a34704ace");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e327e1fb-42c4-4540-9c23-cc8fdc2a55ed");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "145c14c0-2ea3-4911-bae9-9b1759119c83", null, "User", "=USER" },
                    { "1ec8b394-4eba-454c-86a7-690e6b573664", null, "Adminstrator", "ADMINSTRATOR" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "145c14c0-2ea3-4911-bae9-9b1759119c83");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1ec8b394-4eba-454c-86a7-690e6b573664");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "74309ec1-cf0c-43d3-b7c7-d41a34704ace", null, "Adminstrator", "ADMINSTRATOR" },
                    { "e327e1fb-42c4-4540-9c23-cc8fdc2a55ed", null, "User", "=USER" }
                });
        }
    }
}
