using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KuaforYonetim.Migrations
{
    public partial class SeedHizmetData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Hizmetler",
                columns: new[] { "HizmetId", "Ad", "CalisanId", "TahminiSure", "Ucret" },
                values: new object[] { 1, "Saç Kesimi", null, new TimeSpan(0, 0, 30, 0, 0), 250m });

            migrationBuilder.InsertData(
                table: "Hizmetler",
                columns: new[] { "HizmetId", "Ad", "CalisanId", "TahminiSure", "Ucret" },
                values: new object[] { 2, "Sakal Traşı", null, new TimeSpan(0, 0, 10, 0, 0), 100m });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Hizmetler",
                keyColumn: "HizmetId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Hizmetler",
                keyColumn: "HizmetId",
                keyValue: 2);
        }
    }
}
