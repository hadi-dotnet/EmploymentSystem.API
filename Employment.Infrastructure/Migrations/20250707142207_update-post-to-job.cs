using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Job.Infrastructure.Migrations
{
    public partial class updateposttojob : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_JobsType_PostTypeID",
                table: "Jobs");

            migrationBuilder.RenameColumn(
                name: "PostTypeID",
                table: "Jobs",
                newName: "JobTypeID");

            migrationBuilder.RenameIndex(
                name: "IX_Jobs_PostTypeID",
                table: "Jobs",
                newName: "IX_Jobs_JobTypeID");

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_JobsType_JobTypeID",
                table: "Jobs",
                column: "JobTypeID",
                principalTable: "JobsType",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_JobsType_JobTypeID",
                table: "Jobs");

            migrationBuilder.RenameColumn(
                name: "JobTypeID",
                table: "Jobs",
                newName: "PostTypeID");

            migrationBuilder.RenameIndex(
                name: "IX_Jobs_JobTypeID",
                table: "Jobs",
                newName: "IX_Jobs_PostTypeID");

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_JobsType_PostTypeID",
                table: "Jobs",
                column: "PostTypeID",
                principalTable: "JobsType",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
