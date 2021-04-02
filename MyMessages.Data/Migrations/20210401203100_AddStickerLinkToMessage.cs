using Microsoft.EntityFrameworkCore.Migrations;

namespace MyMessages.Data.Migrations
{
    public partial class AddStickerLinkToMessage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StickerId",
                table: "Messages",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Messages_StickerId",
                table: "Messages",
                column: "StickerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Stickers_StickerId",
                table: "Messages",
                column: "StickerId",
                principalTable: "Stickers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Stickers_StickerId",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_StickerId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "StickerId",
                table: "Messages");
        }
    }
}
