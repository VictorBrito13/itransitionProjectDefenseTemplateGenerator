using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ItransitionTemplates.Migrations
{
    /// <inheritdoc />
    public partial class dates_as_strings_20 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Comments_TemplateId",
                table: "Comments",
                column: "TemplateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Templates_TemplateId",
                table: "Comments",
                column: "TemplateId",
                principalTable: "Templates",
                principalColumn: "TemplateId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Templates_TemplateId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_TemplateId",
                table: "Comments");
        }
    }
}
