using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Developer_Toolbox.Migrations
{
    public partial class AddedTargetWeeklyChallengesToBadge : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BadgeId",
                table: "WeeklyChallenges",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WeeklyChallenges_BadgeId",
                table: "WeeklyChallenges",
                column: "BadgeId");

            migrationBuilder.AddForeignKey(
                name: "FK_WeeklyChallenges_Badges_BadgeId",
                table: "WeeklyChallenges",
                column: "BadgeId",
                principalTable: "Badges",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WeeklyChallenges_Badges_BadgeId",
                table: "WeeklyChallenges");

            migrationBuilder.DropIndex(
                name: "IX_WeeklyChallenges_BadgeId",
                table: "WeeklyChallenges");

            migrationBuilder.DropColumn(
                name: "BadgeId",
                table: "WeeklyChallenges");
        }
    }
}
