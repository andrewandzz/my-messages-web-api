using Microsoft.EntityFrameworkCore.Migrations;

namespace MyMessages.Data.Migrations
{
    public partial class AddStickersTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Stickers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    Path = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stickers", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Stickers",
                columns: new[] { "Id", "Name", "Path" },
                values: new object[,]
                {
                    { 1, "Apricot", "stickers\\apricot.png" },
                    { 2, "Calendula", "stickers\\calendula.png" },
                    { 3, "Chamomile", "stickers\\chamomile.png" },
                    { 4, "Cloud", "stickers\\cloud.png" },
                    { 5, "Flowers", "stickers\\flowers.png" },
                    { 6, "Heart", "stickers\\heart.png" },
                    { 7, "Hills", "stickers\\hills.png" },
                    { 8, "Mallow", "stickers\\mallow.png" },
                    { 9, "Mushroom", "stickers\\mushroom.png" },
                    { 10, "Rainbow", "stickers\\rainbow.png" },
                    { 11, "Sun", "stickers\\sun.png" },
                    { 12, "Tree", "stickers\\tree.png" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Stickers");
        }
    }
}
