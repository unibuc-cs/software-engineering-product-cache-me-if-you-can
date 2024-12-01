using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Developer_Toolbox.Data.Migrations
{
    public partial class CreatedBadgeEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUserBadge_Badge_BadgesId",
                table: "ApplicationUserBadge");

            migrationBuilder.DropForeignKey(
                name: "FK_Badge_Activities_TargetActivityId",
                table: "Badge");

            migrationBuilder.DropForeignKey(
                name: "FK_Badge_Categories_TargetCategoryId",
                table: "Badge");

            migrationBuilder.DropForeignKey(
                name: "FK_BadgeTag_Badge_RelatedBadgesId",
                table: "BadgeTag");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Badge",
                table: "Badge");

            migrationBuilder.RenameTable(
                name: "Badge",
                newName: "Badges");

            migrationBuilder.RenameIndex(
                name: "IX_Badge_TargetCategoryId",
                table: "Badges",
                newName: "IX_Badges_TargetCategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Badge_TargetActivityId",
                table: "Badges",
                newName: "IX_Badges_TargetActivityId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Badges",
                table: "Badges",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUserBadge_Badges_BadgesId",
                table: "ApplicationUserBadge",
                column: "BadgesId",
                principalTable: "Badges",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Badges_Activities_TargetActivityId",
                table: "Badges",
                column: "TargetActivityId",
                principalTable: "Activities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Badges_Categories_TargetCategoryId",
                table: "Badges",
                column: "TargetCategoryId",
                principalTable: "Categories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BadgeTag_Badges_RelatedBadgesId",
                table: "BadgeTag",
                column: "RelatedBadgesId",
                principalTable: "Badges",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUserBadge_Badges_BadgesId",
                table: "ApplicationUserBadge");

            migrationBuilder.DropForeignKey(
                name: "FK_Badges_Activities_TargetActivityId",
                table: "Badges");

            migrationBuilder.DropForeignKey(
                name: "FK_Badges_Categories_TargetCategoryId",
                table: "Badges");

            migrationBuilder.DropForeignKey(
                name: "FK_BadgeTag_Badges_RelatedBadgesId",
                table: "BadgeTag");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Badges",
                table: "Badges");

            migrationBuilder.RenameTable(
                name: "Badges",
                newName: "Badge");

            migrationBuilder.RenameIndex(
                name: "IX_Badges_TargetCategoryId",
                table: "Badge",
                newName: "IX_Badge_TargetCategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Badges_TargetActivityId",
                table: "Badge",
                newName: "IX_Badge_TargetActivityId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Badge",
                table: "Badge",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUserBadge_Badge_BadgesId",
                table: "ApplicationUserBadge",
                column: "BadgesId",
                principalTable: "Badge",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Badge_Activities_TargetActivityId",
                table: "Badge",
                column: "TargetActivityId",
                principalTable: "Activities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Badge_Categories_TargetCategoryId",
                table: "Badge",
                column: "TargetCategoryId",
                principalTable: "Categories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BadgeTag_Badge_RelatedBadgesId",
                table: "BadgeTag",
                column: "RelatedBadgesId",
                principalTable: "Badge",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
