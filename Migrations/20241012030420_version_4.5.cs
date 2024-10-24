using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ItransitionTemplates.Migrations
{
    /// <inheritdoc />
    public partial class version_45 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<ulong>(
                name: "UserId",
                table: "Response",
                type: "bigint unsigned",
                nullable: false,
                defaultValue: 0ul);

            migrationBuilder.CreateIndex(
                name: "IX_Response_UserId",
                table: "Response",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Response_Users_UserId",
                table: "Response",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Response_Users_UserId",
                table: "Response");

            migrationBuilder.DropIndex(
                name: "IX_Response_UserId",
                table: "Response");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Response");
        }
    }
}
