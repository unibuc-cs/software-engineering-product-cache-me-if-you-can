using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Developer_Toolbox.Migrations
{
    public partial class UpdateDBContext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LockedSolutions_LockedExercises_LockedExerciseId",
                table: "LockedSolutions");

            migrationBuilder.AddForeignKey(
                name: "FK_LockedSolutions_LockedExercises_LockedExerciseId",
                table: "LockedSolutions",
                column: "LockedExerciseId",
                principalTable: "LockedExercises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LockedSolutions_LockedExercises_LockedExerciseId",
                table: "LockedSolutions");

            migrationBuilder.AddForeignKey(
                name: "FK_LockedSolutions_LockedExercises_LockedExerciseId",
                table: "LockedSolutions",
                column: "LockedExerciseId",
                principalTable: "LockedExercises",
                principalColumn: "Id");
        }
    }
}
