using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Job.Infrastructure.Migrations
{
    public partial class addExperience : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Experience",
                columns: table => new
                {
                    ExperienceID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeID = table.Column<string>(type: "NVARCHAR(450)", maxLength: 450, nullable: false),
                    Title = table.Column<string>(type: "NVARCHAR(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "NVARCHAR", nullable: false),
                    StartSAT = table.Column<DateTime>(type: "smalldatetime", nullable: false),
                    FInishAT = table.Column<DateTime>(type: "smalldatetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Experience", x => x.ExperienceID);
                    table.ForeignKey(
                        name: "FK_Experience_Employees_EmployeeID",
                        column: x => x.EmployeeID,
                        principalTable: "Employees",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApplyExperience",
                columns: table => new
                {
                    ApplyExperienceID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExperienceID = table.Column<int>(type: "int", nullable: false),
                    CompanyID = table.Column<string>(type: "NVARCHAR(450)", maxLength: 450, nullable: true),
                    IsExperienceApply = table.Column<bool>(type: "BIT", nullable: false),
                    CompanyName = table.Column<string>(type: "NVARCHAR(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplyExperience", x => x.ApplyExperienceID);
                    table.ForeignKey(
                        name: "FK_ApplyExperience_Companys_CompanyID",
                        column: x => x.CompanyID,
                        principalTable: "Companys",
                        principalColumn: "UserID");
                    table.ForeignKey(
                        name: "FK_ApplyExperience_Experience_ExperienceID",
                        column: x => x.ExperienceID,
                        principalTable: "Experience",
                        principalColumn: "ExperienceID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplyExperience_CompanyID",
                table: "ApplyExperience",
                column: "CompanyID");

            migrationBuilder.CreateIndex(
                name: "IX_ApplyExperience_ExperienceID",
                table: "ApplyExperience",
                column: "ExperienceID");

            migrationBuilder.CreateIndex(
                name: "IX_Experience_EmployeeID",
                table: "Experience",
                column: "EmployeeID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplyExperience");

            migrationBuilder.DropTable(
                name: "Experience");
        }
    }
}
