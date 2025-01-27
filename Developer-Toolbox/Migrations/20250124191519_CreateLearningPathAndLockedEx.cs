using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Developer_Toolbox.Migrations
{
    public partial class CreateLearningPathAndLockedEx : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LockedExerciseId",
                table: "Solutions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "LearningPaths",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LearningPaths", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LockedExercises",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LearningPathId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Summary = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Restrictions = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Examples = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Difficulty = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TestCases = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Instructions = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LockedExercises", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LockedExercises_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LockedExercises_LearningPaths_LearningPathId",
                        column: x => x.LearningPathId,
                        principalTable: "LearningPaths",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Solutions_LockedExerciseId",
                table: "Solutions",
                column: "LockedExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_LockedExercises_LearningPathId",
                table: "LockedExercises",
                column: "LearningPathId");

            migrationBuilder.CreateIndex(
                name: "IX_LockedExercises_UserId",
                table: "LockedExercises",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Solutions_LockedExercises_LockedExerciseId",
                table: "Solutions",
                column: "LockedExerciseId",
                principalTable: "LockedExercises",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Solutions_LockedExercises_LockedExerciseId",
                table: "Solutions");

            migrationBuilder.DropTable(
                name: "LockedExercises");

            migrationBuilder.DropTable(
                name: "LearningPaths");

            migrationBuilder.DropIndex(
                name: "IX_Solutions_LockedExerciseId",
                table: "Solutions");

            migrationBuilder.DropColumn(
                name: "LockedExerciseId",
                table: "Solutions");
        }
    }
}
