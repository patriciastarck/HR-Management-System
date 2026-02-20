using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HR_Management_System.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTrainingParticipationRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_employee_participation",
                table: "trainingparticipation");

            migrationBuilder.DropForeignKey(
                name: "fk_training_participation",
                table: "trainingparticipation");

            migrationBuilder.AddForeignKey(
                name: "fk_employee_participation",
                table: "trainingparticipation",
                column: "employee_id",
                principalTable: "employee",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_training_participation",
                table: "trainingparticipation",
                column: "training_id",
                principalTable: "training",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_employee_participation",
                table: "trainingparticipation");

            migrationBuilder.DropForeignKey(
                name: "fk_training_participation",
                table: "trainingparticipation");

            migrationBuilder.AddForeignKey(
                name: "fk_employee_participation",
                table: "trainingparticipation",
                column: "employee_id",
                principalTable: "employee",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_training_participation",
                table: "trainingparticipation",
                column: "training_id",
                principalTable: "training",
                principalColumn: "id");
        }
    }
}
