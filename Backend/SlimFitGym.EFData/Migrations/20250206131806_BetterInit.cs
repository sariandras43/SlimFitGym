using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SlimFitGym.EFData.Migrations
{
    /// <inheritdoc />
    public partial class BetterInit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MachineCount",
                table: "RoomsAndMachines",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "RoomsAndMachines",
                keyColumn: "Id",
                keyValue: 1,
                column: "MachineCount",
                value: 1);

            migrationBuilder.UpdateData(
                table: "RoomsAndMachines",
                keyColumn: "Id",
                keyValue: 2,
                column: "MachineCount",
                value: 4);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MachineCount",
                table: "RoomsAndMachines");
        }
    }
}
