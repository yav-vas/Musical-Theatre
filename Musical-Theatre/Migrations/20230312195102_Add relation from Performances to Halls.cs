using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Musical_Theatre.Migrations
{
    /// <inheritdoc />
    public partial class AddrelationfromPerformancestoHalls : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Performances_Hall_Id",
                table: "Performances",
                column: "Hall_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Performances_Halls_Hall_Id",
                table: "Performances",
                column: "Hall_Id",
                principalTable: "Halls",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Performances_Halls_Hall_Id",
                table: "Performances");

            migrationBuilder.DropIndex(
                name: "IX_Performances_Hall_Id",
                table: "Performances");
        }
    }
}
