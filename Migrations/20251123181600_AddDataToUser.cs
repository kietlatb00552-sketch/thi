using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ASM_C_6_API.Migrations
{
    /// <inheritdoc />
    public partial class AddDataToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "Email", "FirstName", "LastName", "PasswordHash", "PhoneNumber", "RefreshToken", "RoleId", "Username" },
                values: new object[,]
                {
                    { 1, "Nguyendinhnam24109@gmail.com", "System", "Administrator", "AQAAAAEAACcQNAAAAEJv1y7sX5G6k8jvY9Vh3F4Z2x5Y6Z7X8Y9Z0A1B2C3D4E5F6G7H8I9J0K1L2M3N4O5P6Q7R8S9T0U1V2W3X4Y5Z6", "0123456789", "", 1, "admin" },
                    { 2, "nguyendinhnam288@gmail.com", "Regular", "User", "AQAAAAEAACcQNAAAAEJv1y7sX5G6k8jvY9Vh3F4Z2x5Y6Z7X8Y9Z0A1B2C3D4E5F6G7H8I9J0K1L2M3N4O5P6Q7R8S9T0U1V2W3X4Y5Z6", "0987654321", "", 2, "user" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2);
        }
    }
}
