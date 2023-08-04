using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HotelListingAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddedDefaultRoles3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                    { "42da8e3f-7029-42d8-98e3-20444dc69d9f", null, "Adminstrator", "ADMINSTRATOR" },
                    { "7a87a846-387a-4ba1-99aa-f3125c45ce39", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "42da8e3f-7029-42d8-98e3-20444dc69d9f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7a87a846-387a-4ba1-99aa-f3125c45ce39");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "145c14c0-2ea3-4911-bae9-9b1759119c83", null, "User", "=USER" },
                    { "1ec8b394-4eba-454c-86a7-690e6b573664", null, "Adminstrator", "ADMINSTRATOR" }
                });
        }
    }
}
