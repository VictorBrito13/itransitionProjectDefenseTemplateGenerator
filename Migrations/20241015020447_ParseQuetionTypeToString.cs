using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ItransitionTemplates.Migrations
{
    /// <inheritdoc />
    public partial class ParseQuetionTypeToString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "questionType",
                table: "Questions",
                newName: "QuestionType");

            migrationBuilder.AlterColumn<string>(
                name: "QuestionType",
                table: "Questions",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "QuestionType",
                table: "Questions",
                newName: "questionType");

            migrationBuilder.AlterColumn<int>(
                name: "questionType",
                table: "Questions",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
