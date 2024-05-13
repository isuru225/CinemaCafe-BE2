using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieAppBackend.Migrations
{
    /// <inheritdoc />
    public partial class Add1to1relationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AvailableFoods",
                columns: table => new
                {
                    FoodBeveragesId = table.Column<int>(type: "int", nullable: false),
                    TheatersId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AvailableFoods", x => new { x.FoodBeveragesId, x.TheatersId });
                    table.ForeignKey(
                        name: "FK_AvailableFoods_FoodBeverages_FoodBeveragesId",
                        column: x => x.FoodBeveragesId,
                        principalTable: "FoodBeverages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AvailableFoods_Theaters_TheatersId",
                        column: x => x.TheatersId,
                        principalTable: "Theaters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Purchases",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PaymentMethod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VAT = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BookingInfoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Purchases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Purchases_BookingInfos_BookingInfoId",
                        column: x => x.BookingInfoId,
                        principalTable: "BookingInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderedFoods",
                columns: table => new
                {
                    foodBeveragesId = table.Column<int>(type: "int", nullable: false),
                    purchasesId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderedFoods", x => new { x.foodBeveragesId, x.purchasesId });
                    table.ForeignKey(
                        name: "FK_OrderedFoods_FoodBeverages_foodBeveragesId",
                        column: x => x.foodBeveragesId,
                        principalTable: "FoodBeverages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderedFoods_Purchases_purchasesId",
                        column: x => x.purchasesId,
                        principalTable: "Purchases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderedFoods_purchasesId",
                table: "OrderedFoods",
                column: "purchasesId");

            migrationBuilder.CreateIndex(
                name: "IX_FoodBeverageTheater_TheatersId",
                table: "AvailableFoods",
                column: "TheatersId");

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_BookingInfoId",
                table: "Purchases",
                column: "BookingInfoId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FoodBeveragePurchase");

            migrationBuilder.DropTable(
                name: "FoodBeverageTheater");

            migrationBuilder.DropTable(
                name: "Purchases");
        }
    }
}
