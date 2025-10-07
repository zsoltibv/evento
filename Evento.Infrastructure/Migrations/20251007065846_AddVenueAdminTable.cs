using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Evento.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddVenueAdminTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a5c8ffb6-2c7d-457e-839b-9da38ac8912b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e8967d67-14d6-4572-8042-7b24815b1f1a");

            migrationBuilder.AddColumn<int>(
                name: "VenueId",
                table: "AspNetUsers",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "VenueAdmins",
                columns: table => new
                {
                    VenueId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VenueAdmins", x => new { x.VenueId, x.UserId });
                    table.ForeignKey(
                        name: "FK_VenueAdmins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VenueAdmins_Venues_VenueId",
                        column: x => x.VenueId,
                        principalTable: "Venues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0fa24c22-6da8-4d90-ad4b-46002d212ad5", null, "Admin", "ADMIN" },
                    { "594b2059-216b-4261-b00e-aebe9d4d6de9", null, "User", "USER" },
                    { "7cde4c7c-8b00-4664-87a5-96ad513d248e", null, "VenueAdmin", "VENUE_ADMIN" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_VenueId",
                table: "AspNetUsers",
                column: "VenueId");

            migrationBuilder.CreateIndex(
                name: "IX_VenueAdmins_UserId",
                table: "VenueAdmins",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Venues_VenueId",
                table: "AspNetUsers",
                column: "VenueId",
                principalTable: "Venues",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Venues_VenueId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "VenueAdmins");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_VenueId",
                table: "AspNetUsers");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0fa24c22-6da8-4d90-ad4b-46002d212ad5");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "594b2059-216b-4261-b00e-aebe9d4d6de9");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7cde4c7c-8b00-4664-87a5-96ad513d248e");

            migrationBuilder.DropColumn(
                name: "VenueId",
                table: "AspNetUsers");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "a5c8ffb6-2c7d-457e-839b-9da38ac8912b", null, "Admin", "ADMIN" },
                    { "e8967d67-14d6-4572-8042-7b24815b1f1a", null, "User", "USER" }
                });
        }
    }
}
