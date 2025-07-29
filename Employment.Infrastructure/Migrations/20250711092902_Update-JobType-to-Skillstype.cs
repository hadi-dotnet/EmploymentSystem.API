using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Job.Infrastructure.Migrations
{
    public partial class UpdateJobTypetoSkillstype : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_JobsType_JobTypeID",
                table: "Jobs");

            migrationBuilder.DropForeignKey(
                name: "FK_Skills_JobsType_JobTypeID",
                table: "Skills");

            migrationBuilder.DropTable(
                name: "JobsType");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Skills");

            migrationBuilder.RenameColumn(
                name: "JobTypeID",
                table: "Skills",
                newName: "SkillTypeID");

            migrationBuilder.RenameIndex(
                name: "IX_Skills_JobTypeID",
                table: "Skills",
                newName: "IX_Skills_SkillTypeID");

            migrationBuilder.RenameColumn(
                name: "JobTypeID",
                table: "Jobs",
                newName: "SkillsTypeID");

            migrationBuilder.RenameIndex(
                name: "IX_Jobs_JobTypeID",
                table: "Jobs",
                newName: "IX_Jobs_SkillsTypeID");

            migrationBuilder.CreateTable(
                name: "SkillsType",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeName = table.Column<string>(type: "NVARCHAR(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SkillsType", x => x.id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_SkillsType_SkillsTypeID",
                table: "Jobs",
                column: "SkillsTypeID",
                principalTable: "SkillsType",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Skills_SkillsType_SkillTypeID",
                table: "Skills",
                column: "SkillTypeID",
                principalTable: "SkillsType",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_SkillsType_SkillsTypeID",
                table: "Jobs");

            migrationBuilder.DropForeignKey(
                name: "FK_Skills_SkillsType_SkillTypeID",
                table: "Skills");

            migrationBuilder.DropTable(
                name: "SkillsType");

            migrationBuilder.RenameColumn(
                name: "SkillTypeID",
                table: "Skills",
                newName: "JobTypeID");

            migrationBuilder.RenameIndex(
                name: "IX_Skills_SkillTypeID",
                table: "Skills",
                newName: "IX_Skills_JobTypeID");

            migrationBuilder.RenameColumn(
                name: "SkillsTypeID",
                table: "Jobs",
                newName: "JobTypeID");

            migrationBuilder.RenameIndex(
                name: "IX_Jobs_SkillsTypeID",
                table: "Jobs",
                newName: "IX_Jobs_JobTypeID");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Skills",
                type: "NVARCHAR(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "JobsType",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeName = table.Column<string>(type: "NVARCHAR(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobsType", x => x.id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_JobsType_JobTypeID",
                table: "Jobs",
                column: "JobTypeID",
                principalTable: "JobsType",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Skills_JobsType_JobTypeID",
                table: "Skills",
                column: "JobTypeID",
                principalTable: "JobsType",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
