using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Job.Infrastructure.Migrations
{
    public partial class updateskillsl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Skills_SkillsType_SkillTypeId",
                table: "Skills");

            migrationBuilder.RenameColumn(
                name: "SkillTypeId",
                table: "Skills",
                newName: "SkillTypeID");

            migrationBuilder.RenameIndex(
                name: "IX_Skills_SkillTypeId",
                table: "Skills",
                newName: "IX_Skills_SkillTypeID");

            migrationBuilder.AddForeignKey(
                name: "FK_Skills_SkillsType_SkillTypeID",
                table: "Skills",
                column: "SkillTypeID",
                principalTable: "SkillsType",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Skills_SkillsType_SkillTypeID",
                table: "Skills");

            migrationBuilder.RenameColumn(
                name: "SkillTypeID",
                table: "Skills",
                newName: "SkillTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Skills_SkillTypeID",
                table: "Skills",
                newName: "IX_Skills_SkillTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Skills_SkillsType_SkillTypeId",
                table: "Skills",
                column: "SkillTypeId",
                principalTable: "SkillsType",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
