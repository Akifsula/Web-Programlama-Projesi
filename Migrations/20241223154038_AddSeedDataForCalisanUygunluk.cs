using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KuaforYonetim.Migrations
{
    public partial class AddSeedDataForCalisanUygunluk : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CalisanUygunluk_Calisanlar_CalisanId",
                table: "CalisanUygunluk");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CalisanUygunluk",
                table: "CalisanUygunluk");

            migrationBuilder.DropIndex(
                name: "IX_CalisanUygunluk_CalisanId",
                table: "CalisanUygunluk");

            migrationBuilder.RenameTable(
                name: "CalisanUygunluk",
                newName: "CalisanUygunluklar");

            migrationBuilder.AlterColumn<int>(
                name: "Gun",
                table: "CalisanUygunluklar",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CalisanUygunluklar",
                table: "CalisanUygunluklar",
                column: "UygunlukId");

            migrationBuilder.InsertData(
                table: "CalisanUygunluklar",
                columns: new[] { "UygunlukId", "BaslangicSaati", "BitisSaati", "CalisanId", "Gun" },
                values: new object[] { 1, new TimeSpan(0, 9, 0, 0, 0), new TimeSpan(0, 17, 0, 0, 0), 1, 1 });

            migrationBuilder.InsertData(
                table: "CalisanUygunluklar",
                columns: new[] { "UygunlukId", "BaslangicSaati", "BitisSaati", "CalisanId", "Gun" },
                values: new object[] { 2, new TimeSpan(0, 10, 0, 0, 0), new TimeSpan(0, 18, 0, 0, 0), 2, 2 });

            migrationBuilder.CreateIndex(
                name: "IX_CalisanUygunluklar_CalisanId_Gun_BaslangicSaati_BitisSaati",
                table: "CalisanUygunluklar",
                columns: new[] { "CalisanId", "Gun", "BaslangicSaati", "BitisSaati" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CalisanUygunluklar_Calisanlar_CalisanId",
                table: "CalisanUygunluklar",
                column: "CalisanId",
                principalTable: "Calisanlar",
                principalColumn: "CalisanId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CalisanUygunluklar_Calisanlar_CalisanId",
                table: "CalisanUygunluklar");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CalisanUygunluklar",
                table: "CalisanUygunluklar");

            migrationBuilder.DropIndex(
                name: "IX_CalisanUygunluklar_CalisanId_Gun_BaslangicSaati_BitisSaati",
                table: "CalisanUygunluklar");

            migrationBuilder.DeleteData(
                table: "CalisanUygunluklar",
                keyColumn: "UygunlukId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "CalisanUygunluklar",
                keyColumn: "UygunlukId",
                keyValue: 2);

            migrationBuilder.RenameTable(
                name: "CalisanUygunluklar",
                newName: "CalisanUygunluk");

            migrationBuilder.AlterColumn<string>(
                name: "Gun",
                table: "CalisanUygunluk",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CalisanUygunluk",
                table: "CalisanUygunluk",
                column: "UygunlukId");

            migrationBuilder.CreateIndex(
                name: "IX_CalisanUygunluk_CalisanId",
                table: "CalisanUygunluk",
                column: "CalisanId");

            migrationBuilder.AddForeignKey(
                name: "FK_CalisanUygunluk_Calisanlar_CalisanId",
                table: "CalisanUygunluk",
                column: "CalisanId",
                principalTable: "Calisanlar",
                principalColumn: "CalisanId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
