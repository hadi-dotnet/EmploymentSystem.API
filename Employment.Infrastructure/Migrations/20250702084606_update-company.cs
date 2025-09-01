using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Job.Infrastructure.Migrations
{
    public partial class updatecompany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Employees_EmployeeID",
                table: "Posts");

            migrationBuilder.RenameColumn(
                name: "EmployeeID",
                table: "Posts",
                newName: "CompanyID");

            migrationBuilder.RenameIndex(
                name: "IX_Posts_EmployeeID",
                table: "Posts",
                newName: "IX_Posts_CompanyID");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Companys_CompanyID",
                table: "Posts",
                column: "CompanyID",
                principalTable: "Companys",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Companys_CompanyID",
                table: "Posts");

            migrationBuilder.RenameColumn(
                name: "CompanyID",
                table: "Posts",
                newName: "EmployeeID");

            migrationBuilder.RenameIndex(
                name: "IX_Posts_CompanyID",
                table: "Posts",
                newName: "IX_Posts_EmployeeID");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Employees_EmployeeID",
                table: "Posts",
                column: "EmployeeID",
                principalTable: "Employees",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
