using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Job.Infrastructure.Migrations
{
    public partial class updatehh : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_PostsType_PostTypeID",
                table: "Jobs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PostsType",
                table: "PostsType");

            migrationBuilder.RenameTable(
                name: "PostsType",
                newName: "JobsType");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JobsType",
                table: "JobsType",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_JobsType_PostTypeID",
                table: "Jobs",
                column: "PostTypeID",
                principalTable: "JobsType",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_JobsType_PostTypeID",
                table: "Jobs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JobsType",
                table: "JobsType");

            migrationBuilder.RenameTable(
                name: "JobsType",
                newName: "PostsType");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PostsType",
                table: "PostsType",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_PostsType_PostTypeID",
                table: "Jobs",
                column: "PostTypeID",
                principalTable: "PostsType",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
