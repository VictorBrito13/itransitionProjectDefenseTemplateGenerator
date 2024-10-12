using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ItransitionTemplates.Migrations
{
    /// <inheritdoc />
    public partial class version_44 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Question_Response_ResponseId",
                table: "Question");

            migrationBuilder.DropIndex(
                name: "IX_Question_ResponseId",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "ResponseId",
                table: "Question");

            migrationBuilder.AddColumn<ulong>(
                name: "QuestionId",
                table: "Response",
                type: "bigint unsigned",
                nullable: false,
                defaultValue: 0ul);

            migrationBuilder.CreateIndex(
                name: "IX_Response_QuestionId",
                table: "Response",
                column: "QuestionId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Response_Question_QuestionId",
                table: "Response",
                column: "QuestionId",
                principalTable: "Question",
                principalColumn: "QuestionId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Response_Question_QuestionId",
                table: "Response");

            migrationBuilder.DropIndex(
                name: "IX_Response_QuestionId",
                table: "Response");

            migrationBuilder.DropColumn(
                name: "QuestionId",
                table: "Response");

            migrationBuilder.AddColumn<ulong>(
                name: "ResponseId",
                table: "Question",
                type: "bigint unsigned",
                nullable: false,
                defaultValue: 0ul);

            migrationBuilder.CreateIndex(
                name: "IX_Question_ResponseId",
                table: "Question",
                column: "ResponseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Question_Response_ResponseId",
                table: "Question",
                column: "ResponseId",
                principalTable: "Response",
                principalColumn: "ResponseId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
