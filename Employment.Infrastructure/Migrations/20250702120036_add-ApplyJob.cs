using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Job.Infrastructure.Migrations
{
    public partial class addApplyJob : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApplyJob",
                columns: table => new
                {
                    ApplyJobID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeID = table.Column<string>(type: "NVARCHAR(450)", maxLength: 450, nullable: false),
                    PostID = table.Column<int>(type: "INT", nullable: false),
                    IsApproved = table.Column<bool>(type: "BIT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplyJob", x => x.ApplyJobID);
                    table.ForeignKey(
                        name: "FK_ApplyJob_Employees_EmployeeID",
                        column: x => x.EmployeeID,
                        principalTable: "Employees",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ApplyJob_Posts_PostID",
                        column: x => x.PostID,
                        principalTable: "Posts",
                        principalColumn: "PostID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplyJob_EmployeeID",
                table: "ApplyJob",
                column: "EmployeeID");

            migrationBuilder.CreateIndex(
                name: "IX_ApplyJob_PostID",
                table: "ApplyJob",
                column: "PostID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplyJob");
        }
    }
}
