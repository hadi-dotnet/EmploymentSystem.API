using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Job.Infrastructure.Migrations
{
    public partial class UpdateLingth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "Companys",
                type: "NVARCHAR(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "NVARCHAR",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "About",
                table: "Companys",
                type: "NVARCHAR(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "NVARCHAR",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "Companys",
                type: "NVARCHAR",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "About",
                table: "Companys",
                type: "NVARCHAR",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(200)",
                oldMaxLength: 200,
                oldNullable: true);
        }
    }
}
