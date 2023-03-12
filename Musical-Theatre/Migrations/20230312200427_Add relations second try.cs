using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Musical_Theatre.Migrations
{
    /// <inheritdoc />
    public partial class Addrelationssecondtry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Checker_Id",
                table: "Tickets",
                type: "varchar(255)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PriceCategoryId",
                table: "Seats",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_Checker_Id",
                table: "Tickets",
                column: "Checker_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_Seat_Id",
                table: "Tickets",
                column: "Seat_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Seats_PerformanceId",
                table: "Seats",
                column: "PerformanceId");

            migrationBuilder.CreateIndex(
                name: "IX_Seats_PriceCategoryId",
                table: "Seats",
                column: "PriceCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_PriceCategories_PerformanceId",
                table: "PriceCategories",
                column: "PerformanceId");

            migrationBuilder.AddForeignKey(
                name: "FK_PriceCategories_Performances_PerformanceId",
                table: "PriceCategories",
                column: "PerformanceId",
                principalTable: "Performances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Seats_Performances_PerformanceId",
                table: "Seats",
                column: "PerformanceId",
                principalTable: "Performances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Seats_PriceCategories_PriceCategoryId",
                table: "Seats",
                column: "PriceCategoryId",
                principalTable: "PriceCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_AspNetUsers_Checker_Id",
                table: "Tickets",
                column: "Checker_Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Seats_Seat_Id",
                table: "Tickets",
                column: "Seat_Id",
                principalTable: "Seats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PriceCategories_Performances_PerformanceId",
                table: "PriceCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_Seats_Performances_PerformanceId",
                table: "Seats");

            migrationBuilder.DropForeignKey(
                name: "FK_Seats_PriceCategories_PriceCategoryId",
                table: "Seats");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_AspNetUsers_Checker_Id",
                table: "Tickets");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Seats_Seat_Id",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_Checker_Id",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_Seat_Id",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Seats_PerformanceId",
                table: "Seats");

            migrationBuilder.DropIndex(
                name: "IX_Seats_PriceCategoryId",
                table: "Seats");

            migrationBuilder.DropIndex(
                name: "IX_PriceCategories_PerformanceId",
                table: "PriceCategories");

            migrationBuilder.DropColumn(
                name: "PriceCategoryId",
                table: "Seats");

            migrationBuilder.AlterColumn<int>(
                name: "Checker_Id",
                table: "Tickets",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldNullable: true);
        }
    }
}
