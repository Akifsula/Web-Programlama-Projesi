using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KuaforYonetim.Migrations
{
    public partial class AddDurumToRandevu : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Onaylandi",
                table: "Randevular");

            migrationBuilder.AddColumn<int>(
                name: "Durum",
                table: "Randevular",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Durum",
                table: "Randevular");

            migrationBuilder.AddColumn<bool>(
                name: "Onaylandi",
                table: "Randevular",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
