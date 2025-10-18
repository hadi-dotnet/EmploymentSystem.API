using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Job.Infrastructure.Migrations
{
    public partial class updateskills : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Skills_Employees_EmployeeID",
                table: "Skills");

            migrationBuilder.DropForeignKey(
                name: "FK_Skills_SkillsType_SkillsTypeid",
                table: "Skills");

            migrationBuilder.DropIndex(
                name: "IX_Skills_SkillsTypeid",
                table: "Skills");

            migrationBuilder.DropColumn(
                name: "SkillsTypeid",
                table: "Skills");

            migrationBuilder.CreateIndex(
                name: "IX_Skills_SkillTypeId",
                table: "Skills",
                column: "SkillTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Skills_Employees_EmployeeID",
                table: "Skills",
                column: "EmployeeID",
                principalTable: "Employees",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Skills_SkillsType_SkillTypeId",
                table: "Skills",
                column: "SkillTypeId",
                principalTable: "SkillsType",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Skills_Employees_EmployeeID",
                table: "Skills");

            migrationBuilder.DropForeignKey(
                name: "FK_Skills_SkillsType_SkillTypeId",
                table: "Skills");

            migrationBuilder.DropIndex(
                name: "IX_Skills_SkillTypeId",
                table: "Skills");

            migrationBuilder.AddColumn<int>(
                name: "SkillsTypeid",
                table: "Skills",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Skills_SkillsTypeid",
                table: "Skills",
                column: "SkillsTypeid");

            migrationBuilder.AddForeignKey(
                name: "FK_Skills_Employees_EmployeeID",
                table: "Skills",
                column: "EmployeeID",
                principalTable: "Employees",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Skills_SkillsType_SkillsTypeid",
                table: "Skills",
                column: "SkillsTypeid",
                principalTable: "SkillsType",
                principalColumn: "id");
        }
    }
}
