using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Job.Infrastructure.Migrations
{
    public partial class updatemessagestomany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Messages_SenderBy",
                table: "Messages");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_SenderBy",
                table: "Messages",
                column: "SenderBy");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Messages_SenderBy",
                table: "Messages");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_SenderBy",
                table: "Messages",
                column: "SenderBy",
                unique: true);
        }
    }
}
