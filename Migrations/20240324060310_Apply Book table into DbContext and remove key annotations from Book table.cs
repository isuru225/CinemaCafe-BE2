using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieAppBackend.Migrations
{
    /// <inheritdoc />
    public partial class ApplyBooktableintoDbContextandremovekeyannotationsfromBooktable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    MovieItemId = table.Column<int>(type: "int", nullable: false),
                    TheaterId = table.Column<int>(type: "int", nullable: false),
                    BookingInfoId = table.Column<int>(type: "int", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => new { x.UserId, x.BookingInfoId, x.MovieItemId, x.TheaterId });
                    table.ForeignKey(
                        name: "FK_Books_BookingInfos_BookingInfoId",
                        column: x => x.BookingInfoId,
                        principalTable: "BookingInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Books_Movie_MovieItemId",
                        column: x => x.MovieItemId,
                        principalTable: "Movie",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Books_Theaters_TheaterId",
                        column: x => x.TheaterId,
                        principalTable: "Theaters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Books_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Books_BookingInfoId",
                table: "Books",
                column: "BookingInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_Books_MovieItemId",
                table: "Books",
                column: "MovieItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Books_TheaterId",
                table: "Books",
                column: "TheaterId");

            migrationBuilder.CreateIndex(
                name: "IX_Books_UserId",
                table: "Books",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Books");
        }
    }
}
