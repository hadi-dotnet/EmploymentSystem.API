using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Job.Infrastructure.Migrations
{
    public partial class addnewtable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_SkillsType_SkillsTypeID",
                table: "Jobs");

            migrationBuilder.DropIndex(
                name: "IX_Jobs_SkillsTypeID",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "SkillsTypeID",
                table: "Jobs");

            migrationBuilder.AlterColumn<int>(
                name: "RemoteOROnSite",
                table: "Jobs",
                type: "INT",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "BIT");

            migrationBuilder.AlterColumn<int>(
                name: "FullTimeORPartTime",
                table: "Jobs",
                type: "INT",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "BIT");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Jobs",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "smalldatetime");

            migrationBuilder.CreateTable(
                name: "JobSkillType",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobID = table.Column<int>(type: "INT", nullable: false),
                    SkillTypeID = table.Column<int>(type: "INT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobSkillType", x => x.id);
                    table.ForeignKey(
                        name: "FK_JobSkillType_Jobs_JobID",
                        column: x => x.JobID,
                        principalTable: "Jobs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JobSkillType_SkillsType_SkillTypeID",
                        column: x => x.SkillTypeID,
                        principalTable: "SkillsType",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "SkillsType",
                columns: new[] { "id", "TypeName" },
                values: new object[] { 122, "Backend Develper" });

            migrationBuilder.InsertData(
                table: "SkillsType",
                columns: new[] { "id", "TypeName" },
                values: new object[] { 123, "Frontend Developer" });

            migrationBuilder.CreateIndex(
                name: "IX_JobSkillType_JobID",
                table: "JobSkillType",
                column: "JobID");

            migrationBuilder.CreateIndex(
                name: "IX_JobSkillType_SkillTypeID",
                table: "JobSkillType",
                column: "SkillTypeID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JobSkillType");

            migrationBuilder.DeleteData(
                table: "SkillsType",
                keyColumn: "id",
                keyValue: 122);

            migrationBuilder.DeleteData(
                table: "SkillsType",
                keyColumn: "id",
                keyValue: 123);

            migrationBuilder.AlterColumn<bool>(
                name: "RemoteOROnSite",
                table: "Jobs",
                type: "BIT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INT");

            migrationBuilder.AlterColumn<bool>(
                name: "FullTimeORPartTime",
                table: "Jobs",
                type: "BIT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INT");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Jobs",
                type: "smalldatetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AddColumn<int>(
                name: "SkillsTypeID",
                table: "Jobs",
                type: "INT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_SkillsTypeID",
                table: "Jobs",
                column: "SkillsTypeID");

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_SkillsType_SkillsTypeID",
                table: "Jobs",
                column: "SkillsTypeID",
                principalTable: "SkillsType",
                principalColumn: "id");
        }
    }
}
