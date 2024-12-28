using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Developer_Toolbox.Migrations
{
    public partial class AddedRelatedBadgesToWeeklyChallenges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "BadgeWeeklyChallenge",
                columns: table => new
                {
                    RelatedBadgesId = table.Column<int>(type: "int", nullable: false),
                    TargetWeeklyChallengesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BadgeWeeklyChallenge", x => new { x.RelatedBadgesId, x.TargetWeeklyChallengesId });
                    table.ForeignKey(
                        name: "FK_BadgeWeeklyChallenge_Badges_RelatedBadgesId",
                        column: x => x.RelatedBadgesId,
                        principalTable: "Badges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BadgeWeeklyChallenge_WeeklyChallenges_TargetWeeklyChallengesId",
                        column: x => x.TargetWeeklyChallengesId,
                        principalTable: "WeeklyChallenges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BadgeWeeklyChallenge_TargetWeeklyChallengesId",
                table: "BadgeWeeklyChallenge",
                column: "TargetWeeklyChallengesId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BadgeWeeklyChallenge");

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
    }
}
