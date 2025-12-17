using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ASM_C_6_API.Migrations
{
    /// <inheritdoc />
    public partial class AddDataToDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Carts",
                columns: new[] { "CartId", "CreatedDate", "UpdatedDate", "UserId" },
                values: new object[] { 1, new DateTime(2025, 11, 24, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 11, 24, 0, 0, 0, 0, DateTimeKind.Utc), 2 });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "CategoryName", "CreatedDate", "Description", "ImageUrl", "IsActive" },
                values: new object[,]
                {
                    { 1, "Rice Dishes", new DateTime(2025, 11, 24, 0, 0, 0, 0, DateTimeKind.Utc), "Cơm và các món ăn kèm truyền thống", "/images/categories/rice.jpg", true },
                    { 2, "Noodles", new DateTime(2025, 11, 24, 0, 0, 0, 0, DateTimeKind.Utc), "Phở, mì và các món bún", "/images/categories/noodles.jpg", true },
                    { 3, "Drinks", new DateTime(2025, 11, 24, 0, 0, 0, 0, DateTimeKind.Utc), "Nước giải khát: trà, nước ngọt, nước trái cây", "/images/categories/drinks.jpg", true }
                });

            migrationBuilder.InsertData(
                table: "Combos",
                columns: new[] { "ComboId", "ComboName", "CreatedDate", "Description", "DiscountPrice", "ImageUrl", "IsAvailable", "Price" },
                values: new object[] { 1, "Lunch Combo", new DateTime(2025, 11, 24, 0, 0, 0, 0, DateTimeKind.Utc), "Gồm 1 cơm gà + 1 trà đá", 7.99m, "/images/combos/lunch_combo.jpg", true, 8.99m });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "OrderId", "DeliveredDate", "DeliveryAddress", "IsPaid", "Note", "OrderDate", "PaymentMethod", "PaymentUrl", "PhoneNumber", "ReceiverName", "Status", "TotalAmount", "UserId" },
                values: new object[] { 1, null, "123 Example Street", false, "Giao giờ hành chính", new DateTime(2025, 11, 24, 0, 0, 0, 0, DateTimeKind.Utc), 0, null, "0987654321", "John Doe", 0, 6.99m, 2 });

            migrationBuilder.InsertData(
                table: "FoodItems",
                columns: new[] { "FoodItemId", "CategoryId", "CreatedDate", "Description", "FoodName", "ImageUrl", "IsAvailable", "Price", "Theme" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2025, 11, 24, 0, 0, 0, 0, DateTimeKind.Utc), "Gà nướng sốt đặc biệt phục vụ cùng cơm trắng", "Grilled Chicken with Rice", "/images/food/grilled_chicken.jpg", true, 6.99m, "Savory" },
                    { 2, 2, new DateTime(2025, 11, 24, 0, 0, 0, 0, DateTimeKind.Utc), "Phở bò thơm ngon với nước dùng đậm đà", "Beef Pho", "/images/food/beef_pho.jpg", true, 5.50m, "Traditional" },
                    { 3, 3, new DateTime(2025, 11, 24, 0, 0, 0, 0, DateTimeKind.Utc), "Trà đá mát lạnh", "Iced Tea", "/images/food/iced_tea.jpg", true, 1.50m, "Refreshing" }
                });

            migrationBuilder.InsertData(
                table: "CartItems",
                columns: new[] { "CartItemId", "AddedDate", "CartId", "ComboId", "FoodItemId", "Quantity", "UnitPrice" },
                values: new object[] { 1, new DateTime(2025, 11, 24, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, 1, 1, 6.99m });

            migrationBuilder.InsertData(
                table: "ComboItems",
                columns: new[] { "ComboItemId", "ComboId", "FoodItemId", "Quantity" },
                values: new object[,]
                {
                    { 1, 1, 1, 1 },
                    { 2, 1, 3, 1 }
                });

            migrationBuilder.InsertData(
                table: "OrderDetails",
                columns: new[] { "OrderDetailId", "ComboId", "FoodItemId", "OrderId", "Quantity", "UnitPrice" },
                values: new object[] { 1, null, 1, 1, 1, 6.99m });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CartItems",
                keyColumn: "CartItemId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ComboItems",
                keyColumn: "ComboItemId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ComboItems",
                keyColumn: "ComboItemId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "FoodItems",
                keyColumn: "FoodItemId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "OrderDetails",
                keyColumn: "OrderDetailId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Carts",
                keyColumn: "CartId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Combos",
                keyColumn: "ComboId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "FoodItems",
                keyColumn: "FoodItemId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "FoodItems",
                keyColumn: "FoodItemId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "OrderId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 3);
        }
    }
}
