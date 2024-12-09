using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Developer_Toolbox.Data.Migrations
{
    public partial class CreatedManyToManyBadgeTag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BadgeTag");

            migrationBuilder.CreateTable(
                name: "BadgeTags",
                columns: table => new
                {
                    BadgeId = table.Column<int>(type: "int", nullable: false),
                    TagId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BadgeTags", x => new { x.BadgeId, x.TagId });
                    table.ForeignKey(
                        name: "FK_BadgeTags_Badges_BadgeId",
                        column: x => x.BadgeId,
                        principalTable: "Badges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BadgeTags_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BadgeTags_TagId",
                table: "BadgeTags",
                column: "TagId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BadgeTags");

            migrationBuilder.CreateTable(
                name: "BadgeTag",
                columns: table => new
                {
                    RelatedBadgesId = table.Column<int>(type: "int", nullable: false),
                    TargetTagsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BadgeTag", x => new { x.RelatedBadgesId, x.TargetTagsId });
                    table.ForeignKey(
                        name: "FK_BadgeTag_Badges_RelatedBadgesId",
                        column: x => x.RelatedBadgesId,
                        principalTable: "Badges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BadgeTag_Tags_TargetTagsId",
                        column: x => x.TargetTagsId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BadgeTag_TargetTagsId",
                table: "BadgeTag",
                column: "TargetTagsId");
        }
    }
}
