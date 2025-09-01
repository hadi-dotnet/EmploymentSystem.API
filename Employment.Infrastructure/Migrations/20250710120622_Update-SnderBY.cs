using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Job.Infrastructure.Migrations
{
    public partial class UpdateSnderBY : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SenderBy",
                table: "Messages",
                type: "NVARCHAR(450)",
                maxLength: 450,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_SenderBy",
                table: "Messages",
                column: "SenderBy",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_AspNetUsers_SenderBy",
                table: "Messages",
                column: "SenderBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_AspNetUsers_SenderBy",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_SenderBy",
                table: "Messages");

            migrationBuilder.AlterColumn<int>(
                name: "SenderBy",
                table: "Messages",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(450)",
                oldMaxLength: 450);
        }
    }
}
