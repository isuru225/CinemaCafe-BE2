using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieAppBackend.Migrations
{
    /// <inheritdoc />
    public partial class AddedTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BookingInfos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SelectedSeats = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShowTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AdultTicketQty = table.Column<int>(type: "int", nullable: false),
                    ChildTicketQty = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingInfos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FoodBeverages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoodBeverages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Movie",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IMDBRating = table.Column<float>(type: "real", nullable: false),
                    Genres = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ScreeningDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movie", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Theaters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TheaterName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Theaters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MobileNumber = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Screening",
                columns: table => new
                {
                    MoviesId = table.Column<int>(type: "int", nullable: false),
                    theatersId = table.Column<int>(type: "int", nullable: false),
                    ShowTime = table.Column<DateTime>(type: "DateTime", nullable: false),
                    AdultTicketPrice = table.Column<Decimal>(type: "Decimal", nullable: false),
                    ChildTicketPrice = table.Column<Decimal>(type: "Decimal", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Screening", x => new { x.MoviesId, x.theatersId });
                    table.ForeignKey(
                        name: "FK_Screening_Movie_MoviesId",
                        column: x => x.MoviesId,
                        principalTable: "Movie",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MovieItemTheater_Theaters_theatersId",
                        column: x => x.theatersId,
                        principalTable: "Theaters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MovieItemTheater_theatersId",
                table: "Screening",
                column: "theatersId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookingInfos");

            migrationBuilder.DropTable(
                name: "FoodBeverages");

            migrationBuilder.DropTable(
                name: "MovieItemTheater");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Movie");

            migrationBuilder.DropTable(
                name: "Theaters");
        }
    }
}
