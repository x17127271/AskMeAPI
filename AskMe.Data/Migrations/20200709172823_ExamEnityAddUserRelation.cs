using Microsoft.EntityFrameworkCore.Migrations;

namespace AskMe.Data.Migrations
{
    public partial class ExamEnityAddUserRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Exams");

            migrationBuilder.DropColumn(
                name: "TotalFailed",
                table: "Exams");

            migrationBuilder.DropColumn(
                name: "TotalSuccess",
                table: "Exams");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Exams",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Exams_UserId",
                table: "Exams",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Exams_Users_UserId",
                table: "Exams",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exams_Users_UserId",
                table: "Exams");

            migrationBuilder.DropIndex(
                name: "IX_Exams_UserId",
                table: "Exams");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Exams");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Exams",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalFailed",
                table: "Exams",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalSuccess",
                table: "Exams",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
