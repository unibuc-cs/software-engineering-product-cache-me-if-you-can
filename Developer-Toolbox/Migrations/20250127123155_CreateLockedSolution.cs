using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Developer_Toolbox.Migrations
{
    public partial class CreateLockedSolution : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LockedSolutions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SolutionCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Score = table.Column<int>(type: "int", nullable: true),
                    LockedExerciseId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LockedSolutions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LockedSolutions_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LockedSolutions_LockedExercises_LockedExerciseId",
                        column: x => x.LockedExerciseId,
                        principalTable: "LockedExercises",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_LockedSolutions_LockedExerciseId",
                table: "LockedSolutions",
                column: "LockedExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_LockedSolutions_UserId",
                table: "LockedSolutions",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LockedSolutions");
        }
    }
}
