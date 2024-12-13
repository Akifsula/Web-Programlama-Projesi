using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KuaforYonetim.Migrations
{
    public partial class AddCalisanSeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Calisanlar",
                columns: new[] { "CalisanId", "AdSoyad", "UzmanlikAlanlari" },
                values: new object[] { 1, "Ahmet Yılmaz", "Saç Kesimi, Sakal Traşı" });

            migrationBuilder.InsertData(
                table: "Calisanlar",
                columns: new[] { "CalisanId", "AdSoyad", "UzmanlikAlanlari" },
                values: new object[] { 2, "Mehmet Kaya", "Boyama, Saç Şekillendirme" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Calisanlar",
                keyColumn: "CalisanId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Calisanlar",
                keyColumn: "CalisanId",
                keyValue: 2);
        }
    }
}
