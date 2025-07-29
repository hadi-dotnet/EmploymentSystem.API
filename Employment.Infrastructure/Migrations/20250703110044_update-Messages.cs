using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Job.Infrastructure.Migrations
{
    public partial class updateMessages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Conversations_Companys_CompanyID",
                table: "Conversations");

            migrationBuilder.DropForeignKey(
                name: "FK_Conversations_Employees_EmployeeID",
                table: "Conversations");

            migrationBuilder.RenameColumn(
                name: "EmployeeID",
                table: "Conversations",
                newName: "UserID2");

            migrationBuilder.RenameColumn(
                name: "CompanyID",
                table: "Conversations",
                newName: "UserID1");

            migrationBuilder.RenameIndex(
                name: "IX_Conversations_EmployeeID",
                table: "Conversations",
                newName: "IX_Conversations_UserID2");

            migrationBuilder.RenameIndex(
                name: "IX_Conversations_CompanyID",
                table: "Conversations",
                newName: "IX_Conversations_UserID1");

            migrationBuilder.AddForeignKey(
                name: "FK_Conversations_AspNetUsers_UserID1",
                table: "Conversations",
                column: "UserID1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Conversations_AspNetUsers_UserID2",
                table: "Conversations",
                column: "UserID2",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Conversations_AspNetUsers_UserID1",
                table: "Conversations");

            migrationBuilder.DropForeignKey(
                name: "FK_Conversations_AspNetUsers_UserID2",
                table: "Conversations");

            migrationBuilder.RenameColumn(
                name: "UserID2",
                table: "Conversations",
                newName: "EmployeeID");

            migrationBuilder.RenameColumn(
                name: "UserID1",
                table: "Conversations",
                newName: "CompanyID");

            migrationBuilder.RenameIndex(
                name: "IX_Conversations_UserID2",
                table: "Conversations",
                newName: "IX_Conversations_EmployeeID");

            migrationBuilder.RenameIndex(
                name: "IX_Conversations_UserID1",
                table: "Conversations",
                newName: "IX_Conversations_CompanyID");

            migrationBuilder.AddForeignKey(
                name: "FK_Conversations_Companys_CompanyID",
                table: "Conversations",
                column: "CompanyID",
                principalTable: "Companys",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Conversations_Employees_EmployeeID",
                table: "Conversations",
                column: "EmployeeID",
                principalTable: "Employees",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
