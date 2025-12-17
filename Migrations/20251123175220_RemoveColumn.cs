using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASM_C_6_API.Migrations
{
    /// <inheritdoc />
    public partial class RemoveColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_Combos_ComboId",
                table: "OrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_Combos_ComboId1",
                table: "OrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_FoodItems_FoodItemId",
                table: "OrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_FoodItems_FoodItemId1",
                table: "OrderDetails");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetails_ComboId1",
                table: "OrderDetails");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetails_FoodItemId1",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "ComboId1",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "FoodItemId1",
                table: "OrderDetails");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_Combos_ComboId",
                table: "OrderDetails",
                column: "ComboId",
                principalTable: "Combos",
                principalColumn: "ComboId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_FoodItems_FoodItemId",
                table: "OrderDetails",
                column: "FoodItemId",
                principalTable: "FoodItems",
                principalColumn: "FoodItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_Combos_ComboId",
                table: "OrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_FoodItems_FoodItemId",
                table: "OrderDetails");

            migrationBuilder.AddColumn<int>(
                name: "ComboId1",
                table: "OrderDetails",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FoodItemId1",
                table: "OrderDetails",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_ComboId1",
                table: "OrderDetails",
                column: "ComboId1");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_FoodItemId1",
                table: "OrderDetails",
                column: "FoodItemId1");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_Combos_ComboId",
                table: "OrderDetails",
                column: "ComboId",
                principalTable: "Combos",
                principalColumn: "ComboId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_Combos_ComboId1",
                table: "OrderDetails",
                column: "ComboId1",
                principalTable: "Combos",
                principalColumn: "ComboId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_FoodItems_FoodItemId",
                table: "OrderDetails",
                column: "FoodItemId",
                principalTable: "FoodItems",
                principalColumn: "FoodItemId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_FoodItems_FoodItemId1",
                table: "OrderDetails",
                column: "FoodItemId1",
                principalTable: "FoodItems",
                principalColumn: "FoodItemId");
        }
    }
}
