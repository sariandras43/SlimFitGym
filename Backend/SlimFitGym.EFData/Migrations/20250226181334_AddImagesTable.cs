using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SlimFitGym.EFData.Migrations
{
    /// <inheritdoc />
    public partial class AddImagesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AccountId = table.Column<int>(type: "INTEGER", nullable: false),
                    MachineId = table.Column<int>(type: "INTEGER", nullable: false),
                    RoomId = table.Column<int>(type: "INTEGER", nullable: false),
                    Url = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$10$Xnc08woQRvena/LbEL1HRO8Y.Aan3EmQuaqp/L2HgNpKhY0wzyd..");

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 2,
                column: "Password",
                value: "$2a$10$Yzf9Wxgdo5mBfa9CLXfmNeUmeAj20U8F/B5ms.XCvnSfrcf18YC1W");

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 3,
                column: "Password",
                value: "$2a$10$CfdtKA/E6j6yMmS1cEpVT.rqcR6Z5y1L6JjIHpc/kbmUHnCCt8U0S");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$10$S.s/NxUERfUvz/YwsYixbOcAwiRpk4lGov09DeXmn3II.DEVMarJS");

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 2,
                column: "Password",
                value: "$2a$10$0aFYm1TtkhkDJgdqL75wxOzE3q4NuTctsXyIe5tYOhxImHtsQOXUq");

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 3,
                column: "Password",
                value: "$2a$10$aAUelKxI0DXfligNehpDQ.qQ/Oy6S7U6OV.9cWaGTMbndLMpFvkce");
        }
    }
}
