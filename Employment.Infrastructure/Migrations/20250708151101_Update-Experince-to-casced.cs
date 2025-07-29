using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Job.Infrastructure.Migrations
{
    public partial class UpdateExperincetocasced : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Experience_Employees_EmployeeID",
                table: "Experience");

            migrationBuilder.AddForeignKey(
                name: "FK_Experience_Employees_EmployeeID",
                table: "Experience",
                column: "EmployeeID",
                principalTable: "Employees",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Experience_Employees_EmployeeID",
                table: "Experience");

            migrationBuilder.AddForeignKey(
                name: "FK_Experience_Employees_EmployeeID",
                table: "Experience",
                column: "EmployeeID",
                principalTable: "Employees",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
