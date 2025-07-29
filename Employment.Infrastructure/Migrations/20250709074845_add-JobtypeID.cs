using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Job.Infrastructure.Migrations
{
    public partial class addJobtypeID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Skills",
                type: "NVARCHAR(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<int>(
                name: "JobTypeID",
                table: "Skills",
                type: "INT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Skills_JobTypeID",
                table: "Skills",
                column: "JobTypeID");

            migrationBuilder.AddForeignKey(
                name: "FK_Skills_JobsType_JobTypeID",
                table: "Skills",
                column: "JobTypeID",
                principalTable: "JobsType",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Skills_JobsType_JobTypeID",
                table: "Skills");

            migrationBuilder.DropIndex(
                name: "IX_Skills_JobTypeID",
                table: "Skills");

            migrationBuilder.DropColumn(
                name: "JobTypeID",
                table: "Skills");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Skills",
                type: "NVARCHAR(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "NVARCHAR(50)",
                oldMaxLength: 50,
                oldNullable: true);
        }
    }
}
