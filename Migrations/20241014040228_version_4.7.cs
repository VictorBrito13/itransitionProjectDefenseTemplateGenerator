using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ItransitionTemplates.Migrations
{
    /// <inheritdoc />
    public partial class version_47 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Admin_Templates_TemplateId",
                table: "Admin");

            migrationBuilder.DropForeignKey(
                name: "FK_Admin_Users_UserId",
                table: "Admin");

            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Users_UserId",
                table: "Comment");

            migrationBuilder.DropForeignKey(
                name: "FK_Question_Templates_TemplateId",
                table: "Question");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionOption_Question_QuestionId",
                table: "QuestionOption");

            migrationBuilder.DropForeignKey(
                name: "FK_Response_Question_QuestionId",
                table: "Response");

            migrationBuilder.DropForeignKey(
                name: "FK_Response_Users_UserId",
                table: "Response");

            migrationBuilder.DropForeignKey(
                name: "fk_Tag_has_templates_Tag_TagId",
                table: "Tag_has_templates");

            migrationBuilder.DropForeignKey(
                name: "fk_Tag_has_templates_Template_TemplateId",
                table: "Tag_has_templates");

            migrationBuilder.DropForeignKey(
                name: "FK_Templates_Topic_TopicId",
                table: "Templates");

            migrationBuilder.DropForeignKey(
                name: "FK_UserAllowedToAnswer_Templates_TemplateId",
                table: "UserAllowedToAnswer");

            migrationBuilder.DropForeignKey(
                name: "FK_UserAllowedToAnswer_Users_UserId",
                table: "UserAllowedToAnswer");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserAllowedToAnswer",
                table: "UserAllowedToAnswer");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Topic",
                table: "Topic");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tag",
                table: "Tag");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Response",
                table: "Response");

            migrationBuilder.DropPrimaryKey(
                name: "PK_QuestionOption",
                table: "QuestionOption");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Question",
                table: "Question");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Comment",
                table: "Comment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Admin",
                table: "Admin");

            migrationBuilder.RenameTable(
                name: "UserAllowedToAnswer",
                newName: "UserAllowedToAnswers");

            migrationBuilder.RenameTable(
                name: "Topic",
                newName: "Topics");

            migrationBuilder.RenameTable(
                name: "Tag",
                newName: "Tags");

            migrationBuilder.RenameTable(
                name: "Response",
                newName: "Responses");

            migrationBuilder.RenameTable(
                name: "QuestionOption",
                newName: "QuestionOptions");

            migrationBuilder.RenameTable(
                name: "Question",
                newName: "Questions");

            migrationBuilder.RenameTable(
                name: "Comment",
                newName: "Comments");

            migrationBuilder.RenameTable(
                name: "Admin",
                newName: "Admins");

            migrationBuilder.RenameIndex(
                name: "IX_UserAllowedToAnswer_TemplateId",
                table: "UserAllowedToAnswers",
                newName: "IX_UserAllowedToAnswers_TemplateId");

            migrationBuilder.RenameIndex(
                name: "IX_Topic_TopicId_Name",
                table: "Topics",
                newName: "IX_Topics_TopicId_Name");

            migrationBuilder.RenameIndex(
                name: "IX_Response_UserId",
                table: "Responses",
                newName: "IX_Responses_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Response_QuestionId",
                table: "Responses",
                newName: "IX_Responses_QuestionId");

            migrationBuilder.RenameIndex(
                name: "IX_QuestionOption_QuestionId",
                table: "QuestionOptions",
                newName: "IX_QuestionOptions_QuestionId");

            migrationBuilder.RenameIndex(
                name: "IX_Question_TemplateId",
                table: "Questions",
                newName: "IX_Questions_TemplateId");

            migrationBuilder.RenameIndex(
                name: "IX_Admin_TemplateId",
                table: "Admins",
                newName: "IX_Admins_TemplateId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserAllowedToAnswers",
                table: "UserAllowedToAnswers",
                columns: new[] { "UserId", "TemplateId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Topics",
                table: "Topics",
                column: "TopicId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tags",
                table: "Tags",
                column: "TagId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Responses",
                table: "Responses",
                column: "ResponseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_QuestionOptions",
                table: "QuestionOptions",
                column: "QuestionOptionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Questions",
                table: "Questions",
                column: "QuestionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Comments",
                table: "Comments",
                columns: new[] { "UserId", "TemplateId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Admins",
                table: "Admins",
                columns: new[] { "UserId", "TemplateId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Admins_Templates_TemplateId",
                table: "Admins",
                column: "TemplateId",
                principalTable: "Templates",
                principalColumn: "TemplateId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Admins_Users_UserId",
                table: "Admins",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Users_UserId",
                table: "Comments",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionOptions_Questions_QuestionId",
                table: "QuestionOptions",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "QuestionId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Templates_TemplateId",
                table: "Questions",
                column: "TemplateId",
                principalTable: "Templates",
                principalColumn: "TemplateId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Responses_Questions_QuestionId",
                table: "Responses",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "QuestionId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Responses_Users_UserId",
                table: "Responses",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tag_has_templates_Tags_TagId",
                table: "Tag_has_templates",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "TagId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tag_has_templates_Templates_TemplateId",
                table: "Tag_has_templates",
                column: "TemplateId",
                principalTable: "Templates",
                principalColumn: "TemplateId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Templates_Topics_TopicId",
                table: "Templates",
                column: "TopicId",
                principalTable: "Topics",
                principalColumn: "TopicId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserAllowedToAnswers_Templates_TemplateId",
                table: "UserAllowedToAnswers",
                column: "TemplateId",
                principalTable: "Templates",
                principalColumn: "TemplateId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserAllowedToAnswers_Users_UserId",
                table: "UserAllowedToAnswers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Admins_Templates_TemplateId",
                table: "Admins");

            migrationBuilder.DropForeignKey(
                name: "FK_Admins_Users_UserId",
                table: "Admins");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Users_UserId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionOptions_Questions_QuestionId",
                table: "QuestionOptions");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Templates_TemplateId",
                table: "Questions");

            migrationBuilder.DropForeignKey(
                name: "FK_Responses_Questions_QuestionId",
                table: "Responses");

            migrationBuilder.DropForeignKey(
                name: "FK_Responses_Users_UserId",
                table: "Responses");

            migrationBuilder.DropForeignKey(
                name: "FK_Tag_has_templates_Tags_TagId",
                table: "Tag_has_templates");

            migrationBuilder.DropForeignKey(
                name: "FK_Tag_has_templates_Templates_TemplateId",
                table: "Tag_has_templates");

            migrationBuilder.DropForeignKey(
                name: "FK_Templates_Topics_TopicId",
                table: "Templates");

            migrationBuilder.DropForeignKey(
                name: "FK_UserAllowedToAnswers_Templates_TemplateId",
                table: "UserAllowedToAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_UserAllowedToAnswers_Users_UserId",
                table: "UserAllowedToAnswers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserAllowedToAnswers",
                table: "UserAllowedToAnswers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Topics",
                table: "Topics");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tags",
                table: "Tags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Responses",
                table: "Responses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Questions",
                table: "Questions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_QuestionOptions",
                table: "QuestionOptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Comments",
                table: "Comments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Admins",
                table: "Admins");

            migrationBuilder.RenameTable(
                name: "UserAllowedToAnswers",
                newName: "UserAllowedToAnswer");

            migrationBuilder.RenameTable(
                name: "Topics",
                newName: "Topic");

            migrationBuilder.RenameTable(
                name: "Tags",
                newName: "Tag");

            migrationBuilder.RenameTable(
                name: "Responses",
                newName: "Response");

            migrationBuilder.RenameTable(
                name: "Questions",
                newName: "Question");

            migrationBuilder.RenameTable(
                name: "QuestionOptions",
                newName: "QuestionOption");

            migrationBuilder.RenameTable(
                name: "Comments",
                newName: "Comment");

            migrationBuilder.RenameTable(
                name: "Admins",
                newName: "Admin");

            migrationBuilder.RenameIndex(
                name: "IX_UserAllowedToAnswers_TemplateId",
                table: "UserAllowedToAnswer",
                newName: "IX_UserAllowedToAnswer_TemplateId");

            migrationBuilder.RenameIndex(
                name: "IX_Topics_TopicId_Name",
                table: "Topic",
                newName: "IX_Topic_TopicId_Name");

            migrationBuilder.RenameIndex(
                name: "IX_Responses_UserId",
                table: "Response",
                newName: "IX_Response_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Responses_QuestionId",
                table: "Response",
                newName: "IX_Response_QuestionId");

            migrationBuilder.RenameIndex(
                name: "IX_Questions_TemplateId",
                table: "Question",
                newName: "IX_Question_TemplateId");

            migrationBuilder.RenameIndex(
                name: "IX_QuestionOptions_QuestionId",
                table: "QuestionOption",
                newName: "IX_QuestionOption_QuestionId");

            migrationBuilder.RenameIndex(
                name: "IX_Admins_TemplateId",
                table: "Admin",
                newName: "IX_Admin_TemplateId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserAllowedToAnswer",
                table: "UserAllowedToAnswer",
                columns: new[] { "UserId", "TemplateId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Topic",
                table: "Topic",
                column: "TopicId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tag",
                table: "Tag",
                column: "TagId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Response",
                table: "Response",
                column: "ResponseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Question",
                table: "Question",
                column: "QuestionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_QuestionOption",
                table: "QuestionOption",
                column: "QuestionOptionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Comment",
                table: "Comment",
                columns: new[] { "UserId", "TemplateId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Admin",
                table: "Admin",
                columns: new[] { "UserId", "TemplateId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Admin_Templates_TemplateId",
                table: "Admin",
                column: "TemplateId",
                principalTable: "Templates",
                principalColumn: "TemplateId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Admin_Users_UserId",
                table: "Admin",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Users_UserId",
                table: "Comment",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Question_Templates_TemplateId",
                table: "Question",
                column: "TemplateId",
                principalTable: "Templates",
                principalColumn: "TemplateId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionOption_Question_QuestionId",
                table: "QuestionOption",
                column: "QuestionId",
                principalTable: "Question",
                principalColumn: "QuestionId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Response_Question_QuestionId",
                table: "Response",
                column: "QuestionId",
                principalTable: "Question",
                principalColumn: "QuestionId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Response_Users_UserId",
                table: "Response",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_Tag_has_templates_Tag_TagId",
                table: "Tag_has_templates",
                column: "TagId",
                principalTable: "Tag",
                principalColumn: "TagId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_Tag_has_templates_Template_TemplateId",
                table: "Tag_has_templates",
                column: "TemplateId",
                principalTable: "Templates",
                principalColumn: "TemplateId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Templates_Topic_TopicId",
                table: "Templates",
                column: "TopicId",
                principalTable: "Topic",
                principalColumn: "TopicId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserAllowedToAnswer_Templates_TemplateId",
                table: "UserAllowedToAnswer",
                column: "TemplateId",
                principalTable: "Templates",
                principalColumn: "TemplateId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserAllowedToAnswer_Users_UserId",
                table: "UserAllowedToAnswer",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
