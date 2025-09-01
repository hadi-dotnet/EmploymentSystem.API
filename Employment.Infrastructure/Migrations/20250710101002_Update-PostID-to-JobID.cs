using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Job.Infrastructure.Migrations
{
    public partial class UpdatePostIDtoJobID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplyJob_Jobs_PostID",
                table: "ApplyJob");

            migrationBuilder.RenameColumn(
                name: "PostID",
                table: "ApplyJob",
                newName: "JobID");

            migrationBuilder.RenameIndex(
                name: "IX_ApplyJob_PostID",
                table: "ApplyJob",
                newName: "IX_ApplyJob_JobID");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplyJob_Jobs_JobID",
                table: "ApplyJob",
                column: "JobID",
                principalTable: "Jobs",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplyJob_Jobs_JobID",
                table: "ApplyJob");

            migrationBuilder.RenameColumn(
                name: "JobID",
                table: "ApplyJob",
                newName: "PostID");

            migrationBuilder.RenameIndex(
                name: "IX_ApplyJob_JobID",
                table: "ApplyJob",
                newName: "IX_ApplyJob_PostID");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplyJob_Jobs_PostID",
                table: "ApplyJob",
                column: "PostID",
                principalTable: "Jobs",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
