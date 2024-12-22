using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KuaforYonetim.Migrations
{
    public partial class FixCalisanHizmetRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CalisanHizmetler");

            migrationBuilder.AddColumn<int>(
                name: "CalisanId",
                table: "Hizmetler",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UzmanlikAlanlari",
                table: "Calisanlar",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "CalisanUygunluk",
                columns: table => new
                {
                    UygunlukId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CalisanId = table.Column<int>(type: "int", nullable: false),
                    Gun = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BaslangicSaati = table.Column<TimeSpan>(type: "time", nullable: false),
                    BitisSaati = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalisanUygunluk", x => x.UygunlukId);
                    table.ForeignKey(
                        name: "FK_CalisanUygunluk_Calisanlar_CalisanId",
                        column: x => x.CalisanId,
                        principalTable: "Calisanlar",
                        principalColumn: "CalisanId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Calisanlar",
                columns: new[] { "CalisanId", "AdSoyad", "UzmanlikAlanlari" },
                values: new object[,]
                {
                    { 1, "Ahmet Yılmaz", "Saç Kesimi, Sakal Traşı" },
                    { 2, "Mehmet Kaya", "Boyama, Saç Şekillendirme" }
                });

            migrationBuilder.InsertData(
                table: "Hizmetler",
                columns: new[] { "HizmetId", "Ad", "CalisanId", "TahminiSure", "Ucret" },
                values: new object[,]
                {
                    { 1, "Saç Kesimi", null, new TimeSpan(0, 0, 30, 0, 0), 250m },
                    { 2, "Sakal Traşı", null, new TimeSpan(0, 0, 10, 0, 0), 100m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Hizmetler_CalisanId",
                table: "Hizmetler",
                column: "CalisanId");

            migrationBuilder.CreateIndex(
                name: "IX_CalisanUygunluk_CalisanId",
                table: "CalisanUygunluk",
                column: "CalisanId");

            migrationBuilder.AddForeignKey(
                name: "FK_Hizmetler_Calisanlar_CalisanId",
                table: "Hizmetler",
                column: "CalisanId",
                principalTable: "Calisanlar",
                principalColumn: "CalisanId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Hizmetler_Calisanlar_CalisanId",
                table: "Hizmetler");

            migrationBuilder.DropTable(
                name: "CalisanUygunluk");

            migrationBuilder.DropIndex(
                name: "IX_Hizmetler_CalisanId",
                table: "Hizmetler");

            migrationBuilder.DeleteData(
                table: "Calisanlar",
                keyColumn: "CalisanId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Calisanlar",
                keyColumn: "CalisanId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Hizmetler",
                keyColumn: "HizmetId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Hizmetler",
                keyColumn: "HizmetId",
                keyValue: 2);

            migrationBuilder.DropColumn(
                name: "CalisanId",
                table: "Hizmetler");

            migrationBuilder.DropColumn(
                name: "UzmanlikAlanlari",
                table: "Calisanlar");

            migrationBuilder.CreateTable(
                name: "CalisanHizmetler",
                columns: table => new
                {
                    CalisanId = table.Column<int>(type: "int", nullable: false),
                    HizmetId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalisanHizmetler", x => new { x.CalisanId, x.HizmetId });
                    table.ForeignKey(
                        name: "FK_CalisanHizmetler_Calisanlar_CalisanId",
                        column: x => x.CalisanId,
                        principalTable: "Calisanlar",
                        principalColumn: "CalisanId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CalisanHizmetler_Hizmetler_HizmetId",
                        column: x => x.HizmetId,
                        principalTable: "Hizmetler",
                        principalColumn: "HizmetId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CalisanHizmetler_HizmetId",
                table: "CalisanHizmetler",
                column: "HizmetId");
        }
    }
}
