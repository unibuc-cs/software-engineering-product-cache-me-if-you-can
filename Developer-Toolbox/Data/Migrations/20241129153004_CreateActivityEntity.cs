using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Developer_Toolbox.Data.Migrations
{
    public partial class CreateActivityEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "reputationPoints",
                table: "Activities",
                newName: "ReputationPoints");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ReputationPoints",
                table: "Activities",
                newName: "reputationPoints");
        }
    }
}
