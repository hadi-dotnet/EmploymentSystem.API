using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Job.Infrastructure.Migrations
{
    public partial class updatenames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplyExperience_Companys_CompanyID",
                table: "ApplyExperience");

            migrationBuilder.DropForeignKey(
                name: "FK_Experience_Employees_EmployeeID",
                table: "Experience");

            migrationBuilder.DropIndex(
                name: "IX_ApplyExperience_CompanyID",
                table: "ApplyExperience");

            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "ApplyJob");

            migrationBuilder.DropColumn(
                name: "CompanyID",
                table: "ApplyExperience");

            migrationBuilder.DropColumn(
                name: "CompanyName",
                table: "ApplyExperience");

            migrationBuilder.DropColumn(
                name: "IsExperienceApply",
                table: "ApplyExperience");

            migrationBuilder.RenameColumn(
                name: "SkillName",
                table: "Skills",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "SkillId",
                table: "Skills",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "PostTitle",
                table: "Posts",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "PostID",
                table: "Posts",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "MessagesID",
                table: "Messages",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "FInishAT",
                table: "Experience",
                newName: "FinishAT");

            migrationBuilder.RenameColumn(
                name: "StartSAT",
                table: "Experience",
                newName: "StartAT");

            migrationBuilder.RenameColumn(
                name: "ExperienceID",
                table: "Experience",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "CompanyName",
                table: "Companys",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "CompanyImage",
                table: "Companys",
                newName: "Image");

            migrationBuilder.RenameColumn(
                name: "CompanyAddress",
                table: "Companys",
                newName: "Address");

            migrationBuilder.RenameColumn(
                name: "AboutCompany",
                table: "Companys",
                newName: "About");

            migrationBuilder.RenameColumn(
                name: "ApplySkillID",
                table: "ApplySkill",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "ApplyJobID",
                table: "ApplyJob",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "ApplyExperienceID",
                table: "ApplyExperience",
                newName: "ID");

            migrationBuilder.AddColumn<string>(
                name: "CompanyID",
                table: "Experience",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CompanyName",
                table: "Experience",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Experience_CompanyID",
                table: "Experience",
                column: "CompanyID");

            migrationBuilder.AddForeignKey(
                name: "FK_Experience_Companys_CompanyID",
                table: "Experience",
                column: "CompanyID",
                principalTable: "Companys",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Experience_Employees_EmployeeID",
                table: "Experience",
                column: "EmployeeID",
                principalTable: "Employees",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Experience_Companys_CompanyID",
                table: "Experience");

            migrationBuilder.DropForeignKey(
                name: "FK_Experience_Employees_EmployeeID",
                table: "Experience");

            migrationBuilder.DropIndex(
                name: "IX_Experience_CompanyID",
                table: "Experience");

            migrationBuilder.DropColumn(
                name: "CompanyID",
                table: "Experience");

            migrationBuilder.DropColumn(
                name: "CompanyName",
                table: "Experience");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Skills",
                newName: "SkillName");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Skills",
                newName: "SkillId");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Posts",
                newName: "PostTitle");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Posts",
                newName: "PostID");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Messages",
                newName: "MessagesID");

            migrationBuilder.RenameColumn(
                name: "FinishAT",
                table: "Experience",
                newName: "FInishAT");

            migrationBuilder.RenameColumn(
                name: "StartAT",
                table: "Experience",
                newName: "StartSAT");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Experience",
                newName: "ExperienceID");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Companys",
                newName: "CompanyName");

            migrationBuilder.RenameColumn(
                name: "Image",
                table: "Companys",
                newName: "CompanyImage");

            migrationBuilder.RenameColumn(
                name: "Address",
                table: "Companys",
                newName: "CompanyAddress");

            migrationBuilder.RenameColumn(
                name: "About",
                table: "Companys",
                newName: "AboutCompany");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "ApplySkill",
                newName: "ApplySkillID");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "ApplyJob",
                newName: "ApplyJobID");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "ApplyExperience",
                newName: "ApplyExperienceID");

            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "ApplyJob",
                type: "BIT",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "CompanyID",
                table: "ApplyExperience",
                type: "NVARCHAR(450)",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyName",
                table: "ApplyExperience",
                type: "NVARCHAR(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsExperienceApply",
                table: "ApplyExperience",
                type: "BIT",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_ApplyExperience_CompanyID",
                table: "ApplyExperience",
                column: "CompanyID");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplyExperience_Companys_CompanyID",
                table: "ApplyExperience",
                column: "CompanyID",
                principalTable: "Companys",
                principalColumn: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Experience_Employees_EmployeeID",
                table: "Experience",
                column: "EmployeeID",
                principalTable: "Employees",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
