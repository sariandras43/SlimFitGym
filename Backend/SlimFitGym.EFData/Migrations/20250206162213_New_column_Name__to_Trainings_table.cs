using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SlimFitGym.EFData.Migrations
{
    /// <inheritdoc />
    public partial class New_column_Name__to_Trainings_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Training",
                type: "TEXT",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Training",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "TRX edzés");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Training");
        }
    }
}
