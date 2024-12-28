using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Developer_Toolbox.Migrations
{
    public partial class AddedManyToManyRelationshipBadgeChallenges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BadgeWeeklyChallenge");

            migrationBuilder.CreateTable(
                name: "BadgeChallenges",
                columns: table => new
                {
                    BadgeId = table.Column<int>(type: "int", nullable: false),
                    WeeklyChallengeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BadgeChallenges", x => new { x.BadgeId, x.WeeklyChallengeId });
                    table.ForeignKey(
                        name: "FK_BadgeChallenges_Badges_BadgeId",
                        column: x => x.BadgeId,
                        principalTable: "Badges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BadgeChallenges_WeeklyChallenges_WeeklyChallengeId",
                        column: x => x.WeeklyChallengeId,
                        principalTable: "WeeklyChallenges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BadgeChallenges_WeeklyChallengeId",
                table: "BadgeChallenges",
                column: "WeeklyChallengeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BadgeChallenges");

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
    }
}
