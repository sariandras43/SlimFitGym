using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SlimFitGym.EFData.Migrations
{
    /// <inheritdoc />
    public partial class New_table_entries : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Entries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AccountId = table.Column<int>(type: "INTEGER", nullable: false),
                    EntryDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entries", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Passes",
                keyColumn: "Id",
                keyValue: 1,
                column: "MaxEntries",
                value: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Entries");

            migrationBuilder.UpdateData(
                table: "Passes",
                keyColumn: "Id",
                keyValue: 1,
                column: "MaxEntries",
                value: 10);
        }
    }
}
