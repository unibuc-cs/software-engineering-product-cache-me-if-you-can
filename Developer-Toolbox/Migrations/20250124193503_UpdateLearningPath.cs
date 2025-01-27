using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Developer_Toolbox.Migrations
{
    public partial class UpdateLearningPath : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "LearningPaths",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LearningPaths_UserId",
                table: "LearningPaths",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_LearningPaths_AspNetUsers_UserId",
                table: "LearningPaths",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LearningPaths_AspNetUsers_UserId",
                table: "LearningPaths");

            migrationBuilder.DropIndex(
                name: "IX_LearningPaths_UserId",
                table: "LearningPaths");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "LearningPaths",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
