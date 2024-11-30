using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Developer_Toolbox.Data.Migrations
{
    public partial class CreateBadgeEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Badge",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TargetActivityId = table.Column<int>(type: "int", nullable: false),
                    TargetNoOfTimes = table.Column<int>(type: "int", nullable: false),
                    TargetCategoryId = table.Column<int>(type: "int", nullable: true),
                    TargetLevel = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Badge", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Badge_Activities_TargetActivityId",
                        column: x => x.TargetActivityId,
                        principalTable: "Activities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Badge_Categories_TargetCategoryId",
                        column: x => x.TargetCategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ApplicationUserBadge",
                columns: table => new
                {
                    BadgesId = table.Column<int>(type: "int", nullable: false),
                    UsersId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserBadge", x => new { x.BadgesId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_ApplicationUserBadge_AspNetUsers_UsersId",
                        column: x => x.UsersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserBadge_Badge_BadgesId",
                        column: x => x.BadgesId,
                        principalTable: "Badge",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                        name: "FK_BadgeTag_Badge_RelatedBadgesId",
                        column: x => x.RelatedBadgesId,
                        principalTable: "Badge",
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
                name: "IX_ApplicationUserBadge_UsersId",
                table: "ApplicationUserBadge",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_Badge_TargetActivityId",
                table: "Badge",
                column: "TargetActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_Badge_TargetCategoryId",
                table: "Badge",
                column: "TargetCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_BadgeTag_TargetTagsId",
                table: "BadgeTag",
                column: "TargetTagsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationUserBadge");

            migrationBuilder.DropTable(
                name: "BadgeTag");

            migrationBuilder.DropTable(
                name: "Badge");
        }
    }
}
