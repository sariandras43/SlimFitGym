using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SlimFitGym.EFData.Migrations
{
    /// <inheritdoc />
    public partial class New_indexes_and_table_Trainer_Applicants : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TrainerApplicants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AccountId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainerApplicants", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_Name",
                table: "Rooms",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Machines_Name",
                table: "Machines",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Benefits_BenefitName",
                table: "Benefits",
                column: "BenefitName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_Email",
                table: "Accounts",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrainerApplicants");

            migrationBuilder.DropIndex(
                name: "IX_Rooms_Name",
                table: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_Machines_Name",
                table: "Machines");

            migrationBuilder.DropIndex(
                name: "IX_Benefits_BenefitName",
                table: "Benefits");

            migrationBuilder.DropIndex(
                name: "IX_Accounts_Email",
                table: "Accounts");
        }
    }
}
